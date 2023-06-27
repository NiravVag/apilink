using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
using DTO.ExtraFees;
using DTO.Inspection;
using DTO.Invoice;
using DTO.InvoicePreview;
using DTO.Supplier;
using Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BI.Maps
{
    public class InvoiceMap : ApiCommonData
    {
        /// <summary>
        /// Map the invoice base details from the repo
        /// </summary>
        /// <param name="invoiceBaseDetailRepo"></param>
        /// <returns></returns>
        public InvoiceBaseDetail MapInvoiceBaseDetails(InvoiceBaseDetailRepo invoiceBaseDetailRepo)
        {
            if (invoiceBaseDetailRepo == null)
                return null;

            return new InvoiceBaseDetail
            {
                InvoiceNo = invoiceBaseDetailRepo.InvoiceNo,
                OldInvoiceNo = invoiceBaseDetailRepo.InvoiceNo,
                InvoiceDate = Static_Data_Common.GetCustomDate(invoiceBaseDetailRepo.InvoiceDate),
                PostDate = Static_Data_Common.GetCustomDate(invoiceBaseDetailRepo.PostDate),
                Subject = invoiceBaseDetailRepo.Subject,
                BillTo = invoiceBaseDetailRepo.BilledTo,
                InvoiceType = invoiceBaseDetailRepo.InvoiceType,
                BilledName = invoiceBaseDetailRepo.BilledName,
                BilledAddress = invoiceBaseDetailRepo.BilledAddress,
                PaymentTerms = invoiceBaseDetailRepo.PaymentTerms,
                PaymentDuration = invoiceBaseDetailRepo.PaymentDuration,
                InvoiceStatus = invoiceBaseDetailRepo.InvoiceStatus,
                Office = invoiceBaseDetailRepo.Office,
                BankDetails = invoiceBaseDetailRepo.BankDetails,
                BillMethod = invoiceBaseDetailRepo.BillingMethod,
                Currency = invoiceBaseDetailRepo.Currency,
                InvoiceCurrency = invoiceBaseDetailRepo.InvoiceCurrency,
                ExchangeRate = invoiceBaseDetailRepo.ExchangeRate,
                PaymentStatus = invoiceBaseDetailRepo.PaymentStatus,
                PaymentDate = Static_Data_Common.GetCustomDate(invoiceBaseDetailRepo.PaymentDate),
                TaxValue = invoiceBaseDetailRepo.TaxValue,
                CustomerId = invoiceBaseDetailRepo.CustomerId,
                SupplierId = invoiceBaseDetailRepo.SupplierId,
                FactoryId = invoiceBaseDetailRepo.FactoryId,
                TotalInvoiceFees = invoiceBaseDetailRepo.TotalInvoiceFees,
                TotalTaxAmount = invoiceBaseDetailRepo.TotalTaxAmount,
                TotalTravelFees = invoiceBaseDetailRepo.TotalTravelFees,
                InvoicingRequest = invoiceBaseDetailRepo.InvoicingRequest,
                IsTravelExpense = invoiceBaseDetailRepo.IsTravelExpense,
                IsInspectionFees = invoiceBaseDetailRepo.IsInspectionFees,
                InvoiceCurrencyName = invoiceBaseDetailRepo.InvoiceCurrencyName,
                BilledQuantityType = invoiceBaseDetailRepo.BilledQuantityType,
                ContactIds = invoiceBaseDetailRepo.BilledTo == (int)InvoiceTo.Customer ?
                                invoiceBaseDetailRepo.ContactList.Select(x => x.CustomerContactId) : invoiceBaseDetailRepo.BilledTo == (int)InvoiceTo.Supplier ?
                                invoiceBaseDetailRepo.ContactList.Select(x => x.SupplierContactId) : invoiceBaseDetailRepo.ContactList.Select(x => x.FactoryContactId)
            };
        }

        /// <summary>
        /// Map Invoice Billed address if the billtotype is customer
        /// </summary>
        /// <param name="addressList"></param>
        /// <returns></returns>
        public List<CommonDataSource> MapCustomerBilledAddress(List<CustomerAccountingAddress> addressList)
        {
            List<CommonDataSource> dataSourceList = new List<CommonDataSource>();
            foreach (var address in addressList)
            {
                CommonDataSource dataSource = new CommonDataSource() { Id = address.Id, Name = address.EnglishAddress };
                dataSourceList.Add(dataSource);
            }
            return dataSourceList;
        }

        public List<CommonDataSource> MapSupplierFactoryBilledAddress(List<SupplierAddress> addressList)
        {
            List<CommonDataSource> dataSourceList = new List<CommonDataSource>();
            foreach (var address in addressList)
            {
                CommonDataSource dataSource = new CommonDataSource() { Id = address.Id, Name = address.Address };
                dataSourceList.Add(dataSource);
            }
            return dataSourceList;
        }

        public List<InvoiceTransactionDetails> MapInvoiceTransactionDetails(List<InvoiceTransactionDetailRepo> transactionList, List<InvoiceBookingQuantityDetails> bookingQuantityDetails,
                                                                List<InvoiceBookingQuotation> bookingQuotationList,
                                                                List<SupplierGeoLocation> factoryGeoLocations,
                                                                List<InvoiceBookingServiceTypes> bookingServiceTypes,
                                                                List<InvoiceExtraFeeItem> extraFeesList)
        {

            return transactionList.Join(bookingQuantityDetails, invtrans => invtrans.BookingNo, bookingqty => bookingqty.BookingNo, (invtrans, bookingqty) => new { invtrans, bookingqty }).
                                                    Join(bookingServiceTypes, transqty => transqty.invtrans.BookingNo, serviceType => serviceType.BookingNo,
                                                    (transqtyJoin, serviceType) => new { transqtyJoin, serviceType }).
                                                    Join(factoryGeoLocations, transfac => transfac.transqtyJoin.invtrans.FactoryId, factoryLoc => factoryLoc.FactoryId,
                                                    (transfacqtyJoin, factoryLoc) => new { transfacqtyJoin, factoryLoc }).
                                                    GroupJoin(bookingQuotationList, transquot => transquot.transfacqtyJoin.transqtyJoin.invtrans.BookingNo, quot => quot.BookingNo,
                                                    (transactionJoinData, quot) => new { transactionJoin = transactionJoinData, quotation = quot }).
                                                    Select(x => new InvoiceTransactionDetails
                                                    {
                                                        Id = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.Id,
                                                        BookingNo = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.BookingNo,
                                                        CustomerBookingNo = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.CustomerBookingNo,
                                                        PriceCategory = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.PriceCategory,
                                                        QuotationNo = x.quotation.Where(y => y.BookingNo == x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.BookingNo).FirstOrDefault()?.QuotationNo,
                                                        ServiceDateFrom = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.ServiceDateFrom.ToString(StandardDateFormat),
                                                        ServiceDateTo = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.ServiceDateTo.ToString(StandardDateFormat),
                                                        ServiceType = x.transactionJoin.transfacqtyJoin.serviceType.ServiceType,
                                                        Customer = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.Customer,
                                                        Supplier = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.Supplier,
                                                        Factory = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.Factory,
                                                        FactoryCountry = x.transactionJoin.factoryLoc.Country,
                                                        FactoryProvince = x.transactionJoin.factoryLoc.Province,
                                                        FactoryCity = x.transactionJoin.factoryLoc.City,
                                                        FactoryCounty = x.transactionJoin.factoryLoc.County,
                                                        FactoryTown = x.transactionJoin.factoryLoc.Town,
                                                        TotalBookingQty = x.transactionJoin.transfacqtyJoin.transqtyJoin.bookingqty.BookingQty,
                                                        TotalInspectedQty = x.transactionJoin.transfacqtyJoin.transqtyJoin.bookingqty.InspectedQuantity,
                                                        TotalPresentedQty = x.transactionJoin.transfacqtyJoin.transqtyJoin.bookingqty.PresentedQuantity,
                                                        ManDay = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.ManDay,
                                                        UnitPrice = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.UnitPrice.GetValueOrDefault().ToString(),
                                                        InspectionFees = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.InspectionFees.GetValueOrDefault()),
                                                        AirCost = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.AirCost.GetValueOrDefault()),
                                                        LandCost = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.LandCost.GetValueOrDefault()),
                                                        HotelCost = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.HotelCost.GetValueOrDefault()),
                                                        OtherCost = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.OtherCost.GetValueOrDefault()),
                                                        Discount = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.Discount.GetValueOrDefault()),
                                                        TravelOtherFees = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.TravelOtherFees.GetValueOrDefault()),
                                                        TravelTotalFees = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.TravelTotalFees.GetValueOrDefault()),

                                                        ExtraFees = string.Format("{0:0.00}", extraFeesList.
                                                        FirstOrDefault(y => y.BookingId == x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.BookingNo) != null ? extraFeesList.
                                                        FirstOrDefault(y => y.BookingId == x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.BookingNo).TotalExtraFees.GetValueOrDefault() : 0),

                                                        ExtraFeeSubTotal = string.Format("{0:0.00}", extraFeesList.
                                                        FirstOrDefault(y => y.BookingId == x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.BookingNo) != null ? extraFeesList.
                                                        FirstOrDefault(y => y.BookingId == x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.BookingNo).TotalExtraSubFees.GetValueOrDefault() : 0),

                                                        ExtraFeeTax = string.Format("{0:0.00}", extraFeesList.
                                                        FirstOrDefault(y => y.BookingId == x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.BookingNo) != null ? extraFeesList.
                                                        FirstOrDefault(y => y.BookingId == x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.BookingNo).TotalExtrFeeTax.GetValueOrDefault() : 0),

                                                        IsTravelExpense = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.IsTravelExpense,
                                                        IsInspectionFees = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.IsInspectionFees,
                                                        Remarks = x.transactionJoin.transfacqtyJoin.transqtyJoin.invtrans.Remarks
                                                    }).ToList();
        }

        /// <summary>
        /// map audit invoice details
        /// </summary>
        /// <param name="transactionList"></param>
        /// <param name="bookingQuantityDetails"></param>
        /// <param name="bookingQuotationList"></param>
        /// <param name="factoryGeoLocations"></param>
        /// <param name="bookingServiceTypes"></param>
        /// <param name="extraFeesList"></param>
        /// <returns></returns>
        public List<InvoiceTransactionDetails> MapAuditInvoiceTransactionDetails(List<InvoiceTransactionDetailRepo> transactionList, List<InvoiceBookingQuantityDetails> bookingQuantityDetails,
                                                                List<InvoiceBookingQuotation> bookingQuotationList, List<SupplierGeoLocation> factoryGeoLocations, List<InvoiceBookingServiceTypes> bookingServiceTypes, List<InvoiceExtraFeeItem> extraFeesList)
        {

            return transactionList.Join(bookingServiceTypes, invTrans => invTrans.AuditNo, serviceType => serviceType.BookingNo,
                                                    (transqtyJoin, serviceType) => new { transqtyJoin, serviceType }).
                                                    Join(factoryGeoLocations, transfac => transfac.transqtyJoin.FactoryId, factoryLoc => factoryLoc.FactoryId,
                                                    (transfacqtyJoin, factoryLoc) => new { transfacqtyJoin, factoryLoc }).
                                                    GroupJoin(bookingQuotationList, transquot => transquot.transfacqtyJoin.transqtyJoin.AuditNo, quot => quot.BookingNo,
                                                    (transactionJoinData, quot) => new { transactionJoin = transactionJoinData, quotation = quot }).
                                                    Select(x => new InvoiceTransactionDetails
                                                    {
                                                        Id = x.transactionJoin.transfacqtyJoin.transqtyJoin.Id,
                                                        BookingNo = x.transactionJoin.transfacqtyJoin.transqtyJoin.AuditNo,
                                                        QuotationNo = x.quotation.FirstOrDefault(y => y.BookingNo == x.transactionJoin.transfacqtyJoin.transqtyJoin.AuditNo)?.QuotationNo,
                                                        ServiceDateFrom = x.transactionJoin.transfacqtyJoin.transqtyJoin.ServiceDateFrom.ToString(StandardDateFormat),
                                                        ServiceDateTo = x.transactionJoin.transfacqtyJoin.transqtyJoin.ServiceDateTo.ToString(StandardDateFormat),
                                                        ServiceType = x.transactionJoin.transfacqtyJoin.serviceType.ServiceType,
                                                        Customer = x.transactionJoin.transfacqtyJoin.transqtyJoin.Customer,
                                                        Supplier = x.transactionJoin.transfacqtyJoin.transqtyJoin.Supplier,
                                                        Factory = x.transactionJoin.transfacqtyJoin.transqtyJoin.Factory,
                                                        FactoryCountry = x.transactionJoin.factoryLoc.Country,
                                                        FactoryProvince = x.transactionJoin.factoryLoc.Province,
                                                        FactoryCity = x.transactionJoin.factoryLoc.City,
                                                        FactoryCounty = x.transactionJoin.factoryLoc.County,
                                                        FactoryTown = x.transactionJoin.factoryLoc.Town,
                                                        ManDay = x.transactionJoin.transfacqtyJoin.transqtyJoin.ManDay,
                                                        UnitPrice = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.UnitPrice.GetValueOrDefault()),
                                                        InspectionFees = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.InspectionFees.GetValueOrDefault()),
                                                        AirCost = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.AirCost.GetValueOrDefault()),
                                                        LandCost = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.LandCost.GetValueOrDefault()),
                                                        HotelCost = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.HotelCost.GetValueOrDefault()),
                                                        OtherCost = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.OtherCost.GetValueOrDefault()),
                                                        Discount = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.Discount.GetValueOrDefault()),
                                                        TravelOtherFees = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.TravelOtherFees.GetValueOrDefault()),
                                                        TravelTotalFees = string.Format("{0:0.00}", x.transactionJoin.transfacqtyJoin.transqtyJoin.TravelTotalFees.GetValueOrDefault()),

                                                        ExtraFees = string.Format("{0:0.00}", extraFeesList.
                                                        FirstOrDefault(y => y.AuditId == x.transactionJoin.transfacqtyJoin.transqtyJoin.AuditNo) != null ? extraFeesList.
                                                        FirstOrDefault(y => y.AuditId == x.transactionJoin.transfacqtyJoin.transqtyJoin.AuditNo).TotalExtraFees.GetValueOrDefault() : 0),

                                                        ExtraFeeSubTotal = string.Format("{0:0.00}", extraFeesList.
                                                        FirstOrDefault(y => y.AuditId == x.transactionJoin.transfacqtyJoin.transqtyJoin.AuditNo) != null ? extraFeesList.
                                                        FirstOrDefault(y => y.AuditId == x.transactionJoin.transfacqtyJoin.transqtyJoin.AuditNo).TotalExtraSubFees.GetValueOrDefault() : 0),

                                                        ExtraFeeTax = string.Format("{0:0.00}", extraFeesList.
                                                        FirstOrDefault(y => y.AuditId == x.transactionJoin.transfacqtyJoin.transqtyJoin.AuditNo) != null ? extraFeesList.
                                                        FirstOrDefault(y => y.AuditId == x.transactionJoin.transfacqtyJoin.transqtyJoin.AuditNo).TotalExtrFeeTax.GetValueOrDefault() : 0),

                                                        IsTravelExpense = x.transactionJoin.transfacqtyJoin.transqtyJoin.IsTravelExpense,
                                                        IsInspectionFees = x.transactionJoin.transfacqtyJoin.transqtyJoin.IsInspectionFees,
                                                        Remarks = x.transactionJoin.transfacqtyJoin.transqtyJoin.Remarks
                                                    }).ToList();
        }

        public InvAutTranDetail MapUpdateInvoiceDetails(InvAutTranDetail invAutTranDetail, UpdateInvoiceDetail invoiceDetail,
                                                                                UpdateInvoiceBaseDetail invoiceBaseDetail, int userId)
        {

            invAutTranDetail.TotalInvoiceFees = invoiceDetail.TotalInspectionFees;
            invAutTranDetail.InvoiceMethod = invoiceBaseDetail.BillMethod;
            invAutTranDetail.InvoiceTo = invoiceBaseDetail.BillTo;
            invAutTranDetail.InvoicePaymentStatus = invoiceBaseDetail.InvoicePaymentStatus;
            invAutTranDetail.InvoicePaymentDate = invoiceBaseDetail.InvoicePaymentDate?.ToDateTime();
            invAutTranDetail.InvoicedName = invoiceBaseDetail.BilledName;
            invAutTranDetail.InvoicedAddress = invoiceBaseDetail.BilledAddress;
            invAutTranDetail.PaymentTerms = invoiceBaseDetail.PaymentTerms;
            invAutTranDetail.PaymentDuration = invoiceBaseDetail.PaymentDuration;
            invAutTranDetail.Office = invoiceBaseDetail.Office;
            invAutTranDetail.InvoiceDate = invoiceBaseDetail.InvoiceDate.ToDateTime();
            invAutTranDetail.PostedDate = invoiceBaseDetail.PostDate.ToDateTime();

            invAutTranDetail.InvoiceNo = invoiceBaseDetail.InvoiceNo;
            invAutTranDetail.UnitPrice = invoiceDetail.UnitPrice;
            invAutTranDetail.InspectionFees = invoiceDetail.InspectionFees;
            invAutTranDetail.TravelAirFees = invoiceDetail.TravelAirFees;
            invAutTranDetail.TravelLandFees = invoiceDetail.TravelLandFees;
            invAutTranDetail.TravelOtherFees = invoiceDetail.TravelOtherFees;
            invAutTranDetail.HotelFees = invoiceDetail.HotelFees;
            invAutTranDetail.OtherFees = invoiceDetail.OtherFees;
            invAutTranDetail.Discount = invoiceDetail.Discount;
            invAutTranDetail.ManDays = invoiceDetail.ManDays;

            invAutTranDetail.TotalTaxAmount = invoiceDetail.TotalTaxAmount;
            invAutTranDetail.TravelTotalFees = invoiceDetail.TotalTravelFees;

            invAutTranDetail.InspectionId = invoiceDetail.BookingNo;
            invAutTranDetail.Remarks = invoiceDetail.Remarks;
            invAutTranDetail.Subject = invoiceBaseDetail.Subject;
            invAutTranDetail.InvoiceStatus = (int)InvoiceStatus.Modified;
            invAutTranDetail.UpdatedBy = userId;
            invAutTranDetail.UpdatedOn = DateTime.Now;

            return invAutTranDetail;
        }

        public InvAutTranDetail MapUpdateAuditInvoiceDetails(InvAutTranDetail invAutTranDetail, UpdateInvoiceDetail invoiceDetail,
                                                                                UpdateInvoiceBaseDetail invoiceBaseDetail, int userId)
        {

            invAutTranDetail.TotalInvoiceFees = invoiceDetail.TotalInspectionFees;
            invAutTranDetail.InvoiceMethod = invoiceBaseDetail.BillMethod;
            invAutTranDetail.InvoiceTo = invoiceBaseDetail.BillTo;
            invAutTranDetail.InvoicePaymentStatus = invoiceBaseDetail.InvoicePaymentStatus;
            invAutTranDetail.InvoicePaymentDate = invoiceBaseDetail.InvoicePaymentDate?.ToDateTime();
            invAutTranDetail.InvoicedName = invoiceBaseDetail.BilledName;
            invAutTranDetail.InvoicedAddress = invoiceBaseDetail.BilledAddress;
            invAutTranDetail.PaymentTerms = invoiceBaseDetail.PaymentTerms;
            invAutTranDetail.PaymentDuration = invoiceBaseDetail.PaymentDuration;
            invAutTranDetail.Office = invoiceBaseDetail.Office;
            invAutTranDetail.InvoiceDate = invoiceBaseDetail.InvoiceDate.ToDateTime();
            invAutTranDetail.PostedDate = invoiceBaseDetail.PostDate.ToDateTime();

            invAutTranDetail.InvoiceNo = invoiceBaseDetail.InvoiceNo;
            invAutTranDetail.UnitPrice = invoiceDetail.UnitPrice;
            invAutTranDetail.InspectionFees = invoiceDetail.InspectionFees;
            invAutTranDetail.TravelAirFees = invoiceDetail.TravelAirFees;
            invAutTranDetail.TravelLandFees = invoiceDetail.TravelLandFees;
            invAutTranDetail.TravelOtherFees = invoiceDetail.TravelOtherFees;
            invAutTranDetail.HotelFees = invoiceDetail.HotelFees;
            invAutTranDetail.OtherFees = invoiceDetail.OtherFees;
            invAutTranDetail.Discount = invoiceDetail.Discount;
            invAutTranDetail.ManDays = invoiceDetail.ManDays;

            invAutTranDetail.TotalTaxAmount = invoiceDetail.TotalTaxAmount;
            invAutTranDetail.TravelTotalFees = invoiceDetail.TotalTravelFees;

            invAutTranDetail.AuditId = invoiceDetail.BookingNo;
            invAutTranDetail.Remarks = invoiceDetail.Remarks;
            invAutTranDetail.Subject = invoiceBaseDetail.Subject;
            invAutTranDetail.InvoiceStatus = (int)InvoiceStatus.Modified;
            invAutTranDetail.UpdatedBy = userId;
            invAutTranDetail.UpdatedOn = DateTime.Now;

            return invAutTranDetail;
        }

        public InvAutTranStatusLog MapInvoiceTransactionStatusLog(int invoiceId, int inspectionId, int statusId, int userId, int entityId)
        {
            return new InvAutTranStatusLog()
            {
                InvoiceId = invoiceId,
                InspectionId = inspectionId,
                StatusId = statusId,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                EntityId = entityId
            };
        }

        public InvAutTranStatusLog MapAuditInvoiceTransactionStatusLog(int invoiceId, int auditId, int statusId, int userId, int entityId)
        {
            return new InvAutTranStatusLog()
            {
                InvoiceId = invoiceId,
                AuditId = auditId,
                StatusId = statusId,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                EntityId = entityId
            };
        }

        //booking details map
        public InvoiceDetailsPreview BookingDataMap(InvoiceBookingPDFDetail bookingItem,
            List<InvoiceBookingProductsData> ProductList, List<InvoiceBookingQuantityDetails> totalQtyBookingList, InvoiceDetailsPreview invoiceData,
            List<BookingServiceType> serviceTypeList, List<InvoiceBookingQuotation> QuotationNoList, InvoiceDetailsRepo invoiceDetailData,
            List<SupplierGeoLocation> factoryLocationList, List<InvoiceBookingProductsData> productPoCountryList)
        {
            var factoryAddressData = factoryLocationList?.FirstOrDefault(x => x.FactoryId == bookingItem?.FactoryId);
            var endDate = DateTime.DaysInMonth(bookingItem.ServiceDateFrom.Year, bookingItem.ServiceDateFrom.Month);

            double extraFees = 0;
            var monthEnd = endDate == 31 ? endDate + MonthWord : endDate + MonthWord1;

            //get short month name
            var monthShortName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(bookingItem.ServiceDateFrom.Month);

            invoiceData.TravelHotelCost = (invoiceDetailData?.TotalTravelFee + invoiceDetailData?.HotelCost).ToString() ?? string.Empty;

            invoiceData.TotalInvoiceFees = (invoiceDetailData?.TotalInvoiceFees + (Double.TryParse(invoiceData?.ExtrafeeTotal, out extraFees) ? extraFees : 0)).ToString() ?? string.Empty;
            invoiceData.TaxValue = invoiceDetailData?.TaxValue?.ToString() ?? string.Empty;

            invoiceData.UnitPrice = invoiceDetailData?.UnitPrice?.ToString() ?? string.Empty;

            invoiceData.Discount = invoiceDetailData?.Discount?.ToString() ?? string.Empty;
            invoiceData.Currency = invoiceDetailData?.Currency ?? string.Empty;
            invoiceData.ManDay = invoiceDetailData?.ManDay?.ToString() ?? string.Empty;

            invoiceData.Description = MonthDesc + " " + monthShortName + " " + Month1stWord + " - " + monthShortName + " " + monthEnd;

            invoiceData.BookingNo = bookingItem?.BookingNo.ToString() ?? string.Empty;
            invoiceData.QuotationNo = QuotationNoList?.Where(x => x.BookingNo == bookingItem?.BookingNo).Select(x => x.QuotationNo.ToString() ?? string.Empty).FirstOrDefault();
            invoiceData.CustomerBookingNo = bookingItem?.CustomerBookingNo ?? string.Empty;

            invoiceData.CustomerName = bookingItem?.CustomerName ?? string.Empty;
            invoiceData.SupplierName = bookingItem?.SupplierName ?? string.Empty;
            invoiceData.SupplierCode = bookingItem?.SupplierCode ?? string.Empty;
            invoiceData.FactoryName = bookingItem?.FactoryName ?? string.Empty;
            invoiceData.CustomerPriceCategory = bookingItem?.CustomerPriceCategory ?? string.Empty;

            invoiceData.FactoryCountry = factoryAddressData?.Country ?? string.Empty;
            invoiceData.FactoryProvince = factoryAddressData?.Province ?? string.Empty;
            invoiceData.FactoryCity = factoryAddressData?.City ?? string.Empty;
            invoiceData.FactoryCounty = factoryAddressData?.County ?? string.Empty;
            invoiceData.FactoryTown = factoryAddressData?.Town ?? string.Empty;


            invoiceData.CustomerBuyer = bookingItem?.CustomerBuyer != null ? string.Join(", ", bookingItem?.CustomerBuyer) : "";
            invoiceData.CustomerBrand = bookingItem?.CustomerBrand != null ? string.Join(", ", bookingItem?.CustomerBrand) : "";
            invoiceData.CustomerDepartment = bookingItem?.CustomerDepartment != null ? string.Join(", ", bookingItem?.CustomerDepartment) : "";
            invoiceData.CustomerContacts = bookingItem?.CustomerContact != null ? string.Join(", ", bookingItem?.CustomerContact) : "";
            invoiceData.Merchandiser = bookingItem?.Merchandiser != null ? string.Join(", ", bookingItem?.Merchandiser) : "";
            invoiceData.CustomerCollection = bookingItem?.CustomerCollection ?? string.Empty;

            //product category,sub category,sub category 2 section
            invoiceData.CustomerProductCategory = ProductList?.Where(x => x.BookingId == bookingItem?.BookingNo).Select(x => x.ProductCategory).FirstOrDefault() ?? string.Empty;
            invoiceData.ProductSubCategory = string.Join(", ", ProductList?.Where(x => x.BookingId == bookingItem?.BookingNo).Select(y => y.ProductSubCategory).Distinct()) ?? string.Empty;
            invoiceData.ProductSubCategory2 = string.Join(", ", ProductList?.Where(x => x.BookingId == bookingItem?.BookingNo).Select(y => y.ProductSubCategory2).Distinct()) ?? string.Empty;

            invoiceData.ServiceDateFromToDate = bookingItem?.ServiceDateFrom == bookingItem?.ServiceDateTo ? bookingItem?.ServiceDateFrom.ToString(StandardDateFormat)
                 : bookingItem?.ServiceDateFrom.ToString(StandardDateFormat) + " - " +
                bookingItem?.ServiceDateTo.ToString(StandardDateFormat);

            invoiceData.ServiceType = string.Join(", ", serviceTypeList?.Where(x => x.BookingNo == bookingItem?.BookingNo).Select(x => x.ServiceTypeName).ToList()) ?? string.Empty;
            invoiceData.TotalbookingQty = totalQtyBookingList?.Where(x => x.BookingNo == bookingItem?.BookingNo).Select(x => x.BookingQty?.ToString()).FirstOrDefault() ?? string.Empty;
            invoiceData.TotalInspectedQty = totalQtyBookingList?.Where(x => x.BookingNo == bookingItem?.BookingNo).Select(x => x.InspectedQuantity?.ToString()).FirstOrDefault() ?? string.Empty;
            invoiceData.PresentedQty = totalQtyBookingList?.Where(x => x.BookingNo == bookingItem?.BookingNo).Select(x => x.PresentedQuantity?.ToString()).FirstOrDefault() ?? string.Empty;

            invoiceData.AirCost = invoiceDetailData?.AirCost?.ToString() ?? string.Empty;

            invoiceData.HotelCost = invoiceDetailData?.HotelCost?.ToString() ?? string.Empty;

            invoiceData.LandCost = invoiceDetailData?.LandCost?.ToString() ?? string.Empty;

            invoiceData.OtherCost = invoiceDetailData?.OtherCost?.ToString() ?? string.Empty;

            invoiceData.TravelTotalCost = invoiceDetailData?.TotalTravelFee?.ToString() ?? string.Empty;

            invoiceData.InspFee = invoiceDetailData?.InspFee?.ToString() ?? string.Empty;

            invoiceData.TravelOtherFees = invoiceDetailData?.TravelOtherFees?.ToString() ?? string.Empty;

            invoiceData.DestinationCountry = string.Join(", ", productPoCountryList?.Where(x => x.BookingId == bookingItem?.BookingNo)
                                                    .Select(x => x.DestinationCountry).Distinct().ToList());
            invoiceData.BookingPO = string.Join(", ", productPoCountryList?.Where(x => x.BookingId == bookingItem?.BookingNo)
                                                    .Select(x => x.PoNumber).Distinct().ToList());

            invoiceData.InspectionLocation = bookingItem.InspectionLocation;

            invoiceData.Season = string.Concat(bookingItem.Season, bookingItem.SeasonYear.HasValue ? " " + bookingItem.SeasonYear : "");
            invoiceData.Inspectors = string.Join(", ", bookingItem?.ScheduleStaffItems.Select(z => z.Name).Distinct());
            invoiceData.InvoiceManday = invoiceData?.ManDay?.ToString() ?? string.Empty;
            return invoiceData;
        }

        public InvoiceDetailsPreview AuditDataMap(InvoiceBookingPDFDetail bookingItem,
          List<InvoiceBookingProductsData> ProductList, List<InvoiceBookingQuantityDetails> totalQtyBookingList, InvoiceDetailsPreview invoiceData,
          List<BookingServiceType> serviceTypeList, List<InvoiceBookingQuotation> QuotationNoList, InvoiceDetailsRepo invoiceDetailData,
          List<SupplierGeoLocation> factoryLocationList, List<InvoiceBookingProductsData> productPoCountryList, List<Entities.AudTranFaProfile> audTranFaProfiles)
        {
            var factoryAddressData = factoryLocationList?.FirstOrDefault(x => x.FactoryId == bookingItem?.FactoryId);
            var endDate = DateTime.DaysInMonth(bookingItem.ServiceDateFrom.Year, bookingItem.ServiceDateFrom.Month);

            var monthEnd = endDate == 31 ? endDate + MonthWord : endDate + MonthWord1;
            double extraFees = 0;

            //get short month name
            var monthShortName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(bookingItem.ServiceDateFrom.Month);

            invoiceData.TravelHotelCost = (invoiceDetailData?.TotalTravelFee + invoiceDetailData?.HotelCost).ToString() ?? string.Empty;

            invoiceData.TotalInvoiceFees = (invoiceDetailData?.TotalInvoiceFees + (Double.TryParse(invoiceData?.ExtrafeeTotal, out extraFees) ? extraFees : 0)).ToString() ?? string.Empty;
            invoiceData.TaxValue = invoiceDetailData?.TaxValue?.ToString() ?? string.Empty;

            invoiceData.UnitPrice = invoiceDetailData?.UnitPrice?.ToString() ?? string.Empty;

            invoiceData.Discount = invoiceDetailData?.Discount?.ToString() ?? string.Empty;
            invoiceData.Currency = invoiceDetailData?.Currency ?? string.Empty;
            invoiceData.ManDay = invoiceDetailData?.ManDay?.ToString() ?? string.Empty;

            invoiceData.Description = MonthDesc + " " + monthShortName + " " + Month1stWord + " - " + monthShortName + " " + monthEnd;

            invoiceData.BookingNo = bookingItem?.BookingNo.ToString() ?? string.Empty;
            invoiceData.QuotationNo = QuotationNoList?.Where(x => x.BookingNo == bookingItem?.BookingNo).Select(x => x.QuotationNo.ToString() ?? string.Empty).FirstOrDefault() ?? string.Empty;
            invoiceData.CustomerBookingNo = bookingItem?.CustomerBookingNo ?? string.Empty;

            invoiceData.CustomerName = bookingItem?.CustomerName ?? string.Empty;
            invoiceData.SupplierName = bookingItem?.SupplierName ?? string.Empty;
            invoiceData.SupplierCode = bookingItem?.SupplierCode ?? string.Empty;
            invoiceData.FactoryName = bookingItem?.FactoryName ?? string.Empty;
            invoiceData.CustomerPriceCategory = bookingItem?.CustomerPriceCategory ?? string.Empty;

            invoiceData.FactoryCountry = factoryAddressData?.Country ?? string.Empty;
            invoiceData.FactoryProvince = factoryAddressData?.Province ?? string.Empty;
            invoiceData.FactoryCity = factoryAddressData?.City ?? string.Empty;
            invoiceData.FactoryCounty = factoryAddressData?.County ?? string.Empty;
            invoiceData.FactoryTown = factoryAddressData?.Town ?? string.Empty;

            // audit has single mapping
            invoiceData.CustomerBrand = bookingItem?.Brand ?? string.Empty;
            invoiceData.CustomerDepartment = bookingItem?.CustomerDept ?? string.Empty;

            invoiceData.CustomerCollection = bookingItem?.CustomerCollection ?? string.Empty;
            invoiceData.CustomerProductCategory = ProductList?.Where(x => x.BookingId == bookingItem?.BookingNo).Select(x => x.ProductCategory).FirstOrDefault() ?? string.Empty;

            invoiceData.ServiceDateFromToDate = bookingItem?.ServiceDateFrom == bookingItem?.ServiceDateTo ? bookingItem?.ServiceDateFrom.ToString(StandardDateFormat)
                 : bookingItem?.ServiceDateFrom.ToString(StandardDateFormat) + " - " +
                bookingItem?.ServiceDateTo.ToString(StandardDateFormat);
            invoiceData.ServiceType = string.Join(", ", serviceTypeList?.Where(x => x.BookingNo == bookingItem?.BookingNo).Select(x => x.ServiceTypeName).ToList()) ?? string.Empty;
            invoiceData.TotalbookingQty = totalQtyBookingList?.Where(x => x.BookingNo == bookingItem?.BookingNo).Select(x => x.BookingQty?.ToString()).FirstOrDefault() ?? string.Empty;
            invoiceData.TotalInspectedQty = totalQtyBookingList?.Where(x => x.BookingNo == bookingItem?.BookingNo).Select(x => x.InspectedQuantity?.ToString()).FirstOrDefault() ?? string.Empty;

            invoiceData.AirCost = invoiceDetailData?.AirCost?.ToString() ?? string.Empty;

            invoiceData.HotelCost = invoiceDetailData?.HotelCost?.ToString() ?? string.Empty;

            invoiceData.LandCost = invoiceDetailData?.LandCost?.ToString() ?? string.Empty;

            invoiceData.OtherCost = invoiceDetailData?.OtherCost?.ToString() ?? string.Empty;

            invoiceData.TravelTotalCost = invoiceDetailData?.TotalTravelFee?.ToString() ?? string.Empty;

            invoiceData.InspFee = invoiceDetailData?.InspFee?.ToString() ?? string.Empty;

            invoiceData.TravelOtherFees = invoiceDetailData?.TravelOtherFees?.ToString() ?? string.Empty;

            invoiceData.DestinationCountry = string.Join(", ", productPoCountryList?.Where(x => x.BookingId == bookingItem?.BookingNo)
                                                    .Select(x => x.DestinationCountry).Distinct().ToList());
            invoiceData.BookingPO = string.Join(", ", productPoCountryList?.Where(x => x.BookingId == bookingItem?.BookingNo)
                                                    .Select(x => x.PoNumber).Distinct().ToList());
            invoiceData.TotalStaff = audTranFaProfiles?.FirstOrDefault(x => x.AuditId == bookingItem.BookingNo)?.TotalStaff?.ToString();
            return invoiceData;
        }

        //invoice details map
        public InvoiceDetailsPreview InvoiceDataMap(InvoiceDetailsPreview invoiceData, InvoiceDetailsRepo invoiceRepoData,
                    List<BilledContactsName> billedContactsList, List<ExtraFeeData> extrafees)
        {
            if (extrafees != null)
            {
                var extraFee = extrafees.FirstOrDefault(x => x.InvoiceId == invoiceRepoData.Id && x.BilledTo == invoiceRepoData.InvoiceTo.GetValueOrDefault())?.ExtraFee.GetValueOrDefault() ?? 0;
                invoiceData.ExtrafeeTotal = extraFee.ToString();
            }

            invoiceData.Remarks = invoiceRepoData?.Remarks ?? string.Empty;
            invoiceData.Subject = invoiceRepoData?.Subject ?? string.Empty;
            invoiceData.InvoiceDate = invoiceRepoData?.InvoiceDate?.ToString(StandardDateFormat) ?? string.Empty;
            invoiceData.InvoiceNumber = invoiceRepoData?.InvoiceNumber ?? string.Empty;
            invoiceData.PaymentTerm = invoiceRepoData?.PaymentTerm ?? string.Empty;
            invoiceData.CurrentDate = DateTime.Now.ToString(StandardDateFormat) ?? string.Empty;
            invoiceData.PostDate = invoiceRepoData?.PostDate?.ToString(StandardDateFormat) ?? string.Empty;

            invoiceData.BilledAddress = invoiceRepoData?.BilledAddress ?? string.Empty;
            invoiceData.BilledName = invoiceRepoData?.BilledName ?? string.Empty;

            if (invoiceRepoData.BilledMethod > 0)
            {
                invoiceData.BillingMethod = invoiceRepoData?.BilledMethod == (int)BillingMethodEnum.Sampling ? BillingMethodEnum.Sampling.ToString() :
                    invoiceRepoData?.BilledMethod == (int)BillingMethodEnum.ManDay ? BillingMethodEnum.ManDay.ToString() : string.Empty;
            }

            invoiceData.PaymentDuration = invoiceRepoData?.PaymentDuration ?? string.Empty;

            invoiceData.DueDate = int.TryParse(invoiceRepoData?.PaymentDuration, out int defaultout) ?
                invoiceRepoData?.InvoiceDate?.AddDays(defaultout).ToString(StandardDateFormat) : string.Empty;

            if (invoiceRepoData?.InvoiceTo > 0 && billedContactsList != null)
            {
                if (invoiceRepoData?.InvoiceTo == (int)InvoiceTo.Customer)
                {
                    invoiceData.BilledContacts = string.Join(", ", billedContactsList?.Where(x => !string.IsNullOrWhiteSpace(x.CustomerContactName))
                        .Select(x => x.CustomerContactName).Distinct().ToList());
                }
                else if (invoiceRepoData?.InvoiceTo == (int)InvoiceTo.Supplier)
                {
                    invoiceData.BilledContacts = string.Join(", ", billedContactsList?.Where(x => !string.IsNullOrWhiteSpace(x.SupplierContactName))
                        .Select(x => x.SupplierContactName).Distinct().ToList());
                }
                else if (invoiceRepoData?.InvoiceTo == (int)InvoiceTo.Factory)
                {
                    invoiceData.BilledContacts = string.Join(", ", billedContactsList?.Where(x => !string.IsNullOrWhiteSpace(x.FactoryContactName))
                        .Select(x => x.FactoryContactName).Distinct().ToList());
                }
            }

            invoiceData.BankId = invoiceRepoData?.AccountId.ToString() ?? string.Empty;
            invoiceData.AccountName = invoiceRepoData?.AccountName ?? string.Empty;
            invoiceData.AccountNumber = invoiceRepoData?.AccountNumber ?? string.Empty;
            invoiceData.BankAddress = invoiceRepoData?.BankAddress ?? string.Empty;
            invoiceData.BankName = invoiceRepoData?.BankName ?? string.Empty;
            invoiceData.BankSwiftCode = invoiceRepoData?.BankSwiftCode ?? string.Empty;

            invoiceData.OfficeAddress = invoiceRepoData?.OfficeAddress ?? string.Empty;
            invoiceData.OfficeFax = invoiceRepoData?.OfficeFax ?? string.Empty;
            invoiceData.OfficeMail = invoiceRepoData?.OfficeMail ?? string.Empty;
            invoiceData.OfficeName = invoiceRepoData?.OfficeName ?? string.Empty;
            invoiceData.OfficePhone = invoiceRepoData?.OfficePhone ?? string.Empty;
            invoiceData.OfficeWebsite = invoiceRepoData?.OfficeWebsite ?? string.Empty;
            invoiceData.BankTaxId = invoiceRepoData?.BankTaxId != null ? string.Join(",", invoiceRepoData?.BankTaxId) ?? string.Empty : string.Empty;

            invoiceData.ManualChargeBack = invoiceRepoData?.ManualChargeBack.ToString() ?? string.Empty;
            invoiceData.ManualDescription = invoiceRepoData?.ManualDescription ?? string.Empty;
            invoiceData.ManualOtherCost = invoiceRepoData?.ManualOtherCost.ToString() ?? string.Empty;
            invoiceData.ManualRemarks = invoiceRepoData?.ManualRemarks ?? string.Empty;
            invoiceData.ManualServiceFee = invoiceRepoData?.ManualServiceFee.ToString() ?? string.Empty;
            invoiceData.ServiceType = invoiceRepoData?.ServiceType ?? string.Empty;
            invoiceData.Currency = invoiceRepoData?.Currency ?? string.Empty;
            invoiceData.CustomerName = invoiceRepoData?.CustomerName ?? string.Empty;
            invoiceData.TaxValue = invoiceRepoData?.TaxValue != null ? (invoiceRepoData?.TaxValue * 100).ToString() + "%" ?? string.Empty : string.Empty;

            invoiceData.CreditRefundAmount = invoiceRepoData?.CreditRefundAmount.ToString() ?? string.Empty;
            invoiceData.CreditRemarks = invoiceRepoData?.CreditRemarks ?? string.Empty;
            invoiceData.CreditSortAmount = invoiceRepoData?.CreditSortAmount.ToString() ?? string.Empty;
            invoiceData.CreditDate = invoiceRepoData?.CreditDate?.ToString(StandardDateFormat) ?? string.Empty;
            invoiceData.CreditNumber = invoiceRepoData?.CreditNumber ?? string.Empty;
            invoiceData.TaxDate = invoiceRepoData.InvoiceDate?.ToString(StandardDateFormat) ?? string.Empty;
            invoiceData.InvoiceDateTime = invoiceRepoData.CreatedOn != null && invoiceRepoData.InvoiceDate != null ? invoiceRepoData.InvoiceDate?.Date.AddTicks(invoiceRepoData.CreatedOn.Value.TimeOfDay.Ticks).ToString(StandardDateTimeFormat) : string.Empty;
            return invoiceData;
        }
        public InvoiceBankPreview BankDetailsMap(InvoiceBankRepo item)
        {
            return new InvoiceBankPreview
            {
                InvoiceBankTaxList = new List<InvoiceBankTaxPreview>(),
                AccountName = item.AccountName,
                AccountNumber = item.AccountNumber,
                ChopLink = item.ChopLink,
                SignLink = item.SignLink,
                Id = item.Id.ToString()
            };

        }

        public InvoiceBankTaxPreview BankTaxDetailsMap(InvoiceBankTaxRepo taxItem)
        {
            return new InvoiceBankTaxPreview()
            {
                TaxId = taxItem.TaxId.ToString(),
                TaxName = taxItem.Name,
                TaxValue = taxItem.Value.ToString(),
                FromDate = taxItem.FromDate.ToString(StandardDateFormat),
                ToDate = taxItem.ToDate?.ToString(StandardDateFormat)
            };
        }

        //booking details map
        public InvoiceDetailsPreview BookingDataMapForSimpleInvoice(List<InvoiceBookingPDFDetail> bookingItem,
            List<InvoiceBookingProductsData> ProductList, List<InvoiceBookingQuantityDetails> totalQtyBookingList, InvoiceDetailsPreview invoiceData,
            List<BookingServiceType> serviceTypeList, List<InvoiceBookingQuotation> QuotationNoList, InvoiceDetailsRepo invoiceDetailData,
            List<SupplierGeoLocation> factoryLocationList, List<InvoiceBookingProductsData> productPoCountryList)
        {
            List<string> desc = new List<string>();
            List<string> cusDept = new List<string>();
            List<string> cusBuyer = new List<string>();
            List<string> cusBrand = new List<string>();
            foreach (var booking in bookingItem)
            {
                //get short month name
                var monthShortName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(booking.ServiceDateFrom.Month);

                var endDate = DateTime.DaysInMonth(booking.ServiceDateFrom.Year, booking.ServiceDateFrom.Month);

                var monthEnd = endDate == 31 ? endDate + MonthWord : endDate + MonthWord1;

                desc.Add(MonthDesc + " " + monthShortName + " " + Month1stWord + " - " + monthShortName + " " + monthEnd);

                cusDept.AddRange(booking.CustomerDepartment);

                cusBuyer.AddRange(booking.CustomerBuyer);

                cusBrand.AddRange(booking.CustomerBrand);
            }

            invoiceData.CustomerName = string.Join(" ,", bookingItem?.Select(x => x.CustomerName).Distinct()) ?? string.Empty;
            invoiceData.ServiceType = string.Join(", ", serviceTypeList?.Select(x => x.ServiceTypeName).Distinct());
            invoiceData.Description = string.Join(", ", desc.Distinct());
            invoiceData.CustomerDepartment = string.Join(" ,", cusDept.Distinct());
            invoiceData.CustomerBuyer = string.Join(" ,", cusBuyer.Distinct());
            invoiceData.CustomerCollection = string.Join(" ,", bookingItem.Select(x => x.CustomerCollection).Distinct());
            invoiceData.CustomerBrand = string.Join(" ,", cusBrand.Distinct());
            invoiceData.CustomerPriceCategory = string.Join(" ,", bookingItem?.Select(x => x.CustomerPriceCategory).Distinct()) ?? string.Empty;
            invoiceData.Currency = invoiceDetailData?.Currency ?? string.Empty;

            return invoiceData;
        }

        //extra fee details map
        public InvoiceDetailsPreview ExtraFeeMap(InvoiceDetailsPreview invoiceData, InvoiceDetailsRepo invoiceRepoData, List<ExtraFeeData> extrafees, InvoiceBookingPDFDetail bookingItem,
            List<BilledContactsName> billedContactsList)
        {
            var extraFeeData = extrafees.FirstOrDefault(x => x.ExtraFeeId == invoiceRepoData.Id && x.BilledTo == invoiceRepoData.InvoiceTo.GetValueOrDefault());

            var extraFee = extrafees.FirstOrDefault(x => x.ExtraFeeId == invoiceRepoData.Id && x.BilledTo == invoiceRepoData.InvoiceTo.GetValueOrDefault())?.ExtraFee.GetValueOrDefault() ?? 0;
            invoiceData.ExtraFee = extraFee.ToString();
            // invoiceData.Remarks = invoiceRepoData?.Remarks ?? string.Empty;            

            if (invoiceRepoData?.InvoiceTo > 0)
            {
                if (invoiceRepoData?.InvoiceTo == (int)InvoiceTo.Customer)
                {
                    invoiceData.BilledContacts = string.Join(", ", billedContactsList?.Where(x => !string.IsNullOrWhiteSpace(x.CustomerContactName))
                        .Select(x => x.CustomerContactName).Distinct().ToList());
                }
                else if (invoiceRepoData?.InvoiceTo == (int)InvoiceTo.Supplier)
                {
                    invoiceData.BilledContacts = string.Join(", ", billedContactsList?.Where(x => !string.IsNullOrWhiteSpace(x.SupplierContactName))
                        .Select(x => x.SupplierContactName).Distinct().ToList());
                }
                else if (invoiceRepoData?.InvoiceTo == (int)InvoiceTo.Factory)
                {
                    invoiceData.BilledContacts = string.Join(", ", billedContactsList?.Where(x => !string.IsNullOrWhiteSpace(x.FactoryContactName))
                        .Select(x => x.FactoryContactName).Distinct().ToList());
                }
            }
            invoiceData.InvoiceDate = invoiceRepoData?.InvoiceDate?.ToString(StandardDateFormat) ?? string.Empty;
            invoiceData.InvoiceNumber = invoiceRepoData?.InvoiceNumber ?? string.Empty;
            invoiceData.CurrentDate = DateTime.Now.ToString(StandardDateFormat) ?? string.Empty;

            invoiceData.BankId = extraFeeData?.BankId.ToString() ?? string.Empty;
            invoiceData.AccountName = extraFeeData?.AccountName ?? string.Empty;
            invoiceData.AccountNumber = extraFeeData?.AccountNumber ?? string.Empty;
            invoiceData.BankAddress = extraFeeData?.BankAddress ?? string.Empty;
            invoiceData.BankName = extraFeeData?.BankName ?? string.Empty;
            invoiceData.BankSwiftCode = extraFeeData?.BankSwiftCode ?? string.Empty;

            invoiceData.BankTaxId = string.Join(",", extraFeeData?.BankTaxList) ?? string.Empty;

            invoiceData.OfficeAddress = extraFeeData?.OfficeAddress ?? string.Empty;
            invoiceData.OfficeFax = extraFeeData?.OfficeFax ?? string.Empty;
            invoiceData.OfficeMail = extraFeeData?.OfficeMail ?? string.Empty;
            invoiceData.OfficeName = extraFeeData?.OfficeName ?? string.Empty;
            invoiceData.OfficePhone = extraFeeData?.OfficePhone ?? string.Empty;
            invoiceData.OfficeWebsite = extraFeeData?.OfficeWebsite ?? string.Empty;
            invoiceData.TaxDate = invoiceRepoData?.InvoiceDate?.ToString(StandardDateFormat) ?? string.Empty;
            invoiceData.BilledName = invoiceRepoData.BilledName;
            invoiceData.BilledAddress = invoiceRepoData.BilledAddress;
            invoiceData.PaymentDuration = invoiceRepoData.PaymentDurations.ToString();
            invoiceData.PaymentTerm = invoiceRepoData.PaymentTerm;
            return invoiceData;
        }

        public List<InvoiceSummaryItem> MapinspQuotationDataToInvoice(List<InvoiceSummaryItem> lstinvoicedata, List<QuotationInspectionTravelCost> lstquotation, List<InvoiceBookingFactoryDetails> factoryCountries)
        {
            foreach (var inv_item in lstinvoicedata.ToList())
            {
                var quinspmanday = lstquotation.FirstOrDefault(x => x.BookingId == inv_item.BookingId);
                if (quinspmanday != null && quinspmanday.Mandays > 0)
                {
                    inv_item.QuManday = (int)quinspmanday.Mandays;
                }

                var factoryCountry = factoryCountries.FirstOrDefault(x => x.FactoryId == inv_item.FactoryId);
                if (factoryCountry != null)
                {
                    inv_item.FactoryCountry = factoryCountry.FactoryCountryName;
                }
            }
            return lstinvoicedata;
        }

        public List<InvoiceBookingDetail> MapFactoryDataToInvoiceBooking(List<InvoiceBookingDetail> bookingDetails, List<InvoiceBookingFactoryDetails> factoryDetails)
        {
            foreach (var factoryData in factoryDetails)
            {
                var filteredBookingDetails = bookingDetails.Where(x => x.FactoryId == factoryData.FactoryId).ToList();

                if (filteredBookingDetails != null && filteredBookingDetails.Any())
                {
                    filteredBookingDetails.ForEach(x => x.FactoryCountryId = factoryData.FactoryCountryId);

                    filteredBookingDetails.ForEach(x => x.FactoryCountryName = factoryData.FactoryCountryName);
                    filteredBookingDetails.ForEach(x => x.FactoryCountryCode = factoryData.FactoryCountryCode);

                    filteredBookingDetails.ForEach(x => x.FactoryProvinceId = factoryData.FactoryProvinceId);
                    filteredBookingDetails.ForEach(x => x.FactoryCityId = factoryData.FactoryCityId);
                    filteredBookingDetails.ForEach(x => x.FactoryCountyId = factoryData.FactoryCountyId);
                }
            }

            return bookingDetails;
        }


        public InvTranFile MapInvoiceTranFile(SaveInvoicePdfUrl invoicePdfUrl, int invoiceId)
        {
            return new InvTranFile()
            {
                Active = true,
                CreatedOn = DateTime.Now,
                CreatedBy = invoicePdfUrl.CreatedBy,
                FileName = invoicePdfUrl.InvoiceNo,
                FilePath = invoicePdfUrl.FilePath,
                FileType = invoicePdfUrl.FileType,
                UniqueId = invoicePdfUrl.UniqueId,
                InvoiceId = invoiceId,
                InvoiceNo = invoicePdfUrl.InvoiceNo
            };
        }
    }
}
