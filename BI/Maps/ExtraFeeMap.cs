using DTO.Common;
using DTO.CommonClass;
using DTO.ExtraFees;
using DTO.Invoice;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.Maps
{
    public class ExtraFeeMap : ApiCommonData
    {
        public ExtraFeeSummaryResponseItem ExtraFeeSummaryMap(ExtraFeeSummaryItem entity, List<ExtraFeeSummaryItem> feeData)
        {
            return new ExtraFeeSummaryResponseItem
            {
                BookingId = entity.BookingId,
                CustomerBookingNo = entity.CustomerBookingNo,
                BilledTo = entity.BilledTo,
                ExFeeType = string.Join(", ", feeData.Select(x => x.ExFeeType).Distinct()),
                CustomerName = entity.CustomerName,
                SupplierName = entity.SupplierName,
                TotalAmt = entity.TotalAmt,
                Currency = entity.Currency,
                Service = entity.Service,
                InvoiceNumber = entity.InvoiceNo ?? "",
                ExtraFeeInvoiceNumber = entity.ExtraFeeInvoiceNo ?? "",
                Remarks = entity.Remarks ?? "",
                extrafeeid = entity.ExfTranId.GetValueOrDefault(),
                StatusId = entity.StatusId.GetValueOrDefault(),
                ExchangeRate = entity.ExchangeRate,
                InvoiceCurrency = entity.InvoiceCurrency
            };
        }
        public List<ExtraFeeSummaryExportItem> ExtraFeeSummaryExportMap(List<ExtraFeeSummaryItem> lstentity)
        {
            List<ExtraFeeSummaryExportItem> extrafeesitem = new List<ExtraFeeSummaryExportItem>();
            for (int i = 0; i < lstentity.Count; i++)
            {
                var entity = lstentity[i];
                var item = new ExtraFeeSummaryExportItem
                {
                    BookingId = entity.BookingId,
                    CustomerBookingNo = entity.CustomerBookingNo ?? "",
                    BilledTo = entity.BilledTo,
                    ExFeeType = entity.ExFeeType,
                    CustomerName = entity.CustomerName,
                    SupplierName = entity.SupplierName,
                    TotalAmt = entity.TotalAmt,
                    Currency = entity.Currency,
                    Service = entity.Service,
                    InvoiceNumber = entity.InvoiceNo ?? "",
                    ExtraFeeInvoiceNumber = entity.ExtraFeeInvoiceNo ?? "",
                    Remarks = entity.Remarks ?? "",
                    ExtraTypefee = (entity.ExtraTypefee * (entity.ExchangeRate.HasValue ? entity.ExchangeRate : 1)) ?? 0,
                    ExtraTypeRemarks = entity.ExtraTypeRemarks ?? "",
                    StatusName = entity.StatusName ?? string.Empty,
                    InvoiceDate = entity.InvoiceDate?.ToString(StandardDateFormat),
                    PaymentDate = entity.PaymentDate?.ToString(StandardDateFormat),
                    PaymentStatus = entity.PaymentStatus,
                    InvoiceCurrency = entity.InvoiceCurrency,
                    ExchangeRate = entity.ExchangeRate,
                    ExtraFeeId= entity.ExfTranId
                };
                extrafeesitem.Add(item);
            }
            return extrafeesitem;
        }

        // map the edit invoice data
        public EditExtraFees ExtraFeeEditMap(EditExtraFeesRepo entity)
        {
            List<int> contactIdList = new List<int>();
            List<CommonDataSource> contactList = new List<CommonDataSource>();
            int? bookingId = 0;

            if (entity.BilledToId == (int)InvoiceTo.Customer)
            {
                contactIdList = entity.ContactIdList.Where(x => x.CustomerContactId > 0).Select(x => x.CustomerContactId.GetValueOrDefault()).ToList();
                contactList = entity.ContactIdList.Where(x => x.CustomerContactId > 0).Select(x => new CommonDataSource
                {
                    Id = x.CustomerContactId.GetValueOrDefault(),
                    Name = x.CustomerContactName
                }).ToList();
            }

            else if (entity.BilledToId == (int)InvoiceTo.Supplier)
            {
                contactIdList = entity.ContactIdList.Where(x => x.SupContactId > 0).Select(x => x.SupContactId.GetValueOrDefault()).ToList();
                contactList = entity.ContactIdList.Where(x => x.SupContactId > 0).Select(x => new CommonDataSource
                {
                    Id = x.SupContactId.GetValueOrDefault(),
                    Name = x.SupContactName
                }).ToList();
            }

            else if (entity.BilledToId == (int)InvoiceTo.Factory)
            {
                contactIdList = entity.ContactIdList.Where(x => x.FactContactId > 0).Select(x => x.FactContactId.GetValueOrDefault()).ToList();
                contactList = entity.ContactIdList.Where(x => x.FactContactId > 0).Select(x => new CommonDataSource
                {
                    Id = x.FactContactId.GetValueOrDefault(),
                    Name = x.FactContactName
                }).ToList();
            }

            if (entity.ServiceId == (int)Service.InspectionId)
            {
                bookingId = entity.BookingNumberId;
            }
            else if (entity.ServiceId == (int)Service.AuditId)
            {
                bookingId = entity.AuditId;
            }

            return new EditExtraFees
            {
                BankId = entity.BankId,
                BilledToId = entity.BilledToId,                
                BilledName = entity.BilledName,
                BilledAddress = entity.BilledAddress,
                PaymentDuration = entity.PaymentDuration,
                PaymentTerms = entity.PaymentTerms,
                BillingEntityId = entity.BillingEntityId,
                BookingNumberId = bookingId,
                CurrencyId = entity.CurrencyId,
                CustomerId = entity.CustomerId,
                ExtraFeeInvoiceNo = entity.ExtraFeeInvoiceNo,
                FactoryId = entity.FactoryId,
                Id = entity.Id,
                InvoiceNumberId = entity.InvoiceNumberId,
                ExtraFeeTypeList = entity.ExtraFeeTypeList,
                PaymentDate = Static_Data_Common.GetCustomDate(entity.PaymentDate),
                PaymentStatusId = entity.PaymentStatusId,
                Remarks = entity.Remarks,
                ServiceId = entity.ServiceId,
                StatusId = entity.StatusId,
                StatusName = entity.StatusName,
                SubTotal = entity.SubTotal,
                SupplierId = entity.SupplierId,
                TaxAmt = entity.TaxAmt,
                TaxId = entity.TaxId,
                TaxValue = entity.TaxValue,
                TotalFees = entity.TotalFees,
                OfficeId = entity.OfficeId,
                ContactIdList = contactIdList,
                ContactList = contactList,
                ExtraFeeInvoiceDate = Static_Data_Common.GetCustomDate(entity.ExtraFeeInvoiceDate),
                InvoiceCurrencyId = entity.InvoiceCurrencyId,
                ExchangeRate = entity.ExchangeRate
            };
        }
    }
}
