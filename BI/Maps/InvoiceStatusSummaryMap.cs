using DTO.Common;
using DTO.Inspection;
using DTO.Invoice;
using DTO.Report;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.Maps
{
    public class InvoiceStatusSummaryMap : ApiCommonData
    {
        public List<InvoiceStatusSummary> MapInvoiceStatusBookingSummary(InvoiceStatusSummaryResult data)
        {
            var response = new List<InvoiceStatusSummary>();
            var _serviceTypeName = "";
            string _serviceDate = "";

            foreach (var invoiceData in data.invoiceStatusDataList)
            {
                var serviceTypedata = data.serviceTypeList.FirstOrDefault(x => x.InspectionId == invoiceData.BookingId);
                if (serviceTypedata != null)
                    _serviceTypeName = serviceTypedata.serviceTypeName;

                var quotationdata = data.quotationList?.FirstOrDefault(x => x.BookingNo == invoiceData.BookingId);

                _serviceDate = invoiceData.BookingServiceDateFrom == invoiceData.BookingServiceDateTo ?
                                        invoiceData.BookingServiceDateFrom.ToString(StandardDateFormat) :
                                        string.Join(" - ", invoiceData.BookingServiceDateFrom.ToString(StandardDateFormat),
                                        invoiceData.BookingServiceDateTo.ToString(StandardDateFormat));

                var invoiceItem = data.InvoiceList?.FirstOrDefault(x => x.BookingId == invoiceData.BookingId);

                var serviceName = invoiceItem?.ServiceName ?? (data.ServiceId == (int)Service.InspectionId ? Service_Inspection : data.ServiceId == (int)Service.AuditId ? Service_Audit : "");
                var holdType = string.Empty;
                var holdReason = string.Empty;
                if (data.InspectionHoldReasons != null)
                {
                    var holdReasons = data.InspectionHoldReasons.FirstOrDefault(x => x.InspectionId == invoiceData.BookingId);
                    if (holdReasons != null)
                    {
                        holdType = holdReasons.Reason;
                        holdReason = holdReasons.Comment;
                    }
                }

                var brandBooking = data?.BookingBrandList?.Where(x => x.BookingId == invoiceData.BookingId).Select(x => x.BrandName);

                var invoiceStatus = new InvoiceStatusSummary()
                {
                    Id = invoiceItem != null ? invoiceItem.Id : 0,
                    InvoiceNo = invoiceItem?.InvoiceNo,
                    BookingId = invoiceData.BookingId,
                    ServiceDate = _serviceDate,
                    ServiceId = invoiceItem?.ServiceId ?? data.ServiceId,
                    ServiceName = serviceName,
                    CustomerId = invoiceData.CustomerId,
                    SupplierId = invoiceData.SupplierId.GetValueOrDefault(),
                    FactoryId = invoiceData.FactoryId,
                    CustomerName = invoiceData.CustomerName,
                    SupplierName = invoiceData.SupplierName,
                    FactoryName = invoiceData.FactoryName,
                    InvoiceTo = invoiceItem?.InvoiceTo,
                    InvoiceTypeId = invoiceItem?.InvoiceTypeId,
                    InvoiceTypeName = invoiceItem?.InvoiceTypeName,
                    InvoiceStatusName = invoiceItem?.StatusName,
                    InvoiceStatusColor = invoiceItem != null ? InvoiceStatusColor.GetValueOrDefault(invoiceItem.StatusId.GetValueOrDefault(), "") : "",
                    InvoiceDate = invoiceItem?.InvoiceDate?.ToString(StandardDateFormat),
                    IsInspection = invoiceItem?.IsInspection,
                    PaymentStatusId = invoiceItem?.PaymentStatusId,
                    PaymentStatusName = invoiceItem?.PaymentStatusName,
                    PaymentDate = invoiceItem?.PaymentDate?.ToString(StandardDateFormat),
                    Service = _serviceTypeName,
                    HoldType = holdType,
                    HoldReason = holdReason,
                    ServiceStartDate = invoiceData.BookingServiceDateFrom.GetCustomDate(),
                    ServiceEndDate = invoiceData.BookingServiceDateTo.GetCustomDate(),
                    BookingStatusId = invoiceData.StatusId,
                    BookingStatusName = invoiceData.StatusName,
                    FactoryCountry = invoiceData.FactoryCountry,
                    BrandNames = brandBooking != null ? string.Join(", ", brandBooking) : string.Empty,
                    CurrencyCode = invoiceItem?.CurrencyCode,
                    InvoiceAmount = invoiceItem?.InvoiceAmount,
                };

                var extraFeeItems = data.ExtraFeesInvoiceList.Where(x => x.BookingId == invoiceData.BookingId).ToList();
                if (extraFeeItems.Any())
                {
                    invoiceStatus.ExtraFeeInvoiceNo = string.Join(", ", extraFeeItems.Select(x => x.InvoiceNo).ToList());
                    invoiceStatus.ExtraFeesStatusName = string.Join(", ", extraFeeItems.Select(x => x.StatusName).ToList());
                    invoiceStatus.ExtraFeesAmount = extraFeeItems.Sum(x => x.InvoiceAmount);
                    invoiceStatus.ExtraFeesCurrencyCode = string.Join(", ", extraFeeItems.Select(x => x.CurrencyCode).ToList());
                    invoiceStatus.ExtraFeesInvoiceDate = string.Join(", ", extraFeeItems.Select(x => x.InvoiceDate?.ToString(StandardDateFormat)).ToList());
                    invoiceStatus.ExtraFeesInvoiceTo = string.Join(", ", extraFeeItems.Select(x => x.InvoiceTo).ToList());
                    invoiceStatus.ExtraFeesPaymentStatusName = string.Join(", ", extraFeeItems.Select(x => x.PaymentStatusName).ToList());
                    invoiceStatus.ExtraFeesPaymentDate = string.Join(", ", extraFeeItems.Select(x => x.PaymentDate?.ToString(StandardDateFormat)).ToList());
                }
                if (quotationdata != null)
                {
                    var _factoryContactId = data.FactoryContactIdList?.Where(x => x.FactoryId == quotationdata.FactoryId).Select(x => x.FactoryContactId).FirstOrDefault();
                    var _supplierContactId = data.SupplierContactIdList?.Where(x => x.SupplierId == quotationdata.SupplierId).Select(x => x.SupplierContactId).FirstOrDefault();
                    var _supplierAddress = data.SupplierAddressIdList?.Where(x => x.SupplierId == quotationdata.SupplierId).Select(x => x.SupplierAddress).FirstOrDefault();


                    invoiceStatus.QuotationId = quotationdata.QuotationNo;
                    invoiceStatus.PaymentTerms = quotationdata.PaymentTermsId.GetValueOrDefault();
                    invoiceStatus.BillTo = quotationdata.BillTo;

                    invoiceStatus.QuotationStatus = quotationdata.QuotationStatus;
                    invoiceStatus.QuotationStatusId = quotationdata.QuotationStatusId;

                    invoiceStatus.QuotationSupplierId = quotationdata.SupplierId;
                    invoiceStatus.QuotationSupplierName = quotationdata.SupplierName;
                    invoiceStatus.QuotationSupplierAddress = _supplierAddress;
                    if (_supplierContactId > 0)
                        invoiceStatus.QuotationSupplierContacts = new List<int> { _supplierContactId.GetValueOrDefault() };


                    invoiceStatus.QuotationFactoryId = quotationdata.FactoryId;
                    invoiceStatus.QuotationFactoryName = quotationdata.FactoryName;
                    invoiceStatus.QuotationFactoryAddress = quotationdata.FactoryAddress;
                    if (_factoryContactId > 0)
                        invoiceStatus.QuotationFactoryContacts = new List<int>() { _factoryContactId.GetValueOrDefault() };

                    invoiceStatus.QuotationBilledName = quotationdata.QuotationBilledName;
                    invoiceStatus.QuotationCurrencyId = quotationdata.QuotationCurrencyId;
                    invoiceStatus.QuotationCurrencyName = quotationdata.QuotationCurrencyName;
                    invoiceStatus.QuotationCurrencyCode = quotationdata.QuotationCurrencyCode;
                    invoiceStatus.BillingEntityId = quotationdata.BillingEntityId;
                    invoiceStatus.BillingEntityName = quotationdata.BillingEntityName;
                    invoiceStatus.BankcurrencyId = quotationdata.BankcurrencyId;
                    invoiceStatus.BankcurrencyName = quotationdata.BankcurrencyName;
                    invoiceStatus.BankId = quotationdata.BankId;
                    invoiceStatus.BankName = quotationdata.BankName;

                    // set total fees for only one booking - if we have multiple quotation
                    if (response.Count == 0 || !response.Any(x => x.QuotationId == quotationdata.QuotationNo))
                    {
                        invoiceStatus.QuotationTotalFees = quotationdata.QuotationTotalFees;
                    }
                    else
                    {
                        invoiceStatus.QuotationTotalFees = 0;
                    }

                    invoiceStatus.TaxList = quotationdata.TaxList;
                    invoiceStatus.CustomerLegalName = quotationdata.CustomerLegalName;
                    invoiceStatus.SupplierLegalName = quotationdata.SupplierLegalName;
                    invoiceStatus.FactoryLegalName = quotationdata.FactoryLegalName;
                }

                response.Add(invoiceStatus);
            }
            return response.OrderBy(x => x.QuotationId).ThenByDescending(x => x.QuotationTotalFees).ToList();
        }
        public static List<ExportInvoiceStatus> MapExportInvoiceStatusSummary(InvoiceStatusSummaryResult summaryResponse, List<InvoiceCommunicationTableRepo> invoiceCommunicationData)
        {

            List<InvoiceStatusSummaryItem> invoiceDataList = summaryResponse.invoiceStatusDataList;
            IEnumerable<ServiceTypeList> serviceTypeList = summaryResponse.serviceTypeList;
            List<InvoiceBookingQuotation> quotationList = summaryResponse.quotationList;
            List<InvoiceItem> invoiceList = summaryResponse.InvoiceList;
            List<BookingBrandAccess> bookingBrandList = summaryResponse.BookingBrandList;

            var response = new List<ExportInvoiceStatus>();
            var _serviceTypeName = "";
            string _serviceDate = "";
            foreach (var invoiceData in invoiceDataList)
            {
                var serviceTypedata = serviceTypeList.FirstOrDefault(x => x.InspectionId == invoiceData.BookingId);
                if (serviceTypedata != null)
                    _serviceTypeName = serviceTypedata.serviceTypeName;

                var quotationdata = quotationList?.FirstOrDefault(x => x.BookingNo == invoiceData.BookingId);
                _serviceDate = invoiceData.BookingServiceDateFrom == invoiceData.BookingServiceDateTo ?
                                        invoiceData.BookingServiceDateFrom.ToString(StandardDateFormat) :
                                        string.Join(" - ", invoiceData.BookingServiceDateFrom.ToString(StandardDateFormat),
                                        invoiceData.BookingServiceDateTo.ToString(StandardDateFormat));

                var invoiceItem = invoiceList.FirstOrDefault(x => x.BookingId == invoiceData.BookingId);

                var brandBooking = bookingBrandList?.Where(x => x.BookingId == invoiceData.BookingId).Select(x => x.BrandName);

                var invoiceCommunication = invoiceCommunicationData?.Where(x => x.InvoiceNumber == invoiceItem?.InvoiceNo)
                    .Select(x => x.Comment).ToList();

                var invoiceStatus = new ExportInvoiceStatus()
                {
                    InvoiceNo = invoiceItem?.InvoiceNo,
                    BookingId = invoiceData.BookingId,
                    QuotationId = quotationdata?.QuotationNo,
                    ServiceDate = _serviceDate,
                    ServiceName = invoiceItem?.ServiceName,
                    CustomerName = invoiceData.CustomerName,
                    SupplierName = invoiceData.SupplierName,
                    FactoryName = invoiceData.FactoryName,
                    InvoiceTo = invoiceItem?.InvoiceTo,
                    InvoiceTypeName = invoiceItem?.InvoiceTypeName,
                    StatusName = invoiceItem?.StatusName,
                    PaymentStatusName = invoiceItem?.PaymentStatusName,
                    PaymentDate = invoiceItem?.PaymentDate,
                    QuotationStatus = quotationdata?.QuotationStatus,
                    FactoryCountry = invoiceData.FactoryCountry,
                    InvoiceAmount = invoiceItem?.InvoiceAmount,
                    CurrencyCode = invoiceItem?.CurrencyCode,
                    InvoiceDate = invoiceItem?.InvoiceDate?.ToString(StandardDateFormat),
                    BrandNames = brandBooking != null ? string.Join(", ", brandBooking) : "",
                    Communication = string.Join("| ", invoiceCommunication)
                };

                var extraFeeItems = summaryResponse.ExtraFeesInvoiceList.Where(x => x.BookingId == invoiceData.BookingId).ToList();

                if (extraFeeItems.Any())
                {
                    invoiceStatus.ExtraFeeInvoiceNo = string.Join(", ", extraFeeItems.Select(x => x.InvoiceNo).Distinct().ToList());
                    invoiceStatus.ExtraFeesStatusName = string.Join(", ", extraFeeItems.Select(x => x.StatusName).Distinct().ToList());
                    invoiceStatus.ExtraFeesAmount = extraFeeItems.Sum(x => x.InvoiceAmount);
                    invoiceStatus.ExtraFeesInvoiceDate = string.Join(", ", extraFeeItems.Select(x => x.InvoiceDate?.ToString(StandardDateFormat)).Distinct().ToList());
                    invoiceStatus.ExtraFeesInvoiceTo = string.Join(", ", extraFeeItems.Select(x => x.InvoiceTo).Distinct().ToList());
                    invoiceStatus.ExtraFeesPaymentStatusName = string.Join(", ", extraFeeItems.Select(x => x.PaymentStatusName).Distinct().ToList());
                    invoiceStatus.ExtraFeesPaymentDate = string.Join(", ", extraFeeItems.Select(x => x.PaymentDate?.ToString(StandardDateFormat)).Distinct().ToList());
                    invoiceStatus.ExtraFeesCurrency = string.Join(", ", extraFeeItems.Select(x => x.CurrencyCode).Distinct().ToList());
                }

                response.Add(invoiceStatus);
            }
            return response;
        }

        /// <summary>
        /// map the invoice data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static InvoiceCommunicationTable MapInvoiceCommunication(InvoiceCommunicationTableRepo item)
        {
            return new InvoiceCommunicationTable()
            {
                Comment = item.Comment,
                CreatedBy = item.CreatedBy,
                CreatedOn = item.CreatedOn.Value.ToString(StandardDateFormat)
            };
        }
    }
}
