using Contracts.Repositories;
using DTO.Common;
using DTO.Inspection;
using DTO.InspectionCertificate;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace DAL.Repositories
{
    public class InspectionCertificateRepository : Repository, IInspectionCertificateRepository
    {

        public InspectionCertificateRepository(API_DBContext context) : base(context)
        {

        }

        /// <summary>
        /// get inspection data
        /// </summary>
        /// <param name="bookingRequest"></param>
        /// <returns></returns>

        public IQueryable<InspTransaction> GetInspectionICData(ICBookingSearchRequest bookingRequest)
        {
            return _context.InspTransactions.Where(x => InspectedStatusList.Contains(x.StatusId))
            .OrderBy(x => x.ServiceDateFrom).ThenBy(x => x.ServiceDateTo);
        }

        public async Task<List<CustomerDecisionBookId>> GetInspCusDecision(ICBookingSearchRequest bookingRequest)
        {
            return await (from trans in _context.InspTransactions
                          join prodtran in _context.InspProductTransactions on trans.Id equals prodtran.InspectionId
                          join fb in _context.FbReportDetails on prodtran.FbReportId equals fb.Id
                          join cusdec in _context.InspRepCusDecisions on fb.Id equals cusdec.ReportId
                          where (((bookingRequest.ServiceFromDate == null && bookingRequest.ServiceToDate == null) ||
                          !((trans.ServiceDateFrom > bookingRequest.ServiceToDate.ToDateTime()) || (trans.ServiceDateTo < bookingRequest.ServiceFromDate.ToDateTime())))
                           && (trans.Id == bookingRequest.BookingId || bookingRequest.BookingId == null)
                           && (trans.CustomerId == bookingRequest.CustomerId || bookingRequest.CustomerId == null)
                           && cusdec.CustomerResultId ==
                          (int)InspCustomerDecisionEnum.Pass && cusdec.Active.HasValue && cusdec.Active.Value && fb.Active.HasValue && fb.Active.Value
                          && prodtran.Active.HasValue && prodtran.Active.HasValue)
                          select new CustomerDecisionBookId { BookingId = trans.Id, CustomerId = trans.CustomerId }).Distinct().ToListAsync();

        }

        public async Task<List<ICBookingSearchProductResponse>> GetBookingProductList(List<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.Active.HasValue && x.Active.Value
             || x.InspIcTranProducts.Any(z => z.Active.HasValue && z.Active.Value && bookingIds.Contains(z.BookingProduct.InspectionId)))
                .Select(x => new ICBookingSearchProductResponse
                {
                    PONo = x.Po.Pono,
                    POId = x.Po.Id,
                    ProductCode = x.ProductRef.Product.ProductId,
                    ProductDescription = x.ProductRef.Product.ProductDescription,
                    DestinationCountry = x.DestinationCountry.CountryName,
                    Unit = x.ProductRef.UnitNavigation.Name,
                    FBReportId = x.ProductRef.FbReportId,
                    BookingId = x.InspectionId,
                    InspPOTransactionId = x.Id,
                    BusinessLine = x.Inspection.BusinessLine
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ICBookingSearchProductResponse>> GetBookingProductListSoftline(List<int> bookingIds)
        {
            return await _context.InspPurchaseOrderColorTransactions.Where(x => bookingIds.Contains(x.PoTrans.InspectionId) && x.Active.HasValue && x.Active.Value)
                .Select(x => new ICBookingSearchProductResponse
                {
                    PONo = x.PoTrans.Po.Pono,
                    POId = x.PoTrans.Po.Id,
                    ProductCode = x.ProductRef.Product.ProductId,
                    ProductDescription = x.ProductRef.Product.ProductDescription,
                    DestinationCountry = x.PoTrans.DestinationCountry.CountryName,
                    Unit = x.ProductRef.UnitNavigation.Name,
                    FBReportId = x.ProductRef.FbReportId,
                    BookingId = x.PoTrans.InspectionId,
                    InspPOTransactionId = x.PoTrans.Id,
                    Color = x.ColorName,
                    ColorCode = x.ColorCode,
                    PoColorId = x.Id,
                    BusinessLine = x.PoTrans.Inspection.BusinessLine
                }).AsNoTracking().ToListAsync();
        }
        public async Task<List<ICBookingProductFB>> GetProductFBList(List<int> fbReportIds)
        {
            try
            {
                return await _context.FbReportQuantityDetails.Where(x => fbReportIds.Contains(x.FbReportDetail.Id) && x.Active.HasValue && x.Active.Value)
              .Select(x => new ICBookingProductFB
              {
                  FBReportId = x.FbReportDetail.Id,
                  FBPresentedQty = x.PresentedQuantity.HasValue ? x.PresentedQuantity.Value : 0,
                  FBStatus = x.FbReportDetail.FbReportStatus,
                  inspoTransid = x.InspPoTransactionId,
                  InsPOColorTransId = x.InspColorTransactionId
              }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<InspIcTranProduct>> GetICProducts(List<int> inspPOTransactionId)
        {
            return await _context.InspIcTranProducts.Where(x => x.Active.HasValue && x.Active.Value && x.BookingProductId.HasValue &&
                                                            inspPOTransactionId.Contains(x.BookingProductId.Value)).ToListAsync();
        }
        public async Task<List<InspIcTranProduct>> GetICProductsByICID(int id)
        {
            return await _context.InspIcTranProducts.Where(x => x.Active.HasValue && x.Active.Value &&
                                                            x.Icid == id).ToListAsync();
        }
        public async Task<int> SaveIC(InspIcTransaction entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        public async Task<InspectionCertificateRequest> GetICDetails(int id)
        {
            return await _context.InspIcTransactions.Where(x => x.Id == id)
                .Select(x => new InspectionCertificateRequest
                {
                    CustomerId = x.CustomerId.HasValue ? x.CustomerId.Value : 0,
                    SupplierId = x.SupplierId.HasValue ? x.SupplierId.Value : 0,
                    SupplierAddress = x.SupplierAddress,
                    //ICTitle = x.Ictitle,
                    ICStatus = x.Icstatus,
                    ICStatusName = x.IcstatusNavigation.StatusName,
                    ICTitleId = x.IctitleId,
                    BeneficiaryName = x.BeneficiaryName,
                    ApprovalDate = Static_Data_Common.GetCustomDate(x.ApprovalDate),
                    ICNo = x.Icno,
                    Id = x.Id,
                    Comment = x.Comment,
                    BuyerName = x.BuyerName
                }).FirstOrDefaultAsync();
        }
        public async Task<List<InspectionCertificateBookingRequest>> GetICProductDetails(int id)
        {
            return await _context.InspIcTranProducts
                .Where(x => x.Icid == id) //&& x.Active.HasValue && x.Active.Value
                .Select(x => new InspectionCertificateBookingRequest
                {
                    PONo = x.BookingProduct.Po.Pono,
                    ProductCode = x.BookingProduct.ProductRef.Product.ProductId,
                    ProductDescription = x.BookingProduct.ProductRef.Product.ProductDescription,
                    DestinationCountry = x.BookingProduct.DestinationCountry.CountryName,
                    Unit = x.BookingProduct.ProductRef.UnitNavigation.Name,
                    BookingNumber = x.BookingProduct.InspectionId,
                    BookingProductId = x.BookingProductId.HasValue ? x.BookingProductId.Value : 0,
                    ShipmentQty = x.ShipmentQty.HasValue ? x.ShipmentQty.Value : 0,
                    Color = x.PoColor.ColorName,
                    ColorCode = x.PoColor.ColorCode,
                    PoColorId = x.PoColorId,
                    BusinessLine = x.BookingProduct.Inspection.BusinessLine
                }).ToListAsync();
        }
        public async Task<InspectionCertificatePDF> GetICPDFDetails(int id)
        {
            return await _context.InspIcTransactions.Where(x => x.Id == id)
                .Select(x => new InspectionCertificatePDF
                {
                    CustomerName = x.Customer.CustomerName,
                    SupplierAddress = x.SupplierAddress,
                    ICTitle = x.Ictitle.Name,
                    ICTitleId = x.IctitleId,
                    BeneficiaryName = x.BeneficiaryName,
                    ApprovalDate = x.ApprovalDate.HasValue ? x.ApprovalDate.Value.ToLongDateString() : null,
                    ICNo = x.Icno,
                    Id = x.Id,
                    Comment = x.Comment,
                    BuyerName = x.BuyerName,
                    BusinessLine = x.InspIcTranProducts.FirstOrDefault().BookingProduct.Inspection.BusinessLine
                })
                .FirstOrDefaultAsync();
        }
        public async Task<List<InspectionCertificateProductPDF>> GetICPDFProductDetails(int id)
        {
            return await _context.InspIcTranProducts
                .Where(x => x.Icid == id)
                .Select(x => new InspectionCertificateProductPDF
                {
                    PONo = x.BookingProduct.Po.Pono,
                    ProductCode = x.BookingProduct.ProductRef.Product.ProductId,
                    ProductDescription = x.BookingProduct.ProductRef.Product.ProductDescription,
                    DestinationCountry = x.BookingProduct.DestinationCountry.CountryName,
                    Unit = x.BookingProduct.ProductRef.UnitNavigation.Name,
                    BookingNumber = x.BookingProduct.InspectionId, // 
                    AQL = x.BookingProduct.ProductRef.AqlNavigation.Value,
                    Minor = x.BookingProduct.ProductRef.MinorNavigation.Value,
                    Major = x.BookingProduct.ProductRef.MajorNavigation.Value,
                    Critical = x.BookingProduct.ProductRef.CriticalNavigation.Value,
                    Quantity = x.ShipmentQty.Value,
                    serviceDateFrom = x.BookingProduct.Inspection.ServiceDateFrom,
                    serviceDateTo = x.BookingProduct.Inspection.ServiceDateTo,
                    Color = x.PoColor.ColorName,
                    ColorCode = x.PoColor.ColorCode
                }).OrderByDescending(x => x.BookingNumber).ThenBy(x => x.ProductCode).ToListAsync();
        }
        public async Task<InspIcTransaction> GetICDetailsProductDetails(int id)
        {
            return await _context.InspIcTransactions
                            .Include(x => x.InspIcTranProducts)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            // x.InspIcTranProducts.Any(y => y.Active.HasValue && y.Active.Value) && && x.Icstatus != (int)InspectionCertificateStatus.Cancel
        }



        public IQueryable<InspectionCertificateData> GetInspectionCertificateDetails()
        {
            return _context.InspIcTransactions
                               .Select(x =>
                                    new InspectionCertificateData
                                    {
                                        IcId = x.Id,
                                        IcNo = x.Icno,
                                        CreationDate = x.CreatedOn,
                                        InspIcTranProducts = x.InspIcTranProducts,
                                        StatusId = x.Icstatus,
                                        StatusName = x.IcstatusNavigation.StatusName
                                    }).AsNoTracking();
        }

        public IQueryable<ICItemRepo> GetICInspectionTransactions()
        {
            return from inspictrans in _context.InspIcTransactions
                   join icstaus in _context.InspIcStatuses on inspictrans.Icstatus equals icstaus.Id
                   join inspicprotrans in _context.InspIcTranProducts on inspictrans.Id equals inspicprotrans.Icid
                   join insppo in _context.InspPurchaseOrderTransactions on inspicprotrans.BookingProductId equals insppo.Id
                   join insptran in _context.InspTransactions on insppo.InspectionId equals insptran.Id
                   join cus in _context.CuCustomers on insptran.CustomerId equals cus.Id
                   join sup in _context.SuSuppliers on insptran.SupplierId equals sup.Id
                   where (cus.Active.HasValue && cus.Active.Value)
                   orderby inspictrans.Id descending
                   select new ICItemRepo
                   {
                       BookingNo = insptran.Id,
                       Customer = cus.CustomerName,
                       ICId = inspictrans.Id,
                       ICNo = inspictrans.Icno,
                       ServiceDateFrom = insptran.ServiceDateFrom,
                       ServiceDateTo = insptran.ServiceDateTo,
                       StatusId = inspictrans.Icstatus,
                       StatusName = icstaus.StatusName,
                       Supplier = sup.SupplierName,
                       customerId = insptran.CustomerId,
                       SupplierId = insptran.SupplierId,
                       ICCreatedDate = inspictrans.CreatedOn,
                       BuyerName = inspictrans.BuyerName
                   };
        }

        public IQueryable<InspectionTransactionData> GetInspectionTransactions(IQueryable<int?> poTransactionIds)
        {
            return _context.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value && poTransactionIds.Contains(x.Id))
                               .Select(x =>
                                    new InspectionTransactionData
                                    {
                                        PoTransactionId = x.Id,
                                        CustomerId = x.Inspection.Customer.Id,
                                        SupplierId = x.Inspection.Supplier.Id,
                                        CustomerName = x.Inspection.Customer.CustomerName,
                                        SupplierName = x.Inspection.Supplier.SupplierName,
                                        BookingNumber = x.Inspection.Id,
                                        PoNo = x.Po.Pono,
                                        ServiceDateFrom = x.Inspection.ServiceDateFrom,
                                        ServiceDateTo = x.Inspection.ServiceDateTo,
                                    }).AsNoTracking();
        }


        public IQueryable<InspectionCertificateData> GetInspectionCertificateDetails(int icId)
        {
            return _context.InspIcTransactions.Where(x => x.Id == icId)
                               .Select(x =>
                                    new InspectionCertificateData
                                    {
                                        IcNo = x.Icno,
                                        CreationDate = x.CreatedOn,
                                        InspIcTranProducts = x.InspIcTranProducts,
                                        StatusId = x.Icstatus
                                    }).AsNoTracking();
        }

        public IEnumerable<ICSummaryProducts> GetICSummaryProducts(IEnumerable<int> poTransactionIds, int icID)
        {
            return _context.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value && poTransactionIds.Contains(x.Id))
                               .Select(x =>
                                    new ICSummaryProducts
                                    {
                                        BookingNumber = x.Inspection.Id,
                                        PoNo = x.Po.Pono,
                                        ProductCode = x.ProductRef.Product.ProductId,
                                        ProductDesc = x.ProductRef.Product.ProductDescription,
                                        BookingQuantity = x.BookingQuantity,
                                        ICShipmentQuantity = x.InspIcTranProducts.FirstOrDefault(y => y.BookingProductId == x.Id && y.Icid == icID && y.ShipmentQty.HasValue).ShipmentQty.Value,
                                        ReportTitle = x.ProductRef.FbReport.ReportTitle,
                                        ReportStatus = x.ProductRef.FbReport.FbReportStatusNavigation.FbstatusName,
                                        InspectedQuantity = x.FbReportQuantityDetails.Where(y => y.InspPoTransactionId == x.Id && y.InspectedQuantity.HasValue).Sum(z => z.InspectedQuantity.Value)
                                    }).AsNoTracking();
        }

        public Task<List<InspIcStatus>> GetICStatus()
        {
            return _context.InspIcStatuses
                  .Where(x => x.Active != null && x.Active.Value).ToListAsync();
        }
        public async Task<List<QuantityPoId>> GetFBPresentedQty(List<int> inspPoTransactionId)
        {
            return await _context.FbReportQuantityDetails.Where(x => inspPoTransactionId.Contains(x.InspPoTransactionId) && x.Active.Value
                                    && x.PresentedQuantity.HasValue)
                .Select(x => new QuantityPoId
                {
                    InspPoTransactionId = x.InspPoTransactionId,
                    Quantity = x.PresentedQuantity
                }).ToListAsync();
        }
        public async Task<List<QuantityPoId>> GetICProductQty(List<int> inspPoTransactionId)
        {
            return await _context.InspIcTranProducts.Where(x => inspPoTransactionId.
                            Contains(x.BookingProductId.HasValue ? x.BookingProductId.Value : 0) && x.Active.HasValue && x.Active.Value && x.ShipmentQty.HasValue)
                .Select(x => new
                {
                    x.BookingProductId,
                    x.ShipmentQty
                })
                .GroupBy(x => x.BookingProductId, x => x, (key, _data) => new QuantityPoId
                {
                    InspPoTransactionId = key.GetValueOrDefault(),
                    Quantity = _data.Sum(x => x.ShipmentQty)
                })
                .ToListAsync();
        }
        public Task<List<DropDown>> GetICTitleList()
        {
            return _context.InspIcTitles.Where(x => x.Active.HasValue && x.Active.Value)
                .Select(x => new DropDown
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        // Get the dept booking ids by booking ids
        public async Task<List<BookingDeptAccess>> GetDeptBookingIdsByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranCuDepartments.Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(x =>
                 new BookingDeptAccess
                 {
                     DeptId = x.Department.Id,
                     DeptName = x.Department.Name,
                     BookingId = x.InspectionId
                 }).ToListAsync();
        }

        // Get the brand booking ids by booking ids
        public async Task<List<BookingBrandAccess>> GetBrandBookingIdsByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranCuBrands.Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(x => new BookingBrandAccess
                {
                    BrandId = x.Brand.Id,
                    BrandName = x.Brand.Name,
                    BookingId = x.InspectionId
                }).ToListAsync();
        }

        /// <summary>
        /// Get the booking service types by booking list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingServiceType>> GetBookingServiceTypes(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranServiceTypes.Where(x => x.Active && bookingIds.Contains(x.InspectionId)).
                    Select(x => new BookingServiceType
                    {
                        BookingNo = x.InspectionId,
                        ServiceTypeId = x.ServiceTypeId,
                        ServiceTypeName = x.ServiceType.Name
                    }).ToListAsync();
        }

        /// <summary>
        /// issued ic booking id list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<ICBookingModel>> GetIssuedICBookingIdList(IQueryable<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.Active.Value &&
            x.InspIcTranProducts.Any(y => y.Ic.Icstatus != (int)InspectionCertificateStatus.Cancel))
                         .Select(x => new ICBookingModel { BookingId = x.InspectionId }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get partial issue ic booking id list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<ICBookingModel>> GetPartialIssueICBookingIdList(IQueryable<int> bookingIds)
        {
            return await (from fbqty in _context.FbReportQuantityDetails

                          join pordertran in _context.InspPurchaseOrderTransactions on fbqty.InspPoTransactionId equals pordertran.Id

                          join icprodtran in _context.InspIcTranProducts on pordertran.Id equals icprodtran.BookingProductId into ictranpro
                          from ictranprodata in ictranpro.DefaultIfEmpty()

                          join cusdes in _context.InspRepCusDecisions on fbqty.FbReportDetailId equals cusdes.ReportId
                          where ((((fbqty.PresentedQuantity - ictranprodata.ShipmentQty) > 0) && ictranprodata.Active.Value) || ictranprodata.Icid == null)

                          && cusdes.CustomerResultId == (int)InspCustomerDecisionEnum.Pass
                          && bookingIds.Contains(pordertran.InspectionId) && fbqty.Active.Value && pordertran.Active.Value
                          && cusdes.Active.Value
                          select new ICBookingModel()
                          {
                              BookingId = pordertran.InspectionId
                          })
                         .Distinct().ToListAsync();
        }
    }
}
