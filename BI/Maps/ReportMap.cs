using DTO.Common;
using DTO.CommonClass;
using DTO.Inspection;
using DTO.InspectionCustomerDecision;
using DTO.Invoice;
using DTO.Kpi;
using DTO.OtherManday;
using DTO.Quotation;
using DTO.Report;
using DTO.Schedule;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BI.Maps
{
    public class ReportMap : ApiCommonData
    {

        public CustomerReportItem GetInspectionReportData(CustomerReportBookingValues entity, IEnumerable<InspectionStatus> inspectionStatus,
                                                                   IEnumerable<ServiceTypeList> serviceTypeList, IEnumerable<BookingReportSummaryLinkRepo> bookingReportSummaryLink,
                                                                   IEnumerable<PoDetails> potransactions, List<BookingContainer> bookingContainers, IEnumerable<InspectionCsData> inspectionCsList,
                                                                   List<SupplierCode> supplierCodes)
        {


            var serviceType = serviceTypeList.FirstOrDefault(x => x.InspectionId == entity.BookingId);

            var reportSummary = bookingReportSummaryLink.FirstOrDefault(x => x.BookingId == entity.BookingId);

            var nonCombineProductCount = potransactions.Where(x => x.CombineProductId.GetValueOrDefault() == 0 && x.BookingId == entity.BookingId)
                                            .Select(x => x.ProductId).Distinct().Count();

            var combineProductCount = potransactions.Where(x => x.CombineProductId > 0 && x.BookingId == entity.BookingId)
                                        .Select(x => x.CombineProductId).Distinct().Count();

            var containerCount = bookingContainers.Where(x => x.ContainerId > 0 && x.BookingId == entity.BookingId).Select(x => x.ContainerId).Distinct().Count();

            var productCount = serviceTypeList?.Where(x => x.InspectionId == entity.BookingId).Select(x => x.serviceTypeId).FirstOrDefault() == (int)InspectionServiceTypeEnum.Container
                                 ? containerCount : nonCombineProductCount + combineProductCount;

            var supplierCode = supplierCodes?.Where(x => x.CustomerId == entity.CustomerId && x.SupplierId == entity.SupplierId).Select(x => x.Code).FirstOrDefault();

            var supplierNameAndCode = !string.IsNullOrEmpty(supplierCode) ?
                                        "(" + supplierCode + ") - " + entity?.SupplierName : entity?.SupplierName;

            return new CustomerReportItem()
            {
                BookingId = entity.BookingId,
                CustomerBookingNo = entity.CustomerBookingNo,
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                FactoryName = entity.FactoryName,
                ServiceDateFrom = entity?.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = entity?.ServiceDateTo.ToString(StandardDateFormat),
                SupplierName = supplierNameAndCode,
                ServiceType = serviceType?.serviceTypeName,
                ServiceTypeId = serviceType?.serviceTypeId,
                StatusId = entity.StatusId,
                StatusName = inspectionStatus?.Where(x => x.Id == entity.StatusId)?.Select(x => x.StatusName)?.FirstOrDefault(),
                IsPicking = entity.IsPicking,
                PreviousBookingNo = entity.PreviousBookingNo,
                FactoryId = entity.FactoryId,
                FbMissionId = entity.missionId.GetValueOrDefault(),
                OverAllStatus = entity.MissionStatus,
                Office = entity.Office,
                ProductCategory = entity.ProductCategory,
                ReportSummaryLink = reportSummary?.ReportSummaryLink,
                ReportDate = entity.ReportDate?.ToString(StandardDateFormat),
                ProductCount = productCount,
                InspectionCsList = inspectionCsList.Where(x => x.InspectionId == entity.BookingId).ToList(),
                BookingType = entity.BookingType,
                IsEAQF = entity.IsEAQF
            };
        }

        public CustomerReportItem GetInspectionReportResult(CustomerReportBookingValues entity, IEnumerable<InspectionStatus> inspectionStatus,
                                                                   IEnumerable<ServiceTypeList> serviceTypeList, IEnumerable<BookingReportSummaryLinkRepo> bookingReportSummaryLink,
                                                                   IEnumerable<PoDetails> potransactions, IEnumerable<BookingReportSummaryLinkRepo> bookingContainerReportSummaryLink)
        {

            // picking check added
            bool isPicking = false;

            foreach (var poTransaction in potransactions.Where(x => x.BookingId == entity.BookingId))
            {
                if (poTransaction.PickingQuantity > 0)
                {
                    isPicking = true;
                    break;
                }
            }

            return new CustomerReportItem()
            {
                BookingId = entity.BookingId,
                CustomerBookingNo = entity.CustomerBookingNo,
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                FactoryName = entity.FactoryName,
                ServiceDateFrom = entity?.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = entity?.ServiceDateTo.ToString(StandardDateFormat),
                SupplierName = entity?.SupplierName,
                ServiceType = serviceTypeList.Where(x => x.InspectionId == entity.BookingId).Select(x => x.serviceTypeName).FirstOrDefault(),
                ServiceTypeId = serviceTypeList.Where(x => x.InspectionId == entity.BookingId).Select(x => x.serviceTypeId).FirstOrDefault(),
                StatusId = entity.StatusId,
                StatusName = inspectionStatus?.Where(x => x.Id == entity.StatusId)?.Select(x => x.StatusName)?.FirstOrDefault(),
                IsPicking = isPicking,
                PreviousBookingNo = entity.PreviousBookingNo,
                FactoryId = entity.FactoryId,
                FbMissionId = entity.missionId.GetValueOrDefault(),
                OverAllStatus = entity.MissionStatus,
                Office = entity.Office,
                ProductCategory = entity.ProductCategory,
                ReportSummaryLink = serviceTypeList.Any(x => x.serviceTypeId == (int)InspectionServiceTypeEnum.Container && x.InspectionId == entity.BookingId) ?
                                                 bookingContainerReportSummaryLink?.Where(x => x.BookingId == entity.BookingId
                                                 && !string.IsNullOrEmpty(x.ReportSummaryLink)).Select(x => x.ReportSummaryLink).FirstOrDefault() :
                                                 bookingReportSummaryLink?.Where(x => x.BookingId == entity.BookingId
                                                 && !string.IsNullOrEmpty(x.ReportSummaryLink)).Select(x => x.ReportSummaryLink).FirstOrDefault(),
                ReportDate = entity.ReportDate?.ToString(StandardDateFormat)
            };
        }

        private string GetResultTextColor(string title)
        {
            if (title != null)
            {
                if (title.ToLower() == "pass")
                {
                    return ReportResult.Limegreen.ToString();
                }
                else if (title.ToLower() == "fail")
                {
                    return ReportResult.Red.ToString();
                }
                else
                {
                    return ReportResult.Orange.ToString();
                }
            }
            return null;
        }

        public ReportProductItem GetProductList(ReportProducts products, IEnumerable<FBReportDetails> reportList, IEnumerable<ReportProducts> productList, IEnumerable<SchScheduleQc> qcList, IEnumerable<BookingPONumbers> poList)
        {
            //if the products are combined and AQL is not selected, make the first product in the list as parent product
            var isAQLSelected = productList.Where(z => z.CombineProductId == products.CombineProductId && z.CombineAqlQuantity.GetValueOrDefault() > 0).Count();

            string strPONumber = string.Empty;
            // export case
            if (poList == null || poList.Count() == 0)
            {
                strPONumber = products.PONumber;
            }
            else if (products.ContainerId != null && poList != null && poList.Any()) // container case 
            {
                strPONumber = string.Join(" ,", poList.Where(x => x.ContainerRefId == products.Id).Select(y => y.PoNumber).ToArray().Distinct());

            }
            else if (poList != null && poList.Any() && products.ContainerId == null) // product case
            {
                strPONumber = string.Join(" ,", poList.Where(x => x.ProductRefId == products.Id).Select(y => y.PoNumber).ToArray().Distinct());
            }
            else
            {
                strPONumber = products.PONumber;
            }
            //take the report data 
            var reportData = new FBReportDetails();
            if (products.ApiReportId > 0)
                reportData = reportList.FirstOrDefault(x => x.ReportId == products.ApiReportId);

            return new ReportProductItem()
            {
                Id = products.Id,
                bookingId = products.BookingId,
                ProductId = products.ProductId,
                ProductName = products.ProductName,
                ProductDescription = products.ProductDescription,
                ProductQuantity = products.ProductQuantity,
                ProductCategoryName = products.ProductCategoryName,
                ProductSubCategoryName = products.ProductSubCategoryName,
                FbReportId = products.FbReportId,
                ApiReportId = products.ApiReportId,
                ColorCode = ReportResult.FFFF.ToString(),
                CombineProductId = products.CombineProductId,
                ReviewStatus = reportData?.ReviewStatus,
                FillingStatus = reportData?.FillingStatus,
                ReportStatus = reportData?.StatusName,
                PONumber = strPONumber,
                InspectedQuantity = reportData?.InspectedQuantity,
                finalReportLink = reportData?.FinalReportPath,
                finalManualReportLink = reportData?.FinalManualReportPath,
                ReportStatusId = reportData?.ReportStatusId,
                CombineProductCount = products.CombineProductId > 0 ? productList.Where(x => x.CombineProductId == products.CombineProductId).Count() : 1,

                IsParentProduct = (products.CombineProductId.GetValueOrDefault() == 0) ? true : (products.CombineAqlQuantity != null &&
                    products.CombineAqlQuantity != 0) ? true : (isAQLSelected == 0 && productList.Where(z => z.CombineProductId == products.CombineProductId).FirstOrDefault().ProductId == products.ProductId ? true : false),
                Result = reportData?.OverAllResult,
                ReportTitle = reportData?.ReportTitle,
                ResultColor = GetResultTextColor(reportData?.OverAllResult),
                CombineAqlQuantity = products.CombineAqlQuantity,

                ReportStatusColor =
                    ReportStatusColor.TryGetValue
                    (reportData?.ReportStatusId
                     ?? 0, out string reportColor) ? reportColor : "",

                FillingStatusColor =
                    ReportStatusColor.TryGetValue
                    (reportData?.FillingStatusId
                     ?? 0, out string fillingColor) ? fillingColor : "",

                ReviewStatusColor =
                    ReportStatusColor.TryGetValue
                    (reportData?.ReviewStatusId
                     ?? 0, out string reviewColor) ? reviewColor : "",
                QcName = string.Join(",", qcList.Select(y => y.Qc?.PersonName).ToArray().Distinct()),
                ContainerName = products.ContainerId != null ? InspectionServiceTypeData.GetValueOrDefault((int)InspectionServiceTypeEnum.Container) + products.ContainerId : ""
            };
        }

        public CustomerReportItem MapProductsToBooking(CustomerReportItem entity, IEnumerable<ReportProductItem> products)
        {
            return new CustomerReportItem()
            {
                BookingId = entity.BookingId,
                CustomerBookingNo = entity.CustomerBookingNo,
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                FactoryName = entity.SupplierName,
                ServiceDateFrom = entity?.ServiceDateFrom,
                ServiceDateTo = entity?.ServiceDateTo,
                SupplierName = entity?.SupplierName,
                ServiceType = entity.ServiceType,
                Office = entity.Office,
                StatusId = entity.StatusId,
                //StatusName = inspectionStatus?.Where(x => x.Id == entity.StatusId)?.Select(x => x.StatusName)?.FirstOrDefault(),
                IsPicking = false,
                PreviousBookingNo = entity.PreviousBookingNo,
                FactoryId = entity.FactoryId,
                ProductCategory = entity.ProductCategory,
                ReportProducts = products.Where(x => x.bookingId == entity.BookingId).Select(x => new ReportProductItem
                {
                    bookingId = x.bookingId,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductDescription = x.ProductDescription,
                    ProductQuantity = x.ProductQuantity,
                    ProductSubCategoryName = x.ProductSubCategoryName,
                    FbReportId = x.FbReportId,
                    CombineProductId = x.CombineProductId,
                    ReportStatus = x.ReportStatus,
                    PONumber = x.PONumber,
                    InspectedQuantity = x.InspectedQuantity,
                    finalReportLink = x.finalReportLink,
                    Result = x.Result,
                    ReportTitle = x.ReportTitle,
                    ContainerName = x.ContainerName
                })
            };
        }

        /// <summary>
        /// Map the customer report export
        /// </summary>
        /// <param name="reportDataList"></param>
        /// <param name="serviceTypeList"></param>
        /// <param name="fbReportList"></param>
        /// <param name="poDetails"></param>
        /// <returns></returns>
        public List<ExportCustomerReportData> MapExportCustomerReport(List<ExportInspectionReportData> reportDataList, List<ServiceTypeList> serviceTypeList, List<ExportFBReportRepo> fbReportList, List<ProductPOList> poDetails,
                                                                        List<SupplierCode> supplierCodeList)
        {
            List<ExportCustomerReportData> exportReportDataList = new List<ExportCustomerReportData>();

            var groupedReportList = reportDataList.GroupBy(x => x.FbReportId).ToList();
            var firstReport = false;
            foreach (var reportProducts in groupedReportList)
            {
                firstReport = true;
                foreach (var reportData in reportProducts)
                {
                    var serviceType = serviceTypeList.FirstOrDefault(x => x.InspectionId == reportData.BookingNo);

                    var fbReportData = fbReportList.FirstOrDefault(x => x.FbReportId == reportData.FbReportId);

                    var poList = poDetails.Where(x => x.ProductRefId == reportData.ProductRefId).Distinct().ToList();

                    //get supplier code by customer id and supplier id
                    var supplierCode = supplierCodeList?.Where(x => x.CustomerId == reportData.CustomerId && x.SupplierId == reportData.SupplierId).
                                        Select(x => x.Code).FirstOrDefault();

                    ExportCustomerReportData exportReportData = new ExportCustomerReportData();

                    exportReportData.Customer = reportData.Customer;
                    exportReportData.CustomerBookingNo = reportData.CustomerBookingNo;
                    exportReportData.BookingNo = reportData.BookingNo;
                    exportReportData.Supplier = reportData.Supplier;
                    exportReportData.SupplierCode = supplierCode;
                    exportReportData.Factory = reportData.Factory;
                    exportReportData.ServiceDateFrom = reportData.ServiceDateFrom;
                    exportReportData.ServiceDateTo = reportData.ServiceDateTo;
                    exportReportData.ServiceType = serviceType?.serviceTypeName;
                    exportReportData.ProductCategory = reportData.ProductCategory;
                    exportReportData.ProductId = reportData.ProductId;
                    exportReportData.ProductDescription = reportData.ProductDescription;
                    exportReportData.Quantity = reportData.BookingQuantity;

                    //if containerid is there set containername and set ponumber else po number comma seperated
                    if (reportData.ContainerRefId > 0)
                    {
                        exportReportData.ContainerName = reportData.ContainerRefId > 0 ? "Container-" + reportData.ContainerRefId : "";
                        exportReportData.PoNumber = reportData.PoNumber;
                    }
                    else
                        exportReportData.PoNumber = string.Join(", ", poList.Select(x => x.PoNumber).Distinct().ToList());
                    //set report details only for the first time even it has multipler products/container belongs to it
                    if (firstReport)
                    {
                        exportReportData.InspectedQuantity = fbReportData?.InspectedQuantity;
                        exportReportData.ReportNo = fbReportData?.ReportNo;
                        exportReportData.ReportStatus = fbReportData?.ReportStatus;
                        exportReportData.Result = fbReportData?.Result;
                    }

                    firstReport = false;
                    exportReportDataList.Add(exportReportData);
                }
            }

            return exportReportDataList;
        }

        /// <summary>
        /// Map the data for filling/review export
        /// </summary>
        /// <param name="reportDataList"></param>
        /// <param name="serviceTypeList"></param>
        /// <param name="fbReportList"></param>
        /// <param name="poDetails"></param>
        /// <returns></returns>
        public List<ExportFillingReportData> MapExportFillingReport(List<ExportInspectionReportData> reportDataList, List<ServiceTypeList> serviceTypeList, List<ExportFBReportRepo> fbReportList, List<ProductPOList> poDetails,
            List<InspectionCsData> inspectionCsList)
        {
            List<ExportFillingReportData> exportReportDataList = new List<ExportFillingReportData>();

            var groupedReportList = reportDataList.GroupBy(x => x.FbReportId).ToList();
            var firstReport = false;
            foreach (var reportProducts in groupedReportList)
            {
                firstReport = true;
                foreach (var reportData in reportProducts)
                {
                    var serviceType = serviceTypeList.FirstOrDefault(x => x.InspectionId == reportData.BookingNo);

                    var fbReportData = fbReportList.FirstOrDefault(x => x.FbReportId == reportData.FbReportId);

                    var poList = poDetails.Where(x => x.ProductRefId == reportData.ProductRefId).Distinct().ToList();

                    ExportFillingReportData exportReportData = new ExportFillingReportData();

                    exportReportData.Customer = reportData.Customer;
                    exportReportData.CustomerBookingNo = reportData.CustomerBookingNo;
                    exportReportData.BookingNo = reportData.BookingNo;
                    exportReportData.Supplier = reportData.Supplier;
                    exportReportData.Factory = reportData.Factory;
                    exportReportData.ServiceDateFrom = reportData.ServiceDateFrom;
                    exportReportData.ServiceDateTo = reportData.ServiceDateTo;
                    exportReportData.ServiceType = serviceType?.serviceTypeName;
                    exportReportData.ProductCategory = reportData.ProductCategory;
                    exportReportData.ProductId = reportData.ProductId;
                    exportReportData.ProductDescription = reportData.ProductDescription;
                    exportReportData.Quantity = reportData.BookingQuantity;
                    //if containerid is there set containername and set ponumber else po number comma seperated
                    if (reportData.ContainerRefId > 0)
                    {
                        exportReportData.ContainerName = reportData.ContainerRefId > 0 ? "Container-" + reportData.ContainerRefId : "";
                        exportReportData.PoNumber = reportData.PoNumber;
                    }
                    else
                        exportReportData.PoNumber = string.Join(", ", poList.Select(x => x.PoNumber).Distinct().ToList());

                    //set report details only for the first time even it has multipler products/container belongs to it
                    if (firstReport)
                    {
                        exportReportData.InspectedQuantity = fbReportData?.InspectedQuantity;
                        exportReportData.ReportNo = fbReportData?.ReportNo;
                        exportReportData.FillingStatus = fbReportData?.FillingStatus;
                        exportReportData.ReviewStatus = fbReportData?.ReviewStatus;
                        exportReportData.ReportStatus = fbReportData?.ReportStatus;
                        exportReportData.Result = fbReportData?.Result;
                    }
                    exportReportData.CsNames = string.Join(", ", inspectionCsList?.Where(x => x.InspectionId == reportData.BookingNo).Select(x => x.CsName).ToList());
                    firstReport = false;
                    exportReportDataList.Add(exportReportData);
                    //product.
                }
            }

            return exportReportDataList;
        }


        /// <summary>
        /// map the staff data and calculate the utilization rate
        /// </summary>
        /// <param name="repoItem"></param>
        /// <param name="inspectionOccupancyData"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isUtilizationRate"></param>
        /// <returns></returns>
        public InspectionOccupancySummary MapInspectionOccupancies(InspectionOccupancyRepoItem repoItem, InspectionOccupancyData inspectionOccupancyData, DateTime fromDate, DateTime toDate, bool isUtilizationRate)
        {
            //get the count of qc working days
            var numberOfWorkingDays = inspectionOccupancyData.ScheduleQcs.Count(x => x.QcId == repoItem.Id);

            var maxWorkingDays = 0;
            var bankHolidays = 0;
            var leaves = 0;
            var totalActualWds = 0;
            decimal utilizationRate = 0;
            InspectionOccupancyCategory? inspectionOccupancyCategory = null;

            if (inspectionOccupancyData != null)
            {
                //calculate the max working days and bank holidays based on the join data
                //if join date between the request from and to date new from date is join date and calculation
                if (repoItem.JoinDate >= fromDate && repoItem.JoinDate <= toDate)
                {
                    maxWorkingDays = (toDate.Date - repoItem.JoinDate.Value.Date).Days;
                    bankHolidays = inspectionOccupancyData.Holidays.Where(x => x.HolidayDate >= repoItem.JoinDate && (x.LocationId == null || x.LocationId == repoItem.OfficeId) && x.CountryId == repoItem.OfficeCountryId).Select(x => x.HolidayDate).Distinct().Count();
                }
                //if join date less than the from date then calculation based on the request from date and to date
                else if (repoItem.JoinDate <= fromDate)
                {
                    maxWorkingDays = (toDate.Date - fromDate.Date).Days;
                    bankHolidays = inspectionOccupancyData.Holidays.Where(x => (x.LocationId == null || x.LocationId == repoItem.OfficeId) && x.CountryId == repoItem.OfficeCountryId).Select(x => x.HolidayDate).Distinct().Count();
                }

                //get the hr leaves based on the staff id
                leaves = inspectionOccupancyData.HrLeaves.Count(x => x.StaffId == repoItem.Id);
                //calculate the total actual working days
                totalActualWds = maxWorkingDays - leaves - bankHolidays;
                //when utilization rate checked from ui
                if (isUtilizationRate && totalActualWds > 0)
                {
                    //utilization rate formula based on number of working days and total actual wds 
                    utilizationRate = (Math.Round((decimal)numberOfWorkingDays / (decimal)totalActualWds, 2) * 100);
                    //when utilization rate is less than 30 inspection occupancy category is low
                    if (utilizationRate <= 30)
                    {
                        inspectionOccupancyCategory = InspectionOccupancyCategory.Low;
                    }
                    //when utilization rate is between 30 to 60 inspection occupancy category is orange
                    else if (utilizationRate > 30 && utilizationRate <= 60)
                    {
                        inspectionOccupancyCategory = InspectionOccupancyCategory.Medium;
                    }
                    //when utilization rate is greater than 60 inspection occupancy category is green
                    else
                    {
                        inspectionOccupancyCategory = InspectionOccupancyCategory.High;
                    }
                }
            }



            return new InspectionOccupancySummary
            {
                Name = repoItem.Name,
                Office = repoItem.Office,
                OfficeCountry = repoItem.OfficeCountry,
                EmployeeType = repoItem.EmployeeType,
                JoinDate = repoItem.JoinDate?.ToString(StandardDateFormat),
                NumberOfWds = numberOfWorkingDays,
                MaxWds = maxWorkingDays,
                Leaves = leaves,
                BankHolidays = bankHolidays,
                TotalActualWds = totalActualWds,
                UtilizationRate = utilizationRate,
                MaxCapacity = Math.Max(totalActualWds, numberOfWorkingDays),
                OutsourceCompany = repoItem.OutSourceCompany,
                ContractEnd = repoItem.RegisnDate?.ToString(StandardDateFormat),
                OtherManday = inspectionOccupancyData.OtherMandays.Where(x => x.QcId == repoItem.Id).Sum(y => y.Manday),
                InspectionOccupancyCategory = inspectionOccupancyCategory
            };
        }

        //map export inspection occupancies
        public ExportInspectionOccupancySummary MapExportInspectionOccupancies(InspectionOccupancyRepoItem repoItem, IEnumerable<HrHoliday> holidays, IEnumerable<HrLeave> hrLeaves, IEnumerable<ActualManday> scheduleQcs, IEnumerable<OtherMandayDataRepo> otherMandayDatas, DateTime fromDate, DateTime toDate)
        {
            //get the number of working days
            var numberOfWorkingDays = scheduleQcs.Count(x => x.QcId == repoItem.Id);

            var maxWorkingDays = 0;
            var bankHolidays = 0;
            //check join date is between requested from date and to date
            if (repoItem.JoinDate >= fromDate && repoItem.JoinDate <= toDate)
            {
                //request to date - join date 
                maxWorkingDays = (toDate.Date - repoItem.JoinDate.Value.Date).Days;
                bankHolidays = holidays.Count(x => x.StartDate >= repoItem.JoinDate);
            }
            else if (repoItem.JoinDate <= fromDate)
            {
                //request to date - from date
                maxWorkingDays = (toDate.Date - fromDate.Date).Days;
                bankHolidays = holidays.Count();
            }

            //calculates the leaves
            var leaves = hrLeaves.Count(x => x.StaffId == repoItem.Id);
            //calculate total working days
            var totalActualWds = maxWorkingDays - leaves - bankHolidays;

            return new ExportInspectionOccupancySummary
            {
                Name = repoItem.Name,
                Office = repoItem.Office,
                OfficeCountry = repoItem.OfficeCountry,
                EmployeeType = repoItem.EmployeeType,
                JoinDate = repoItem.JoinDate?.ToString(StandardDateFormat),
                NumberOfWds = numberOfWorkingDays,
                MaxWds = maxWorkingDays,
                Leaves = leaves,
                BankHolidays = bankHolidays,
                TotalActualWds = totalActualWds,
                UtilizationRate = ((double)numberOfWorkingDays / (double)totalActualWds) * 100,
                MaxCapacity = Math.Max(totalActualWds, numberOfWorkingDays),
                OutsourceCompany = repoItem.OutSourceCompany,
                ContractEnd = repoItem.RegisnDate?.ToString(StandardDateFormat),
                OtherManday = otherMandayDatas.Where(x => x.QcId == repoItem.Id).Sum(y => y.Manday)
            };
        }

        /// <summary>
        ///  get the map customer report details
        /// </summary>
        /// <param name="inspectionDetails"></param>
        /// <param name="fbProductDetails"></param>
        /// <param name="productPOLists"></param>
        /// <param name="customerDecisions"></param>
        /// <param name="fbReportQuantities"></param>
        /// <param name="fbInspSummaries"></param>
        /// <param name="fBReportDefects"></param>
        /// <param name="brands"></param>
        /// <param name="customerProductCategories"></param>
        /// <param name="customerProductTypes"></param>
        /// <param name="factoryDetails"></param>
        /// <returns></returns>
        public List<CustomerReportDetails> MapCustomerReportDetails(List<CustomerReportInspectionRepo> inspectionDetails, List<CustomerReportDetailsRepo> fbReportDetails, IEnumerable<ReportProducts> productList, List<ProductPOList> productPOLists,
            List<CustomerDecisionProductList> customerDecisions, List<BookingShipment> fbReportQuantities,
            List<FBReportInspSubSummary> fbInspSummaries, List<FBReportDefects> fBReportDefects, List<BookingBrandAccess> brands,
            List<ParentDataSource> customerProductCategories, List<ParentDataSource> customerProductTypes, List<InvoiceBookingFactoryDetails> factoryDetails)
        {
            List<CustomerReportDetails> customerReportDetails = new List<CustomerReportDetails>();

            foreach (var inspection in inspectionDetails)
            {
                var products = productList.Where(x => x.BookingId == inspection.InspectionId).ToList();
                var factoryCountry = factoryDetails.FirstOrDefault(x => x.FactoryId == inspection.FactoryId);
                var brandNames = string.Join(", ", brands.Where(x => x.BookingId == inspection.InspectionId).Select(y => y.BrandName).ToList());
                foreach (var product in products)
                {
                    var fbReport = fbReportDetails.FirstOrDefault(x => x.ReportId == product.FbReportId);
                    var customerDecision = customerDecisions.Where(x => x.ReportId == fbReport.ReportId).OrderByDescending(x => x.CreatedOn.GetValueOrDefault()).FirstOrDefault();
                    var fbReportQuantity = fbReportQuantities.Where(x => x.ReportId == fbReport.ReportId).ToList();
                    var customerReport = new CustomerReportDetails()
                    {
                        Checkpoints = fbInspSummaries.Where(x => x.FBReportId == fbReport.ReportId).Select(y => new CustomerReportCheckpoints() { Name = y.Name, Result = y.Result }).ToList(),
                        CustomerDecision = customerDecision?.CustomerDecisionName,
                        CustomerDecisionComment = customerDecision?.CustomerDecisionComment,
                        CustomerDecisionDate = customerDecision?.CreatedOn?.ToString(StandardDateFormat1),
                        FactoryName = inspection?.FactoryName,
                        FactoryCode = inspection?.FactoryCode,
                        SupplierCode = inspection?.SupplierCode,
                        SupplierName = inspection?.SupplierName,
                        InspectionCount = inspection != null && inspection.ReInspectionType.HasValue ? inspection.ReInspectionType.Value + 1 : 1,
                        PO = string.Join(", ", productPOLists.Where(x => x.FbReportId == fbReport.ReportId).Select(y => y.PoNumber).Distinct().ToList()),
                        InspectedQty = fbReportQuantity.Sum(x => x.InspectedQty.GetValueOrDefault()),
                        PresentedQty = fbReportQuantity.Sum(x => x.PresentedQty.GetValueOrDefault()),
                        ProductRef = product?.ProductName,
                        ProductDescription = product?.ProductDescription,
                        Year = inspection?.Year,
                        Season = inspection?.Season,
                        ReportNo = fbReport.ReportNo,
                        Brand = brandNames,
                        InspectionFromDate = fbReport.ServiceFromDate?.ToString(StandardDateFormat),
                        InspectionToDate = fbReport.ServiceToDate?.ToString(StandardDateFormat),
                        Unit = product?.Unit,
                        ReportResult = fbReport.ReportResult,
                        Defects = fBReportDefects?.Where(x => x.FBReportDetailId == fbReport.ReportId).Select(y => new CustomerReportDefects()
                        {
                            DefectFamily = y.DefectCategory,
                            DefectName = y.DefectDesc,
                            DefectCode = y.DefectCode,
                            MarjorCount = y.Major,
                            MinorCount = y.Minor,
                            Position = y.Position
                        }).ToList(),
                        ProductCategory = customerProductCategories?.FirstOrDefault(x => x.ParentId == product?.ProductSubCategoryId)?.Name,
                        ProductType = customerProductTypes.FirstOrDefault(x => x.ParentId == product?.ProductSubCategory2Id)?.Name,
                        FactoryCountry = factoryCountry?.FactoryCountryName,
                        FactoryCountryCode = factoryCountry?.FactoryCountryCode,
                        InspectionType = inspection?.InspectionType
                    };
                    customerReportDetails.Add(customerReport);
                }

            }
            return customerReportDetails;
        }
    }
}