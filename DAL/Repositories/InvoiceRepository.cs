using Contracts.Repositories;
using DTO.Claim;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
using DTO.ExtraFees;
using DTO.Invoice;
using DTO.Kpi;
using DTO.Quotation;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class InvoiceRepository : Repository, IInvoiceRepository
    {
        public InvoiceRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// get price card rules based on customer and invoicerequesttype and from and toDate
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerPriceCardRepo>> GetPriceCardRuleList(InvoiceGenerateRequest requestDto)
        {

            // filter by customer and invoicerequesttype and from and toDate
            var priceDetails = _context.CuPrDetails.Where(x => x.CustomerId == requestDto.CustomerId &&

                                                 x.Active.Value &&

                                                requestDto.RealInspectionFromDate.ToDateTime().Date <= x.PeriodTo &&

                                                requestDto.RealInspectionToDate.ToDateTime().Date >= x.PeriodFrom);

            if (requestDto.InvoicingRequest > 0)
            {
                priceDetails = priceDetails.Where(x => x.InvoiceRequestType == requestDto.InvoicingRequest);
            }

            if (requestDto.InvoiceTo > 0)
            {
                priceDetails = priceDetails.Where(x => x.BillingToId == requestDto.InvoiceTo);
            }

            return await priceDetails.Select(x => new CustomerPriceCardRepo
            {
                TariffTypeId = x.TravelMatrixTypeId,
                BillingMethodId = x.BillingMethodId,
                BillingToId = x.BillingToId,
                CustomerId = x.CustomerId,
                CustomerName = x.Customer.CustomerName,
                CurrencyId = x.CurrencyId,
                Remarks = x.Remarks,
                FreeTravelKM = x.FreeTravelKm.GetValueOrDefault(),
                Id = x.Id,
                ServiceId = x.ServiceId,
                TaxIncluded = x.TaxIncluded == null ? null : x.TaxIncluded,
                TravelIncluded = x.TravelIncluded == null ? null : x.TravelIncluded,
                UnitPrice = x.UnitPrice,
                HolidayPrice = x.HolidayPrice,
                ProductPrice = x.ProductPrice,
                PriceToEachProduct = x.PriceToEachProduct,
                PeriodFrom = Static_Data_Common.GetCustomDate(x.PeriodFrom),
                PeriodTo = Static_Data_Common.GetCustomDate(x.PeriodTo),
                TravelMatrixTypeId = x.TravelMatrixTypeId,
                MaxProductCount = x.MaxProductCount,
                SampleSizeBySet = x.SampleSizeBySet,
                MinBillingDay = x.MinBillingDay,
                MaxSampleSize = x.MaxSampleSize,
                AdditionalSampleSize = x.AdditionalSampleSize,
                AdditionalSamplePrice = x.AdditionalSamplePrice,
                Quantity8 = x.Quantity8,
                Quantity13 = x.Quantity13,
                Quantity20 = x.Quantity20,
                Quantity32 = x.Quantity32,
                Quantity50 = x.Quantity50,
                Quantity80 = x.Quantity80,
                Quantity125 = x.Quantity125,
                Quantity200 = x.Quantity200,
                Quantity315 = x.Quantity315,
                Quantity500 = x.Quantity500,
                Quantity800 = x.Quantity800,
                Quantity1250 = x.Quantity1250,

                InvoiceRequestSelectAll = x.InvoiceRequestSelectAll,
                IsInvoiceConfigured = x.IsInvoiceConfigured,

                InvoiceRequestType = x.InvoiceRequestType,
                InvoiceRequestAddress = x.InvoiceRequestAddress,
                InvoiceRequestBilledName = x.InvoiceRequestBilledName,

                BillingEntity = x.BillingEntity,
                BankAccount = x.BankAccount,
                PaymentDuration = x.PaymentDuration,
                PaymentTerms = x.PaymentTerms,
                BillQuantityType = x.BilledQuantityType,

                InvoiceNoDigit = x.InvoiceNoDigit,
                InvoiceNoPrefix = x.InvoiceNoPrefix,

                InvoiceOffice = x.InvoiceOffice,

                InvoiceInspFeeFrom = x.InvoiceInspFeeFrom,

                InvoiceHotelFeeFrom = x.InvoiceHotelFeeFrom,
                InvoiceDiscountFeeFrom = x.InvoiceDiscountFeeFrom,
                InvoiceOtherFeeFrom = x.InvoiceOtherFeeFrom,
                InvoiceTravelExpense = x.InvoiceTmfeeFrom,
                MandayBuffer = x.MandayBuffer,
                MandayProductivity = x.ManDayProductivity,
                MandayReports = x.MandayReportCount,
                SubCategorySelectAll = x.SubCategorySelectAll,
                PriceComplexType = x.PriceComplexType,
                InterventionType = x.InterventionType

            }).ToListAsync();
        }

        /// <summary>
        /// Get travel expense invoice order ids
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<int>> GetTravelExpenseInvoiceOrderIds(InvoiceGenerateRequest requestDto)
        {
            return await _context.InvAutTranDetails.Where(x =>
                      x.InspectionId != null &&
                      x.ServiceId == requestDto.Service &&
                      x.Inspection.CustomerId == requestDto.CustomerId &&
                      x.Inspection.ServiceDateTo >= requestDto.RealInspectionFromDate.ToDateTime().Date &&
                      x.Inspection.ServiceDateTo <= requestDto.RealInspectionToDate.ToDateTime().Date &&
                      x.IsTravelExpense.Value &&
                      x.InvoiceStatus != (int)InvoiceStatus.Cancelled).
                         Select(x => x.InspectionId.GetValueOrDefault()).Distinct().ToListAsync();

        }


        /// <summary>
        /// get travel expense invoice audit order ids
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<int>> GetTravelExpenseInvoiceAuditOrderIds(InvoiceGenerateRequest requestDto)
        {
            return await _context.InvAutTranDetails.Where(x =>
                       x.AuditId != null &&
                       x.ServiceId == requestDto.Service &&
                       x.Audit.CustomerId == requestDto.CustomerId &&
                       x.Audit.ServiceDateTo >= requestDto.RealInspectionFromDate.ToDateTime().Date &&
                       x.Audit.ServiceDateTo <= requestDto.RealInspectionToDate.ToDateTime().Date &&
                       x.IsTravelExpense.Value &&
                       x.InvoiceStatus != (int)InvoiceStatus.Cancelled).
                         Select(x => x.AuditId.GetValueOrDefault()).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get inspection invoice order ids
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>

        public async Task<List<int>> GetInspectionInvoiceOrderIds(InvoiceGenerateRequest requestDto)
        {
            return await _context.InvAutTranDetails.Where(x =>
                      x.InspectionId != null &&
                      x.ServiceId == requestDto.Service &&
                      x.Inspection.CustomerId == requestDto.CustomerId &&
                      x.Inspection.ServiceDateTo >= requestDto.RealInspectionFromDate.ToDateTime().Date &&
                      x.Inspection.ServiceDateTo <= requestDto.RealInspectionToDate.ToDateTime().Date &&
                      x.IsInspection.Value &&
                      x.InvoiceStatus != (int)InvoiceStatus.Cancelled).
                         Select(x => x.InspectionId.GetValueOrDefault()).Distinct().ToListAsync();
        }
        /// <summary>
        /// get quotation inspection invoice order ids
        /// </summary>
        /// <param name="bookingList"></param>
        /// <returns></returns>
        public async Task<List<QuotationBooking>> GetQuotationInspectionInvoiceOrderIds(List<int> bookingList)
        {
            return await _context.InvAutTranDetails.Where(x =>
                      x.InspectionId != null && bookingList.Contains(x.InspectionId.GetValueOrDefault()) &&
                      x.InvoiceStatus != (int)InvoiceStatus.Cancelled).
                         Select(x =>
                         new QuotationBooking
                         {
                             BookingId = x.InspectionId.GetValueOrDefault(),
                             QuotationId = x.Inspection.QuQuotationInsps.Select(x => x.IdQuotation).FirstOrDefault()
                         }
                         ).Distinct().ToListAsync();
        }


        public async Task<List<int>> GetAuditInvoiceOrderIds(InvoiceGenerateRequest requestDto)
        {
            return await _context.InvAutTranDetails.Where(x =>
                      x.AuditId != null &&
                      x.ServiceId == requestDto.Service &&
                      x.Audit.CustomerId == requestDto.CustomerId &&
                      x.Audit.ServiceDateTo >= requestDto.RealInspectionFromDate.ToDateTime().Date &&
                      x.Audit.ServiceDateTo <= requestDto.RealInspectionToDate.ToDateTime().Date &&
                      x.IsInspection.Value &&
                      x.InvoiceStatus != (int)InvoiceStatus.Cancelled).
                         Select(x => x.AuditId.GetValueOrDefault()).Distinct().ToListAsync();
        }

        /// <summary>
        /// get invoiced audit booking information with quotation ids
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<List<QuotationBooking>> GetQuotationAuditInvoiceOrderIds(List<int> bookingIdList)
        {
            return await _context.InvAutTranDetails.Where(x =>
                      x.AuditId != null
                      && bookingIdList.Contains(x.InspectionId.GetValueOrDefault()) &&
                      x.InvoiceStatus != (int)InvoiceStatus.Cancelled).
                       Select(x =>
                         new QuotationBooking
                         {
                             BookingId = x.InspectionId.GetValueOrDefault(),
                             QuotationId = x.Audit.QuQuotationAudits.FirstOrDefault().IdQuotation
                         }
                         ).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get invoice order with same factory and same date
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="factoryId"></param>
        /// <param name="inspectionDate"></param>
        /// <returns></returns>
        public async Task<List<InvAutTranDetail>> GetInvoiceOrdersSameFacIdAndInspectionDate(int customerId, int factoryId, DateTime inspectionDate)
        {
            return await _context.InvAutTranDetails.Where(x =>
                    x.Inspection.CustomerId == customerId &&
                    x.Inspection.FactoryId == factoryId &&
                    x.Inspection.ServiceDateTo == inspectionDate && x.InvoiceStatus != (int)InvoiceStatus.Cancelled &&
                    x.InvoiceTo != (int)InvoiceTo.Customer)
                    .ToListAsync();
        }
        /// <summary>
        /// get invoice booking date 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public DateTime GetInvoiceBookingLastDate(IEnumerable<int?> bookingIds)
        {
            return _context.InspTransactions.Where(x => bookingIds.Contains(x.Id)).
                              Select(x => x.ServiceDateTo).Max();
        }

        /// <summary>
        /// get invoice audit last date
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public DateTime GetInvoiceAuditLastDate(IEnumerable<int?> auditIds)
        {
            return _context.AudTransactions.Where(x => auditIds.Contains(x.Id)).
                              Select(x => x.ServiceDateTo).Max();
        }
        /// <summary>
        /// get invoice details by invoice no
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        public async Task<InvoiceDataForNewBooking> GetInvoiceDetailsbyInvoiceNumber(string invoiceNumber)
        {
            return await _context.InvAutTranDetails.
                              Where(x => x.InvoiceNo == invoiceNumber && x.InvoiceStatus != (int)InvoiceStatus.Cancelled).
                              Select(x =>
                              new InvoiceDataForNewBooking()
                              {
                                  InvoiceId = x.Id,
                                  InvoiceDate = x.InvoiceDate,
                                  PaymentDate = x.InvoicePaymentDate,
                                  PaymentStatus = x.InvoicePaymentStatus
                              }).IgnoreQueryFilters().FirstOrDefaultAsync();
        }


        /// <summary>
        /// get travel matrix data
        /// </summary>
        /// <param name="matrixRequest"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TravelMatrixSearch>> GetMatrixData(QuotationMatrixRequest matrixRequest)
        {
            var queryResult = _context.InvTmDetails.Where(x => x.Active.Value && x.CountyId == matrixRequest.CountyId &&
                         x.SourceCurrencyId == matrixRequest.CurrencyId);

            // matrix type customized then apply customer filter
            if (matrixRequest.MatrixTypeId == (int)TravelMatrixType.Customized)
            {
                queryResult = queryResult.Where(x => x.CustomerId == matrixRequest.customerId);
            }
            else  // matrix type not customized then apply actual type
            {
                queryResult = queryResult.Where(x => x.TravelMatrixTypeId == matrixRequest.MatrixTypeId);
            }

            return await queryResult
           .Select(x => new TravelMatrixSearch
           {
               Id = x.Id,
               CityId = x.CityId,
               CountryId = x.CountryId,
               CountyId = x.CountyId,
               ProvinceId = x.ProvinceId,
               CustomerId = x.CustomerId,
               AirCost = x.AirCost,
               BusCost = x.BusCost,
               HotelCost = x.HotelCost,
               TaxiCost = x.TaxiCost,
               TrainCost = x.TrainCost,
               DistanceKM = x.DistanceKm,
               FixExchangeRate = x.FixedExchangeRate,
               MarkUpCostAir = x.MarkUpAirCost,
               SourceCurrencyId = x.SourceCurrencyId,
               TravelCurrencyId = x.TravelCurrencyId,
               MarkUpCost = x.MarkUpCost,
               TravelTime = x.TravelTime,
               OtherCost = x.OtherCost,
               InspPortId = x.InspPortCountyId,
               Remarks = x.Remarks,
               TariffTypeId = x.TravelMatrixTypeId,
               CustomerName = x.Customer.CustomerName,
               CountryName = x.Country.CountryName,
               ProvinceName = x.Province.ProvinceName,
               CityName = x.City.CityName,
               CountyName = x.County.CountyName,
               TariffTypeName = x.TravelMatrixType.Name,
               InspPortName = x.InspPortCounty.CountyName,
               SourceCurrencyName = x.SourceCurrency.CurrencyName,
               TravelCurrencyName = x.TravelCurrency.CurrencyName
           }).ToListAsync();
        }

        /// <summary>
        /// Get Quotation Travel Costs
        /// </summary>
        /// <param name="bookingNo"></param>
        /// <returns></returns>
        public async Task<List<QuotationInspectionTravelCost>> GetQuotationDataByBookingNo(int bookingNo)
        {
            return await _context.QuQuotationInsps.Where(x => x.IdBooking == bookingNo &&
                                                   x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                                                    Select(x => new QuotationInspectionTravelCost
                                                    {
                                                        QuotationId = x.IdQuotationNavigation.Id,
                                                        InspectionFees = x.InspFees.GetValueOrDefault(),
                                                        Mandays = x.NoOfManDay.GetValueOrDefault(),
                                                        TravelAirCost = x.TravelAir.GetValueOrDefault(),
                                                        TravelLandCost = x.TravelLand.GetValueOrDefault(),
                                                        TravelHotelCost = x.TravelHotel.GetValueOrDefault(),
                                                        OtherCost = x.IdQuotationNavigation.OtherCosts.GetValueOrDefault(),
                                                        Discount = x.IdQuotationNavigation.Discount.GetValueOrDefault(),
                                                        TotalCost = x.IdQuotationNavigation.TotalCost,
                                                        BillingTo = x.IdQuotationNavigation.BillingPaidById,
                                                        InvoiceType = x.IdQuotationNavigation.BillingMethodId

                                                    }).ToListAsync();
        }

        /// <summary>
        /// get quotation data by booking number list
        /// </summary>
        /// <param name="bookingNo"></param>
        /// <returns></returns>
        public async Task<List<QuotationInspectionTravelCost>> GetQuotationDataByBookingIdsList(List<int> bookingNo)
        {
            return await _context.QuQuotationInsps.Where(x => bookingNo.Contains(x.IdBooking) &&
                                                   x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                                                    Select(x => new QuotationInspectionTravelCost
                                                    {
                                                        BookingId = x.IdBooking,
                                                        QuotationId = x.IdQuotationNavigation.Id,
                                                        InspectionFees = x.InspFees.GetValueOrDefault(),
                                                        UnitPrice = x.UnitPrice.GetValueOrDefault(),
                                                        Mandays = x.NoOfManDay.GetValueOrDefault(),
                                                        TravelAirCost = x.TravelAir.GetValueOrDefault(),
                                                        TravelLandCost = x.TravelLand.GetValueOrDefault(),
                                                        TravelHotelCost = x.TravelHotel.GetValueOrDefault(),
                                                        OtherCost = x.IdQuotationNavigation.OtherCosts.GetValueOrDefault(),
                                                        Discount = x.IdQuotationNavigation.Discount.GetValueOrDefault(),
                                                        TotalCost = x.IdQuotationNavigation.TotalCost,
                                                        BillingTo = x.IdQuotationNavigation.BillingPaidById,
                                                        InvoiceType = x.IdQuotationNavigation.PaymentTerms.GetValueOrDefault(),
                                                        RuleId = x.IdQuotationNavigation.RuleId,
                                                        SuggestedManday = x.SuggestedManday,
                                                        PaymentTermsValue = x.IdQuotationNavigation.PaymentTermsValue,
                                                        PaymentTermsCount = x.IdQuotationNavigation.PaymentTermsCount
                                                    }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Quotation Data By AuditNo
        /// </summary>
        /// <param name="auditNo"></param>
        /// <returns></returns>
        public async Task<List<QuotationInspectionTravelCost>> GetQuotationDataByAuditNo(int auditNo)
        {
            return await _context.QuQuotationAudits.Where(x => x.IdBooking == auditNo &&
                                                   x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                                                    Select(x => new QuotationInspectionTravelCost
                                                    {
                                                        QuotationId = x.IdQuotationNavigation.Id,
                                                        InspectionFees = x.InspFees.GetValueOrDefault(),
                                                        Mandays = x.NoOfManDay.GetValueOrDefault(),
                                                        TravelAirCost = x.TravelAir.GetValueOrDefault(),
                                                        TravelLandCost = x.TravelLand.GetValueOrDefault(),
                                                        TravelHotelCost = x.TravelHotel.GetValueOrDefault(),
                                                        OtherCost = x.IdQuotationNavigation.OtherCosts.GetValueOrDefault(),
                                                        Discount = x.IdQuotationNavigation.Discount.GetValueOrDefault(),
                                                        TotalCost = x.IdQuotationNavigation.TotalCost,
                                                        InvoiceType = x.IdQuotationNavigation.BillingMethodId,
                                                        BillingTo = x.IdQuotationNavigation.BillingPaidById
                                                    }).ToListAsync();
        }
        /// <summary>
        /// get quotation data by audit booking list
        /// </summary>
        /// <param name="auditNo"></param>
        /// <returns></returns>
        public async Task<List<QuotationInspectionTravelCost>> GetQuotationDataByAuditList(List<int> auditNo)
        {
            return await _context.QuQuotationAudits.Where(x => auditNo.Contains(x.IdBooking) &&
                                                   x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                                                    Select(x => new QuotationInspectionTravelCost
                                                    {
                                                        QuotationId = x.IdQuotationNavigation.Id,
                                                        BookingId = x.IdBooking,
                                                        UnitPrice = x.UnitPrice.GetValueOrDefault(),
                                                        InspectionFees = x.InspFees.GetValueOrDefault(),
                                                        Mandays = x.NoOfManDay.GetValueOrDefault(),
                                                        TravelAirCost = x.TravelAir.GetValueOrDefault(),
                                                        TravelLandCost = x.TravelLand.GetValueOrDefault(),
                                                        TravelHotelCost = x.TravelHotel.GetValueOrDefault(),
                                                        OtherCost = x.IdQuotationNavigation.OtherCosts.GetValueOrDefault(),
                                                        Discount = x.IdQuotationNavigation.Discount.GetValueOrDefault(),
                                                        TotalCost = x.IdQuotationNavigation.TotalCost,
                                                        BillingTo = x.IdQuotationNavigation.BillingPaidById,
                                                        PaymentTermsValue = x.IdQuotationNavigation.PaymentTermsValue,
                                                        PaymentTermsCount = x.IdQuotationNavigation.PaymentTermsCount
                                                    }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get billed to based on booking list
        /// </summary>
        /// <param name="bookingNoList"></param>
        /// <returns></returns>
        public async Task<List<QuotationInspectionBilledTo>> GetQuotationDataByBookingNoList(IEnumerable<int> bookingNoList)
        {
            return await _context.QuQuotationInsps.Where(x => bookingNoList.Contains(x.IdBooking) &&
                                                   x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                                                    Select(x => new QuotationInspectionBilledTo
                                                    {
                                                        BookingId = x.IdBooking,
                                                        BillingTo = x.IdQuotationNavigation.BillingPaidById
                                                    }).ToListAsync();
        }

        /// <summary>
        /// get billed to based on audits
        /// </summary>
        /// <param name="auditNoList"></param>
        /// <returns></returns>
        public async Task<List<QuotationAuditBilledTo>> GetQuotationDataByAuditNoList(IEnumerable<int> auditNoList)
        {
            return await _context.QuQuotationAudits.Where(x => auditNoList.Contains(x.IdBooking) &&
                                                   x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                                                    Select(x => new QuotationAuditBilledTo
                                                    {
                                                        AuditId = x.IdBooking,
                                                        BillingTo = x.IdQuotationNavigation.BillingPaidById
                                                    }).ToListAsync();
        }

        public async Task<int> GetNumberOfQCPerDayByFactory(int factoryId, DateTime serviceDate, List<int> bookingIds)
        {
            return await _context.SchScheduleQcs
                         .Where(x => x.Active && x.Booking.FactoryId == factoryId && x.ServiceDate.Date == serviceDate.Date
                          && bookingIds.Contains(x.BookingId)
                            && x.Qctype == (int)QCType.QC)
                .CountAsync();
        }

        public IQueryable<InvoiceBookingDetail> GetAllInvoiceBookingData()
        {
            return _context.InspTransactions.
                    Select(x => new InvoiceBookingDetail
                    {
                        CustomerId = x.CustomerId,
                        SupplierId = x.SupplierId,
                        FactoryId = x.FactoryId,
                        BookingId = x.Id,
                        CustomerBookingNo = x.CustomerBookingNo,
                        StatusId = x.StatusId,

                        PriceCategoryId = x.PriceCategoryId.GetValueOrDefault(),
                        ServiceFrom = x.ServiceDateFrom,
                        ServiceTo = x.ServiceDateTo,
                        OfficeId = x.OfficeId.GetValueOrDefault()
                    }).OrderByDescending(x => x.ServiceTo);
        }

        public async Task<List<InvoiceBookingFactoryDetails>> GetBookingFactoryDetails(IEnumerable<int> factoryIds)
        {
            return await _context.SuAddresses.Where(x => factoryIds.Contains(x.SupplierId) && x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).
                        Select(x => new InvoiceBookingFactoryDetails()
                        {
                            FactoryId = x.SupplierId,
                            FactoryCountryId = x.CountryId,
                            FactoryCountryName = x.Country.CountryName,
                            FactoryCountryCode = x.Country.Alpha2Code,
                            FactoryProvinceId = x.RegionId,
                            FactoryCityId = x.CityId,
                            FactoryCountyId = x.CountyId
                        }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get all invoice audit data
        /// </summary>
        /// <returns></returns>

        public IQueryable<InvoiceBookingDetail> GetAllInvoiceAuditData()
        {
            return _context.AudTransactions.
                    Select(x => new InvoiceBookingDetail
                    {
                        CustomerId = x.CustomerId,
                        SupplierId = x.SupplierId,
                        FactoryId = x.FactoryId,
                        AuditId = x.Id,
                        StatusId = x.StatusId,

                        FactoryCountryId = x.Factory.SuAddresses.Where(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).
                                              Select(y => y.CountryId).FirstOrDefault(),

                        FactoryCityId = x.Factory.SuAddresses.Where(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).
                                              Select(y => y.CityId).FirstOrDefault(),

                        FactoryProvinceId = x.Factory.SuAddresses.Where(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).
                                              Select(y => y.City.ProvinceId).FirstOrDefault(),

                        FactoryCountyId = x.Factory.SuAddresses.
                        Where(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Select(y => y.CountyId).FirstOrDefault(),


                        FactoryCountryName = x.Factory.SuAddresses.
                                               Where(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).
                                               Select(y => y.Country.Alpha2Code).FirstOrDefault(),
                        AuditBrandId = x.BrandId,
                        AuditDepartmentId = x.DepartmentId,
                        TotalStaff = x.AudTranFaProfiles.FirstOrDefault(x => x.Active).TotalStaff,
                        ServiceFrom = x.ServiceDateFrom,
                        ServiceTo = x.ServiceDateTo,
                        OfficeId = x.OfficeId.GetValueOrDefault()
                    }).OrderByDescending(x => x.ServiceTo);
        }

        /// <summary>
        /// Get invoice new booking list
        /// </summary>
        /// <returns></returns>
        public IQueryable<InvoiceNewBookingDetailRepo> GetNewInvoiceBookingData()
        {
            return _context.InspTransactions.
                    Select(x => new InvoiceNewBookingDetailRepo
                    {
                        BookingId = x.Id,
                        BookingQuantity = x.InspProductTransactions.Where(y => y.Active.Value).Select(y => y.TotalBookingQuantity).Sum(),

                        CustomerId = x.CustomerId,
                        SupplierId = x.SupplierId,
                        FactoryId = x.FactoryId,

                        StatusId = x.StatusId,

                        InvoiceStatus = x.InvAutTranDetails.Select(y => y.InvoiceStatus).FirstOrDefault(),
                        CustomerName = x.Customer.CustomerName,
                        FactoryName = x.Factory.SupplierName,
                        SupplierName = x.Supplier.SupplierName,

                        PriceCategoryId = x.PriceCategoryId.GetValueOrDefault(),
                        PriceCategoryName = x.PriceCategory.Name,

                        ServiceFromDate = x.ServiceDateFrom,
                        ServiceToDate = x.ServiceDateTo,

                        ServiceTypeName = x.InspTranServiceTypes.Where(y => y.Active).Select(y => y.ServiceType.Name).FirstOrDefault(),
                    }).OrderByDescending(x => x.ServiceToDate);
        }

        /// <summary>
        /// Audit new invoice booking data
        /// </summary>
        /// <returns></returns>
        public IQueryable<InvoiceNewBookingDetailRepo> GetNewInvoiceAuditBookingData()
        {
            return _context.AudTransactions.
                    Select(x => new InvoiceNewBookingDetailRepo
                    {
                        BookingId = x.Id,
                        CustomerId = x.CustomerId,
                        SupplierId = x.SupplierId,
                        FactoryId = x.FactoryId,
                        StatusId = x.StatusId,
                        InvoiceStatus = x.InvAutTranDetails.Select(y => y.InvoiceStatus).FirstOrDefault(),
                        CustomerName = x.Customer.CustomerName,
                        FactoryName = x.Factory.SupplierName,
                        SupplierName = x.Supplier.SupplierName,
                        ServiceFromDate = x.ServiceDateFrom,
                        ServiceToDate = x.ServiceDateTo,
                        ServiceTypeName = x.AudTranServiceTypes.Where(y => y.Active).Select(y => y.ServiceType.Name).FirstOrDefault(),
                    }).OrderByDescending(x => x.ServiceToDate);
        }

        public async Task<IEnumerable<InvoiceBankTax>> GetTaxDetails(int bankId, DateTime maxInspectionDate, DateTime minInspectionDate)
        {
            return await _context.InvTranBankTaxes.
                 Where(x => x.Active == true && x.BankId == bankId
                 && (x.FromDate <= maxInspectionDate)
                 && ((x.ToDate == null) || (x.ToDate >= minInspectionDate)))
                .Select(x => new InvoiceBankTax
                {
                    TaxName = x.TaxName,
                    TaxValue = x.TaxValue,
                    Id = x.Id
                }).ToListAsync();
        }

        public async Task<IEnumerable<BankTaxData>> GetBankTaxDetails(List<int> bankIdList)
        {
            return await _context.InvTranBankTaxes.
                 Where(x => x.Active && bankIdList.Contains(x.BankId))
                .Select(x => new BankTaxData
                {
                    TaxName = x.TaxName,
                    TaxValue = x.TaxValue,
                    BankId = x.BankId
                }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<BankTaxData>> GetBankValidTaxDetails(List<int> bankIdList)
        {
            var todayDate = DateTime.Now.Date;
            return await _context.InvTranBankTaxes.
                 Where(x => x.Active && bankIdList.Contains(x.BankId) && x.ToDate != null && x.ToDate.Value.Date >= todayDate)
                .Select(x => new BankTaxData
                {
                    TaxName = x.TaxName,
                    TaxValue = x.TaxValue,
                    BankId = x.BankId
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<PriceInvoiceRequest>> GetInvoiceRequestAddressByBrand(IEnumerable<int?> RuleIds, IEnumerable<int> brandIdList)
        {
            if (brandIdList.Any())
            {
                return await _context.InvTranInvoiceRequests.
                                Where(x => x.Active == true && RuleIds.Contains(x.CuPriceCardId.GetValueOrDefault()) && brandIdList.Contains(x.BrandId.GetValueOrDefault()))
                               .Select(x => new PriceInvoiceRequest
                               {
                                   BilledName = x.BilledName,
                                   BilledAddress = x.BilledAddress,
                                   Id = x.Id,
                                   CuPriceCardId = x.CuPriceCardId
                                   //  InvoiceRequestContactList = x.InvTranInvoiceRequestContacts.Where(z => z.Active.Value).Select(y => y.ContactId.GetValueOrDefault()).ToList()
                               }).ToListAsync();
            }
            else
            {
                return await _context.InvTranInvoiceRequests.
                                Where(x => x.Active == true && RuleIds.Contains(x.CuPriceCardId.GetValueOrDefault()))
                               .Select(x => new PriceInvoiceRequest
                               {
                                   BilledName = x.BilledName,
                                   BilledAddress = x.BilledAddress,
                                   Id = x.Id,
                                   CuPriceCardId = x.CuPriceCardId
                                   // InvoiceRequestContactList = x.InvTranInvoiceRequestContacts.Where(z => z.Active.Value).Select(y => y.ContactId.GetValueOrDefault()).ToList()
                               }).ToListAsync();
            }

        }

        public async Task<List<PriceInvoiceRequest>> GetInvoiceRequestAddressByDepartment(IEnumerable<int?> RuleIds, IEnumerable<int> DepartmentIds)
        {
            if (DepartmentIds.Any())
            {
                return await _context.InvTranInvoiceRequests.
                     Where(x => x.Active == true && RuleIds.Contains(x.CuPriceCardId.GetValueOrDefault()) && DepartmentIds.Contains(x.DepartmentId.GetValueOrDefault()))
                    .Select(x => new PriceInvoiceRequest
                    {
                        BilledName = x.BilledName,
                        BilledAddress = x.BilledAddress,
                        Id = x.Id,
                        CuPriceCardId = x.CuPriceCardId
                        // InvoiceRequestContactList = x.InvTranInvoiceRequestContacts.Where(z => z.Active.Value).Select(y => y.ContactId.GetValueOrDefault()).ToList()
                    }).ToListAsync();
            }
            else
            {
                return await _context.InvTranInvoiceRequests.
                     Where(x => x.Active == true && RuleIds.Contains(x.CuPriceCardId.GetValueOrDefault()))
                    .Select(x => new PriceInvoiceRequest
                    {
                        BilledName = x.BilledName,
                        BilledAddress = x.BilledAddress,
                        Id = x.Id,
                        CuPriceCardId = x.CuPriceCardId
                        // InvoiceRequestContactList = x.InvTranInvoiceRequestContacts.Where(z => z.Active.Value).Select(y => y.ContactId.GetValueOrDefault()).ToList()
                    }).ToListAsync();
            }
        }

        public async Task<List<PriceInvoiceRequest>> GetInvoiceRequestAddressByBuyer(IEnumerable<int?> RuleIds, IEnumerable<int> BuyerIds)
        {

            if (BuyerIds.Any())
            {
                return await _context.InvTranInvoiceRequests.
                     Where(x => x.Active == true && RuleIds.Contains(x.CuPriceCardId.GetValueOrDefault()) && BuyerIds.Contains(x.BuyerId.GetValueOrDefault()))
                    .Select(x => new PriceInvoiceRequest
                    {
                        BilledName = x.BilledName,
                        BilledAddress = x.BilledAddress,
                        Id = x.Id,
                        CuPriceCardId = x.CuPriceCardId
                        //  InvoiceRequestContactList = x.InvTranInvoiceRequestContacts.Where(z => z.Active.Value).Select(y => y.ContactId.GetValueOrDefault()).ToList()
                    }).ToListAsync();
            }
            else
            {
                return await _context.InvTranInvoiceRequests.
                                     Where(x => x.Active == true && RuleIds.Contains(x.CuPriceCardId.GetValueOrDefault()))
                                    .Select(x => new PriceInvoiceRequest
                                    {
                                        BilledName = x.BilledName,
                                        BilledAddress = x.BilledAddress,
                                        Id = x.Id,
                                        CuPriceCardId = x.CuPriceCardId
                                        //  InvoiceRequestContactList = x.InvTranInvoiceRequestContacts.Where(z => z.Active.Value).Select(y => y.ContactId.GetValueOrDefault()).ToList()
                                    }).ToListAsync();
            }

        }

        public async Task<List<InvoiceRequestContactId>> GetInvoiceRequestContacts(IEnumerable<int> InvoiceRequestIds)
        {
            return await _context.InvTranInvoiceRequestContacts.
                                    Where(x => x.Active == true && InvoiceRequestIds.Contains(x.InvoiceRequestId.GetValueOrDefault()))
                                   .Select(x => new InvoiceRequestContactId
                                   {
                                       ContactId = x.ContactId.GetValueOrDefault(),
                                       InvoiceRequestId = x.InvoiceRequestId
                                   }).ToListAsync();
        }

        public async Task<string> GetInvoiceNumber(string prefix)
        {
            return await (from invoice in _context.InvAutTranDetails
                          where invoice.InvoiceNo.StartsWith(prefix)
                          orderby invoice.InvoiceNo descending
                          select invoice.InvoiceNo).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Check invoice number is exist or not
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        public async Task<bool> CheckInvoiceNumberExist(string invoiceNumber)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceNo == invoiceNumber).AnyAsync();
        }

        public async Task<List<CustomerPriceBookingProducts>> GetBookingProductDetails(int bookingId)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value && x.InspectionId == bookingId).
                            Select(x => new CustomerPriceBookingProducts
                            {
                                ProductId = x.ProductId,
                                ProductName = x.Product.ProductId,
                                AQLQuantity = x.AqlQuantity,
                                CombinedAQLQuantity = x.CombineAqlQuantity,
                                UnitCount = x.UnitCount,
                                CombineProductId = x.CombineProductId,
                                SubCategoryId = x.Product.ProductSubCategory,
                                SubCategory2Id = x.Product.ProductCategorySub2,
                                PresentedQuantity = x.FbReport.PresentedQty,
                                InspectedQuanity = x.FbReport.InspectedQty
                            }).ToListAsync();
        }

        /// <summary>
        /// Get ProductDetails By BookingList
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CustomerPriceBookingProducts>> GetProductDetailsByBookingList(List<int> bookingIdList)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value && bookingIdList.Contains(x.InspectionId)).
                            Select(x => new CustomerPriceBookingProducts
                            {
                                ProductId = x.ProductId,
                                ProductName = x.Product.ProductId,
                                BookingId = x.InspectionId,
                                BookingQuantity = x.TotalBookingQuantity,
                                AQLQuantity = x.AqlQuantity,
                                CombinedAQLQuantity = x.CombineAqlQuantity,
                                UnitCount = x.UnitCount,
                                CombineProductId = x.CombineProductId,
                                IsDispalyMaster = x.IsDisplayMaster,
                                ParentProductId = x.ParentProductId,
                                CusbookingNumber = x.Inspection.CustomerBookingNo,
                                FormSerialNumber = x.BookingFormSerial,
                                UnitId = x.Unit
                            }).ToListAsync();
        }



        /// <summary>
        /// Get the invoice base details which are common
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        public async Task<InvoiceBaseDetailRepo> GetInvoiceBaseDetails(string invoiceNo)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceNo == invoiceNo && x.ServiceId == (int)Service.InspectionId &&
                x.InvoiceStatus != (int)InvoiceStatus.Cancelled)
                .Select(x => new InvoiceBaseDetailRepo()
                {
                    InvoiceId = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.InvoiceDate,
                    PostDate = x.PostedDate,
                    Subject = x.Subject,
                    BilledTo = x.InvoiceTo,
                    InspectionId = x.InspectionId,
                    InvoiceCurrency = x.InvoiceCurrency,
                    ExchangeRate = x.ExchangeRate,
                    BankId = x.BankId,
                    BilledName = x.InvoicedName,
                    BilledAddress = x.InvoicedAddress,
                    PaymentTerms = x.PaymentTerms,
                    PaymentDuration = x.PaymentDuration,
                    InvoiceStatus = x.InvoiceStatusNavigation.Name,
                    Office = x.Office,
                    BankDetails = new InvoiceBankDetail()
                    {
                        BankId = x.BankId,
                        AccountName = x.Bank.AccountName,
                        AccountNumber = x.Bank.AccountNumber,
                        AccountCurrency = x.Bank.AccountCurrencyNavigation.CurrencyCodeA,
                        BankName = x.Bank.BankName,
                        BankAddress = x.Bank.BankAddress
                    },
                    BillingMethod = x.InvoiceMethod,
                    Currency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    PaymentStatus = x.InvoicePaymentStatus,
                    PaymentDate = x.InvoicePaymentDate,
                    TaxValue = x.TaxValue,
                    CustomerId = x.Inspection.CustomerId,
                    SupplierId = x.Inspection.SupplierId,
                    FactoryId = x.Inspection.FactoryId,
                    TotalInvoiceFees = x.TotalInvoiceFees,
                    TotalTaxAmount = x.TotalTaxAmount,
                    TotalTravelFees = x.TravelTotalFees,
                    IsTravelExpense = x.IsTravelExpense,
                    IsInspectionFees = x.IsInspection,
                    InvoicingRequest = x.Rule.InvoiceRequestType.GetValueOrDefault(),
                    InvoiceType = x.InvoiceType,
                    BilledQuantityType = x.Rule.BilledQuantityTypeNavigation.Name,
                    InvoiceCurrencyName = x.InvoiceCurrencyNavigation.CurrencyName
                }).AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task<List<InvAutTranContactDetail>> GetInvoiceContactDetails(int invoiceId)
        {
            return await _context.InvAutTranContactDetails.
                     Where(x => x.InvoiceId.GetValueOrDefault() == invoiceId)
                    .AsNoTracking().ToListAsync();
        }

        public async Task<InvoiceBaseDetailRepo> GetAuditInvoiceBaseDetails(string invoiceNo)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceNo == invoiceNo &&
            x.ServiceId == (int)Service.AuditId
            && x.InvoiceStatus != (int)InvoiceStatus.Cancelled)
                .Select(x => new InvoiceBaseDetailRepo()
                {
                    InvoiceId = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.InvoiceDate,
                    PostDate = x.PostedDate,
                    Subject = x.Subject,
                    BilledTo = x.InvoiceTo,
                    AuditId = x.AuditId,
                    InvoiceCurrency = x.InvoiceCurrency,
                    BankId = x.BankId,
                    BilledName = x.InvoicedName,
                    BilledAddress = x.InvoicedAddress,
                    PaymentTerms = x.PaymentTerms,
                    PaymentDuration = x.PaymentDuration,
                    InvoiceStatus = x.InvoiceStatusNavigation.Name,
                    Office = x.Office,
                    BankDetails = new InvoiceBankDetail()
                    {
                        AccountName = x.Bank.AccountName,
                        AccountNumber = x.Bank.AccountNumber,
                        AccountCurrency = x.Bank.AccountCurrencyNavigation.CurrencyCodeA,
                        BankName = x.Bank.BankName,
                        BankAddress = x.Bank.BankAddress
                    },
                    BillingMethod = x.InvoiceMethod,
                    Currency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    PaymentStatus = x.InvoicePaymentStatus,
                    PaymentDate = x.InvoicePaymentDate,
                    TaxValue = x.TaxValue,
                    CustomerId = x.Audit.CustomerId,
                    SupplierId = x.Audit.SupplierId,
                    FactoryId = x.Audit.FactoryId,
                    TotalInvoiceFees = x.TotalInvoiceFees,
                    TotalTaxAmount = x.TotalTaxAmount,
                    TotalTravelFees = x.TravelTotalFees,
                    IsTravelExpense = x.IsTravelExpense,
                    IsInspectionFees = x.IsInspection,
                    InvoicingRequest = x.Rule.InvoiceRequestType.GetValueOrDefault(),
                    InvoiceType = x.InvoiceType
                }).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the invoice payment status
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetInvoicePaymentStatus()
        {
            return await _context.InvPaymentStatuses.Where(x => x.Active)

                .Select(x => new CommonDataSource() { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the invoice transaction details
        /// </summary>
        /// <returns></returns>
        public async Task<List<InvoiceTransactionDetailRepo>> GetInvoiceTransactionDetails(string invoiceNo)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceNo == invoiceNo &&
             x.ServiceId == (int)Service.InspectionId &&
            x.InvoiceStatus != (int)InvoiceStatus.Cancelled)

                .Select(x => new InvoiceTransactionDetailRepo()
                {
                    Id = x.Id,
                    BookingNo = x.InspectionId,
                    CustomerBookingNo = x.Inspection.CustomerBookingNo,
                    ServiceDateFrom = x.Inspection.ServiceDateFrom,
                    ServiceDateTo = x.Inspection.ServiceDateTo,
                    PriceCategory = x.Inspection.PriceCategory.Name,
                    Customer = x.Inspection.Customer.CustomerName,
                    Supplier = x.Inspection.Supplier.SupplierName,
                    Factory = x.Inspection.Factory.SupplierName,
                    FactoryId = x.Inspection.FactoryId,
                    ManDay = x.ManDays,
                    UnitPrice = x.UnitPrice,
                    InspectionFees = x.InspectionFees,
                    AirCost = x.TravelAirFees,
                    LandCost = x.TravelLandFees,
                    HotelCost = x.HotelFees,
                    TravelOtherFees = x.TravelOtherFees,
                    TravelTotalFees = x.TravelTotalFees,
                    IsTravelExpense = x.IsTravelExpense,
                    IsInspectionFees = x.IsInspection,
                    OtherCost = x.OtherFees,
                    Discount = x.Discount,
                    Remarks = x.Remarks,
                    BilledTo = x.InvoiceTo,
                    BankId = x.BankId,
                    InvoiceCurrency = x.InvoiceCurrency
                }).OrderBy(x => x.ServiceDateTo).ThenBy(x => x.Factory).AsNoTracking().ToListAsync();
        }


        public async Task<List<InvoiceTransactionDetailRepo>> GetAuditInvoiceTransactionDetails(string invoiceNo)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceNo == invoiceNo &&
            x.ServiceId == (int)Service.AuditId
            && x.InvoiceStatus != (int)InvoiceStatus.Cancelled)

                .Select(x => new InvoiceTransactionDetailRepo()
                {
                    Id = x.Id,
                    AuditNo = x.AuditId,
                    ServiceDateFrom = x.Audit.ServiceDateFrom,
                    ServiceDateTo = x.Audit.ServiceDateTo,
                    Customer = x.Audit.Customer.CustomerName,
                    Supplier = x.Audit.Supplier.SupplierName,
                    Factory = x.Audit.Factory.SupplierName,
                    FactoryId = x.Audit.FactoryId,
                    ManDay = x.ManDays,
                    UnitPrice = x.UnitPrice,
                    InspectionFees = x.InspectionFees,
                    AirCost = x.TravelAirFees,
                    LandCost = x.TravelLandFees,
                    HotelCost = x.HotelFees,
                    TravelOtherFees = x.TravelOtherFees,
                    TravelTotalFees = x.TravelTotalFees,
                    IsTravelExpense = x.IsTravelExpense,
                    IsInspectionFees = x.IsInspection,
                    OtherCost = x.OtherFees,
                    Discount = x.Discount,
                    Remarks = x.Remarks,
                    BilledTo = x.InvoiceTo,
                    BankId = x.BankId,
                    InvoiceCurrency = x.InvoiceCurrency
                }).OrderBy(x => x.ServiceDateTo).ThenBy(x => x.Factory).AsNoTracking().ToListAsync();
        }

        public async Task<List<InvoiceBookingQuantityDetails>> GetBookingQuantityDetails(IEnumerable<int> bookingIds)
        {

            return await _context.InspTransactions.Where(y => bookingIds.Contains(y.Id)).
                Select(z => new InvoiceBookingQuantityDetails()
                {
                    BookingNo = z.Id,
                    BookingQty = z.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value).Select(x => x.TotalBookingQuantity).Sum(),
                    InspectedQuantity = z.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value).SelectMany(x => x.FbReportQuantityDetails.Where(y => y.Active.Value)).Select(r => r.InspectedQuantity).Sum(),
                    PresentedQuantity = z.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value).SelectMany(x => x.FbReportQuantityDetails.Where(y => y.Active.Value)).Select(r => r.PresentedQuantity).Sum()
                }).AsNoTracking().ToListAsync();

        }

        public async Task<List<InvoiceBookingQuotation>> GetBookingQuotationDetails(IEnumerable<int> bookingIds)
        {

            return await _context.QuQuotationInsps.Where(y => bookingIds.Contains(y.IdBooking) && y.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                Select(z => new InvoiceBookingQuotation()
                {
                    BookingNo = z.IdBooking,
                    QuotationNo = z.IdQuotation
                }).AsNoTracking().ToListAsync();

        }
        /// <summary>
        /// get quotation id bookings
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InvoiceBookingQuotation>> GetAuditBookingQuotationDetails(IEnumerable<int> bookingIds)
        {
            return await _context.QuQuotationAudits.Where(y => bookingIds.Contains(y.IdBooking) && y.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                Select(z => new InvoiceBookingQuotation()
                {
                    BookingNo = z.IdBooking,
                    QuotationNo = z.IdQuotation
                }).AsNoTracking().ToListAsync();

        }

        public async Task<List<InvoiceBookingServiceTypes>> GetBookingServiceTypes(IEnumerable<int> bookingIds)
        {

            return await _context.InspTranServiceTypes.Where(y => y.Active && bookingIds.Contains(y.Inspection.Id)).
                Select(z => new InvoiceBookingServiceTypes()
                {
                    BookingNo = z.InspectionId,
                    ServiceType = z.ServiceType.Name,
                    ServiceTypeId = z.ServiceTypeId
                }).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// get audit booking service types
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InvoiceBookingServiceTypes>> GetAuditBookingServiceTypes(IEnumerable<int> bookingIds)
        {
            return await _context.AudTranServiceTypes.Where(y => y.Active && bookingIds.Contains(y.Audit.Id)).
                Select(z => new InvoiceBookingServiceTypes()
                {
                    BookingNo = z.AuditId,
                    ServiceType = z.ServiceType.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the additional booking info by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<InvoiceBookingMoreInfo> GetInvoiceBookingMoreInfo(int bookingId)
        {
            return await _context.InspTransactions.Where(x => x.Id == bookingId).
                    Select(x => new InvoiceBookingMoreInfo
                    {
                        BookingNo = x.Id,
                        //Brands = string.Join(",", x.InspTranCuBrands.Select(y => y.Brand.Name)),
                        //Departments = string.Join(",", x.InspTranCuDepartments.Select(y => y.Department.Name)),
                        //QCNames = string.Join(",", x.SchScheduleQcs.Select(y => y.Qc.PersonName)),
                        PriceCategory = x.PriceCategory.Name,
                        Collection = x.Collection.Name,
                        PresentedQuantity = x.InspPurchaseOrderTransactions.Where(z => z.Active.HasValue && z.Active.Value).SelectMany(z => z.FbReportQuantityDetails.Where(y => y.Active.Value)).Select(r => r.PresentedQuantity).Sum()
                    }).AsNoTracking().FirstOrDefaultAsync();
        }
        /// <summary>
        /// Get the invoice transations by invoice id list
        /// </summary>
        /// <param name="invoiceIds"></param>
        /// <returns></returns>
        public async Task<List<InvAutTranDetail>> GetInvoiceListById(IEnumerable<int> invoiceIds)
        {
            return await _context.InvAutTranDetails.Include(x => x.InvAutTranContactDetails)
                 .Include(x => x.InvExfTransactions).
                 ThenInclude(x => x.InvExfTranStatusLogs)
                .Where(x => invoiceIds.Contains(x.Id)
                                                    && x.InvoiceStatus != (int)InvoiceStatus.Cancelled).ToListAsync();
        }

        public async Task<List<InvAutTranDetail>> GetInvoiceListByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Include(x => x.InvAutTranContactDetails)
                          .Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault())
                           && x.InvoiceStatus != (int)InvoiceStatus.Cancelled).ToListAsync();
        }



        /// <summary>
        /// get invoice data by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InvAutTranDetail>> GetInvoiceListByBookingId(IEnumerable<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Include(x => x.InvAutTranContactDetails).
                                                    Include(x => x.InvExfTransactions).
                                                    Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault())
                                                    && x.InvoiceStatus != (int)InvoiceStatus.Cancelled).ToListAsync();
        }

        /// <summary>
        /// get invoice data by booking ids
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<List<InvAutTranDetail>> GetInvoiceListByAuditId(IEnumerable<int> auditIds)
        {
            return await _context.InvAutTranDetails.Include(x => x.InvAutTranContactDetails).
                                                    Include(x => x.InvExfTransactions).
                                                    Where(x => auditIds.Contains(x.AuditId.GetValueOrDefault())
                                                    && x.InvoiceStatus != (int)InvoiceStatus.Cancelled).ToListAsync();
        }




        /// <summary>
        /// Get the invoice transaction by invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public async Task<InvAutTranDetail> GetInvoiceById(int invoiceId)
        {
            return await _context.InvAutTranDetails.Where(x => x.Id == invoiceId
                                            && x.InvoiceStatus != (int)InvoiceStatus.Cancelled).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the invoice reference office
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetInvoiceOffice()
        {
            return await _context.InvRefOffices.Where(x => x.Active.HasValue && x.Active.Value)

                .Select(x => new CommonDataSource() { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get Product List by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InvoiceBookingProductsData>> GetProductListByBooking(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingIds.Contains(y.InspectionId)).Select(z => new InvoiceBookingProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductId = z.Product.ProductId,
                ProductName = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.Product.ProductCategorySub2Navigation.Name,
                ProductBookingQuantity = z.TotalBookingQuantity,
                ProductBarCode = z.Product.Barcode,
                ReportNumber = z.FbReport.ReportTitle,
                FactoryReference = z.Product.FactoryReference,
                AQLQty = z.AqlQuantity,
                CombineAQLQty = z.CombineAqlQuantity,
                CombineProductId = z.CombineProductId,
                FactoryName = z.Inspection.Factory.SupplierName,
                ServiceDateTo = z.Inspection.ServiceDateTo
            }).OrderBy(x => x.BookingId).ThenBy(x => x.ProductName).AsNoTracking().ToListAsync();
        }

        /// <returns></returns>
        public async Task<List<InvoiceBookingProducts>> GetInvoiceBookingProducts(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingIds.Contains(y.InspectionId)).Select(z => new InvoiceBookingProducts()
            {
                ProductId = z.ProductId,
                ProductRefId = z.Id,
                ProductName = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.Product.ProductCategorySub2Navigation.Name,
                BookingQuantity = z.TotalBookingQuantity,
                InspectionQuantity = z.FbReport.FbReportQuantityDetails.Select(x => x.InspectedQuantity).Sum(),
                PresentedQuantity = z.FbReport.FbReportQuantityDetails.Select(x => x.PresentedQuantity).Sum(),
            }).AsNoTracking().OrderBy(x => x.ProductId).ToListAsync();
        }

        /// <summary>
        /// Fetch all the invoice details from InvAutTranDetails
        /// </summary>
        /// <returns>Iqueryable InvoiceSummaryItem</returns>
        public IQueryable<InvAutTranDetail> GetInvoiceDetailsByServiceType(int serviceTypeId)
        {
            return _context.InvAutTranDetails.Where(x => x.InvoiceStatus != (int)InvoiceStatus.Cancelled && x.ServiceId == serviceTypeId);
        }

        /// <summary>
        /// get Audit invoice details
        /// </summary>
        /// <returns></returns>
        public IQueryable<InvoiceSummaryItem> GetAuditInvoiceDetails()
        {
            return _context.InvAutTranDetails.Where(x => x.InvoiceStatus != (int)InvoiceStatus.Cancelled && x.ServiceId == (int)Service.AuditId)
                .Select(x => new InvoiceSummaryItem
                {
                    Id = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.InvoiceDate,
                    InvoiceToId = x.InvoiceTo,
                    BookingId = x.AuditId,
                    AuditId = x.AuditId,
                    ServiceId = x.ServiceId,
                    InvoiceTypeId = x.InvoiceType,
                    InvoiceTypeName = x.InvoiceTypeNavigation.Name,
                    CustomerId = x.Audit.CustomerId,
                    SupplierId = x.Audit.SupplierId,
                    FactoryId = x.Audit.FactoryId,
                    StatusId = x.InvoiceStatus.GetValueOrDefault(),
                    StatusName = x.InvoiceStatusNavigation.Name,
                    BookingServiceDateFrom = x.Audit.ServiceDateFrom,
                    BookingServiceDateTo = x.Audit.ServiceDateTo,
                }).OrderByDescending(x => x.InvoiceNo);
        }

        /// <summary>
        /// Fetch all the invoice details from InvAutTranDetails
        /// </summary>
        /// <returns>IEnumerable InvoiceSummaryItem</returns>
        public async Task<IEnumerable<InvoiceSummaryItem>> GetInvoiceDetailsByInvoiceNo(List<string> invoiceNoList)
        {
            return await _context.InvAutTranDetails.Where(x => invoiceNoList.Contains(x.InvoiceNo) && x.ServiceId == (int)Service.InspectionId &&
            x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled)
                .Select(x => new InvoiceSummaryItem
                {
                    Id = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.InvoiceDate,
                    InvoiceTo = x.InvoiceToNavigation.Label,
                    InvoiceToId = x.InvoiceTo,
                    BookingId = x.InspectionId,
                    ServiceId = x.ServiceId,
                    ServiceName = x.Service.Name,
                    InvoiceCurrency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    InspFees = x.InspectionFees,
                    TravelFee = x.TravelTotalFees,
                    OtherExpense = x.OtherFees,
                    TotalFee = x.TotalInvoiceFees,
                    IsInspection = x.IsInspection,
                    IsTravelExpense = x.IsTravelExpense,
                    CustomerName = x.Inspection.Customer.CustomerName,
                    SupplierName = x.Inspection.Supplier.SupplierName,
                    FactoryName = x.Inspection.Factory.SupplierName,
                    CreatedBy = x.CreatedByNavigation.FullName,
                    TaxValue = x.TaxValue,
                    HotelFee = x.HotelFees,
                    Discount = x.Discount,
                    TravelAirFee = x.TravelAirFees,
                    TravelLandFee = x.TravelLandFees,
                    TravelOtherFee = x.TravelOtherFees,
                    StatusId = x.InvoiceStatus.GetValueOrDefault(),
                    StatusName = x.InvoiceStatusNavigation.Name,
                    ProrateBookingNo = x.ProrateBookingNumbers,
                    InvoiceRemarks = x.Remarks,
                    CustomerId = x.Inspection.CustomerId,
                    InvoiceTypeId = x.InvoiceType,
                    InvoiceTypeName = x.InvoiceTypeNavigation.Name,
                    Invoiceoffice = x.OfficeNavigation.Name,
                    BookingServiceDateFrom = x.Inspection.ServiceDateFrom,
                    BookingServiceDateTo = x.Inspection.ServiceDateTo,
                    FactoryId = x.Inspection.FactoryId
                }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Fetch all the invoice details from InvAutTranDetails
        /// </summary>
        /// <returns>IEnumerable InvoiceSummaryItem</returns>
        public IQueryable<InvoiceSummaryItem> GetQueryableInvoiceDetailsByInvoiceNo(IQueryable<string> invoiceNoList)
        {
            return _context.InvAutTranDetails.Where(x => invoiceNoList.Contains(x.InvoiceNo) && x.ServiceId == (int)Service.InspectionId &&
          x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled)
                .Select(x => new InvoiceSummaryItem
                {
                    Id = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    InvoicedName = x.InvoicedName,
                    InvoiceDate = x.InvoiceDate,
                    InvoiceTo = x.InvoiceToNavigation.Label,
                    InvoiceToId = x.InvoiceTo,
                    BookingId = x.InspectionId,
                    ServiceId = x.ServiceId,
                    BankId = x.BankId.GetValueOrDefault(),
                    ServiceName = x.Service.Name,
                    InvoiceCurrency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    InspFees = x.InspectionFees,
                    TravelFee = x.TravelTotalFees,
                    OtherExpense = x.OtherFees,
                    TotalFee = x.TotalInvoiceFees,
                    IsInspection = x.IsInspection,
                    IsTravelExpense = x.IsTravelExpense,
                    CustomerName = x.Inspection.Customer.CustomerName,
                    SupplierName = x.Inspection.Supplier.SupplierName,
                    FactoryName = x.Inspection.Factory.SupplierName,
                    CreatedBy = x.CreatedByNavigation.FullName,
                    TaxValue = x.TaxValue,
                    HotelFee = x.HotelFees,
                    Discount = x.Discount,
                    TravelAirFee = x.TravelAirFees,
                    TravelLandFee = x.TravelLandFees,
                    TravelOtherFee = x.TravelOtherFees,
                    StatusId = x.InvoiceStatus.GetValueOrDefault(),
                    StatusName = x.InvoiceStatusNavigation.Name,
                    ProrateBookingNo = x.ProrateBookingNumbers,
                    InvoiceRemarks = x.Remarks,
                    CustomerId = x.Inspection.CustomerId,
                    InvoiceTypeId = x.InvoiceType,
                    InvoiceTypeName = x.InvoiceTypeNavigation.Name,
                    Invoiceoffice = x.OfficeNavigation.Name,
                    ActualManday = x.Inspection.SchScheduleQcs.Where(y => y.Active).Sum(z => z.ActualManDay),
                    BookingServiceDateFrom = x.Inspection.ServiceDateFrom,
                    BookingServiceDateTo = x.Inspection.ServiceDateTo,
                    BillingMethodName = x.InvoiceMethodNavigation.Label,
                    InvoiceOfficeName = x.OfficeNavigation.Name,
                    PaymentStatusId = x.InvoicePaymentStatus.GetValueOrDefault(),
                    PaymentStatusName = x.InvoicePaymentStatusNavigation.Name,
                    PaymentDate = x.InvoicePaymentDate,
                    FactoryId = x.Inspection.FactoryId.GetValueOrDefault()
                });
        }
        public IQueryable<int> GetQueryableInvoiceBookingNo(IQueryable<string> invoiceNoList)
        {
            return _context.InvAutTranDetails.Where(x => invoiceNoList.Contains(x.InvoiceNo) && x.ServiceId == (int)Service.InspectionId &&
          x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled)
                .Select(x => x.InspectionId.GetValueOrDefault()).Distinct();
        }
        public IQueryable<int> GetQueryableInvoiceAuditNo(IQueryable<string> invoiceNoList)
        {
            return _context.InvAutTranDetails.Where(x => invoiceNoList.Contains(x.InvoiceNo) && x.ServiceId == (int)Service.AuditId &&
          x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled)
                .Select(x => x.AuditId.GetValueOrDefault()).Distinct();
        }

        /// <summary>
        /// Get Audit invoice details
        /// </summary>
        /// <param name="invoiceNoList"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InvoiceSummaryItem>> GetAuditInvoiceDetailsByInvoiceNo(List<string> invoiceNoList)
        {
            return await _context.InvAutTranDetails.Where(x => invoiceNoList.Contains(x.InvoiceNo) && x.ServiceId == (int)Service.AuditId &&
            x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled)
                .Select(x => new InvoiceSummaryItem
                {
                    Id = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.InvoiceDate,
                    InvoiceTo = x.InvoiceToNavigation.Label,
                    InvoiceToId = x.InvoiceTo,
                    BookingId = x.InspectionId,
                    AuditId = x.AuditId,
                    InvoiceCurrency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    InspFees = x.InspectionFees,
                    TravelFee = x.TravelTotalFees,
                    OtherExpense = x.OtherFees,
                    TotalFee = x.TotalInvoiceFees,
                    IsInspection = x.IsInspection,
                    ServiceId = x.ServiceId,
                    ServiceName = x.Service.Name,
                    IsTravelExpense = x.IsTravelExpense,
                    CustomerName = x.Audit.Customer.CustomerName,
                    SupplierName = x.Audit.Supplier.SupplierName,
                    FactoryName = x.Audit.Factory.SupplierName,
                    CreatedBy = x.CreatedByNavigation.FullName,
                    TaxValue = x.TaxValue,
                    HotelFee = x.HotelFees,
                    Discount = x.Discount,
                    TravelAirFee = x.TravelAirFees,
                    TravelLandFee = x.TravelLandFees,
                    TravelOtherFee = x.TravelOtherFees,
                    StatusId = x.InvoiceStatus.GetValueOrDefault(),
                    StatusName = x.InvoiceStatusNavigation.Name,
                    ProrateBookingNo = x.ProrateBookingNumbers,
                    InvoiceRemarks = x.Remarks,
                    CustomerId = x.Audit.CustomerId,
                    InvoiceTypeId = x.InvoiceType,
                    InvoiceTypeName = x.InvoiceTypeNavigation.Name,
                    Invoiceoffice = x.OfficeNavigation.Name,
                    BookingServiceDateFrom = x.Audit.ServiceDateFrom,
                    BookingServiceDateTo = x.Audit.ServiceDateTo,
                    FactoryId = x.Audit.FactoryId
                }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get Audit invoice details
        /// </summary>
        /// <param name="invoiceNoList"></param>
        /// <returns></returns>
        public IQueryable<InvoiceSummaryItem> GetQueryableAuditInvoiceDetailsByInvoiceNo(IQueryable<string> invoiceNoList)
        {
            return _context.InvAutTranDetails.Where(x => invoiceNoList.Contains(x.InvoiceNo) && x.ServiceId == (int)Service.AuditId &&
         x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled)
                .Select(x => new InvoiceSummaryItem
                {
                    Id = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    InvoicedName = x.InvoicedName,
                    InvoiceDate = x.InvoiceDate,
                    InvoiceTo = x.InvoiceToNavigation.Label,
                    InvoiceToId = x.InvoiceTo,
                    BookingId = x.AuditId,
                    AuditId = x.AuditId,
                    InvoiceCurrency = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    InspFees = x.InspectionFees,
                    TravelFee = x.TravelTotalFees,
                    OtherExpense = x.OtherFees,
                    TotalFee = x.TotalInvoiceFees,
                    IsInspection = x.IsInspection,
                    ServiceId = x.ServiceId,
                    ServiceName = x.Service.Name,
                    IsTravelExpense = x.IsTravelExpense,
                    CustomerName = x.Audit.Customer.CustomerName,
                    SupplierName = x.Audit.Supplier.SupplierName,
                    FactoryName = x.Audit.Factory.SupplierName,
                    CreatedBy = x.CreatedByNavigation.FullName,
                    TaxValue = x.TaxValue,
                    HotelFee = x.HotelFees,
                    Discount = x.Discount,
                    TravelAirFee = x.TravelAirFees,
                    TravelLandFee = x.TravelLandFees,
                    TravelOtherFee = x.TravelOtherFees,
                    StatusId = x.InvoiceStatus.GetValueOrDefault(),
                    StatusName = x.InvoiceStatusNavigation.Name,
                    ProrateBookingNo = x.ProrateBookingNumbers,
                    InvoiceRemarks = x.Remarks,
                    CustomerId = x.Audit.CustomerId,
                    InvoiceTypeId = x.InvoiceType,
                    InvoiceTypeName = x.InvoiceTypeNavigation.Name,
                    Invoiceoffice = x.OfficeNavigation.Name,
                    ActualManday = x.Audit.AudTranAuditors.Count(z => z.StaffId > 0 && z.Active),
                    BookingServiceDateFrom = x.Audit.ServiceDateFrom,
                    BookingServiceDateTo = x.Audit.ServiceDateTo,
                    InvoiceOfficeName = x.OfficeNavigation.Name,
                    BillingMethodName = x.InvoiceMethodNavigation.Label,
                    PaymentStatusId = x.InvoicePaymentStatus.GetValueOrDefault(),
                    PaymentStatusName = x.InvoicePaymentStatusNavigation.Name,
                    PaymentDate = x.InvoicePaymentDate,
                    FactoryId = x.Audit.FactoryId,
                    BankId = x.BankId.GetValueOrDefault()
                });
        }

        /// <summary>
        /// get extra fee item 
        /// </summary>
        /// <param name="invoiceNoList"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InvoiceExtraFeeItem>> GetInvoiceExtraFeeItem(List<int> invoiceNoList)
        {
            return await _context.InvExfTransactions.Where(x => invoiceNoList.Contains(x.InvoiceId.GetValueOrDefault()) &&
                         x.Active.Value && x.StatusId.HasValue && x.StatusId.Value != (int)ExtraFeeStatus.Cancelled)
                .Select(x => new InvoiceExtraFeeItem
                {
                    Id = x.Id,
                    InvoiceNo = x.Invoice.InvoiceNo,
                    InvoiceId = x.InvoiceId,
                    InvoiceTo = x.BilledTo,
                    BookingId = x.InspectionId,
                    AuditId = x.AuditId,
                    TotalExtraFees = x.TotalExtraFee,
                    TotalExtraSubFees = x.ExtraFeeSubTotal,
                    TotalExtrFeeTax = x.TaxAmount,
                    BilledTo = x.BilledTo
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get extra fee item 
        /// </summary>
        /// <param name="invoiceNoList"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InvoiceExtraFeeItem>> GetQueryableInvoiceExtraFeeItem(IQueryable<int> invoiceNoList)
        {
            return await _context.InvExfTransactions.Where(x => invoiceNoList.Contains(x.InvoiceId.GetValueOrDefault()) &&
                         x.Active.Value && x.StatusId.HasValue && x.StatusId.Value != (int)ExtraFeeStatus.Cancelled)
                .Select(x => new InvoiceExtraFeeItem
                {
                    Id = x.Id,
                    InvoiceNo = x.Invoice.InvoiceNo,
                    InvoiceId = x.InvoiceId,
                    InvoiceTo = x.BilledTo,
                    BookingId = x.InspectionId,
                    AuditId = x.AuditId,
                    TotalExtraFees = x.TotalExtraFee,
                    TotalExtraSubFees = x.ExtraFeeSubTotal,
                    TotalExtrFeeTax = x.TaxAmount,
                    BilledTo = x.BilledTo
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Fetch all the invoice details from InvAutTranDetails
        /// </summary>
        /// <returns>Iqueryable InvoiceSummaryItem</returns>
        public async Task<List<InvAutTranDetail>> GetInvoice(string invoiceNo)
        {
            return await _context.InvAutTranDetails.
                Include(x => x.InvExfTransactions).
                 ThenInclude(x => x.InvExfTranStatusLogs).
                Where(x => x.InvoiceNo == invoiceNo).ToListAsync();
        }

        /// <summary>
        /// Fetch the invoice status List
        /// </summary>
        /// <returns>List DataSource</returns>
        public async Task<List<CommonDataSource>> GetInvoiceStatus()
        {
            return await _context.InvStatuses.Where(x => x.Active).AsNoTracking().
                Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        /// <summary>
        /// save invoice status log
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> SaveInvoiceStatusLog(List<InvAutTranStatusLog> entity)
        {
            foreach (var item in entity)
            {
                _context.InvAutTranStatusLogs.Add(item);
            }
            if (await _context.SaveChangesAsync() > 0)
                return 1;
            else
                return 0;

        }

        /// <summary>
        /// Get the KPI Template List
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<List<KPITemplate>> GetTemplateList(int typeId)
        {
            return await _context.RefKpiTeamplates.Where(x => x.Active && x.TypeId == typeId)
                .Select(x => new KPITemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    TypeId = x.TypeId
                }).ToListAsync();
        }

        /// <summary>
        /// Get Active invoice booking ids
        /// </summary>
        /// <returns></returns>
        public async Task<List<int>> GetActiveInvoiceInspectionIdList(List<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x =>
                         bookingIds.Contains(x.InspectionId.GetValueOrDefault()) &&
                         x.InvoiceStatus != (int)InvoiceStatus.Cancelled).
                         Select(x => x.InspectionId.GetValueOrDefault()).Distinct().ToListAsync();
        }

        /// <summary>
        /// get active invoice audit  ids
        /// </summary>
        /// <returns></returns>
        public async Task<List<int>> GetActiveInvoiceAuditIdList(List<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x =>
                         bookingIds.Contains(x.AuditId.GetValueOrDefault()) &&
                         x.InvoiceStatus != (int)InvoiceStatus.Cancelled).
                         Select(x => x.AuditId.GetValueOrDefault()).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get Active extra fee reference 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="billingTo"></param>
        /// <param name="currencyId"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InvExfTransaction>> GetBookingActiveExtraFeeReference(List<int?> bookingIds, int billingTo, int currencyId, int bankId)
        {
            var extraFees = _context.InvExfTransactions.Where(x =>
                         x.Active.Value && x.StatusId == (int)ExtraFeeStatus.Pending);

            if (bookingIds.Any())
            {
                extraFees = extraFees.Where(x => bookingIds.Contains(x.InspectionId));
            }
            if (billingTo > 0)
            {
                extraFees = extraFees.Where(x => x.BilledTo == billingTo);
            }
            if (currencyId > 0)
            {
                extraFees = extraFees.Where(x => x.CurrencyId == currencyId);
            }

            return await extraFees.ToListAsync();
        }
        /// <summary>
        /// get audit extra fees reference
        /// </summary>
        /// <param name="auditIds"></param>
        /// <param name="billingTo"></param>
        /// <param name="currencyId"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InvExfTransaction>> GetAuditActiveExtraFeeReference(List<int?> auditIds, int billingTo, int currencyId, int bankId)
        {
            var extraFees = _context.InvExfTransactions.Where(x =>
                         x.Active.Value && x.StatusId == (int)ExtraFeeStatus.Pending);

            if (auditIds.Any())
            {
                extraFees = extraFees.Where(x => auditIds.Contains(x.AuditId));
            }
            if (billingTo > 0)
            {
                extraFees = extraFees.Where(x => x.BilledTo == billingTo);
            }
            if (currencyId > 0)
            {
                extraFees = extraFees.Where(x => x.CurrencyId == currencyId);
            }

            return await extraFees.ToListAsync();
        }
        //Get Queryable Booking Quotation Details
        public async Task<List<InvoiceBookingQuotation>> GetQueryableBookingQuotationDetails(List<int> bookingIds)
        {

            return await _context.QuQuotationInsps.Where(y => bookingIds.Contains(y.IdBooking) && y.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                Select(z => new InvoiceBookingQuotation()
                {
                    BookingNo = z.IdBooking,
                    QuotationNo = z.IdQuotation,
                    QuotationStatus = z.IdQuotationNavigation.IdStatusNavigation.Label,
                    PaymentTermsId = z.IdQuotationNavigation.PaymentTerms,
                    BillTo = z.IdQuotationNavigation.BillingPaidById,
                    QuotationStatusId = z.IdQuotationNavigation.IdStatus,
                    SupplierId = z.IdQuotationNavigation.SupplierId,
                    SupplierName = z.IdQuotationNavigation.SupplierLegalName,
                    //SupplierAddress = z.IdQuotationNavigation.Supplier.SuAddresses.FirstOrDefault().Address,
                    //SupplierContacts = z.IdQuotationNavigation.Supplier.SuContacts.FirstOrDefault().Id,
                    FactoryId = z.IdQuotationNavigation.FactoryId,
                    FactoryName = z.IdQuotationNavigation.LegalFactoryName,
                    FactoryAddress = z.IdQuotationNavigation.FactoryAddress,
                    //FactoryContacts = z.IdQuotationNavigation.Factory.SuContacts.FirstOrDefault().Id,
                    QuotationBilledName = z.IdQuotationNavigation.Rule.InvoiceRequestBilledName,
                    QuotationCurrencyId = z.IdQuotationNavigation.CurrencyId,
                    QuotationCurrencyName = z.IdQuotationNavigation.Currency.CurrencyName,
                    QuotationCurrencyCode = z.IdQuotationNavigation.Currency.CurrencyCodeA,
                    BillingEntityId = z.IdQuotationNavigation.BillingEntity.GetValueOrDefault(),
                    BillingEntityName = z.IdQuotationNavigation.BillingEntityNavigation.Name,
                    BankcurrencyId = z.IdQuotationNavigation.Rule.BankAccountNavigation.AccountCurrency.GetValueOrDefault(),
                    BankcurrencyName = z.IdQuotationNavigation.Rule.BankAccountNavigation.AccountCurrencyNavigation.CurrencyCodeA,
                    BankId = z.IdQuotationNavigation.Rule.BankAccount.GetValueOrDefault(),
                    BankName = z.IdQuotationNavigation.Rule.BankAccountNavigation.BankName,
                    QuotationTotalFees = z.IdQuotationNavigation.TotalCost,
                    CustomerLegalName = z.IdQuotationNavigation.CustomerLegalName,
                    SupplierLegalName = z.IdQuotationNavigation.SupplierLegalName,
                    FactoryLegalName = z.IdQuotationNavigation.LegalFactoryName
                }).AsNoTracking().ToListAsync();

        }
        //Get Queryable Audit Booking Quotation Details
        public async Task<List<InvoiceBookingQuotation>> GetQueryableAuditBookingQuotationDetails(IQueryable<int> bookingIds)
        {
            return await _context.QuQuotationAudits.Where(y => bookingIds.Contains(y.IdBooking) && y.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                Select(z => new InvoiceBookingQuotation()
                {
                    BookingNo = z.IdBooking,
                    QuotationNo = z.IdQuotation,
                    QuotationStatus = z.IdQuotationNavigation.IdStatusNavigation.Label,
                    BankcurrencyId = z.IdQuotationNavigation.Rule.BankAccountNavigation.AccountCurrency.GetValueOrDefault(),
                    BankcurrencyName = z.IdQuotationNavigation.Rule.BankAccountNavigation.AccountCurrencyNavigation.CurrencyCodeA,
                    BankId = z.IdQuotationNavigation.Rule.BankAccount.GetValueOrDefault(),
                    BankName = z.IdQuotationNavigation.Rule.BankAccountNavigation.BankName,
                    QuotationTotalFees = z.IdQuotationNavigation.TotalCost,
                    CustomerLegalName = z.IdQuotationNavigation.CustomerLegalName,
                    SupplierLegalName = z.IdQuotationNavigation.SupplierLegalName,
                    FactoryLegalName = z.IdQuotationNavigation.LegalFactoryName,
                    QuotationStatusId = z.IdQuotationNavigation.IdStatus,
                    PaymentTermsId = z.IdQuotationNavigation.PaymentTerms,
                    QuotationBilledName = z.IdQuotationNavigation.Rule.InvoiceRequestBilledName,
                    QuotationCurrencyId = z.IdQuotationNavigation.CurrencyId,
                    QuotationCurrencyName = z.IdQuotationNavigation.Currency.CurrencyName,
                    QuotationCurrencyCode = z.IdQuotationNavigation.Currency.CurrencyCodeA,
                    BillingEntityId = z.IdQuotationNavigation.BillingEntity.GetValueOrDefault(),
                    BillingEntityName = z.IdQuotationNavigation.BillingEntityNavigation.Name,
                    SupplierId = z.IdQuotationNavigation.SupplierId,
                    SupplierName = z.IdQuotationNavigation.SupplierLegalName,
                    //SupplierAddress = z.IdQuotationNavigation.Supplier.SuAddresses.FirstOrDefault().Address,
                    //SupplierContacts = z.IdQuotationNavigation.Supplier.SuContacts.FirstOrDefault().Id,
                    FactoryId = z.IdQuotationNavigation.FactoryId,
                    FactoryName = z.IdQuotationNavigation.LegalFactoryName,
                    FactoryAddress = z.IdQuotationNavigation.FactoryAddress,
                    //FactoryContacts = z.IdQuotationNavigation.Factory.SuContacts.FirstOrDefault().Id,
                    BillTo = z.IdQuotationNavigation.BillingPaidById,
                }).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// Get the price card supplier
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrSuppliers(List<int> priceCardIdList)
        {
            return await _context.CuPrSuppliers.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.SupplierId,
                    Name = x.Supplier.SupplierName,
                    PriceCardId = x.CuPrId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card product categories
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrProductCategories(List<int> priceCardIdList)
        {
            return await _context.CuPrProductCategories.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.ProductCategoryId,
                    Name = x.ProductCategory.Name,
                    PriceCardId = x.CuPrId
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<CommonPriceCardDataSource>> GetPrProductSubCategories(List<int> priceCardIdList)
        {
            return await _context.CuPrProductSubCategories.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.ProductSubCategoryId,
                    Name = x.ProductSubCategory.Name,
                    PriceCardId = x.CuPrId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card service types
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrServiceTypes(List<int> priceCardIdList)
        {
            return await _context.CuPrServiceTypes.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.ServiceTypeId,
                    Name = x.ServiceType.Name,
                    PriceCardId = x.CuPrId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card countries
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrCountries(List<int> priceCardIdList)
        {
            return await _context.CuPrCountries.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.FactoryCountryId,
                    Name = x.FactoryCountry.CountryName,
                    PriceCardId = x.CuPrId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card provinces
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrProvince(List<int> priceCardIdList)
        {
            return await _context.CuPrProvinces.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.FactoryProvinceId,
                    Name = x.FactoryProvince.ProvinceName,
                    PriceCardId = x.CuPrId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card departments
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrDepartment(List<int> priceCardIdList)
        {
            return await _context.CuPrDepartments.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.DepartmentId.GetValueOrDefault(),
                    Name = x.Department.Name,
                    PriceCardId = x.CuPriceId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card brands
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrBrand(List<int> priceCardIdList)
        {
            return await _context.CuPrBrands.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.BrandId.GetValueOrDefault(),
                    Name = x.Brand.Name,
                    PriceCardId = x.CuPriceId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card buyers
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrBuyer(List<int> priceCardIdList)
        {
            return await _context.CuPrBuyers.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.BuyerId.GetValueOrDefault(),
                    Name = x.Buyer.Name,
                    PriceCardId = x.CuPriceId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card price categories
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrPriceCategory(List<int> priceCardIdList)
        {
            return await _context.CuPrPriceCategories.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.PriceCategoryId.GetValueOrDefault(),
                    Name = x.PriceCategory.Name,
                    PriceCardId = x.CuPriceId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card holiday types
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrHolidayType(List<int> priceCardIdList)
        {
            return await _context.CuPrHolidayTypes.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.HolidayInfoId.GetValueOrDefault(),
                    Name = x.HolidayInfo.Name,
                    PriceCardId = x.CuPriceId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the price card invoice request contacts
        /// </summary>
        /// <param name="priceCardIdList"></param>
        /// <returns></returns>
        public async Task<List<CommonPriceCardDataSource>> GetPrContacts(List<int> priceCardIdList)
        {
            return await _context.InvTranInvoiceRequestContacts.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceCardId.Value))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.ContactId.GetValueOrDefault(),
                    Name = x.Contact.ContactName,
                    PriceCardId = x.CuPriceCardId.GetValueOrDefault()
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<PriceSubCategory>> GetPrSubCategoryList(List<int> priceCardIdList)
        {
            return await _context.CuPrTranSubcategories.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId.Value))
                .Select(x => new PriceSubCategory
                {
                    Id = x.Id,
                    SubCategoryName = x.SubCategory2.Name,
                    AQL_QTY_125 = x.AqlQty125,
                    AQL_QTY_1250 = x.AqlQty1250,
                    AQL_QTY_13 = x.AqlQty13,
                    AQL_QTY_20 = x.AqlQty20,
                    AQL_QTY_200 = x.AqlQty200,
                    AQL_QTY_315 = x.AqlQty315,
                    AQL_QTY_32 = x.AqlQty32,
                    AQL_QTY_50 = x.AqlQty50,
                    AQL_QTY_500 = x.AqlQty500,
                    AQL_QTY_8 = x.AqlQty8,
                    AQL_QTY_80 = x.AqlQty80,
                    AQL_QTY_800 = x.AqlQty800,
                    CuPriceCardId = x.CuPriceId,
                    SubCategory2Id = x.SubCategory2Id,
                    MandayBuffer = x.MandayBuffer,
                    MandayProductivity = x.MandayProductivity,
                    MandayReports = x.MandayReports,
                    UnitPrice = x.UnitPrice
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<PriceSpecialRule>> GetPrSpecialRules(List<int> priceCardIdList)
        {
            return await _context.CuPrTranSpecialRules.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPriceId.Value))
                .Select(x => new PriceSpecialRule
                {
                    Id = x.Id,
                    CuPriceCardId = x.CuPriceId,
                    MandayProductivity = x.MandayProductivity,
                    MandayReports = x.MandayReports,
                    UnitPrice = x.UnitPrice,
                    PieceRate_Billing_Q_Start = x.PieceRateBillingQStart,
                    Piecerate_Billing_Q_End = x.PiecerateBillingQEnd,
                    AdditionalFee = x.AdditionalFee,
                    Piecerate_MinBilling = x.PiecerateMinBilling,
                    PerInterventionRange1 = x.PerInterventionRange1,
                    PerInterventionRange2 = x.PerInterventionRange2,
                    Max_Style_Per_Day = x.MaxStylePerDay,
                    Max_Style_Per_Week = x.MaxStylePerWeek,
                    Max_Style_per_Month = x.MaxStylePerMonth,
                    Interventionfee = x.InterventionFee
                }).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// get the credit note invoice details (currency, inspection fees)
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<CreditNoteInvoiceDetails>> GetInvoiceDetailByBookingIds(List<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault())
                                                   && x.InvoiceStatus != (int)InvoiceStatus.Cancelled)
                .Select(y => new CreditNoteInvoiceDetails()
                {
                    InspectionId = y.InspectionId,
                    InvoiceId = y.Id,
                    InvoiceNo = y.InvoiceNo,
                    Currency = y.InvoiceCurrencyNavigation.CurrencyName,
                    CurrencyCode = y.InvoiceCurrencyNavigation.CurrencyCodeA,
                    InspectionFees = y.InspectionFees,
                    InvoiceDate = y.InvoiceDate,
                    InvoicePostDate = y.PostedDate,
                    BankAccountName = y.Bank.AccountName,
                    BankAccountNumber = y.Bank.AccountNumber,
                    BankName = y.Bank.BankName,
                    BankAddress = y.Bank.BankAddress,
                    BankSwiftCode = y.Bank.SwiftCode
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get factory contact id list
        /// </summary>
        /// <param name="factoryIds"></param>
        /// <returns></returns>
        public async Task<List<FactoryContact>> GetFactoryContactIdList(List<int> factoryIds)
        {
            return await _context.SuContacts.Where(x => x.Active.Value && factoryIds.Contains(x.SupplierId)).Select(x => new FactoryContact()
            {
                FactoryContactId = x.Id,
                FactoryId = x.SupplierId
            }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get supplier contact id list
        /// </summary>
        /// <param name="supplierIds"></param>
        /// <returns></returns>
        public async Task<List<SupplierContact>> GetSupplierContactIdList(List<int> supplierIds)
        {
            return await _context.SuContacts.Where(x => x.Active.Value && supplierIds.Contains(x.SupplierId)).Select(x => new SupplierContact()
            {
                SupplierContactId = x.Id,
                SupplierId = x.SupplierId
            }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get supplier address list
        /// </summary>
        /// <param name="supplierIds"></param>
        /// <returns></returns>
        public async Task<List<SupplierAddressDetails>> GetSupplierAddressList(List<int> supplierIds)
        {
            return await _context.SuAddresses.Where(x => supplierIds.Contains(x.SupplierId)).Select(x => new SupplierAddressDetails()
            {
                SupplierAddress = x.Address,
                SupplierId = x.SupplierId
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<string>> getInvoicePdfFiles(List<string> InvoiceNumbers)
        {
            return await _context.InvTranFiles.
                 Where(x => x.Active.Value && x.FileType == (int)InvRefFileTypeEnum.InvoicePreview && InvoiceNumbers.Contains(x.InvoiceNo)).
                 Select(y => y.InvoiceNo).Distinct().ToListAsync();
        }

        public async Task<List<InvTranFile>> GetInvoiceTransactionFiles(int invoiceId, int fileType)
        {
            return await _context.InvTranFiles.
                Where(x => x.InvoiceId == invoiceId && x.FileType == fileType && x.Active == true).
                AsNoTracking().IgnoreQueryFilters().ToListAsync();
        }

        public async Task<List<CommonPriceCardDataSource>> GetPrCity(List<int> priceCardIdList)
        {
            return await _context.CuPrCities.Where(x => x.Active.Value && priceCardIdList.Contains(x.CuPrId))
                .Select(x => new CommonPriceCardDataSource
                {
                    Id = x.FactoryCityId,
                    Name = x.FactoryCity.CityName,
                    PriceCardId = x.CuPrId
                }).AsNoTracking().ToListAsync();
        }
    }
}

