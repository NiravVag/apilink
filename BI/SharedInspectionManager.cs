using AutoMapper;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Dashboard;
using DTO.DefectDashboard;
using DTO.Common;
using DTO.Inspection;
using DTO.RejectionDashboard;
using DTO.InspectionCustomerDecision;
using DTO.ManagementDashboard;
using DTO.RepoRequest.Enum;
using DTO.Schedule;
using DTO.SharedInspection;
using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static DTO.Common.Static_Data_Common;
using System.Threading.Tasks;
using DTO.QuantitativeDashboard;
using DTO.FinanceDashboard;
using System.Data;
using DTO.Report;
using DTO.CommonClass;

namespace BI
{


    public class SharedInspectionManager : ApiCommonData, ISharedInspectionManager
    {
        private readonly ISharedInspectionRepo _repo = null;
        private readonly IMapper _mapper;

        public SharedInspectionManager(ISharedInspectionRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IQueryable<InspTransaction> GetAllInspectionQuery()
        {
            return _repo.GetAllInspectionsQuery();
        }

        public IQueryable<FbReportDetail> GetAllFbReportDetails()
        {
            return _repo.GetAllFbReportDetailsQuery();
        }

        public SharedInspectionModel GetInspectionQueryRequestMap(InspectionSummarySearchRequest request)
        {
            return new SharedInspectionModel()
            {
                SearchTypeId = request.SearchTypeId,
                SearchTypeText = request.SearchTypeText,
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                FactoryIdlst = request.FactoryIdlst,
                StatusIdlst = request.StatusIdlst,
                DateTypeid = request.DateTypeid,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Officeidlst = request.Officeidlst,
                QuotationsStatusIdlst = request.QuotationsStatusIdlst,
                CustomerList = request.CustomerList,
                IsQuotationSearch = request.IsQuotationSearch,
                ServiceTypelst = request.ServiceTypelst,
                AdvancedSearchtypeid = request.AdvancedSearchtypeid,
                AdvancedSearchtypetext = request.AdvancedSearchtypetext,
                UserIdList = request.UserIdList,
                SelectedCountryIdList = request.SelectedCountryIdList,
                SelectedProvinceIdList = request.SelectedProvinceIdList,
                SelectedCityIdList = request.SelectedCityIdList,
                SelectedBrandIdList = request.SelectedBrandIdList,
                SelectedDeptIdList = request.SelectedDeptIdList,
                SelectedCollectionIdList = request.SelectedCollectionIdList,
                SelectedBuyerIdList = request.SelectedBuyerIdList,
                SelectedPriceCategoryIdList = request.SelectedPriceCategoryIdList,
                IsEcoPack = request.IsEcoPack,
                IsPicking = request.IsPicking,
                IsEAQF = request.IsEAQF,
                quotationId = request.quotationId,
                BookingType = request.BookingType
            };
        }

        public SharedInspectionModel GetManagementDashboardInspectionRequestMap(ManagementDashboardRequest request)
        {
            return new SharedInspectionModel()
            {
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                FromDate = request.ServiceDateFrom,
                ToDate = request.ServiceDateTo,
                FactoryIdlst = request.FactoryIdList,
                StatusIdlst = request.StatusIdList,
                SelectedCountryIdList = request.CountryIdList,
                Officeidlst = request.OfficeIdList
            };
        }

        public SharedInspectionModel GetScheduleInspectionQueryRequestMap(ScheduleSearchRequest request)
        {
            return new SharedInspectionModel()
            {
                SearchTypeId = request.SearchTypeId,
                SearchTypeText = request.SearchTypeText,
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                FactoryIdlst = request.FactoryIdlst,
                StatusIdlst = request.StatusIdlst,
                DateTypeid = request.DateTypeid,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Officeidlst = request.Officeidlst,
                CountryId = request.CountryId,
                CityId = request.CityId,
                ProvinceId = request.ProvinceId,
                QuotationsStatusIdlst = request.QuotationsStatusIdlst,
                QcIdlst = request.QcIdlst,
                ZoneIdlst = request.ZoneIdlst,
                ServiceTypelst = request.ServiceTypelist,
                IsEAQF = request.IsEAQF.GetValueOrDefault()
            };
        }

        public IQueryable<InspTransaction> GetInspectionQuerywithRequestFilters(SharedInspectionModel request, IQueryable<InspTransaction> inspectionQuery)
        {
            //search by inspection booking no/po no/customer booking no
            if (!string.IsNullOrWhiteSpace(request.SearchTypeText?.Trim()) && (Enum.TryParse(request.SearchTypeId.ToString(), out SearchType _numberSearchTypeEnum)))
            {
                switch (_numberSearchTypeEnum)
                {
                    case SearchType.PoNo:
                        {
                            inspectionQuery = inspectionQuery.Where(x => x.InspPurchaseOrderTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.Po.Pono == request.SearchTypeText.Trim()));
                            break;
                        }
                    case SearchType.BookingNo:
                        {
                            if (int.TryParse(request.SearchTypeText?.Trim(), out int bookid))
                                inspectionQuery = inspectionQuery.Where(x => x.Id == bookid);
                            break;
                        }
                    case SearchType.CustomerBookingNo:
                        {
                            inspectionQuery = inspectionQuery.Where(x => x.CustomerBookingNo.Trim() == request.SearchTypeText.Trim());
                            break;
                        }
                    case SearchType.ReportNo:
                        {
                            inspectionQuery = inspectionQuery.Where(x => x.FbReportDetails.Any(y => y.Active.Value && y.ReportTitle.Trim() == request.SearchTypeText.Trim()));
                            break;
                        }
                    case SearchType.ProductId:
                        {
                            inspectionQuery = inspectionQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.Product.ProductId.Trim() == request.SearchTypeText.Trim()));
                            break;
                        }
                }
            }

            //filter by creation date or service date or firstservicedate
            if (Enum.TryParse(request.DateTypeid.ToString(), out SearchType _datesearchtype) && (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null))
            {
                switch (_datesearchtype)
                {
                    case SearchType.ApplyDate:
                        {
                            inspectionQuery = inspectionQuery.Where(x => EF.Functions.DateDiffDay(request.FromDate.ToDateTime(), x.CreatedOn) >= 0 &&
                                                 EF.Functions.DateDiffDay(x.CreatedOn, request.ToDate.ToDateTime()) >= 0);
                            break;
                        }
                    case SearchType.ServiceDate:
                        {
                            if (request.IsDashboardRequest)
                            {
                                inspectionQuery = inspectionQuery.Where(x => x.ServiceDateTo <= request.ToDate.ToDateTime() && x.ServiceDateTo >= request.FromDate.ToDateTime());
                            }
                            else
                            {
                                inspectionQuery = inspectionQuery.Where(x => !((x.ServiceDateFrom > request.ToDate.ToDateTime()) || (x.ServiceDateTo < request.FromDate.ToDateTime())));
                            }
                            break;
                        }
                    case SearchType.FirstServiceDate:
                        {
                            inspectionQuery = inspectionQuery.Where(x => !((x.FirstServiceDateFrom > request.ToDate.ToDateTime()) || (x.FirstServiceDateTo < request.FromDate.ToDateTime())));
                            break;
                        }
                }
            }

            if (request.CustomerId > 0)
            {
                inspectionQuery = inspectionQuery.Where(x => x.CustomerId == request.CustomerId);
            }

            //apply supplier filter
            if (request.SupplierId > 0)
            {
                inspectionQuery = inspectionQuery.Where(x => x.SupplierId == request.SupplierId);
            }

            //apply status list filter
            if (request.StatusIdlst != null && request.StatusIdlst.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => request.StatusIdlst.ToList().Contains(x.StatusId));
            }

            //apply office list filter
            if (request.Officeidlst != null && request.Officeidlst.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.OfficeId != null && request.Officeidlst.ToList().Contains(x.OfficeId.Value));
            }

            //advance search type (ponmber,product name)
            if ((!string.IsNullOrEmpty(request.AdvancedSearchtypetext?.Trim())) && (Enum.TryParse(request.AdvancedSearchtypeid.ToString(), out AdvanceSearchType _advanceseachtypeenum)))
            {
                switch (_advanceseachtypeenum)
                {
                    case AdvanceSearchType.FactoryReference:
                        {
                            if (request.CustomerId > 0)
                            {
                                inspectionQuery = inspectionQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Product.CustomerId == request.CustomerId.Value && y.Active.Value && y.Product.FactoryReference.Trim() == request.AdvancedSearchtypetext.Trim()));
                            }
                            else
                            {
                                inspectionQuery = inspectionQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.Product.FactoryReference.Trim() == request.AdvancedSearchtypetext.Trim()));
                            }
                            break;
                        }
                    case AdvanceSearchType.ProductName:
                        {
                            if (request.CustomerId > 0)
                            {
                                inspectionQuery = inspectionQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Product.CustomerId == request.CustomerId.Value && y.Active.Value && y.Product.ProductId.Trim() == request.AdvancedSearchtypetext.Trim()));
                            }
                            else
                            {
                                inspectionQuery = inspectionQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.Product.ProductId.Trim() == request.AdvancedSearchtypetext.Trim()));

                            }
                            break;
                        }

                    case AdvanceSearchType.BarCode:
                        {
                            if (request.CustomerId > 0)
                            {
                                inspectionQuery = inspectionQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Product.CustomerId == request.CustomerId.Value && y.Active.Value && y.Product.Barcode.Trim() == request.AdvancedSearchtypetext.Trim()));
                            }
                            else
                            {
                                inspectionQuery = inspectionQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.Product.Barcode.Trim() == request.AdvancedSearchtypetext.Trim()));

                            }
                            break;
                        }
                }

            }

            //apply factory filter
            if (request.FactoryIdlst != null && request.FactoryIdlst.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.FactoryId > 0 && request.FactoryIdlst.ToList().Contains(x.FactoryId.GetValueOrDefault()));
            }

            //apply factory country filter
            if (request.SelectedCountryIdList != null && request.SelectedCountryIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.Factory.SuAddresses.Any(y => request.SelectedCountryIdList.Contains(y.CountryId)));
            }

            //apply factory province filter
            if (request.SelectedProvinceIdList != null && request.SelectedProvinceIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.Factory.SuAddresses.Any(y => request.SelectedProvinceIdList.Contains(y.RegionId)));
            }

            //apply factory city filter
            if (request.SelectedCityIdList != null && request.SelectedCityIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.Factory.SuAddresses.Any(y => request.SelectedCityIdList.Contains(y.CityId)));
            }

            //Filter based on Service type
            if (request.ServiceTypelst != null && request.ServiceTypelst.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.InspTranServiceTypes.Any(y => y.Active && request.ServiceTypelst.Contains(y.ServiceTypeId)));
            }

            //filter by price category
            if (request.SelectedPriceCategoryIdList != null && request.SelectedPriceCategoryIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => request.SelectedPriceCategoryIdList.Contains(x.PriceCategoryId));
            }

            //filter by collection
            if (request.SelectedCollectionIdList != null && request.SelectedCollectionIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => request.SelectedCollectionIdList.Contains(x.CollectionId));
            }


            //filter by selected product category list
            if (request.SelectedProdCategoryIdList != null && request.SelectedProdCategoryIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.ProductCategoryId.HasValue && request.SelectedProdCategoryIdList.Contains(x.ProductCategoryId.Value));
            }

            //filter by selected product list
            if (request.SelectedProductIdList != null && request.SelectedProductIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.Value && request.SelectedProductIdList.Contains(y.ProductId)));
            }

            //apply bookingType filter
            if (request.BookingType == (int)InspectionBookingTypeEnum.Announced)
            {
                inspectionQuery = inspectionQuery.Where(x => x.BookingType == request.BookingType || x.BookingType == null);
            }
            else if (request.BookingType == (int)InspectionBookingTypeEnum.UnAnnounced)
            {
                inspectionQuery = inspectionQuery.Where(x => x.BookingType == request.BookingType);
            }

            //filter by picking
            if (request.IsPicking)
            {
                inspectionQuery = inspectionQuery.Where(x => x.IsPickingRequired == request.IsPicking);
            }

            //filter by eaqf
            if (request.IsEAQF)
            {
                inspectionQuery = inspectionQuery.Where(x => x.IsEaqf == request.IsEAQF);
            }

            //filter by isEcoPack
            if (request.IsEcoPack)
            {
                inspectionQuery = inspectionQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.IsEcopack == request.IsEcoPack));
            }


            if (request.SelectedBuyerIdList != null && request.SelectedBuyerIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.InspTranCuBuyers.Any(y => y.Active && request.SelectedBuyerIdList.Contains(y.BuyerId)));
            }

            if (request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.InspTranCuBrands.Any(y => y.Active && request.SelectedBrandIdList.Contains(y.BrandId)));
            }

            if (request.SelectedDeptIdList != null && request.SelectedDeptIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.InspTranCuDepartments.Any(y => y.Active && request.SelectedDeptIdList.Contains(y.DepartmentId)));
            }

            //apply factory country filter
            if (request?.CountryId > 0)
            {
                inspectionQuery = inspectionQuery.Where(x => x.Factory.SuAddresses.Any(y => y.CountryId == request.CountryId));
            }

            if (request?.ProvinceId > 0)
            {
                inspectionQuery = inspectionQuery.Where(x => x.Factory.SuAddresses.Any(y => y.RegionId == request.ProvinceId));
            }

            if (request?.CityId > 0)
            {
                inspectionQuery = inspectionQuery.Where(x => x.Factory.SuAddresses.Any(y => y.CityId == request.CityId));
            }

            //apply factory zone filter
            if (request.ZoneIdlst != null && request.ZoneIdlst.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.Factory.SuAddresses.Any(y => request.ZoneIdlst.Contains(y.County.ZoneId.GetValueOrDefault())));
            }

            if (request.QcIdlst != null && request.QcIdlst.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.SchScheduleQcs.Any(y => y.Active && request.QcIdlst.Contains(y.Qcid)));
            }
            if (request.CustomerList != null && request.CustomerList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => request.CustomerList.Contains(x.CustomerId));
            }

            if (request.SelectedSupplierIdList != null && request.SelectedSupplierIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => request.SelectedSupplierIdList.Contains(x.SupplierId));
            }
            if (request.SelectedFactoryIdList != null && request.SelectedFactoryIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => request.SelectedFactoryIdList.Contains(x.FactoryId.GetValueOrDefault()));
            }

            //apply servicetype filter
            if (request.ServiceTypelst != null && request.ServiceTypelst.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.InspTranServiceTypes.Any(y => y.Active && request.ServiceTypelst.Contains(y.ServiceTypeId)));
            }
            return inspectionQuery;
        }

        public IQueryable<FbReportDetail> GetFbReportDetailswithRequestFilters(SharedInspectionModel request, IQueryable<FbReportDetail> fbReportDetailsQuery)
        {

            //search filter ---newly added
            if (request.SearchFilters != null && request.SearchFilters.Any())
            {
                foreach (var searchFilter in request.SearchFilters)
                {
                    if (!string.IsNullOrWhiteSpace(searchFilter.SearchTypeText))
                    {
                        searchFilter.SearchTypeText = searchFilter.SearchTypeText.Trim();
                        switch (searchFilter.SearchType)
                        {
                            case (int)SearchType.PoNo:
                                {
                                    fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.InspPurchaseOrderTransactions.Any(y => y.Po.Pono == searchFilter.SearchTypeText)));
                                    break;
                                }
                            case (int)SearchType.BookingNo:
                                {
                                    if (int.TryParse(searchFilter.SearchTypeText?.Trim(), out int bookid))
                                        fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.InspectionId == bookid);
                                    break;
                                }
                            case (int)SearchType.CustomerBookingNo:
                                {
                                    fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.CustomerBookingNo.Trim() == searchFilter.SearchTypeText);
                                    break;
                                }
                            case (int)SearchType.ReportNo:
                                {
                                    fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.ReportTitle.Trim() == searchFilter.SearchTypeText);
                                    break;
                                }
                            case (int)SearchType.ProductId:
                                {
                                    fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.Product.ProductId.Trim() == searchFilter.SearchTypeText));
                                    break;
                                }
                        }
                    }
                }
            }

            //filter by creation date or service date or firstservicedate
            if (Enum.TryParse(request.DateTypeid.ToString(), out SearchType _datesearchtype) && (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null))
            {
                switch (_datesearchtype)
                {
                    case SearchType.ApplyDate:
                        {
                            fbReportDetailsQuery = fbReportDetailsQuery.Where(x => EF.Functions.DateDiffDay(request.FromDate.ToDateTime(), x.CreatedOn) >= 0 &&
                                                  EF.Functions.DateDiffDay(x.CreatedOn, request.ToDate.ToDateTime()) >= 0);
                            break;
                        }
                    case SearchType.ServiceDate:
                        {
                            fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.ServiceDateTo <= request.ToDate.ToDateTime() && x.Inspection.ServiceDateTo >= request.FromDate.ToDateTime());
                            break;
                        }
                    case SearchType.FirstServiceDate:
                        {
                            fbReportDetailsQuery = fbReportDetailsQuery.Where(x => !((x.Inspection.FirstServiceDateFrom > request.ToDate.ToDateTime()) || (x.Inspection.FirstServiceDateTo < request.FromDate.ToDateTime())));
                            break;
                        }
                }
            }

            if (request.CustomerId > 0)
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.CustomerId == request.CustomerId);
            }

            //apply supplier filter
            if (request.SupplierId > 0)
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.SupplierId == request.SupplierId);
            }

            //apply status list filter
            if (request.StatusIdlst != null && request.StatusIdlst.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => request.StatusIdlst.ToList().Contains(x.Inspection.StatusId));
            }

            //apply office list filter
            if (request.Officeidlst != null && request.Officeidlst.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.OfficeId != null && request.Officeidlst.ToList().Contains(x.Inspection.OfficeId.Value));
            }

            //advance search type (ponmber,product name)
            if ((!string.IsNullOrEmpty(request.AdvancedSearchtypetext?.Trim())) && (Enum.TryParse(request.AdvancedSearchtypeid.ToString(), out AdvanceSearchType _advanceseachtypeenum)))
            {
                switch (_advanceseachtypeenum)
                {
                    case AdvanceSearchType.FactoryReference:
                        {
                            if (request.CustomerId > 0)
                            {
                                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Product.CustomerId == request.CustomerId.Value && y.Active.Value && y.Product.FactoryReference.Trim() == request.AdvancedSearchtypetext.Trim()));
                            }
                            else
                            {
                                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.Product.FactoryReference.Trim() == request.AdvancedSearchtypetext.Trim()));
                            }
                            break;
                        }
                    case AdvanceSearchType.ProductName:
                        {
                            if (request.CustomerId > 0)
                            {
                                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Product.CustomerId == request.CustomerId.Value && y.Active.Value && y.Product.ProductId.Trim() == request.AdvancedSearchtypetext.Trim()));
                            }
                            else
                            {
                                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.Product.ProductId.Trim() == request.AdvancedSearchtypetext.Trim()));

                            }
                            break;
                        }

                    case AdvanceSearchType.BarCode:
                        {
                            if (request.CustomerId > 0)
                            {
                                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Product.CustomerId == request.CustomerId.Value && y.Active.Value && y.Product.Barcode.Trim() == request.AdvancedSearchtypetext.Trim()));
                            }
                            else
                            {
                                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.Product.Barcode.Trim() == request.AdvancedSearchtypetext.Trim()));

                            }
                            break;
                        }
                }

            }

            //apply factory filter
            if (request.FactoryIdlst != null && request.FactoryIdlst.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => request.FactoryIdlst.ToList().Contains(x.Inspection.FactoryId.GetValueOrDefault()));
            }

            //apply factory country filter
            if (request.SelectedCountryIdList != null && request.SelectedCountryIdList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.Factory.SuAddresses.Any(y => request.SelectedCountryIdList.Contains(y.CountryId)));
            }

            //apply factory province filter
            if (request.SelectedProvinceIdList != null && request.SelectedProvinceIdList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.Factory.SuAddresses.Any(y => request.SelectedProvinceIdList.Contains(y.RegionId)));
            }

            //apply factory city filter
            if (request.SelectedCityIdList != null && request.SelectedCityIdList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.Factory.SuAddresses.Any(y => request.SelectedCityIdList.Contains(y.CityId)));
            }

            //Filter based on Service type
            if (request.ServiceTypelst != null && request.ServiceTypelst.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.InspTranServiceTypes.Any(y => y.Active && request.ServiceTypelst.Contains(y.ServiceTypeId)));
            }

            //filter by price category
            if (request.SelectedPriceCategoryIdList != null && request.SelectedPriceCategoryIdList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => request.SelectedPriceCategoryIdList.Contains(x.Inspection.PriceCategoryId));
            }

            //filter by collection
            if (request.SelectedCollectionIdList != null && request.SelectedCollectionIdList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => request.SelectedCollectionIdList.Contains(x.Inspection.CollectionId));
            }


            //filter by selected product category list
            if (request.SelectedProdCategoryIdList != null && request.SelectedProdCategoryIdList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.ProductCategoryId.HasValue && request.SelectedProdCategoryIdList.Contains(x.Inspection.ProductCategoryId.Value));
            }

            //filter by selected product list
            if (request.SelectedProductIdList != null && request.SelectedProductIdList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.InspProductTransactions.Any(y => y.Active.Value && request.SelectedProductIdList.Contains(y.ProductId)));
            }

            //filter by picking
            if (request.IsPicking)
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.IsPickingRequired == request.IsPicking);
            }

            //filter by isEcoPack
            if (request.IsEcoPack)
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.IsEcopack == request.IsEcoPack));
            }


            if (request.SelectedBuyerIdList != null && request.SelectedBuyerIdList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.InspTranCuBuyers.Any(y => y.Active && request.SelectedBuyerIdList.Contains(y.BuyerId)));
            }

            if (request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.InspTranCuBrands.Any(y => y.Active && request.SelectedBrandIdList.Contains(y.BrandId)));
            }

            if (request.SelectedDeptIdList != null && request.SelectedDeptIdList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.InspTranCuDepartments.Any(y => y.Active && request.SelectedDeptIdList.Contains(y.DepartmentId)));
            }

            //apply factory country filter
            if (request?.CountryId > 0)
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.Factory.SuAddresses.Any(y => y.CountryId == request.CountryId));
            }

            if (request?.ProvinceId > 0)
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.Factory.SuAddresses.Any(y => y.RegionId == request.ProvinceId));
            }

            if (request?.CityId > 0)
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.Factory.SuAddresses.Any(y => y.CityId == request.CityId));
            }

            //apply factory zone filter
            if (request.ZoneIdlst != null && request.ZoneIdlst.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.Factory.SuAddresses.Any(y => request.ZoneIdlst.Contains(y.County.ZoneId.GetValueOrDefault())));
            }

            if (request.QcIdlst != null && request.QcIdlst.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => x.Inspection.SchScheduleQcs.Any(y => y.Active && request.QcIdlst.Contains(y.Qcid)));
            }
            if (request.CustomerList != null && request.CustomerList.Any())
            {
                fbReportDetailsQuery = fbReportDetailsQuery.Where(x => request.CustomerList.Contains(x.Inspection.CustomerId));
            }

            return fbReportDetailsQuery;
        }


        /// <summary>
        /// EP Plus get stream object from the collection
        /// </summary>
        /// <param name="result"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public Stream GetAsStreamObject<T>(IEnumerable<T> result, string sheetName = "sheet1")
        {

            if (result == null || !result.Any())
                return null;

            var stream = new MemoryStream();

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add(sheetName);
                workSheet.Cells.LoadFromCollection(result, true);


                //get the first indexer
                int datecolumn = 1;

                //loop through the object and get the list of datecolumns
                foreach (var PropertyInfo in result.FirstOrDefault().GetType().GetProperties())
                {
                    //check if property is of DateTime type or nullable DateTime type
                    if (PropertyInfo.PropertyType == typeof(DateTime) || PropertyInfo.PropertyType == typeof(DateTime?))
                    {
                        workSheet.Column(datecolumn).Style.Numberformat.Format = StandardDateFormat;
                    }
                    datecolumn++;
                }

                package.Save();
            }
            stream.Position = 0;
            return stream;

        }

        /// <summary>
        /// Get the stream data from the datatable
        /// </summary>
        /// <param name="result"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public Stream GetAsStreamObjectAndLoadDataTable(dynamic result, string sheetName = "sheet1")
        {
            var stream = new MemoryStream();
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add(sheetName);
                workSheet.Cells.LoadFromDataTable(result, true);
                workSheet.DefaultColWidth = EpplusDefaultColumnWidth;

                //set date time format in excel 
                int colNumber = 1;
                foreach (DataColumn col in result.Columns)
                {
                    if (col.DataType == typeof(DateTime))
                    {
                        workSheet.Column(colNumber).Style.Numberformat.Format = StandardDateFormat;
                    }
                    colNumber++;
                }
                package.Save();
            }
            stream.Position = 0;
            return stream;
        }

        public SharedInspectionModel GetCustomerDecisionQueryRequestMap(CustomerDecisionSummaryRequest request)
        {
            return new SharedInspectionModel()
            {
                SearchTypeId = request.SearchTypeId,
                SearchTypeText = request.SearchTypeText,
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                FactoryIdlst = request.FactoryIdlst,
                StatusIdlst = request.StatusIdlst,
                DateTypeid = request.DateTypeid,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Officeidlst = request.Officeidlst,
                CustomerList = request.CustomerList,
                ServiceTypelst = request.ServiceTypelst,
                AdvancedSearchtypeid = request.AdvancedSearchtypeid.ToString(),
                AdvancedSearchtypetext = request.AdvancedSearchtypetext,
                SelectedCountryIdList = request.SelectedCountryIdList,
                SelectedBrandIdList = request.SelectedBrandIdList,
                SelectedDeptIdList = request.SelectedDeptIdList,
                SelectedCollectionIdList = request.SelectedCollectionIdList,
                SelectedBuyerIdList = request.SelectedBuyerIdList
            };
        }

        public IQueryable<InspTransaction> GetInspectionsQuery(int bookingId)
        {
            return _repo.GetInspectionsQuery(bookingId);
        }

        public async Task<List<InspectionStatus>> GetInspectionStatusList(IQueryable<InspTransaction> bookingData)
        {
            return await bookingData.Select(x => new { x.StatusId, x.Status.Status, x.Id, x.Status.Priority })
                   .GroupBy(p => new { p.StatusId, p.Status, p.Priority }, p => p, (key, _data) =>
                 new InspectionStatus
                 {
                     Id = key.StatusId,
                     StatusName = key.Status,
                     TotalCount = _data.Count(),
                     Priority = key.Priority
                 }).OrderBy(x => x.Priority).ToListAsync();
        }
        public SharedInspectionModel GetDashBoardInspectionQueryRequestMap(DefectDashboardFilterRequest request)
        {
            return new SharedInspectionModel()
            {
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,

                FromDate = request.FromDate,
                ToDate = request.ToDate,
                SelectedCountryIdList = request.FactoryCountryIds,
                SelectedBrandIdList = request.SelectedBrandIdList,
                SelectedBuyerIdList = request.SelectedBuyerIdList,
                SelectedCollectionIdList = request.SelectedCollectionIdList,
                SelectedDeptIdList = request.SelectedDeptIdList,
                StatusIdlst = InspectedStatusList,
                FactoryIdlst = request.FactoryIds,
                SelectedProdCategoryIdList = request.SelectedProdCategoryIdList,
                SelectedProductIdList = request.SelectedProductIdList,
                ServiceTypelst = request.ServiceTypelst,
                SearchTypeId = request.SearchTypeId,
                SearchTypeText = request.SearchTypeText
            };
        }

        public SharedInspectionModel GetRejectionDashBoardInspectionSearchRequestMap(RejectionDashboardSearchRequest request)
        {
            return new SharedInspectionModel()
            {
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                FromDate = request.ServiceDateFrom,
                ToDate = request.ServiceDateTo,
                SelectedCountryIdList = request.SelectedCountryIdList,
                SelectedBrandIdList = request.SelectedBrandIdList,
                SelectedBuyerIdList = request.SelectedBuyerIdList,
                SelectedCollectionIdList = request.SelectedCollectionIdList,
                SelectedDeptIdList = request.SelectedDeptIdList,
                StatusIdlst = InspectedStatusList,
                FactoryIdlst = request.FactoryId > 0 ? new[] { request.FactoryId.GetValueOrDefault() }.ToList() : null,
                SelectedProdCategoryIdList = request.SelectedProdCategoryIdList,
                SelectedProductIdList = request.SelectedProductIdList,
                SelectedSupplierIdList = request.SelectedSupplierIdList,
                SelectedFactoryIdList = request.SelectedFactoryIdList,
                ServiceTypelst = request.SelectedServiceTypeIdList,
                SearchTypeId = request.SearchTypeId,
                SearchTypeText = request.SearchTypeText
            };
        }
        public SharedInspectionModel GetQuantitativeDashBoardInspectionQueryRequestMap(QuantitativeDashboardFilterRequest request)
        {
            return new SharedInspectionModel()
            {
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                FromDate = request.ServiceDateFrom,
                ToDate = request.ServiceDateTo,
                SelectedCountryIdList = request.SelectedCountryIdList,
                SelectedBrandIdList = request.SelectedBrandIdList,
                SelectedBuyerIdList = request.SelectedBuyerIdList,
                SelectedCollectionIdList = request.SelectedCollectionIdList,
                SelectedDeptIdList = request.SelectedDeptIdList,
                StatusIdlst = InspectedStatusList,
                SelectedProdCategoryIdList = request.SelectedProdCategoryIdList,
                SelectedProductIdList = request.SelectedProductIdList
                //DateTypeid = (int)SearchType.ServiceDate
            };

        }
        public SharedInspectionModel GetFinanceDashBoardInspectionQueryRequestMap(FinanceDashboardSearchRequest request)
        {
            return new SharedInspectionModel()
            {
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                FromDate = request.ServiceDateFrom,
                ToDate = request.ServiceDateTo,
                SelectedCountryIdList = request.CountryIdList,
                SelectedBrandIdList = request.BrandIdList,
                SelectedBuyerIdList = request.BuyerIdList,
                CustomerList = request.RatioCustomerIdList,
                SelectedDeptIdList = request.DeptIdList,
                StatusIdlst = InspectedStatusList,
                DateTypeid = (int)SearchType.ServiceDate,
                IsDashboardRequest = true
            };

        }

        public SharedInspectionModel GetCustomerReportInspectionQueryRequestMap(int customerId, DateTime fromDate, DateTime toDate, CustomerReportDetailsRequest request)
        {
            var SearchFilters = new List<SearchFilter>();
            if (!string.IsNullOrEmpty(request.Po))
            {
                SearchFilters.Add(new SearchFilter()
                {
                    SearchType = (int)SearchType.PoNo,
                    SearchTypeText = request.Po,
                });
            }
            if (!string.IsNullOrEmpty(request.ProductRef))
            {
                SearchFilters.Add(new SearchFilter()
                {
                    SearchType = (int)SearchType.ProductId,
                    SearchTypeText = request.ProductRef,
                });
            }
            if (request.InspectionNo > 0)
            {

                SearchFilters.Add(new SearchFilter()
                {
                    SearchType = (int)SearchType.BookingNo,
                    SearchTypeText = request.InspectionNo.ToString(),
                });
            }
            if (!string.IsNullOrEmpty(request.ReportNo))
            {
                SearchFilters.Add(new SearchFilter()
                {
                    SearchType = (int)SearchType.ReportNo,
                    SearchTypeText = request.ReportNo
                });
            }
            return new SharedInspectionModel()
            {
                CustomerId = customerId,
                FromDate = new DateObject(fromDate.Year, fromDate.Month, fromDate.Day),
                ToDate = new DateObject(toDate.Year, toDate.Month, toDate.Day),
                DateTypeid = (int)SearchType.ServiceDate,
                StatusIdlst = InspectedStatusList,
                SearchFilters = SearchFilters
            };
        }

        public SharedInspectionModel GetDashboardMapInspectionRequestMap(DashboardMapFilterRequest request)
        {
            return new SharedInspectionModel()
            {
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                FromDate = request.ServiceDateFrom,
                DateTypeid = (int)SearchType.ServiceDate,
                ToDate = request.ServiceDateTo,
                FactoryIdlst = request.FactoryIds,
                StatusIdlst = request.StatusIds,
                SelectedCountryIdList = request.CountryIds,
                Officeidlst = request.OfficeIds,
                SelectedProductIdList = request.ProductIds,
                SelectedProdCategoryIdList = request.ProductCategoryIds,
                SelectedCollectionIdList = request.CollectionIds,
                SelectedBuyerIdList = request.BuyerIds,
                SelectedBrandIdList = request.BrandIds,
                IsDashboardRequest = true
            };
        }
    }
}
