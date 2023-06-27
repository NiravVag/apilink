using DTO.Common;
using DTO.Invoice;
using DTO.Report;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.Maps
{
    public class InvoiceSummaryMap : ApiCommonData
    {
        public List<InvoiceSummary> MapInvoiceSummary(List<InvoiceSummaryItem> invoiceDataList, IEnumerable<ServiceTypeList> serviceTypeList, List<InvoiceExtraFeeItem> ExtraFeeList)
        {
            var response = new List<InvoiceSummary>();
            var invoiceIdList = invoiceDataList.Select(x => x.InvoiceNo).Distinct().ToList();

            foreach (var invoiceId in invoiceIdList)
            {
                var invoiceBookingList = invoiceDataList.Where(x => x.InvoiceNo == invoiceId).ToList();
                var invoiceData = invoiceDataList.FirstOrDefault(x => x.InvoiceNo == invoiceId);
                var invoiceExtraFeeData = ExtraFeeList.Where(x => x.InvoiceNo == invoiceId).ToList();
                var feeData = CalculateTotalValueWithTax(invoiceBookingList);

                var serviceTypeNameList = new List<string>();

                //service type based on invoice and service
                if (invoiceBookingList.Any(x => x.ServiceId == (int)Service.InspectionId))
                {
                    serviceTypeNameList = serviceTypeList.Where(z => invoiceBookingList.Select(x => x.BookingId).Contains(z.InspectionId)).Select(y => y.serviceTypeName).Distinct().ToList();
                }
                else if (invoiceBookingList.Any(x => x.ServiceId == (int)Service.AuditId))
                {
                    serviceTypeNameList = serviceTypeList.Where(z => invoiceBookingList.Select(x => x.AuditId).Contains(z.AuditId)).Select(y => y.serviceTypeName).Distinct().ToList();
                }

                response.Add(new InvoiceSummary()
                {
                    Id = invoiceData.Id,
                    InvoiceNo = invoiceId,
                    InvoiceDate = invoiceData.InvoiceDate?.ToString(StandardDateFormat),
                    InvoiceTo = invoiceData.InvoiceTo,
                    InvoiceCurrency = invoiceData.InvoiceCurrency,
                    TotalFee = Math.Round(invoiceBookingList.Select(x => x.TotalFee.GetValueOrDefault()).Sum()
                    + invoiceExtraFeeData.Where(x => x.InvoiceTo == invoiceData.InvoiceToId).Select(x => x.TotalExtraFees.GetValueOrDefault()).Sum(), 2, MidpointRounding.AwayFromZero),
                    IsInspection = invoiceData.IsInspection.GetValueOrDefault(),
                    IsTravelExpense = invoiceData.IsTravelExpense.GetValueOrDefault(),
                    ServiceId = invoiceData.ServiceId.GetValueOrDefault(),
                    Service = invoiceData.ServiceName,
                    ServiceType = string.Join(" ,", serviceTypeNameList),
                    InspFees = Math.Round(feeData.InspFees, 2, MidpointRounding.AwayFromZero),
                    TravelFee = Math.Round(feeData.TravelFee, 2, MidpointRounding.AwayFromZero),
                    OtherExpense = Math.Round(feeData.OtherExpense, 2),
                    HotelFee = Math.Round(feeData.HotelFee.GetValueOrDefault(), 2),
                    Discount = invoiceBookingList.Select(x => x.Discount).Sum(),
                    InvoiceToName = invoiceData.InvoiceTo.ToLower() == QuotationPaidBy.customer.ToString().ToLower() ?
                      invoiceData.CustomerName
                    : invoiceData.InvoiceTo.ToLower() == QuotationPaidBy.supplier.ToString().ToLower() ?
                     invoiceData.SupplierName : invoiceData.FactoryName,
                    InvoiceStatusId = invoiceData.StatusId,
                    InvoiceStatusName = invoiceData.StatusName,
                    CustomerIdList = invoiceBookingList.Select(x => x.CustomerId).Distinct().ToList(),
                    CustomerName = invoiceData.CustomerName,
                    FactoryCountry = string.Join(", ", invoiceBookingList.Select(x => x.FactoryCountry).Distinct().ToList()),
                    InvoiceTypeName = invoiceData.InvoiceTypeName,
                    InvoiceOfficeName = invoiceData.InvoiceOfficeName,
                    PaymentStatusName = invoiceData.PaymentStatusName,
                    BillingMethodName = invoiceData.BillingMethodName,
                    InvoiceTypeId = invoiceData.InvoiceTypeId.GetValueOrDefault(),
                    BillTo = invoiceData.InvoiceToId.GetValueOrDefault(),
                    BankId = invoiceData.BankId,
                    ExtraFees = Math.Round(invoiceExtraFeeData.Where(x => x.InvoiceTo == invoiceData.InvoiceToId).Select(x => x.TotalExtraFees.GetValueOrDefault()).Sum(), 2, MidpointRounding.AwayFromZero)
                });
            }

            return response;
        }

        public InvoiceSummary CalculateTotalValueWithTax(List<InvoiceSummaryItem> invoiceData)
        {
            var res = new InvoiceSummary();
            foreach (var booking in invoiceData)
            {
                res.InspFees = Math.Round((!(res.InspFees > 0) ? 0 : res.InspFees) + booking.InspFees.GetValueOrDefault() + Math.Round(booking.InspFees.GetValueOrDefault() * booking.TaxValue.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero);
                res.TravelFee = Math.Round((!(res.TravelFee > 0) ? 0 : res.TravelFee) + booking.TravelFee.GetValueOrDefault() + (booking.TravelFee.GetValueOrDefault() * booking.TaxValue.GetValueOrDefault()), 2, MidpointRounding.AwayFromZero);
                res.HotelFee = Math.Round((!(res.HotelFee > 0) ? 0 : res.HotelFee.GetValueOrDefault()) + booking.HotelFee.GetValueOrDefault() + (booking.HotelFee.GetValueOrDefault() * booking.TaxValue.GetValueOrDefault()), 2, MidpointRounding.AwayFromZero);
                res.OtherExpense = Math.Round((!(res.OtherExpense > 0) ? 0 : res.OtherExpense) + booking.OtherExpense.GetValueOrDefault() + (booking.OtherExpense.GetValueOrDefault() * booking.TaxValue.GetValueOrDefault()), 2, MidpointRounding.AwayFromZero);
            }

            return res;
        }

        public InvoiceBookingData MapInvoiceBooking(InvoiceSummaryItem invoiceData, List<InvoiceExtraFeeItem> ExtraFeeList, List<InvoiceBookingFactoryDetails> factoryDetails)
        {

            var totalExtraFees = ExtraFeeList.FirstOrDefault(x => x.BookingId == invoiceData.BookingId

            && x.BilledTo == invoiceData.InvoiceToId && invoiceData.Id == x.InvoiceId)?.TotalExtraFees;

            return new InvoiceBookingData()
            {
                BookingId = invoiceData.BookingId.GetValueOrDefault(),
                InspFees = Math.Round(invoiceData.InspFees.GetValueOrDefault() + (invoiceData.InspFees.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault()), 2),
                TravelFee = Math.Round(invoiceData.TravelFee.GetValueOrDefault() + (invoiceData.TravelFee.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault()), 2),
                OtherExpense = Math.Round(invoiceData.OtherExpense.GetValueOrDefault() + (invoiceData.OtherExpense.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault()), 2),
                HotelFee = Math.Round(invoiceData.HotelFee.GetValueOrDefault() + (invoiceData.HotelFee.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault()), 2),
                Discount = invoiceData.Discount,
                InvoiceTotal = Math.Round(invoiceData.TotalFee.GetValueOrDefault() + totalExtraFees.GetValueOrDefault(), 2),
                CreatedBy = invoiceData.CreatedBy,
                InvoiceDate = invoiceData.InvoiceDate?.ToString(StandardDateFormat),
                InvoiceNo = invoiceData.InvoiceNo,
                InvoieRemarks = invoiceData.InvoiceRemarks,
                InvoiceCurrency = invoiceData.InvoiceCurrency,
                ExtraFees = Math.Round(totalExtraFees ?? totalExtraFees.GetValueOrDefault(), 2),
                BookingServiceDateFrom = invoiceData.BookingServiceDateFrom.ToString(StandardDateFormat),
                BookingServiceDateTo = invoiceData.BookingServiceDateTo.ToString(StandardDateFormat),
                UnitPrice = invoiceData.UnitPrice,
                BillingManDays = invoiceData.BillingManDays,
                FactoryCountry = factoryDetails.FirstOrDefault(x => x.FactoryId == invoiceData.FactoryId)?.FactoryCountryName
            };
        }

        /// <summary>
        /// map invoice audit.
        /// </summary>
        /// <param name="invoiceData"></param>
        /// <param name="ExtraFeeList"></param>
        /// <returns></returns>
        public InvoiceBookingData MapInvoiceAudit(InvoiceSummaryItem invoiceData, List<InvoiceExtraFeeItem> ExtraFeeList, List<InvoiceBookingFactoryDetails> factoryDetails)
        {

            var totalExtraFees = ExtraFeeList.FirstOrDefault(x => x.AuditId == invoiceData.AuditId

            && x.BilledTo == invoiceData.InvoiceToId && invoiceData.Id == x.InvoiceId)?.TotalExtraFees;

            return new InvoiceBookingData()
            {
                AuditId = invoiceData.AuditId.GetValueOrDefault(),
                InspFees = Math.Round(invoiceData.InspFees.GetValueOrDefault() + (invoiceData.InspFees.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault()), 2),
                TravelFee = Math.Round(invoiceData.TravelFee.GetValueOrDefault() + (invoiceData.TravelFee.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault()), 2),
                OtherExpense = Math.Round(invoiceData.OtherExpense.GetValueOrDefault() + (invoiceData.OtherExpense.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault()), 2),
                HotelFee = Math.Round(invoiceData.HotelFee.GetValueOrDefault() + (invoiceData.HotelFee.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault()), 2),
                Discount = invoiceData.Discount,
                InvoiceTotal = Math.Round(invoiceData.TotalFee.GetValueOrDefault() + totalExtraFees.GetValueOrDefault(), 2),
                CreatedBy = invoiceData.CreatedBy,
                InvoiceDate = invoiceData.InvoiceDate?.ToString(StandardDateFormat),
                InvoiceNo = invoiceData.InvoiceNo,
                InvoieRemarks = invoiceData.InvoiceRemarks,
                InvoiceCurrency = invoiceData.InvoiceCurrency,
                ExtraFees = Math.Round(totalExtraFees ?? totalExtraFees.GetValueOrDefault(), 2),
                BookingServiceDateFrom = invoiceData.BookingServiceDateFrom.ToString(StandardDateFormat),
                BookingServiceDateTo = invoiceData.BookingServiceDateTo.ToString(StandardDateFormat),
                FactoryCountry = factoryDetails.FirstOrDefault(x => x.FactoryId == invoiceData.FactoryId)?.FactoryCountryName
            };
        }

        public List<InvoiceReportTemplateItem> MapInvoiceReportTemplates(InvoiceReportTemplate templateList)
        {
            return templateList.ResultList.ConvertAll(x => new InvoiceReportTemplateItem()
            {
                TemplateId = x.TemplateId,
                TemplateName = x.TemplateName,
                CustomerId = x.CustomerId,
                InvoiceType = x.InvoiceType
            }).ToList();
        }

        public ExportInvoiceBookingData MapInvoiceExport(InvoiceSummaryItem invoiceData, IEnumerable<ServiceTypeList> serviceTypeList, IEnumerable<InvoiceExtraFeeItem> extraFeeDatalst)
        {
            InvoiceExtraFeeItem extrafee = new InvoiceExtraFeeItem();
            string servicetype = "";
            int bookingId = 0;
            if (invoiceData.ServiceId == (int)Service.InspectionId)
            {
                extrafee = extraFeeDatalst.FirstOrDefault(y => y.BookingId == invoiceData.BookingId && y.BilledTo == invoiceData.InvoiceToId);
                servicetype = serviceTypeList.Where(x => x.InspectionId == invoiceData.BookingId).Select(x => x.serviceTypeName).FirstOrDefault();
                bookingId = invoiceData.BookingId.GetValueOrDefault();
            }
            else if (invoiceData.ServiceId == (int)Service.AuditId)
            {
                extrafee = extraFeeDatalst.FirstOrDefault(y => y.AuditId == invoiceData.AuditId && y.BilledTo == invoiceData.InvoiceToId);
                servicetype = serviceTypeList.Where(x => x.AuditId == invoiceData.AuditId).Select(x => x.serviceTypeName).FirstOrDefault();
                bookingId = invoiceData.AuditId.GetValueOrDefault();
            }

            return new ExportInvoiceBookingData()
            {
                BookingId = bookingId,
                InspFees = Math.Round(invoiceData.InspFees.GetValueOrDefault() + (Math.Round(invoiceData.InspFees.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero),
                TravelFee = Math.Round(invoiceData.TravelFee.GetValueOrDefault() + Math.Round(invoiceData.TravelFee.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero),
                OtherExpense = Math.Round(invoiceData.OtherExpense.GetValueOrDefault() + Math.Round(invoiceData.OtherExpense.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero),
                HotelFee = Math.Round(invoiceData.HotelFee.GetValueOrDefault() + Math.Round(invoiceData.HotelFee.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero),
                Discount = Math.Round(invoiceData.Discount.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero),
                InvoiceTotal = Math.Round(invoiceData.TotalFee.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero) + (extrafee != null ? Math.Round(extrafee.TotalExtraFees.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero) : 0),
                CreatedBy = invoiceData.CreatedBy,
                InvoiceNo = invoiceData.InvoiceNo,
                InvoiceTypeName = invoiceData.InvoiceTypeName,
                InvoiceDate = invoiceData.InvoiceDate,
                InvoiceTo = invoiceData.InvoiceTo,
                InvoiceToName = invoiceData?.InvoiceToId.GetValueOrDefault() == (int)QuotationPaidBy.customer ? invoiceData.CustomerName : invoiceData?.InvoiceToId.GetValueOrDefault() == (int)QuotationPaidBy.supplier ? invoiceData.SupplierName : invoiceData.FactoryName,
                CustomerName = invoiceData?.CustomerName,
                InvoiceCurrency = invoiceData.InvoiceCurrency,
                IsInspection = invoiceData.IsInspection.GetValueOrDefault(),
                IsTravelExpense = invoiceData.IsTravelExpense.GetValueOrDefault(),
                Service = invoiceData.ServiceName,
                ServiceType = servicetype,
                TravelAirFee = Math.Round(invoiceData.TravelAirFee.GetValueOrDefault() + Math.Round(invoiceData.TravelAirFee.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero),
                TravelLandFee = Math.Round(invoiceData.TravelLandFee.GetValueOrDefault() + Math.Round(invoiceData.TravelLandFee.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero),
                TravelOtherFee = Math.Round(invoiceData.TravelOtherFee.GetValueOrDefault() + Math.Round(invoiceData.TravelOtherFee.GetValueOrDefault() * invoiceData.TaxValue.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero),
                ExtraFees = extrafee != null ? Math.Round(extrafee.TotalExtraFees.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero) : 0,
                QuotationManDay = invoiceData.QuManday.GetValueOrDefault(),
                ActualManday = invoiceData.ActualManday,
                BookingServiceDateFrom = invoiceData.BookingServiceDateFrom,
                BookingServiceDateTo = invoiceData.BookingServiceDateTo,
                FactoryCountry = invoiceData.FactoryCountry,
                BillingMethodName = invoiceData.BillingMethodName,
                PaymentStatusName = invoiceData.PaymentStatusName,
                InvoiceOfficeName = invoiceData.InvoiceOfficeName
            };
        }
    }
}
