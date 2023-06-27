using DTO.Common;
using DTO.Invoice;
using DTO.SaleInvoice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.Maps
{
    public class SaleInvoiceMap : ApiCommonData
    {
        public List<SaleInvoiceSummary> MapSaleInvoiceSummary(List<InvoiceSummaryItem> invoiceDataList, List<InvoiceExtraFeeItem> invoiceExtraFeeList, List<SaleInvoiceFile> invoiceFiles)
        {
            var response = new List<SaleInvoiceSummary>();
            var invoiceIdList = invoiceDataList.Select(x => x.InvoiceNo).Distinct().ToList();

            foreach (var invoiceId in invoiceIdList)
            {
                var invoiceData = invoiceDataList.FirstOrDefault(x => x.InvoiceNo == invoiceId);
                var invoiceFile = invoiceFiles.FirstOrDefault(x => x.InvoiceNo == invoiceId);
                var invoiceBookingList = invoiceDataList.Where(x => x.InvoiceNo == invoiceId).ToList();
                var invoiceExtraFeeData = invoiceExtraFeeList.Where(x => x.InvoiceNo == invoiceId).ToList();

                if (invoiceData != null)
                {
                    response.Add(new SaleInvoiceSummary()
                    {
                        Id = invoiceData.Id,
                        InvoiceNo = invoiceId,
                        InvoicedName = invoiceData.InvoicedName,
                        InvoiceDate = invoiceData.InvoiceDate?.ToString(StandardDateFormat),
                        InvoiceCurrency = invoiceData.InvoiceCurrency,
                        TotalFee = Math.Round(invoiceBookingList.Sum(x => x.TotalFee.GetValueOrDefault()) + invoiceExtraFeeData.Where(x => x.InvoiceTo == invoiceData.InvoiceToId).Sum(x => x.TotalExtraFees.GetValueOrDefault()), 2, MidpointRounding.AwayFromZero),
                        PaymentStatusId = invoiceData.PaymentStatusId,
                        PaymentStatusName = invoiceData.PaymentStatusName,
                        PaymentDate = invoiceData.PaymentDate?.ToString(StandardDateFormat),
                        UniqueId = invoiceFile?.UniqueId
                    });
                }
            }

            return response;
        }

        public List<ExportSalesInvoiceForInternalUserData> MapExportSalesInvoiceForInternalUserSummary(List<InvoiceSummaryItem> invoiceDataList, List<InvoiceExtraFeeItem> invoiceExtraFeeList)
        {
            var response = new List<ExportSalesInvoiceForInternalUserData>();
            var invoiceIdList = invoiceDataList.Select(x => x.InvoiceNo).Distinct().ToList();

            foreach (var invoiceId in invoiceIdList)
            {
                var invoiceData = invoiceDataList.FirstOrDefault(x => x.InvoiceNo == invoiceId);
                var invoiceBookingList = invoiceDataList.Where(x => x.InvoiceNo == invoiceId).ToList();
                var invoiceExtraFeeData = invoiceExtraFeeList.Where(x => x.InvoiceNo == invoiceId).ToList();

                response.Add(new ExportSalesInvoiceForInternalUserData()
                {
                    InvoiceNo = invoiceId,
                    InvoiceName = invoiceData.InvoicedName,
                    InvoiceDate = invoiceData.InvoiceDate?.ToString(StandardDateFormat),
                    InvoiceCurrency = invoiceData.InvoiceCurrency,
                    TotalFee = Math.Round(invoiceBookingList.Sum(x => x.TotalFee.GetValueOrDefault()) + invoiceExtraFeeData.Where(x => x.InvoiceTo == invoiceData.InvoiceToId).Sum(x => x.TotalExtraFees.GetValueOrDefault()), 2, MidpointRounding.AwayFromZero),
                    PaymentStatusName = invoiceData.PaymentStatusName,
                    PaymentDate = invoiceData.PaymentDate?.ToString(StandardDateFormat)
                });
            }

            return response;

        }

        public List<ExportSalesInvoiceForExternalUserData> MapExportSalesInvoiceForExternalUsersSummary(List<InvoiceSummaryItem> invoiceDataList, List<InvoiceExtraFeeItem> invoiceExtraFeeList)
        {
            var response = new List<ExportSalesInvoiceForExternalUserData>();
            var invoiceIdList = invoiceDataList.Select(x => x.InvoiceNo).Distinct().ToList();

            foreach (var invoiceId in invoiceIdList)
            {
                var invoiceData = invoiceDataList.FirstOrDefault(x => x.InvoiceNo == invoiceId);
                var invoiceBookingList = invoiceDataList.Where(x => x.InvoiceNo == invoiceId).ToList();
                var invoiceExtraFeeData = invoiceExtraFeeList.Where(x => x.InvoiceNo == invoiceId).ToList();

                response.Add(new ExportSalesInvoiceForExternalUserData()
                {
                    InvoiceNo = invoiceId,
                    InvoiceDate = invoiceData.InvoiceDate?.ToString(StandardDateFormat),
                    InvoiceCurrency = invoiceData.InvoiceCurrency,
                    TotalFee = Math.Round(invoiceBookingList.Sum(x => x.TotalFee.GetValueOrDefault()) + invoiceExtraFeeData.Where(x => x.InvoiceTo == invoiceData.InvoiceToId).Sum(x => x.TotalExtraFees.GetValueOrDefault()), 2, MidpointRounding.AwayFromZero),
                    PaymentStatusName = invoiceData.PaymentStatusName,
                    PaymentDate = invoiceData.PaymentDate?.ToString(StandardDateFormat)
                });
            }

            return response;

        }
    }
}
