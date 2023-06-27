using DTO.Common;
using DTO.Customer;
using DTO.Eaqf;
using DTO.Inspection;
using DTO.Invoice;
using DTO.MasterConfig;
using DTO.Quotation;
using DTO.Report;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class ManualInvoiceMap : ApiCommonData
    {
        public InvManTransaction MapManualInvoiceEntity(SaveManualInvoice request, int userId, int? entityId)
        {
            return new InvManTransaction
            {
                Status = (int)InvoiceStatus.Created,
                InvoiceDate = request.InvoiceDate.ToDateTime(),
                SupplierId = request.SupplierId,
                CustomerId = request.CustomerId,
                InvoiceToId = request.InvoiceTo,
                InvoiceNo = request.InvoiceNo,
                Attn = request.Attn,
                BilledName = request.BilledName,
                BilledAddress = request.BilledAddress,
                Email = request.Email,
                ServiceId = request.ServiceId,
                BookingNo = request.ServiceId == (int)Service.InspectionId ? request.BookingNo : null,
                AuditId = request.ServiceId == (int)Service.AuditId ? request.BookingNo : null,
                ServiceType = request.ServiceType,
                Currency = request.CurrencyId,
                PaymentTerms = request.PaymentTerms,
                BankId = request.BankId,
                OfficeId = request.OfficeId,
                PaymentDuration = request.PaymentDuration,
                FromDate = request.FromDate.ToNullableDateTime(),
                ToDate = request.ToDate.ToNullableDateTime(),
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                CountryId = request.CountryId,
                EntityId = entityId,
                PaymentMode = request.PaymentMode,
                PaymentRef = request.PaymentRef
            };
        }

        public InvManTranDetail MapManualInvoiceItemEntity(SaveManualInvoiceItem request, int userId)
        {
            var subTotal = request.ServiceFee.GetValueOrDefault() + request.EXPChargeBack.GetValueOrDefault() + request.OtherCost.GetValueOrDefault() - request.Discount.GetValueOrDefault();
            return new InvManTranDetail
            {
                Active = true,
                Description = request.Description,
                ServiceFee = request.ServiceFee,
                ExpChargeBack = request.EXPChargeBack,
                OtherCost = request.OtherCost,
                Remarks = request.Remarks,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                Discount = request.Discount,
                Manday = request.Manday,
                UnitPrice = request.UnitPrice,
                Subtotal = Math.Round(subTotal, InvoiceRoundUpValue, MidpointRounding.AwayFromZero)
            };
        }

        public void MapManualInvoiceEntity(InvManTransaction entity, SaveManualInvoice request, int userId)
        {
            entity.Status = (int)InvoiceStatus.Modified;
            entity.CountryId = request.CountryId;
            entity.InvoiceDate = request.InvoiceDate.ToDateTime();
            entity.CustomerId = request.CustomerId;
            entity.SupplierId = request.SupplierId;
            entity.InvoiceToId = request.InvoiceTo;
            entity.InvoiceNo = request.InvoiceNo;
            entity.Attn = request.Attn;
            entity.BilledName = request.BilledName;
            entity.BilledAddress = request.BilledAddress;
            entity.Email = request.Email;
            entity.ServiceId = request.ServiceId;
            entity.BookingNo = request.BookingNo;
            entity.ServiceType = request.ServiceType;
            entity.Currency = request.CurrencyId;
            entity.PaymentTerms = request.PaymentTerms;
            entity.BankId = request.BankId;
            entity.OfficeId = request.OfficeId;
            entity.PaymentDuration = request.PaymentDuration;
            entity.FromDate = request.FromDate.ToNullableDateTime();
            entity.ToDate = request.ToDate.ToNullableDateTime();
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
        }

        public void MapManualInvoiceItemEntity(InvManTranDetail entity, SaveManualInvoiceItem request, int userId)
        {
            var subTotal = request.ServiceFee + request.EXPChargeBack.GetValueOrDefault() + request.OtherCost.GetValueOrDefault() - request.Discount.GetValueOrDefault();
            entity.Active = true;
            entity.Description = request.Description;
            entity.ServiceFee = request.ServiceFee;
            entity.ExpChargeBack = request.EXPChargeBack;
            entity.OtherCost = request.OtherCost;
            entity.Discount = request.Discount;
            entity.Remarks = request.Remarks;
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
            entity.Subtotal = Math.Round(subTotal.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
        }
        public ManualInvoiceSummaryItem MapManualInvoiceSummary(ManualInvoiceItemRepo entity, IEnumerable<InvManTranDetail> items, IEnumerable<EaqfInvoiceDetails> invoiceFiles)
        {
            var invoiceItems = items.Where(x => x.InvManualId == entity.Id);

            string invoicePdfUrl = null;
            if (invoiceFiles != null)
            {
                invoicePdfUrl = invoiceFiles.FirstOrDefault(x => x.InvoiceNo == entity.InvoiceNo)?.InvoicePdfUrl;
            }

            return new ManualInvoiceSummaryItem()
            {
                Id = entity.Id,
                Attn = entity.Attn,
                CustomerId = entity.CustomerId,
                BilledName = entity.BilledName,
                Customer = entity.CustomerName,
                InvoiceDate = entity.InvoiceDate?.ToString(StandardDateFormat),
                InvoiceNo = entity.InvoiceNo,
                Service = entity.Service,
                ServiceType = entity.ServiceType,
                ServiceFee = Math.Round(invoiceItems.Sum(x => x.ServiceFee).GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                EXPChargeBack = Math.Round(invoiceItems.Sum(x => x.ExpChargeBack).GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                OtherCost = Math.Round(invoiceItems.Sum(x => x.OtherCost).GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                Tax = Math.Round(entity.Tax.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero) * 100,
                TaxAmount = Math.Round(entity.TaxAmount.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                TotalFee = Math.Round(entity.TotalFee.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                CreatedOn = entity.CreatedOn?.ToString(StandardDateFormat),
                CreatedBy = entity.CreatedBy,
                UpdatedOn = entity.UpdatedOn?.ToString(StandardDateFormat),
                UpdatedBy = entity.UpdatedBy,
                InvoicePdfUrl = invoicePdfUrl
            };
        }
        public SaveManualInvoice MapGetManualInvoice(InvManTransaction entity, IEnumerable<InvManTranDetail> items)
        {
            return new SaveManualInvoice()
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                BankId = entity.BankId,
                BilledAddress = entity.BilledAddress,
                BilledName = entity.BilledName,
                CurrencyId = entity.Currency.Value,
                Email = entity.Email,
                FromDate = entity.FromDate.Value.GetCustomDate(),
                ToDate = entity.ToDate.Value.GetCustomDate(),
                InvoiceItems = items.Select(x => MapGetManualInvoiceItem(x)),
                InvoiceNo = entity.InvoiceNo,
                OfficeId = entity.OfficeId,
                PaymentTerms = entity.PaymentTerms,
                ServiceId = entity.ServiceId,
                ServiceType = entity.ServiceType,
                PaymentDuration = entity.PaymentDuration,
                InvoiceTo = entity.InvoiceToId,
                Attn = entity.Attn,
                InvoiceDate = entity.InvoiceDate.GetCustomDate(),
                CountryId = entity.CountryId,
                SupplierId = entity.SupplierId,
                BookingNo = entity.BookingNo
            };
        }

        public SaveManualInvoiceItem MapGetManualInvoiceItem(InvManTranDetail entity)
        {
            return new SaveManualInvoiceItem()
            {
                Description = entity.Description,
                EXPChargeBack = entity.ExpChargeBack,
                Id = entity.Id,
                OtherCost = entity.OtherCost,
                Remarks = entity.Remarks,
                ServiceFee = entity.ServiceFee,
                UnitPrice = entity.UnitPrice,
                Manday = entity.Manday,
                Discount = entity.Discount
            };
        }

        public ManualInvoiceExportSummary MapManualInvoiceExportSummary(ManualInvoiceExportRepoItem entity)
        {
            var taxAmount = Math.Round(entity.SubTotal.GetValueOrDefault() * entity.Tax.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
            return new ManualInvoiceExportSummary()
            {
                Attn = entity.Attn,
                BilledName = entity.BilledName,
                CustomerName = entity.CustomerName,
                InvoiceDate = entity.InvoiceDate?.ToString(StandardDateFormat),
                InvoiceNo = entity.InvoiceNo,
                Service = entity.Service,
                ServiceType = entity.ServiceType,
                ServiceFee = Math.Round(entity.ServiceFee.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                ExpChargeBack = Math.Round(entity.ExpChargeBack.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                OtherCost = Math.Round(entity.OtherCost.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                Tax = Math.Round(entity.Tax.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero) * 100,
                TaxAmount = taxAmount,
                SubTotal = Math.Round(entity.SubTotal.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                TotalAmount = Math.Round(entity.SubTotal.GetValueOrDefault() + taxAmount, InvoiceRoundUpValue, MidpointRounding.AwayFromZero),
                CreatedOn = entity.CreatedOn?.ToString(StandardDateFormat),
                CreatedBy = entity.CreatedBy,
                UpdatedOn = entity.UpdatedOn?.ToString(StandardDateFormat),
                UpdatedBy = entity.UpdatedBy,
                InvoiceTo = entity.InvoiceToName
            };
        }

        public SaveManualInvoice MapManualInvoiceDetailsForEAQF(InspectionBookingDetail bookingData, SaveQuotationEaqfRequest request, CuContact contact,
            IEnumerable<ServiceTypeList> servcieType, SupplierAddress factoryAddres, int bankId, int currenyId, int billingEntity,
            int invoiceOfficeId, List<ZohoCustomerAddress> customerAddresses, string invoiceNumber)
        {
            List<SaveManualInvoiceItem> saveManualInvoiceItems = new List<SaveManualInvoiceItem>();

            foreach (var item in request.OrderDetails)
            {
                if (item.OrderType == "orderfee")
                {
                    saveManualInvoiceItems.Add(new SaveManualInvoiceItem()
                    {
                        Description = item.Description,
                        ServiceFee = item.Manday * item.Amount,
                        Manday = item.Manday,
                        UnitPrice = item.Amount
                    });
                }
                else if (item.OrderType == "otherfee")
                {
                    saveManualInvoiceItems.Add(new SaveManualInvoiceItem()
                    {
                        Description = item.Description,
                        OtherCost = item.Amount
                    });
                }
                else if (item.OrderType == "discount")
                {
                    saveManualInvoiceItems.Add(new SaveManualInvoiceItem()
                    {
                        Description = item.Description,
                        Discount = item.Amount
                    });
                }
            }

            var manualInvoice = new SaveManualInvoice()
            {
                FromDate = new DateObject(bookingData.ServiceDateFrom.Year, bookingData.ServiceDateFrom.Month, bookingData.ServiceDateFrom.Day),
                ToDate = new DateObject(bookingData.ServiceDateTo.Year, bookingData.ServiceDateTo.Month, bookingData.ServiceDateTo.Day),
                ServiceId = request.Service,
                CurrencyId = currenyId,
                BankId = bankId,
                CustomerId = bookingData.CustomerId,
                BilledName = bookingData.CustomerName,
                BilledAddress = customerAddresses?.FirstOrDefault(x => x.AddressType == (int)RefAddressTypeEnum.HeadOffice)?.Address,
                BillingEntity = billingEntity,
                InvoiceTo = (int)QuotationPaidBy.customer,
                InvoiceDate = new DateObject(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                OfficeId = invoiceOfficeId,
                SupplierId = bookingData.SupplierId,
                ServiceType = servcieType?.FirstOrDefault()?.serviceTypeId ?? null,
                BookingNo = bookingData.InspectionId,
                InvoiceNo = invoiceNumber,
                CountryId = factoryAddres?.countryId,
                UserId = request.UserId,
                PaymentMode = request.PaymentMode,
                PaymentRef = request.PaymentRef,
                Attn = contact.ContactName,
                Email = contact.Email,
                InvoiceItems = saveManualInvoiceItems
            };
            return manualInvoice;
        }
    }
}