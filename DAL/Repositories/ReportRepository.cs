using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Entities;
using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using DTO.Report;
using Entities.Enums;
using System.Threading.Tasks;
using DTO.Kpi;

namespace DAL.Repositories
{
    public class ReportRepository : Repository, IReportRepository
    {
        public ReportRepository(API_DBContext context) : base(context)
        {
        }

        //Fetch all the Inspections
        public IQueryable<CustomerReportBookingValues> GetAllInspectionsReports()
        {
            return _context.InspTransactions
                .Select(x => new CustomerReportBookingValues
                {
                    BookingId = x.Id,
                    CustomerBookingNo = x.CustomerBookingNo,
                    CustomerId = x.CustomerId,
                    FactoryId = x.FactoryId,
                    SupplierId = x.SupplierId,
                    missionId = x.FbMissionId,
                    OfficeId = x.OfficeId,
                    Office = x.Office.LocationName,
                    CustomerName = x.Customer.CustomerName,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryName = x.Factory.SupplierName,
                    ServiceDateFrom = x.ServiceDateFrom,
                    ServiceDateTo = x.ServiceDateTo,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Status,
                    StatusPriority = x.Status.Priority,
                    IsPicking = false,
                    PreviousBookingNo = x.PreviousBookingNo,
                    Status = x.Status,
                    MissionStatus = x.FbMissionStatusNavigation.StatusName,
                    ReportDate = x.InspTranStatusLogs.Where(z => z.StatusId == (int)BookingStatus.ReportSent).OrderByDescending(z => z.CreatedOn).Select(z => z.StatusChangeDate).FirstOrDefault()
                }).AsNoTracking();
        }

        /// <summary>
        /// get Qc details by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<SchScheduleQc>> GetQCDetails(int bookingId)
        {
            return await _context.SchScheduleQcs
                          .Include(x => x.Qc)
                            .Where(x => x.Active && x.BookingId == bookingId).ToListAsync();
        }

        //Fetch the Product list for an inspection
        public async Task<IEnumerable<ReportProducts>> GetProductsByBooking(int bookingId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId).
                Select(z => new ReportProducts()
                {
                    Id = z.Id,
                    BookingId = bookingId,
                    BookingStatusId = z.Inspection.StatusId,
                    ProductId = z.ProductId,
                    ProductName = z.Product.ProductId,
                    ProductDescription = z.Product.ProductDescription,
                    ProductQuantity = z.TotalBookingQuantity,
                    ProductCategoryName = z.Product.ProductCategoryNavigation.Name,
                    ProductSubCategoryName = z.Product.ProductSubCategoryNavigation.Name,
                    FbReportId = z.FbReport.FbReportMapId.GetValueOrDefault(),
                    ApiReportId = z.FbReportId.GetValueOrDefault(),
                    ColorCode = ReportResult.FFFF.ToString(),
                    TotalBookingQuantity = z.TotalBookingQuantity,
                    CombineProductId = z.CombineProductId.GetValueOrDefault(),
                    CombineAqlQuantity = z.CombineAqlQuantity.GetValueOrDefault()
                }).AsNoTracking().ToListAsync();
        }

        //Fetch the Product list for an inspection
        public async Task<IEnumerable<ReportProducts>> GetContainersByBooking(int bookingId)
        {
            return await _context.InspContainerTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && y.InspectionId == bookingId).Select(z => new ReportProducts()
            {
                Id = z.Id,
                BookingId = bookingId,
                BookingStatusId = z.Inspection.StatusId,
                ProductQuantity = z.TotalBookingQuantity,
                FbReportId = z.FbReport.FbReportMapId.GetValueOrDefault(),
                ApiReportId = z.FbReportId.GetValueOrDefault(),
                ColorCode = ReportResult.FFFF.ToString(),
                TotalBookingQuantity = z.TotalBookingQuantity,
            }).AsNoTracking().ToListAsync();
        }

        //Fetch the Product List for a particular booking
        public async Task<IEnumerable<ReportProducts>> GetProductListByBooking(IEnumerable<int> bookingId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value && bookingId.Contains(y.InspectionId)).Select(z => new ReportProducts()
            {
                BookingId = z.InspectionId,
                ProductId = z.ProductId,
                ProductName = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                ProductQuantity = z.TotalBookingQuantity,
                ProductSubCategoryId = z.Product.ProductSubCategory,
                ProductSubCategory2Id = z.Product.ProductCategorySub2,
                ProductCategoryName = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategoryName = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2Name = z.Product.ProductCategorySub2Navigation.Name,
                FbReportId = z.FbReportId.GetValueOrDefault(),
                ColorCode = ReportResult.FFFF.ToString(),
                CombineProductId = z.CombineProductId.GetValueOrDefault(),
                CombineAqlQuantity = z.CombineAqlQuantity.GetValueOrDefault(),
                Unit = z.UnitNavigation.Name,
                Aql = z.Aql,
                Critical = z.Critical,
                Major = z.Major,
                Minor = z.Minor,
                AqlQuantity = z.AqlQuantity,
                ProductRefId = z.Id,
                TotalBookingQuantity = z.TotalBookingQuantity
            }).AsNoTracking().ToListAsync();

        }

        //Fetch the Product List for a particular booking
        public async Task<IEnumerable<ReportProducts>> GetProductPoListByBooking(List<int> bookingId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value && bookingId.Contains(y.InspectionId)).Select(z => new ReportProducts()
            {
                BookingId = z.InspectionId,
                ProductId = z.ProductRef.ProductId,
                ProductName = z.ProductRef.Product.ProductId,
                ProductDescription = z.ProductRef.Product.ProductDescription,
                ProductQuantity = z.BookingQuantity,
                ProductSubCategoryName = z.ProductRef.Product.ProductCategoryNavigation.Name,
                FbReportId = z.ProductRef.FbReportId.GetValueOrDefault(),
                FbReportContainerId = z.ContainerRef.FbReportId.GetValueOrDefault(),
                ColorCode = ReportResult.FFFF.ToString(),
                CombineProductId = z.ProductRef.CombineProductId.GetValueOrDefault(),
                PONumber = z.Po.Pono.ToString(),
                CombineAqlQuantity = z.ProductRef.CombineAqlQuantity.GetValueOrDefault(),
                ContainerId = z.ContainerRef.ContainerId
            }).AsNoTracking().ToListAsync();
        }



        //Fetch only po details
        public async Task<IEnumerable<BookingPONumbers>> GetPONumbersbyBooking(int bookingId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId).Select(z => new BookingPONumbers()
            {
                ProductRefId = z.ProductRef.Id,
                PoNumber = z.Po.Pono.ToString(),
                ContainerRefId = z.ContainerRef.ContainerId
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<int>> GetReportIdsByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value
                        && bookingIds.Contains(x.InspectionId)).
                            Select(x => x.FbReportId.GetValueOrDefault()).ToListAsync();
        }

        public async Task<List<BookingReportSummaryLinkRepo>> GetBookingReportSummaryLink(List<int> bookingIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
                                && bookingIds.Contains(y.Inspection.Id) && inspectedStatusIds.Contains(y.Inspection.StatusId)).
                                Select(z => new BookingReportSummaryLinkRepo()
                                {
                                    BookingId = z.InspectionId,
                                    ReportSummaryLink = z.FbReport.ReportSummaryLink
                                }).ToListAsync();
        }

        public async Task<List<BookingReportSummaryLinkRepo>> GetBookingContainerReportSummaryLink(List<int> bookingIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspContainerTransactions.Where(y => y.Active.HasValue && y.Active.Value
                                && bookingIds.Contains(y.Inspection.Id) && inspectedStatusIds.Contains(y.Inspection.StatusId)).
                                Select(z => new BookingReportSummaryLinkRepo()
                                {
                                    BookingId = z.InspectionId,
                                    ReportSummaryLink = z.FbReport.ReportSummaryLink
                                }).ToListAsync();
        }

        /// <summary>
        /// Get booking summary link data by bookingds and inspection status list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<List<BookingReportSummaryLinkRepo>> GetBookingReportSummaryLinkData(List<int> bookingIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspTransactions.Where(y => bookingIds.Contains(y.Id) && inspectedStatusIds.Contains(y.StatusId)).
                                Select(z => new BookingReportSummaryLinkRepo()
                                {
                                    BookingId = z.Id,
                                    ReportSummaryLink = z.FbReportDetails.Where(x => x.Active.Value).Select(x => x.ReportSummaryLink).FirstOrDefault()
                                }).ToListAsync();
        }

        /// <summary>
        /// Get the fb report details by booking id query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<ExportFBReportRepo>> GetExportFBReportData(IQueryable<int> bookingIds)
        {
            return await _context.FbReportDetails.Where(x => x.Active.Value && bookingIds.Contains(x.InspectionId.Value)).
                    Select(x => new ExportFBReportRepo()
                    {
                        FbReportId = x.Id,
                        InspectionId = x.InspectionId,
                        InspectedQuantity = x.FbReportQuantityDetails.Where(y => y.Active.Value).Sum(y => y.InspectedQuantity),
                        ReportNo = x.ReportTitle,
                        FillingStatus = x.FbFillingStatusNavigation.StatusName,
                        ReviewStatus = x.FbReviewStatusNavigation.StatusName,
                        ReportStatus = x.FbReportStatusNavigation.StatusName,
                        Result = x.Result.ResultName
                    }).ToListAsync();
        }

        /// <summary>
        /// Get the booking data for the non container service by booking id query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<ExportInspectionReportData>> GetBookingProductList(IQueryable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingIds.Contains(y.InspectionId)).Select(z => new ExportInspectionReportData()
            {
                Customer = z.Inspection.Customer.CustomerName,
                CustomerBookingNo = z.Inspection.CustomerBookingNo,
                BookingNo = z.Inspection.Id,
                Factory = z.Inspection.Factory.SupplierName,
                Supplier = z.Inspection.Supplier.SupplierName,
                SupplierId = z.Inspection.SupplierId,
                CustomerId = z.Inspection.CustomerId,
                ServiceDateFrom = z.Inspection.ServiceDateFrom,
                ServiceDateTo = z.Inspection.ServiceDateTo,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductId = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                BookingQuantity = z.TotalBookingQuantity,
                FbReportId = z.FbReportId,
                ProductRefId = z.Id
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        ///  Get the booking data for the container service by booking id query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<ExportInspectionReportData>> GetBookingContainerList(IQueryable<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingIds.Contains(y.InspectionId)).Select(z => new ExportInspectionReportData()
            {
                Customer = z.Inspection.Customer.CustomerName,
                CustomerBookingNo = z.Inspection.CustomerBookingNo,
                BookingNo = z.Inspection.Id,
                Factory = z.Inspection.Factory.SupplierName,
                Supplier = z.Inspection.Supplier.SupplierName,
                SupplierId = z.Inspection.SupplierId,
                CustomerId = z.Inspection.CustomerId,
                ServiceDateFrom = z.Inspection.ServiceDateFrom,
                ServiceDateTo = z.Inspection.ServiceDateTo,
                ProductCategory = z.ProductRef.Product.ProductCategoryNavigation.Name,
                ProductId = z.ProductRef.Product.ProductId,
                ProductDescription = z.ProductRef.Product.ProductDescription,
                BookingQuantity = z.ProductRef.TotalBookingQuantity,
                FbReportId = z.ContainerRef.FbReportId,
                PoNumber = z.Po.Pono,
                ContainerRefId = z.ContainerRefId,
                ProductRefId = z.ProductRefId
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking po details by booking id query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<ProductPOList>> GetPoListByBookingIds(IQueryable<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.Value && bookingIds.Contains(x.InspectionId)).
                          Select(x => new ProductPOList()
                          {
                              FbReportId = x.ProductRef.FbReportId,
                              ProductRefId = x.ProductRefId,
                              PoId = x.PoId,
                              PoNumber = x.Po.Pono
                          }).ToListAsync();
        }

        /// <summary>
        /// Get the booking po details by booking id query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>        
        public async Task<List<ProductPOList>> GetPoListByReportIds(IQueryable<int> reportIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.Value && reportIds.Contains(x.ProductRef.FbReportId.GetValueOrDefault()))
                          .Select(x => new ProductPOList()
                          {
                              FbReportId = x.ProductRef.FbReportId,
                              ProductRefId = x.ProductRefId,
                              PoId = x.PoId,
                              PoNumber = x.Po.Pono
                          }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the fb report detail by api report id
        /// </summary>
        /// <param name="apiReportId"></param>
        /// <returns></returns>
        public async Task<FbReportDetail> GetFBReportDetail(int apiReportId)
        {
            return await _context.FbReportDetails.Include(x => x.FbReportManualLogs).Where(x => x.Active.Value && x.Id == apiReportId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ReportProducts>> GetProductListByReportIds(IEnumerable<int> reportIds, string productRef, string po)
        {
            var query = _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value && reportIds.Contains(y.FbReportId.GetValueOrDefault()));
            if (!string.IsNullOrWhiteSpace(productRef))
            {
                productRef = productRef.Trim();
                query = query.Where(x => x.Product.ProductId == productRef);
            }

            if (!string.IsNullOrWhiteSpace(po))
            {
                po = po.Trim();
                query = query.Where(x => x.InspPurchaseOrderTransactions.Any(x => x.Po.Pono == po));
            }

            return await query.Select(z => new ReportProducts()
            {
                BookingId = z.InspectionId,
                ProductId = z.ProductId,
                ProductName = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                ProductSubCategoryId = z.Product.ProductSubCategory,
                ProductSubCategory2Id = z.Product.ProductCategorySub2,
                FbReportId = z.FbReportId.GetValueOrDefault(),
                Unit = z.UnitNavigation.Name
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<FbReportQualityPlan>> GetFbReportQualityPlansByReportIds(IEnumerable<int> reportIds)
        {
            return await _context.FbReportQuantityPlans.Where(x => reportIds.Contains(x.FbReportDetailsId.GetValueOrDefault())).AsNoTracking().ToListAsync();
        }

        public async Task<List<GapKpiFbReportPackingPackagingLabellingProduct>> GetFbReportPackingPackagingLabellingProducts(IEnumerable<int> fbReportDetailIds, List<int> packingTypes)
        {
            return await _context.FbReportPackingPackagingLabellingProducts.Where(x => fbReportDetailIds.Contains(x.FbReportdetailsId.GetValueOrDefault()) && packingTypes.Contains(x.PackingType.GetValueOrDefault()))
                .Select(y => new GapKpiFbReportPackingPackagingLabellingProduct()
                {
                    FbReportDetailId = y.FbReportdetailsId.GetValueOrDefault(),
                    SampleSize = y.SampleSizeCtns.GetValueOrDefault(),
                    PackingType = y.PackingType.GetValueOrDefault()
                }).AsNoTracking().ToListAsync();
        }
    }
}
