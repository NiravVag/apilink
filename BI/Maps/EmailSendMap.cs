using DTO.Common;
using DTO.CommonClass;
using DTO.EmailSend;
using DTO.Inspection;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public class EmailSendMap : ApiCommonData
    {
        public EmailSendSummaryItem EmailSendSummaryMap(InspectionBookingItems entity, IEnumerable<DTO.Report.ServiceTypeList> serviceTypeList,
            List<BookingReportData> reportData, List<LogEmailSuccessReportCount> bookingEmailSuccessReportCount)
        {
            return new EmailSendSummaryItem
            {
                BookingId = entity.BookingId,
                CustomerBookingNo = entity.CustomerBookingNo,
                CustomerId = entity.CustomerId.GetValueOrDefault(),
                CustomerName = entity.CustomerName,
                SupplierId = entity.SupplierId.GetValueOrDefault(),
                SupplierName = entity.SupplierName,
                FactoryId = entity.FactoryId.GetValueOrDefault(),
                FactoryName = entity.FactoryName,
                Office = entity.Office,
                ServiceType = serviceTypeList.Where(x => x.InspectionId == entity.BookingId).Select(x => x.serviceTypeName).FirstOrDefault(),
                StatusId = entity.StatusId,
                ServiceDate = entity.ServiceDateFrom.Equals(entity.ServiceDateTo) ? entity.ServiceDateFrom.ToString(StandardDateFormat)
                                : String.Concat(entity.ServiceDateFrom.ToString(StandardDateFormat), " - ", entity.ServiceDateTo.ToString(StandardDateFormat)),
                ReportCount = reportData.Where(x => x.BookingId == entity.BookingId).Select(x => x.ReportId).Distinct().Count(),
                SuccessReportCount = bookingEmailSuccessReportCount.FirstOrDefault(x => x.InspectionId == entity.BookingId) != null ?
                                                    bookingEmailSuccessReportCount.FirstOrDefault(x => x.InspectionId == entity.BookingId).ReportCount : 0,
                ServiceFromDate = entity.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceToDate = entity.ServiceDateTo.ToString(StandardDateFormat)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingReportData"></param>
        /// <returns></returns>
        public EmailSend EmailSendBookingProductReportMap(BookingReportDetails bookingReportData, int productId, int bookingId)
        {
            var _reportId = bookingReportData.InspectionProductList.Where(x => x.ProductId == productId && x.BookingId == bookingId).Select(x => x.ReportId).FirstOrDefault();

            var reportData = bookingReportData.ReportDetailsList.FirstOrDefault(x => x.ReportId == _reportId);

            var combineProductId = bookingReportData.InspectionProductList.Where(x => x.ProductId == productId && x.CombineProductId > 0 && x.BookingId == bookingId).Select(x => x.CombineProductId).FirstOrDefault();
            var IscombineAqlQty = bookingReportData.InspectionProductList.Any(x => x.BookingId == bookingId && x.CombineProductId == combineProductId.GetValueOrDefault() && x.CombineAqlQuantity > 0);
            var combineAqlQuantity = bookingReportData.InspectionProductList.Where(x => x.ProductId == productId && x.CombineProductId > 0 && x.BookingId == bookingId).Select(x => x.CombineAqlQuantity).FirstOrDefault();
            var _isparent = combineProductId.GetValueOrDefault() == 0 || (combineAqlQuantity != null && combineAqlQuantity != 0) ||
                (combineProductId > 0 && combineAqlQuantity.GetValueOrDefault() == 0 && !IscombineAqlQty && bookingReportData.InspectionProductList.Where(x => x.BookingId == bookingId && x.CombineProductId == combineProductId).FirstOrDefault().ProductId == productId);
            var reportstatusid = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.ReportStatusId).FirstOrDefault();
            var reportlink = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.ReportLink).FirstOrDefault();
            var finalManualReportLink = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.FinalManualReportPath).FirstOrDefault();
            return new EmailSend()
            {
                BookingId = bookingId,
                POIds = string.Join(", ", bookingReportData.InspectionProductList.Where(x => x.ProductId == productId && x.BookingId == bookingId).Select(x => x.PoNumber).ToList()),
                ProductId = productId,
                POIdList = bookingReportData.InspectionProductList.Where(x => x.ProductId == productId && x.BookingId == bookingId).Select(x => x.PoNumber),

                POList = bookingReportData.InspectionProductList.Where(x => x.ProductId == productId && x.BookingId == bookingId).Select(x => new CommonDataSource
                {
                    Id = x.PoId,
                    Name = x.PoNumber
                }).ToList(),

                ProductName = bookingReportData.InspectionProductList.Where(x => x.ProductId == productId && x.BookingId == bookingId).Select(x => x.ProductName).FirstOrDefault(),
                ReportId = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.ReportId).FirstOrDefault(),
                ReportName = reportData?.ReportName,

                ReportLink = reportlink,
                FinalManualReportLink = finalManualReportLink,
                ReportResult = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.ReportResult).FirstOrDefault(),
                ReportStatus = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.ReportStatus).FirstOrDefault(),
                TotalBookingQuantity = bookingReportData.InspectionProductList.Where(x => x.ProductId == productId && x.BookingId == bookingId).Select(x => x.TotalBookingQty).Sum(),
                CombineProductId = bookingReportData.InspectionProductList.Where(x => x.ProductId == productId && x.BookingId == bookingId).Select(x => x.CombineProductId.GetValueOrDefault()).FirstOrDefault(),

                CombineProductCount = bookingReportData.InspectionProductList.Any(x => x.BookingId == bookingId && x.CombineProductId == combineProductId && x.CombineProductId > 0) ?
                bookingReportData.InspectionProductList.Where(x => x.BookingId == bookingId && x.CombineProductId == combineProductId && x.CombineProductId > 0 && x.ReportId == _reportId).Select(x => x.ProductId).Distinct().Count() : 1,

                IsParentProduct = _isparent,
                IsOkToSend = _isparent && reportstatusid.GetValueOrDefault() == (int)FBStatus.ReportValidated && (!string.IsNullOrEmpty(reportlink) || !string.IsNullOrEmpty(finalManualReportLink)),
                FbReportId = reportData?.FbReportId,
                ReportRevision = reportData?.ReportRevision,
                ReportSendRevision = reportData?.ReportSendRevisison,
                RequestedReportRevision = reportData?.RequestedReportRevision,
                ReportVersion = reportData?.ReportVersion,
                IsReportSend = reportData?.ReportSendCount > 0
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingReportData"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public EmailSend EmailSendBookingContainerReportMap(BookingReportDetails bookingReportData, int? containerId)
        {
            var _reportId = bookingReportData.InspectionContainerList.Where(x => x.ContainerId == containerId).Select(x => x.ReportId).FirstOrDefault();
            var reportstatusid = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.ReportStatusId).FirstOrDefault();
            var _reportlink = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.ReportLink).FirstOrDefault();
            var _finalManualReportlink = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.FinalManualReportPath).FirstOrDefault();
            return new EmailSend()
            {
                BookingId = bookingReportData.InspectionContainerList.Where(x => x.ContainerId == containerId).Select(x => x.BookingId).FirstOrDefault(),
                ContainerId = bookingReportData.InspectionContainerList.Where(x => x.ContainerId == containerId).Select(x => x.ContainerId).FirstOrDefault(),
                ReportId = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.ReportId).FirstOrDefault(),
                ReportLink = _reportlink,
                FinalManualReportLink = _finalManualReportlink,
                ReportResult = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.ReportResult).FirstOrDefault(),
                ReportStatus = bookingReportData.ReportDetailsList.Where(x => x.ReportId == _reportId).Select(x => x.ReportStatus).FirstOrDefault(),
                TotalBookingQuantity = bookingReportData.InspectionContainerList.Where(x => x.ContainerId == containerId).Select(x => x.TotalBookingQty).Sum(),
                IsParentProduct = true,
                CombineProductCount = 1,
                IsOkToSend = reportstatusid.GetValueOrDefault() == (int)FBStatus.ReportValidated && (!string.IsNullOrEmpty(_reportlink) || !string.IsNullOrEmpty(_finalManualReportlink))
                //ReportSend
            };
        }

        /// <summary>
        /// Map Report Send field
        /// </summary>
        /// <param name="emailSend"></param>
        /// <param name="EmailReportCount"></param>
        /// <returns></returns>
        public EmailSend EmailBookingReportSendMap(EmailSend emailSend, List<LogEmailReportCount> EmailReportCount)
        {
            emailSend.ReportSend = EmailReportCount.FirstOrDefault(x => x.InspectionId == emailSend.BookingId && x.ReportId == emailSend.ReportId) != null ?
                EmailReportCount.FirstOrDefault(x => x.InspectionId == emailSend.BookingId && x.ReportId == emailSend.ReportId).ReportCount : 0;
            return emailSend;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailSendDetails"></param>
        /// <returns></returns>
        public EmailSendFileDetails EmailSendFileMap(EmailSendFileDetailsRepo emailSendDetails)
        {
            return new EmailSendFileDetails()
            {
                BookingId = emailSendDetails.BookingId.GetValueOrDefault(),
                InvoiceId = emailSendDetails.InvoiceId,
                InvoiceNo = emailSendDetails.InvoiceNo,
                EmailSendFileId = emailSendDetails.EmailSendFileId,
                FileLink = emailSendDetails.FileLink,
                FileName = emailSendDetails.FileName,
                FileTypeId = emailSendDetails.FileTypeId.GetValueOrDefault(),
                ReportId = emailSendDetails.ReportId,
                FileTypeName = emailSendDetails.FileTypeName,
                ReportName = emailSendDetails?.ReportName
            };
        }

        public EmailRuleData EmailSendDetailsMap(EmailSendConfigDetails emailSendDetails)
        {
            return new EmailRuleData()
            {
                ApiResultName = string.Join(", ", emailSendDetails.ApiResultNameList),
                BrandName = string.Join(", ", emailSendDetails.BrandNameList),
                BuyerName = string.Join(", ", emailSendDetails.BuyerNameList),
                CollectionName = string.Join(", ", emailSendDetails.CollectionNameList),
                DepartmentName = string.Join(", ", emailSendDetails.DepartmentNameList),
                FactoryCountryName = string.Join(", ", emailSendDetails.FactoryCountryNameList),
                FactoryName = string.Join(", ", emailSendDetails.FactoryNameList),
                ProductCategoryName = string.Join(", ", emailSendDetails.ProductCategoryNameList),
                RuleId = emailSendDetails.Id,
                ServiceTypeName = string.Join(", ", emailSendDetails.ServiceTypeNameList),
                SpecialRuleName = string.Join(", ", emailSendDetails.SpecialRuleNameList),
                SupplierName = string.Join(", ", emailSendDetails.SupplierNameList),
                ReportInEmail = emailSendDetails.ReportInEmail,
                ReportSendType = emailSendDetails.ReportSendType
            };
        }
    }
}