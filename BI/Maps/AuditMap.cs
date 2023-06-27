using DTO.Customer;
using DTO.References;
using DTO.Audit;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.HumanResource;
using System.Linq;
using System.IO;
using DTO.RepoRequest.Audit;
using DTO.Common;
using DTO.Quotation;
using DTO.AuditReport;
using Entities.Enums;
using AuditStatus = DTO.Audit.AuditStatus;

namespace BI.Maps
{
    public class AuditMap : ApiCommonData
    {
        public AuditEvaluationRound GetEvaluationRound(AudEvaluationRound entity)
        {
            if (entity == null)
                return null;
            return new AuditEvaluationRound
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
        public Auditor GetAuditor(AudTranAuditor entity)
        {
            if (entity == null)
                return null;
            return new Auditor
            {
                Name = entity?.Staff?.PersonName,
                Id = entity.StaffId
            };
        }
        public AuditStatus GetAuditStatus(AudStatus entity)
        {
            if (entity == null)
                return null;
            return new AuditStatus
            {
                Id = entity.Id,
                StatusName = entity.Status,
            };
        }
        public AuditStatus GetAuditStatus(AuditRepoStatus entity)
        {
            if (entity == null)
                return null;
            return new AuditStatus
            {
                Id = entity.Id,
                StatusName = entity.StatusName,
                TotalCount = entity.TotalCount,
                StatusColor = AuditStatusColor.GetValueOrDefault(entity.Id, "")
            };
        }

        public AuditStatus GetInvoiceAuditStatus(AuditRepoStatus entity)
        {
            if (entity == null)
                return null;
            return new AuditStatus
            {
                Id = entity.Id,
                StatusName = entity.StatusName,
                TotalCount = entity.TotalCount,
                StatusColor = AuditSummaryStatusColor.GetValueOrDefault(entity.Id, "")
            };
        }

        public AuditCancelRescheduleReasons GetAuditCancelRescheduleReasons(AudCancelRescheduleReason entity)
        {
            if (entity == null)
                return null;
            return new AuditCancelRescheduleReasons
            {
                Id = entity.Id,
                Reason = entity.Reason
            };
        }
        public AuditBookingContact GetBookingContact(AudBookingContact entity)
        {
            if (entity == null)
                return null;
            return new AuditBookingContact
            {
                Id = entity.Id,
                PlanningTeamEMailCC = entity.BookingEmailCc ?? "",
                PlanningTeamEmailTo = entity.BookingEmailTo ?? "",
                ContactInformation = entity.ContactInformation ?? "",
                PenaltyEmail = entity.PenaltyEmail,
                OfficeAddress = entity?.Office?.Address,
                OfficeFax = entity?.Office?.Fax,
                OfficeName = entity?.Office?.LocationName,
                OfficeRegionalLanguageAddress = "",
                OfficeTelNo = entity?.Office?.Tel
            };
        }
        public AuditItem GetAuditSearchItem(AuditRepoItem entity, IEnumerable<AuditStatus> lstAuditStatus,
                                                        List<AuditServiceTypeRepoResponse> serviceTypeList,
                                                        List<AuditFactoryCountryRepoResponse> factoryList,
                                                        List<AuditAuditorRepoResponse> auditorList,
                                                        List<AuditQuotationRepoResponse> quotationStatusList,
                                                        List<AuditSupplierCustomerRepoResponse> supplierCustomerList,
                                                        List<AuditFactoryCustomerRepoResponse> factoryCustomerList)
        {
            if (entity == null)
                return null;

            return new AuditItem()
            {
                AuditId = entity.AuditId,
                CustomerName = entity?.CustomerName,
                FactoryName = entity?.FactoryName,
                PoNumber = entity?.PoNumber,
                ReportNo = entity?.ReportNo,
                ServiceDateFrom = entity?.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = entity?.ServiceDateTo.ToString(StandardDateFormat),
                SupplierName = entity?.SupplierName,
                Office = entity?.Office,
                StatusId = entity.StatusId,
                StatusName = lstAuditStatus.Any() ? (lstAuditStatus.Where(x => x.Id == entity.StatusId).Select(y => y.StatusName).FirstOrDefault()) : "",
                BookingCreatedBy = entity?.BookingCreatedBy,
                CustomerBookingNo = entity.CustomerBookingNo,
                EaqfReference = entity.CustomerBookingNo,
                CreatedOnEaqf = entity.CreatedOnEaqf,
                FactoryCountry = factoryList.Where(x => x.AuditId == entity.AuditId).Select(x => x.FactoryCountryName).FirstOrDefault(),
                FactoryCity = factoryList.Where(x => x.AuditId == entity.AuditId).Select(x => x.FactoryCity).FirstOrDefault(),
                FactoryState = factoryList.Where(x => x.AuditId == entity.AuditId).Select(x => x.FactoryState).FirstOrDefault(),
                Auditors = string.Join(",", auditorList.Where(x => x.AuditId == entity.AuditId).Select(x => x.AuditorName).ToList()),
                ServiceType = serviceTypeList.Where(x => x.AuditId == entity.AuditId).Select(x => x.ServiceTypeName).FirstOrDefault(),
                QuotationStatus = quotationStatusList.Where(x => x.AuditId == entity.AuditId).Select(x => new QuotStatus
                {
                    Id = x.StatusId,
                    Label = x.StatusName
                }).FirstOrDefault(),
                SupplierCustomerCode = supplierCustomerList.FirstOrDefault(x => x.AuditId == entity.AuditId)?.SupplierCustomerCode,
                FactoryCustomerCode = factoryCustomerList.FirstOrDefault(x => x.AuditId == entity.AuditId)?.FactoryCustomerCode
            };
        }


        public AuditSaveCancelRescheduleItem MapAuditCancelDetails(AudTransaction entity, int optypeid)
        {
            if (entity == null)
                return null;
            var cancelentity = entity.AudTranCancelReschedules.Where(x => x.OperationTypeId == optypeid).FirstOrDefault();
            if (cancelentity != null && optypeid == (int)Entities.Enums.AuditBookingOperationType.Cancel)
            {
                return new AuditSaveCancelRescheduleItem()
                {
                    AuditId = cancelentity.AuditId,
                    Reasontypeid = cancelentity?.ReasonTypeId,
                    Comment = cancelentity?.Comments,
                    Cancelrescheduletimetype = cancelentity?.TimeTypeId,
                    CurrencyId = cancelentity?.CurrencyId,
                    Internalcomment = cancelentity?.InternalComments,
                    Travelexpense = cancelentity?.TravellingExpense != null ? Convert.ToDouble(cancelentity?.TravellingExpense) : 0,
                    Optypeid = cancelentity.OperationTypeId
                };
            }
            else
            {
                return new AuditSaveCancelRescheduleItem() { AuditId = entity.Id, Optypeid = optypeid, Cancelrescheduletimetype = 0 };
            }
        }
        public AuditCancelRescheduleItem GetAuditCancelItem(AudTransaction entity)
        {
            if (entity == null)
                return null;
            return new AuditCancelRescheduleItem()
            {
                AuditId = entity.Id,
                CustomerName = entity?.Customer?.CustomerName,
                FactoryName = entity?.Factory?.SupplierName,
                PoNumber = entity?.PoNumber,
                ReportNo = entity?.ReportNo,
                ServiceDateFrom = entity?.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = entity?.ServiceDateTo.ToString(StandardDateFormat),
                SupplierName = entity?.Supplier.SupplierName,
                ServiceType = entity?.AudTranServiceTypes?.Select(x => x.ServiceType?.Name)?.FirstOrDefault(),
                Office = entity?.Office?.LocationName,
                StatusId = entity.StatusId
            };
        }
        public AuditBasicDetails GetAuditBasicDetails(AudTransaction entity)
        {
            if (entity == null)
                return null;
            return new AuditBasicDetails()
            {
                AuditId = entity.Id,
                CustomerName = entity?.Customer?.CustomerName,
                FactoryName = entity?.Factory?.SupplierName,
                PoNumber = entity?.PoNumber,
                ReportNo = entity?.ReportNo,
                ServiceDateFrom = entity?.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = entity?.ServiceDateTo.ToString(StandardDateFormat),
                SupplierName = entity?.Supplier.SupplierName,
                ServiceType = entity?.AudTranServiceTypes?.Select(x => x.ServiceType?.Name)?.FirstOrDefault(),
                StatusId = entity.StatusId,
                CustomerBookingNo = entity.CustomerBookingNo
            };
        }
        public AuditReportDetails MapAuditReportSummary(AudTransaction entity, Func<string, string> _funcGetMimeType)
        {
            if (entity == null)
                return null;
            var reportdetails = entity?.AudTranReportDetails?.FirstOrDefault();
            if (reportdetails == null)
                return null;

            //fetch the uploaded files from old table if any
            var attachments = entity.AudTranReport1S?.Where(x => x.Active)?.Select(x => new AuditReportAttachment()
            {
                FileName = x.FileName,
                Id = x.Id,
                IsNew = false,
                //Uniqueld = x.GuidId,
                Guid = x.GuidId,
                MimeType = _funcGetMimeType(Path.GetExtension(x.FileName)),
                FileUrl = null
            });

            //fetch the uploaded files from new table if any
            attachments = attachments.Concat(entity.AudTranReports?.Where(x => x.Active)?.Select(x => new AuditReportAttachment()
            {
                FileName = x.FileName,
                Id = x.Id,
                IsNew = false,
                Uniqueld = x.UniqueId,
                MimeType = _funcGetMimeType(Path.GetExtension(x.FileName)),
                FileUrl = x.FileUrl
            }));

            var response = new AuditReportDetails()
            {
                Auidtid = entity.Id,
                Comment = reportdetails.Comments,
                Servicedatefrom = Static_Data_Common.GetCustomDate(reportdetails.ServiceDateFrom),
                Servicedateto = Static_Data_Common.GetCustomDate(reportdetails.ServiceDateTo),
                Auditors = entity.AudTranAuditors.Where(x => x.IsAudited != null && x.IsAudited.Value && x.Active).Select(x => x.StaffId).ToList(),
                Attachments = attachments
            };
            return response;
        }
        public AuditDetails MapAuditDetails(AudTransaction entity, Func<string, string> _funcGetMimeType)
        {
            if (entity == null)
                return null;
            var faprofile = entity.AudTranFaProfiles?.FirstOrDefault();
            var response = new AuditDetails()
            {
                AuditId = entity.Id,
                CustomerId = entity.CustomerId,
                BrandId = entity.BrandId,
                DepartmentId = entity.DepartmentId,
                ServiceDateFrom = Static_Data_Common.GetCustomDate(entity.ServiceDateFrom),
                ServiceDateTo = Static_Data_Common.GetCustomDate(entity.ServiceDateTo),
                SeasonId = entity.SeasonId,
                SeasonYearId = entity.SeasonYearId,
                EvaluationRoundId = entity.EvalutionId,
                SupplierId = entity.SupplierId,
                FactoryId = entity.FactoryId,
                Accreditations = faprofile?.Accrediations,
                AdminStaff = faprofile?.AdministrativeStaff ?? 0,
                AnnualHolidays = faprofile?.AnnualHolidays,
                AnnualProduction = faprofile?.AnnualProduction,
                APIComments = entity.ApiBookingComments,
                ApplicantEmail = entity.ApplicantEmail,
                ApplicantName = entity.ApplicantName,
                ApplicantPhNo = entity.ApplicantPhNo,
                BrandsProduced = faprofile.TypesOfBrands,
                CustomerComments = entity.CusBookingComments,
                FactoryCreationDate = Static_Data_Common.GetCustomDate(faprofile.CreatedDate),
                FactoryExtension = faprofile?.PossibilityOfExtension,
                FactorySurfaceArea = faprofile?.CompanySurfaceArea,
                InternalComments = entity.InternalComments,
                Investment = faprofile?.InvestmentBackground,
                Liability = faprofile?.PublicLiabilityInsurance,
                ManufactureProducts = faprofile?.TypeOfProductManufactured,
                MaximumCapacity = faprofile?.MaximumCapacity,
                NoOfCustomers = faprofile?.NoOfCustomer,
                NoOfSuppliers = faprofile?.NoOfSuppliersComponent,
                NumberOfSites = faprofile?.NumberOfSites,
                OfficeId = entity.OfficeId ?? 0,
                OpenHour = faprofile.CompanyOpenTime,
                ProductionStaff = faprofile?.ProductionStaff ?? 0,
                PoNumber = entity.PoNumber,
                QualityStaff = faprofile.QualityStaff ?? 0,
                ReportNo = entity.ReportNo,
                SalesStaff = faprofile?.SalesStaff ?? 0,
                TotalCapacityByCustomer = faprofile.PercentageCusTotalCapacity,
                TradeAssociation = faprofile.IndustryTradeAssociation,
                TotalStaff = faprofile.TotalStaff ?? 0,
                StatusId = entity.StatusId,
                AuditTypeid = entity.AuditTypeId,
                CustomerBookingNo = entity.CustomerBookingNo,
                CreatedbyUserType = entity?.CreatedByNavigation?.UserTypeId,
                CustomerContactListItems = entity.AudTranCuContacts.Where(x => x.Active).Select(x => x.ContactId).ToList(),
                FactoryContactListItems = entity.AudTranFaContacts.Where(x => x.Active).Select(x => x.ContactId).ToList(),
                SupplierContactListItems = entity.AudTranSuContacts.Where(x => x.Active).Select(x => x.ContactId).ToList(),
                ServiceTypeId = entity.AudTranServiceTypes.Where(x => x.Active).Select(x => x.ServiceTypeId).FirstOrDefault(),
                Auditors = entity.AudTranAuditors?.Where(x => x.Active).Select(x => x.StaffId).ToList(),
                AuditCS = entity.AudTranCS?.Where(x => x.Active).Select(x => x.StaffId).ToList(),
                AuditworkprocessItems = entity.AudTranWorkProcesses?.Where(x => x.Active).Select(x => x.WorkProcessId).ToList(),
                Attachments = entity.AudTranFileAttachments?.Where(x => x.Active).Select(x => new AuditAttachment()
                {
                    FileName = x.FileName,
                    Id = x.Id,
                    IsNew = false,
                    uniqueld = x.UniqueId,
                    FileUrl = x.FileUrl,
                    MimeType = _funcGetMimeType(Path.GetExtension(x.FileName))
                }),
                AuditProductCategoryId = entity.CuProductCategory,
                IsEaqf = false
            };

            return response;
        }

        public AuditCusReportBookingDetails GetAuditCusReport(AuditRepoCusReportBookingDetails auditData, List<AuditcusReport> lstreportexists,
                                                                        List<AuditServiceType> lstauditservice, List<AuditFactoryCountry> _auditFactoryCountryList, Func<string, string> _funcGetMimeType)

        {
            var reportdetails = lstreportexists.FirstOrDefault(x => x.AuditId == auditData.AuditId);
            var factoryCountry = _auditFactoryCountryList.FirstOrDefault(x => x.AuditId == auditData.AuditId);
            return new AuditCusReportBookingDetails()
            {
                AuditId = auditData.AuditId,
                Customer = auditData.Customer,
                Factory = auditData.Factory,
                OfficeName = auditData.officeName,
                ReportNo = auditData.ReportNo,
                CustomerBookingNo = auditData.CustomerBookingNo,
                ServiceDate = ((auditData.ServiceFromDate.Date == auditData.ServiceToDate.Date) ? auditData.ServiceFromDate.ToString(StandardDateFormat) :
                            (auditData.ServiceFromDate.ToString(StandardDateFormat) + "-" + auditData.ServiceToDate.ToString(StandardDateFormat))),
                StatusId = auditData.StatusId,
                StatusName = auditData.StatusName,
                Supplier = auditData.Supplier,
                Reportid = reportdetails != null ? reportdetails.ReportId : auditData.ReportId,
                Fbreportid = auditData.ReportId,
                ServiceType = string.Join(",", lstauditservice.Where(x => x.bookingid == auditData.AuditId).Select(x => x.ServiceType).ToList()),
                MimeType = reportdetails != null ? _funcGetMimeType(Path.GetExtension(reportdetails?.filename)) : "",
                pathextension = reportdetails != null ? Path.GetExtension(reportdetails?.filename) : "",
                ReportUrl = reportdetails != null ? reportdetails?.fileUrl : "",
                FbReportUrl = auditData.ReportUrl,
                ReportFileUniqueId = reportdetails != null ? reportdetails?.ReportFileUniqueId : "",
                ReportFileName = reportdetails != null ? reportdetails?.filename : "",
                FactoryCountry = factoryCountry?.CountryName,
            };
        }

        public AuditType MapAuditType(AudType entity)
        {
            if (entity == null)
                return null;
            return new AuditType()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
