using System;
using System.Collections.Generic;
using System.Linq;
using DTO.Quotation;
using DTO.Common;
using DTO.File;
using DTO.Inspection;
using DTO.Report;
using DTO.Schedule;
using Entities;
using Entities.Enums;
using DTO.HumanResource;
using DTO.User;
using DTO.Customer;
using System.Globalization;
using DTO.CommonClass;

namespace BI.Maps
{
    public class ScheduleMap : ApiCommonData
    {
        public ScheduleBookingItem GetInspectionSearchResult(ScheduleBookingInfo entity,
            IEnumerable<InspectionStatus> inspectionStatus
            , IEnumerable<ServiceTypeList> serviceTypeList,
            IEnumerable<FactoryCountry> factoryCountryData,
            List<ScheduleStaffItem> QClist, List<ScheduleStaffItem> CSlist,
            IEnumerable<PoDetails> quotDetails,
            List<CuCheckPoint> cuCheckPoints, List<ScheduleQuotationManDay> quotationMandayByDate
            , IEnumerable<ScheduleProductsData> prodTran,
            IEnumerable<ScheduleContainersRepo> bookContainer,
            List<BookingBrandAccess> brandData,
            List<BookingDeptAccess> deptData,
            List<CommonCheckPointServiceTypeDataSource> serviceTypeCheckpointData,
            IEnumerable<BookingProductCategory> bookingProductCategories,
            IEnumerable<InspectionPOExportData> inspectionPurchaseOrders,
            IEnumerable<InspectionPOColorTransaction> inspectionPOColorTransactions,
            List<FBReportDetails> reportDetails,
            List<CSNameRepo> csNameList, List<InspectionSupplierFactoryContacts> factoryContacts)
        {
            double suggestedManday = 0;
            double estimateManday = 0;

            double calculatedWorkingHours = 0;
            var reportTitleList = new List<ReportDetails>();

            var quotationData = quotDetails.FirstOrDefault(x => x.BookingId == entity.BookingId && x.QuotationStatus != (int)QuotationStatus.Canceled);

            if (quotationData != null)
            {
                estimateManday = quotationData.Manday.GetValueOrDefault();
                suggestedManday = quotationData.SuggestedManday.GetValueOrDefault();
                calculatedWorkingHours = quotationData.CalculatedWorkingHours.GetValueOrDefault();
            }

            var factoryLocation = factoryCountryData.FirstOrDefault(x => x.BookingId == entity.BookingId);

            var factoryContactList = factoryContacts?.Where(x => x.InspectionId == entity.BookingId).Select(x => x.ContactName + " (" + x.Phone + ")").ToList();
            var factoryContact = factoryContactList.Count > 0 ? string.Join(", ", factoryContactList) : null;

            var showAddButton = false;

            if (entity.StatusId == (int)BookingStatus.Confirmed)
            {
                var checkPointData = cuCheckPoints.FirstOrDefault(x => x.CustomerId == entity.CustomerId && x.CheckpointTypeId == (int)CheckPointTypeEnum.QuotationRequired);
                if (checkPointData != null)
                {
                    bool brandCheckpoint = true;
                    bool deptCheckpoint = true;
                    bool serviceTypeCheckpoint = true;
                    bool countryCheckpoint = true;

                    if (checkPointData.CuCheckPointsBrands != null && checkPointData.CuCheckPointsBrands.Any())
                    {
                        var brandDetails = brandData.Where(x => x.BookingId == entity.BookingId).ToList();
                        brandCheckpoint = brandDetails.Any(x => checkPointData.CuCheckPointsBrands.Any(y => y.Active && y.BrandId == x.BrandId));
                    }

                    if (checkPointData.CuCheckPointsDepartments != null && checkPointData.CuCheckPointsDepartments.Any())
                    {
                        var deptDetails = deptData.Where(x => x.BookingId == entity.BookingId).ToList();
                        deptCheckpoint = deptDetails.Any(x => checkPointData.CuCheckPointsDepartments.Any(y => y.Active && y.DeptId == x.DeptId));
                    }

                    if (checkPointData.CuCheckPointsCountries != null && checkPointData.CuCheckPointsCountries.Any())
                    {
                        var factoryCountryDetails = factoryCountryData.Where(x => x.BookingId == entity.BookingId).ToList();
                        countryCheckpoint = factoryCountryDetails.Any(x => checkPointData.CuCheckPointsCountries.Any(y => y.Active && y.CountryId == x.FactoryCountryId));
                    }

                    if (serviceTypeCheckpointData != null && serviceTypeCheckpointData.Any())
                    {
                        var serviceTypeDetails = serviceTypeList.Where(x => x.InspectionId == entity.BookingId).ToList();
                        serviceTypeCheckpoint = serviceTypeDetails.Any(x => serviceTypeCheckpointData.Any(y => y.ServiceTypeId == x.serviceTypeId));
                    }

                    if (brandCheckpoint && deptCheckpoint && serviceTypeCheckpoint && countryCheckpoint)
                    {
                        if (quotDetails.Where(x => x.BookingId == entity.BookingId).OrderByDescending(x => x.quotCreatedDate).Select(x => x.QuotationStatus).FirstOrDefault() == (int)QuotationStatus.CustomerValidated)
                        {
                            showAddButton = true;
                        }
                    }
                    else
                    {
                        showAddButton = true;
                    }
                }

                else
                {
                    showAddButton = true;
                }
            }

            //get the booking products data
            var bookingProductCategory = bookingProductCategories.Where(x => x.BookingId == entity.BookingId);

            var productDetails = prodTran.Where(x => x.BookingId == entity.BookingId).FirstOrDefault();

            var csNames = string.Join(", ", csNameList.Where(x => x.BookingId == entity.BookingId).Select(x => x.Name).ToList());

            //if it is container booking return true
            var _isContainerBooking = bookContainer.Where(x => x.BookingId == entity.BookingId).Select(x => x.ContainerId > 0).Any();

            //get container list
            var containerListByBooking = bookContainer.Where(x => x.BookingId == entity.BookingId).ToList();

            //is contanier booking
            if (_isContainerBooking && containerListByBooking.Any())
            {
                foreach (var _container in containerListByBooking)
                {
                    //get product id list
                    var productList = inspectionPurchaseOrders.Where(x => x.ContainerId == _container.ContainerId).Select(x => x.ProductRefId).ToList();

                    foreach (var productRef in productList)
                    {
                        //get product id
                        var productId = prodTran.Where(x => x.BookingId == entity.BookingId && x.ProductRefId == productRef).Select(x => x.ProductId).FirstOrDefault();
                        var reportTitle = reportDetails.Where(x => x.FbReportId == _container.ReportId).Select(x => x.ReportTitle).FirstOrDefault();

                        //map the report info to list
                        reportTitleList.Add(MapReportInfo(_container.ReportId, reportTitle, productId));
                    }
                }
            }
            else
            {
                //get product list
                var productList = prodTran.Where(x => x.BookingId == entity.BookingId);

                foreach (var prod in productList)
                {
                    var reportTitle = reportDetails.Where(x => x.FbReportId == prod.ReportId).Select(x => x.ReportTitle).FirstOrDefault();

                    //map the report info to list
                    reportTitleList.Add(MapReportInfo(prod.ReportId, reportTitle, prod.ProductId));
                }
            }

            return new ScheduleBookingItem()
            {
                BookingId = entity.BookingId,
                CustomerName = entity?.CustomerName,
                FactoryName = entity.FactoryName + " " + entity.FactoryRegionalName,
                ServiceDateFrom = entity?.ServiceDateFrom,
                ServiceDateTo = entity?.ServiceDateTo.ToString(StandardDateFormat),
                ServiceDate = entity?.ServiceDateFrom == entity?.ServiceDateTo ? entity?.ServiceDateFrom.ToString(StandardDateFormat) : string.Join(" - ", entity?.ServiceDateFrom.ToString(StandardDateFormat), entity?.ServiceDateTo.ToString(StandardDateFormat)),
                SupplierName = entity.SupplierName,
                ServiceType = serviceTypeList.Where(x => x.InspectionId == entity.BookingId).Select(x => x.serviceTypeName).FirstOrDefault(),
                Office = entity.Office,
                StatusName = inspectionStatus?.Where(x => x.Id == entity.StatusId)?.Select(x => x.StatusName)?.FirstOrDefault(),
                StatusId = entity?.StatusId,
                ProvinceName = factoryLocation?.ProvinceName,
                CityName = factoryLocation?.CityName,
                CountyName = factoryLocation?.CountyName,
                ZoneName = factoryLocation?.ZoneName,
                TownName = factoryLocation?.TownName,
                FactoryAddress = (factoryLocation?.FactoryCountryId == (int)ZohoCountryEnum.China) ? factoryLocation?.FactoryAdress + "(" + factoryLocation?.FactoryRegionalAddress + ")" : factoryLocation?.FactoryAdress,
                FactoryContact = factoryContact,
                ServiceDateQCNames = QClist.Where(x => x.BookingId == entity.BookingId && x.QCType == (int)QCType.QC). //&& x.Active).
                       GroupBy(x => x.ServiceDate).Select(g => new ServiceDateQCNames
                       {
                           ServiceDate = g.Key.ToString(StandardDateFormat),
                           QCInfo = g.Select(c => new QCInfo
                           {
                               QCName = c.Name,
                               ActualManDay = c.ActualManDay,
                               TotalBooking = QClist.Where(y => y.Id == c.Id && y.ServiceDate == g.Key).Select(x => x.BookingId).Count()
                           }).ToList()
                       }).ToList(),
                ServiceDateAdditionalQCNames = QClist.Where(x => x.BookingId == entity.BookingId && x.QCType == (int)QCType.AdditionalQC). //&& x.Active).
                    GroupBy(x => x.ServiceDate).Select(g => new ServiceDateQCNames
                    {
                        ServiceDate = g.Key.ToString(StandardDateFormat),
                        QCInfo = g.Select(c => new QCInfo
                        {
                            QCName = c.Name
                        }).ToList()
                    }).ToList(),
                ServiceDateCSNames = CSlist.Where(x => x.BookingId == entity.BookingId). // && x.Active).
                    GroupBy(x => x.ServiceDate).Select(g => new ServiceDateCSNames
                    {
                        ServiceDate = g.Key.ToString(StandardDateFormat),
                        CSInfo = g.Select(c => new CSInfo
                        {
                            CSName = c.Name
                        }).ToList()
                    }).ToList(),
                ManDay = estimateManday,
                CalculatedWorkingHours = calculatedWorkingHours,
                ActualManDay = QClist.Where(x => x.BookingId == entity.BookingId && x.QCType == (int)QCType.QC).Sum(x => x.ActualManDay),
                IsMandayButtonVisible = ((entity.StatusId == (int)BookingStatus.Confirmed || entity.StatusId == (int)BookingStatus.AllocateQC) && quotDetails?.Where(x => x.BookingId == entity.BookingId).Count() > 0),
                FirstServiceDate = entity?.FirstServiceDateFrom == entity?.FirstServiceDateTo ? entity?.FirstServiceDateFrom?.ToString(StandardDateFormat) : string.Join(" - ", entity?.FirstServiceDateFrom?.ToString(StandardDateFormat), entity?.FirstServiceDateTo?.ToString(StandardDateFormat)),
                SampleSize = prodTran.Where(x => x.BookingId == entity.BookingId && x.CombineProductId > 0)
                            .Select(x => x.CombineAqlQuantity.GetValueOrDefault()).Sum() + prodTran.
                            Where(x => x.BookingId == entity.BookingId && !(x.CombineProductId > 0)).
                            Select(x => x.AqlQuantity.GetValueOrDefault()).Sum(),
                ReportSampleSize = prodTran.Where(x => x.BookingId == entity.BookingId).Select(x => x.InspectedQty.GetValueOrDefault()).Sum(),
                QuotationStatus = quotDetails.Where(x => x.BookingId == entity.BookingId).OrderByDescending(x => x.quotCreatedDate).Select(x => x.QuotationStatusName).FirstOrDefault(),
                Productcount = prodTran.Where(x => x.BookingId == entity.BookingId).
                                Select(x => x.ProductId).Distinct().Count(),
                ReportCount = (serviceTypeList.Where(x => x.InspectionId == entity.BookingId).Select(x => x.serviceTypeId).FirstOrDefault() == (int)InspectionServiceTypeEnum.Container) ?
                                bookContainer.Where(x => x.ContainerId > 0 && x.BookingId == entity.BookingId).Select(x => x.ContainerId).Distinct().Count() :
                                prodTran.Count(x => x.CombineProductId == 0 && x.BookingId == entity.BookingId) + prodTran.Where(x => x.CombineProductId != 0 && x.BookingId == entity.BookingId).Select(x => x.CombineProductId).Distinct().Count(),
                ShowAddButton = showAddButton,
                PlannedManday = quotationMandayByDate.Any() ? quotationMandayByDate.Where(x => x.BookingId == entity.BookingId).Sum(x => x.ManDay) : quotDetails.Where(x => x.BookingId == entity.BookingId).OrderByDescending(x => x.quotCreatedDate).Select(x => x.Manday.GetValueOrDefault()).FirstOrDefault(),
                HasQcVisible = QClist.Where(x => x.BookingId == entity.BookingId && x.IsQcVisibility == false).Count() > 0 ? false : true,
                QcVisibleToEmail = QClist.Where(x => x.BookingId == entity.BookingId && x.IsQcVisibility == true).Count() > 0 ? true : false,
                quotationMandayListByDate = quotationMandayByDate.Where(x => x.BookingId == entity.BookingId).ToList(),
                ProductCategory = string.Join(", ", bookingProductCategory.Select(x => x.ProductCategoryName).Distinct()),
                ProductSubCategory = string.Join(", ", bookingProductCategory.Select(x => x.ProductCategorySubName).Distinct()),
                ProductSubCategory2 = bookingProductCategory.Select(x => x.ProductCategorySub2Name).Distinct().ToList(),
                CuProductId = productDetails.ProductId,
                ProductId = productDetails.ProductName,
                IsMSChartProduct = prodTran.Where(x => x.BookingId == entity.BookingId).All(y => y.IsMSChart),
                POColorTransactions = inspectionPOColorTransactions.Where(x => x.BookingId == entity.BookingId).ToList(),
                InspectionProducts = prodTran.Where(x => x.BookingId == entity.BookingId).ToList(),
                Season = !string.IsNullOrEmpty(entity.Season) ? entity.Season + (entity.Year.HasValue ? "-" + entity.Year : "") : "",
                Brands = string.Join(", ", brandData.Where(x => x.BookingId == entity.BookingId).Select(y => y.BrandName).Distinct().ToList()),
                //PONumber = string.Join(", ", inspectionPurchaseOrders.Where(x => x.InspectionId == entity.BookingId).Select(y => y.PONumber).Distinct().ToList()),
                CSNames = csNames,
                ReportTitleList = reportTitleList,
                FactoryCountry = factoryLocation?.CountryName,
                InspectionPOs = inspectionPurchaseOrders.Where(x => x.InspectionId == entity.BookingId).ToList(),
                CreateDate = entity.CreateDate,
                SuggestedManday = suggestedManday,
                IsEAQF = entity.IsEAQF
            };
        }

        /// <summary>
        /// map the report details
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="reportTitle"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ReportDetails MapReportInfo(int? reportId, string reportTitle, int productId)
        {
            var reportInfo = new ReportDetails
            {
                ReportMapId = reportId,
                ReportTitle = reportTitle,
                ProductId = productId

            };
            return reportInfo;
        }

        public StaffSchedule MapStaff(QCStaffInfo user)
        {
            FileResponse image = null;
            if (user.StaffImage != null)
            {
                image = new FileResponse
                {
                    Content = user.StaffImage.Photo,
                    MimeType = user.StaffImage.PhotoMType
                };
            }

            string bookingAssigned = string.Empty;
            List<int> bookingIds = user.ScheduleQC?.Select(x => x.BookingId).ToList();

            if (bookingIds != null && bookingIds.Count > 0)
            {
                bookingAssigned = "( #" + string.Join(", #", bookingIds) + ")";
            }

            return new StaffSchedule
            {
                StaffID = user.Id,
                StaffName = user.Name,
                EmailAddress = user.EmailAddress,
                EmergencyCall = user.EmergencyCall,
                Location = user.Location,
                LocationId = user.LocationId,
                ZoneId = user.ZoneId,
                BookingsAssigned = bookingAssigned,
                EmployeeType = user.EmployeeType,
                AssignedBookingCount = bookingIds != null ? bookingIds.Count : 0,
                StaffImage = image,
                StartPortId = user.StartPortId,
                StartPortName = user.StartPortName
            };
        }
        public ScheduleCombineProduct MapBookingProductCombine(InspPoCombineTransaction inspPo)
        {
            if (inspPo == null)
                return null;
            return new ScheduleCombineProduct
            {
                ProductId = inspPo?.Product?.ProductId,
                ProductDesc = inspPo?.Product?.ProductDescription,
                CombineProductId = inspPo?.CombineProductId
            };
        }
        public StaffSchedule MapStaffProduct(HrStaff entity)
        {
            if (entity == null)
                return null;
            return new StaffSchedule
            {
                StaffID = entity.Id,
                StaffName = entity.PersonName
            };
        }

        public List<ScheduleBookingItemExportSummarynew> MapExportSummary(IEnumerable<ScheduleBookingItem> items)
        {
            var resultDataList = new List<ScheduleBookingItemExportSummarynew>();

            foreach (var item in items)
            {
                int count = 1;
                DateTime.TryParseExact(item.ServiceDateTo, StandardDateFormat, new CultureInfo("en-us"), DateTimeStyles.None, out DateTime itemServiceDateTo);
                if (item.ServiceDateQCNames.Any())
                {
                    foreach (var row in item.ServiceDateQCNames)
                    {
                        DateTime rowServiceDate;
                        double plannedManday = 0;
                        int planCount = 0;
                        if (DateTime.TryParseExact(row.ServiceDate, StandardDateFormat, new CultureInfo("en-us"), DateTimeStyles.None, out rowServiceDate))
                        {
                            plannedManday = item.quotationMandayListByDate.Where(x => x.ServiceDate == rowServiceDate).Sum(x => x.ManDay);
                            planCount = 1;
                        }
                        foreach (var data in row.QCInfo)
                        {
                            var objScheduleInfo = new ScheduleBookingItemExportSummarynew()
                            {
                                BookingId = item.BookingId,
                                CustomerName = item.CustomerName,
                                SupplierName = item.SupplierName,
                                FactoryName = item.FactoryName,
                                FactoryAddress = item.FactoryAddress,
                                ProvinceName = item.ProvinceName,
                                CityName = item.CityName,
                                //ReportCount = item.ReportCount,
                                //ProductCount = item.Productcount,
                                QuotationStatus = item.QuotationStatus,
                                FirstServiceDate = item.FirstServiceDate,
                                SampleSize = item.SampleSize,
                                ServiceType = item.ServiceType,
                                ServiceFromDate = item.ServiceDateFrom,
                                ServiceToDate = itemServiceDateTo,
                                ScheduleDate = rowServiceDate,
                                CountyName = item.CountyName,
                                TownName = item.TownName,
                                Zone = item.ZoneName,
                                QCName = data.QCName,
                                ActualManday = data.ActualManDay,
                                ScheduleManday = data.TotalBooking > 0 ? Math.Round(NumberOne / data.TotalBooking, NumberTwo) : 0,
                                AdditionalQCName = string.Join(",", item.ServiceDateAdditionalQCNames.
                                Where(x => x.ServiceDate == row.ServiceDate).SelectMany(x => x.QCInfo.Select(y => y.QCName)).ToArray()),
                                CSName = string.Join(",", item.ServiceDateCSNames.
                                Where(x => x.ServiceDate == row.ServiceDate).SelectMany(x => x.CSInfo.Select(y => y.CSName)).ToArray()),
                                QCVisibility = item.HasQcVisible ? "True" : "False",
                                Office = item.Office,
                                CreateDate = item.CreateDate
                            };
                            if (item.StatusId == (int)BookingStatus.Inspected || item.StatusId == (int)BookingStatus.ReportSent)
                                objScheduleInfo.ReportSampleSize = item.ReportSampleSize;

                            if (planCount == 1)
                            {
                                objScheduleInfo.PlannedManday = plannedManday;
                                planCount++;
                            }
                            if (count == 1)
                            {
                                // objScheduleInfo.ActualManday = item.ActualManDay;
                                objScheduleInfo.QuotationManday = item.ManDay.GetValueOrDefault();
                                objScheduleInfo.ReportCount = item.ReportCount;
                                objScheduleInfo.ProductCount = item.Productcount;
                                objScheduleInfo.CalculatedTotalWorkingHours = item.CalculatedWorkingHours.GetValueOrDefault();
                                objScheduleInfo.ProductCategory = item.ProductCategory;
                                objScheduleInfo.ProductSubCategory = item.ProductSubCategory;
                                objScheduleInfo.ProductSubCategory2 = string.Join(", ", item.ProductSubCategory2);
                                objScheduleInfo.FactoryCountry = item.FactoryCountry;
                                objScheduleInfo.CSNames = item.CSNames;
                                objScheduleInfo.SuggestedManday = item.SuggestedManday;
                                count++;
                            }

                            resultDataList.Add(objScheduleInfo);
                        }
                    }
                }
                else
                {
                    var objScheduleInfo = new ScheduleBookingItemExportSummarynew()
                    {
                        BookingId = item.BookingId,
                        CustomerName = item.CustomerName,
                        SupplierName = item.SupplierName,
                        FactoryName = item.FactoryName,
                        FactoryAddress = item.FactoryAddress,
                        ProvinceName = item.ProvinceName,
                        CityName = item.CityName,
                        //ReportCount = item.ReportCount,
                        //ProductCount = item.Productcount,
                        QuotationStatus = item.QuotationStatus,
                        FirstServiceDate = item.FirstServiceDate,
                        SampleSize = item.SampleSize,
                        ServiceType = item.ServiceType,
                        ServiceFromDate = item.ServiceDateFrom,
                        ServiceToDate = itemServiceDateTo,
                        ScheduleDate = null,
                        CountyName = item.CountyName,
                        TownName = item.TownName,
                        Zone = item.ZoneName,
                        QCName = string.Empty,
                        ActualManday = 0,
                        AdditionalQCName = string.Empty,
                        CSName = string.Empty,
                        QCVisibility = string.Empty,
                        CalculatedTotalWorkingHours = item.CalculatedWorkingHours.GetValueOrDefault(),
                        Office = item.Office,
                        CSNames = item.CSNames,
                        CreateDate = item.CreateDate
                    };
                    if (item.StatusId == (int)BookingStatus.Inspected || item.StatusId == (int)BookingStatus.ReportSent)
                        objScheduleInfo.ReportSampleSize = item.ReportSampleSize;

                    if (count == 1)
                    {
                        // objScheduleInfo.ActualManday = item.ActualManDay;
                        objScheduleInfo.QuotationManday = item.ManDay.GetValueOrDefault();
                        objScheduleInfo.PlannedManday = item.PlannedManday;
                        objScheduleInfo.ReportCount = item.ReportCount;
                        objScheduleInfo.ProductCount = item.Productcount;
                        objScheduleInfo.ProductCategory = item.ProductCategory;
                        objScheduleInfo.ProductSubCategory = item.ProductSubCategory;
                        objScheduleInfo.ProductSubCategory2 = string.Join(", ", item.ProductSubCategory2);
                        objScheduleInfo.FactoryCountry = item.FactoryCountry;
                        objScheduleInfo.CSNames = item.CSNames;
                        objScheduleInfo.SuggestedManday = item.SuggestedManday;
                    }
                    resultDataList.Add(objScheduleInfo);
                }
            }

            var qcDatila = resultDataList.Select(x => new { x.QCName, x.ScheduleDate })
                       .GroupBy(p => new { p.QCName, p.ScheduleDate }, p => p, (key, _data) =>
                     new
                     {
                         QCName = key.QCName,
                         ScheduleDate = key.ScheduleDate
                     }).ToList();

            foreach (var qc in qcDatila)
            {
                var qcInfo = resultDataList.Where(x => x.ScheduleDate == qc.ScheduleDate && x.QCName == qc.QCName);
                var minusValue = Math.Round(qcInfo.Sum(x => x.ScheduleManday), MidpointRounding.AwayFromZero) - qcInfo.Sum(x => x.ScheduleManday);
                qcInfo.Last().ScheduleManday = qcInfo.Last().ScheduleManday + minusValue;
            }
            return resultDataList;
        }


        public List<ScheduleBookingItemExportSummaryProductLevel> MapExportSummaryProductLevel(IEnumerable<ScheduleBookingItem> bookingItems)
        {
            var resultDataList = new List<ScheduleBookingItemExportSummaryProductLevel>();

            foreach (var booking in bookingItems)
            {
                int count = 1;
                DateTime.TryParseExact(booking.ServiceDateTo, StandardDateFormat, new CultureInfo("en-us"), DateTimeStyles.None, out DateTime itemServiceDateTo);
                double plannedManday = 0;
                int planCount = 0;
                DateTime rowServiceDate;
                if (DateTime.TryParseExact(booking.ServiceDate, StandardDateFormat, new CultureInfo("en-us"), DateTimeStyles.None, out rowServiceDate))
                {
                    plannedManday = booking.quotationMandayListByDate.Where(x => x.ServiceDate == rowServiceDate).Sum(x => x.ManDay);
                    planCount = 1;
                }
                var scheduleQCs = booking.ServiceDateQCNames.SelectMany(x => x.QCInfo).Distinct().ToList();
                var scheduleQCNames = string.Join(", ", scheduleQCs.Select(y => y.QCName).Distinct().ToList());
                var additionalQCNames = string.Join(", ", booking.ServiceDateAdditionalQCNames.SelectMany(x => x.QCInfo.Select(y => y.QCName)).Distinct().ToArray());
                var csNames = string.Join(",", booking.ServiceDateCSNames.SelectMany(x => x.CSInfo.Select(y => y.CSName)).Distinct().ToArray());
                if (booking.InspectionProducts.Any())
                {
                    foreach (var inspectionProduct in booking.InspectionProducts)
                    {
                        var productPOColorList = booking.POColorTransactions.Where(x => x.BookingId == booking.BookingId && x.ProductId == inspectionProduct.ProductId).ToList();
                        //get report title
                        var reportTitle = booking.ReportTitleList.Where(x => x.ProductId == inspectionProduct.ProductId).Select(x => x.ReportTitle).FirstOrDefault();

                        var scheduleBookingItemProductLevel = new ScheduleBookingItemExportSummaryProductLevel()
                        {
                            BookingId = booking.BookingId,
                            CustomerName = booking.CustomerName,
                            SupplierName = booking.SupplierName,
                            FactoryName = booking.FactoryName,
                            FactoryAddress = booking.FactoryAddress,
                            ProvinceName = booking.ProvinceName,
                            CityName = booking.CityName,
                            QuotationStatus = booking.QuotationStatus,
                            FirstServiceDate = booking.FirstServiceDate,
                            ServiceType = booking.ServiceType,
                            ServiceFromDate = booking.ServiceDateFrom,
                            ServiceToDate = itemServiceDateTo,
                            CountyName = booking.CountyName,
                            TownName = booking.TownName,
                            Zone = booking.ZoneName,
                            QCName = scheduleQCNames,
                            AdditionalQCName = additionalQCNames,
                            CSName = csNames,
                            QCVisibility = booking.HasQcVisible ? "True" : "False",
                            Office = booking.Office,
                            MSChart = booking.IsMSChartProduct ? "Yes" : "No",
                            Brand = booking.Brands,
                            ColorCode = string.Join(", ", productPOColorList.OrderBy(z => z.ColorTransactionId).Select(x => x.ColorCode).ToList()),
                            ColorName = string.Join(", ", productPOColorList.OrderBy(z => z.ColorTransactionId).Select(y => y.ColorName).ToList()),
                            InspectionStatus = booking.StatusName,
                            OrderQty = inspectionProduct.OrderQty,
                            PoNumber = string.Join(", ", booking.InspectionPOs.Where(x => x.ProductId == inspectionProduct.ProductId).Select(y => y.PONumber).Distinct().ToList()),
                            ProductDescription = inspectionProduct.ProductDescription,
                            ProductRef = inspectionProduct.ProductName,
                            Season = booking.Season,
                            Unit = inspectionProduct.Unit,
                            ProductCategory = booking.ProductCategory,
                            ProductSubCategory = booking.ProductSubCategory,
                            ProductSubCategory2 = string.Join(", ", booking.ProductSubCategory2),
                            FactoryCountry = booking.FactoryCountry,
                            CSNames = booking.CSNames,
                            ReportTitle = reportTitle,
                            FactoryContact = booking.FactoryContact,
                            CreateDate = booking.CreateDate
                        };

                        if (planCount == 1)
                        {
                            scheduleBookingItemProductLevel.PlannedManday = plannedManday;
                            planCount++;
                        }
                        if (count == 1)
                        {
                            scheduleBookingItemProductLevel.ActualManday = scheduleQCs.Sum(x => x.ActualManDay);
                            scheduleBookingItemProductLevel.QuotationManday = booking.ManDay.GetValueOrDefault();
                            scheduleBookingItemProductLevel.SampleSize = booking.SampleSize;
                            scheduleBookingItemProductLevel.SuggestedManday = booking.SuggestedManday;
                            
                            if (booking.StatusId == (int)BookingStatus.Inspected || booking.StatusId == (int)BookingStatus.ReportSent)
                                scheduleBookingItemProductLevel.ReportSampleSize = booking.ReportSampleSize;
                            
                            count++;
                        }

                        resultDataList.Add(scheduleBookingItemProductLevel);
                    }
                }
            }
            return resultDataList;
        }
        public StaffSchedule MapAllocatedStaff(SchScheduleQc user, DateTime date, List<SchScheduleQc> qcList)
        {
            var list = qcList.Where(y => y.Qcid == user.Qcid && y.ServiceDate == date).Select(y => y.BookingId).ToList();

            return new StaffSchedule
            {
                StaffID = user.Qcid,
                StaffName = user.Qc?.PersonName, //+ (staffCount > 0 ? "(" + staffCount + ")" : string.Empty),
                EmailAddress = user.Qc?.EmaiLaddress,
                EmergencyCall = user.Qc?.EmergencyCall,
                Location = user.Qc?.Location.LocationName,
                BookingsAssigned = (list.Count > 0) ? "( #" + string.Join(", #", list) + ")" : null,
                BookingsAssignedShow = (list.Count <= 2) ? (list.Count > 0) ? "( #" + string.Join(", #", list) + ")" : null : "( #" + list[0] + ", #" + list[1] + ",...)",
                EmployeeType = user.Qc?.EmployeeTypeId,
                AssignedBookingCount = list.Count,
                isLeader = user.QcLeader.GetValueOrDefault()
            };
        }
        /// <summary>
        /// Map allocated cs details
        /// </summary>
        /// <param name="repoData"></param>
        /// <param name="date"></param>
        /// <param name="csList"></param>
        /// <returns></returns>
        public StaffSchedule MapAllocatedCSStaff(StaffScheduleRepo repoData, DateTime date, List<StaffScheduleRepo> csList)
        {
            var bookingList = csList.Where(y => y.StaffID == repoData.StaffID && y.ServiceDate == date).Select(y => y.BookingId).ToList();

            return new StaffSchedule
            {
                StaffID = repoData.StaffID,
                StaffName = repoData.StaffName,
                EmailAddress = repoData.EmailAddress,
                EmergencyCall = repoData.EmergencyCall,
                Location = repoData.Location,
                BookingsAssigned = (bookingList.Count > 0) ? "( #" + string.Join(", #", bookingList) + ")" : null,
                BookingsAssignedShow = (bookingList.Count <= 2) ? (bookingList.Count > 0) ? "( #" + string.Join(", #", bookingList) + ")" : null : "( #" + bookingList[0] + ", #" + bookingList[1] + ",...)",
                EmployeeType = repoData.EmployeeType,
                AssignedBookingCount = bookingList.Count,
                isLeader = false
            };
        }

        /// <summary>
        /// Map allocated Qc staff details
        /// </summary>
        /// <param name="repoData"></param>
        /// <param name="date"></param>
        /// <param name="qcList"></param>
        /// <returns></returns>
        public StaffSchedule MapAllocatedQcStaff(StaffScheduleQcRepo repoData, DateTime date, List<StaffScheduleQcRepo> qcList)
        {
            var bookingList = qcList.Where(y => y.StaffID == repoData.StaffID && y.ServiceDate == date).Select(y => y.BookingId).ToList();

            return new StaffSchedule
            {
                StaffID = repoData.StaffID,
                StaffName = repoData.StaffName,
                EmailAddress = repoData.EmailAddress,
                EmergencyCall = repoData.EmergencyCall,
                Location = repoData.Location,
                BookingsAssigned = (bookingList.Count > 0) ? "( #" + string.Join(", #", bookingList) + ")" : null,
                BookingsAssignedShow = (bookingList.Count <= 2) ?
                                (bookingList.Count > 0) ? "( #" + string.Join(", #", bookingList) + ")" : null :
                                "( #" + bookingList[0] + ", #" + bookingList[1] + ",...)",
                EmployeeType = repoData.EmployeeType,
                AssignedBookingCount = bookingList.Count,
                isLeader = repoData.isLeader
            };
        }

        public StaffSchedule MapAllocatedStaff(SchScheduleC user, DateTime date, List<SchScheduleC> qcList)
        {
            var list = qcList.Where(y => y.Csid == user.Csid && y.ServiceDate == date).Select(y => y.BookingId).ToList();

            return new StaffSchedule
            {
                StaffID = user.Csid,
                StaffName = user.Cs?.PersonName, //+ (staffCount > 0 ? "(" + staffCount + ")" : string.Empty),
                EmailAddress = user.Cs?.EmaiLaddress,
                EmergencyCall = user.Cs?.EmergencyCall,
                Location = user.Cs?.Location.LocationName,
                BookingsAssigned = (list.Count > 0) ? "( #" + string.Join(", #", list) + ")" : null,
                BookingsAssignedShow = (list.Count <= 2) ? (list.Count > 0) ? "( #" + string.Join(", #", list) + ")" : null : "( #" + list[0] + ", #" + list[1] + ",...)",
                EmployeeType = user.Cs?.EmployeeTypeId,
                AssignedBookingCount = list.Count
                //StaffImage = image
            };
        }
        //Get user list from UserStaffDetails class to QCStaffInfo
        public QCStaffInfo MapStaffData(UserStaffDetails userData)
        {
            return new QCStaffInfo
            {
                Name = userData.FullName,
                Id = userData.StaffId,
                EmployeeType = userData.EmployeeTypeId,
                Location = userData.LocationName
                //LocationId = userData.LocationId
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productData"></param>
        /// <returns></returns>
        public ScheduleProductModel MaproductData(ScheduleProductData productData, List<SchedulePOData> poData)
        {
            return new ScheduleProductModel
            {
                CuProductId = productData.CuProductId,
                MSChart = productData.IsMSChart ? ScheduleMSChartYes : ScheduleMSChartNo,
                OrderQty = productData.OrderQty,
                ProductId = productData.ProductId,
                PONumber = string.Join(", ", poData.Where(x => x.CuProductId == productData.CuProductId).Select(x => x.PONumber).ToList())
            };
        }
    }
}
