using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Invoice;
using DTO.MobileApp;
using DTO.Quotation;
using DTO.User;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class QuotationRepository : Repository, IQuotationRepository
    {

        public QuotationRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<QuBillMethod>> GetBillMethodList()
        {
            return await _context.QuBillMethods.ToListAsync();
        }

        public async Task<IEnumerable<QuPaidBy>> GetPaidByList()
        {
            return await _context.QuPaidBies.ToListAsync();
        }


        public async Task<IEnumerable<InspTransaction>> GetInspectionList(FilterOrderRequest request)
        {
            IQueryable<InspTransaction> data = null;
            if (request.BookingIds != null && request.BookingIds.Any())
            {
                data = _context.InspTransactions
                    .Include(x => x.PriceCategory)
                     .Include(x => x.InspProductTransactions)
                     .ThenInclude(x => x.InspPurchaseOrderTransactions)
                    .ThenInclude(x => x.Po)
                     .Include(x => x.InspPurchaseOrderTransactions)
                    .ThenInclude(x => x.DestinationCountry)
                   .Include(x => x.InspProductTransactions)
                    .ThenInclude(x => x.Product)
                   .ThenInclude(x => x.CuProductFileAttachments)
                   .Include(x => x.InspProductTransactions)
                       .ThenInclude(x => x.UnitNavigation)
                    .Include(x => x.InspProductTransactions)
                      .ThenInclude(x => x.Product)
                      .ThenInclude(x => x.ProductCategoryNavigation)
                    .Include(x => x.InspProductTransactions)
                       .ThenInclude(x => x.Product)
                       .ThenInclude(x => x.ProductSubCategoryNavigation)
                     .Include(x => x.InspProductTransactions)
                       .ThenInclude(x => x.Product)
                       .ThenInclude(x => x.ProductCategorySub2Navigation)
                     .Include(x => x.InspProductTransactions)
                       .ThenInclude(x => x.Product)
                       .ThenInclude(x => x.ProductCategorySub3Navigation)
                   .Include(x => x.InspProductTransactions)
                        .ThenInclude(x => x.AqlNavigation)
                   .Include(x => x.InspTranServiceTypes)
                       .ThenInclude(x => x.ServiceType)
                   .Include(x => x.Office)
                   .Include(x => x.Factory)
                   .ThenInclude(x => x.SuAddresses)
                   .Include(x => x.Status)
                   .Include(x => x.Customer)
                   .ThenInclude(x => x.CuServiceTypes)
                   .Include(x => x.InspTranFileAttachmentZips)
                   .Include(x => x.PaymentOptionsNavigation)
                     .Include(x => x.InspProductTransactions)
                        .ThenInclude(x => x.SampleTypeNavigation)
                   .Where(x => request.BookingIds.Contains(x.Id));
            }
            else
            {
                data = _context.InspTransactions
                    .Include(x => x.PriceCategory)
                   .Include(x => x.InspProductTransactions)
                    .ThenInclude(x => x.InspPurchaseOrderTransactions)
                    .ThenInclude(x => x.Po)
                   .Include(x => x.InspPurchaseOrderTransactions)
                    .ThenInclude(x => x.DestinationCountry)

                   .Include(x => x.InspProductTransactions)
                    .ThenInclude(x => x.Product)
                   .ThenInclude(x => x.CuProductFileAttachments)
                   .Include(x => x.InspProductTransactions)
                       .ThenInclude(x => x.UnitNavigation)
                    .Include(x => x.InspProductTransactions)
                      .ThenInclude(x => x.Product)
                      .ThenInclude(x => x.ProductCategoryNavigation)
                     .Include(x => x.InspProductTransactions)
                      .ThenInclude(x => x.Product)
                      .ThenInclude(x => x.ProductSubCategoryNavigation)
                    .Include(x => x.InspProductTransactions)
                      .ThenInclude(x => x.Product)
                      .ThenInclude(x => x.ProductCategorySub2Navigation)
                     .Include(x => x.InspProductTransactions)
                      .ThenInclude(x => x.Product)
                      .ThenInclude(x => x.ProductCategorySub3Navigation)
                   .Include(x => x.InspProductTransactions)
                        .ThenInclude(x => x.AqlNavigation)
                   .Include(x => x.InspTranServiceTypes)
                       .ThenInclude(x => x.ServiceType)
                   .Include(x => x.Office)
                    .Include(x => x.Factory)
                   .ThenInclude(x => x.SuAddresses)
                   .Include(x => x.Status)
                   .Include(x => x.Customer)
                   .ThenInclude(x => x.CuServiceTypes)
                   .Include(x => x.InspTranFileAttachmentZips)
                   .Include(x => x.PaymentOptionsNavigation)
                   .Include(x => x.InspProductTransactions)
                        .ThenInclude(x => x.SampleTypeNavigation)
                   .Where(x => x.CustomerId == request.CustomerId
                       && x.SupplierId == request.SupplierId
                       && x.FactoryId == request.FactoryId
                       && x.ServiceDateFrom >= request.StartDate.ToDateTime() && x.ServiceDateFrom <= request.EndDate.ToDateTime()
                       && x.ServiceDateTo >= request.StartDate.ToDateTime() && x.ServiceDateTo <= request.EndDate.ToDateTime());

                if (request.BookingNo != null && request.BookingNo.Value > 0)
                    data = data.Where(x => x.Id == request.BookingNo);

                if (request.OfficeIds != null && request.OfficeIds.Any())
                    data = data.Where(x => request.OfficeIds.Contains(x.OfficeId.Value));

                data = data.Where(x => (x.StatusId == (int)BookingStatus.Verified || x.StatusId == (int)BookingStatus.Confirmed ||

                x.StatusId == (int)BookingStatus.AllocateQC || x.StatusId == (int)BookingStatus.Rescheduled

                || x.StatusId == (int)BookingStatus.Inspected || x.StatusId == (int)BookingStatus.ReportSent)

                && !x.InvAutTranDetails.Any(z => z.InvoiceStatus != (int)InvoiceStatus.Cancelled)

                && !_context.QuInspProducts
                .Include(y => y.IdQuotationNavigation)
                .Include(y => y.ProductTran)
                    .Any(y => x.Id == y.ProductTran.InspectionId && y.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                 );
            }
            return await data.ToListAsync();
        }

        public async Task<IEnumerable<AudTransaction>> GetAuditList(FilterOrderRequest request)
        {
            var data = _context.AudTransactions
            .Include(x => x.AudTranServiceTypes)
                .ThenInclude(x => x.ServiceType)
            .Include(x => x.Office)
            .Where(x => x.CustomerId == request.CustomerId
                && x.SupplierId == request.SupplierId
                && x.FactoryId == request.FactoryId
                && x.ServiceDateFrom >= request.StartDate.ToDateTime() && x.ServiceDateFrom <= request.EndDate.ToDateTime()
                && x.ServiceDateTo >= request.StartDate.ToDateTime() && x.ServiceDateTo <= request.EndDate.ToDateTime());

            if (request.BookingNo != null && request.BookingNo.Value > 0)
                data = data.Where(x => x.Id == request.BookingNo);

            if (request.OfficeIds != null && request.OfficeIds.Any())
                data = data.Where(x => request.OfficeIds.Contains(x.OfficeId.Value));

            data = data.Where(x => (x.StatusId == (int)AuditStatus.Confirmed || x.StatusId == (int)AuditStatus.Scheduled
                  || x.StatusId == (int)AuditStatus.Audited) && !x.InvAutTranDetails.Any(z => z.InvoiceStatus != (int)InvoiceStatus.Cancelled)
                  && !_context.QuQuotationAudits.Include(y => y.IdQuotationNavigation)
                 .Any(y => x.Id == y.IdBooking && y.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled));

            return await data.ToListAsync();
        }
        public async Task<IEnumerable<QuQuotationInspManday>> GetQuotationInspManday(int quotationId)
        {
            return await _context.QuQuotationInspMandays.Where(x => x.Active.HasValue && x.Active.Value && x.QuotationId == quotationId).OrderBy(x => x.ServiceDate).ToListAsync();
        }
        public async Task<IEnumerable<QuQuotationAudManday>> GetQuotationAudManday(int quotationId)
        {
            return await _context.QuQuotationAudMandays.Where(x => x.QuotationId == quotationId).ToListAsync();
        }
        public async Task<QuQuotation> GetBookingPoDetails(int id)
        {
            return await _context.QuQuotations
                        .Include(x => x.QuQuotationInsps)
                            .ThenInclude(x => x.IdBookingNavigation)
                            .ThenInclude(x => x.InspProductTransactions)
                                .ThenInclude(x => x.MajorNavigation)
                        .Include(x => x.QuQuotationInsps)
                            .ThenInclude(x => x.IdBookingNavigation)
                            .ThenInclude(x => x.InspProductTransactions)
                                .ThenInclude(x => x.MinorNavigation)
                        .Include(x => x.QuQuotationInsps)
                            .ThenInclude(x => x.IdBookingNavigation)
                            .ThenInclude(x => x.InspProductTransactions)
                                .ThenInclude(x => x.CriticalNavigation)
                        .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<QuQuotation> GetOnlyQuotation(int id)
        {
            return await _context.QuQuotations
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<QuQuotation> GetQuotation(int id)
        {
            return await _context.QuQuotations
                // .Include(x => x.Customer)
                //     .ThenInclude(x => x.CuServiceTypes)
                //         .ThenInclude(x => x.LevelPick1Navigation)
                // .Include(x => x.Customer)
                //     .ThenInclude(x => x.CuServiceTypes)
                //        .ThenInclude(x => x.MinorTolerancePick1Navigation)
                //.Include(x => x.Customer)
                //     .ThenInclude(x => x.CuServiceTypes)
                //        .ThenInclude(x => x.MajorTolerancePick1Navigation)
                .Include(x => x.Customer)
                //.ThenInclude(x => x.CuServiceTypes)
                //      .ThenInclude(x => x.CriticalPick1Navigation)
                .Include(x => x.QuQuotationCustomerContacts)
                    .ThenInclude(x => x.IdContactNavigation)
                .Include(x => x.QuQuotationSupplierContacts)
                    .ThenInclude(x => x.IdContactNavigation)
                .Include(x => x.QuQuotationFactoryContacts)
                    .ThenInclude(x => x.IdContactNavigation)
                .Include(x => x.QuQuotationContacts)
                    .ThenInclude(x => x.IdContactNavigation)
                        .ThenInclude(x => x.ItUserMasters)
                .Include(x => x.QuQuotationAudits)
                    .ThenInclude(x => x.IdBookingNavigation)
                        .ThenInclude(x => x.AudTranServiceTypes)
                            .ThenInclude(x => x.ServiceType)
                    // combine order list added
                    .Include(x => x.QuInspProducts)
                    .ThenInclude(x => x.ProductTran)
                        .ThenInclude(x => x.Inspection)
                        .ThenInclude(x => x.PriceCategory)

                    .Include(x => x.QuInspProducts)
                    .ThenInclude(x => x.ProductTran)
                        .ThenInclude(x => x.Inspection).ThenInclude(x => x.PaymentOptionsNavigation)
                  .Include(x => x.QuInspProducts)
                    .ThenInclude(x => x.ProductTran)
                        .ThenInclude(x => x.Inspection)
                            .ThenInclude(x => x.InspPurchaseOrderTransactions)

                .Include(x => x.QuInspProducts)
                    .ThenInclude(x => x.ProductTran)
                        .ThenInclude(x => x.Inspection)
                            .ThenInclude(x => x.InspTranFileAttachmentZips)



                .Include(x => x.QuInspProducts)
                    .ThenInclude(x => x.ProductTran)
                                .ThenInclude(x => x.Product)
                                    .ThenInclude(x => x.CuProductFileAttachments)
            //Fetch ProductSubcategory and productsubcategory2 on edit quotation

                 .Include(x => x.QuInspProducts)
                     .ThenInclude(x => x.ProductTran)
                                .ThenInclude(x => x.Product)
                                    .ThenInclude(x => x.ProductCategoryNavigation)

                .Include(x => x.QuInspProducts)
                    .ThenInclude(x => x.ProductTran)
                                .ThenInclude(x => x.Product)
                                    .ThenInclude(x => x.ProductSubCategoryNavigation)

                    .Include(x => x.QuInspProducts)
                       .ThenInclude(x => x.ProductTran)
                                .ThenInclude(x => x.Product)
                                    .ThenInclude(x => x.ProductCategorySub2Navigation)
                     //Fetch ProductSubcategory and productsubcategory2 on edit quotation

                     .Include(x => x.QuInspProducts)
                       .ThenInclude(x => x.ProductTran)
                                .ThenInclude(x => x.Product)
                                    .ThenInclude(x => x.ProductCategorySub3Navigation)

                .Include(x => x.QuInspProducts)
                    .ThenInclude(x => x.ProductTran)
                        .ThenInclude(x => x.InspPurchaseOrderTransactions)
                       .ThenInclude(x => x.Po)

                .Include(x => x.QuInspProducts)
                   .ThenInclude(x => x.ProductTran)
                        .ThenInclude(x => x.InspPurchaseOrderTransactions)
                         .ThenInclude(x => x.DestinationCountry)

                .Include(x => x.QuInspProducts)
                    .ThenInclude(x => x.ProductTran)
                        .ThenInclude(x => x.Inspection)

                .Include(x => x.QuInspProducts)
                       .ThenInclude(x => x.ProductTran)
                                  .ThenInclude(x => x.UnitNavigation)

            .Include(x => x.QuInspProducts)
                       .ThenInclude(x => x.ProductTran)
                            .ThenInclude(x => x.AqlNavigation)

             .Include(x => x.QuInspProducts)
                       .ThenInclude(x => x.ProductTran)
                        .ThenInclude(x => x.Inspection)
                        .ThenInclude(x => x.InspTranServiceTypes)
                            .ThenInclude(x => x.ServiceType)

               .Include(x => x.QuInspProducts)
                    .ThenInclude(x => x.ProductTran)
                        .ThenInclude(x => x.Inspection)
                        .ThenInclude(x => x.Status)

                 .Include(x => x.QuInspProducts)
                    .ThenInclude(x => x.ProductTran)
                        .ThenInclude(x => x.SampleTypeNavigation)

                .Include(x => x.Country)
                .Include(x => x.Service)
                .Include(x => x.BillingMethod)
                .Include(x => x.BillingPaidBy)
                .Include(x => x.Currency)
                .Include(x => x.Customer)
                .Include(x => x.Supplier)
                .Include(x => x.Factory)
                    .ThenInclude(x => x.SuAddresses)
                    .ThenInclude(x => x.Country)
                .Include(x => x.Office)
                .Include(x => x.IdStatusNavigation)
                .Include(x => x.QuQuotationInsps)
                .Include(x => x.QuQuotationInspMandays)
                .Include(x => x.QuQuotationAudMandays)
                //.Where(x => x.QuQuotationInspMandays.Any(y => y.Active.HasValue && y.Active.Value))
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<InspTransaction>> GetInspectionListByBooking(IEnumerable<int> idList)
        {
            return await _context.InspTransactions
                        .Include(x => x.InspProductTransactions)
                         .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.ProductCategoryNavigation)
                        .Include(x => x.InspProductTransactions)
                            .ThenInclude(x => x.UnitNavigation)
                        .Include(x => x.InspProductTransactions)
                             .ThenInclude(x => x.AqlNavigation)
                        .Include(x => x.InspTranServiceTypes)
                            .ThenInclude(x => x.ServiceType)
                        .Include(x => x.Office)
                        .Where(x => idList.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<AudTransaction>> GetAuditListByBooking(IEnumerable<int> idList)
        {
            return await _context.AudTransactions
                        .Include(x => x.AudTranServiceTypes)
                            .ThenInclude(x => x.ServiceType)
                        .Include(x => x.Office)
                        .Where(x => idList.Contains(x.Id)).ToListAsync();
        }


        public async Task<bool> SetStatus(SetStatusRequest objStatusRequest)
        {
            var quotation = await _context.QuQuotations.FirstOrDefaultAsync(x => x.Id == objStatusRequest.Id);

            if (quotation == null)
                return false;

            quotation.IdStatus = objStatusRequest.StatusId;
            quotation.ApiRemark = objStatusRequest.ApiRemark;
            quotation.CustomerRemark = objStatusRequest.CusComment;
            quotation.ApiInternalRemark = objStatusRequest.ApiInternalRemark;
            if (objStatusRequest.StatusId == (int)QuotationStatus.CustomerValidated)
            {
                quotation.ValidatedOn = objStatusRequest.ValidatedOn;
                quotation.ValidatedBy = objStatusRequest.ValidatedById;
            }

            _context.Entry(quotation).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return true;
        }

        //Quotation Audit Export Details
        public async Task<IEnumerable<QuotationInspAuditExportRepo>> GetQuotationAuditExport(IQueryable<int> quIds)
        {
            return await _context.QuQuotationAudits
               .Where(x => quIds.Contains(x.IdQuotation))
               .Select(x => new QuotationInspAuditExportRepo()
               {
                   QuotationId = x.IdQuotation,
                   QuotationDate = x.IdQuotationNavigation.CreatedDate,
                   BookingId = x.IdBooking,
                   CustomerBookingNo = x.IdBookingNavigation.CustomerBookingNo,
                   ServiceDateFrom = x.IdBookingNavigation.ServiceDateFrom,
                   ServiceDateTo = x.IdBookingNavigation.ServiceDateTo,
                   CustomerId = x.IdQuotationNavigation.CustomerId,
                   CustomerName = x.IdQuotationNavigation.Customer.CustomerName,
                   CustomerLegalName = x.IdQuotationNavigation.CustomerLegalName,
                   SupplierId = x.IdQuotationNavigation.SupplierId,
                   SupplierName = x.IdQuotationNavigation.Supplier.SupplierName,
                   SupplierLegalName = x.IdQuotationNavigation.SupplierLegalName,
                   FactoryId = x.IdQuotationNavigation.FactoryId,
                   FactoryName = x.IdQuotationNavigation.Factory.SupplierName,
                   FactoryLegalName = x.IdQuotationNavigation.LegalFactoryName,
                   FactoryAddress = x.IdQuotationNavigation.FactoryAddress,
                   Office = x.IdQuotationNavigation.Office.LocationName,
                   QuotationStatusId = x.IdQuotationNavigation.IdStatus,
                   QuotationStatus = x.IdQuotationNavigation.IdStatusNavigation.Label,
                   EstimatedManDay = x.IdQuotationNavigation.EstimatedManday,
                   InspectionCost = x.IdQuotationNavigation.InspectionFees,
                   Discount = x.IdQuotationNavigation.Discount,
                   TravelCostAir = x.IdQuotationNavigation.TravelCostsAir.GetValueOrDefault(),
                   TravelCostLand = x.IdQuotationNavigation.TravelCostsLand.GetValueOrDefault(),
                   TravelCostHotel = x.IdQuotationNavigation.TravelCostsHotel.GetValueOrDefault(),
                   OtherCost = x.IdQuotationNavigation.OtherCosts,
                   TotalFee = x.IdQuotationNavigation.TotalCost,
                   Currency = x.IdQuotationNavigation.Currency.CurrencyName,
                   BillPaidById = x.IdQuotationNavigation.BillingPaidById,
                   BillPaidBy = x.IdQuotationNavigation.BillingPaidBy.Label,
                   APIRemark = x.IdQuotationNavigation.ApiRemark,
                   CustomerRemark = x.IdQuotationNavigation.CustomerRemark,
                   PaymentTerm = x.IdQuotationNavigation.PaymentTermsNavigation.Name,
                   BillingEntity = x.IdQuotationNavigation.BillingEntityNavigation.Name,
                   ValidatedOn = x.IdQuotationNavigation.ValidatedOn,
                   ValidatedByName = x.IdQuotationNavigation.ValidatedByNavigation.FullName,
                   ValidatedByUserType = x.IdQuotationNavigation.ValidatedByNavigation.UserTypeId,
                   BrandName = x.IdBookingNavigation.Brand.Name,
                   DepartmentName = x.IdBookingNavigation.Department.Name
               }).AsNoTracking().ToListAsync();
        }

        //Quotation Insp Export Details
        public async Task<IEnumerable<QuotationInspAuditExportRepo>> GetQuotationInspProductExport(IQueryable<int> quIds)
        {
            return await _context.QuInspProducts
               .Where(x => quIds.Contains(x.IdQuotation) && x.ProductTran.Active.HasValue && x.ProductTran.Active == true)
               .Select(x => new QuotationInspAuditExportRepo()
               {
                   QuotationId = x.IdQuotation,
                   QuotationDate = x.IdQuotationNavigation.CreatedDate,
                   BookingId = x.ProductTran.InspectionId,
                   CustomerBookingNo = x.ProductTran.Inspection.CustomerBookingNo,
                   ServiceDateFrom = x.ProductTran.Inspection.ServiceDateFrom,
                   ServiceDateTo = x.ProductTran.Inspection.ServiceDateTo,
                   ProductRefId = x.ProductTranId,
                   ProductReference = x.ProductTran.Product.ProductId,
                   ProductDescription = x.ProductTran.Product.ProductDescription,
                   FBReportStartDate = x.ProductTran.FbReport.ServiceFromDate,
                   FBReportEndDate = x.ProductTran.FbReport.ServiceToDate,
                   CustomerId = x.IdQuotationNavigation.CustomerId,
                   CustomerName = x.IdQuotationNavigation.Customer.CustomerName,
                   CustomerLegalName = x.IdQuotationNavigation.CustomerLegalName,
                   SupplierId = x.IdQuotationNavigation.SupplierId,
                   SupplierName = x.IdQuotationNavigation.Supplier.SupplierName,
                   SupplierLegalName = x.IdQuotationNavigation.SupplierLegalName,
                   FactoryId = x.IdQuotationNavigation.FactoryId,
                   FactoryName = x.IdQuotationNavigation.Factory.SupplierName,
                   FactoryLegalName = x.IdQuotationNavigation.LegalFactoryName,
                   FactoryAddress = x.IdQuotationNavigation.FactoryAddress,
                   Office = x.IdQuotationNavigation.Office.LocationName,
                   QuotationStatusId = x.IdQuotationNavigation.IdStatus,
                   QuotationStatus = x.IdQuotationNavigation.IdStatusNavigation.Label,
                   ReportResult = x.ProductTran.FbReport.OverAllResult,
                   BookingQty = x.ProductTran.TotalBookingQuantity,
                   SampleSize = x.ProductTran.AqlQuantity.GetValueOrDefault(),
                   CombineAQL = x.ProductTran.CombineAqlQuantity,
                   FactoryRef = x.ProductTran.Product.FactoryReference,
                   InspectionCost = x.IdQuotationNavigation.InspectionFees,
                   Discount = x.IdQuotationNavigation.Discount,
                   TravelCostAir = x.IdQuotationNavigation.TravelCostsAir.GetValueOrDefault(),
                   TravelCostLand = x.IdQuotationNavigation.TravelCostsLand.GetValueOrDefault(),
                   TravelCostHotel = x.IdQuotationNavigation.TravelCostsHotel.GetValueOrDefault(),
                   OtherCost = x.IdQuotationNavigation.OtherCosts,
                   TotalFee = x.IdQuotationNavigation.TotalCost,
                   Currency = x.IdQuotationNavigation.Currency.CurrencyName,
                   BillPaidById = x.IdQuotationNavigation.BillingPaidById,
                   BillPaidBy = x.IdQuotationNavigation.BillingPaidBy.Label,
                   APIRemark = x.IdQuotationNavigation.ApiRemark,
                   CustomerRemark = x.IdQuotationNavigation.CustomerRemark,
                   PaymentTerm = x.IdQuotationNavigation.PaymentTermsNavigation.Name,
                   BookingStatus = x.ProductTran.Inspection.Status.Status,
                   BillingEntity = x.IdQuotationNavigation.BillingEntityNavigation.Name,
                   ValidatedOn = x.IdQuotationNavigation.ValidatedOn,
                   ValidatedByName = x.IdQuotationNavigation.ValidatedByNavigation.FullName,
                   ValidatedByUserType = x.IdQuotationNavigation.ValidatedByNavigation.UserTypeId,
               }).OrderByDescending(x => x.QuotationId).AsNoTracking().ToListAsync();
        }


        public IQueryable<QuInspProduct> GetQuotationInspProductList(QuotationSummaryRepoRequest request)
        {
            var data = _context.QuInspProducts
              .Where(x => x.IdQuotationNavigation.ServiceId == (int)request.ServiceId);

            //.Where(x => request.Statusidlst!=null && request.Statusidlst.Count()>0 && request.Statusidlst.Contains(x.IdStatus));
            if (request.Statusidlst != null && request.Statusidlst.Any())
                data = data.Where(x => request.Statusidlst.Contains(x.IdQuotationNavigation.IdStatus));

            if (request.Customerid > 0)
                data = data.Where(x => x.IdQuotationNavigation.CustomerId == request.Customerid);

            if (request.Supplierid > 0)
                data = data.Where(x => x.IdQuotationNavigation.SupplierId == request.Supplierid);

            if (request.Factoryidlst != null && request.Factoryidlst.Any())
                data = data.Where(x => request.Factoryidlst.Contains(x.IdQuotationNavigation.FactoryId));

            if (request.Officeidlst != null && request.Officeidlst.Any())
                data = data.Where(x => request.Officeidlst.Contains(x.IdQuotationNavigation.OfficeId));

            return data;
        }


        public IQueryable<QuQuotationAudit> GetQuotationAuditList(QuotationSummaryRepoRequest request)
        {
            var data = _context.QuQuotationAudits
            //  .Include(x => x.IdQuotationNavigation)
              .Where(x => x.IdQuotationNavigation.ServiceId == (int)request.ServiceId);

            //.Where(x => request.Statusidlst!=null && request.Statusidlst.Count()>0 && request.Statusidlst.Contains(x.IdStatus));
            if (request.Statusidlst != null && request.Statusidlst.Count() > 0)
                data = data.Where(x => request.Statusidlst.Contains(x.IdQuotationNavigation.IdStatus));

            if (request.Customerid > 0)
                data = data.Where(x => x.IdQuotationNavigation.CustomerId == request.Customerid);

            if (request.Supplierid > 0)
                data = data.Where(x => x.IdQuotationNavigation.SupplierId == request.Supplierid);

            if (request.Factoryidlst != null && request.Factoryidlst.Any())
                data = data.Where(x => request.Factoryidlst.Contains(x.IdQuotationNavigation.FactoryId));

            return data;
        }


        private IQueryable<QuQuotation> GetQuotationList(QuotationSummaryRepoRequest request)
        {
            var data = _context.QuQuotations
                    //.Include(x => x.BillingMethod)
                    //.Include(x => x.BillingPaidBy)
                    //.Include(x => x.Currency)
                    //    //.Include(x => x.QuTranStatusLogs)
                    //    //            .ThenInclude(x => x.CreatedByNavigation)
                    //    .Include(x => x.QuInspProducts)
                    //        .ThenInclude(x => x.ProductTran)
                    //            .ThenInclude(x => x.Inspection)
                    //                .ThenInclude(x => x.InspTranServiceTypes)
                    //                    .ThenInclude(x => x.ServiceType)

                    //      .Include(x => x.QuQuotationInsps)
                    //            .ThenInclude(x => x.IdBookingNavigation)
                    //                .ThenInclude(x => x.Status)

                    //    .Include(x => x.QuQuotationAudits)
                    //        .ThenInclude(x => x.IdBookingNavigation)
                    //        .ThenInclude(x => x.AudTranServiceTypes)
                    //        .ThenInclude(x => x.ServiceType)
                    //      .Include(x => x.QuQuotationAudits)
                    //        .ThenInclude(x => x.IdBookingNavigation)
                    //        .ThenInclude(x => x.AudTranReportDetails)
                    //    .Include(x => x.Customer)
                    //    //.ThenInclude(x => x.CuProducts)
                    //    .Include(x => x.QuInspProducts)
                    //        .ThenInclude(x => x.ProductTran)
                    //        .ThenInclude(x => x.InspPurchaseOrderTransactions)
                    //          .ThenInclude(x => x.Po)
                    //    .Include(x => x.QuInspProducts)
                    //            .ThenInclude(x => x.ProductTran)
                    //            .ThenInclude(x => x.Product)
                    //    .Include(x => x.QuInspProducts)
                    //            .ThenInclude(x => x.ProductTran)
                    //            .ThenInclude(x => x.FbReport)
                    //    .Include(x => x.Supplier)
                    //    .Include(x => x.Factory)
                    //    .Include(x => x.Office)
                    //    .Include(x => x.IdStatusNavigation)
                    //    .Include(x => x.BillingEntityNavigation)
                    //    .Include(x => x.PaymentTermsNavigation)
                    //    .Include(x => x.ValidatedByNavigation)
                    .Where(x => x.ServiceId == (int)request.ServiceId);


            if (request.Statusidlst != null && request.Statusidlst.Count() > 0)
                data = data.Where(x => request.Statusidlst.Contains(x.IdStatus));

            if (request.Customerid > 0)
                data = data.Where(x => x.CustomerId == request.Customerid);

            if (request.Supplierid > 0)
                data = data.Where(x => x.SupplierId == request.Supplierid);

            if (request.Factoryidlst != null && request.Factoryidlst.Any())
                data = data.Where(x => request.Factoryidlst.Contains(x.FactoryId));

            if (request.Officeidlst != null && request.Officeidlst.Any())
                data = data.Where(x => request.Officeidlst.Contains(x.OfficeId));

            return data;
        }

        public async Task<IEnumerable<QuotationInsp>> GetQuotationInspList(List<int> quIds)
        {
            return await _context.QuQuotationInsps
                .Where(x => quIds.Contains(x.IdQuotation) && x.IdBookingNavigation.Status.Active.HasValue && x.IdBookingNavigation.Status.Active.Value)
              .Select(x => new QuotationInsp()
              {
                  QuotationId = x.IdQuotation,
                  BookingId = x.IdBooking,
                  CusBookingNo = x.IdBookingNavigation.CustomerBookingNo,
                  BookingStatusId = x.IdBookingNavigation.StatusId,
                  BookingStatusName = x.IdBookingNavigation.Status.Status,
                  ServiceDateFrom = x.IdBookingNavigation.ServiceDateFrom,
                  ServiceDateTo = x.IdBookingNavigation.ServiceDateTo,
                  ServiceTypeName = x.IdBookingNavigation.InspTranServiceTypes.FirstOrDefault(y => y.Active).ServiceType.Name,
                  IsEAQF = x.IdBookingNavigation.IsEaqf
              }).ToListAsync();
        }
        public async Task<IEnumerable<QuotationInvoiceItem>> GetQuotationAuditandInvoice(List<int> quIds)
        {
            return await _context.QuQuotationAudits
                .Where(x => quIds.Contains(x.IdQuotation) && x.IdBookingNavigation.Status.Active.HasValue && x.IdBookingNavigation.Status.Active.Value)
               .Select(x => new QuotationInvoiceItem
               {
                   QuotationId = x.IdQuotation,
                   BookingId = x.IdBooking,
                   CusBookingNo = x.IdBookingNavigation.CustomerBookingNo,
                   BookingStatusId = x.IdBookingNavigation.StatusId,
                   BookingStatusName = x.IdBookingNavigation.Status.Status,
                   ServiceDateFrom = x.IdBookingNavigation.ServiceDateFrom,
                   ServiceDateTo = x.IdBookingNavigation.ServiceDateTo,
                   QuoInvoiceDate = x.InvoiceDate,
                   QuoInvoiceNo = x.InvoiceNo,
                   QuoInvoiceREmarks = x.InvoiceRemarks,
                   Manday = x.NoOfManDay,
                   TravelDistance = x.TravelDistance,
                   TravelTime = x.TravelTime,
                   ServiceTypeName = x.IdBookingNavigation.AudTranServiceTypes.FirstOrDefault(y => y.Active).ServiceType.Name,
                   BrandName = x.IdBookingNavigation.Brand.Name,
                   DepartmentName = x.IdBookingNavigation.Department.Name
               }).ToListAsync();



        }
        //Get Uuotation Audit and invoice by Query id
        public async Task<IEnumerable<QuotationInvoiceItem>> GetQuotationAuditDetails(IQueryable<int> quIds)
        {
            return await _context.QuQuotationAudits
                .Where(x => quIds.Contains(x.IdQuotation) && x.IdBookingNavigation.Status.Active.HasValue && x.IdBookingNavigation.Status.Active.Value)
               .Select(x => new QuotationInvoiceItem
               {
                   QuotationId = x.IdQuotation,
                   BookingId = x.IdBooking,
                   CusBookingNo = x.IdBookingNavigation.CustomerBookingNo,
                   BookingStatusId = x.IdBookingNavigation.StatusId,
                   BookingStatusName = x.IdBookingNavigation.Status.Status,
                   ServiceDateFrom = x.IdBookingNavigation.ServiceDateFrom,
                   ServiceDateTo = x.IdBookingNavigation.ServiceDateTo,
                   Manday = x.NoOfManDay,
                   TravelDistance = x.TravelDistance,
                   TravelTime = x.TravelTime,
                   ServiceTypeName = x.IdBookingNavigation.AudTranServiceTypes.Where(y => y.Active).FirstOrDefault().ServiceType.Name
               }).AsNoTracking().ToListAsync();



        }

        //Get quotation audit report  by query id
        public async Task<IEnumerable<QuotationAuditReportItem>> GetQuotationAuditReportDetails(IQueryable<int> BookIds)
        {
            return await _context.AudTranReportDetails
                .Where(x => BookIds.Contains(x.AuditId) && x.Active)
               .Select(x => new QuotationAuditReportItem
               {
                   BookingId = x.AuditId,
                   ServiceDateFrom = x.ServiceDateFrom,
                   ServiceDateTo = x.ServiceDateTo
               }).AsNoTracking().ToListAsync();



        }
        public async Task<IEnumerable<QuInspProduct>> GetQuotationInspProdList(List<int> quIds)
        {
            return await _context.QuInspProducts
                .Where(x => quIds.Contains(x.IdQuotation)).ToListAsync();
        }


        public IQueryable<QuotationItemRepo> GetQuotationItemByBookingAndAudit(IQueryable<int> QuIds)
        {
            var data = _context.QuQuotations
               .Where(x => QuIds.Contains(x.Id))
              .Select(x => new QuotationItemRepo()
              {
                  QuotationId = x.Id,
                  QuotationDate = x.CreatedDate.ToString(StandardDateFormat),
                  CustomerId = x.Customer.Id,
                  CustomerName = x.Customer.CustomerName,
                  SupplierName = x.Supplier.SupplierName,
                  SupplierId = x.Supplier.Id,
                  FactoryName = x.Factory.SupplierName,
                  Office = x.Office.LocationName,
                  StatusId = x.IdStatus,
                  StatusName = x.IdStatusNavigation.Label,
                  discount = x.Discount.GetValueOrDefault(),
                  EstimatedManDay = x.EstimatedManday,
                  InspectionFees = x.InspectionFees,
                  TravelCost = x.TravelCostsAir + x.TravelCostsLand + x.TravelCostsHotel,
                  TotalCost = x.TotalCost,
                  ServiceId = x.ServiceId,
                  BillingEntity = x.BillingEntityNavigation.Name,
                  OtherCost = x.OtherCosts.GetValueOrDefault(),
                  PaymentTerm = x.PaymentTermsNavigation.Name,
                  BillMethodName = x.BillingMethod.Label,
                  BillPaidByName = x.BillingPaidBy.Label,
                  BillPaidById = x.BillingPaidById,
                  CurrencyName = x.Currency.CurrencyCodeA,
                  ValidatedBy = x.ValidatedByNavigation.UserTypeId,
                  ValidatedUserName = string.IsNullOrEmpty(x.ValidatedByNavigation.FullName) ? "" : x.ValidatedByNavigation.FullName,
                  ValidatedOn = x.ValidatedOn.HasValue ? x.ValidatedOn.Value.ToString(StandardDateTimeFormat1) : null,
                  customerRemark = x.CustomerRemark ?? "",
              });
            return data;
        }




        public async Task<IEnumerable<QuStatus>> GetStatusList()
        {
            return await _context.QuStatuses.ToListAsync();
        }



        public async Task<List<QuotationExportInformation>> GetQuotationAdditionalInfo(List<int> quotids)
        {
            return await _context.QuQuotations
                      .Include(x => x.Currency)
                      .Include(x => x.BillingPaidBy)
                      .Include(x => x.Customer)
                      .ThenInclude(x => x.CuAddresses)
                      .Include(x => x.Supplier)
                      .ThenInclude(x => x.SuAddresses)
                      .Include(x => x.QuQuotationFactoryContacts)
                      .ThenInclude(x => x.IdContactNavigation)
                      .Include(x => x.QuQuotationCustomerContacts)
                      .ThenInclude(x => x.IdContactNavigation)
                      .Include(x => x.QuQuotationSupplierContacts)
                      .ThenInclude(x => x.IdContactNavigation)
                      .Where(x => quotids.Contains(x.Id))
                      .Select(x => new QuotationExportInformation
                      {
                          quotationid = x.Id,
                          currency = x.Currency.CurrencyName,
                          apiremark = x.ApiRemark,
                          billPaidBy = x.BillingPaidBy.Label,
                          customerLegalName = x.CustomerLegalName,
                          customerRemark = x.CustomerRemark,
                          factoryLegalName = x.LegalFactoryName,
                          supplierLegalName = x.SupplierLegalName,
                          billPaidByAddress = (x.BillingPaidById == (int)QuotationPaidBy.customer ? x.Customer.CuAddresses.Where(y => y.AddressType == (int)RefAddressTypeEnum.Accounting).Any() ?
                                                x.Customer.CuAddresses.Where(y => y.AddressType == (int)RefAddressTypeEnum.Accounting).Select(y => y.Address).FirstOrDefault()
                                                : x.Customer.CuAddresses.Where(y => y.AddressType == (int)RefAddressTypeEnum.HeadOffice).Select(y => y.Address).FirstOrDefault()
                                            : x.BillingPaidById == (int)QuotationPaidBy.supplier ? x.Supplier.SuAddresses.Where(z => z.AddressTypeId == (int)SuAddressTypeEnum.Accounting).Any() ?
                                            x.Supplier.SuAddresses.Where(z => z.AddressTypeId == (int)SuAddressTypeEnum.Accounting).Select(t => t.Address).FirstOrDefault()
                                            : x.Supplier.SuAddresses.Where(z => z.AddressTypeId == (int)SuAddressTypeEnum.Headoffice).Select(t => t.Address).FirstOrDefault()
                                            : x.FactoryAddress),
                          billPaidByContact = string.Join(',', (x.BillingPaidById == (int)QuotationPaidBy.customer ? x.QuQuotationCustomerContacts.Where(z => z.Quotation).Select(y => y.IdContactNavigation.ContactName)
                                            : x.BillingPaidById == (int)QuotationPaidBy.supplier ? x.QuQuotationSupplierContacts.Where(z => z.Quotation).Select(y => y.IdContactNavigation.ContactName)
                                            : x.QuQuotationFactoryContacts.Where(z => z.Quotation).Select(y => y.IdContactNavigation.ContactName)).ToList())
                      }).ToListAsync();
        }
        public async Task<bool> QuotationInspExists(int bookingId)
        {
            return await _context.QuQuotationInsps
                   .Include(x => x.IdQuotationNavigation)
                   .Where(x => x.IdBooking == bookingId && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).AnyAsync();
        }
        public async Task<int> GetQuotationIdByAuditid(int AuditbookingId)
        {
            return await _context.QuQuotationAudits
                   .Include(x => x.IdQuotationNavigation)
                   .Where(x => x.IdBooking == AuditbookingId && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                   .Select(x => x.IdQuotation).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<QuQuotationInspManday>> GetQuotationInspManDay(int bookingId)
        {
            return await _context.QuQuotationInspMandays
                .Where(x => x.BookingId == bookingId && x.Quotation.IdStatus != (int)QuotationStatus.Canceled
                 && x.Active.Value)
                .Select(x => new QuQuotationInspManday
                {
                    QuotationId = x.QuotationId,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    BookingId = x.BookingId,
                    NoOfManday = x.NoOfManday,
                    ServiceDate = x.ServiceDate,
                    Id = x.Id,
                    Remarks = x.Remarks,
                    Active = x.Active,
                    Quotation = x.Quotation
                }).OrderBy(x => x.ServiceDate).ToListAsync();
        }
        public async Task<IEnumerable<User>> CustomerEmailIdQuotation(int quotationId, int[] statusIdList)
        {
            return await _context.QuQuotationCustomerContacts
                 .Where(x => x.IdQuotation == quotationId && x.Email && statusIdList.Contains(x.IdQuotationNavigation.IdStatus))
                  .Select(x => new User
                  {
                      EmailAddress = x.IdContactNavigation.Email
                  }).ToListAsync();
        }
        public async Task<IEnumerable<User>> FactoryEmailIdQuotation(int quotationId, int[] statusIdList)
        {
            return await _context.QuQuotationFactoryContacts
               .Where(x => x.IdQuotation == quotationId && x.Email && statusIdList.Contains(x.IdQuotationNavigation.IdStatus))
                .Select(x => new User
                {
                    EmailAddress = x.IdContactNavigation.Mail
                }).ToListAsync();
        }
        public async Task<IEnumerable<User>> SupplierEmailIdQuotation(int quotationId, int[] statusIdList)
        {
            return await _context.QuQuotationSupplierContacts
              .Where(x => x.IdQuotation == quotationId && x.Email && statusIdList.Contains(x.IdQuotationNavigation.IdStatus))
               .Select(x => new User
               {
                   EmailAddress = x.IdContactNavigation.Mail
               }).ToListAsync();

        }
        public async Task<IEnumerable<User>> InternalUserEmailIdQuotation(int quotationId, int[] statusIdList)
        {
            return await _context.QuQuotationContacts
                    .Where(x => x.IdQuotation == quotationId && x.Email && statusIdList.Contains(x.IdQuotationNavigation.IdStatus))
                    .Select(x => new User
                    {
                        EmailAddress = x.IdContactNavigation.CompanyEmail
                    }).ToListAsync();
        }

        //audit quotation exists which is not canceled return the audit ids
        public async Task<IEnumerable<int>> IsAuditQuotationExists(IEnumerable<int> auditIds, int quotationId)
        {
            return await _context.QuQuotationAudits.Where(x => x.IdQuotation != quotationId && auditIds.Contains(x.IdBooking)
                    && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).Select(x => x.IdBooking).ToListAsync();
        }

        // booking quotation exists which is not canceled return the booking ids
        public async Task<IEnumerable<int>> IsBookingQuotationExists(IEnumerable<int> bookingIds, int quotationId)
        {
            return await _context.QuInspProducts.Where(x => x.IdQuotation != quotationId && bookingIds.Contains(x.ProductTran.InspectionId)
                    && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).Select(x => x.ProductTran.InspectionId).Distinct().ToListAsync();
        }

        //Fetch the quotation details for client specific export
        public async Task<ClientQuotationItem> GetClientQuotation(int quotationId)
        {
            return await _context.QuQuotations.Where(x => x.Id == quotationId && x.IdStatus != (int)QuotationStatus.Canceled)
                .Select(y => new ClientQuotationItem
                {
                    QuotationId = quotationId,
                    Booking = y.QuQuotationInsps,
                    QuotationDate = y.CreatedDate,
                    CustomerId = y.CustomerId,
                    SupplierId = y.SupplierId,
                    FactoryId = y.FactoryId,
                    SupplierName = y.Supplier.SupplierName,
                    FatoryName = y.Factory.SupplierName,
                    QuotationPrice = y.InspectionFees,
                    TravelCostAir = y.TravelCostsAir,
                    TravelCostLand = y.TravelCostsLand,
                    HotelCost = y.TravelCostsHotel,
                    OtherCost = y.OtherCosts,
                    ManDay = y.EstimatedManday,
                    CuServiceType = y.Customer.CuServiceTypes,
                    InspServiceTypeId = y.QuQuotationInsps.SelectMany(x => x.IdBookingNavigation.InspTranServiceTypes).Where(z => z.Active)
                                    .Select(x => x.ServiceTypeId).FirstOrDefault(),
                    InspectionLocation = y.Factory.SuAddresses
                }).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Get Base Quotation Details
        /// </summary>
        /// <returns></returns>
        public IQueryable<QuotationExportRepo> GetAllQuotations()
        {
            return _context.QuQuotationInsps
                .Select(x => new QuotationExportRepo
                {
                    QuotationNo = x.IdQuotation,
                    BookingNo = x.IdBooking,
                    CustomerId = x.IdQuotationNavigation.CustomerId,
                    SupplierId = x.IdQuotationNavigation.SupplierId,
                    FactoryId = x.IdQuotationNavigation.FactoryId,
                    Customer = x.IdQuotationNavigation.Customer.CustomerName,
                    Supplier = x.IdQuotationNavigation.Supplier.SupplierName,
                    Factory = x.IdQuotationNavigation.Factory.SupplierName,
                    QuotationDate = x.IdQuotationNavigation.CreatedDate,
                    StatusId = x.IdQuotationNavigation.IdStatus,
                    OfficeId = x.IdQuotationNavigation.OfficeId,
                    Office = x.IdQuotationNavigation.Office.LocationName,
                    Status = x.IdQuotationNavigation.IdStatusNavigation.Label,
                    EstimatedManDay = x.IdQuotationNavigation.EstimatedManday,
                    InspectionCost = x.InspFees.GetValueOrDefault(),
                    TravelCost = (x.TravelAir.GetValueOrDefault() + x.TravelLand.GetValueOrDefault() + x.TravelHotel.GetValueOrDefault()),
                    Discount = x.IdQuotationNavigation.Discount,
                    TotalFees = x.TotalCost.GetValueOrDefault(),
                    APIRemark = x.IdQuotationNavigation.ApiRemark,
                    CustomerRemark = x.IdQuotationNavigation.CustomerRemark,
                    BillPaidById = x.IdQuotationNavigation.BillingPaidById,
                    BillPaidBy = x.IdQuotationNavigation.BillingPaidBy.Label,
                    FactoryAddress = x.IdQuotationNavigation.FactoryAddress,
                    Currency = x.IdQuotationNavigation.Currency.CurrencyName,
                    SupplierLegalName = x.IdQuotationNavigation.SupplierLegalName,
                    CustomerLegalName = x.IdQuotationNavigation.CustomerLegalName,
                    FactoryLegalName = x.IdQuotationNavigation.LegalFactoryName,
                    BillingEntity = x.IdQuotationNavigation.BillingEntityNavigation.Name,
                    OtherCost = x.IdQuotationNavigation.OtherCosts,
                    PaymentTerm = x.IdQuotationNavigation.PaymentTermsNavigation.Name,
                    ValidatedOn = x.IdQuotationNavigation.ValidatedOn,
                    ValidatedByName = x.IdQuotationNavigation.ValidatedByNavigation.FullName,
                    ValidatedByUserType = x.IdQuotationNavigation.ValidatedByNavigation.UserTypeId,

                });
        }
        /// <summary>
        /// Get Quotation Booking Mapped Details
        /// </summary>
        /// <param name="quotationIds"></param>
        /// <returns></returns>
        public async Task<List<QuotationBookingMapRepo>> GetQuotationBookingsByQuotationIds(IEnumerable<int> quotationIds)
        {
            return await _context.QuQuotationInsps.Where(x => quotationIds.Contains(x.IdQuotation)).
                            Select(x => new QuotationBookingMapRepo
                            {
                                QuotationNo = x.IdQuotation,
                                BookingNo = x.IdBooking
                            }).ToListAsync();
        }


        public async Task<List<QuotationBookingMapRepo>> GetQuotationBookingsByServiceDate(DateTime startDate, DateTime endDate, int? bookingNo)
        {
            var quotationBookingMapRepoList = new List<QuotationBookingMapRepo>();
            if (bookingNo != null)
            {
                quotationBookingMapRepoList = await _context.QuQuotationInsps.Where(x => !((x.IdBookingNavigation.ServiceDateFrom > endDate)
                                                      || (x.IdBookingNavigation.ServiceDateTo < startDate)) && x.IdBooking == bookingNo).
                            Select(x => new QuotationBookingMapRepo
                            {
                                QuotationNo = x.IdQuotation,
                                BookingNo = x.IdBooking
                            }).ToListAsync();
            }
            else
            {
                quotationBookingMapRepoList = await _context.QuQuotationInsps.Where(x => !((x.IdBookingNavigation.ServiceDateFrom > endDate)
                                                      || (x.IdBookingNavigation.ServiceDateTo < startDate))).
                            Select(x => new QuotationBookingMapRepo
                            {
                                QuotationNo = x.IdQuotation,
                                BookingNo = x.IdBooking
                            }).ToListAsync();
            }
            return quotationBookingMapRepoList;
        }

        public async Task<List<QuotationBookingMapRepo>> GetQuotationBookingsByBookingNo(int bookingNo)
        {
            return await _context.QuQuotationInsps.Where(x => x.IdBooking == bookingNo).
                            Select(x => new QuotationBookingMapRepo
                            {
                                QuotationNo = x.IdQuotation,
                                BookingNo = x.IdBooking
                            }).ToListAsync();
        }

        /// <summary>
        /// Get Mapped Booking Product details
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<QuotationBookingProductRepo>> GetQuotationBookingProductsByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.Active.HasValue && x.Active.Value).
                            Select(x => new QuotationBookingProductRepo
                            {
                                ProductName = x.Product.ProductId,
                                BookingNo = x.InspectionId,
                                FactoryReference = x.Product.FactoryReference,
                                FBReportStartDate = x.FbReport.ServiceFromDate,
                                FBReportEndDate = x.FbReport.ServiceToDate,
                                ProductDesc = x.Product.ProductDescription,
                                ReportResult = x.FbReport.OverAllResult,
                                BookingQty = x.TotalBookingQuantity,
                                SampleSize = x.AqlQuantity.GetValueOrDefault(),
                                CombineAQL = x.CombineAqlQuantity,
                                ProductRefId = x.Id,
                                TimePrepatation = x.Product.TimePreparation,
                                SampleSize8h = x.Product.SampleSize8h,
                                ProdSubCategory3Id = x.Product.ProductCategorySub3,
                                CombineProductId = x.CombineProductId,
                                ProdUnit = x.Unit
                            }).ToListAsync();
        }

        /// <summary>
        /// Get Booking Products Po Data By ProductRefIds by query id
        /// </summary>
        /// <param name="productRefIds"></param>
        /// <returns></returns>
        public async Task<List<BookingProductPoRepo>> GetBookingProductsPoListByProductRefIds(IQueryable<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.Active.HasValue && x.Active.Value).
                            Select(x => new BookingProductPoRepo
                            {
                                PoName = x.Po.Pono,
                                ProductRefId = x.ProductRefId,
                                BookingId = x.InspectionId
                            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Mapped Booked Details
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<QuotationBookingRepo>> GetQuotationBookingDetailsByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id)).
                           Select(x => new QuotationBookingRepo
                           {
                               BookingNo = x.Id,
                               ServiceDateFrom = x.ServiceDateFrom,
                               ServiceDateTo = x.ServiceDateTo,
                               InspectionStatus = x.Status.Status,
                               CustomerBookingNo = x.CustomerBookingNo
                           }).ToListAsync();

        }
        /// <summary>
        /// Get quotation customer contacts by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<QuotationBookingContactRepo>> GetQuotationCustomerContactsById(List<int> lstid)
        {
            return await _context.QuQuotationCustomerContacts.
                Where(x => lstid.Contains(x.IdQuotation) && x.IdContactNavigation.Active.HasValue && x.IdContactNavigation.Active.Value && (x.Email || x.InvoiceEmail)).
                Select(x => new QuotationBookingContactRepo()
                {
                    QuotationId = x.IdQuotation,
                    ContactName = x.IdContactNavigation.ContactName,
                    Email = x.IdContactNavigation.Email,
                    InvoiceEmail = x.InvoiceEmail,
                    IsEmail = x.Email
                })
                .AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get quotation customer contacts by Query id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Tuple<int, string>>> GetQuotationCustomerContactsByIds(IQueryable<int> lstid)
        {
            return await _context.QuQuotationCustomerContacts.
                Where(x => lstid.Contains(x.IdQuotation) && x.IdContactNavigation.Active.HasValue && x.IdContactNavigation.Active.Value).
                Select(x => new Tuple<int, string>(x.IdQuotation, x.IdContactNavigation.ContactName)).AsNoTracking().
                ToListAsync();
        }


        /// <summary>
        /// Get quotation supplier contacts by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<QuotationBookingContactRepo>> GetQuotationSupplierContactsById(List<int> lstid)
        {
            return await _context.QuQuotationSupplierContacts.
                 Where(x => lstid.Contains(x.IdQuotation) && x.IdContactNavigation.Active.HasValue && x.IdContactNavigation.Active.Value && (x.Email || x.InvoiceEmail))
                 .Select(x => new QuotationBookingContactRepo()
                 {
                     QuotationId = x.IdQuotation,
                     ContactName = x.IdContactNavigation.ContactName,
                     Email = x.IdContactNavigation.Mail,
                     InvoiceEmail = x.InvoiceEmail,
                     IsEmail = x.Email
                 })
                 .AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get quotation supplier contacts by query id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Tuple<int, string>>> GetQuotationSupplierContactsByIds(IQueryable<int> lstid)
        {
            return await _context.QuQuotationSupplierContacts.
                 Where(x => lstid.Contains(x.IdQuotation) && x.IdContactNavigation.Active.HasValue && x.IdContactNavigation.Active.Value).
                Select(x => new Tuple<int, string>(x.IdQuotation, x.IdContactNavigation.ContactName)).AsNoTracking().
                ToListAsync();
        }
        /// <summary>
        /// Get quotation factory contacts by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<QuotationBookingContactRepo>> GetQuotationFactoryContactsById(List<int> lstid)
        {
            return await _context.QuQuotationFactoryContacts.
                Where(x => lstid.Contains(x.IdQuotation) && x.IdContactNavigation.Active.HasValue && x.IdContactNavigation.Active.Value && (x.Email || x.InvoiceEmail)).
                 Select(x => new QuotationBookingContactRepo()
                 {
                     QuotationId = x.IdQuotation,
                     ContactName = x.IdContactNavigation.ContactName,
                     Email = x.IdContactNavigation.Mail,
                     InvoiceEmail = x.InvoiceEmail,
                     IsEmail = x.Email
                 })
                 .AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get quotation factory contacts by query id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Tuple<int, string>>> GetQuotationFactoryContactsByIds(IQueryable<int> lstid)
        {
            return await _context.QuQuotationFactoryContacts.
                Where(x => lstid.Contains(x.IdQuotation) && x.IdContactNavigation.Active.HasValue && x.IdContactNavigation.Active.Value).
                Select(x => new Tuple<int, string>(x.IdQuotation, x.IdContactNavigation.ContactName)).AsNoTracking().
                ToListAsync();
        }

        //Fetch All the Billing entities
        public async Task<IEnumerable<CommonDataSource>> GetBillingEntities()
        {
            return await _context.RefBillingEntities.Where(x => x.Active)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        //Get Qu quotation Insp values by quotation Id
        public async Task<List<QuotationInvoiceItem>> GetquotationInsp(List<int> quotationIds)
        {
            return await _context.QuQuotationInsps.Where(x => quotationIds.Contains(x.IdQuotation))
                .Select(x => new QuotationInvoiceItem
                {
                    QuotationId = x.IdQuotation,
                    QuoInvoiceDate = x.InvoiceDate,
                    QuoInvoiceNo = x.InvoiceNo,
                    QuoInvoiceREmarks = x.InvoiceRemarks,
                    Manday = x.NoOfManDay,
                    BookingId = x.IdBooking,
                    TravelDistance = x.TravelDistance,
                    TravelTime = x.TravelTime
                })
                .ToListAsync();
        }

        //Get Qu quotation Insp values by quotation Id
        public async Task<List<QuotationInvoiceItem>> GetquotationInvoiceList(IQueryable<int> quotationIds)
        {
            return await _context.QuQuotationInsps.Where(x => quotationIds.Contains(x.IdQuotation))
                .Select(x => new QuotationInvoiceItem
                {
                    QuotationId = x.IdQuotation,
                    QuoInvoiceDate = x.InvoiceDate,
                    QuoInvoiceNo = x.InvoiceNo,
                    QuoInvoiceREmarks = x.InvoiceRemarks,
                    Manday = x.NoOfManDay,
                    BookingId = x.IdBooking,
                    TravelDistance = x.TravelDistance,
                    TravelTime = x.TravelTime,
                    CalculatedWorkingHours = x.CalculatedWorkingHrs,
                    CalculatedWorkingManday = x.CalculatedWorkingManDay,
                    Quantity = x.Quantity,
                    BilledQtyType = x.BilledQtyTypeNavigation.Name
                }).AsNoTracking()
                .ToListAsync();
        }

        //Get Qu quotation Audit values by quotation Id
        public async Task<List<QuotationInvoiceItem>> GetquotationAudit(List<int> quotationIds)
        {
            return await _context.QuQuotationAudits.Where(x => quotationIds.Contains(x.IdQuotation))
                .Select(x => new QuotationInvoiceItem
                {
                    QuotationId = x.IdQuotation,
                    QuoInvoiceDate = x.InvoiceDate,
                    QuoInvoiceNo = x.InvoiceNo,
                    QuoInvoiceREmarks = x.InvoiceRemarks,
                    Manday = x.NoOfManDay,
                    TravelDistance = x.TravelDistance,
                    TravelTime = x.TravelTime
                })
                .ToListAsync();
        }

        //Get Qu quotation Insp values by quotation Id
        public async Task<QuQuotationInsp> GetquotationInsp(int quotationId)
        {
            return await _context.QuQuotationInsps.Where(x => x.IdQuotation == quotationId).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Get booking status list by Quotation id
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetquotationBookingStatus(int quotationId)
        {
            return await _context.QuQuotationInsps.Where(x => x.IdQuotation == quotationId).
                          Select(x => x.IdBookingNavigation.StatusId)
                         .ToListAsync();
        }

        //Get Qu quotation Audit values by quotation Id
        public async Task<QuQuotationAudit> GetquotationAudit(int quotationId)
        {
            return await _context.QuQuotationAudits.Where(x => x.IdQuotation == quotationId).FirstOrDefaultAsync();
        }

        //fetch the quotation data to save in the qu_tran_status_logs
        public async Task<List<QuTranStatusLog>> GetQuotationDataForStatusLogs(int quotationId)
        {
            var res = _context.QuQuotations.Where(x => x.Id == quotationId).FirstOrDefaultAsync();
            List<QuTranStatusLog> result = new List<QuTranStatusLog>();

            if (res.Result.ServiceId == (int)Service.InspectionId)
            {
                result = await _context.QuQuotationInsps.Where(x => x.IdQuotation == quotationId)
                    .Select(x => new QuTranStatusLog
                    {
                        BookingId = x.IdBooking,
                        AuditId = 0,
                        StatusId = x.IdQuotationNavigation.IdStatus,
                        QuotationId = x.IdQuotation
                    }).ToListAsync();
            }

            else
            {
                result = await _context.QuQuotationAudits.Where(x => x.IdQuotation == quotationId)
                    .Select(x => new QuTranStatusLog
                    {
                        BookingId = 0,
                        AuditId = x.IdBooking,
                        StatusId = x.IdQuotationNavigation.IdStatus,
                        QuotationId = x.IdQuotation
                    }).ToListAsync();
            }

            return result;
        }

        //get the man day for a list of dates
        public IQueryable<Manday> GetQuotationMandayByDate(IEnumerable<DateTime> serviceDates)
        {
            return _context.QuQuotationInspMandays.Where(x => serviceDates.Contains(x.ServiceDate) && x.Active.HasValue && x.Active.Value &&
                          x.Quotation.IdStatus != (int)QuotationStatus.Canceled && x.Booking.StatusId != (int)BookingStatus.Cancel && x.Booking.StatusId != (int)BookingStatus.Hold)
                .Select(x => new Manday
                {
                    BookingId = x.BookingId,
                    ServiceDate = x.ServiceDate,
                    NoOfManday = x.NoOfManday.GetValueOrDefault(),
                    OfficeId = x.Quotation.OfficeId
                }).AsNoTracking();
        }

        //Fetch the Quotation Man Day for the bookings
        public async Task<QuotManday> GetQuotationTravelManDay(int bookingId)
        {
            return await _context.QuQuotationInsps//.Include(x => x.Quotation)
                .Where(x => x.IdBooking == bookingId && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled) //&& x.Active.HasValue && x.Active.Value)
                .Select(x => new QuotManday
                {
                    BookingId = x.IdBooking,
                    ManDay = x.NoOfManDay,
                    TravelManDay = x.NoOfTravelManDay
                }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<HrHoliday> GetHolidayInfo(DateTime fromDate, DateTime toDate, int locationId)
        {
            return await _context.HrHolidays.Where(x => x.LocationId == locationId
                            && !((x.StartDate > toDate) || (x.EndDate < fromDate))).FirstOrDefaultAsync();
        }

        public async Task<CuPrDetail> GetHolidayTypes(int priceId)
        {
            return await _context.CuPrDetails.Where(x => x.Id == priceId && x.Active.HasValue && x.Active.Value).
                            Include(x => x.CuPrHolidayTypes).FirstOrDefaultAsync();
        }

        // Get Quotation Bookings by booking service start date and service end date and cusbookingno
        public async Task<List<QuotationBookingMapRepo>> GetQuotationBookingsByCusBookingNo(string cusBookingNo)
        {
            return await _context.QuQuotationInsps.Where(x => x.IdBookingNavigation.CustomerBookingNo == cusBookingNo).
                            Select(x => new QuotationBookingMapRepo
                            {
                                QuotationNo = x.IdQuotation,
                                BookingNo = x.IdBooking
                            }).ToListAsync();
        }

        // Get Quotation Bookings by cus booking no
        public async Task<List<QuotationBookingMapRepo>> GetQuotationBookingsByServiceDateCusBookingNo(DateTime startDate, DateTime endDate, string cusBookingNo)
        {
            return await _context.QuQuotationInsps.Where(x => !((x.IdBookingNavigation.ServiceDateFrom > endDate)
                                                      || (x.IdBookingNavigation.ServiceDateTo < startDate))
                                                      && x.IdBookingNavigation.CustomerBookingNo == cusBookingNo).
                            Select(x => new QuotationBookingMapRepo
                            {
                                QuotationNo = x.IdQuotation,
                                BookingNo = x.IdBooking
                            }).ToListAsync();
        }

        public IQueryable<MobilePendingQuotation> GetMobileQuotationDetails()
        {
            return _context.QuQuotationInsps.Where(x => (x.IdBookingNavigation.StatusId != (int)BookingStatus.Cancel) && (x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.SentToClient || x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated))
                .Select(x => new MobilePendingQuotation
                {
                    QuotationId = x.IdQuotation,
                    CustomerId = x.IdBookingNavigation.CustomerId,
                    SupplierId = x.IdBookingNavigation.SupplierId,
                    FactoryId = x.IdBookingNavigation.FactoryId,
                    SupplierName = x.IdBookingNavigation.Supplier.SupplierName,
                    FactoryName = x.IdBookingNavigation.Factory.SupplierName,
                    QuotationStatusId = x.IdQuotationNavigation.IdStatus,
                    ServiceDateFrom = x.IdBookingNavigation.ServiceDateFrom,
                    ServiceDateTo = x.IdBookingNavigation.ServiceDateTo,
                    BookingId = x.IdBooking,
                    ManDay = x.NoOfManDay.GetValueOrDefault(),
                    UnitPrice = x.UnitPrice.GetValueOrDefault(),
                    ServiceFee = x.InspFees.GetValueOrDefault(),
                    TravelAir = x.TravelAir.GetValueOrDefault(),
                    TravelLand = x.TravelLand.GetValueOrDefault(),
                    Hotel = x.TravelHotel.GetValueOrDefault(),
                    TotalPrice = x.TotalCost.GetValueOrDefault(),
                    OtherCost = x.IdQuotationNavigation.OtherCosts.GetValueOrDefault(),
                    Currency = x.IdQuotationNavigation.Currency.CurrencyName,
                    BookingStatusId = x.IdBookingNavigation.StatusId,
                    BilledTo = x.IdQuotationNavigation.BillingPaidById,
                    Discount = x.IdQuotationNavigation.Discount
                });
        }

        public async Task<List<InvoiceInfo>> InvoiceInfoBybookingdId(IEnumerable<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled && x.InspectionId.HasValue && bookingIds.Contains(x.InspectionId.Value)).
                 Select(x => new InvoiceInfo
                 {
                     Inspectiono = x.InspectionId,
                     InvoiceDate = x.InvoiceDate,
                     InvoiceNo = x.InvoiceNo,
                     InvoiceREmarks = x.Remarks
                 }).ToListAsync();

        }
        //Inspection invoice details by query Id
        public async Task<List<InvoiceInfo>> InvoiceDetailsBybookingdId(IQueryable<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled && x.InspectionId.HasValue && bookingIds.Contains(x.InspectionId.Value)).
                 Select(x => new InvoiceInfo
                 {
                     Inspectiono = x.InspectionId,
                     InvoiceDate = x.InvoiceDate,
                     InvoiceNo = x.InvoiceNo,
                     InvoiceREmarks = x.Remarks
                 }).AsNoTracking().ToListAsync();

        }
        public async Task<List<InvoiceInfo>> InvoiceInfoByAuditId(IEnumerable<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled && x.AuditId.HasValue && bookingIds.Contains(x.AuditId.Value)).
                 Select(x => new InvoiceInfo
                 {
                     AuditNo = x.AuditId,
                     InvoiceDate = x.InvoiceDate,
                     InvoiceNo = x.InvoiceNo,
                     InvoiceREmarks = x.Remarks
                 }).ToListAsync();

        }
        public async Task<List<InvoiceInfo>> InvoiceDetailsByAuditId(IQueryable<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x => x.InvoiceStatus.HasValue && x.InvoiceStatus.Value != (int)InvoiceStatus.Cancelled && x.AuditId.HasValue && bookingIds.Contains(x.AuditId.Value)).
                 Select(x => new InvoiceInfo
                 {
                     AuditNo = x.AuditId,
                     InvoiceDate = x.InvoiceDate,
                     InvoiceNo = x.InvoiceNo,
                     InvoiceREmarks = x.Remarks
                 }).ToListAsync();

        }

        /// <summary>
        /// fetch the quotation details by booking Id 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<QuQuotation> GetBookingQuotationDetails(int bookingId)
        {
            return await _context.QuQuotationInsps.Where(x => x.IdBooking == bookingId && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).Select(x => x.IdQuotationNavigation).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get quotation other cost
        /// </summary>
        /// <returns></returns>
        public async Task<List<QuotationData>> GetQuotationOtherCost(IEnumerable<int> bookingIdList)
        {
            return await _context.QuQuotationInsps.Where(x => bookingIdList.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                .Select(x => new QuotationData
                {
                    BookingId = x.IdBooking,
                    OtherCost = x.IdQuotationNavigation.OtherCosts,
                    QuotationId = x.IdQuotation
                }).OrderBy(x => x.QuotationId).ThenBy(x => x.BookingId).ToListAsync();
        }

        /// <summary>
        /// Get skipQuotationSentToClient Checkpoint
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<QuotationCheckpointData> GetCheckpointByCustomerId(int customerId)
        {
            return await _context.CuCheckPoints
                .Where(x => x.Active && x.CustomerId == customerId && x.ServiceId == (int)Service.InspectionId &&
                       x.CheckpointTypeId == (int)CheckPointTypeEnum.SkipQuotationSentToClient)
                       .Select(x => new QuotationCheckpointData
                       {
                           CheckpointId = x.Id
                       }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get bookingdata by booking Ids
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<BookingQuotationData>> GetBookingDetails(List<int> bookingIdList)
        {
            return await _context.InspTransactions.Where(x => bookingIdList.Contains(x.Id))
                .Select(x => new BookingQuotationData
                {
                    BookingId = x.Id
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get quotation data by id
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        public async Task<QuQuotation> GetQuotationData(int quotationId)
        {
            return await _context.QuQuotations.
                Include(x => x.QuQuotationInsps).
                Where(x => x.Id == quotationId && x.IdStatus != (int)QuotationStatus.Canceled).
                FirstOrDefaultAsync();
        }

        /// <summary>
        /// get number 
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        public async Task<int> GetBookingsByQuotation(int bookingId)
        {
            int quotationId = _context.QuQuotationInsps
                .Where(x => x.IdBooking == bookingId && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                .Select(x => x.IdQuotation).FirstOrDefault();

            return await _context.QuQuotationInsps.Where(x => x.IdQuotation == quotationId)
                .CountAsync();
        }

        public async Task<List<int>> GetCheckpointBrandByCheckpointId(int checkpointId)
        {
            return await _context.CuCheckPointsBrands
                .Where(x => x.Active && x.CheckpointId == checkpointId)
                       .Select(x => x.BrandId).ToListAsync();
        }

        public async Task<List<int>> GetCheckpointDepartmentByCheckpointId(int checkpointId)
        {
            return await _context.CuCheckPointsDepartments
                .Where(x => x.Active && x.CheckpointId == checkpointId)
                       .Select(x => x.DeptId).ToListAsync();
        }

        public async Task<List<int>> GetCheckpointServiceTypeByCheckpointId(int checkpointId)
        {
            return await _context.CuCheckPointsServiceTypes
                .Where(x => x.Active && x.CheckpointId == checkpointId)
                       .Select(x => x.ServiceType.ServiceTypeId).ToListAsync();
        }

        public async Task<string> GetquotationPdfPath(int quotationId)
        {
            return await _context.QuQuotationPdfVersions
                 .Where(x => x.QuotationId == quotationId)
                 .OrderByDescending(x => x.UploadDate)
                 .Select(x => x.FileUrl)
                 .FirstOrDefaultAsync();
        }

        public async Task<QuQuotationPdfVersion> GetquotationPdfFile(int quotationId)
        {
            return await _context.QuQuotationPdfVersions
                 .Where(x => x.QuotationId == quotationId)
                 .OrderByDescending(x => x.UploadDate)
                 .FirstOrDefaultAsync();
        }

        public async Task<List<QuQuotationPdfVersion>> GetquotationPdfHistoryList(int quotationId)
        {
            return await _context.QuQuotationPdfVersions
                 .Where(x => x.SendToClient.Value && x.QuotationId == quotationId)
                 .OrderBy(x => x.UploadDate)
                 .ToListAsync();
        }

        public async Task<double> GetCustomerRequirementIndex(int bookingId)
        {
            return await _context.InspTransactions.Where(x => x.Id == bookingId)
               .Join(_context.CuServiceTypes, booking => booking.CustomerId, serviceType => serviceType.CustomerId,
                (booking, serviceType) => new { InspTransaction = booking, CuServiceType = serviceType })
               .Where(x => x.CuServiceType.Active && x.CuServiceType.ServiceTypeId == x.InspTransaction.InspTranServiceTypes.Where(y => y.Active)
               .Select(y => y.ServiceTypeId).FirstOrDefault())
               .Select(x => x.CuServiceType.CustomerRequirementIndex.GetValueOrDefault()).FirstOrDefaultAsync();

            //return await _context.InspTranServiceTypes.Where(x => x.Active && x.InspectionId == bookingId)
            //    .SelectMany(x => x.ServiceType.CuServiceTypes)
            //    .Where(x => x.Active)
            //    .Select(x => x.CustomerRequirementIndex.GetValueOrDefault()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// pass rule id and get travel include column value
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public async Task<bool?> GetPriceCardTravel(int ruleId)
        {
            return await _context.CuPrDetails.Where(x => x.Active.Value && x.Id == ruleId)
                .Select(x => x.TravelIncluded).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Get quotation internal contacts by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<QuotationBookingContactRepo>> GetQuotationInternalContactsById(List<int> lstid)
        {
            return await _context.QuQuotationContacts.
                 Where(x => lstid.Contains(x.IdQuotation) && x.IdContactNavigation.Active.HasValue && x.IdContactNavigation.Active.Value && x.Email).
                Select(x => new QuotationBookingContactRepo()
                {
                    QuotationId = x.IdQuotation,
                    ContactName = x.IdContactNavigation.PersonName,
                    Email = x.IdContactNavigation.CompanyEmail
                })
                .AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// get quotation insp
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<QuQuotationInsp> GetQuotationInsp(int bookingId)
        {
            return await _context.QuQuotationInsps.Where(x => x.IdBooking == bookingId).AsNoTracking().FirstOrDefaultAsync();
        }

    }
}
