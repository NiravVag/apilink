using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Inspection;
using DTO.Quotation;
using DTO.Report;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.Maps
{
    public class ComplaintMap : ApiCommonData
    {


        public BookingProductinfo GetBookingProductPoMap(BookingProductinfo entity, List<BookingProductPoRepo> poList)
        {
            if (entity == null)
                return null;
            var poNumbers = "";
            var po = poList.Where(x => x.ProductRefId == entity.Id).ToList();
            if (po != null && po.Any())
            {
                poNumbers = String.Join(",", po.Select(x => x.PoName).ToList());
            }
            entity.PoNumbers = poNumbers;
            return entity;

        }
        public ComplaintDetailedInfo GetComplaintDetailsData(ComplaintDetailedRepo data, IEnumerable<ComplaintDetailRepo> complaintDetails, IEnumerable<int> personIncharge)
        {
            List<ComplaintDetail> objcompDetails = new List<ComplaintDetail>();
            foreach (var item in complaintDetails)
            {
                objcompDetails.Add(new ComplaintDetail
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    CategoryId = item.CategoryId,
                    Title = item.Title,
                    Description = item.Description,
                    CorrectiveAction = item.CorrectiveAction,
                    Remarks = item.Remarks,
                    AnswerDate = item.AnswerDate.HasValue ? new DateObject(item.AnswerDate.Value.Year, item.AnswerDate.Value.Month, item.AnswerDate.Value.Day) : null,
                });
            }

            return new ComplaintDetailedInfo
            {
                Id = data.Id,
                ComplaintTypeId = data.ComplaintTypeId,
                ServiceId = data.ServiceId,
                BookingNo = data.ServiceId == 1 ? data.BookingNo : data.AuditId,
                ComplaintDate = new DateObject(data.ComplaintDate.Value.Year, data.ComplaintDate.Value.Month, data.ComplaintDate.Value.Day),
                RecipientTypeId = data.RecipientTypeId,
                DepartmentId = data.DepartmentId,
                CustomerId = data.CustomerId,
                CountryId = data.CountryId,
                OfficeId = data.OfficeId,
                ComplaintDetails = objcompDetails,
                UserIds = personIncharge,
                Remarks = data.Remarks
            };
        }

        public ComplaintSummaryResult GetComplaintSummaryData(ComplaintSummaryRepoResult item)
        {
            var cusBookingNo = !string.IsNullOrEmpty(item?.CustomerBookingNo) ? "/ " + item?.CustomerBookingNo : "";
            return new ComplaintSummaryResult
            {
                Id = item?.Id ?? 0,
                BookingId = item?.BookingId.GetValueOrDefault() ?? 0,
                CustomerId = item?.CustomerId.GetValueOrDefault() ?? 0,
                CustomerName = item?.CustomerName,
                ServiceName = item?.ServiceName,
                ComplaintTypeName = item?.ComplaintTypeName,
                ComplaintDate = item != null ? (item.ComplaintDate.HasValue ? item?.ComplaintDate?.ToString(StandardDateFormat) : "") : "",
                ServiceDate = item?.ServiceDateFrom == item?.ServiceDateTo ? item?.ServiceDateFrom?.ToString(StandardDateFormat) : item?.ServiceDateFrom?.ToString(StandardDateFormat) + " - " + item?.ServiceDateTo?.ToString(StandardDateFormat),
                CreatedBy = item?.CreatedBy,
                BookingNoCustomerNo = item?.BookingId > 0 ? item?.BookingId + cusBookingNo : ""
            };
        }

        public ExportComplaintSummaryRepoResult GetComplaintExportSummaryData(ExportComplaintSummaryRepoResult item, int? serviceId, IEnumerable<ServiceTypeList> serviceTypeList,
            IEnumerable<ExportComplaintDetailRepo> complaintsDetails, IEnumerable<CompTranPersonInCharge> personInChargeList)
        {
            var serviceTypes = new List<string>();
            if (serviceId == (int)Service.InspectionId)
            {
                serviceTypes = serviceTypeList.Where(x => x.InspectionId == item.BookingId).Select(x => x.serviceTypeName).ToList();
            }
            else if (serviceId == (int)Service.AuditId)
            {
                serviceTypes = serviceTypeList.Where(x => x.AuditId == item.AuditId).Select(x => x.serviceTypeName).ToList();
            }
            var complaintDetail = complaintsDetails.Where(x => x.ComplaintId == item.Id).ToList();
            var personInChargeNames = personInChargeList.Where(x => x.ComplaintId == item.Id).Select(x => x.PsersonInChargeNavigation?.PersonName).ToList();

            return new ExportComplaintSummaryRepoResult
            {
                Id = item.Id,
                BookingId = item.BookingId,
                AuditId = item.AuditId,
                CustomerName = item.CustomerName,
                ComplaintTypeName = item.ComplaintTypeName,
                ComplaintOffice = item.ComplaintOffice,
                ComplaintCountry = item.ComplaintCountry,
                ComplaintDate = item.ComplaintDate,
                ServiceName = item.ServiceName,
                ServiceDateFrom = item.ServiceDateFrom,
                ServiceDateTo = item.ServiceDateTo,
                SupplierName = item.SupplierName,
                Factory = item.Factory,
                Department = item.Department,
                RecipientType = item.RecipientType,
                ServiceType = string.Join(",", serviceTypes),
                PersonInCharge = string.Join(",", personInChargeNames),
                Remarks = item.Remarks,
                ComplaintDetails = complaintDetail
            };
        }

        public List<ExportComplaintSummaryResult> MapExportCompalintSummary(IEnumerable<ExportComplaintSummaryRepoResult> items)
        {

            var resultDataList = new List<ExportComplaintSummaryResult>();

            foreach (var item in items)
            {
                if (item.ComplaintDetails.Any())
                {
                    foreach (var complaintDetail in item.ComplaintDetails)
                    {
                        var objScheduleInfo = new ExportComplaintSummaryResult()
                        {
                            BookingId = item.BookingId,
                            AuditId = item.AuditId,
                            CustomerName = item.CustomerName,
                            ComplaintTypeName = item.ComplaintTypeName,
                            ComplaintOffice = item.ComplaintOffice,
                            ComplaintCountry = item.ComplaintCountry,
                            SupplierName = item.SupplierName,
                            Factory = item.Factory,
                            ComplaintDate = item.ComplaintDate,
                            ServiceName = item.ServiceName,
                            ServiceDate = item?.ServiceDateFrom == item?.ServiceDateTo ? item?.ServiceDateFrom?.ToString(StandardDateFormat) : item?.ServiceDateFrom?.ToString(StandardDateFormat) + " - " + item?.ServiceDateTo?.ToString(StandardDateFormat),
                            Department = item.Department,
                            RecipientType = item.RecipientType,
                            ServiceType = item.ServiceType,
                            PersonInCharge = item.PersonInCharge,
                            ProductId = complaintDetail.ProductId,
                            ProductDescription = complaintDetail.ProductDescription,
                            Category = complaintDetail.Category,
                            ComplaintDescription = complaintDetail.Description,
                            CorrectiveAction = complaintDetail.CorrectiveAction,
                            AnswerDate = complaintDetail.AnswerDate,
                            Remarks = complaintDetail.Remarks,
                            Comments = item.Remarks
                        };
                        resultDataList.Add(objScheduleInfo);
                    }
                }
                else
                {
                    var objScheduleInfo = new ExportComplaintSummaryResult()
                    {
                        BookingId = item.BookingId,
                        AuditId = item.AuditId,
                        CustomerName = item.CustomerName,
                        ComplaintTypeName = item.ComplaintTypeName,
                        ComplaintOffice = item.ComplaintOffice,
                        ComplaintCountry = item.ComplaintCountry,
                        SupplierName = item.SupplierName,
                        Factory = item.Factory,
                        ComplaintDate = item.ComplaintDate,
                        ServiceName = item.ServiceName,
                        ServiceDate = item?.ServiceDateFrom == item?.ServiceDateTo ? item?.ServiceDateFrom?.ToString(StandardDateFormat) : item?.ServiceDateFrom?.ToString(StandardDateFormat) + " - " + item?.ServiceDateTo?.ToString(StandardDateFormat),
                        Department = item.Department,
                        RecipientType = item.RecipientType,
                        ServiceType = item.ServiceType,
                        PersonInCharge = item.PersonInCharge,
                        ProductId = item.ProductId,
                        ProductDescription = item.ProductDescription,
                        Category = item.Category,
                        ComplaintDescription = item.ComplaintDescription,
                        CorrectiveAction = item.CorrectiveAction,
                        AnswerDate = item.AnswerDate,
                        Remarks = item.Remarks,
                        Comments = item.Remarks
                    };
                    resultDataList.Add(objScheduleInfo);
                }
            }

            return resultDataList;
        }
    }
}
