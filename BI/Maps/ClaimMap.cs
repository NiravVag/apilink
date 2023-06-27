using DTO.Claim;
using DTO.Common;
using DTO.CommonClass;
using DTO.Inspection;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class ClaimMap : ApiCommonData
    {
        public ClmTransaction MapClaimEntity(ClaimDetails request, int entityId, int userId)
        {
            if (request == null)
                return null;

            var claim = new ClmTransaction
            {
                ClaimNo = request.ClaimNo,
                InspectionNo = request.BookingId,
                ClaimDate = request.ClaimDate?.ToDateTime(),
                RequestedContactName = request.RequestContactName?.Trim(),
                ReceivedFrom = request.ReceivedFromId,
                ClaimSource = request.ClaimSourceId,
                ClaimDescription = request.ClaimDescription?.Trim(),
                CustomerPriority = request.PriorityId,
                CustomerReqRefundAmount = request.Amount,
                CustomerReqRefundCurrency = request.CurrencyId,
                CustomerComments = request.Customercomment?.Trim(),
                Qccontrol100goods = request.QcControlId,
                DefectPercentage = request.DefectPercentage,
                NoOfPieces = request.NoOfPieces,
                CompareToAql = request.CompareToAQL,
                DefectDistribution = request.DefectDistributionId,
                Color = request.Color,
                DefectCartonInspected = request.DefectCartonInspected,
                FobPrice = request.FobPrice,
                FobCurrency = request.FobCurrencyId,
                RetailPrice = request.RetailPrice,
                RetailCurrency = request.RetailCurrencyId,
                StatusId = (int)ClaimStatus.Registered,
                EntityId = entityId,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                ClaimForm = request.ClaimFromId,
                AnalyzerFeedback = request.AnalyzerFeedback
            };

            if (request.StatusId == (int)ClaimStatus.Analyzed)
            {
                claim.AnalyzedBy = userId;
                claim.AnalyzedOn = DateTime.Now;
            }
            if (request.StatusId == (int)ClaimStatus.Validated)
            {
                claim.ValidatedBy = userId;
                claim.ValidatedOn = DateTime.Now;
            }

            if (request.StatusId == (int)ClaimStatus.Closed)
            {
                claim.ClosedBy = userId;
                claim.ClosedOn = DateTime.Now;
            }
            if (request.ReportIdList != null)
            {
                claim.ClmTranReports = new HashSet<ClmTranReport>();
                foreach (var item in request.ReportIdList)
                {
                    claim.ClmTranReports.Add(new ClmTranReport
                    {
                        ReportId = item,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        CreatedBy = userId
                    });
                }

            }

            if (request.DefectFamilyIdList != null)
            {
                claim.ClmTranDefectFamilies = new HashSet<ClmTranDefectFamily>();
                foreach (var item in request.DefectFamilyIdList)
                {
                    claim.ClmTranDefectFamilies.Add(new ClmTranDefectFamily
                    {
                        DefectFamilyId = item,
                        CreatedOn = DateTime.Now,
                        Active = 1,
                        CreatedBy = userId
                    });
                }
            }

            if (request.ClaimDepartmentIdList != null)
            {
                claim.ClmTranDepartments = new HashSet<ClmTranDepartment>();
                foreach (var item in request.ClaimDepartmentIdList)
                {
                    claim.ClmTranDepartments.Add(new ClmTranDepartment
                    {
                        DepartmentId = item,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        CreatedBy = userId
                    });
                }
            }

            if (request.ClaimCustomerRequestIdList != null)
            {
                claim.ClmTranCustomerRequests = new HashSet<ClmTranCustomerRequest>();
                foreach (var item in request.ClaimCustomerRequestIdList)
                {
                    claim.ClmTranCustomerRequests.Add(new ClmTranCustomerRequest
                    {
                        CustomerRequestId = item,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        CreatedBy = userId
                    });
                }
            }

            if (request.CustomerRequestRefundIdList != null)
            {
                claim.ClmTranCustomerRequestRefunds = new HashSet<ClmTranCustomerRequestRefund>();
                foreach (var item in request.CustomerRequestRefundIdList)
                {
                    claim.ClmTranCustomerRequestRefunds.Add(new ClmTranCustomerRequestRefund
                    {
                        RefundTypeId = item,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        CreatedBy = userId
                    });
                }
            }

            if (request.Attachments != null)
            {
                claim.ClmTranAttachments = new HashSet<ClmTranAttachment>();
                foreach (var attachment in request.Attachments)
                {
                    claim.ClmTranAttachments.Add(new ClmTranAttachment
                    {
                        FileType = request.FileTypeId,
                        FileDesc = request.FileDesc,
                        FileName = attachment.FileName,
                        UniqueId = attachment.uniqueld,
                        FileUrl = attachment.FileUrl,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        CreatedBy = userId
                    });
                }
            }

            return claim;
        }

        public void UpdateEnity(ClmTransaction entity, ClaimDetails request, int userId)
        {
            entity.InspectionNo = request.BookingId;
            entity.ClaimDate = request.ClaimDate?.ToDateTime();
            entity.RequestedContactName = request.RequestContactName?.Trim();
            entity.ReceivedFrom = request.ReceivedFromId;
            entity.ClaimSource = request.ClaimSourceId;
            entity.ClaimDescription = request.ClaimDescription?.Trim();
            entity.CustomerPriority = request.PriorityId;
            entity.CustomerReqRefundAmount = request.Amount;
            entity.CustomerReqRefundCurrency = request.CurrencyId;
            entity.CustomerComments = request.Customercomment?.Trim();
            entity.Qccontrol100goods = request.QcControlId;
            entity.DefectPercentage = request.DefectPercentage;
            entity.NoOfPieces = request.NoOfPieces;
            entity.CompareToAql = request.CompareToAQL;
            entity.DefectDistribution = request.DefectDistributionId;
            entity.Color = request.Color;
            entity.DefectCartonInspected = request.DefectCartonInspected;
            entity.FobPrice = request.FobPrice;
            entity.FobCurrency = request.FobCurrencyId;
            entity.RetailPrice = request.RetailPrice;
            entity.RetailCurrency = request.RetailCurrencyId;
            entity.StatusId = request.StatusId;
            entity.ClaimValidateResult = request.ClaimResultId;
            entity.ClaimRemarks = request.ClaimRemarks;
            entity.ClaimRecommendation = request.ClaimRecommendation;
            entity.ClaimRefundAmount = request.FinalAmount;
            entity.ClaimRefundCurrency = request.FinalCurrencyId;
            entity.ClaimRefundRemarks = request.ClaimFinalRefundRemarks;
            entity.RealInspectionFees = request.RealInspectionFees;
            entity.RealInspectionFeesCurrency = request.RealInspectionCurrencyId;
            entity.ClaimForm = request.ClaimFromId;
            entity.AnalyzerFeedback = request.AnalyzerFeedback;

            if (request.StatusId == (int)ClaimStatus.Analyzed)
            {
                entity.AnalyzedBy = userId;
                entity.AnalyzedOn = DateTime.Now;
            }
            if (request.StatusId == (int)ClaimStatus.Validated)
            {
                entity.ValidatedBy = userId;
                entity.ValidatedOn = DateTime.Now;
            }

            if (request.StatusId == (int)ClaimStatus.Closed)
            {
                entity.ClosedBy = userId;
                entity.ClosedOn = DateTime.Now;
            }
            if (entity.ClmTranReports == null)
                entity.ClmTranReports = new HashSet<ClmTranReport>();

            entity.ClmTranReports.Clear();

            if (request.ReportIdList != null)
            {
                entity.ClmTranReports = new HashSet<ClmTranReport>();
                foreach (var item in request.ReportIdList)
                {
                    entity.ClmTranReports.Add(new ClmTranReport
                    {
                        ReportId = item,
                        UpdatedOn = DateTime.Now,
                        Active = true,
                        UpdatedBy = userId
                    });
                }
            }

            if (entity.ClmTranDefectFamilies == null)
                entity.ClmTranDefectFamilies = new HashSet<ClmTranDefectFamily>();

            entity.ClmTranDefectFamilies.Clear();

            if (request.DefectFamilyIdList != null)
            {
                entity.ClmTranDefectFamilies = new HashSet<ClmTranDefectFamily>();
                foreach (var item in request.DefectFamilyIdList)
                {
                    entity.ClmTranDefectFamilies.Add(new ClmTranDefectFamily
                    {
                        DefectFamilyId = item,
                        UpdatedOn = DateTime.Now,
                        Active = 1,
                        UpdatedBy = userId
                    });
                }
            }

            if (entity.ClmTranDepartments == null)
                entity.ClmTranDepartments = new HashSet<ClmTranDepartment>();

            entity.ClmTranDepartments.Clear();

            if (request.ClaimDepartmentIdList != null)
            {
                entity.ClmTranDepartments = new HashSet<ClmTranDepartment>();
                foreach (var item in request.ClaimDepartmentIdList)
                {
                    entity.ClmTranDepartments.Add(new ClmTranDepartment
                    {
                        DepartmentId = item,
                        UpdatedOn = DateTime.Now,
                        Active = true,
                        UpdatedBy = userId
                    });
                }
            }

            if (entity.ClmTranCustomerRequests == null)
                entity.ClmTranCustomerRequests = new HashSet<ClmTranCustomerRequest>();

            entity.ClmTranCustomerRequests.Clear();

            if (request.ClaimCustomerRequestIdList != null)
            {
                entity.ClmTranCustomerRequests = new HashSet<ClmTranCustomerRequest>();
                foreach (var item in request.ClaimCustomerRequestIdList)
                {
                    entity.ClmTranCustomerRequests.Add(new ClmTranCustomerRequest
                    {
                        CustomerRequestId = item,
                        UpdatedOn = DateTime.Now,
                        Active = true,
                        UpdatedBy = userId
                    });
                }
            }

            if (entity.ClmTranCustomerRequestRefunds == null)
                entity.ClmTranCustomerRequestRefunds = new HashSet<ClmTranCustomerRequestRefund>();

            entity.ClmTranCustomerRequestRefunds.Clear();
            if (request.CustomerRequestRefundIdList != null)
            {
                entity.ClmTranCustomerRequestRefunds = new HashSet<ClmTranCustomerRequestRefund>();
                foreach (var item in request.CustomerRequestRefundIdList)
                {
                    entity.ClmTranCustomerRequestRefunds.Add(new ClmTranCustomerRequestRefund
                    {
                        RefundTypeId = item,
                        UpdatedOn = DateTime.Now,
                        Active = true,
                        UpdatedBy = userId
                    });
                }
            }

            if (entity.ClmTranFinalDecisions == null)
                entity.ClmTranFinalDecisions = new HashSet<ClmTranFinalDecision>();

            entity.ClmTranFinalDecisions.Clear();
            if (request.FinalDecisionIdList != null)
            {
                entity.ClmTranFinalDecisions = new HashSet<ClmTranFinalDecision>();
                foreach (var item in request.FinalDecisionIdList)
                {
                    entity.ClmTranFinalDecisions.Add(new ClmTranFinalDecision
                    {
                        FinalDecision = item,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        CreatedBy = userId
                    });
                }
            }

            if (entity.ClmTranClaimRefunds == null)
                entity.ClmTranClaimRefunds = new HashSet<ClmTranClaimRefund>();

            entity.ClmTranClaimRefunds.Clear();
            if (request.FinalRefundIdList != null)
            {
                entity.ClmTranClaimRefunds = new HashSet<ClmTranClaimRefund>();
                foreach (var item in request.FinalRefundIdList)
                {
                    entity.ClmTranClaimRefunds.Add(new ClmTranClaimRefund
                    {
                        RefundTypeId = item,
                        UpdatedOn = DateTime.Now,
                        Active = true,
                        UpdatedBy = userId
                    });
                }
            }

            if (entity.ClmTranAttachments == null)
                entity.ClmTranAttachments = new HashSet<ClmTranAttachment>();

            entity.ClmTranAttachments.Clear();
            if (request.Attachments != null)
            {
                entity.ClmTranAttachments = new HashSet<ClmTranAttachment>();
                foreach (var attachment in request.Attachments)
                {
                    entity.ClmTranAttachments.Add(new ClmTranAttachment
                    {
                        FileType = attachment.FileTypeId,
                        FileDesc = attachment.FileDesc,
                        FileName = attachment.FileName,
                        UniqueId = attachment.uniqueld,
                        FileUrl = attachment.FileUrl,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        CreatedBy = userId
                    });
                }
            }
        }

        public ClaimDetails GetClaimDetails(ClaimData entity, List<CommonDataSource> claimFileTypeList, Func<string, string> _funcGetMimeType)
        {
            if (entity == null)
                return null;

            var item = new ClaimDetails
            {
                Id = entity.Id,
                ClaimNo = entity.ClaimNo,
                BookingId = entity.BookingId,
                ReportIdList = entity.ClmTranReports?.Select(x => x.ReportId.GetValueOrDefault()).ToArray(),
                ClaimDate = Static_Data_Common.GetCustomDate(entity.ClaimDate),
                RequestContactName = entity.RequestContactName,
                ReceivedFromId = entity.ReceivedFromId,
                ClaimSourceId = entity.ClaimSourceId,
                DefectFamilyIdList = entity.ClmTranDefectFamilies?.Select(x => x.DefectFamilyId.GetValueOrDefault()).ToArray(),
                ClaimDepartmentIdList = entity.ClmTranDepartments?.Select(x => x.DepartmentId.GetValueOrDefault()).ToArray(),
                ClaimDescription = entity.ClaimDescription?.Trim(),
                ClaimCustomerRequestIdList = entity.ClmTranCustomerRequests?.Select(x => x.CustomerRequestId.GetValueOrDefault()).ToArray(),
                CustomerRequestRefundIdList = entity.ClmTranCustomerRequestRefunds?.Select(x => x.RefundTypeId.GetValueOrDefault()).ToArray(),
                PriorityId = entity.PriorityId,
                Amount = entity.Amount,
                CurrencyId = entity.CurrencyId,
                Customercomment = entity.Customercomment,
                QcControlId = entity.QcControlId,
                DefectPercentage = entity.DefectPercentage,
                NoOfPieces = entity.NoOfPieces,
                CompareToAQL = entity.CompareToAQL,
                DefectDistributionId = entity.DefectDistributionId,
                Color = entity.Color,
                DefectCartonInspected = entity.DefectCartonInspected,
                FobPrice = entity.FobPrice,
                FobCurrencyId = entity.FobCurrencyId,
                RetailPrice = entity.RetailPrice,
                RetailCurrencyId = entity.RetailCurrencyId,
                StatusId = entity.StatusId.GetValueOrDefault(),
                StatusName = entity.StatusName,
                ClaimResultId = entity.ClaimResultId,
                ClaimRecommendation = entity.ClaimRecommendation,
                FinalAmount = entity.FinalAmount,
                FinalCurrencyId = entity.FinalCurrencyId,
                ClaimFinalRefundRemarks = entity.ClaimRefundRemarks,
                ClaimRemarks = entity.ClaimRemarks,
                RealInspectionFees = entity.RealInspectionFees,
                RealInspectionCurrencyId = entity.RealInspectionCurrencyId,
                FinalDecisionIdList = entity.ClmTranFinalDecisions?.Select(x => x.FinalDecision.GetValueOrDefault()).ToArray(),
                FinalRefundIdList = entity.ClmTranClaimRefunds?.Select(x => x.RefundTypeId.GetValueOrDefault()).ToArray(),
                Attachments = entity.ClmTranAttachments?.Where(x => x.Active.Value).Select(x => new ClaimAttachmentList()
                {
                    FileTypeId = x.FileType,
                    FileTypeName = claimFileTypeList?.FirstOrDefault(z => z.Id == x.FileType).Name,
                    FileDesc = x.FileDesc,
                    FileName = x.FileName,
                    Id = x.Id,
                    IsNew = false,
                    uniqueld = x.UniqueId,
                    FileUrl = x.FileUrl,
                    MimeType = _funcGetMimeType(Path.GetExtension(x.FileName))
                }),
                ClaimFromId = entity.ClaimFromId,
                AnalyzerFeedback = entity.AnalyzerFeedback
            };

            return item;
        }

        public ClaimEmailRequest MapClaimForEmail(ClaimEmailRequest data, ClaimBookingData response, List<InternalReportProducts> productList, int china)
        {
            data.InspectionDate = response?.ServiceFromDate == response?.ServiceToDate ? response?.ServiceFromDate.ToString(StandardDateFormat) : string.Join(" - ", response?.ServiceFromDate.ToString(StandardDateFormat), response?.ServiceToDate.ToString(StandardDateFormat));
            data.CustomerId = response.CustomerId;
            data.CustomerBookingNo = response.CustomerBookingNo;
            data.CustomerName = response.CustomerName;
            data.OfficeId = response.OfficeId;
            data.FactoryAddress = response.FactoryAddress;
            data.FactoryRegionalAddress = response.FactoryRegionalAddress;
            data.SupplierContactName = response.SupplierContactName;
            data.FactoryContactName = response.FactoryContactName;
            data.FactoryName = response.FactoryName;
            // Supplier
            data.SupplierName = response.SupplierName;
            data.SupplierAddress = response.SupplierAddress;
            if (productList != null && productList.Any())
            {
                data.ProductRef = productList.FirstOrDefault().ProductName + (productList.Count > 1 ? $" (+{productList.Count - 1})" : "");
                data.ProductName = string.Join(", ", productList.Select(x=>x.ProductSubCategory2Name).Distinct().ToList());
            }           
            return data;
        }

        public ClaimStatuses GetClaimStatus(ClmRefStatus entity)
        {
            if (entity == null)
                return null;

            return new ClaimStatuses
            {
                Id = entity.Id,
                Label = entity.Name
            };
        }

        public ClaimExportItem GetClaimSearchResult(ClaimExportItem claimData, List<CommonDataSource> defectCategoryList, List<CommonDataSource> claimCustomerRequestList,
            List<CommonDataSource> claimDepartmentList, List<CommonDataSource> claimRefundTypeList, List<CommonDataSource> claimResultList, List<CommonDataSource> finalResultList,
            List<InspectionQcDetail> inspectionQcDetails, List<BookingReportData> bookingReportData, IEnumerable<BookingProductsData> bookingProductsData,
            List<InvoiceDetail> invoiceDetails, List<FactoryCountry> countryList, List<FbReportQuantityData> fbReportQuantityData)
        {
            try
            {
                var reportIds = claimData.ClmTranReports?.Select(x => x.ReportId).ToList();
                var reportNameList = bookingReportData.Where(x => reportIds.Contains(x.ReportId)).Select(x => x.ReportTitle);

                var defectFamilyIds = claimData.ClmTranDefectFamilies?.Select(x => x.DefectFamilyId).ToList();
                var defectFamilyList = defectCategoryList.Where(x => defectFamilyIds.Contains(x.Id)).Select(x => x.Name);

                var departmentIds = claimData.ClmTranDepartments?.Select(x => x.DepartmentId).ToList();
                var departmentNameList = claimDepartmentList.Where(x => departmentIds.Contains(x.Id)).Select(x => x.Name);

                var customerRequestRefundsIds = claimData.ClmTranCustomerRequestRefunds?.Select(x => x.RefundTypeId).ToList();
                var customerRequestRefundsNameList = claimRefundTypeList.Where(x => customerRequestRefundsIds.Contains(x.Id)).Select(x => x.Name);

                var finalDecisionIds = claimData.ClmTranFinalDecisions?.Select(x => x.FinalDecision).ToList();
                var finalDecisionNameList = finalResultList.Where(x => finalDecisionIds.Contains(x.Id)).Select(x => x.Name);

                var qcNameList = inspectionQcDetails.Count > 0 ? inspectionQcDetails.Where(x => x.ClaimId == claimData.ClaimId).Select(x => x.Name).ToList() : new List<string>();

                var bookingProductNames = bookingProductsData.Where(x => x.BookingId == claimData.Inspection).Select(x => x.ProductName).ToList();
                var invoiceData = invoiceDetails.Where(x => x.InspectionId == claimData.Inspection).FirstOrDefault();
                var factoryCountry = countryList.Where(x => x.BookingId == claimData.Inspection).FirstOrDefault();

                var fbReportQuntity = fbReportQuantityData.Where(x => x.InspectionId == claimData.Inspection).ToList();

                if (reportIds != null && reportIds.Count > 0)
                {
                    fbReportQuntity = fbReportQuantityData.Where(x => x.InspectionId == claimData.Inspection && reportIds.Contains(x.ReportId)).ToList();
                }

                var claimValidateData = claimResultList.Where(x => x.Id == claimData.ClaimValidateResult).FirstOrDefault();

                var customerRequestIds = claimData.ClmTranCustomerRequests?.Select(x => x.CustomerRequestId).ToList();
                var customerRequestType = claimCustomerRequestList.Where(x => customerRequestIds.Contains(x.Id)).Select(x => x.Name).ToList();

                var finalRefundIdList = claimData.ClmTranClaimRefunds?.Select(x => x.RefundTypeId.GetValueOrDefault()).ToList();
                var finalRefundNameList = claimRefundTypeList.Where(x => finalRefundIdList.Contains(x.Id)).Select(x => x.Name).ToList();

                var claim = new ClaimExportItem()
                {
                    ClaimId = claimData.ClaimId,
                    ClaimNo = claimData.ClaimNo,
                    ClaimDate = claimData.ClaimDate,
                    Inspection = claimData.Inspection,
                    CustomerName = claimData.CustomerName,
                    ClaimReceivedFrom = claimData.ClaimReceivedFrom,
                    ClaimSource = claimData.ClaimSource,
                    ClaimDescription = claimData.ClaimDescription,
                    CustomerRequestPriority = claimData.CustomerRequestPriority,
                    CustomerRequestType = customerRequestType,
                    SupplierName = claimData.SupplierName,
                    ServiceDateFrom = claimData.ServiceDateFrom,
                    ServiceDateTo = claimData.ServiceDateTo,
                    Office = claimData.Office,
                    Report = reportNameList,
                    DefectFamily = defectFamilyList,
                    Department = departmentNameList,
                    CustomerRequestRefund = customerRequestRefundsNameList,
                    Amount = claimData.Amount,
                    AnalyzerFeedback = claimData.AnalyzerFeedback,
                    CustomerReqRefundCurrency = claimData.CustomerReqRefundCurrency,
                    Remarks = claimData.Remarks,
                    Color = claimData.Color,
                    FobPrice = claimData.FobPrice,
                    FobCurrency = claimData.FobCurrency,
                    RetailPrice = claimData.RetailPrice,
                    RetailCurrency = claimData.RetailCurrency,
                    ValidatorClaimResult = claimValidateData?.Name,
                    ClaimRecommendation = claimData.ClaimRecommendation,
                    ValidatorReviewComment = claimData.ValidatorReviewComment,
                    FinalRefundType = finalRefundNameList,
                    FinalAmount = claimData.FinalAmount,
                    FinalCurrency = claimData.FinalCurrency,
                    CreatedBy = claimData.CreatedBy,
                    CreatedDate = claimData.CreatedDate,
                    AnalyzedBy = claimData.AnalyzedBy,
                    AnalyzedDate = claimData.AnalyzedDate,
                    ValidatedBy = claimData.ValidatedBy,
                    ValidatedDate = claimData.ValidatedDate,
                    ClosedBy = claimData.ClosedBy,
                    ClosedDate = claimData.ClosedDate,
                    ProductName = bookingProductNames,
                    ProductCategory = claimData.ProductCategory,
                    ProductSubCategory = claimData.ProductSubCategory,
                    Factory = claimData.Factory,
                    InspectorName = qcNameList,
                    FinalDecision = finalDecisionNameList,
                    OrderQty = fbReportQuntity.Sum(x => x.OrderQty),
                    InspectedQty = fbReportQuntity.Sum(x => x.InspectedQty),
                    FinalDecisionRemarks = claimData.FinalDecisionRemarks,
                    InvoiceAmount = invoiceData?.TotalInvoiceFees,
                    InvoiceCurrency = invoiceData?.InvoiceCurrencyName,
                    InvoiceNumber = invoiceData?.InvoiceNo,
                    InvoiceDate = invoiceData?.BilledDate,
                    InspectionCountry = factoryCountry?.CountryName,
                    RealInspectionFees = claimData.RealInspectionFees,
                    RealInspectionFeesCurrency = claimData.RealInspectionFeesCurrency,
                    StatusName = claimData.StatusName
                };

                return claim;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClaimItemExportSummary> MapExportSummary(IEnumerable<ClaimExportItem> items)
        {

            var resultDataList = new List<ClaimItemExportSummary>();
            try
            {
                foreach (var item in items)
                {
                    var objScheduleInfo = new ClaimItemExportSummary()
                    {
                        ClaimNo = item.ClaimNo,
                        Report = string.Join(", ", item.Report),
                        Inspection = item.Inspection.GetValueOrDefault(),
                        CustomerName = item.CustomerName,
                        ClaimDate = item.ClaimDate?.ToString(StandardDateFormat),
                        ClaimStatus = item.StatusName,
                        ClaimReceivedFrom = item.ClaimReceivedFrom,
                        ClaimSource = item.ClaimSource,
                        DefectFamily = string.Join(", ", item.DefectFamily),
                        Department = string.Join(", ", item.Department),
                        ClaimDescription = item.ClaimDescription,
                        Office = item.Office,
                        OfficeCountry = item.OfficeCountry,
                        InspectionCountry = item.InspectionCountry,
                        SupplierName = item.SupplierName,
                        Factory = item.Factory,
                        CustomerRequestType = string.Join(", ", item.CustomerRequestType),
                        CustomerRequestPriority = item.CustomerRequestPriority,
                        CustomerRequestRefundType = string.Join(", ", item.CustomerRequestRefund),
                        Amount = item.Amount,
                        CustomerReqRefundCurrency = item.CustomerReqRefundCurrency,
                        Remarks = item.Remarks,
                        InspectionDate = item.ServiceDateFrom == item.ServiceDateTo ? item.ServiceDateFrom.ToString(StandardDateFormat) : string.Join(" - ", item.ServiceDateFrom.ToString(StandardDateFormat), item.ServiceDateTo.ToString(StandardDateFormat)),
                        InspectorName = string.Join(", ", item.InspectorName),
                        ProductName = string.Join(", ", item.ProductName),
                        ProductCategory = item.ProductCategory,
                        ProductSubCategory = item.ProductSubCategory,
                        Color = item.Color,
                        OrderQty = item.OrderQty,
                        InspectedQty = item.InspectedQty,
                        FobValue = item.FobPrice,
                        FobCurrency = item.FobCurrency,
                        RetailValue = item.RetailPrice,
                        RetailCurrency = item.RetailCurrency,
                        AnalyzerFeedback = item.AnalyzerFeedback,
                        ValidatorClaimResult = item.ValidatorClaimResult,
                        ValidatorRecommendation = item.ClaimRecommendation,
                        ValidatorReviewComment = item.ValidatorReviewComment,
                        InvoiceNumber = item.InvoiceNumber,
                        InvoiceAmount = item.InvoiceAmount,
                        InvoiceCurrency = item.InvoiceCurrency,
                        InvoiceDate = item.InvoiceDate?.ToString(StandardDateFormat),
                        RealInspectionFees = item.RealInspectionFees,
                        RealInspectionFeesCurrency = item.RealInspectionFeesCurrency,
                        FinalDecision = string.Join(", ", item.FinalDecision),
                        RefundAmount = item.FinalAmount,
                        FinalRefundType = string.Join(", ", item.FinalRefundType),
                        RefundCurrency = item.FinalCurrency,
                        FinalDecisionRemarks = item.FinalDecisionRemarks,
                        CreatedDate = item.CreatedDate?.ToString(StandardDateFormat),
                        CreatedBy = item.CreatedBy,
                        AnalyzedDate = item.AnalyzedDate?.ToString(StandardDateFormat),
                        AnalyzedBy = item.AnalyzedBy,
                        ValidatedDate = item.ValidatedDate?.ToString(StandardDateFormat),
                        ValidatedBy = item.ValidatedBy,
                        ClosedDate = item.ClosedDate?.ToString(StandardDateFormat),
                        ClosedBy = item.ClosedBy
                    };
                    resultDataList.Add(objScheduleInfo);
                }

                return resultDataList;
            }
            catch (Exception ex) { return resultDataList; }
        }

        public InvoiceDetail MapClaimInvoiceDetails(InvoiceDetail entity)
        {
            if (entity == null)
                return null;

            return new InvoiceDetail
            {
                InvoiceNo = entity.InvoiceNo,
                InvoiceDate = entity.BilledDate?.ToString(StandardDateFormat),
                TotalInvoiceFees = entity.TotalInvoiceFees,
                InvoiceCurrency = entity.InvoiceCurrency,
                InvoiceCurrencyName = entity.InvoiceCurrencyName
            };
        }

        public PendingClaimResponse MapPendingClaimSummary(PendingClaimRepoItem item, IEnumerable<ReportProducts> products, IEnumerable<InvAutTranDetail> invoices, IEnumerable<DTO.Report.ServiceTypeList> serviceTypeList)
        {
            var invoice = invoices.FirstOrDefault(x => x.InspectionId == item.InspectionId);
            var bookingProducts = products.Where(x => x.BookingId == item.InspectionId).ToList();
            return new PendingClaimResponse()
            {
                CustomerId = item.CustomerId,
                ClaimId = item.ClaimId,
                BookingNo = item.InspectionId,
                InvoiceNo = invoice?.InvoiceNo,
                Office = item.Office,
                ProductCategory = string.Join(", ", bookingProducts.Select(x => x.ProductCategoryName).Distinct().ToList()),
                ProductSubCategory = string.Join(", ", bookingProducts.Select(x => x.ProductSubCategoryName).Distinct().ToList()),
                ProductSubCategory2 = string.Join(", ", bookingProducts.Select(x => x.ProductSubCategory2Name).Distinct().ToList()),
                ClaimDate = item.ClaimDate?.ToString(StandardDateFormat),
                Customer = item.Customer,
                ServiceDate = item.ServiceDate?.ToString(StandardDateFormat),
                ServiceType = serviceTypeList.FirstOrDefault(x => x.InspectionId == item.InspectionId)?.serviceTypeName,
                Product = string.Join(",", products.Where(x => x.BookingId == item.InspectionId).Select(y => y.ProductName))
            };
        }

        public InvCreTransaction MapCreditEntity(SaveCreditNote request, int createdBy, int? entityId)
        {
            return new InvCreTransaction()
            {
                Active = true,
                BankId = request.BankId,
                BilledAddress = request.BilledAddress,
                BillTo = request.BilledTo,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                CreditDate = request.CreditDate.ToDateTime(),
                CreditNo = request.CreditNo,
                CreditTypeId = request.CreditTypeId,
                Currency = request.CurrencyId,
                Office = request.OfficeId,
                PaymentTerms = request.PaymentTerms,
                PaymentDuration = request.PaymentDuration,
                Subject = request.Subject,
                PostDate = request.PostDate.ToNullableDateTime(),
                EntityId = entityId
            };
        }
        public InvCreTranClaimDetail MapInvCreTranClaimDetailEntity(SaveCreditNoteItem item, int createdBy)
        {
            return new InvCreTranClaimDetail()
            {
                Active = true,
                ClaimId = item.ClaimId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                InspectionId = item.InspectionId,
                InvoiceId = item.InvoiceId,
                RefundAmount = item.RefundAmount,
                Remarks = item.Remarks,
                SortAmount = item.SortAmount
            };
        }
        public GetPendingClaimData MapGetPendingClaimData(PendingClaimRepoItem item, IEnumerable<ReportProducts> products, List<CreditNoteInvoiceDetails> invoices)
        {
            var invoice = invoices.FirstOrDefault(x => x.InspectionId == item.InspectionId);
            var bookingProducts = products.Where(x => x.BookingId == item.InspectionId);
            return new GetPendingClaimData()
            {
                BookingNo = item.InspectionId,
                InvoiceNo = invoice?.InvoiceNo,
                Office = item.Office,
                ProductCategory = string.Join(", ", bookingProducts?.Select(x => x.ProductCategoryName).Distinct().ToList()),
                ProductSubCategory = string.Join(", ", bookingProducts?.Select(x => x.ProductSubCategoryName).Distinct().ToList()),
                ProductSubCategory2 = string.Join(", ", bookingProducts?.Select(x => x.ProductSubCategory2Name).Distinct().ToList()),
                ClaimId = item.ClaimId,
                ClaimNo = item.ClaimNo,
                InspectionDate = item.ServiceDate?.ToString(StandardDateFormat),
                InspectionFee = invoice?.InspectionFees,
                InspectionId = item.InspectionId.Value,
                InvoiceId = invoice.InvoiceId,
                Product = bookingProducts.Select(x => x.ProductName).Distinct().ToList(),
                Currency = invoice.CurrencyCode
            };
        }

        public CreditNoteSummaryItem MapCreditNoteSummary(CreditNoteSummaryRepoItem item, List<CreditNoteDetailsRepoItem> creditClaims)
        {
            var credits = creditClaims.Where(x => x.CreditId == item.Id);
            return new CreditNoteSummaryItem()
            {
                Id = item.Id,
                BillTo = item.BillTo,
                CustomerId = credits.Any() ? credits.FirstOrDefault().CustomerId : null,
                CreatedBy = item.CreatedBy,
                CreatedOn = item.CreatedOn?.ToString(StandardDateFormat),
                CreditNo = item.CreditNo,
                InspectionNo = string.Join(", ", credits.Select(x => x.InspectionId)),
                CreditType = item.CreditType,
                Currency = item.Currency,
                PostDate = item?.PostDate?.ToString(StandardDateFormat),
                CreditDate = item.CreditDate?.ToString(StandardDateFormat),
                RefundTotal = credits.Sum(x => x.RefundAmount),
                SortTotal = credits.Sum(x => x.SortAmount),
            };
        }

        public SaveCreditNote MapEditCreditNote(InvCreTransaction entity, IEnumerable<int?> contactPersons, int customerId)
        {
            return new SaveCreditNote()
            {
                CustomerId = customerId,
                Id = entity.Id,
                BankId = entity.BankId.Value,
                BilledAddress = entity.BilledAddress,
                BilledTo = entity.BillTo,
                ContactPersons = contactPersons,
                CreditDate = entity.CreditDate?.GetCustomDate(),
                CreditNo = entity.CreditNo,
                CurrencyId = entity.Currency,
                CreditTypeId = entity.CreditTypeId,
                PaymentDuration = entity.PaymentDuration,
                OfficeId = entity.Office,
                PaymentTerms = entity.PaymentTerms,
                PostDate = entity.PostDate?.GetCustomDate(),
                Subject = entity.Subject,
            };
        }
        public EditCreditNoteItem MapEditCreditNoteItem(CreditNoteClaimRepoItem claimDetails, IEnumerable<ReportProducts> products)
        {
            var bookingProducts = products.Where(x => x.BookingId == claimDetails.InspectionId).ToList();
            return new EditCreditNoteItem()
            {
                Id = claimDetails.Id,
                ClaimId = claimDetails.ClaimId,
                ClaimNo = claimDetails.ClaimNo,
                InspectionDate = claimDetails.InspectionDate?.ToString(StandardDateFormat),
                InspectionFee = claimDetails.InspectionFee,
                InspectionId = claimDetails.InspectionId,
                InvoiceId = claimDetails.InvoiceId,
                InvoiceNo = claimDetails.InvoiceNo,
                Office = claimDetails.Office,
                ProductCategory = string.Join(", ", bookingProducts.Select(x => x.ProductCategoryName).Distinct().ToList()),
                ProductSubCategory = string.Join(", ", bookingProducts.Select(x => x.ProductSubCategoryName).Distinct().ToList()),
                ProductSubCategory2 = string.Join(", ", bookingProducts.Select(x => x.ProductSubCategory2Name).Distinct().ToList()),
                RefundAmount = claimDetails.RefundAmount,
                Remarks = claimDetails.Remarks,
                SortAmount = claimDetails.SortAmount,
                Product = products.Where(x => x.BookingId == claimDetails.InspectionId).Select(y => y.ProductName),
                Currency = claimDetails.Currency
            };
        }

        public void MapCreditNoteEntity(InvCreTransaction entity, SaveCreditNote request, int userId)
        {
            entity.BankId = request.BankId;
            entity.Subject = request.Subject;
            entity.BilledAddress = request.BilledAddress;
            entity.BillTo = request.BilledTo;
            entity.CreditDate = request.CreditDate.ToNullableDateTime();
            entity.CreditNo = request.CreditNo;
            entity.CreditTypeId = request.CreditTypeId;
            entity.Currency = request.CurrencyId;
            //entity.InvoiceAdrress = request.InvoiceAddress;
            entity.Office = request.OfficeId;
            entity.PaymentDuration = request.PaymentDuration;
            entity.PaymentTerms = request.PaymentTerms;
            entity.PostDate = request.PostDate.ToNullableDateTime();
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
        }
        public InvCreTranClaimDetail MapCreditNoteItemEntity(SaveCreditNoteItem item, int userId)
        {
            return new InvCreTranClaimDetail()
            {
                Active = true,
                ClaimId = item.ClaimId,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                InspectionId = item.InspectionId,
                InvoiceId = item.InvoiceId,
                RefundAmount = item.RefundAmount,
                SortAmount = item.SortAmount,
                Remarks = item.Remarks,
            };
        }
        public void MapCreditNoteItemEntity(InvCreTranClaimDetail entity, SaveCreditNoteItem request, int userId)
        {
            entity.RefundAmount = request.RefundAmount;
            entity.SortAmount = request.SortAmount;
            entity.Remarks = request.Remarks;
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
        }

        public InvCreTranContact MapCreditContactEntity(int? contactPersonId, int userId)
        {
            return new InvCreTranContact()
            {
                Active = true,
                CreatedBy = userId,
                CustomerContactId = contactPersonId,
                CreatedOn = DateTime.Now
            };
        }

        public ExportCreditNoteSummary MapExportCreditNoteSummary(ExportCreditNoteSummaryRepoItem item)
        {
            return new ExportCreditNoteSummary()
            {
                Bank = item.Bank,
                BillTo = item.BillTo,
                Category = item.Category,
                CreatedBy = item.CreatedBy,
                CreatedOn = item.CreatedOn,
                CreditDate = item.CreditDate,
                CreditNo = item.CreditNo,
                CreditType = item.CreditType,
                Currency = item.Currency,
                RefundAmount = item.RefundAmount,
                ClaimNo = item.ClaimNo,
                InspectionFee = item.InspectionFee,
                InspectionFeeCurrency = item.InspectionFeeCurrency,
                InspectionNo = item.InspectionNo,
                InvoiceNo = item.InvoiceNo,
                Remark = item.Remark,
                SortAmount = item.SortAmount,
                Office = item.Office,
                PaymentDuration = item.PaymentDuration,
                PaymentTerms = item.PaymentTerms,
                PostDate = item.PostDate,
                ServiceFromDate = item.ServiceFromDate,
                ServiceToDate = item.ServiceToDate,
                SubCategory = item.SubCategory,
            };
        }
    }
}

