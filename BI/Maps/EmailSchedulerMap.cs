using DTO.Common;
using DTO.CommonClass;
using DTO.Inspection;
using DTO.Quotation;
using DTO.Report;
using DTO.Schedule;
using DTO.User;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace BI.Maps
{
    public class EmailSchedulerMap : ApiCommonData
    {

        public List<ScheduleQCEmail> GetInspectionScheduleQCEmails(IEnumerable<ScheduleStaffItem> schScheduleQcs,
            IEnumerable<ScheduleQCEntityData> scheduleQCEntityData, string missionUrl, IEnumerable<AEDetails> AElist,
            IEnumerable<ServiceTypeList> serviceTypeList, List<FactoryCountry> factoryLocations, List<CommonDataSource> csNameList,
            List<InspectionSupplierFactoryContacts> supplierContacts, List<InspectionSupplierFactoryContacts> factoryContacts,
            List<InspectionPOColorTransaction> PoColorList, IEnumerable<ProductTranData> productTranList,
            List<BookingProductPoRepo> poList, IEnumerable<ReportProducts> products, List<BookingContainer> containers, IEnumerable<BookingReportQuantityData> bookingReportData)
        {

            var scheduleQCReportData = schScheduleQcs.Join(bookingReportData, qc => qc.BookingId, report => report.BookingId, (qc, reportData) => new
            {
                qc,
                reportData
            }).ToList();

            var list = new List<ScheduleQCEmail>();
            foreach (var scheduleQC in scheduleQCReportData)
            {
                var inspection = scheduleQCEntityData.FirstOrDefault(x => x.BookingId == scheduleQC.qc.BookingId);
                var factoryLocation = factoryLocations.FirstOrDefault(x => x.BookingId == inspection.BookingId);
                var factoryContact = factoryContacts.FirstOrDefault(x => x.InspectionId == inspection.BookingId);
                var supplierContact = supplierContacts.FirstOrDefault(x => x.InspectionId == inspection.BookingId);
                var serviceType = serviceTypeList.FirstOrDefault(x => x.InspectionId == inspection.BookingId);

                var isContainerService = serviceType.serviceTypeId == (int)InspectionServiceTypeEnum.Container;

                List<ReportProducts> inspectionProducts = null;
                List<ProductTranData> inspectionProductTranList = null;
                List<string> poNumbers = null;
                List<string> productNames = null;
                List<BookingContainer> bookingContainers = null;
                if (isContainerService)
                {
                    bookingContainers = containers.Where(x => x.BookingId == inspection.BookingId && x.ReportId == scheduleQC.reportData.ReportId).ToList();
                    var bookingPoData = poList.Where(x => x.BookingId == inspection.BookingId && bookingContainers.Select(x => x.ContainerRefId).Contains(x.ContainerRefId.GetValueOrDefault())).ToList();
                    var bookingProductRefIds = bookingPoData.Select(x => x.ProductRefId).Distinct().ToList();

                    inspectionProducts = products.Where(x => x.BookingId == inspection.BookingId && bookingProductRefIds.Contains(x.ProductRefId)).ToList();
                    inspectionProductTranList = productTranList?.Where(x => x.BookingId == inspection.BookingId && bookingProductRefIds.Contains(x.ProductRefId)).ToList();

                    poNumbers = bookingPoData.Select(x => x.PoName).Distinct().ToList();
                    productNames = inspectionProductTranList.Select(x => x.ProductCode).Distinct().ToList();
                }
                else
                {
                    inspectionProducts = products.Where(x => x.BookingId == inspection.BookingId && x.FbReportId == scheduleQC.reportData.ReportId).ToList();
                    inspectionProductTranList = productTranList?.Where(x => x.BookingId == inspection.BookingId && x.FbReportId == scheduleQC.reportData.ReportId).ToList();

                    poNumbers = poList?.Where(x => x.BookingId == inspection.BookingId && inspectionProducts.Select(y => y.ProductRefId).Contains(x.ProductRefId)).Select(x => x.PoName).Distinct().ToList();
                    productNames = inspectionProductTranList.Select(x => x.ProductCode).Distinct().ToList();
                }




                var qcEmail = new ScheduleQCEmail()
                {
                    BookingId = inspection.BookingId,
                    MisssionUrl = missionUrl + "?missionId=" + inspection.MisssionId + "&callFrom=" + (int)UserEmailVerification.ScheduleQCEmail,
                    MissionId = inspection.MisssionId,
                    Service = Service_Inspection,
                    ServiceId = (int)Service.InspectionId,
                    QCId = scheduleQC.qc.Id,
                    QCName = scheduleQC.qc.Name,
                    IsChinaCountry = scheduleQC.qc.IsChinaCountry,
                    QCEmail = new EmailAddressAttribute().IsValid(scheduleQC.qc.CompanyEmail) ? scheduleQC.qc.CompanyEmail : null,
                    CustomerName = inspection.CustomerName,
                    SupplierName = inspection.SupplierName,
                    FactoryName = inspection.FactoryName,
                    FactoryRegionalName = inspection.FactoryRegionalName,
                    ServiceDate = scheduleQC.qc.ServiceDate,
                    AEName = string.Join(",", AElist.Where(x => x.Customerid == inspection.CustomerId).Select(x => x.FullName).Distinct().ToList()),
                    FactoryPhNo = inspection.FactoryPhNo,
                    FactoryAddressEnglish = factoryLocation?.FactoryAdress,
                    FactoryAddressRegional = factoryLocation?.FactoryRegionalAddress,
                    FactoryContactName = factoryContact?.ContactName,
                    FactoryContactPhone = factoryContact?.Phone,
                    SupplierContactName = supplierContact?.ContactName,
                    SupplierContactPhone = supplierContact?.Phone,
                    FactoryContactEmail = inspection.FactoryContactEmail,
                    ServiceType = serviceType?.serviceTypeName,
                    QCNames = string.Join(",", schScheduleQcs.Where(x => x.BookingId == inspection.BookingId && x.ServiceDate == scheduleQC.qc.ServiceDate).Select(y => y.Name).Distinct().ToArray()),
                    TotalNumberofProducts = inspectionProducts.Count,
                    TotalNumberofReports = !isContainerService ? inspectionProducts.Count(x => x.CombineProductId == null) +
                                                                            inspectionProducts.Where(x => x.CombineProductId != null).
                                                                            Select(x => x.CombineProductId).Distinct().Count() :
                                                                            bookingContainers.Select(x => x.ContainerId).Distinct().Count(),
                    ProductCategory = string.Join(", ", inspectionProducts.Select(x => x.ProductCategoryName).Distinct().ToList()),
                    ProductSubCategory2 = string.Join(", ", inspectionProducts.Select(x => x.ProductSubCategory2Name).Distinct().ToList()),
                    CsNames = string.Join(",", csNameList.Where(x => x.Id == inspection.BookingId).Select(y => y.Name).Distinct().ToArray()),
                    FactoryProvince = factoryLocation?.ProvinceName,
                    FactoryCity = factoryLocation?.CityName,
                    FactoryCounty = factoryLocation?.CountyName,
                    FactoryTown = factoryLocation?.TownName,
                    ScheduleComments = inspection.ScheduleComments,
                    ProductList = inspectionProducts.Select(x => new InsectionPOProductData
                    {
                        ProductID = x.ProductId,
                        AqlID = x.Aql,
                        Critical = x.Critical ?? 0,
                        Major = x.Major ?? 0,
                        Minor = x.Minor ?? 0,
                        OrderQuantity = x.TotalBookingQuantity,
                        SampleSize = (x.CombineProductId != null && x.CombineProductId > 0) ? x.CombineAqlQuantity : x.AqlQuantity
                    }).ToList(),
                    QCBookingComments = inspection.QCBookingcomments,
                    BookingOfficeLocation = inspection.BookingOfficeLocation,
                    Color = string.Join(", ", PoColorList?.Where(x => x.BookingId == inspection.BookingId && inspectionProducts.Select(y => y.ProductRefId).Contains(x.ProductRefId.GetValueOrDefault())).Select(x => x.ColorName).ToList()),
                    ProductCode = string.Concat(string.Join(", ", productNames.Take(5).ToList()), (productNames.Count > 5 ? $" (+{productNames.Count - 5})" : "")),
                    Unit = string.Join(", ", inspectionProductTranList?.Select(x => x.Unit).Distinct().ToList()),
                    OrderQty = inspectionProducts.Sum(x => x.TotalBookingQuantity),
                    PONumber = string.Concat(string.Join(", ", poNumbers.Take(5).ToList()), (poNumbers.Count > 5 ? $" (+{poList.Count - 5})" : "")),
                    ReportTitle = scheduleQC.reportData.ReportName,
                    SampleSize = inspectionProducts.Any(x => x.CombineProductId != null && x.CombineProductId > 0) ? inspectionProducts.FirstOrDefault(x => x.CombineProductId != null && x.CombineProductId > 0 && x.CombineAqlQuantity > 0)?.CombineAqlQuantity.GetValueOrDefault() : inspectionProducts.FirstOrDefault()?.AqlQuantity.GetValueOrDefault(),
                    BusinessLine = inspection.BusinessLine
                };

                list.Add(qcEmail);
            }


            return list;
        }

        public List<ScheduleQCEmail> GetAuditScheduleQCEmails(IEnumerable<ScheduleStaffItem> auditorData, IEnumerable<ScheduleQCEntityData> auditData, string missionUrl,
            List<FactoryCountry> factoryLocations, IEnumerable<ServiceTypeList> serviceTypeList, List<CommonDataSource> csNameList,
            List<InspectionSupplierFactoryContacts> supplierContacts, List<InspectionSupplierFactoryContacts> factoryContacts)
        {
            var list = new List<ScheduleQCEmail>();
            foreach (var auditor in auditorData)
            {
                var audit = auditData.FirstOrDefault(x => x.BookingId == auditor.BookingId);
                var factoryLocation = factoryLocations.FirstOrDefault(x => x.BookingId == audit.BookingId);
                var factoryContact = factoryContacts.FirstOrDefault(x => x.InspectionId == audit.BookingId);
                var supplierContact = supplierContacts.FirstOrDefault(x => x.InspectionId == audit.BookingId);
                var qcEmail = new ScheduleQCEmail()
                {
                    BookingId = audit.BookingId,
                    MisssionUrl = missionUrl + "?missionId=" + audit.MisssionId + "&callFrom=" + (int)UserEmailVerification.ScheduleQCEmail,
                    MissionId = audit.MisssionId,
                    ServiceId = (int)Service.AuditId,
                    Service = Service_Audit,
                    QCId = auditor.Id,
                    QCName = auditor.Name,
                    IsChinaCountry = auditor.IsChinaCountry,
                    QCEmail = new EmailAddressAttribute().IsValid(auditor.CompanyEmail) ? auditor.CompanyEmail : null,
                    CustomerName = audit.CustomerName,
                    SupplierName = audit.SupplierName,
                    FactoryName = audit.FactoryName,
                    FactoryRegionalName = audit.FactoryRegionalName,
                    ServiceDate = auditor.ServiceDate,
                    FactoryPhNo = audit.FactoryPhNo,
                    FactoryAddressEnglish = factoryLocation?.FactoryAdress,
                    FactoryAddressRegional = factoryLocation?.FactoryRegionalAddress,
                    FactoryContactName = factoryContact?.ContactName,
                    FactoryContactPhone = factoryContact?.Phone,
                    SupplierContactName = supplierContact?.ContactName,
                    SupplierContactPhone = supplierContact?.Phone,
                    FactoryContactEmail = audit.FactoryContactEmail,
                    ServiceType = serviceTypeList.FirstOrDefault(x => x.AuditId == audit.BookingId)?.serviceTypeName,
                    QCNames = string.Join(",", auditorData.Where(x => x.BookingId == audit.BookingId && x.ServiceDate == auditor.ServiceDate).Select(y => y.Name).Distinct().ToArray()),
                    CsNames = string.Join(",", csNameList.Where(x => x.Id == audit.BookingId).Select(y => y.Name).Distinct().ToArray()),
                    FactoryProvince = factoryLocation?.ProvinceName,
                    FactoryCity = factoryLocation?.CityName,
                    FactoryCounty = factoryLocation?.CountyName,
                    FactoryTown = factoryLocation?.TownName,
                    BookingOfficeLocation = audit.BookingOfficeLocation,
                    ReportTitle = audit.ReportTitle,
                };

                list.Add(qcEmail);
            }
            return list;
        }
    }
}
