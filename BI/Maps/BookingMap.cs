using DTO.BookingRuleContact;
using DTO.CancelBooking;
using DTO.Common;
using DTO.CommonClass;
using DTO.DynamicFields;
using DTO.Inspection;
using DTO.Quotation;
using DTO.References;
using DTO.Report;
using DTO.User;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace BI.Maps
{
    public class BookingMap : ApiCommonData
    {
        public InspectionStatus GetInspectionStatus(InspStatus entity)
        {
            if (entity == null)
                return null;
            return new InspectionStatus
            {
                Id = entity.Id,
                StatusName = entity.Status

            };
        }

        public InspectionStatus GetBookingStatuswithColor(InspectionStatus entity)
        {
            if (entity == null)
                return null;
            return new InspectionStatus
            {
                Id = entity.Id,
                StatusName = entity.StatusName,
                TotalCount = entity.TotalCount,
                StatusColor = BookingSummaryInspectionStatusColor.GetValueOrDefault(entity.Id, "")
            };
        }

        public InspectionStatus GetBookingStatusMap(InspectionStatus entity)
        {
            if (entity == null)
                return null;
            return new InspectionStatus
            {
                Id = entity.Id,
                StatusName = entity.StatusName,
                TotalCount = entity.TotalCount,
                StatusColor = InspectionStatusColor.GetValueOrDefault(entity.Id, "")
            };
        }

        public ProductCategorySub2 GetProductCategorySub2s(RefProductCategorySub2 entity)
        {
            if (entity == null)
                return null;
            return new ProductCategorySub2
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }

        public ProductSubCategory GetProductSubCategory(RefProductCategorySub entity)
        {
            if (entity == null)
                return null;
            return new ProductSubCategory
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }

        public BookingItem GetInspectionSearchResult(InspectionBookingItems entity, IEnumerable<InspectionStatus> inspectionStatus, IEnumerable<CSConfigDetail>
                        customerCSLocations, IEnumerable<int> userRole, int userType, IEnumerable<PoDetails> poDetails, IEnumerable<ServiceTypeList> serviceTypeList,
           IEnumerable<PoDetails> quotationDetails, IEnumerable<FactoryCountry> factoryCountryData, List<BookingReportSummaryLinkRepo> bookingReportSummaryLink,
        List<BookingBrandAccess> BookingBrandList, List<BookingDeptAccess> BookingDeptList, List<BookingBuyerAccess> BookingBuyerList,
        List<BookingContainer> bookingContainers, string entityName, List<SupplierCode> supplierCodes)

        {
            string ponumber = "";
            string productRefId = "";

            string customerProductReference = "";
            int reportcount = 0;
            bool isSplitBookingVisible = false;
            bool isPickingVisible = false;
            bool isPicking = false;
            if (entity == null)
                return null;

            var bookingId = entity.BookingId;

            //var activePOTransactions = entity.InspPoTransactions.Where(x => x.Active.HasValue && x.Active.Value);

            isPicking = entity.IsPicking;

            if (poDetails != null && poDetails.Any())
            {
                var ponumberList = poDetails.Where(x => x.BookingId == bookingId).Select(x => x.PoNumber).Distinct().ToList();
                if (ponumberList.Count > 2)
                {
                    var remainingPonumber = ponumberList.Count - 2;
                    ponumber = string.Join(",", ponumberList.Take(2).ToList());
                    ponumber = ponumber + ", " + "+" + remainingPonumber;
                }
                else
                {
                    ponumber = string.Join(",", ponumberList);
                }
                var productRefIds = poDetails.Where(x => x.BookingId == bookingId).Select(x => x.ProductId).Distinct().ToList();
                if (productRefIds.Count > 2)
                {
                    var remainingProductRefId = productRefIds.Count - 2;
                    productRefId = string.Join(",", productRefIds.Take(2).ToList()) + remainingProductRefId;
                    productRefId = productRefId + ", " + "+" + remainingProductRefId;
                }
                else
                {
                    productRefId = string.Join(",", productRefIds);
                }
                reportcount = poDetails.Count(x => x.BookingId == bookingId && x.ReportId.HasValue);
            }

            //get office list from cs list by customer id
            var officeList = customerCSLocations?.Where(x => x.CustomerId == entity?.CustomerId && x.OfficeId > 0).Select(x => x.OfficeId).Distinct().ToList();

            //get cs name based on customer id, booking location, brand, dept
            var CSName = customerCSLocations?.Where(x => x.CustomerId == entity?.CustomerId &&
                            (x.OfficeId == null || officeList.Contains(entity?.OfficeId)) &&
                            (x.BrandId == null || BookingBrandList.Where(y => y.BookingId == bookingId).Select(y => y.BrandId).ToList().Contains(x.BrandId.GetValueOrDefault())) &&
                            (x.DepartmentId == null || BookingDeptList.Where(y => y.BookingId == bookingId).Select(y => y.DeptId).ToList().Contains(x.DepartmentId.GetValueOrDefault()))
                            ).Select(x => x.CsName).Distinct().ToList();

            //split booking button visible
            if (userType == (int)UserTypeEnum.InternalUser && entity.StatusId != (int)BookingStatus.Cancel &&
                 (userRole.Contains((int)RoleEnum.InspectionRequest) || userRole.Contains((int)RoleEnum.InspectionVerified)) &&
                (entity.StatusId == (int)BookingStatus.Received || entity.StatusId == (int)BookingStatus.Verified ||
                entity.StatusId == (int)BookingStatus.Confirmed))
            {
                isSplitBookingVisible = true;
            }
            //picking button visible
            if (userType == (int)UserTypeEnum.InternalUser && isPicking && entity.StatusId != (int)BookingStatus.Cancel &&
                (userRole.Contains((int)RoleEnum.InspectionRequest) || userRole.Contains((int)RoleEnum.InspectionVerified)))
            //(entity.StatusId == (int)BookingStatus.Received || entity.StatusId == (int)BookingStatus.Verified ||
            //entity.StatusId == (int)BookingStatus.Confirmed))
            {
                isPickingVisible = true;
            }

            string supplierCode = supplierCodes?.Where(x => x.CustomerId == entity.CustomerId && x.SupplierId == entity.SupplierId).Select(x => x.Code).FirstOrDefault();

            string supplierNameAndCode = !string.IsNullOrEmpty(supplierCode) ?
                                        "(" + supplierCode + ") - " + entity?.SupplierName : entity?.SupplierName;

            var bookingItem = new BookingItem()
            {
                BookingId = bookingId,
                CustomerId = entity?.CustomerId,
                CustomerName = entity?.CustomerName,
                FactoryName = entity?.FactoryName,
                PoNumber = ponumber,
                ServiceDateFrom = entity.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = entity.ServiceDateTo.ToString(StandardDateFormat),
                ServiceDateFromEaqf = entity.ServiceDateFrom.ToString(StandardISO8601DateFormat),
                ServiceDateToEaqf = entity.ServiceDateTo.ToString(StandardISO8601DateFormat),
                FirstServiceDateFrom = entity.FirstServiceDateFrom != null ? entity.FirstServiceDateFrom?.ToString(StandardDateFormat) : "",
                FirstServiceDateTo = entity.FirstServiceDateTo != null ? entity.FirstServiceDateTo?.ToString(StandardDateFormat) : "",
                SupplierName = supplierNameAndCode,
                ServiceType = serviceTypeList.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeName).FirstOrDefault(),
                ServiceTypeId = serviceTypeList.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeId).FirstOrDefault(),
                Office = entity?.Office,
                OfficeId = entity?.OfficeId,
                StatusId = entity.StatusId,
                StatusName = inspectionStatus?.Where(x => x.Id == entity.StatusId)?.Select(x => x.StatusName)?.FirstOrDefault(),
                BookingCreatedBy = entity?.BookingCreatedBy,
                IsPicking = isPicking,
                IsEAQF = entity.IsEAQF,
                PreviousBookingNo = entity.PreviousBookingNo,
                FactoryId = entity.FactoryId,
                QuotationStatusId = quotationDetails.Where(x => x.BookingId == bookingId).Select(x => x.QuotationStatus).Distinct().Any() ? quotationDetails.Where(x => x.BookingId == bookingId && x.QuotationStatus != (int)QuotationStatus.Canceled).Select(x => x.QuotationStatus).FirstOrDefault() : quotationDetails.Where(x => x.BookingId == entity.BookingId).Select(x => x.QuotationStatus).FirstOrDefault(),
                ProductCategory = entity?.ProductCategory,
                SupplierId = entity.SupplierId,
                CSName = string.Join(",", CSName),
                IsSplitBookingButtonVisible = isSplitBookingVisible,
                IsPickingButtonVisible = isPickingVisible,
                IsCombineVisible = entity.IsCombineRequired,
                ApplyDate = entity.ApplyDate.ToString(StandardDateFormat),
                QuotationStatusName = quotationDetails.Where(x => x.BookingId == bookingId).Select(x => x.QuotationStatus).Distinct().Any() ? quotationDetails.Where(x => x.BookingId == bookingId && x.QuotationStatus != (int)QuotationStatus.Canceled).Select(x => x.QuotationStatusName).FirstOrDefault() : quotationDetails.Where(x => x.BookingId == bookingId).Select(x => x.QuotationStatusName).FirstOrDefault(),
                CountryId = factoryCountryData.Where(x => x.BookingId == bookingId).Select(x => x.FactoryCountryId).FirstOrDefault(),
                FactoryCountryName = factoryCountryData.Where(x => x.BookingId == bookingId).Select(x => x.CountryName).FirstOrDefault(),
                ReportSummaryLink = "", //bookingReportSummaryLink.Where(x => x.BookingId == entity.BookingId
                                        // && !string.IsNullOrEmpty(x.ReportSummaryLink)).Select(x => x.ReportSummaryLink).FirstOrDefault(),
                CustomerBookingNo = entity.CustomerBookingNo,
                ReportCount = reportcount,
                DeptNames = string.Join(", ", BookingDeptList.Where(x => x.BookingId == bookingId).Select(x => x.DeptName)),
                PriceCategoryName = entity.PriceCategoryName,
                CollectionName = entity.CollectionName,
                BrandNames = string.Join(", ", BookingBrandList.Where(x => x.BookingId == bookingId).Select(x => x.BrandName)),
                BuyerNames = string.Join(", ", BookingBuyerList.Where(x => x.BookingId == bookingId).Select(x => x.BuyerName)),
                BookingCreatedByType = entity.UserTypeId,
                ProductCount = serviceTypeList.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeId).FirstOrDefault() == (int)InspectionServiceTypeEnum.Container ?
                bookingContainers.Where(x => x.ContainerId > 0 && x.BookingId == entity.BookingId).Select(x => x.ContainerId).Distinct().Count()
                    :
                poDetails.Where(x => x.CombineProductId.GetValueOrDefault() == 0 && x.BookingId == entity.BookingId).Select(x => x.ProductId).Distinct().Count()
                        + poDetails.Where(x => x.CombineProductId > 0 && x.BookingId == entity.BookingId)
                            .Select(x => x.CombineProductId).Distinct().Count(),
                ProductRefId = productRefId,
                ProductSubCategory = entity?.ProductSubCategory,
                ProductType = entity.ProductType,
                IsSameDayReport = entity.IsSameDayReport,
                ReportRequest = entity.ReportRequest,
                Instructions = entity.Instructions,
                CreatedDate = entity.CreatedOn,
                CreatedDateEaqf = entity.CreatedOnEaqf,
                BookingType = entity.BookingType
            };
            var factoryCountry = factoryCountryData.FirstOrDefault(x => x.BookingId == bookingId);
            if (factoryCountry != null)
            {
                bookingItem.FactoryCity = factoryCountry.CityName;
                bookingItem.FactoryState = factoryCountry.ProvinceName;
            }
            //assing the created by name based on the usertypeid
            switch (entity.UserTypeId)
            {
                case (int)UserTypeEnum.InternalUser:
                    bookingItem.BookingCreatedByName = entityName + "(" + entity.CreatedByStaff + ")";
                    bookingItem.BookingCreatedFirstName = entity.CreatedByStaff?.Substring(0, 1);
                    break;
                case (int)UserTypeEnum.Customer:
                    bookingItem.BookingCreatedByName = UserType.GetValueOrDefault(entity.UserTypeId) + entity.CreatedByCustomer;
                    bookingItem.BookingCreatedFirstName = entity.CreatedByCustomer?.Substring(0, 1);
                    break;
                case (int)UserTypeEnum.Supplier:
                    bookingItem.BookingCreatedByName = UserType.GetValueOrDefault(entity.UserTypeId) + entity.CreatedBySupplier;
                    bookingItem.BookingCreatedFirstName = entity.CreatedBySupplier?.Substring(0, 1);
                    break;
                case (int)UserTypeEnum.Factory:
                    bookingItem.BookingCreatedByName = UserType.GetValueOrDefault(entity.UserTypeId) + entity.CreatedByFactory;
                    bookingItem.BookingCreatedFirstName = entity.CreatedByFactory.Substring(0, 1);
                    break;
            }

            return bookingItem;


        }

        public ReportItem GetInspectionReportResult(InspTransaction entity, IEnumerable<InspectionStatus> inspectionStatus, int staffId, IEnumerable<FbReportDetail> reportList)
        {
            bool isPicking = false;
            if (entity == null)
                return null;
            var activePOTransactions = entity.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value);
            foreach (var poTransaction in activePOTransactions)
            {
                if (poTransaction.PickingQuantity > 0)
                {
                    isPicking = true;
                    break;
                }
            }

            return new ReportItem()
            {
                BookingId = entity.Id,
                CustomerID = entity?.CustomerId,
                CustomerName = entity?.Customer?.CustomerName,
                FactoryName = entity?.Factory?.SupplierName,
                FbMissionId = (entity.FbMissionId == null) ? 0 : entity.FbMissionId.Value,
                PoNumber = entity?.InspPurchaseOrderTransactions?.Where(y => y.Active.HasValue && y.Active.Value).Select(x => x.Po.Pono)?.FirstOrDefault(),
                ServiceDateFrom = entity?.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = entity?.ServiceDateTo.ToString(StandardDateFormat),
                SupplierName = entity?.Supplier.SupplierName,
                ServiceType = entity?.InspTranServiceTypes?.Select(x => x.ServiceType?.Name)?.FirstOrDefault(),
                Office = entity?.Office?.LocationName,
                StatusId = entity.StatusId,
                StatusName = inspectionStatus?.Where(x => x.Id == entity.StatusId)?.Select(x => x.StatusName)?.FirstOrDefault(),
                BookingCreatedBy = entity?.CreatedByNavigation?.UserTypeId,
                IsPicking = isPicking,
                PreviousBookingNo = entity.PreviousBookingNo,
                FactoryId = entity.FactoryId,
                MissionStatus = entity.FbMissionStatusNavigation?.FbstatusName,
                CountryId = entity?.Supplier?.SuAddresses?.Select(x => x.CountryId)?.FirstOrDefault(),
                IsCsReport = entity.SchScheduleCS.Any(x => x.Csid == staffId),
                ReportProducts = entity.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value).
                Select(x => new ProductItem()
                {
                    ProductId = x.ProductRef?.Product?.Id,
                    ProductName = x.ProductRef?.Product?.ProductId,
                    ProductDescription = x.ProductRef?.Product?.ProductDescription,
                    PoNumber = x.Po?.Pono ?? "",
                    PoDetailId = x.Id,
                    ProductQuantity = x.BookingQuantity,
                    ProductCategoryName = x.ProductRef?.Product?.ProductCategoryNavigation?.Name,
                    ProductSubCategoryName = x.ProductRef?.Product?.ProductSubCategoryNavigation?.Name,
                    FbReportId = x.ProductRef.FbReportId != null ? x.ProductRef.FbReportId.Value : 0,
                    QcName = string.Join(",", entity.SchScheduleQcs.Select(y => y.Qc?.PersonName).ToArray().Distinct()),
                    ColorCode = ReportResult.FFFF.ToString(),
                    CombineProductId = x.ProductRef.CombineProductId.GetValueOrDefault(),
                    MissionTitle = reportList.Where(y => y.Id == x.ProductRef.FbReportId.GetValueOrDefault()).FirstOrDefault()?.MissionTitle ?? "",
                    ReportTitle = reportList.Where(y => y.Id == x.ProductRef.FbReportId.GetValueOrDefault()).FirstOrDefault()?.ReportTitle ?? "",
                    ReportLink = reportList.Where(y => y.Id == x.ProductRef.FbReportId.GetValueOrDefault()).FirstOrDefault()?.FinalReportPath ?? "",
                    FillingStatus = reportList.Where(y => y.Id == x.ProductRef.FbReportId.GetValueOrDefault()).FirstOrDefault()?.FbFillingStatusNavigation?.StatusName ?? "",
                    ReviewStatus = reportList.Where(y => y.Id == x.ProductRef.FbReportId.GetValueOrDefault()).FirstOrDefault()?.FbReviewStatusNavigation?.StatusName ?? "",
                    ReportStatus = reportList.Where(y => y.Id == x.ProductRef.FbReportId.GetValueOrDefault()).FirstOrDefault()?.FbReportStatusNavigation?.StatusName ?? "",
                    CombineProductCount = entity.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
                                                          && x.ProductRef.CombineProductId.HasValue && y.ProductRef.CombineProductId.HasValue && x.ProductRef.CombineProductId.Value == y.ProductRef.CombineProductId.Value).Count() == 0 ? 1 :
                                                          entity.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
                                                          && x.ProductRef.CombineProductId == y.CombineProductId).Count(),

                    IsParentProduct = (x.ProductRef.CombineProductId.GetValueOrDefault() == 0) ? true : (x.ProductRef.CombineAqlQuantity != null &&
                    x.ProductRef.CombineAqlQuantity != 0) ? true : (entity.InspProductTransactions.Where(z => z.CombineProductId == x.ProductRef.CombineProductId && z.CombineAqlQuantity.GetValueOrDefault() > 0).Count() == 0 && entity.InspPurchaseOrderTransactions.Where(z => z.ProductRef.CombineProductId == x.ProductRef.CombineProductId).FirstOrDefault().ProductRef.ProductId == x.ProductRef.ProductId ? true : false),
                    CombineAqlQuantity = x.ProductRef.CombineAqlQuantity,

                    ReportStatusColor =
                    ReportStatusColor.TryGetValue
                    (reportList.Where(y => y.Id == x.ProductRef.FbReportId.GetValueOrDefault()).FirstOrDefault()?.
                    FbReportStatus ?? 0, out string reportColor) ? reportColor : "",

                    FillingStatusColor =
                    ReportStatusColor.TryGetValue
                    (reportList.Where(y => y.Id == x.ProductRef.FbReportId.GetValueOrDefault()).FirstOrDefault()?.
                    FbFillingStatus ?? 0, out string fillingColor) ? fillingColor : "",

                    ReviewStatusColor =
                    ReportStatusColor.TryGetValue
                    (reportList.Where(y => y.Id == x.ProductRef.FbReportId.GetValueOrDefault()).FirstOrDefault()?.
                    FbReviewStatus ?? 0, out string reviewColor) ? reviewColor : ""

                }).OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ThenBy(x => x.ProductName)
            };
        }

        public InspectionBookingDetails MapBookingData(InspectionBookingDetail bookingDetail, Func<string, string> _funcGetMimeType,
                            List<ServiceTypeList> serviceTypeList, InspectionHoldReasons holdReason, List<int> customerContacts,
                            List<int> supplierContacts, List<int?> factoryContacts, List<int> buyers, List<int> brands, List<int> departments,
                            List<int> merchandisers, List<int?> shipmentTypes, List<InspectionProductDetail> productList, List<InspectionPODetail> poList,
                            List<int> containers, List<InspectionDFTransactions> dfTransactions, List<BookingFileAttachment> inspectionFileAttachments,
                            List<InspectionProductSubCategory> productSubCategoryList, List<InspectionProductSubCategory2> productSubCategory2List,
                            List<InspectionProductSubCategory3> productSubCategory3List,
                            List<ProductFileAttachmentRepsonse> productFileAttachments, List<int> prevBookingNoList, List<CommonDataSource> inspectionCsList, List<InspectionPOColorTransaction> poColorTransactions,
                            List<int> customerCheckPointList, List<InspectionPickingDetails> pickingDetails, List<InspectionPickingContactDetails> pickingContactDetails, UserTypeEnum userType)
        {
            if (bookingDetail == null)
                return null;

            // assing total number of reports based on service type
            int totalNumberOfReports = 0;

            if (serviceTypeList.Any(x => x.serviceTypeId == (int)InspectionServiceTypeEnum.Container))
            {
                totalNumberOfReports = containers.Count;
            }
            else
            {
                totalNumberOfReports = productList.Count(x => x.CombineGroupId == null) +
                                                        productList.Where(x => x.CombineGroupId != null).Select(x => x.CombineGroupId).Distinct().Count();
            }

            var response = new InspectionBookingDetails()
            {
                Id = bookingDetail.InspectionId,
                CustomerId = bookingDetail.CustomerId,
                ServiceDateFrom = Static_Data_Common.GetCustomDate(bookingDetail.ServiceDateFrom),
                ServiceDateTo = Static_Data_Common.GetCustomDate(bookingDetail.ServiceDateTo),
                SeasonId = bookingDetail.SeasonId,
                SeasonYearId = bookingDetail.SeasonYearId,
                SupplierId = bookingDetail.SupplierId,
                FactoryId = bookingDetail.FactoryId,
                ApiBookingComments = bookingDetail.ApiBookingComments,
                ApplicantEmail = bookingDetail.ApplicantEmail,
                ApplicantName = bookingDetail.ApplicantName,
                ApplicantPhoneNo = bookingDetail.ApplicantPhoneNo,
                CusBookingComments = bookingDetail.CusBookingComments,
                InternalComments = bookingDetail.InternalComments,
                OfficeId = bookingDetail.OfficeId,
                InternalReferencePo = bookingDetail.InternalReferencePo,
                StatusId = bookingDetail.StatusId,
                StatusName = bookingDetail.StatusName,
                CreatedBy = bookingDetail.CreatedBy,
                PreviousBookingNo = bookingDetail.PreviousBookingNo,
                ReinspectionType = bookingDetail.ReInspectionType,
                CreatedOn = bookingDetail.CreatedOn,
                //productCategoryId = bookingDetail.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value).Select(x => x.Product?.ProductCategory).FirstOrDefault(),
                InspectionCustomerContactList = customerContacts,
                InspectionFactoryContactList = factoryContacts,
                InspectionSupplierContactList = supplierContacts,
                InspectionCustomerBuyerList = buyers,
                InspectionCustomerBrandList = brands,
                InspectionCustomerDepartmentList = departments,
                ServiceTypeId = serviceTypeList.Select(x => x.serviceTypeId).FirstOrDefault(),
                InspectionServiceTypeList = serviceTypeList.Select(x => x.serviceTypeId).ToList(),
                TotalNumberofReports = totalNumberOfReports,
                InspectionProductList = MapInspectionPoProductData(productList, poList, productSubCategoryList, productSubCategory2List, productSubCategory3List, productFileAttachments, poColorTransactions, pickingDetails, pickingContactDetails),
                InspectionDfTransactions = dfTransactions,
                InspectionFileAttachmentList = inspectionFileAttachments,
                isPickingRequired = bookingDetail.IsPickingRequired,
                isCombineRequired = bookingDetail.IsCombineRequired,
                CustomerBookingNo = bookingDetail.CustomerBookingNo,
                QCBookingComments = bookingDetail.QCBookingComments,
                HoldReason = holdReason?.Comment,
                HoldReasonType = holdReason?.Reason,
                FirstServiceDateFrom = Static_Data_Common.GetCustomDate(bookingDetail.FirstServiceDateFrom),
                FirstServiceDateTo = Static_Data_Common.GetCustomDate(bookingDetail.FirstServiceDateTo),
                InspectionCustomerMerchandiserList = merchandisers,
                CreatedUserType = bookingDetail.CreatedUserType,
                CollectionId = bookingDetail.CollectionId,
                PriceCategoryId = bookingDetail.PriceCategoryId,
                CollectionName = bookingDetail.CollectionName,
                PriceCategoryName = bookingDetail.PriceCategoryName,
                CompassBookingNo = bookingDetail.CompassBookingNo,
                BusinessLine = bookingDetail.BusinessLine,
                InspectionLocation = bookingDetail.InspectionLocation,
                ShipmentPort = bookingDetail.ShipmentPort,
                ShipmentDate = Static_Data_Common.GetCustomDate(bookingDetail.ShipmentDate),
                EAN = bookingDetail.EAN,
                CuProductCategory = bookingDetail.CuProductCategory,
                ShipmentTypeList = shipmentTypes,
                PreviousBookingNoList = prevBookingNoList,
                EntityId = bookingDetail.EntityId,
                BookingType = bookingDetail.BookingType,
                BookingTypeName = bookingDetail.BookingTypeName,
                PaymentOptions = bookingDetail.PaymentOptions,
                IsEaqf = bookingDetail.IsEaqf.GetValueOrDefault(),
                GAPDACorrelation = bookingDetail.GAPDACorrelation,
                GAPDAEmail = bookingDetail.GAPDAEmail,
                GAPDAName = bookingDetail.GAPDAName
            };

            if (inspectionCsList != null && inspectionCsList.Any())
            {
                response.CsList = inspectionCsList.Select(x => x.Id).ToList();
            }

            if (response.InspectionProductList != null && response.InspectionProductList.Any(x => x.CombineGroupId != null))
            {
                response.InspectionProductList = response.InspectionProductList.OrderBy(x => x.CombineGroupId)
                                                  .ThenByDescending(x => x.CombineSamplingSize).ThenBy(x => x.ProductName).ToList();
            }
            else if (response.InspectionProductList != null)
            {
                response.InspectionProductList = response.InspectionProductList.OrderBy(x => x.ProductName).ToList();
            }

            //check combine data is enabled if booking has combine option
            bool isCombineDone = true;
            response.isCombineDone = !isCombineDone;
            if (productList != null && productList.Any() && bookingDetail.IsCombineRequired.HasValue
                                                && bookingDetail.IsCombineRequired.Value)
            {
                var combineProducts = productList.Where(x => x.CombineGroupId != null && x.CombineSamplingSize > 0 && x.Active.HasValue && x.Active.Value).ToList();

                response.isCombineDone = (combineProducts != null && combineProducts.Any()) ? isCombineDone : !isCombineDone;
            }

            //send filter po by product check point data
            response.PoProductDependentFilter = false;
            // apply PoProductDependentFilter if usertype is customer and Poproductbysupplier checkpoint is there
            //or if usertype is supplier or factory
            if ((userType == UserTypeEnum.Customer
                && customerCheckPointList.Any(x => x == (int)CheckPointTypeEnum.PoProductBySupplier))
                || (userType == UserTypeEnum.Supplier || userType == UserTypeEnum.Factory))
                response.PoProductDependentFilter = true;

            return response;
        }

        /// <summary>
        /// Map the inspection product,po,color and container data
        /// </summary>
        /// <param name="productList"></param>
        /// <param name="poList"></param>
        /// <param name="productSubCategoryList"></param>
        /// <param name="productSubCategory2List"></param>
        /// <param name="productFileAttachments"></param>
        /// <param name="poColorTransactions"></param>
        /// <returns></returns>
        public static List<InspectionPOProductDetails> MapInspectionPoProductData(List<InspectionProductDetail> productList,
                  List<InspectionPODetail> poList, List<InspectionProductSubCategory> productSubCategoryList, List<InspectionProductSubCategory2> productSubCategory2List,
                  List<InspectionProductSubCategory3> productSubCategory3List, List<ProductFileAttachmentRepsonse> productFileAttachments,
                  List<InspectionPOColorTransaction> poColorTransactions, List<InspectionPickingDetails> pickingDetails,
                  List<InspectionPickingContactDetails> pickingContactDetails)
        {
            List<InspectionPOProductDetails> objProductDetails = new List<InspectionPOProductDetails>();

            //if color transaction data is then the product and po will be repeated for each color transaction
            if (poColorTransactions != null && poColorTransactions.Any())
            {
                foreach (var colorTransaction in poColorTransactions)
                {
                    var bookingProduct = productList.FirstOrDefault(x => x.Id == colorTransaction.ProductRefId);
                    if (bookingProduct != null)
                    {
                        //map the product transaction
                        var productDetail = MapProductTransaction(bookingProduct, productSubCategoryList, productSubCategory2List, productSubCategory3List, productFileAttachments);

                        var poDetail = poList.FirstOrDefault(x => x.ProductRefId == bookingProduct.Id && x.Id == colorTransaction.PoTransactionId);

                        if (poDetail != null)
                        {
                            //map the purchase order transaction
                            productDetail = MapPurchaseOrderTransaction(productDetail, poDetail, pickingDetails, pickingContactDetails);
                        }

                        //map the color transaction
                        productDetail.ColorTransactionId = colorTransaction.ColorTransactionId;
                        productDetail.ColorCode = colorTransaction.ColorCode;
                        productDetail.ColorName = colorTransaction.ColorName;
                        productDetail.BookingQuantity = colorTransaction.BookingQuantity.GetValueOrDefault();
                        productDetail.PickingQuantity = colorTransaction.PickingQuantity.GetValueOrDefault();

                        objProductDetails.Add(productDetail);
                    }
                }
            }
            else
            {
                foreach (var poItem in poList)
                {
                    var bookingProduct = productList.FirstOrDefault(x => x.Id == poItem.ProductRefId);
                    if (bookingProduct != null)
                    {
                        //map the product transaction
                        var productDetail = MapProductTransaction(bookingProduct, productSubCategoryList, productSubCategory2List, productSubCategory3List, productFileAttachments);

                        //map the purchase order transaction
                        productDetail = MapPurchaseOrderTransaction(productDetail, poItem, pickingDetails, pickingContactDetails);

                        objProductDetails.Add(productDetail);
                    }
                }
            }

            return objProductDetails;
        }

        /// <summary>
        /// Map the product transaction details
        /// </summary>
        /// <param name="bookingProduct"></param>
        /// <param name="productSubCategoryList"></param>
        /// <param name="productSubCategory2List"></param>
        /// <param name="productFileAttachments"></param>
        /// <returns></returns>
        private static InspectionPOProductDetails MapProductTransaction(InspectionProductDetail bookingProduct, List<InspectionProductSubCategory> productSubCategoryList, List<InspectionProductSubCategory2> productSubCategory2List,
                  List<InspectionProductSubCategory3> productSubCategory3List, List<ProductFileAttachmentRepsonse> productFileAttachments)
        {
            var productDetail = new InspectionPOProductDetails();

            if (bookingProduct.ProductSubCategoryId == null)
            {
                productDetail.BookingCategorySubProductList = productSubCategoryList.Where(x => x.ProductCategoryId == bookingProduct.ProductCategoryId)
                                                                .Select(x => new CommonDataSource() { Id = x.ProductSubCategoryId.Value, Name = x.ProductSubCategoryName }).ToList();
            }
            else if (bookingProduct.ProductSubCategoryId != null)
            {
                productDetail.BookingCategorySubProductList = new List<CommonDataSource>() { new CommonDataSource() { Id = bookingProduct.ProductSubCategoryId.Value, Name = bookingProduct.ProductSubCategoryName } };
            }

            if (bookingProduct.ProductCategorySub2Id == null)
            {
                productDetail.BookingCategorySub2ProductList = productSubCategory2List.Where(x => x.ProductSubCategoryId == bookingProduct.ProductSubCategoryId)
                                                                .Select(x => new CommonDataSource() { Id = x.ProductSubCategory2Id.Value, Name = x.ProductSubCategory2Name }).ToList();
            }
            else if (bookingProduct.ProductCategorySub2Id != null)
            {
                productDetail.BookingCategorySub2ProductList = new List<CommonDataSource>() { new CommonDataSource() { Id = bookingProduct.ProductCategorySub2Id.Value, Name = bookingProduct.ProductCategorySub2Name } };
            }

            if (bookingProduct.ProductCategorySub3Id == null)
            {
                productDetail.BookingCategorySub3ProductList = productSubCategory3List.Where(x => x.ProductSubCategory2Id == bookingProduct.ProductCategorySub2Id)
                                                                .Select(x => new CommonDataSource() { Id = x.ProductSubCategory3Id.Value, Name = x.ProductSubCategory3Name }).ToList();
            }
            else if (bookingProduct.ProductCategorySub3Id != null)
            {
                productDetail.BookingCategorySub3ProductList = new List<CommonDataSource>() { new CommonDataSource() { Id = bookingProduct.ProductCategorySub3Id.Value, Name = bookingProduct.ProductCategorySub3Name } };
            }

            var FileAttachments = productFileAttachments.Where(x => x.ProductId == bookingProduct.ProductId).ToList();

            productDetail.Id = bookingProduct.Id;
            productDetail.ProductId = bookingProduct.ProductId;
            productDetail.ProductName = bookingProduct.ProductName;
            productDetail.ProductCategorySub2Id = bookingProduct.ProductCategorySub2Id;
            productDetail.ProductCategorySub3Id = bookingProduct.ProductCategorySub3Id;

            productDetail.ProductSubCategoryId = bookingProduct.ProductSubCategoryId;
            productDetail.ProductCategoryId = bookingProduct.ProductCategoryId;
            productDetail.ProductDesc = bookingProduct.ProductDesc;
            productDetail.ProductFileAttachments = FileAttachments;
            productDetail.InspectionId = bookingProduct.InspectionId;
            productDetail.Unit = bookingProduct.Unit;
            productDetail.UnitCount = bookingProduct.UnitCount;

            productDetail.Aql = bookingProduct.Aql;
            productDetail.Remarks = bookingProduct.Remarks;
            productDetail.Critical = bookingProduct.Critical;
            productDetail.Major = bookingProduct.Major;
            productDetail.Minor = bookingProduct.Minor;
            productDetail.AqlQuantity = bookingProduct.AqlQuantity;
            productDetail.SampleType = bookingProduct.SampleType;

            productDetail.CombineSamplingSize = bookingProduct.CombineSamplingSize;
            productDetail.CombineGroupId = bookingProduct.CombineGroupId;
            productDetail.ReportId = bookingProduct.ReportId;

            productDetail.ProductCategoryName = bookingProduct.ProductCategoryName;
            productDetail.ProductSubCategoryName = bookingProduct.ProductSubCategoryName;
            productDetail.ProductCategorySub2Name = bookingProduct.ProductCategorySub2Name;
            productDetail.ProductCategorySub3Name = bookingProduct.ProductCategorySub3Name;

            productDetail.FactoryReference = bookingProduct.FactoryReference;
            productDetail.Barcode = bookingProduct.Barcode;

            productDetail.IsEcopack = bookingProduct.IsEcopack.GetValueOrDefault();
            productDetail.IsDisplayMaster = bookingProduct.IsDisplayMaster.GetValueOrDefault();
            productDetail.FBTemplateId = bookingProduct.FBTemplateId;
            productDetail.FbTemplateName = bookingProduct.FbTemplateName;
            productDetail.TotalBookingQuantity = bookingProduct.TotalBookingQuantity;
            productDetail.ParentProductId = bookingProduct.ParentProductId;
            productDetail.ParentProductName = bookingProduct.ParentProductName;
            productDetail.IsNewProduct = bookingProduct.IsNewProduct != null ? bookingProduct.IsNewProduct : false;

            productDetail.AsReceivedDate = Static_Data_Common.GetCustomDate(bookingProduct.AsReceivedDate);
            productDetail.TfReceivedDate = Static_Data_Common.GetCustomDate(bookingProduct.TfReceivedDate);

            productDetail.ProductImageCount = bookingProduct.ProductImageCount;
            productDetail.UnitNameCount = bookingProduct.UnitCount.GetValueOrDefault() > 0 ? bookingProduct.UnitName + " (" + bookingProduct.UnitCount + ")" : bookingProduct.UnitName;

            return productDetail;
        }

        /// <summary>
        /// Map the purchase order details
        /// </summary>
        /// <param name="productDetail"></param>
        /// <param name="poDetail"></param>
        /// <returns></returns>
        private static InspectionPOProductDetails MapPurchaseOrderTransaction(InspectionPOProductDetails productDetail, InspectionPODetail poDetail, List<InspectionPickingDetails> pickingDetails,
                  List<InspectionPickingContactDetails> pickingContactDetails)
        {
            productDetail.PoTransactionId = poDetail.Id;
            productDetail.ProductRefId = poDetail.ProductRefId;
            productDetail.PoName = poDetail.PoName;
            productDetail.BookingQuantity = poDetail.BookingQuantity;
            productDetail.ExistingBookingQuantity = poDetail.ExistingBookingQuantity;
            productDetail.PoQuantity = poDetail.PoQuantity;
            productDetail.PoReminingQuantity = poDetail.PoReminingQuantity;
            productDetail.PickingQuantity = poDetail.PickingQuantity;
            productDetail.PoId = poDetail.PoId;
            productDetail.DestinationCountryID = poDetail.DestinationCountryID;
            productDetail.DestinationCountryName = poDetail.DestinationCountryName;
            productDetail.ETD = Static_Data_Common.GetCustomDate(poDetail.ETDDate);
            productDetail.CustomerReferencePo = poDetail.CustomerReferencePo;
            productDetail.BaseCustomerReferencePo = poDetail.BaseCustomerReferencePo;
            productDetail.ContainerId = poDetail.ContainerId;

            //if picking details available then add picking information in the edit fetch
            if (pickingDetails.Any())
            {
                productDetail.SaveInspectionPickingList = new List<SaveInspectionPickingDetails>();
                var poPickingDetails = pickingDetails.Where(x => x.PoTransactionId == poDetail.Id).ToList();

                foreach (var pickingData in poPickingDetails)
                {
                    productDetail.SaveInspectionPickingList.Add(MapEditInspectionPickingData(pickingData, pickingContactDetails));
                }
            }


            return productDetail;
        }

        /// <summary>
        /// Map the inspection picking data for edit fetch
        /// </summary>
        /// <param name="pickingData"></param>
        /// <param name="pickingContactDetails"></param>
        /// <returns></returns>
        public static SaveInspectionPickingDetails MapEditInspectionPickingData(InspectionPickingDetails pickingData,
                                                        List<InspectionPickingContactDetails> pickingContactDetails)
        {
            SaveInspectionPickingDetails pickingDetail = new SaveInspectionPickingDetails();
            pickingDetail.Id = pickingData.Id;
            pickingDetail.LabOrCustomerId = pickingData.labId > 0 ? pickingData.labId : pickingData.CustomerId;
            pickingDetail.LabOrCustomerName = pickingData.labId > 0 ? pickingData.labName : pickingData.CustomerName;
            pickingDetail.LabType = pickingData.labId > 0 ? (int)LabTypeEnum.Lab : pickingData.CustomerId > 0 ? (int)LabTypeEnum.Customer : 0;
            pickingDetail.LabOrCustomerAddressId = pickingData.labAddressId > 0 ? pickingData.labAddressId : pickingData.CustomerAddressId;
            pickingDetail.LabOrCustomerAddressName = pickingData.labAddressId > 0 ? pickingData.labAddress : pickingData.CustomerAddress;
            pickingDetail.PickingQuantity = pickingData.PickingQuantity;
            pickingDetail.Remarks = pickingData.Remarks;
            var contactDetails = pickingContactDetails.Where(x => x.InspectionPickingId == pickingData.Id).ToList();
            pickingDetail.PickingContactList = pickingData.labId > 0 ?
                                                contactDetails.Select(x => new SavePickingContact()
                                                {
                                                    Id = x.Id,
                                                    LabOrCustomerContactId = x.LabContactId.GetValueOrDefault(),
                                                    LabOrCustomerContactName = x.LabContactName
                                                }).ToList()
                                                : contactDetails.Select(x => new SavePickingContact()
                                                {
                                                    Id = x.Id,
                                                    LabOrCustomerContactId = x.CustomerContactId.GetValueOrDefault(),
                                                    LabOrCustomerContactName = x.CustomerContactName
                                                }).ToList();
            pickingDetail.LabOrCustomerContactName = string.Join(",", pickingDetail.PickingContactList.Select(x => x.LabOrCustomerContactName).ToList());
            return pickingDetail;
        }

        public BookingMailRequest MapBookingForEmail(BookingMailRequest data, BookingMapEmailData bookingMapEmailData,
                           int chinaId)
        {
            if (bookingMapEmailData.BookingDetail != null)
            {
                data.CustomerName = bookingMapEmailData.BookingDetail.CustomerName;

                data.CustomerBookingNo = bookingMapEmailData.BookingDetail.CustomerBookingNo;

                data.BookingType = bookingMapEmailData.BookingDetail.BookingType;

                data.BookingTypeValue = bookingMapEmailData.BookingDetail.BookingTypeName;

                data.IsEmailRequired = true;

                data.StatusId = bookingMapEmailData.BookingDetail.StatusId;

                data.StatusName = bookingMapEmailData.BookingDetail.StatusName;

                data.BookingComment = bookingMapEmailData.BookingDetail.ApiBookingComments;

                //if service types are available then apply the service type data
                if (bookingMapEmailData.ServiceTypes.Any())
                {
                    var serviceType = bookingMapEmailData.ServiceTypes.FirstOrDefault();

                    if (serviceType != null)
                    {
                        data.ServiceTypeId = serviceType.ServiceTypeId;
                        data.ServiceType = serviceType.ServiceTypeName;
                    }
                }

                data.OfficeId = bookingMapEmailData.BookingDetail.OfficeId;

                data.ServiceDateFrom = bookingMapEmailData.BookingDetail.ServiceDateFrom.ToString(StandardDateFormat, CultureInfo.InvariantCulture);

                data.ServiceDateTo = bookingMapEmailData.BookingDetail.ServiceDateTo.ToString(StandardDateFormat, CultureInfo.InvariantCulture);

                data.ApplicantName = bookingMapEmailData.BookingDetail.ApplicantName;

                data.ApplicantEmail = bookingMapEmailData.BookingDetail.ApplicantEmail;

                data.HoldReason = bookingMapEmailData.BookingHoldReasons?.Comment;
                data.HoldReasonType = bookingMapEmailData.BookingHoldReasons?.Reason;

                data.GuidId = bookingMapEmailData.BookingDetail.GuidId;

                data.ProductCategory = bookingMapEmailData.BookingDetail.ProductCategoryName;

                data.FirstServiceDateFrom = bookingMapEmailData.BookingDetail.FirstServiceDateFrom?.ToString(StandardDateFormat);

                data.FirstServiceDateTo = bookingMapEmailData.BookingDetail.FirstServiceDateTo?.ToString(StandardDateFormat);

                if (bookingMapEmailData.BrandList != null)
                    data.Brand = string.Join(", ", bookingMapEmailData.BrandList?.Select(x => x.Name).ToList());

                if (bookingMapEmailData.DepartmentList != null)
                    data.Department = string.Join(", ", bookingMapEmailData.DepartmentList?.Select(x => x.Name).ToList());

                data.Season = bookingMapEmailData.CustomerSeasonData?.Name;

                if (bookingMapEmailData.BuyerList != null)
                    data.Buyer = string.Join(", ", bookingMapEmailData.BuyerList?.Select(x => x.Name).ToList());


                string factoryemail = "";
                string factoryPhone = "";
                if (bookingMapEmailData.Factcontactlist != null && bookingMapEmailData.Factcontactlist.Any())
                {
                    factoryemail = bookingMapEmailData.Factcontactlist.Select(x => x.ContactEmail).Distinct().ToList().Aggregate((x, y) => x + ";" + y);
                    factoryPhone = string.Join(", ", bookingMapEmailData.Factcontactlist.Select(x => x.Phone).Distinct().ToList());
                }
                //factory
                data.FactoryAddress = bookingMapEmailData.FactoryAddress?.Address;
                data.FactoryRegionalAddress = bookingMapEmailData.FactoryAddress?.RegionalAddress;
                data.SupplierContactName = string.Join(", ", bookingMapEmailData.InspSupplierContacts?.Select(x => x.ContactName).ToList());
                data.FactoryContactName = string.Join(", ", bookingMapEmailData.Factcontactlist?.Select(x => x.ContactName).ToList());
                data.FactoryName = bookingMapEmailData.BookingDetail.FactoryName;
                data.FactoryMail = factoryemail;
                data.FactoryPhone = factoryPhone;
                data.IsChinaCountry = bookingMapEmailData.FactoryAddress?.countryId == chinaId ? true : false;
                data.FactoryCountry = bookingMapEmailData.FactoryAddress?.Country;

                // supplier contact
                var lstsuppliercontact = bookingMapEmailData.InspSupplierContacts.Select(x => new { x.ContactName, x.ContactEmail, x.Phone }).Distinct().ToList();
                string supplieremail = "";
                string supplierPhone = "";
                if (lstsuppliercontact != null && lstsuppliercontact.Any())
                {
                    supplieremail = lstsuppliercontact.Select(x => x.ContactEmail).Distinct().ToList().Aggregate((x, y) => x + ";" + y);
                    supplierPhone = string.Join(", ", lstsuppliercontact.Select(x => x.Phone).Distinct().ToList());
                }
                // Supplier
                data.SupplierName = bookingMapEmailData.BookingDetail.SupplierName;
                data.SupplierPhone = supplierPhone;
                data.SupplierMail = supplieremail;
                data.SupplierAddress = bookingMapEmailData.SupplierAddress?.Address;
                data.ShipmentDate = bookingMapEmailData.BookingDetail.ShipmentDate?.ToString(StandardDateFormat);
                data.ApplyDate = bookingMapEmailData.BookingDetail.CreatedOn?.ToString(StandardDateFormat);
                data.SeasonYear = bookingMapEmailData.BookingDetail.SeasonYear?.ToString();
                data.BusinessLine = bookingMapEmailData.BookingDetail.BusinessLine.GetValueOrDefault();
                data.IsShowSoftLineItems = bookingMapEmailData.BookingDetail.BusinessLine == (int)BusinessLine.SoftLine;
                data.ReInspectionTypeName = bookingMapEmailData.BookingDetail.ReInspectionTypeName;

                data.CusBookingComments = bookingMapEmailData.BookingDetail.CusBookingComments;

                // Da
                data.DACorrelation = bookingMapEmailData.BookingDetail.GAPDACorrelation.GetValueOrDefault();
                data.DAEmail = bookingMapEmailData.BookingDetail.GAPDAEmail;
                data.DAName = bookingMapEmailData.BookingDetail.GAPDAName;
            }

            return data;
        }

        #region CancelBooking
        public BookingCancelRescheduleReasonItem GetBookingCancelReasons(InspCancelReason entity)
        {
            if (entity == null)
                return null;
            return new BookingCancelRescheduleReasonItem
            {
                Id = entity.Id,
                Reason = entity.Reason
            };
        }
        public InspTranCancel SaveCancelEntity(SaveCancelBookingRequest request, int? userId)
        {
            return new InspTranCancel
            {
                InspectionId = request.BookingId,
                Comments = request.Comment,
                CurrencyId = request.CurrencyId,
                InternalComments = request.InternalComment,
                ReasonTypeId = request.ReasonTypeId,
                TimeTypeId = request.TimeTypeId,
                TravellingExpense = request.TravelExpense != null ? Convert.ToDecimal(request.TravelExpense) : 0,
                CreatedBy = userId > 0 ? userId : null
            };
        }
        public InspTranCancel UpdateCancelEntity(SaveCancelBookingRequest request, InspTranCancel objCancel, int? userId)
        {
            objCancel.InspectionId = request.BookingId;
            objCancel.Comments = request.Comment;
            objCancel.CurrencyId = request.CurrencyId;
            objCancel.InternalComments = request.InternalComment;
            objCancel.ReasonTypeId = request.ReasonTypeId;
            objCancel.TimeTypeId = request.TimeTypeId;
            objCancel.TravellingExpense = request.TravelExpense != null ? Convert.ToDecimal(request.TravelExpense) : 0;
            objCancel.ModifiedBy = userId > 0 ? userId : null;
            objCancel.ModifiedOn = DateTime.Now;
            return objCancel;

        }
        public SaveCancelBookingRequest EditCancelEntity(InspTranCancel entity)
        {
            return new SaveCancelBookingRequest
            {
                BookingId = entity.InspectionId,
                Comment = entity.Comments,
                CurrencyId = entity.CurrencyId,
                InternalComment = entity.InternalComments,
                ReasonTypeId = entity.ReasonTypeId,
                TimeTypeId = entity.TimeTypeId,
                TravelExpense = entity.TravellingExpense != null ? Convert.ToDouble(entity.TravellingExpense) : 0
            };
        }
        #endregion CancelBooking
        #region RescheduleBooking
        public SaveRescheduleRequest EditRescheduleEntity(InspTranReschedule entity)
        {
            return new SaveRescheduleRequest
            {
                BookingId = entity.InspectionId,
                Comment = entity.Comments,
                CurrencyId = entity.CurrencyId,
                InternalComment = entity.InternalComments,
                ReasonTypeId = entity.ReasonTypeId,
                TimeTypeId = entity.TimeTypeId,
                ServiceFromDate = Static_Data_Common.GetCustomDate(entity.ServiceFromDate),
                ServiceToDate = Static_Data_Common.GetCustomDate(entity.ServiceToDate),
                TravelExpense = entity.TravellingExpense != null ? Convert.ToDouble(entity.TravellingExpense) : 0,
                FirstServiceDateFrom = Static_Data_Common.GetCustomDate(entity.Inspection.FirstServiceDateFrom),
                FirstServiceDateTo = Static_Data_Common.GetCustomDate(entity.Inspection.FirstServiceDateTo)
            };
        }
        public BookingCancelRescheduleReasonItem GetBookingRescheduleReasons(InspRescheduleReason entity)
        {
            if (entity == null)
                return null;
            return new BookingCancelRescheduleReasonItem
            {
                Id = entity.Id,
                Reason = entity.Reason
            };
        }
        public InspTranReschedule SaveRescheduleEntity(SaveRescheduleRequest request, int? userId)
        {
            return new InspTranReschedule
            {
                InspectionId = request.BookingId,
                Comments = request.Comment,
                CurrencyId = request.CurrencyId,
                InternalComments = request.InternalComment,
                ReasonTypeId = request.ReasonTypeId,
                TimeTypeId = request.TimeTypeId,
                TravellingExpense = request.TravelExpense != null ? Convert.ToDecimal(request.TravelExpense) : 0,
                ServiceFromDate = request.ServiceFromDate.ToDateTime(),
                ServiceToDate = request.ServiceToDate.ToDateTime(),
                CreatedBy = userId > 0 ? userId : null
            };
        }
        public InspTranReschedule UpdateRescheduleEntity(SaveRescheduleRequest request, InspTranReschedule reschedule, int? userId)
        {
            reschedule.InspectionId = request.BookingId;
            reschedule.Comments = request.Comment;
            reschedule.CurrencyId = request.CurrencyId;
            reschedule.InternalComments = request.InternalComment;
            reschedule.ReasonTypeId = request.ReasonTypeId;
            reschedule.TimeTypeId = request.TimeTypeId;
            reschedule.TravellingExpense = request.TravelExpense != null ? Convert.ToDecimal(request.TravelExpense) : 0;
            reschedule.ServiceFromDate = request.ServiceFromDate.ToDateTime();
            reschedule.ServiceToDate = request.ServiceToDate.ToDateTime();
            reschedule.ModifiedBy = userId > 0 ? userId : null;
            reschedule.ModifiedOn = DateTime.Now;
            return reschedule;
        }
        #endregion RescheduleBooking
        #region CancelRescheduleBooking
        public EditCancelBookingItem GetCancelBookingItem(InspTransaction entity, int quotationBookingsCount, bool isRescheduleBooking)
        {
            if (entity == null)
                return null;
            return new EditCancelBookingItem()
            {
                BookingId = entity.Id,
                CustomerName = entity?.Customer?.CustomerName,
                FactoryName = entity?.Factory?.SupplierName,
                ServiceDateFrom = entity?.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = entity?.ServiceDateTo.ToString(StandardDateFormat),
                SupplierName = entity?.Supplier.SupplierName,
                ServiceType = entity?.InspTranServiceTypes?.Select(x => x.ServiceType?.Name)?.FirstOrDefault(),
                ProductCategory = entity?.InspProductTransactions?.Where(y => y.Active.HasValue && y.Active.Value).
                Select(x => x.Product?.ProductCategoryNavigation?.Name)?.FirstOrDefault(),
                CustomerId = entity.CustomerId,
                FactoryId = entity.FactoryId,
                FirstServiceDateFrom = Static_Data_Common.GetCustomDate(entity.FirstServiceDateFrom.GetValueOrDefault()), //.ToString(StandardDateFormat),
                FirstServiceDateTo = Static_Data_Common.GetCustomDate(entity.FirstServiceDateTo.GetValueOrDefault()), //.ToString(StandardDateFormat)
                IsMultiBookingQuotation = quotationBookingsCount > 1,
                StatusId = entity.StatusId,
                IsRescheduleBooking = isRescheduleBooking
            };
        }
        #endregion CancelRescheduleBooking
        #region Booking contact

        public InspBookingContacts GetBookingContact(InspBookingContact entity)
        {
            if (entity == null)
                return null;
            return new InspBookingContacts
            {
                Id = entity.Id,
                ContactInformation = entity.ContactInformation ?? "",
                OfficeAddress = entity?.Office?.Address,
                OfficeFax = entity?.Office?.Fax,
                OfficeName = entity?.Office?.LocationName,
                FactoryCountry = entity?.FactoryCountry.CountryName,
                OfficeTelNo = entity?.Office?.Tel,
                UserId = entity?.UserId,
                Default = entity.Default,
                FullName = entity.User?.FullName,
                StaffId = entity.User?.StaffId,
                StaffName = entity.User?.Staff?.PersonName,
                CompanyEmail = entity.User?.Staff?.CompanyEmail,
                CompanyPhone = entity.User?.Staff?.CompanyMobileNo
            };
        }

        public BookingContactModel GetBookingContactModel(IEnumerable<UserStaffDetails> entity, RefLocation office)
        {
            if (entity == null)
                return null;
            var data = entity.FirstOrDefault();
            BookingContactModel res = new BookingContactModel
            {
                OfficeAddress = office.Address,
                OfficeFax = office.Fax,
                OfficeName = office.LocationName
            };
            string email = "";
            //string tel = "";
            foreach (var item in entity.Where(x => x.StaffId != 0))
            {
                if (item.EmailAddress != null && item.EmailAddress != "")
                    email += item.EmailAddress + ", ";
                //if (item != null && item.User.Staff.CompanyMobileNo != "")
                //    tel += item.User.Staff.CompanyMobileNo + ", ";
            }
            email = email.Trim();
            //tel = office.Tel.Trim();
            email = String.IsNullOrWhiteSpace(email) ? email : email.Remove(email.Length - 1);
            //tel = String.IsNullOrWhiteSpace(tel) ? tel : tel.Remove(email.Length - 1);
            res.PlanningEmailTo = email;
            res.OfficeTelNo = office.Tel;
            return res;
        }

        #endregion Booking contact

        #region Booking rule

        public InspBookingRules GetBookingRule(InspBookingRule entity)
        {
            if (entity == null)
                return null;
            return new InspBookingRules
            {
                Id = entity.Id,
                LeadDays = entity.LeadDays,
                BookingRule = entity?.BookingRule,
                FactoryCountryId = entity?.FactoryCountryId
            };
        }

        #endregion Booking rule

        public ReInspectionTypeResponse GetReInspectionTypes(IEnumerable<RefReInspectionType> refReInspectionTypeList)
        {
            var response = new ReInspectionTypeResponse();
            List<ReInspectionTypeData> inspectionTypeList = new List<ReInspectionTypeData>();
            if (refReInspectionTypeList == null)
                return null;
            foreach (var type in refReInspectionTypeList)
            {
                ReInspectionTypeData refReInspectionType = new ReInspectionTypeData();
                refReInspectionType.id = type.Id;
                refReInspectionType.name = type.Name;
                inspectionTypeList.Add(refReInspectionType);
            }

            if (inspectionTypeList.Count() > 0)
            {
                response.reInspectionTypeList = inspectionTypeList;
                response.result = ReInspectionTypeResult.success;
            }
            else
            {
                response.reInspectionTypeList = null;
                response.result = ReInspectionTypeResult.notFound;

            }

            return response;

        }

        public List<InspectionPOProductItem> MapPOCancelBooking(List<SaveInspectionPODetails> request)
        {
            if (request == null) return null;
            var cancelPORequest = new List<InspectionPOProductItem>();
            foreach (var item in request)
            {
                cancelPORequest.Add(new InspectionPOProductItem
                {
                    PoNo = item.PoName,
                    PoDetailId = item.PoDetailId,
                    ProductId = item.ProductId.ToString(),
                    ProductDesc = item.ProductDesc,
                    BookingQty = item.BookingQuantity,
                    PickingQty = item.PickingQuantity,
                    Remarks = item.Remarks
                });
            }
            return cancelPORequest;
        }

        public List<InspectionPOProductItem> MapPOBookingList(List<ProductInfo> request)
        {
            if (request == null) return null;
            var cancelPORequest = new List<InspectionPOProductItem>();
            foreach (var item in request)
            {
                cancelPORequest.Add(new InspectionPOProductItem
                {
                    PoNo = item.PoName,
                    PoDetailId = item.PoDetailId,
                    ProductId = item.ProductName,
                    ProductDesc = item.ProductDescription,
                    BookingQty = item.BookingQuantity,
                    PickingQty = null,
                    Remarks = null,
                    ColorCode = item.ColorCode,
                    ColorName = item.ColorName,
                    Unit = item.UnitName
                });
            }
            return cancelPORequest;
        }
        public InspTransaction UpdateBookingStatusServiceDate(SaveRescheduleRequest request, int? userId, InspTransaction entity)
        {
            entity.Id = request.BookingId;
            entity.ServiceDateFrom = request.ServiceFromDate.ToDateTime();
            entity.ServiceDateTo = request.ServiceToDate.ToDateTime();
            entity.UpdatedBy = userId > 0 ? userId : null;
            entity.StatusId = (int)BookingStatus.Rescheduled;
            entity.UpdatedOn = DateTime.Now;
            entity.FirstServiceDateFrom = request.FirstServiceDateFrom.ToDateTime();
            entity.FirstServiceDateTo = request.FirstServiceDateTo.ToDateTime();
            return entity;
        }
        public InspTranStatusLog BookingStatusLog(SaveRescheduleRequest request, InspTransaction entity, int userId)
        {
            return new InspTranStatusLog
            {
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                StatusId = (int)BookingStatus.Rescheduled,
                ServiceDateFrom = request.ServiceFromDate.ToDateTime(),
                ServiceDateTo = request.ServiceToDate.ToDateTime(),
                StatusChangeDate = DateTime.Now,
                EntityId = entity.EntityId
            };
        }
        public InspTranStatusLog BookingStatusLogForCancel(SaveCancelBookingRequest request, InspTransaction entity, int userId)
        {
            return new InspTranStatusLog
            {
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                StatusId = (int)BookingStatus.Cancel,
                StatusChangeDate = DateTime.Now,
                EntityId = entity.EntityId
            };
        }
        /// <summary>
        /// Map the dynamic field configuration for the customer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public InspDfTransaction MapDFCustomerConfigurationEntity(InspectionDFTransactions request, int userId)
        {
            if (request == null)
                return null;

            var entity = new InspDfTransaction
            {
                Id = request.Id,
                BookingId = request.BookingId,
                ControlConfigurationId = request.ControlConfigurationId,
                Value = request.Value,
                Active = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
            };

            return entity;
        }

        /// <summary>
        /// Map Dynamic Fields value configured for customer 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public InspectionDFTransactions GetInspectionDfTransactionResult(InspDfTransaction entity)
        {
            if (entity == null)
                return null;

            InspectionDFTransactions objTranscation = null;


            objTranscation = new InspectionDFTransactions()
            {
                Id = entity.Id,
                BookingId = entity.BookingId,
                ControlConfigurationId = entity.ControlConfigurationId,
                Value = entity.Value

            };

            return objTranscation;
        }

        //Map booking details to Product for the booking summary export
        public BookingExportItem MapProductsToBooking(BookingExportItem entity, IEnumerable<BookingItem> bookings,
                                            int? estimateManday, List<InspectionBookingDFData> bookingDFDataList,
                                            IEnumerable<FBReportDetails> fbReportDetails)
        {
            var bookingData = bookings.Where(x => x.BookingId == entity.BookingId).FirstOrDefault();

            return new BookingExportItem
            {
                BookingId = entity.BookingId,
                ProductId = entity.ProductId,
                ProductName = entity.ProductName,
                PoNumber = entity.PoNumber,
                ProductDescription = entity.ProductDescription,
                BookingQuantity = entity.BookingQuantity,
                BookingStatus = entity.BookingStatus,
                ProductSubCategory = entity.ProductSubCategory,
                ProductSubCategory2 = entity.ProductSubCategory2,
                FactoryReference = entity.FactoryReference,
                InspectedQuantity = entity.ReportId > 0 ? fbReportDetails.Where(x => x.ReportId == entity.ReportId).Select(x => x.InspectedQuantity).FirstOrDefault() : 0,
                ReportPath = entity.ReportId > 0 ? fbReportDetails.Where(x => x.ReportId == entity.ReportId).Select(x => x.ReportPath).FirstOrDefault() : "",
                ReportResult = entity.ReportId > 0 ? fbReportDetails.Where(x => x.ReportId == entity.ReportId).Select(x => x.ReportResult).FirstOrDefault() : "",
                ReportResultId = entity.ReportId > 0 ? fbReportDetails.Where(x => x.ReportId == entity.ReportId).Select(x => x.ReportStatusId).FirstOrDefault() : 0,
                ReportId = entity.ReportId,
                ReportStatus = entity.ReportId > 0 ? fbReportDetails.Where(x => x.ReportId == entity.ReportId).Select(x => x.ReportStatus).FirstOrDefault() : "",
                CreatedDate = entity.CreatedDate,
                InspectionFromDate = entity.ServiceStartDate,
                InspectionToDate = entity.ServiceEndDate,
                UpdatedDate = entity.UpdatedDate,
                ProductImage = entity.ProductImage,
                CombineProductId = entity.CombineProductId,
                CombineAqlQuantity = entity.CombineAqlQuantity,
                CustomerName = bookingData.CustomerName,
                CustomerId = bookingData.CustomerId.GetValueOrDefault(),
                SupplierName = bookingData.SupplierName,
                FactoryName = bookingData.FactoryName,
                ServiceType = bookingData.ServiceType,
                StatusName = bookingData.StatusName,
                StatusId = bookingData.StatusId,
                Office = bookingData.Office,
                QuotationStatusName = bookingData.QuotationStatusName,
                ProductCategory = entity?.ProductCategory,
                ApplyDate = bookingData.ApplyDate,
                FirstServiceDateFrom = bookingData.FirstServiceDateFrom,
                FirstServiceDateTo = bookingData.FirstServiceDateTo,
                CustomerBookingNo = bookingData.CustomerBookingNo,
                CSName = bookingData.CSName,
                Etd = entity.Etd,
                SRDate = entity.SRDate,
                QuotaitonManDay = estimateManday ?? 0,
                bookingDFList = bookingDFDataList.Where(x => x.BookingNo == entity.BookingId).ToList(),
                ServiceStartDate = bookingData.ServiceDateFrom,
                ServiceEndDate = bookingData.ServiceDateTo,
                DeptIds = bookingData.DeptIds,
                FactoryCity = bookingData.FactoryCity,
                FactoryCountry = bookingData.FactoryCountry,
                FactoryState = bookingData.FactoryState,
                APIBookingRemarks = bookingData.BookingAPIComments,
                IsPickingRequired = bookingData.IsPickingButtonVisible,
                DestinationCountry = entity.DestinationCountry,
                ContainerName = entity.ContainerName,
                CollectionName = bookingData.CollectionName,
                PriceCategoryName = bookingData.PriceCategoryName,
                Barcode = entity.Barcode,
                IsEcoPack = entity.IsEcoPack,
                BrandNames = bookingData.BrandNames,
                BuyerNames = bookingData.BuyerNames,
                DeptNames = bookingData.DeptNames,
                IsPicking = bookingData.IsPicking,
                FactoryCountryName = bookingData.FactoryCountryName,
                BookingCreatedByName = bookingData.BookingCreatedByName
            };
        }
        /// <summary>
        /// Map the dynamic fields with the booking data
        /// </summary>
        /// <param name="dtBookingTable"></param>
        /// <param name="bookingDFHeaders"></param>
        /// <param name="bookingDFDataList"></param>
        /// <returns></returns>
        public DataTable MapBookingWithDynamicFields(DataTable dtBookingTable, List<InspectionBookingDFData> bookingDFDataList)
        {
            if (bookingDFDataList != null && bookingDFDataList.Any())
            {
                var bookingDFHeaders = bookingDFDataList.Select(x => x.DFName).Distinct().ToList();

                //add the dynamic headers
                foreach (var dfHeader in bookingDFHeaders)
                {
                    dtBookingTable.Columns.Add(dfHeader, typeof(string));
                }

                foreach (DataRow row in dtBookingTable.Rows)
                {
                    var dfList = bookingDFDataList.Where(x =>
                          x.BookingNo == Int32.Parse(row["BookingNo"].ToString())).ToList();

                    if (dfList.Any())
                    {
                        foreach (var dfHeader in bookingDFHeaders)
                        {
                            row[dfHeader] = dfList.FirstOrDefault(x => x.DFName == dfHeader)?.DFValue;
                        }
                    }
                }
            }

            return dtBookingTable;
        }

        /// <summary>
        /// Map the booking export data
        /// </summary>
        /// <param name="bookingProductList"></param>
        /// <param name="bookingDetails"></param>
        /// <param name="bookingDeptAccess"></param>
        /// <param name="bookingBrandAccess"></param>
        /// <param name="bookingBuyerAccess"></param>
        /// <param name="serviceTypeList"></param>
        /// <param name="csConfigList"></param>
        /// <param name="quotationDetails"></param>
        /// <param name="poDetails"></param>
        /// <param name="factoryCountryList"></param>
        /// <returns></returns>
        public List<InspectionBookingExportItem> MapProductDataToBooking(List<InspectionProductsExportData> bookingProductList, List<InspectionBookingExportData> bookingDetails,
                                           List<BookingDeptAccess> bookingDeptAccess, List<BookingBrandAccess> bookingBrandAccess, List<BookingBuyerAccess> bookingBuyerAccess,
                                           List<ServiceTypeList> serviceTypeList, List<CSConfigDetail> csConfigList, List<InspectionQuotationExportData> quotationDetails,
                                           List<InspectionPOExportData> poDetails, List<FactoryCountry> factoryCountryList, IEnumerable<InspTranStatusLog> statusLogs,
                                           List<InspectionHoldReasons> inspectionHoldReasons, List<InspectionCancelReason> inspectionCancelReasons, string entityName,
                                           List<SupplierCode> supplierCodes)
        {
            var exportItems = new List<InspectionBookingExportItem>();

            int prevBookingId = 0;

            //if you change the below sorting total manday value will go wrong
            bookingProductList = bookingProductList.OrderBy(x => x.BookingId).ToList();

            foreach (var bookingProduct in bookingProductList)
            {
                List<InspectionPOExportData> poData = new List<InspectionPOExportData>();

                var bookingId = bookingProduct.BookingId;
                //take booking base detail by booking id
                var bookingData = bookingDetails.FirstOrDefault(x => x.BookingNo == bookingId);
                //take quotation detail by bookingid
                var quotationData = quotationDetails.Where(x => x.BookingId == bookingId).ToList();
                //if booking service type is container then filter po details by productid and containerid
                if (serviceTypeList.Any(x => x.InspectionId == bookingId && x.serviceTypeId == (int)InspectionServiceTypeEnum.Container))
                {
                    poData = poDetails.Where(x => x.ProductRefId == bookingProduct.ProductRefId && x.ContainerId == bookingProduct.ContainerId).ToList();
                }
                else
                {
                    //if booking service type is other than container then filter po details only by productid
                    poData = poDetails.Where(x => x.ProductRefId == bookingProduct.ProductRefId).ToList();
                }

                //status data
                var status = statusLogs.FirstOrDefault(x => x.BookingId == bookingId && x.StatusId == bookingData.StatusId);

                var reportSentDate = status != null ? status.StatusChangeDate : null;
                var reportSentTime = status != null ? status.StatusChangeDate?.ToShortTimeString() : string.Empty;
                var holdType = string.Empty;
                var holdReason = string.Empty;
                if (inspectionHoldReasons != null)
                {
                    var holdReasons = inspectionHoldReasons.FirstOrDefault(x => x.InspectionId == bookingId);
                    if (holdReasons != null)
                    {
                        holdType = holdReasons.Reason;
                        holdReason = holdReasons.Comment;
                    }
                }

                //add the cancel reason if available

                var cancelReasonData = inspectionCancelReasons.FirstOrDefault(x => x.InspectionId == bookingId);

                var factoryGeoLocation = factoryCountryList.FirstOrDefault(x => x.BookingId == bookingId);

                var supplierCode = supplierCodes?.Where(x => x.CustomerId == bookingData.CustomerId && x.SupplierId == bookingData.SupplierId).Select(x => x.Code).FirstOrDefault();

                var exportItem = new InspectionBookingExportItem();

                exportItem.BookingNo = bookingProduct.BookingId;
                exportItem.CustomerBookingNo = bookingData.CustomerBookingNo;
                exportItem.Customer = bookingData.Customer;
                exportItem.Supplier = bookingData.Supplier;
                exportItem.SupplierCode = supplierCode;
                exportItem.Factory = bookingData.Factory;
                exportItem.ApplyDate = bookingData.ApplyDate;
                exportItem.FirstServiceDateFrom = bookingData.FirstServiceDateFrom;
                exportItem.FirstServiceDateTo = bookingData.FirstServiceDateTo;
                exportItem.ConfirmServiceDateFrom = bookingData.ServiceDateFrom;
                exportItem.ConfirmServiceDateTo = bookingData.ServiceDateTo;
                exportItem.InspectionDateFrom = bookingProduct.ServiceStartDate;
                exportItem.InspectionDateTo = bookingProduct.ServiceEndDate;
                exportItem.ServiceType = serviceTypeList.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeName).FirstOrDefault();
                exportItem.Status = bookingData.Status;
                exportItem.DestinationCountry = string.Join(",", poData.Select(x => x.DestinationCountry).Distinct().ToArray());
                exportItem.ETDDate = string.Join(",", poData.Select(x => x.ETD?.ToString(StandardDateFormat)).Distinct().ToArray());
                exportItem.SRDate = string.Join(",", poData.Select(x => x.ETD?.AddDays(-10).ToString(StandardDateFormat)).Distinct().ToArray());
                //assign the quotation manday for the first row
                if (prevBookingId != bookingId)
                {
                    exportItem.QuotationManDay = quotationData.Select(x => x.ManDay).FirstOrDefault();
                    prevBookingId = bookingId;
                }
                exportItem.PoNumber = string.Join(",", poData.Select(x => x.PONumber).Distinct().ToArray());
                exportItem.ContainerName = bookingProduct.ContainerId > 0 ? "Container-" + bookingProduct.ContainerId : "";
                exportItem.ProductRef = bookingProduct.ProductName;
                exportItem.CombineId = bookingProduct.CombineProductId;
                exportItem.ProductRefDescription = bookingProduct.ProductDescription;
                exportItem.OrderQuantity = poData.Sum(x => x.BookingQuantity);
                exportItem.InspectedQuantity = bookingProduct.InspectedQuantity;
                exportItem.FactoryReference = bookingProduct.FactoryReference;
                exportItem.ProductCategory = bookingProduct.ProductCategory;
                exportItem.ProductSubCategory = bookingProduct.ProductSubCategory;
                exportItem.ProductSubCategory2 = bookingProduct.ProductSubCategory2;
                exportItem.ReportStatus = bookingProduct.ReportStatus;
                exportItem.ReportResult = bookingProduct.ReportResult;
                exportItem.Office = bookingData.Office;

                //get office list from cs list by customer id
                var officeList = csConfigList.Where(x => x.CustomerId == bookingData.CustomerId && x.OfficeId > 0).Select(x => x.OfficeId).Distinct().ToList();

                var brandAccessList = bookingBrandAccess.Where(y => y.BookingId == bookingId).Select(y => y.BrandId).ToList();

                var deptAccessList = bookingDeptAccess.Where(y => y.BookingId == bookingId).Select(y => y.DeptId).ToList();

                //get cs name based on customer id, booking location, brand, dept
                exportItem.CS = string.Join(",", csConfigList.Where(x => x.CustomerId == bookingData.CustomerId &&
                                (x.OfficeId == null || officeList.Contains(bookingData.OfficeId)) &&
                                (x.BrandId == null || brandAccessList.Contains(x.BrandId.GetValueOrDefault())) &&
                                (x.DepartmentId == null || deptAccessList.Contains(x.DepartmentId.GetValueOrDefault()))
                                ).Select(x => x.CsName).Distinct().ToArray());


                exportItem.QuotationStatus = quotationData.Select(x => x.QuotationStatus).FirstOrDefault();

                exportItem.Department = string.Join(",", bookingDeptAccess.Where(x => x.BookingId == bookingId).Select(x => x.DeptName).Distinct().ToArray());
                exportItem.Brand = string.Join(",", bookingBrandAccess.Where(x => x.BookingId == bookingId).Select(x => x.BrandName).Distinct().ToArray());
                exportItem.Buyer = string.Join(",", bookingBuyerAccess.Where(x => x.BookingId == bookingId).Select(x => x.BuyerName).Distinct().ToArray());
                exportItem.PriceCategory = bookingData.PriceCategory;
                exportItem.Collection = bookingData.Collection;
                exportItem.FactoryCountry = factoryGeoLocation?.CountryName;
                exportItem.FactoryProvince = factoryGeoLocation?.ProvinceName;
                exportItem.FactoryCity = factoryGeoLocation?.CityName;

                exportItem.BarCode = bookingProduct.Barcode;
                exportItem.EchoPack = bookingProduct.IsEcoPack.GetValueOrDefault(false);
                exportItem.Picking = bookingData.Picking.GetValueOrDefault(false);
                exportItem.CustomerBookingRemarks = bookingData.CustomerBookingRemarks;
                exportItem.ProductRemarks = bookingProduct.ProductRemarks;
                exportItem.NewProduct = bookingProduct.IsNewProduct != null ? bookingProduct.IsNewProduct.Value : false;
                exportItem.ReportSentDate = reportSentDate;
                exportItem.ReportSentTime = reportSentTime;
                //based on the user type assign the created by user name
                switch (bookingData.UserTypeId)
                {
                    case (int)UserTypeEnum.InternalUser:
                        exportItem.CreatedBy = entityName + "(" + bookingData.CreatedByStaff + ")";
                        break;
                    case (int)UserTypeEnum.Customer:
                        exportItem.CreatedBy = UserType.GetValueOrDefault(bookingData.UserTypeId) + bookingData.CreatedByCustomer;
                        break;
                    case (int)UserTypeEnum.Supplier:
                        exportItem.CreatedBy = UserType.GetValueOrDefault(bookingData.UserTypeId) + bookingData.CreatedBySupplier;
                        break;
                    case (int)UserTypeEnum.Factory:
                        exportItem.CreatedBy = UserType.GetValueOrDefault(bookingData.UserTypeId) + bookingData.CreatedByFactory;
                        break;
                }
                exportItem.HoldType = holdType;
                exportItem.HoldReason = holdReason;

                exportItem.CancelReasonType = cancelReasonData?.CancelType;
                exportItem.CancelReason = cancelReasonData?.CancelReason;

                exportItems.Add(exportItem);

            }

            return exportItems;
        }


        /// <summary>
        /// Map Booking Repo details for container info
        /// </summary>
        /// <param name="bookingContainerRepoList"></param>
        /// <param name="scheduleQCList"></param>
        /// <returns></returns>
        public List<BookingContainersData> MapBookingContainerList(IEnumerable<BookingContainersRepo> bookingContainerRepoList, List<SchScheduleQc> scheduleQCList)
        {
            List<BookingContainersData> bookingcontainerList = new List<BookingContainersData>();
            foreach (var bookingContainerRepo in bookingContainerRepoList)
            {
                var bookingContainersData = new BookingContainersData();
                bookingContainersData.Id = bookingContainerRepo.Id;
                bookingContainersData.ContainerId = bookingContainerRepo.ContainerId;
                bookingContainersData.BookingId = bookingContainerRepo.BookingId;
                bookingContainersData.TotalBookingQuantity = bookingContainerRepo.TotalBookingQuantity;
                bookingContainersData.BookingStatus = bookingContainerRepo.BookingStatus;
                bookingContainersData.InspectedQuantity = bookingContainerRepo.InspectedQuantity;
                bookingContainersData.InspectionDate = bookingContainerRepo.InspectionDate;
                bookingContainersData.ReportId = bookingContainerRepo.ReportId;
                bookingContainersData.ApiReportId = bookingContainerRepo.ApiReportId;
                bookingContainersData.ReportStatus = bookingContainerRepo.ReportStatus;
                bookingContainersData.ContainerName = bookingContainerRepo.ContainerName;
                bookingContainersData.ContainerSize = bookingContainerRepo.ContainerSize;
                bookingContainersData.ReportResult = bookingContainerRepo.ReportResult;
                bookingContainersData.ReportResultId = bookingContainerRepo.ReportResultId;
                bookingContainersData.ReportTitle = bookingContainerRepo.ReportTitle;
                bookingContainersData.ReportPath = bookingContainerRepo.ReportPath;
                bookingContainersData.FinalManualReportLink = bookingContainerRepo.FinalManualReportPath;
                bookingContainersData.ReportStatusId = bookingContainerRepo.ReportStatusId;
                bookingContainersData.ServiceStartDate = bookingContainerRepo.ServiceStartDate;
                bookingContainersData.CreatedDate = bookingContainerRepo.CreatedDate;
                bookingContainersData.UpdatedDate = bookingContainerRepo.UpdatedDate;
                bookingContainersData.IsPlaceHolderVisible = bookingContainerRepo.IsPlaceHolderVisible;
                bookingContainersData.FillingStatus = bookingContainerRepo.FillingStatus;
                bookingContainersData.ReviewStatus = bookingContainerRepo.ReviewStatus;
                bookingContainersData.ReportResultColor = bookingContainerRepo.ReportResultId.HasValue ? ReportResultColor.TryGetValue
                                                                       (bookingContainerRepo.ReportResultId.Value, out string reportResultColor) ? reportResultColor : "" : "";
                bookingContainersData.ReportStatusColor = bookingContainerRepo.ReportStatusId.HasValue ? ReportStatusColor.TryGetValue
                                                                       (bookingContainerRepo.ReportStatusId.Value, out string reportColor) ? reportColor : "" : "";
                bookingContainersData.ReviewResultColor = bookingContainerRepo.ReviewResultId.HasValue ? ReportStatusColor.TryGetValue
                                                                       (bookingContainerRepo.ReviewResultId.Value, out string reviewColor) ? reviewColor : "" : "";
                bookingContainersData.FillingStatusColor = bookingContainerRepo.FillingStatusId.HasValue ? ReportStatusColor.TryGetValue
                                                                    (bookingContainerRepo.FillingStatusId.Value, out string filllingColor) ? filllingColor : "" : "";
                bookingContainersData.QCName = scheduleQCList != null ? string.Join(",", scheduleQCList.Select(y => y.Qc?.PersonName).ToArray().Distinct()) : "";
                bookingContainersData.FinalReportLink = bookingContainerRepo.FinalReportLink;
                bookingcontainerList.Add(bookingContainersData);

            }

            return bookingcontainerList;
        }

        /// <summary>
        /// Map the product transaction entity 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="productTotalBookingQty"></param>
        /// <returns></returns>
        public InspProductTransaction MapAddProductTransactionEntity(SaveInspectionPOProductDetails request, int productTotalBookingQty)
        {
            var productTransaction = new InspProductTransaction()
            {
                Id = request.Id,
                ProductId = request.ProductId,
                TotalBookingQuantity = productTotalBookingQty,
                CombineProductId = request.CombineProductId,
                CombineAqlQuantity = request.CombineAqlQuantity,
                Aql = request.Aql,
                Major = request.Major,
                Minor = request.Minor,
                Critical = request.Critical,
                Unit = request.Unit,
                UnitCount = request.UnitCount,
                Active = true,
                CreatedOn = DateTime.Now,
                AqlQuantity = request.AqlQuantity,
                SampleType = request.SampleType,
                Remarks = request.Remarks,
                IsEcopack = request.IsEcopack,
                IsDisplayMaster = request.IsDisplayMaster,
                ParentProductId = request.ParentProductId,
                FbtemplateId = request.FbTemplateId,
                AsReceivedDate = request.AsReceivedDate?.ToNullableDateTime(),
                TfReceivedDate = request.TfReceivedDate?.ToNullableDateTime(),
                IsGoldenSampleAvailable = request.IsGoldenSampleAvailable,
                GoldenSampleComments = request.GoldenSampleComments,
                IsSampleCollection = request.IsSampleCollection,
                SampleCollectionComments = request.SampleCollectionComments,
                ProductionStatus = request.ProductionStatus,
                PackingStatus = request.PackingStatus
            };

            return productTransaction;
        }

        /// <summary>
        /// Map the purchase order transaction entity
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public InspPurchaseOrderTransaction MapProductPOTransaction(SaveInspectionPOProductDetails request)
        {
            var productPOTransaction = new InspPurchaseOrderTransaction()
            {
                Id = request.Id,
                PoId = request.PoId,
                BookingQuantity = request.BookingQuantity,
                Active = true,
                PickingQuantity = request.PickingQuantity,
                CreatedOn = DateTime.Now,
                Etd = request.Etd?.ToDateTime(),
                DestinationCountryId = request.DestinationCountryID,
                CustomerReferencePo = request.CustomerReferencePo,
            };

            return productPOTransaction;
        }

        /// <summary>
        /// Map the po transaction with the color transaction booking qty
        /// </summary>
        /// <param name="request"></param>
        /// <param name="poColorList"></param>
        /// <returns></returns>
        public InspPurchaseOrderTransaction MapProductPOForColorTransaction(SaveInspectionPOProductDetails request, List<SaveInspectionPOProductDetails> poColorList)
        {
            //get the purchase order total booking qty
            var poTotalBookingQuantity = poColorList.Where(x => x.PoId == request.PoId).Sum(x => x.BookingQuantity);

            var poTotalPickingQuantity = poColorList.Where(x => x.PoId == request.PoId).Sum(x => x.PickingQuantity);

            var productPOTransaction = new InspPurchaseOrderTransaction()
            {
                Id = request.Id,
                PoId = request.PoId,
                BookingQuantity = poTotalBookingQuantity,
                Active = true,
                PickingQuantity = poTotalPickingQuantity,
                CreatedOn = DateTime.Now,
                Etd = request.Etd?.ToDateTime(),
                DestinationCountryId = request.DestinationCountryID,
                CustomerReferencePo = request.CustomerReferencePo,
            };

            return productPOTransaction;
        }

        /// <summary>
        /// Map the purchase order color transaction
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public InspPurchaseOrderColorTransaction MapPOColorTransaction(SaveInspectionPOProductDetails request)
        {
            var poColorTransaction = new InspPurchaseOrderColorTransaction()
            {
                Id = request.Id,
                ColorCode = request.ColorCode,
                ColorName = request.ColorName,
                BookingQuantity = request.BookingQuantity,
                PickingQuantity = request.PickingQuantity,
                Active = true,
                CreatedOn = DateTime.Now,
            };

            return poColorTransaction;
        }

        /// <summary>
        /// Map container entity
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalBookingQuantity"></param>
        /// <returns></returns>
        public InspContainerTransaction MapContainerTransaction(int containerId, int totalBookingQuantity)
        {
            var productPOTransaction = new InspContainerTransaction()
            {
                TotalBookingQuantity = totalBookingQuantity,
                Active = true,
                ContainerId = containerId,
                CreatedOn = DateTime.Now
            };

            return productPOTransaction;
        }

        /// <summary>
        /// Map the purchase order transaction on edit
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entityPODetail"></param>
        /// <param name="isPickingRequired"></param>
        /// <returns></returns>
        public InspPurchaseOrderTransaction MapProductPOTransactionOnEdit(SaveInsepectionRequest request, SaveInspectionPOProductDetails poRequestData, InspPurchaseOrderTransaction entityPODetail,
                    List<SaveInspectionPOProductDetails> productPOList)
        {
            if (request.BusinessLine == (int)BusinessLine.SoftLine)
            {
                var totalPoBookingQty = productPOList.Where(x => x.PoId == entityPODetail.PoId).Sum(x => x.BookingQuantity);

                entityPODetail.BookingQuantity = totalPoBookingQty;

                if (request.IsPickingRequired.HasValue && request.IsPickingRequired.Value)
                {
                    var totalPoPickingQty = productPOList.Where(x => x.PoId == entityPODetail.PoId).Sum(x => x.PickingQuantity);
                    entityPODetail.PickingQuantity = totalPoPickingQty;
                }
            }
            else
            {
                entityPODetail.BookingQuantity = poRequestData.BookingQuantity;
                entityPODetail.PickingQuantity = poRequestData.PickingQuantity;
            }

            entityPODetail.PoId = poRequestData.PoId;
            entityPODetail.DestinationCountryId = poRequestData.DestinationCountryID;
            entityPODetail.Etd = poRequestData.Etd?.ToDateTime();
            entityPODetail.CustomerReferencePo = poRequestData.CustomerReferencePo;
            entityPODetail.UpdatedOn = DateTime.Now;
            return entityPODetail;
        }

        public InspProductTransaction MapProductTransactionOnEditBooking(SaveInspectionPOProductDetails requestProductData, InspProductTransaction entityProductData)
        {
            entityProductData.Aql = requestProductData.Aql;
            entityProductData.Critical = requestProductData.Critical;
            entityProductData.Major = requestProductData.Major;
            entityProductData.Minor = requestProductData.Minor;
            entityProductData.UnitCount = requestProductData.UnitCount;
            entityProductData.Unit = requestProductData.Unit;
            entityProductData.Remarks = requestProductData.Remarks;
            entityProductData.UpdatedOn = DateTime.Now;
            entityProductData.IsEcopack = requestProductData.IsEcopack;
            entityProductData.IsDisplayMaster = requestProductData.IsDisplayMaster;
            entityProductData.ParentProductId = requestProductData.ParentProductId;
            entityProductData.FbtemplateId = requestProductData.FbTemplateId;
            entityProductData.AsReceivedDate = requestProductData.AsReceivedDate?.ToNullableDateTime();
            entityProductData.TfReceivedDate = requestProductData.TfReceivedDate?.ToNullableDateTime();

            return entityProductData;
        }

        /// <summary>
        /// Map the inspection draft for the save method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public InspTransactionDraft MapSaveInspectionDraftBooking(DraftInspectionRequest request)
        {
            InspTransactionDraft inspTransactionDraft = new InspTransactionDraft();
            inspTransactionDraft.CustomerId = request.CustomerId;
            inspTransactionDraft.SupplierId = request.SupplierId;
            inspTransactionDraft.FactoryId = request.FactoryId;
            if (request.ServiceDateFrom != null)
                inspTransactionDraft.ServiceDateFrom = request.ServiceDateFrom.ToNullableDateTime();
            if (request.ServiceDateTo != null)
                inspTransactionDraft.ServiceDateTo = request.ServiceDateTo.ToNullableDateTime();
            inspTransactionDraft.BrandId = request.BrandId;
            inspTransactionDraft.DepartmentId = request.DepartmentId;

            inspTransactionDraft.BookingInfo = request.BookingInfo;

            inspTransactionDraft.IsReInspectionDraft = request.IsReInspectionDraft;
            inspTransactionDraft.IsReBookingDraft = request.IsReBookingDraft;
            inspTransactionDraft.PreviousBookingNo = request.PreviousBookingNo;



            return inspTransactionDraft;
        }

        /// <summary>
        /// Map the update inspection draft booking
        /// </summary>
        /// <param name="request"></param>
        /// <param name="inspTransactionDraft"></param>
        /// <returns></returns>
        public InspTransactionDraft MapUpdateInspectionDraftBooking(DraftInspectionRequest request, InspTransactionDraft inspTransactionDraft)
        {
            inspTransactionDraft.CustomerId = request.CustomerId;
            inspTransactionDraft.SupplierId = request.SupplierId;
            inspTransactionDraft.FactoryId = request.FactoryId;
            if (request.ServiceDateFrom != null)
                inspTransactionDraft.ServiceDateFrom = request.ServiceDateFrom.ToNullableDateTime();
            if (request.ServiceDateTo != null)
                inspTransactionDraft.ServiceDateTo = request.ServiceDateTo.ToNullableDateTime();
            inspTransactionDraft.BrandId = request.BrandId;
            inspTransactionDraft.DepartmentId = request.DepartmentId;
            inspTransactionDraft.BookingInfo = request.BookingInfo;
            inspTransactionDraft.IsReInspectionDraft = request.IsReInspectionDraft;
            inspTransactionDraft.IsReBookingDraft = request.IsReBookingDraft;
            inspTransactionDraft.PreviousBookingNo = request.PreviousBookingNo;

            return inspTransactionDraft;
        }

        /// <summary>
        /// Map the inspection draft data list
        /// </summary>
        /// <param name="inspectionDraftDataList"></param>
        /// <returns></returns>
        public List<DraftInspection> MapInspectionDraftList(List<DraftInspectionRepo> inspectionDraftDataList)
        {
            List<DraftInspection> draftInspectionList = new List<DraftInspection>();
            bool IsReinspectionDraft = false;
            bool IsReBookingDraft = false;



            foreach (var draftData in inspectionDraftDataList)
            {
                var draftInspection = new DraftInspection();
                draftInspection.Id = draftData.Id;
                draftInspection.Customer = draftData.Customer;
                draftInspection.Supplier = draftData.Supplier;
                draftInspection.Factory = draftData.Factory;
                draftInspection.Brand = draftData.Brand;
                draftInspection.Department = draftData.Department;
                draftInspection.BookingInfo = draftData.BookingInfo;

                //assign the reinspection draft
                draftInspection.IsReInspectionDraft = IsReinspectionDraft;
                if (draftData.IsReInspectionDraft.HasValue && draftData.IsReInspectionDraft.Value)
                    draftInspection.IsReInspectionDraft = !IsReinspectionDraft;

                //assign the rebooking draft
                draftInspection.IsReBookingDraft = IsReinspectionDraft;
                if (draftData.IsReBookingDraft.HasValue && draftData.IsReBookingDraft.Value)
                    draftInspection.IsReBookingDraft = !IsReinspectionDraft;
                draftInspection.PreviousBookingNo = draftData.PreviousBookingNo;

                if (draftData.ServiceDateFrom != null && draftData.ServiceDateTo != null)
                {
                    if (draftData.ServiceDateFrom == draftData.ServiceDateTo)
                        draftInspection.ServiceDate = draftData.ServiceDateFrom.GetValueOrDefault().ToString(StandardDateFormat);
                    else
                        draftInspection.ServiceDate = draftData.ServiceDateFrom.GetValueOrDefault().ToString(StandardDateFormat) + "-" +
                                                      draftData.ServiceDateTo.GetValueOrDefault().ToString(StandardDateFormat);
                }

                draftInspection.CreatedOn = draftData.CreatedOn.GetValueOrDefault().ToString(StandardDateTimeFormat);

                //if(draftData.is)

                draftInspectionList.Add(draftInspection);
            }

            return draftInspectionList;
        }

        /// <summary>
        /// Map the inspection picking data
        /// </summary>
        /// <param name="pickingRequestData"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public InspTranPicking MapInspectionPicking(SaveInspectionPickingDetails pickingRequestData, int userId)
        {
            var pickingTransactionData = new InspTranPicking();
            //Determine whether customer picks the product or lab picks the product
            //if lab picks the product then assign the lab information
            if (pickingRequestData.LabType == (int)LabTypeEnum.Lab)
            {
                pickingTransactionData.LabId = pickingRequestData.LabOrCustomerId;
                pickingTransactionData.LabAddressId = pickingRequestData.LabOrCustomerAddressId;
            }
            //if customer picks the product then assign the customer information
            else if (pickingRequestData.LabType == (int)LabTypeEnum.Customer)
            {
                pickingTransactionData.CustomerId = pickingRequestData.LabOrCustomerId;
                pickingTransactionData.CusAddressId = pickingRequestData.LabOrCustomerAddressId;
            }
            //assing the picking Data
            pickingTransactionData.PickingQty = pickingRequestData.PickingQuantity;
            pickingTransactionData.Remarks = pickingRequestData.Remarks;
            pickingTransactionData.CreatedBy = userId;
            pickingTransactionData.CreationDate = DateTime.Now;
            pickingTransactionData.Active = true;

            return pickingTransactionData;
        }

        /// <summary>
        /// Map the inspection picking contacts
        /// </summary>
        /// <param name="labType"></param>
        /// <param name="contactId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public InspTranPickingContact MapInspectionPickingContact(int? labType, int contactId, int userId)
        {
            var inspTranPickingContact = new InspTranPickingContact();
            //if lab picks the product then assign the lab then assign to lab contact id
            if (labType == (int)LabTypeEnum.Lab)
                inspTranPickingContact.LabContactId = contactId;
            //if customer picks the product then assign the lab then assign to customer contact id
            if (labType == (int)LabTypeEnum.Customer)
                inspTranPickingContact.CusContactId = contactId;
            inspTranPickingContact.CreatedBy = userId;
            inspTranPickingContact.CreatedOn = DateTime.Now;
            inspTranPickingContact.Active = true;
            return inspTranPickingContact;
        }

        /// <summary>
        /// Map the inspection picking data for the update
        /// </summary>
        /// <param name="pickingData"></param>
        /// <param name="entityPickingDetail"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public InspTranPicking MapUpdateInspectionPicking(SaveInspectionPickingDetails pickingData, InspTranPicking entityPickingDetail, int userId)
        {
            //assign the lab data if lab picks the product
            if (pickingData.LabType == (int)LabTypeEnum.Lab)
            {
                entityPickingDetail.LabId = pickingData.LabOrCustomerId;
                entityPickingDetail.LabAddressId = pickingData.LabOrCustomerAddressId;

                foreach (var contactData in pickingData.PickingContactList)
                {
                    //add the lab contacts if not available in the db
                    if (!entityPickingDetail.InspTranPickingContacts.Any(x => x.Active && x.LabContactId == contactData.LabOrCustomerContactId))
                    {
                        InspTranPickingContact pickingContact = MapInspectionPickingContact(pickingData.LabType, contactData.LabOrCustomerContactId, userId);
                        entityPickingDetail.InspTranPickingContacts.Add(pickingContact);
                    }
                }
            }
            //assign the customer data if customer picks the product
            else if (pickingData.LabType == (int)LabTypeEnum.Customer)
            {
                entityPickingDetail.CustomerId = pickingData.LabOrCustomerId;
                entityPickingDetail.CusAddressId = pickingData.LabOrCustomerAddressId;

                //add the customer contacts if not available in the db
                foreach (var contactData in pickingData.PickingContactList)
                {
                    if (!entityPickingDetail.InspTranPickingContacts.Any(x => x.Active && x.CusContactId == contactData.LabOrCustomerContactId))
                    {
                        InspTranPickingContact pickingContact = MapInspectionPickingContact(pickingData.LabType, contactData.LabOrCustomerContactId, userId);
                        entityPickingDetail.InspTranPickingContacts.Add(pickingContact);
                    }
                }
            }

            entityPickingDetail.PickingQty = pickingData.PickingQuantity;
            entityPickingDetail.Remarks = pickingData.Remarks;
            entityPickingDetail.UpdatedBy = userId;
            entityPickingDetail.UpdationDate = DateTime.Now;

            return entityPickingDetail;
        }

    }
}
