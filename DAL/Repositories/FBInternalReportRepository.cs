using Contracts.Repositories;
using DTO.Common;
using DTO.FBInternalReport;
using DTO.Inspection;
using DTO.Report;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class FBInternalReportRepository : Repository, IFBInternalReportRepository
    {
        public FBInternalReportRepository(API_DBContext context) : base(context)
        {
        }

        //Fetch all the Inspections
        public IQueryable<InternalReportBookingValues> GetAllInspectionsReports()
        {
            return _context.InspTransactions
                .Select(x => new InternalReportBookingValues
                {
                    BookingId = x.Id,
                    CustomerBookingNo = x.CustomerBookingNo,
                    CustomerId = x.CustomerId,
                    FactoryId = x.FactoryId,
                    SupplierId = x.SupplierId,
                    OfficeId = x.OfficeId,
                    CustomerName = x.Customer.CustomerName,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryName = x.Factory.SupplierName,
                    ServiceDateFrom = x.ServiceDateFrom,
                    ServiceDateTo = x.ServiceDateTo,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Status,
                    IsPicking = false,
                    PreviousBookingNo = x.PreviousBookingNo,
                    Status = x.Status
                });
        }

        //Fetch the Product list for an inspection
        public IQueryable<InternalReportProducts> GetProductsByBooking(int bookingId)
        {
            return _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId).Select(z => new InternalReportProducts()
            {
                BookingId = bookingId,
                ProductId = z.Product.Id,
                ProductName = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                ProductQuantity = z.TotalBookingQuantity,
                ProductSubCategoryName = z.Product.ProductCategoryNavigation.Name,
                FbReportId = z.FbReportId.GetValueOrDefault(),
                ColorCode = ReportResult.FFFF.ToString(),
                CombineProductId = z.CombineProductId.GetValueOrDefault(),
                // PONumber = z.InspPurchaseOrderTransactions.Select(x=>x.Po.Pono).ToList(),
                CombineAqlQuantity = z.CombineAqlQuantity.GetValueOrDefault()

            }).AsNoTracking();
        }

        //Get the service Type of each booking. 
        public async Task<IEnumerable<ServiceTypeList>> GetServiceType(IEnumerable<int> bookingIds)
        {

            return await _context.InspTranServiceTypes.Where(x => x.Active && x.ServiceType.Active && bookingIds.Contains(x.InspectionId))
                      .Select(x => new ServiceTypeList
                      {
                          InspectionId = x.InspectionId,
                          serviceTypeId = x.ServiceTypeId,
                          serviceTypeName = x.ServiceType.Name
                      }).AsNoTracking().ToListAsync();
        }

        //Get the service Type of each booking query id. 
        public async Task<IEnumerable<ServiceTypeList>> GetServiceTypeList(IQueryable<int> bookingIds)
        {

            return await _context.InspTranServiceTypes.Where(x => x.Active && x.ServiceType.Active && bookingIds.Contains(x.InspectionId))
                      .Select(x => new ServiceTypeList
                      {
                          InspectionId = x.InspectionId,
                          serviceTypeId = x.ServiceTypeId,
                          serviceTypeName = x.ServiceType.Name
                      }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the base inspection details
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        public QCInspectionDetailsRepo GetQCInspectionDetails(int inspectionID)
        {
            return _context.InspTransactions.Where(x => x.Id == inspectionID).
                        Select(x =>
                                new QCInspectionDetailsRepo
                                {
                                    InspectionID = x.Id,
                                    CustomerBookingNo = x.CustomerBookingNo,
                                    Customer = x.Customer.CustomerName,
                                    Supplier = x.Supplier.SupplierName,
                                    Factory = x.Factory.SupplierName,
                                    FactoryPhoneNo = x.Factory.Phone,
                                    FactoryAddress = x.Factory.SuAddresses.Select(y => y.Address).FirstOrDefault(),
                                    FactoryRegionalAddress = x.Factory.SuAddresses.Select(y => y.LocalLanguage).FirstOrDefault(),
                                    ServiceDateFrom = x.ServiceDateFrom,
                                    ServiceDateTo = x.ServiceDateTo,
                                    SupplierId = x.SupplierId,
                                    FactoryId = x.FactoryId,
                                    QuQuotationInspMandays = x.QuQuotationInsps,
                                    InspectionServiceTypes = x.InspTranServiceTypes,
                                    ServiceTypeName = x.InspTranServiceTypes.Where(y => y.Active).Select(z => z.ServiceType.Name).FirstOrDefault(),
                                    QuotationId = x.QuQuotationInspMandays.Where(y => y.Active.HasValue && y.Active.Value
                                      && y.Quotation.IdStatus != (int)QuotationStatus.Canceled).Select(z => z.QuotationId).Distinct().FirstOrDefault(),
                                    ScheduleComments = x.ScheduleComments,
                                    QCBookingComments = x.QcbookingComments,
                                    CollectionName = x.Collection.Name,
                                    BussinessLine = x.BusinessLine,
                                    TotalNonCombineAQLQuantity = x.InspProductTransactions.Where(y => y.Active.Value && !y.CombineProductId.HasValue).Sum(z => z.AqlQuantity.GetValueOrDefault())
                                }).AsNoTracking().FirstOrDefault();
        }
        /// <summary>
        /// Get the inspection product details
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        public IEnumerable<QCInspectionProductDetails> GetQCInspectionProductDetails(int inspectionID)
        {
            return _context.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value && x.InspectionId == inspectionID).
                    Select(x =>
                            new QCInspectionProductDetails
                            {
                                PoNumber = x.Po.Pono,
                                ProductName = x.ProductRef.Product.ProductId,
                                ProductDescription = x.ProductRef.Product.ProductDescription,
                                DestinationCountry = x.DestinationCountry.Alpha2Code,
                                FactoryReference = x.ProductRef.Product.FactoryReference,
                                BarCode = x.ProductRef.Product.Barcode,
                                AQL = x.ProductRef.AqlNavigation.Id == (int)AqlType.AQLCustom ? x.ProductRef.SampleTypeNavigation.SampleType : x.ProductRef.AqlNavigation.Value,
                                BookingQty = x.BookingQuantity,
                                AQLQuantity = x.ProductRef.AqlQuantity,
                                CombinedAQLQuantity = x.ProductRef.CombineAqlQuantity,
                                CombineProductId = x.ProductRef.CombineProductId,
                                IsParentProduct = false,
                                Picking = x.PickingQuantity,
                                ProductRemarks = x.ProductRef.Remarks,
                                PickingRemarks = x.InspTranPickings.Where(y => y.Active).Select(y => y.Remarks).FirstOrDefault(),
                                ProdCategory = x.ProductRef.Product.ProductCategoryNavigation.Name,
                                ProdSubCategory = x.ProductRef.Product.ProductSubCategoryNavigation.Name,
                                ProdSub2Category = x.ProductRef.Product.ProductCategorySub2Navigation.Name,
                                IsEcopack = x.ProductRef.IsEcopack
                            }).OrderBy(x => x.CombineProductId).AsNoTracking().ToList();

        }

        public async Task<List<QCInspectionProductDetails>> GetQCInspectionProductSoftlineDetails(int inspectionID)
        {
            return await _context.InspPurchaseOrderColorTransactions.Where(x => x.Active.HasValue && x.Active.Value && x.PoTrans.InspectionId == inspectionID).
                    Select(x =>
                            new QCInspectionProductDetails
                            {
                                PoNumber = x.PoTrans.Po.Pono,
                                ProductName = x.ProductRef.Product.ProductId,
                                ProductDescription = x.ProductRef.Product.ProductDescription,
                                DestinationCountry = x.PoTrans.DestinationCountry.Alpha2Code,
                                FactoryReference = x.ProductRef.Product.FactoryReference,
                                BarCode = x.ProductRef.Product.Barcode,
                                AQL = x.ProductRef.AqlNavigation.Id == (int)AqlType.AQLCustom ? x.ProductRef.SampleTypeNavigation.SampleType : x.ProductRef.AqlNavigation.Value,
                                BookingQty = x.BookingQuantity.GetValueOrDefault(),
                                AQLQuantity = x.ProductRef.AqlQuantity,
                                CombinedAQLQuantity = x.ProductRef.CombineAqlQuantity,
                                CombineProductId = x.ProductRef.CombineProductId,
                                Picking = x.PickingQuantity,
                                ProductRemarks = x.ProductRef.Remarks,
                                PickingRemarks = x.PoTrans.InspTranPickings.Where(y => y.Active).Select(y => y.Remarks).FirstOrDefault(),
                                ProdCategory = x.ProductRef.Product.ProductCategoryNavigation.Name,
                                ProdSubCategory = x.ProductRef.Product.ProductSubCategoryNavigation.Name,
                                ProdSub2Category = x.ProductRef.Product.ProductCategorySub2Navigation.Name,
                                IsEcopack = x.ProductRef.IsEcopack,
                                Color = x.ColorName,
                            }).OrderBy(x => x.CombineProductId).AsNoTracking().ToListAsync();
        }

        public IQueryable<QcPickingItem> GetQcPickingDetails(int bookingId)
        {
            return (from inspTrans in _context.InspTransactions
                    join inspPotran in _context.InspPurchaseOrderTransactions on inspTrans.Id equals inspPotran.InspectionId
                    where inspTrans.Id == bookingId && inspPotran.Active == true
                    select new QcPickingItem
                    {
                        BookingId = inspTrans.Id,
                        CustomerId = inspTrans.CustomerId,
                        CustomerName = inspTrans.Customer.CustomerName,
                        CustomerBookingNo = inspTrans.CustomerBookingNo,
                        SupplierName = inspTrans.Supplier.SupplierName,
                        FactoryName = inspTrans.Factory.SupplierName,
                        PoTransId = inspPotran.Id,
                        SupplierEmail = inspTrans.Supplier.Email,
                        ServiceDateFrom = inspTrans.ServiceDateFrom.ToString(StandardDateFormat),
                        ServiceDateTo = inspTrans.ServiceDateTo.ToString(StandardDateFormat),
                        OfficeId = inspTrans.OfficeId
                    });
        }

        /// <summary>
        /// get lab address by po trans id
        /// </summary>
        /// <param name="poTransIds"></param>
        /// <returns></returns>
        public IQueryable<QcPickingData> GetLabAddress(IEnumerable<int> poTransIds)
        {
            return from inspPicking in _context.InspTranPickings
                   join pickingcontacts in _context.InspTranPickingContacts on inspPicking.Id equals pickingcontacts.PickingTranId
                   join labContact in _context.InspLabContacts on pickingcontacts.LabContactId equals labContact.Id
                   join labAddress in _context.InspLabAddresses on inspPicking.LabAddressId equals labAddress.Id
                   join lab in _context.InspLabDetails on labContact.LabId equals lab.Id
                   where poTransIds.Contains(inspPicking.PoTranId) && inspPicking.Active && pickingcontacts.Active
                   select new QcPickingData
                   {
                       PoTransId = inspPicking.PoTranId,
                       LabAddress = labAddress.Address,
                       LabName = lab.LabName,
                       ContactName = labContact.ContactName,
                       Telephone = labContact.Phone,
                       Email = labContact.Mail,
                       PickingQuantity = inspPicking.PickingQty,
                       RegionalAddress = labAddress.RegionalLanguage,
                       Lab = true,
                       AddressId = inspPicking.LabAddressId.GetValueOrDefault()
                   };
        }

        /// <summary>
        /// get cus address by po trans id
        /// </summary>
        /// <param name="poTransIds"></param>
        /// <returns></returns>
        public IQueryable<QcPickingData> GetCusAddress(IEnumerable<int> poTransIds)
        {
            return from inspPicking in _context.InspTranPickings
                   join pickingcontacts in _context.InspTranPickingContacts on inspPicking.Id equals pickingcontacts.PickingTranId
                   join cusContact in _context.CuContacts on pickingcontacts.CusContactId equals cusContact.Id
                   join cusAddress in _context.CuAddresses on inspPicking.CusAddressId equals cusAddress.Id
                   join cus in _context.CuCustomers on cusContact.CustomerId equals cus.Id
                   where poTransIds.Contains(inspPicking.PoTranId) && inspPicking.Active && pickingcontacts.Active
                   && cus.Active.HasValue && cus.Active.Value && cusAddress.Active.HasValue && cusAddress.Active.Value
                   select new QcPickingData
                   {
                       PoTransId = inspPicking.PoTranId,
                       LabAddress = cusAddress.Address,
                       LabName = cus.CustomerName,
                       ContactName = cusContact.ContactName,
                       Telephone = cusContact.Phone,
                       Email = cusContact.Email,
                       PickingQuantity = inspPicking.PickingQty,
                       Customer = true,
                       AddressId = inspPicking.CusAddressId.GetValueOrDefault()
                   };
        }

        public IQueryable<PickingProductData> GetPickingProducts(IEnumerable<int> poTransIds)
        {
            return (from inspPicking in _context.InspTranPickings
                    join inspPo in _context.InspPurchaseOrderTransactions on inspPicking.PoTranId equals inspPo.Id
                    //  join cuPoOrderDetail in _context.CuPurchaseOrderDetails on inspPo.PoDetailId equals cuPoOrderDetail.Id
                    //join cuProd in _context.CuProducts on inspPo.ProductRef.ProductId equals cuProd.Id
                    //join cuPoOrder in _context.CuPurchaseOrders on cuPoOrderDetail.PoId equals cuPoOrder.Id
                    where poTransIds.Contains(inspPo.Id) && inspPicking.Active
                    select new PickingProductData
                    {
                        ProductId = inspPo.ProductRef.Product.ProductId,
                        FactoryReference = inspPo.ProductRef.Product.FactoryReference,
                        PONumber = inspPo.Po.Pono,
                        DestinationCountry = inspPo.DestinationCountry.CountryName,
                        isLab = inspPicking.LabAddressId == null ? false : true,
                        AddressId = inspPicking.LabAddressId != null ? inspPicking.LabAddressId.GetValueOrDefault() : inspPicking.CusAddressId.GetValueOrDefault(),
                        PickingQuantity = inspPicking.PickingQty
                    }).OrderBy(x => x.ProductId);
        }
    }
}
