using Contracts.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO.Inspection;
using Entities.Enums;
using DTO.Quotation;
using DTO.Report;
using DTO.CommonClass;
using DTO.CustomerPriceCard;
using DTO.Kpi;
using DTO.Invoice;
using DTO.Common;
using DTO.Customer;
using DTO.Supplier;
using DTO.EntPages;
using DTO.InvoicePreview;

namespace DAL.Repositories
{
    public class InspectionBookingRepository : Repository, IInspectionBookingRepository
    {
        public InspectionBookingRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// Add inspection booking details
        /// </summary>
        /// <param name="entity">Inspection booking Transaction</param>
        /// <returns></returns>
        public Task<int> AddInspectionBooking(InspTransaction entity)
        {
            _context.InspTransactions.Add(entity);
            return _context.SaveChangesAsync();
        }
        /// <summary>
        /// Update inspection booking details
        /// </summary>
        /// <param name="entity">Inspection booking Transaction</param>
        /// <returns>Updated Records Id</returns>
        public Task<int> EditInspectionBooking(InspTransaction entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> EditBookingPoTransaction(InspPoTransaction entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public IQueryable<InspTransaction> GetAllInspectionsQuery()
        {
            return _context.InspTransactions;
        }

        public IQueryable<InspectionBookingItems> GetAllInspections()
        {
            return _context.InspTransactions
                .Select(x => new InspectionBookingItems
                {
                    BookingId = x.Id,
                    CustomerId = x.CustomerId,
                    SupplierId = x.SupplierId,
                    FactoryId = x.FactoryId,
                    CustomerName = x.Customer.CustomerName,
                    CustomerEmail = x.Customer.Email,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryName = x.Factory.SupplierName,
                    ServiceDateFrom = x.ServiceDateFrom,
                    ServiceDateTo = x.ServiceDateTo,
                    FirstServiceDateFrom = x.FirstServiceDateFrom,
                    FirstServiceDateTo = x.FirstServiceDateTo,
                    Office = x.Office.LocationName,
                    OfficeId = x.OfficeId,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Status,
                    StatusPriority = x.Status.Priority,
                    BookingCreatedBy = x.CreatedBy,
                    PreviousBookingNo = x.PreviousBookingNo,
                    ApplyDate = x.CreatedOn.GetValueOrDefault(),
                    Status = x.Status,
                    Customer = x.Customer,
                    CustomerBookingNo = x.CustomerBookingNo,
                    BookingAPiRemarks = x.ApiBookingComments,
                    IsPicking = x.IsPickingRequired ?? false,
                    PriceCategoryId = x.PriceCategoryId,
                    PriceCategoryName = x.PriceCategory.Name,
                    CollectionId = x.CollectionId,
                    CollectionName = x.Collection.Name,
                    UserTypeId = x.CreatedByNavigation.UserTypeId,
                    IsCombineRequired = x.IsCombineRequired
                }).OrderByDescending(x => x.ServiceDateTo);
        }

        public IQueryable<InspTransaction> GetAllBookingPoTransactions()
        {
            return _context.InspTransactions
                             .Include(x => x.InspPurchaseOrderTransactions)
                                 .ThenInclude(x => x.ProductRef);

        }

        public IQueryable<InspTransaction> GetAllInspectionsReports()
        {
            return _context.InspTransactions
                   .Include(x => x.Customer)
                   .Include(x => x.Supplier)
                   .Include(x => x.Factory)
                    .ThenInclude(x => x.SuAddresses)
                   .Include(x => x.CreatedByNavigation)
                   .Include(x => x.SchScheduleQcs)
                        .ThenInclude(x => x.Qc)

                    .Include(x => x.SchScheduleCS)
                        .ThenInclude(x => x.Cs)
                   .Include(x => x.InspPurchaseOrderTransactions)
                             .ThenInclude(x => x.Po)

                   .Include(x => x.InspPurchaseOrderTransactions)
                              .ThenInclude(x => x.ProductRef)
                             .ThenInclude(x => x.Product)
                                .ThenInclude(x => x.ProductCategoryNavigation)

                   .Include(x => x.InspPurchaseOrderTransactions)
                            .ThenInclude(x => x.ProductRef)
                             .ThenInclude(x => x.Product)
                             .ThenInclude(x => x.ProductSubCategoryNavigation)
                                 .ThenInclude(x => x.RefProductCategorySub2S)
                   .Include(x => x.Status)
                   .Include(x => x.InspTranServiceTypes)
                   .ThenInclude(x => x.ServiceType)
                   .Include(x => x.Office)
                   .Include(x => x.FbMissionStatusNavigation)
                   .OrderBy(x => x.ServiceDateFrom).ThenBy(x => x.ServiceDateTo);
        }

        public IQueryable<InspTransaction> GetAllBookingPurchaseOrders()
        {
            return _context.InspTransactions
                   .Include(x => x.InspProductTransactions)
                                .ThenInclude(x => x.Product)
                                    .Include(x => x.InspProductTransactions)
                                     .ThenInclude(x => x.InspPurchaseOrderTransactions)
                   .Include(x => x.Status);
        }

        public async Task<List<InspProductTransaction>> GetBookingPOTransactionDetails(int bookingId)
        {
            return await _context.InspProductTransactions.Include(x => x.InspPurchaseOrderTransactions).Where(x => x.Active.Value && x.InspectionId == bookingId).ToListAsync();
        }

        /// <summary>
        /// Get the product transaction details from the booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<InspProductTransaction>> GetBookingProductTransactionDetails(int bookingId)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value && x.InspectionId == bookingId).ToListAsync();
        }

        public async Task<InspTransaction> GetBookingTransaction(int bookingId)
        {
            return await _context.InspTransactions.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == bookingId);
        }

        public async Task<InspTransaction> GetBookingCombineOrders(int bookingId)
        {
            return await _context.InspTransactions
                .Include(x => x.InspProductTransactions)
                .FirstOrDefaultAsync(x => x.Id == bookingId);
        }

        /// <summary>
        /// get quotation manday by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<double?> GetQuotationManDayByBooking(int bookingId)
        {
            return await _context.QuQuotationInsps.Where(x => x.IdBooking == bookingId && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                .Select(y => y.NoOfManDay).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the inspection booking details based on the inspection id
        /// </summary>
        /// <param name="inspectionID">Inspection Id</param>
        /// <returns>Returns the inspection booking details</returns>
        public async Task<InspTransaction> GetInspectionByID(int inspectionID)
        {

            // Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            // stopwatch.Start();
            return await _context.InspTransactions
                    .Include(x => x.InspTranCuContacts)
                    .Include(x => x.InspTranFaContacts)
                    .Include(x => x.InspTranSuContacts)
                    .Include(x => x.InspTranStatusLogs)

                    .Include(x => x.InspContainerTransactions)

                    .Include(x => x.InspProductTransactions)
                    .ThenInclude(x => x.InspPurchaseOrderTransactions)
                    .ThenInclude(x => x.InspPurchaseOrderColorTransactions)

                    .Include(x => x.InspTranPickings)
                    .ThenInclude(x => x.InspTranPickingContacts)

                    //.Include(x => x.InspProductTransactions)
                    //.ThenInclude(x => x.InspPurchaseOrderTransactions)
                    //.ThenInclude(x => x.InspTranPickings)
                    //.ThenInclude(x=>x.InspTranPickingContacts)

                    //.Include(x => x.InspProductTransactions)
                    //.ThenInclude(x => x.InspPurchaseOrderColorTransactions)

                    .Include(x => x.CuProductFileAttachments)

                    .Include(x => x.InspTranServiceTypes)
                    .Include(x => x.InspTranCuBuyers)

                    .Include(x => x.InspTranCuBrands)
                    .Include(x => x.InspTranCuDepartments)
                    .Include(x => x.InspTranCuMerchandisers)
                    .Include(x => x.InspTranShipmentTypes)
                    .Include(x => x.InspTranCS)
                    .Include(x => x.InspDfTransactions)
                    .Include(x => x.InspTranFileAttachments)
                    .Include(x => x.InspTranFileAttachmentZips)
                    .Include(x => x.InspTranHoldReasons)



                    .FirstOrDefaultAsync(x => x.Id == inspectionID);

            // Stop timing.
            // stopwatch.Stop();

            // Write result.
            //  Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

            // return dataset;

        }


        //public async Task<InspTransaction> GetInspectionByID(int inspectionID)
        //{

        //    // Stopwatch stopwatch = new Stopwatch();
        //    // Begin timing.
        //    // stopwatch.Start();
        //    return await _context.InspTransactions
        //            .Include(x => x.Customer)

        //            .Include(x => x.Supplier)
        //            .ThenInclude(x => x.SuAddresses)

        //            .Include(x => x.Factory)
        //            .ThenInclude(x => x.SuAddresses)

        //            .Include(x => x.CreatedByNavigation)
        //            .Include(x => x.Status)

        //            .Include(x => x.InspTranServiceTypes)
        //            .ThenInclude(x => x.ServiceType)

        //            .Include(x => x.Office)
        //            //PoId and ProductId removal from the potransaction table
        //            .Include(x => x.InspProductTransactions)
        //                      //   .ThenInclude(x => x.PoDetail)
        //                      .ThenInclude(x => x.Product)
        //                         .ThenInclude(x => x.ProductCategoryNavigation)
        //                         .ThenInclude(x => x.RefProductCategorySubs)

        //            .Include(x => x.InspProductTransactions)
        //                      //  .ThenInclude(x => x.PoDetail)
        //                      .ThenInclude(x => x.Product)
        //                      .ThenInclude(x => x.ProductSubCategoryNavigation)
        //                          .ThenInclude(x => x.RefProductCategorySub2S)

        //              .Include(x => x.InspProductTransactions)
        //                     .ThenInclude(x => x.AqlNavigation)

        //             .Include(x => x.InspProductTransactions)
        //                      // .ThenInclude(x => x.PoDetail)
        //                      .ThenInclude(x => x.Product)
        //                      .ThenInclude(x => x.CuProductFileAttachments)

        //             .Include(x => x.InspProductTransactions)
        //                     .ThenInclude(x => x.InspPurchaseOrderTransactions)
        //                     .ThenInclude(x => x.Po)
        //                      .ThenInclude(x => x.CuPurchaseOrderDetails)
        //             //PoId and ProductId removal from the potransaction table sabari
        //             //.Include(x => x.InspPoTransactions)
        //             //        .ThenInclude(x => x.InspTranPickings)
        //             //        .ThenInclude(x => x.InspTranPickingContacts)

        //             .Include(x => x.InspProductTransactions)
        //                      .ThenInclude(x => x.InspPurchaseOrderTransactions)
        //                      .ThenInclude(x => x.DestinationCountry)

        //            .Include(x => x.InspProductTransactions)
        //                      .ThenInclude(x => x.InspPurchaseOrderTransactions)
        //                      .ThenInclude(x => x.InspPurchaseOrderColorTransactions)

        //             .Include(x => x.InspProductTransactions)
        //                      .ThenInclude(x => x.InspPurchaseOrderTransactions)
        //                      .ThenInclude(x => x.ContainerRef)
        //             .Include(x => x.InspProductTransactions)
        //                    .ThenInclude(x => x.ParentProduct)

        //                    .Include(x => x.InspTranCuContacts)
        //                    .ThenInclude(x => x.Contact)
        //            .Include(x => x.InspTranSuContacts)
        //            .ThenInclude(x => x.Contact)
        //            .Include(x => x.InspTranFaContacts)
        //            .ThenInclude(x => x.Contact)
        //            .Include(x => x.InspTranCuBrands)

        //            .ThenInclude(x => x.Brand)
        //            .Include(x => x.InspTranCuBuyers)

        //            .ThenInclude(x => x.Buyer)
        //            .Include(x => x.InspTranCuDepartments)

        //            .ThenInclude(x => x.Department)
        //            .Include(x => x.InspTranFileAttachments)
        //            .Include(x => x.InspTranFileAttachmentZips)
        //            //Include Dynamic customer configuration fields
        //            .Include(x => x.InspDfTransactions)
        //            .Include(x => x.CreatedByNavigation)
        //            .Include(x => x.InspTranCuMerchandisers)

        //            .Include(x => x.InspProductTransactions)
        //                     .ThenInclude(x => x.Fbtemplate)
        //             .Include(x => x.Collection)
        //             .Include(x => x.PriceCategory)
        //             .Include(x => x.InspTranHoldReasons)
        //             .ThenInclude(x => x.ReasonTypeNavigation)
        //             .Include(x => x.InspTranShipmentTypes)
        //             .Include(x => x.InspTranCS)
        //             .Include(x => x.Season)
        //             .ThenInclude(x => x.Season)
        //             .Include(x => x.SeasonYear)
        //             .Include(x => x.InspProductTransactions)
        //                     .ThenInclude(x => x.UnitNavigation)
        //            .FirstOrDefaultAsync(x => x.Id == inspectionID);

        //    // Stop timing.
        //    // stopwatch.Stop();

        //    // Write result.
        //    //  Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

        //    // return dataset;

        //}



        public async Task<InspTransaction> GetInspectionReportDetails(int inspectionID)
        {
            return await _context.InspTransactions.Where(x => x.Id == inspectionID)
                   .Include(x => x.Customer)
                      .ThenInclude(x => x.CuContacts)
                        .ThenInclude(x => x.InspTranCuMerchandisers)
                     .Include(x => x.Customer)
                      .ThenInclude(x => x.CuContacts)
                        .ThenInclude(x => x.InspTranCuContacts)
                    .Include(x => x.Customer)
                    .ThenInclude(x => x.CuDepartments)
                    .ThenInclude(x => x.InspTranCuDepartments)
                    .Include(x => x.Customer)
                    .ThenInclude(x => x.CuBuyers)
                     .ThenInclude(x => x.InspTranCuBuyers)

                     .Include(x => x.InspTranCuBrands)
                     .ThenInclude(x => x.Brand)

                    .Include(x => x.Customer)
                      .ThenInclude(x => x.CuAddresses)
                        .ThenInclude(x => x.Country)

                    .Include(x => x.Customer)
                      .ThenInclude(x => x.CuAddresses)
                        .ThenInclude(x => x.City)

                   .Include(x => x.Supplier)
                   .ThenInclude(x => x.SuAddresses)
                       .ThenInclude(x => x.Country)
                         .ThenInclude(x => x.RefProvinces)
                           .ThenInclude(x => x.RefCities)


                      .Include(x => x.Supplier)
                      .ThenInclude(x => x.SuContacts)
                        .ThenInclude(x => x.InspTranSuContacts)

                   .Include(x => x.Factory)
                   .ThenInclude(x => x.SuAddresses)
                        .ThenInclude(x => x.Country)
                         .ThenInclude(x => x.RefProvinces)
                          .ThenInclude(x => x.RefCities)
                   .Include(x => x.CreatedByNavigation)
                   .Include(x => x.Status)
                   .Include(x => x.SchScheduleQcs)
                   .Include(x => x.SchScheduleCS)
                   .Include(x => x.InspTranServiceTypes)
                   .ThenInclude(x => x.ServiceType)
                   .Include(x => x.Office)
                   .Include(x => x.InspProductTransactions)
                             .ThenInclude(x => x.Product)
                                .ThenInclude(x => x.ProductCategoryNavigation)

                   .Include(x => x.InspProductTransactions)
                             .ThenInclude(x => x.Product)
                             .ThenInclude(x => x.ProductSubCategoryNavigation)
                                 .ThenInclude(x => x.RefProductCategorySub2S)

                    .Include(x => x.InspProductTransactions)
                    .ThenInclude(x => x.InspPurchaseOrderTransactions)
                            .ThenInclude(x => x.Po)

                    .Include(x => x.InspPurchaseOrderTransactions)
                            .ThenInclude(x => x.DestinationCountry)

                     .Include(x => x.InspContainerTransactions)
                            .ThenInclude(x => x.InspPurchaseOrderTransactions)

                        .Include(x => x.InspProductTransactions)
                            .ThenInclude(x => x.AqlNavigation)

                      .Include(x => x.InspProductTransactions)
                            .ThenInclude(x => x.MajorNavigation)

                     .Include(x => x.InspProductTransactions)
                            .ThenInclude(x => x.MinorNavigation)

                     .Include(x => x.InspProductTransactions)
                            .ThenInclude(x => x.CriticalNavigation)

                      .Include(x => x.InspProductTransactions)
                                            .ThenInclude(x => x.Fbtemplate)

                       .Include(x => x.InspProductTransactions)
                            .ThenInclude(x => x.FbReport)

                        .Include(x => x.InspProductTransactions)
                    .ThenInclude(x => x.InspPurchaseOrderTransactions)
                            .ThenInclude(x => x.InspPurchaseOrderColorTransactions)
                    .Include(x => x.InspTranFileAttachments)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync();
        }


        public async Task<InspTransaction> GetInspectionPickingByBookingID(int inspectionID)
        {
            return await _context.InspTransactions.Where(x => x.Id == inspectionID)

                   .Include(x => x.InspPurchaseOrderTransactions)
                             .ThenInclude(x => x.InspTranPickings)
                               .ThenInclude(x => x.InspTranPickingContacts)

                   .FirstOrDefaultAsync();
        }

        public Task<List<InspStatus>> GetBookingStatus()
        {
            return _context.InspStatuses
                  .Where(x => x.Active != null && x.Active.Value).ToListAsync();
        }

        public InspStatus GetBookingStatusById(int statusId)
        {
            return _context.InspStatuses
                  .Where(x => x.Active != null && x.Active.Value && x.Id == statusId).FirstOrDefault();
        }

        public async Task<IEnumerable<InspTransaction>> GetInspections()
        {
            return await _context.InspTransactions.ToListAsync();
        }


        public CuProductFileAttachment GetProductFile(string uniqueId)
        {
            return _context.CuProductFileAttachments.Where(x => x.UniqueId == uniqueId && x.Active && x.Product.Active).FirstOrDefault();
        }

        public async Task<InspTranFileAttachment> GetFile(int id)
        {
            return await _context.InspTranFileAttachments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CuProductFileAttachment> GetProductFile(int id, int userId)
        {
            return await _context.CuProductFileAttachments.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<InspTransaction> GetBookingResponse(int id)
        {
            return await _context.InspTransactions.Where(x => x.Id == id)
                            .Include(x => x.InspPurchaseOrderTransactions)
                            .ThenInclude(x => x.InspTranPickings)
                            .ThenInclude(x => x.InspTranPickingContacts)
                            .Include(x => x.Factory)
                            .Include(x => x.Supplier)
                            .Include(x => x.InspTranFaContacts)
                            .ThenInclude(x => x.Contact)
                            .Include(x => x.Entity)
                            .ThenInclude(x => x.AudBookingRules)
                            //.ThenInclude(x => x.InspBookingRules).Where(x => x.Entity.InspBookingRules.First().SendEmailToCUS_Contact == true)
                            .ThenInclude(x => x.Customer).Where(x => x.Entity.AudBookingRules.First().IsDefault == true)
                            .Include(x => x.Entity)
                            .ThenInclude(x => x.AudBookingContacts)
                            //.ThenInclude(x => x.InspBookingContacts)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<ItUserMaster> GetUserName(int userId)
        {
            return _context.ItUserMasters.Where(x => x.Id == userId && x.Active).FirstOrDefaultAsync();
        }

        public Task<InspCancelReason> GetReasonName(int reasonId)
        {
            return _context.InspCancelReasons.Where(x => x.Id == reasonId && x.Active).FirstOrDefaultAsync();
        }

        public Task<InspRescheduleReason> GetRescheduleReasonName(int reasonId)
        {
            return _context.InspRescheduleReasons.Where(x => x.Id == reasonId && x.Active).FirstOrDefaultAsync();
        }

        public Task<RefServiceType> GetServiceTypeName(int serviceTypeId)
        {
            return _context.RefServiceTypes.Where(x => x.Id == serviceTypeId && x.Active).FirstOrDefaultAsync();
        }

        public Task<CuBrand> GetBrand(int id)
        {
            return _context.CuBrands.Where(x => x.Id == id && x.Active).FirstOrDefaultAsync();
        }

        public Task<CuDepartment> GetDepartment(int id)
        {
            return _context.CuDepartments.Where(x => x.Id == id && x.Active).FirstOrDefaultAsync();
        }

        public Task<CuBuyer> GetBuyer(int id)
        {
            return _context.CuBuyers.Where(x => x.Id == id && x.Active).FirstOrDefaultAsync();
        }

        public Task<List<InspTransaction>> GetInspectionList(IEnumerable<int> LstinspectionID)
        {
            return _context.InspTransactions.Where(x => LstinspectionID.Contains(x.Id))
                      .Include(x => x.Factory)
                      .Include(x => x.InspTranServiceTypes)
                      .Include(x => x.InspProductTransactions)
                        .ThenInclude(x => x.Product)
                       .Include(x => x.InspProductTransactions)
                       .ThenInclude(x => x.InspPurchaseOrderTransactions)
                               .ThenInclude(x => x.Po)
                      .Include(x => x.InspTranCuContacts)
                      .Include(x => x.InspTranSuContacts)
                      .Include(x => x.InspTranFaContacts)
                      .Include(x => x.InspTranCuBrands)
                      .Include(x => x.InspTranCuBuyers)
                      .Include(x => x.InspTranCuDepartments).ToListAsync();
        }

        /// <summary>
        /// GetCustomersByCustomerId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CuCheckPoint> GetCustomersByCustomerId(int customerId)
        {
            return await _context.CuCheckPoints
                .Include(x => x.CuCheckPointsBrands)
                .Include(x => x.CuCheckPointsDepartments)
                .Include(x => x.CuCheckPointsServiceTypes)
                .Where(x => x.Active && x.CustomerId == customerId && x.ServiceId == (int)Service.InspectionId &&
                       x.CheckpointTypeId == (int)CheckPointTypeEnum.QuotationRequired).FirstOrDefaultAsync();
        }

        public IQueryable<InspTransaction> GetExcludedInspectionByStatus(int statusId)
        {
            return _context.InspTransactions.
                    Include(x => x.InspProductTransactions)
                    .ThenInclude(x => x.InspPurchaseOrderTransactions)
                    .Where(x => x.StatusId != statusId);
        }

        public List<CuContactBrand> GetCustomerContactByBrandId(int? brandId)
        {
            return _context.CuContactBrands.Where(x => x.BrandId == brandId && x.Active == true).ToList();
        }

        public List<CuContactDepartment> GetCustomerContactByDeptId(int? departmentId)
        {
            return _context.CuContactDepartments.Where(x => x.DepartmentId == departmentId && x.Active == true).ToList();
        }

        public IEnumerable<ItUserRole> GetUsersBasedOnRole(int roleId, int? userlocation)
        {
            return _context.ItUserRoles.Where(x => x.RoleId == roleId)?.
                  Include(x => x.User)?.
                  ThenInclude(x => x.Staff)?.
                  ThenInclude(x => x.HrOfficeControls).
                  Where(x => x.User.UserTypeId == (int)UserTypeEnum.InternalUser);
        }

        //Get task list based on booking id, tasktype id, isdone value
        public async Task<IEnumerable<MidTask>> GetTask(int bookingId, IEnumerable<int> typeIdList, bool isdone)
        {
            return await _context.MidTasks.Where(x => x.LinkId == bookingId && typeIdList.Contains(x.TaskTypeId) && x.IsDone == isdone).ToListAsync();
        }

        public async Task<int> GetLastStatus(int bookingId)
        {
            return await _context.InspTranStatusLogs.Where(x => x.BookingId == bookingId).OrderByDescending(x => x.StatusChangeDate).Select(x => x.StatusId).Skip(1).FirstOrDefaultAsync();
        }

        public async Task<BookingDate> getInspBookingDateDetails(int bookingId)
        {
            return await _context.InspTransactions.Where(x => x.Id == bookingId).Select(x => new BookingDate
            {
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo
            }).FirstOrDefaultAsync();
        }
        public async Task<List<BookingDate>> getListInspBookingDateDetails(IEnumerable<int> lstbookingId)
        {
            return await _context.InspTransactions.Where(x => lstbookingId.Contains(x.Id)).Select(x => new BookingDate
            {
                BookingId = x.Id,
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo
            }).ToListAsync();
        }
        public async Task<IEnumerable<CustomerCSLocation>> GetCSLocationList(IEnumerable<int> customerId)
        {
            return await _context.DaUserCustomers
                .Where(x => x.UserType == (int)HRProfile.CS &&
                customerId.Contains(x.CustomerId.Value))
                .Select(x => new CustomerCSLocation
                {
                    CustomerId = x.CustomerId.GetValueOrDefault(),
                    LocationList = x.User.Staff.HrOfficeControls,
                    CsName = x.User.Staff.PersonName,
                    CsId = x.User.StaffId,
                    CscompanyPhone = x.User.Staff.CompanyMobileNo
                })
                .ToListAsync();
        }

        public async Task<BookingDate> GetLastServiceDate(int bookingId)
        {
            return await _context.InspTranStatusLogs.Where(x => x.BookingId == bookingId).OrderByDescending(x => x.StatusChangeDate).Skip(1).Select(x => new BookingDate
            {
                ServiceDateFrom = x.ServiceDateFrom.GetValueOrDefault(),
                ServiceDateTo = x.ServiceDateTo.GetValueOrDefault()
            }).AsNoTracking().FirstAsync();
        }

        public async Task<InspTransaction> GetInspectionCustomerContactByID(int inspectionID)
        {
            return await _context.InspTransactions.Where(x => x.Id == inspectionID)
                .Include(x => x.InspTranCuContacts)
                .ThenInclude(x => x.Contact)
                .Include(x => x.CreatedByNavigation)
                .FirstAsync();
        }

        public async Task<IEnumerable<PoDetails>> GetBookingPOTransactionDetails(List<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.ProductRef.Active.HasValue && x.ProductRef.Active.Value && x.Active.HasValue && x.Active.Value && bookingIds.Contains(x.InspectionId)).
                Select(p => new PoDetails
                {
                    PoTransactionId = p.Id,
                    BookingId = p.InspectionId,
                    FactoryReference = p.ProductRef.Product.FactoryReference,
                    ProductId = p.ProductRef.Product.ProductId,
                    PoNumber = p.Po.Pono,
                    PickingQuantity = p.PickingQuantity,
                    ProductTransId = p.ProductRefId,
                    ReportId = p.ProductRef.FbReportId,
                    IsEcoPack = p.ProductRef.IsEcopack,
                    Barcode = p.ProductRef.Product.Barcode,
                    CombineProductId = p.ProductRef.CombineProductId
                }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EaqfReportDetails>> GetEaqfBookingReportDetails(List<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value && bookingIds.Contains(x.InspectionId)).
                Select(x => new EaqfReportDetails
                {
                    BookingId = x.InspectionId,
                    ProductReference = x.Product.ProductId,
                    ProductReferenceDescription = x.Product.ProductDescription,
                    PoNumber = string.Join(',', x.InspPurchaseOrderTransactions.Where(x => x.Active.Value).Select(x => x.Po.Pono).ToList()),
                    ReportTitle = x.FbReport.ReportTitle,
                    ReportLink = x.FbReport.FinalReportPath,
                    ReportResult = x.FbReport.Result.ResultName,
                    ReportStatus = x.FbReport.FbReportStatusNavigation.StatusName
                }).AsSingleQuery().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EaqfInvoiceDetails>> GetEaqfBookingInvoiceDetails(List<int> bookingIds)
        {
            return await _context.InvManTransactions.Where(x => x.Status != 4 && bookingIds.Contains(x.BookingNo.GetValueOrDefault())).
                Select(x => new EaqfInvoiceDetails
                {
                    BookingId = x.BookingNo.GetValueOrDefault(),
                    InvoiceTotal = x.TotalAmount.GetValueOrDefault(),
                    InvoiceNo = x.InvoiceNo
                }).AsSingleQuery().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EaqfInvoiceDetails>> GetEaqfBookingInvoiceFileDetails(List<string> invoiceNoList)
        {
            return await _context.InvTranFiles.Where(x => x.Active.Value && invoiceNoList.Contains(x.InvoiceNo)).
                Select(x => new EaqfInvoiceDetails
                {
                    InvoiceNo = x.InvoiceNo,
                    InvoicePdfUrl = x.FilePath
                }).AsSingleQuery().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<PoDetails>> GetBookingQuotationDetails(List<int> poTransactionIds)
        {
            return await _context.QuInspProducts.Where(x => poTransactionIds.Contains(x.ProductTranId))
                .Select(x => new PoDetails
                {
                    PoTransactionId = x.ProductTranId,
                    QuotationStatus = x.IdQuotationNavigation.IdStatus,
                    BookingId = x.ProductTran.InspectionId,
                    QuotationStatusName = x.IdQuotationNavigation.IdStatusNavigation.Label
                }).AsNoTracking().ToListAsync();
        }
        public async Task<List<PoDetails>> GetBookingQuotationDetailsbybookingId(List<int> lstbookingid)
        {
            return await _context.QuQuotationInsps.Where(x => lstbookingid.Contains(x.IdBooking))
                .Select(x => new PoDetails
                {
                    QuotationStatus = x.IdQuotationNavigation.IdStatus,
                    BookingId = x.IdBooking,
                    QuotationStatusName = x.IdQuotationNavigation.IdStatusNavigation.Label,
                    quotCreatedDate = x.IdQuotationNavigation.CreatedDate,
                    Manday = x.NoOfManDay
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<PoDetails>> GetBookingQuotationDetailsbybookingIdQuery(IQueryable<int> lstbookingid)
        {
            return await _context.QuQuotationInsps.Where(x => lstbookingid.Contains(x.IdBooking))
                .Select(x => new PoDetails
                {
                    QuotationStatus = x.IdQuotationNavigation.IdStatus,
                    BookingId = x.IdBooking,
                    QuotationStatusName = x.IdQuotationNavigation.IdStatusNavigation.Label,
                    quotCreatedDate = x.IdQuotationNavigation.CreatedDate,
                    CalculatedWorkingHours = x.CalculatedWorkingHrs,
                    Manday = x.NoOfManDay,
                    SuggestedManday = x.SuggestedManday
                }).AsNoTracking().ToListAsync();
        }

        //Get the service Type of each booking. 
        public async Task<IEnumerable<ServiceTypeList>> GetServiceType(IEnumerable<int> bookingId)
        {
            return await _context.InspTranServiceTypes
                  .Where(x => bookingId.Contains(x.InspectionId) && x.Active)
                  .Select(x => new ServiceTypeList
                  {
                      InspectionId = x.InspectionId,
                      serviceTypeId = x.ServiceTypeId,
                      serviceTypeName = x.ServiceType.Name,
                      IsAutoQCExpenseClaim = x.ServiceType.IsAutoQcexpenseClaim.GetValueOrDefault()
                  }).AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<ServiceTypeList>> GetProductCategoryList(IEnumerable<int> bookingId)
        {
            return await _context.InspTranServiceTypes
                  .Where(x => bookingId.Contains(x.InspectionId) && x.Active)
                  .Select(x => new ServiceTypeList
                  {
                      InspectionId = x.InspectionId,
                      serviceTypeId = x.ServiceTypeId,
                      serviceTypeName = x.ServiceType.Name,
                      IsAutoQCExpenseClaim = x.ServiceType.IsAutoQcexpenseClaim.GetValueOrDefault()
                  }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ServiceTypeList>> GetServiceTypeList(IQueryable<int> bookingId)
        {
            return await _context.InspTranServiceTypes
                  .Where(x => bookingId.Contains(x.InspectionId) && x.Active)
                  .Select(x => new ServiceTypeList
                  {
                      InspectionId = x.InspectionId,
                      serviceTypeId = x.ServiceTypeId,
                      serviceTypeName = x.ServiceType.Name
                  }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get Audit service types
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ServiceTypeList>> GetAuditServiceType(IEnumerable<int> bookingIds)
        {
            return await _context.AudTranServiceTypes
                  .Include(x => x.ServiceType)
                  .Where(x => bookingIds.Contains(x.AuditId) && x.Active)
                  .Select(x => new ServiceTypeList
                  {
                      AuditId = x.AuditId,
                      serviceTypeId = x.ServiceTypeId,
                      serviceTypeName = x.ServiceType.Name
                  }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get Audit service types
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ServiceTypeList>> GetQueryableAuditServiceType(IQueryable<int> bookingIds)
        {
            return await _context.AudTranServiceTypes
                  .Include(x => x.ServiceType)
                  .Where(x => bookingIds.Contains(x.AuditId) && x.Active)
                  .Select(x => new ServiceTypeList
                  {
                      AuditId = x.AuditId,
                      serviceTypeId = x.ServiceTypeId,
                      serviceTypeName = x.ServiceType.Name
                  }).AsNoTracking().ToListAsync();
        }

        public async Task<List<FactoryCountry>> GetFactorycountryId(IEnumerable<int> bookingIds)
        {
            return await (from fact in _context.SuAddresses
                          join insp in _context.InspTransactions on fact.SupplierId equals insp.FactoryId
                          where bookingIds.Contains(insp.Id) && fact.AddressTypeId == (int)Supplier_Address_Type.HeadOffice
                          select new FactoryCountry
                          {
                              FactoryCountryId = fact.CountryId,
                              FactoryProvinceId = fact.RegionId,
                              FactoryCityId = fact.CityId,
                              FactoryCountyId = fact.CountyId.GetValueOrDefault(),
                              FactoryZoneId = fact.County.ZoneId.GetValueOrDefault(),
                              BookingId = insp.Id,
                              FactoryAdress = fact.Address,
                              FactoryRegionalAddress = fact.LocalLanguage,
                              CountryName = fact.Country.CountryName,
                              ProvinceName = fact.Region.ProvinceName,
                              CityName = fact.City.CityName,
                              CountyName = fact.County.CountyName,
                              ZoneName = fact.County.Zone.Name,
                              TownName = fact.Town.TownName,
                              TownId = fact.TownId.GetValueOrDefault()
                          }).ToListAsync();
        }


        /// <summary>
        /// Get Booking Data by booking id.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<BookingData> GetBookingData(int bookingId)
        {
            return await _context.InspTransactions.Where(x => x.Id == bookingId).Select(x => new BookingData
            {
                BookingId = x.Id,
                CombinedCount = 0,
                CustomerId = x.CustomerId,
                FactoryName = x.Factory.SupplierName,
                SupplierName = x.Supplier.SupplierName,
                SupplierAddress = x.Supplier.SuAddresses.Where(y => y.Address != null).Select(y => y.Address).FirstOrDefault(),
                FactoryAddress = x.Factory.SuAddresses.Where(y => y.Address != null).Select(y => y.Address).FirstOrDefault(),
                InspectionType = x.InspTranServiceTypes.Where(y => y.Active).Select(y => y.ServiceType.Name).FirstOrDefault(),
                InspectionTypeId = x.InspTranServiceTypes.Where(y => y.Active).Select(y => y.ServiceTypeId).FirstOrDefault(),
                supplierId = x.SupplierId,
                factoryId = x.FactoryId,
                CustomerName = x.Customer.CustomerName
            }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the booking service type ids
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingServiceTypes(int bookingId)
        {
            return await _context.InspTranServiceTypes.Where(y => y.Active && y.InspectionId == bookingId).Select(y => y.ServiceTypeId).ToListAsync();
        }

        /// <summary>
        /// Get the booking brand ids
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingBrands(int bookingId)
        {
            return await _context.InspTranCuBrands.Where(y => y.Active && y.InspectionId == bookingId).Select(y => y.BrandId).ToListAsync();
        }

        /// <summary>
        /// Get the booking department ids
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingDepartments(int bookingId)
        {
            return await _context.InspTranCuDepartments.Where(y => y.Active && y.InspectionId == bookingId).Select(y => y.DepartmentId).ToListAsync();
        }

        /// <summary>
        /// Get Products and Report List by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReportProductsData>> GetReportsProductListByBooking(int bookingId, int reportId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && y.InspectionId == bookingId && y.FbReportId == reportId).Select(z => new ReportProductsData()
            {
                InspectionPoId = z.Id,
                ProductId = z.ProductId,
                ProductName = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                BookingQuantity = z.InspPurchaseOrderTransactions.Where(c => c.InspectionId == bookingId).SelectMany(x => x.FbReportQuantityDetails.Where(y => y.FbReportDetailId == reportId && y.Active.Value)).Sum(r => r.OrderQuantity),
                PresentedQuantity = z.InspPurchaseOrderTransactions.Where(c => c.InspectionId == bookingId).SelectMany(x => x.FbReportQuantityDetails.Where(y => y.FbReportDetailId == reportId && y.Active.Value)).Select(r => r.PresentedQuantity).Sum(),
                InspectedQuantity = z.InspPurchaseOrderTransactions.Where(c => c.InspectionId == bookingId).SelectMany(x => x.FbReportQuantityDetails.Where(y => y.FbReportDetailId == reportId && y.Active.Value)).Select(r => r.InspectedQuantity).Sum(),
                Major = z.InspPurchaseOrderTransactions.Where(c => c.InspectionId == bookingId).SelectMany(x => x.FbReportInspDefects.Where(y => y.FbReportDetailId == reportId && y.Active.Value)).Select(x => x.Major).Sum(),
                Minor = z.InspPurchaseOrderTransactions.Where(c => c.InspectionId == bookingId).SelectMany(x => x.FbReportInspDefects.Where(y => y.FbReportDetailId == reportId && y.Active.Value)).Select(x => x.Minor).Sum(),
                Critical = z.InspPurchaseOrderTransactions.Where(c => c.InspectionId == bookingId).SelectMany(x => x.FbReportInspDefects.Where(y => y.FbReportDetailId == reportId && y.Active.Value)).Select(x => x.Critical).Sum(),
                //DestinationCountry = string.Join(',', z.InspPurchaseOrderTransactions.Where(c => c.InspectionId == bookingId).Select(x => x.DestinationCountry.CountryName).Distinct().ToArray()),
                CombineProductId = z.CombineProductId,
                CombineAql = z.CombineAqlQuantity
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get Product List by booking id 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingProductsData>> GetProductListByBooking(IEnumerable<int> bookingId)
        {
            int msFileType = (int)ProductRefFileType.MSChartExcel;
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new BookingProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductId = z.Product.ProductId,
                ProductName = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                BookingQuantity = z.TotalBookingQuantity,
                BookingStatus = z.Inspection.StatusId,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.Product.ProductCategorySub2Navigation.Name,
                ReportResult = z.FbReport.Result.ResultName,
                ReportResultId = z.FbReport.ResultId,
                ReportId = z.FbReport.Id,
                ReportStatus = z.FbReport.FbReportStatusNavigation.StatusName,
                CreatedDate = z.Inspection.CreatedOn,
                ServiceStartDate = z.FbReport.ServiceFromDate,
                ServiceEndDate = z.FbReport.ServiceToDate,
                UpdatedDate = z.Inspection.UpdatedOn,
                ProductImage = z.FbReport.MainProductPhoto,
                CombineProductId = z.CombineProductId.GetValueOrDefault(),
                CombineAqlQuantity = z.CombineAqlQuantity.GetValueOrDefault(),
                ReportPath = z.FbReport.FinalReportPath,
                FinalManualReportPath = z.FbReport.FinalManualReportPath,
                FactoryReference = z.Product.FactoryReference,
                InspectedQuantity = z.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value).SelectMany(x => x.FbReportQuantityDetails.Where(y => y.Active.Value)).Select(r => r.InspectedQuantity).Sum(),
                AqlQuantity = z.AqlQuantity,
                ReportTitle = z.FbReport.ReportTitle,
                Unit = z.Unit,
                UnitCount = z.UnitCount,
                UnitName = z.UnitNavigation.Name,
                IsMSChart = z.Product.CuProductFileAttachments.Where(x => x.Active).Any(y => y.FileTypeId == msFileType)
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking quantity details by booking id list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingQuantityData>> GetBookingQuantityDetails(IEnumerable<int> bookingId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new BookingQuantityData()
            {
                ProductRefId = z.Id,
                BookingId = z.InspectionId,
                ProductId = z.Product.Id,
                ProductName = z.Product.ProductId,
                BookingQuantity = z.TotalBookingQuantity,
                CombineAqlQuantity = z.CombineAqlQuantity,
                CombineProductId = z.CombineProductId,
                AqlQuantity = z.AqlQuantity,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategoryId = z.Product.ProductSubCategory,
                ProductSub2Category = z.Product.ProductCategorySub2Navigation.Name,
                ProductSub2CategoryId = z.Product.ProductCategorySub2,
                UnitId = z.Unit,
                FbReportId = z.FbReportId
            }).AsNoTracking().ToListAsync();
        }


        public async Task<List<InvoiceBookingDetail>> GetOrdersOnSameFactoryAndSameDate(List<int> bookingIds, int customerId, int factoryId, DateTime serviceDate)
        {
            var existingInvoiceIds = await _context.InvAutTranDetails.Where(x => x.Inspection.CustomerId == customerId &&
                                   x.Inspection.FactoryId == factoryId && x.Inspection.ServiceDateTo == serviceDate).Select(x => x.Id).ToListAsync();

            var existingOrdersOnSameFactAndDate = await _context.InvAutTranDetails.Where(x => x.Inspection.CustomerId == customerId &&
                                   x.Inspection.FactoryId == factoryId && x.Inspection.ServiceDateTo == serviceDate &&
                                   !existingInvoiceIds.Contains(x.Id) && !bookingIds.Contains(x.Inspection.Id)
                                   ).
                        Select(x => new InvoiceBookingDetail
                        {
                            CustomerId = x.Inspection.CustomerId,
                            SupplierId = x.Inspection.SupplierId,
                            FactoryId = x.Inspection.FactoryId,
                            BookingId = x.Inspection.Id,
                            CustomerBookingNo = x.Inspection.CustomerBookingNo,
                            StatusId = x.Inspection.StatusId,
                            PriceCategoryId = x.Inspection.PriceCategoryId.GetValueOrDefault(),
                            ServiceFrom = x.Inspection.ServiceDateFrom,
                            ServiceTo = x.Inspection.ServiceDateTo,
                            OfficeId = x.Inspection.OfficeId.GetValueOrDefault()
                        }).OrderByDescending(x => x.ServiceTo).ToListAsync();

            return existingOrdersOnSameFactAndDate;
        }

        public async Task<IEnumerable<BookingQuantityData>> GetContainerReports(IEnumerable<int> bookingId)
        {
            return await _context.InspContainerTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new BookingQuantityData()
            {
                BookingId = z.InspectionId,
                ContainerId = z.ContainerId
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the report quantity details
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingReportQuantityData>> GetBookingReportQuantityDetails(IEnumerable<int> bookingId)
        {
            return await _context.FbReportDetails.Where(y =>
             bookingId.Contains(y.InspectionId.GetValueOrDefault()) && y.Active.Value).Select(z => new BookingReportQuantityData()
             {
                 BookingId = z.InspectionId.GetValueOrDefault(),
                 ReportId = z.Id,
                 ReportName = z.ReportTitle,
                 PresentedQuantity = z.PresentedQty,
                 InspectedQuantity = z.InspectedQty,
                 OrderQuantity = z.OrderQty,
             }).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Get the po transaction list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingPoTransaction>> GetBookingPoList(List<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.
                                Where(x => x.Active.Value && bookingIds.Contains(x.InspectionId)).
                                Select(x => new BookingPoTransaction()
                                {
                                    Id = x.Id,
                                    PoNumber = x.Po.Pono,
                                    InspectionId = x.InspectionId,
                                    DestinationCountry = x.DestinationCountry.CountryName,
                                    ProductRefId = x.ProductRefId,
                                    ContainerRefId = x.ContainerRefId.GetValueOrDefault()
                                }).ToListAsync();
        }

        public async Task<IEnumerable<ScheduleProductsData>> GetScheduleProductListByBooking(IEnumerable<int> bookingId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
                        && bookingId.Contains(y.InspectionId)).Select(z => new ScheduleProductsData()
                        {
                            Id = z.Id,
                            BookingId = z.InspectionId,
                            ProductName = z.Product.ProductId,
                            CombineProductId = z.CombineProductId,
                            CombineAqlQuantity = z.CombineAqlQuantity,
                            AqlQuantity = z.AqlQuantity,
                            ReportId = z.FbReport.FbReportMapId
                        }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ScheduleProductsData>> GetScheduleProductListByBookingQuery(IQueryable<int> bookingId)
        {
            int msFileType = (int)ProductRefFileType.MSChartExcel;

            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new ScheduleProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductName = z.Product.ProductId,
                ProductId = z.Product.Id,
                IsMSChart = z.Product.CuProductFileAttachments.Where(x => x.Active).Any(y => y.FileTypeId == msFileType),
                CombineProductId = z.CombineProductId.GetValueOrDefault(),
                CombineAqlQuantity = z.CombineAqlQuantity.GetValueOrDefault(),
                AqlQuantity = z.AqlQuantity,
                Unit = z.UnitNavigation.Name,
                OrderQty = z.TotalBookingQuantity,
                ProductDescription = z.Product.ProductDescription,
                ReportId = z.FbReport.FbReportMapId,
                ProductRefId = z.Id,
                InspectedQty = z.FbReport.InspectedQty
            }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ScheduleProductsData>> GetScheduleProductListByBooking(IQueryable<int> bookingId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new ScheduleProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductName = z.Product.ProductId,
                CombineProductId = z.CombineProductId.GetValueOrDefault(),
                CombineAqlQuantity = z.CombineAqlQuantity.GetValueOrDefault(),
                AqlQuantity = z.AqlQuantity,
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get container list by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingContainersRepo>> GetContainerListByBooking(IEnumerable<int> bookingId)
        {
            return await _context.InspContainerTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new BookingContainersRepo()
            {
                Id = z.Id,
                ContainerId = z.ContainerId,
                BookingId = z.InspectionId,
                TotalBookingQuantity = z.TotalBookingQuantity,
                BookingStatus = z.Inspection.StatusId,
                ReportResult = z.FbReport.Result.ResultName,
                ReportResultId = z.FbReport.ResultId,
                ReportId = z.FbReport.FbReportMapId,
                ApiReportId = z.FbReport.Id,
                ReportStatus = z.FbReport.FbReportStatusNavigation.StatusName,
                CreatedDate = z.Inspection.CreatedOn,
                ServiceStartDate = z.FbReport.ServiceFromDate,
                ServiceEndDate = z.FbReport.ServiceToDate,
                UpdatedDate = z.Inspection.UpdatedOn,
                ReportPath = z.FbReport.FinalReportPath,
                FinalManualReportPath = z.FbReport.FinalManualReportPath,
                ReportTitle = z.FbReport.ReportTitle,
                ReportStatusId = z.FbReport.FbReportStatus,
                ReviewResultId = z.FbReport.FbReviewStatus,
                FillingStatusId = z.FbReport.FbFillingStatus,
                ReviewStatus = z.FbReport.FbReviewStatusNavigation.StatusName,
                FillingStatus = z.FbReport.FbFillingStatusNavigation.StatusName,
                ContainerSize = z.ContainerSizeNavigation.Name,
                FinalReportLink = z.FbReport.FinalReportPath,
                InspectedQuantity = z.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value).SelectMany(x => x.FbReportQuantityDetails.Where(y => y.Active.Value)).Select(r => r.InspectedQuantity).Sum()
            }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ScheduleContainersRepo>> GetScheduleContainerListByBooking(IEnumerable<int> bookingId)
        {
            return await _context.InspContainerTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new ScheduleContainersRepo()
            {
                Id = z.Id,
                ContainerId = z.ContainerId,
                BookingId = z.InspectionId,
                ReportId = z.FbReport.FbReportMapId
            }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ScheduleContainersRepo>> GetScheduleContainerListByBookingQuery(IQueryable<int> bookingId)
        {
            return await _context.InspContainerTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new ScheduleContainersRepo()
            {
                Id = z.Id,
                ContainerId = z.ContainerId,
                BookingId = z.InspectionId,
                ReportId = z.FbReport.FbReportMapId
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get container list by booking and container
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="conatinerId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReportProductsData>> GetContainerListByBooking(IEnumerable<int> bookingId, int conatinerId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.Value && bookingId.Contains(x.InspectionId)
            && x.ContainerRef.ContainerId == conatinerId)

            .Select(z => new ReportProductsData()
            {
                ContainerId = z.ContainerRefId.GetValueOrDefault(),
                ReportId = z.ContainerRef.FbReport.Id,
                ProductName = z.ProductRef.Product.ProductId,
                ProductDescription = z.ProductRef.Product.ProductDescription,
                BookingQuantity = z.ContainerRef.TotalBookingQuantity,
                PresentedQuantity = z.FbReportQuantityDetails.Where(y => y.Active.Value).Select(r => r.PresentedQuantity).Sum(),
                InspectedQuantity = z.FbReportQuantityDetails.Where(y => y.Active.Value).Select(r => r.InspectedQuantity).Sum(),
                Major = z.FbReportInspDefects.Where(y => y.Active.Value).Select(x => x.Major).Sum(),
                Minor = z.FbReportInspDefects.Where(y => y.Active.Value).Select(x => x.Minor).Sum(),
                Critical = z.FbReportInspDefects.Where(y => y.Active.Value).Select(x => x.Critical).Sum(),

            }).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// get product po list by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingProductsExportData>> GetProductPoListByBooking(IEnumerable<int> bookingId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingId.Contains(y.InspectionId)).Select(z => new BookingProductsExportData()
            {
                BookingId = z.InspectionId,
                ProductId = z.ProductRef.Product.ProductId,
                ProductName = z.ProductRef.Product.ProductId,
                PoNumber = z.Po.Pono,
                ProductDescription = z.ProductRef.Product.ProductDescription,
                BookingQuantity = z.BookingQuantity,
                BookingStatus = z.Inspection.StatusId,
                ProductCategory = z.ProductRef.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.ProductRef.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.ProductRef.Product.ProductCategorySub2Navigation.Name,
                ContainerId = z.ContainerRef.ContainerId,
                CreatedDate = z.Inspection.CreatedOn,
                ServiceStartDate = z.ProductRef.FbReport.ServiceFromDate,
                ServiceEndDate = z.ProductRef.FbReport.ServiceToDate,
                UpdatedDate = z.Inspection.UpdatedOn,
                ProductImage = z.ProductRef.FbReport.MainProductPhoto,
                CombineProductId = z.ProductRef.CombineProductId.GetValueOrDefault(),
                CombineAqlQuantity = z.ProductRef.CombineAqlQuantity.GetValueOrDefault(),
                FactoryReference = z.ProductRef.Product.FactoryReference,
                Etd = z.Etd ?? null,
                SRDate = z.Etd.HasValue ? z.Etd.GetValueOrDefault().AddDays(-10) : z.Etd ?? null,
                DestinationCountry = z.DestinationCountry.CountryName,
                FbReportId = z.ProductRef.FbReportId.GetValueOrDefault(),
                FbContainerReportId = z.ContainerRef.FbReportId.GetValueOrDefault(),
                AqlQty = z.ProductRef.AqlQuantity,
                Barcode = z.ProductRef.Product.Barcode,
                IsNewProduct = z.ProductRef.Product.IsNewProduct.HasValue && z.ProductRef.Product.IsNewProduct.Value ? true : false,
                IsEcoPack = z.ProductRef.IsEcopack,
                Critical = z.ProductRef.CriticalNavigation.Value,
                Major = z.ProductRef.MajorNavigation.Value,
                Minor = z.ProductRef.MinorNavigation.Value,
                PickingQty = z.PickingQuantity,
                Unit = z.ProductRef.UnitNavigation.Name,
                Remarks = z.ProductRef.Remarks,
                DisplayMaster = z.ProductRef.IsDisplayMaster,
                DisplayChild = z.ProductRef.ParentProduct.ProductId,
                Aql = z.ProductRef.AqlNavigation.Value,
                ProductSerialNo = z.ProductRef.BookingFormSerial
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get booking product po list by booking id and product ref id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="productRefId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingPoData>> GetProductPOListByBooking(IEnumerable<int> bookingId, int productRefId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
              && bookingId.Contains(y.InspectionId) && y.ProductRefId == productRefId).Select(z => new BookingPoData()
              {
                  Id = z.Id,
                  BookingId = z.InspectionId,
                  PoNumber = z.Po.Pono,
                  BookingQuantity = z.BookingQuantity,
                  InspectedQuantity = z.FbReportQuantityDetails.Where(x => x.Active.Value).FirstOrDefault().InspectedQuantity,
                  Etd = z.Etd ?? null,
                  SRDate = z.Etd.HasValue ? z.Etd.GetValueOrDefault().AddDays(-10) : z.Etd ?? null,
                  DestinationCountry = z.DestinationCountry.CountryName

              }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get container product list by booking and container ref
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="containerRefId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingProductsData>> GetContainerProductListByBooking(IEnumerable<int> bookingId, int containerRefId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
              && bookingId.Contains(y.InspectionId) && y.ContainerRefId == containerRefId).Select(z => new BookingProductsData()
              {
                  Id = z.ProductRef.Id,
                  BookingId = z.InspectionId,
                  ProductId = z.ProductRef.Product.ProductId,
                  ProductName = z.ProductRef.Product.ProductId,
                  ProductDescription = z.ProductRef.Product.ProductDescription,
                  ProductCategory = z.ProductRef.Product.ProductCategoryNavigation.Name,
                  ProductSubCategory = z.ProductRef.Product.ProductSubCategoryNavigation.Name,
                  ProductSubCategory2 = z.ProductRef.Product.ProductCategorySub2Navigation.Name,
                  FactoryReference = z.ProductRef.Product.FactoryReference
              }).Distinct().AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Get container Po list by booking and product and container
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="containerRefId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingPoData>> GetContainerPoListByBooking(IEnumerable<int> bookingId, int containerRefId, int productRefId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
               && bookingId.Contains(y.InspectionId) && y.ContainerRefId == containerRefId &&
               y.ProductRefId == productRefId).Select(z => new BookingPoData()
               {
                   Id = z.Id,
                   BookingId = z.InspectionId,
                   PoNumber = z.Po.Pono,
                   BookingQuantity = z.BookingQuantity,
                   InspectedQuantity = z.FbReportQuantityDetails.Where(x => x.Active.HasValue && x.Active.Value).FirstOrDefault().InspectedQuantity,
                   Etd = z.Etd ?? null,
                   SRDate = z.Etd.HasValue ? z.Etd.GetValueOrDefault().AddDays(-10) : z.Etd ?? null,
                   DestinationCountry = z.DestinationCountry.CountryName
               }).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Get Report details based on the Report Id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<FBReport> GetFbReports(int reportId)
        {
            return await _context.FbReportDetails.Where(x => x.Id == reportId).Select(x => new FBReport
            {
                ReportPath = x.FinalReportPath,
                FinalManualReportPath = x.FinalManualReportPath,
                ReportResult = x.Result.ResultName,
                ReportTitle = x.ReportTitle,
                ReportResultId = x.ResultId,
                ReportPhoto = x.MainProductPhoto,
                ReportStatus = x.FbReportStatusNavigation.StatusName,
                StartDate = x.ServiceFromDate,
                ToDate = x.ServiceToDate
                //AdditionalPhotos = x.FbReportAdditionalPhotos.Select(y => y.PhotoPath).ToList()
            }).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Get Report details based on the fbReportId
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<FBReport_Detail> GetFbReportsDetail(int fbReportId)
        {
            return await _context.FbReportDetails.Where(x => x.Id == fbReportId).Select(x => new FBReport_Detail
            {
                FBReportId = x.Id,
                FBReportMapId = x.FbReportMapId,
                ReportTitle = x.ReportTitle,
                MissionTitle = x.MissionTitle,
                OverAllResult = x.OverAllResult

            }).FirstOrDefaultAsync();
        }
        public async Task<List<string>> GetFBReportAdditionalPhotos(int reportId)
        {
            return await _context.FbReportAdditionalPhotos.Where(x => x.FbReportDetailId == reportId && x.Active.Value).
                            Select(x => x.PhotoPath).ToListAsync();
        }


        /// <summary>
        /// Customer decision status
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<ReportCustomerDecision> GetReportCustomerDecision(int reportId)
        {
            return await _context.InspRepCusDecisions.Where(x => x.ReportId == reportId &&
            x.Active.HasValue && x.Active.Value).Select(x => new ReportCustomerDecision
            {
                CustomerDecisionCustomStatus = x.CustomerResult.CustomDecisionName,
                CustomerDecisionStatus = x.CustomerResult.CusDec.Name,
                Comments = x.Comments,
                CustomerResultId = x.CustomerResult.CusDecId,
                ReportId = reportId
            }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Check Point count
        /// </summary>
        /// <param name="cusId"></param>
        /// <param name="serviceId"></param>
        /// <param name="checkPointType"></param>
        /// <returns></returns>
        public async Task<int> GetCusCPByCusServiceId(int? cusId, int? serviceId, int checkPointType)
        {
            return await _context.CuCheckPoints.Where(x => x.Active && x.CustomerId == cusId
                         && x.ServiceId == serviceId && x.CheckpointTypeId == checkPointType).Select(x => x.Id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Inspection summary by report Id and its type
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InspectionReportSummary>> GetInspectionSummary(int reportId)
        {
            return await _context.FbReportInspSummaries.Where(y => y.Active.HasValue && y.Active.Value
            && y.FbReportDetailId == reportId && y.FbReportInspsumTypeId == (int)InspSummaryType.Main).OrderBy(c => c.Sort).Select(z => new InspectionReportSummary()
            {
                Id = z.Id,
                Name = z.Name,
                //Photos = z.FbReportInspSummaryPhotos.Select(x => x.Photo).ToList(),
                PhotoCount = z.FbReportInspSummaryPhotos.Count(),
                Remarks = z.Remarks,
                FbReportDetailId = z.FbReportDetailId,
                Result = z.ResultNavigation.ResultName,
                ResultId = z.ResultId,
                ProblematicRemarksCount = z.FbReportProblematicRemarks.Count(x => x.Active.Value)
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetInspectionSummaryPhoto(int reportId)
        {
            return await _context.FbReportInspSummaryPhotos.Where(y => y.Active.HasValue && y.Active.Value
            && y.FbReportSummary.FbReportDetailId == reportId && y.FbReportSummary.FbReportInspsumTypeId == (int)InspSummaryType.Main).Select(z => new CommonDataSource()
            {
                Id = z.FbReportSummary.Id,
                Name = z.Photo
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Defect Details specific to report
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InspectionReportDefects>> GetInspectionDefects(int reportId)
        {
            return await _context.FbReportInspDefects.Where(y => y.Active.HasValue && y.Active.Value
            && y.FbReportDetailId == reportId).Select(z => new InspectionReportDefects()
            {
                Id = z.Id,
                Critical = z.Critical,
                Major = z.Major,
                Minor = z.Minor,
                Description = z.Description,
                Position = z.Position,
                FbReportDetailId = z.FbReportDetailId
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get defect details to specific products
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="inspPOId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InspectionReportDefects>> GetInspectionDefects(int reportId, IEnumerable<int> inspPOId)
        {
            return await _context.FbReportInspDefects.Where(y => y.Active.HasValue && y.Active.Value
            && y.FbReportDetailId == reportId && inspPOId.Contains(y.InspPoTransactionId)).Select(z => new InspectionReportDefects()
            {
                Id = z.Id,
                Critical = z.Critical,
                Major = z.Major,
                Minor = z.Minor,
                Description = z.Description,
                Position = z.Position,
                FbReportDetailId = z.FbReportDetailId
            }).AsNoTracking().ToListAsync();
        }


        //get PO transaction ids by product refid
        public async Task<IEnumerable<int>> GetPoTransactionIdsByProductRefId(int productRefId)
        {
            return await _context.InspPurchaseOrderTransactions.
                         Where(x => x.Active.HasValue && x.Active.Value && x.ProductRefId == productRefId)
                        .Select(y => y.Id)
                        .ToListAsync();
        }

        //get PO transaction ids by container refid
        public async Task<IEnumerable<int>> GetPoTransactionIdsByContainerRefId(int containerRefId)
        {
            return await _context.InspPurchaseOrderTransactions.
                         Where(x => x.Active.HasValue && x.Active.Value && x.ContainerRefId == containerRefId)
                        .Select(y => y.Id)
                        .ToListAsync();
        }

        //get insppotransaction list by inspPoTransIds
        public async Task<IEnumerable<InspProductTransaction>> GetInspPoDetails(IEnumerable<int> inspPoTransIds)
        {
            return await _context.InspProductTransactions.Where(x => inspPoTransIds.Contains(x.Id) && x.Active.HasValue && x.Active.Value).ToListAsync();
        }

        //Get product catgory details by booking id list
        public async Task<IEnumerable<BookingProductCategory>> GetProductCategoryDetails(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.Active.Value)
                .Select(x => new BookingProductCategory
                {
                    BookingId = x.InspectionId,
                    ProductCategoryId = x.Product.ProductCategoryNavigation.Id,
                    ProductCategoryName = x.Product.ProductCategoryNavigation.Name,
                    ProductCategorySubName = x.Product.ProductSubCategoryNavigation.Name,
                    ProductCategorySub2Name = x.Product.ProductCategorySub2Navigation.Name,
                    ProductCategorySubId = x.Product.ProductSubCategoryNavigation.Id,
                    ProductCategorySub2Id = x.Product.ProductCategorySub2Navigation.Id,
                    ProductCategorySub3Name = x.Product.ProductCategorySub3Navigation.Name,
                    ProductCategorySub3Id = x.Product.ProductCategorySub3Navigation.Id
                }).Distinct()
                .ToListAsync();
        }

        //Fetch the PO and Product details for client specific export
        public async Task<IEnumerable<ClientQuotationBookingItem>> GetProductListByBookingForClientQuotation(int bookingId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active == true && x.InspectionId == bookingId)
                    .Select(y => new ClientQuotationBookingItem
                    {
                        BookingId = bookingId,
                        PoId = y.Po.Id,
                        ServiceFromDate = y.Inspection.ServiceDateFrom,
                        ServiceToDate = y.Inspection.ServiceDateTo,
                        ProductCategory = y.ProductRef.Product.ProductCategorySub2Navigation.Name,
                        PONumber = y.Po.Pono,
                        POQuantity = y.ProductRef.TotalBookingQuantity,
                        CombineProductId = y.ProductRef.CombineProductId,
                        CombineAqlQty = y.ProductRef.CombineAqlQuantity,
                        AqlQty = y.ProductRef.AqlQuantity,
                        ProductId = y.ProductRef.ProductId,
                        AQL = y.ProductRef.AqlNavigation.Value,
                        Aql = y.ProductRef.Aql,
                        Critical = y.ProductRef.Critical.GetValueOrDefault(),
                        Minor = y.ProductRef.Minor.GetValueOrDefault(),
                        Major = y.ProductRef.Major.GetValueOrDefault()
                    }).ToListAsync();

        }

        public async Task<List<string>> GetBookingCustomerContact(int bookingId)
        {
            return await _context.InspTranCuContacts.Where(x => x.Active && x.InspectionId == bookingId).Select(x => x.Contact.ContactName).ToListAsync();
        }

        // get booking details based on booking ids
        public async Task<IEnumerable<BookingDetail>> GetBookingData(IEnumerable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id)).
                    Select(x => new BookingDetail
                    {
                        CustomerId = x.CustomerId,
                        SupplierId = x.SupplierId,
                        BookingId = x.Id,
                        PriceCategoryId = x.PriceCategoryId.GetValueOrDefault(),
                        ServiceFrom = x.ServiceDateFrom,
                        ServiceTo = x.ServiceDateTo,
                        OfficeId = x.OfficeId.GetValueOrDefault(),
                        CustomerName = x.Customer.CustomerName,
                        FactoryName = x.Factory.SupplierName,
                        StatusName = x.Status.Status
                    }).AsNoTracking().ToListAsync();
        }

        public async Task<List<BookingContainer>> GetBookingContainer(IEnumerable<int> bookingIds)
        {
            return await _context.InspContainerTransactions.Where(x => x.Active.HasValue && x.Active.Value && bookingIds.Contains(x.InspectionId)).
                            Select(x => new BookingContainer { BookingId = x.InspectionId, ContainerId = x.ContainerId, ContainerRefId = x.Id, ReportId = x.FbReportId }).ToListAsync();
        }

        //get booking info
        public async Task<BookingInfo> GetBookingInfo(int bookingId)
        {
            return await _context.InspTransactions.Where(x => x.Id == bookingId).
                Select(x => new BookingInfo
                {
                    CustomerBookingNo = x.CustomerBookingNo
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// check booking is processed
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<bool> CheckBookingIsProcessed(int bookingId)
        {
            bool isResult = false;

            var resultData = await _context.InspTransactions.Where(x => x.Id == bookingId).FirstOrDefaultAsync();

            if (resultData != null && (resultData.IsProcessing == null) || (resultData.IsProcessing.HasValue && !resultData.IsProcessing.Value))
            {
                resultData.IsProcessing = true;
                _context.Entry(resultData).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                isResult = false;
            }
            else if (resultData != null && (resultData.IsProcessing.HasValue && resultData.IsProcessing.Value))
            {
                isResult = true;
            }

            return isResult;

        }

        /// <summary>
        /// Get the booking buyer list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetBookingBuyerList(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranCuBuyers.Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(x => new CommonDataSource
                {
                    Id = x.Buyer.Id,
                    Name = x.Buyer.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking brand list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetBookingBrandList(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranCuBrands.Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(x => new CommonDataSource
                {
                    Id = x.Brand.Id,
                    Name = x.Brand.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking department list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetBookingDepartmentList(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranCuDepartments.Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(x => new CommonDataSource
                {
                    Id = x.Department.Id,
                    Name = x.Department.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking price category list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetBookingPriceCategoryList(IEnumerable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id) && x.PriceCategory != null)
                .Select(x => new CommonDataSource
                {
                    Id = x.PriceCategory.Id,
                    Name = x.PriceCategory.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the product transaction details from the booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CustomerPriceBookingProductRepo>> GetBookingProductDetails(int bookingId)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value && x.InspectionId == bookingId).
                            Select(x => new CustomerPriceBookingProductRepo
                            {
                                Id = x.ProductId,
                                Name = x.Product.ProductId,
                                AQLQuantity = x.AqlQuantity,
                                CombinedAQLQuantity = x.CombineAqlQuantity,
                                UnitCount = x.UnitCount,
                                CombineProductId = x.CombineProductId
                            }).ToListAsync();
        }


        //get price category based on customer id and product sub category 2 ids
        public async Task<IEnumerable<PriceCategoryDetails>> GetPriceCategory(PriceCategoryRequest request)
        {
            return await _context.CuPriceCategoryPcsub2S.Where(x => x.CustomerId == request.CustomerId &&
            request.ProductSubCategory2IdList.Contains(x.ProductSubCategoryId2) && x.Active.Value)
            .Select(x => new PriceCategoryDetails
            {
                PriceCategoryId = x.PriceCategoryId,
                PriceCategoryName = x.PriceCategory.Name
            }).Distinct().ToListAsync();
        }

        /// <summary>
        /// get Product List by booking id for Mobile
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingProductsData>> GetMobileProductListByBooking(int reportId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && y.FbReportId == reportId).Select(z => new BookingProductsData()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductId = z.Product.ProductId,
                ProductName = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                BookingQuantity = z.TotalBookingQuantity,
                BookingStatus = z.Inspection.StatusId,
                BookingStatusName = z.Inspection.Status.Status,
                ReportResult = z.FbReport.Result.ResultName,
                ReportResultId = z.FbReport.ResultId,
                ReportId = z.FbReport.Id,
                ReportStatus = z.FbReport.FbReportStatusNavigation.StatusName,
                ServiceStartDate = z.FbReport.ServiceFromDate,
                ServiceEndDate = z.FbReport.ServiceToDate,
                UpdatedDate = z.Inspection.UpdatedOn,
                ProductImage = z.FbReport.MainProductPhoto,
                AdditionalPhotos = z.FbReport.FbReportAdditionalPhotos.Select(x => x.PhotoPath),
                CombineProductId = z.CombineProductId.GetValueOrDefault(),
                CombineAqlQuantity = z.CombineAqlQuantity.GetValueOrDefault(),
                ReportPath = z.FbReport.FinalReportPath,
                FinalManualReportPath = z.FbReport.FinalManualReportPath,
                FactoryReference = z.Product.FactoryReference,
                InspectedQuantity = z.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value).SelectMany(x => x.FbReportQuantityDetails.Where(y => y.Active.Value)).Select(r => r.InspectedQuantity).Sum(),
                PresentedQuantity = z.InspPurchaseOrderTransactions.SelectMany(x => x.FbReportQuantityDetails.Where(y => y.Active.Value)).Select(r => r.PresentedQuantity).Sum(),
                //Major = z.InspPurchaseOrderTransactions.SelectMany(x => x.FbReportInspDefects.Where(y => y.Active.Value && y.Major.HasValue)).Select(x => x.Major.GetValueOrDefault()).Sum(),
                //Minor = z.InspPurchaseOrderTransactions.SelectMany(x => x.FbReportInspDefects.Where(y => y.Active.Value && y.Minor.HasValue)).Select(x => x.Minor.GetValueOrDefault()).Sum(),
                //Critical = z.InspPurchaseOrderTransactions.SelectMany(x => x.FbReportInspDefects.Where(y => y.Active.Value && y.Critical.HasValue)).Select(x => x.Critical.GetValueOrDefault()).Sum(),
                DestinationCountry = string.Join(',', z.InspPurchaseOrderTransactions.Select(x => x.DestinationCountry.CountryName).ToArray()),
                PoNumber = string.Join(" ,", z.InspPurchaseOrderTransactions.Select(x => x.Po.Pono)),
                ReportNo = z.FbReport.ReportTitle
            }).AsSingleQuery().AsNoTracking().ToListAsync();
        }

        //get result of report details
        public async Task<List<FBReportInspSubSummary>> GetMobileFBInspSummaryResult(List<int> fbReportIdList)
        {
            return await _context.FbReportInspSubSummaries.Where(x => x.Active.Value && x.FbReportSummary.Active.Value &&
                            fbReportIdList.Contains(x.FbReportSummary.FbReportDetailId)
                    ).
                    Select(x => new FBReportInspSubSummary
                    {
                        Id = x.FbReportSummaryId,
                        Result = x.ResultNavigation.ResultName,
                        FBReportId = x.FbReportSummary.FbReportDetailId,
                        Name = x.Name,
                        Remarks = x.Remarks
                    }).ToListAsync();
        }


        /// <summary>
        /// Get Defect Details specific to report
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InspectionReportDefects>> GetMobileInspectionDefectsByReport(List<int> reportIdList)
        {
            return await _context.FbReportInspDefects.Where(y => y.Active.HasValue && y.Active.Value
            && reportIdList.Contains(y.FbReportDetailId)).Select(z => new InspectionReportDefects()
            {
                Id = z.Id,
                Critical = z.Critical,
                Major = z.Major,
                Minor = z.Minor,
                Description = z.Description,
                Position = z.Position,
                FbReportDetailId = z.FbReportDetailId,
                ProductRefId = z.InspPoTransaction.ProductRefId
            }).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Get Inspection summary by report Id and its type
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InspectionReportSummary>> GetMobileInspectionSummaryByReport(List<int> reportIdList)
        {
            return await _context.FbReportInspSummaries.Where(y => y.Active.HasValue && y.Active.Value
            && reportIdList.Contains(y.FbReportDetailId) && y.FbReportInspsumTypeId == (int)InspSummaryType.Main).Select(z => new InspectionReportSummary()
            {
                Id = z.Id,
                Name = z.Name,
                Photos = z.FbReportInspSummaryPhotos.Select(x => x.Photo).ToList(),
                PhotoCount = z.FbReportInspSummaryPhotos.Count(),
                Remarks = z.Remarks,
                FbReportDetailId = z.FbReportDetailId,
                Result = z.ResultNavigation.ResultName,
                ResultId = z.ResultId
            }).AsSingleQuery().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Customer decision status
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<List<ReportCustomerDecision>> GetMobileReportCustomerDecisionByReport(List<int> reportIdList)
        {
            return await _context.InspRepCusDecisions.Where(x => reportIdList.Contains(x.ReportId) &&
            x.Active.HasValue && x.Active.Value).Select(x => new ReportCustomerDecision
            {
                CustomerDecisionCustomStatus = x.CustomerResult.CustomDecisionName,
                CustomerDecisionStatus = x.CustomerResult.CusDec.Name,
                Comments = x.Comments,
                CustomerResultId = x.CustomerResultId,
                ReportId = x.ReportId
            }).AsNoTracking().ToListAsync();
        }

        // Get the booking department list by departemnt ids and booking ids
        public async Task<List<int>> GetBookingIdsByDeptsAndBookings(IEnumerable<int> departmentIds, IEnumerable<int> bookingIds)
        {
            return await _context.InspTranCuDepartments.Where(x => x.Active && departmentIds.Contains(x.DepartmentId) && bookingIds.Contains(x.InspectionId))
                .Select(x => x.InspectionId).ToListAsync();
        }

        // Get the booking brand list by brand ids and booking ids
        public async Task<List<int>> GetBookingIdsByBrandsAndBookings(IEnumerable<int> brandIds, IEnumerable<int> bookingIds)
        {
            return await _context.InspTranCuBrands.Where(x => x.Active && brandIds.Contains(x.BrandId) && bookingIds.Contains(x.InspectionId))
                .Select(x => x.InspectionId).ToListAsync();
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
                     BookingId = x.InspectionId,
                     DeptCode = x.Department.Code
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

        public async Task<List<BookingCustomerContactAccess>> GetBookingCustomerContacts(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranCuContacts.Where(x => x.Active && bookingIds.Contains(x.InspectionId) && x.Contact.Active.Value).
                    Select(x => new BookingCustomerContactAccess
                    {
                        BookingId = x.InspectionId,
                        CustomerContactId = x.ContactId,
                        CustomerContactName = x.Contact.ContactName,
                        CustomerContactEmail = x.Contact.Email
                    }).ToListAsync();
        }

        public async Task<List<InspectionPriceProductCategory>> GetBookingPriceProductCategory(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value && bookingIds.Contains(x.InspectionId)).
                    Select(x => new InspectionPriceProductCategory
                    {
                        BookingId = x.InspectionId,
                        ProductCategoryId = x.Product.ProductCategoryNavigation.Id,
                        ProductCategoryName = x.Product.ProductCategoryNavigation.Name
                    }).ToListAsync();
        }

        public async Task<List<InspectionPriceProductSubCategory>> GetBookingPriceProductSubCategory(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value && bookingIds.Contains(x.InspectionId)).
                    Select(x => new InspectionPriceProductSubCategory
                    {
                        BookingId = x.InspectionId,
                        ProductSubCategoryId = x.Product.ProductSubCategoryNavigation.Id,
                        ProductSubCategoryName = x.Product.ProductSubCategoryNavigation.Name
                    }).ToListAsync();
        }

        /// <summary>
        /// Service Types by Audit
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<List<BookingServiceType>> GetAuditServiceTypes(IEnumerable<int> auditIds)
        {
            return await _context.AudTranServiceTypes.Where(x => x.Active && auditIds.Contains(x.AuditId)).
                    Select(x => new BookingServiceType
                    {
                        BookingNo = x.AuditId,
                        ServiceTypeId = x.ServiceTypeId,
                        ServiceTypeName = x.ServiceType.Name
                    }).ToListAsync();
        }

        /// <summary>
        ///  Get the booking buyer list by buyer ids and booking ids
        /// </summary>
        /// <param name="buyerIds"></param>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingIdsByBuyersAndBookings(IEnumerable<int> buyerIds, IEnumerable<int> bookingIds)
        {
            return await _context.InspTranCuBuyers.Where(x => x.Active && buyerIds.Contains(x.BuyerId) && bookingIds.Contains(x.InspectionId))
                .Select(x => x.InspectionId).ToListAsync();
        }

        /// <summary>
        /// Get the buyer details with booking ids by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingBuyerAccess>> GetBuyerBookingIdsByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranCuBuyers.Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(x => new BookingBuyerAccess
                {
                    BuyerId = x.Buyer.Id,
                    BuyerName = x.Buyer.Name,
                    BookingId = x.InspectionId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Defect Image Details specific to report
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<List<ReportDefectsImage>> GetMobileInspectionDefectsImageByReport(List<int> defectIdList)
        {
            return await _context.FbReportDefectPhotos.Where(y => y.Active.HasValue && y.Active.Value
            && defectIdList.Contains(y.DefectId)).Select(z => new ReportDefectsImage()
            {
                DefectId = z.DefectId,
                Image = z.Path,
                Desc = z.Description
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get picking data
        /// </summary>
        /// <param name="poTranId"></param>
        /// <returns></returns>
        public async Task<bool> GetPickingData(int poTranId, int prodId)
        {
            return await _context.InspTranPickings.Where(y => y.Active
            && y.PoTranId == poTranId && y.PoTran.ProductRefId == prodId).AnyAsync();
        }
        /// <summary>
        /// get applicant booking details desc by booking id
        /// </summary>
        /// <returns></returns>
        public async Task<InspectionBookingApplicantItems> GetInspectionDetails(int CreatedById)
        {
            return await _context.InspTransactions.Where(x => x.StatusId != (int)BookingStatus.Cancel).OrderByDescending(x => x.Id)
                .Select(x => new InspectionBookingApplicantItems
                {
                    ApplicantEmail = x.ApplicantEmail,
                    ApplicantName = x.ApplicantName,
                    ApplicatPhoneNo = x.ApplicantPhoneNo,
                    CreatedBy = x.CreatedBy
                }).FirstOrDefaultAsync(x => x.CreatedBy == CreatedById);
        }

        //Fetch the Product List for a particular booking
        public async Task<List<InternalReportProducts>> GetProductListByBookingByPO(List<int> bookingId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value && bookingId.Contains(y.InspectionId)).Select(z => new InternalReportProducts()
            {
                BookingId = z.InspectionId,
                ProductId = z.ProductRef.ProductId,
                ProductName = z.ProductRef.Product.ProductId,
                ProductDescription = z.ProductRef.Product.ProductDescription,
                ProductQuantity = z.BookingQuantity,
                ProductCategoryName = z.ProductRef.Product.ProductCategoryNavigation.Name,
                ProductSubCategoryName = z.ProductRef.Product.ProductCategoryNavigation.Name,
                ProductSubCategory2Name = z.ProductRef.Product.ProductCategorySub2Navigation.Name,
                FbReportId = z.ProductRef.FbReportId.GetValueOrDefault(),
                ColorCode = ReportResult.FFFF.ToString(),
                CombineProductId = z.ProductRef.CombineProductId.GetValueOrDefault(),
                PONumber = z.Po.Pono.ToString(),
                CombineAqlQuantity = z.ProductRef.CombineAqlQuantity.GetValueOrDefault(),
                ContainerId = z.ContainerRef.ContainerId,
                FbContainerReportId = z.ContainerRef.FbReportId.GetValueOrDefault(),
                SupplierId = z.Inspection.SupplierId,
                ReportTitle = z.ProductRef.FbReport.ReportTitle,
                ReportPath = z.ProductRef.FbReport.FinalReportPath,
                FinalReportManualPath = z.ProductRef.FbReport.FinalManualReportPath,
                ReportSummaryLink = z.ProductRef.FbReport.ReportSummaryLink,
                ProductImageUrl = z.ProductRef.FbReport.MainProductPhoto,
                ServiceDateFrom = z.ProductRef.FbReport.ServiceFromDate,
                ServiceDateTo = z.ProductRef.FbReport.ServiceToDate,
                ReportStatusId = z.ProductRef.FbReport.ResultId,
                BookingStatus = z.Inspection.StatusId,
                CreatedDate = z.Inspection.CreatedOn
            }).AsNoTracking().ToListAsync();
        }

        //get the previous booking number
        public async Task<int?> GetPreviousBookingNumber(int bookingNo)
        {
            return await _context.InspTransactions.Where(x => x.Id == bookingNo)
                .Select(x => x.PreviousBookingNo).FirstOrDefaultAsync();
        }

        //get the product report data
        public async Task<List<BookingReportData>> GetReportDataByBooking(List<int> bookingNoList)
        {
            return await _context.InspProductTransactions.Where(x => bookingNoList.Contains(x.InspectionId) && x.Active.HasValue && x.Active.Value)
                .Select(x => new BookingReportData
                {
                    BookingId = x.InspectionId,
                    ReportId = x.FbReportId,
                    FactoryId = x.Inspection.FactoryId,
                    FBResultId = x.FbReport.ResultId
                }).Distinct().AsNoTracking().ToListAsync();
        }


        public async Task<List<BookingReportData>> GetReportDataByQueryableBooking(IQueryable<int> bookingNoList)
        {
            return await _context.FbReportDetails.Where(x => bookingNoList.Contains(x.InspectionId.Value) && x.Active.HasValue && x.Active.Value && x.Id > 0)
                .Select(x => new BookingReportData
                {
                    BookingId = x.InspectionId.Value,
                    ReportId = x.Id,
                    FactoryId = x.Inspection.FactoryId,
                    FBResultId = x.ResultId
                }).Distinct().AsNoTracking().ToListAsync();
        }

        public IQueryable<int> GetReportIdDataByQueryableBooking(IQueryable<int> bookingNoList)
        {
            return _context.FbReportDetails.Where(x => bookingNoList.Contains(x.InspectionId.Value) && x.Active.HasValue && x.Active.Value)
                .Select(x => x.Id).Distinct();
        }

        //get the container report data
        public async Task<List<BookingReportData>> GetContainerReportDataByBooking(List<int> bookingNoList)
        {
            return await _context.InspContainerTransactions.Where(x => x.Active.Value && x.FbReport != null && bookingNoList.Contains(x.InspectionId))
                .Select(x => new BookingReportData
                {
                    BookingId = x.InspectionId,
                    ReportId = x.FbReportId,
                    FBResultId = x.FbReport.ResultId
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<BookingReportData>> GetInspectionReportData(List<int> bookingNoList)
        {
            return await _context.FbReportDetails.Where(x => x.Active.HasValue && x.Active.Value && x.InspectionId != null
                                    && bookingNoList.Contains(x.InspectionId.Value) && x.InspProductTransactions.Any(x => x.Active.Value))
                                     .Select(x => new BookingReportData
                                     {
                                         BookingId = x.InspectionId.Value,
                                         ReportId = x.Id,
                                         ReportTitle = x.ReportTitle,
                                         FBResultId = x.ResultId
                                     }).AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsBookingInvoiced(int bookingId)
        {
            return await _context.InvAutTranDetails.
                AnyAsync(x => x.InspectionId == bookingId && x.InvoiceStatus != (int)InvoiceStatus.Cancelled);
        }

        public async Task<bool> IsAnyOneBookingInvoiced(IEnumerable<int> bookingIds)
        {
            return await _context.InvAutTranDetails.
                AnyAsync(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault()) && x.InvoiceStatus != (int)InvoiceStatus.Cancelled);
        }

        /// <summary>
        /// Get the booking id involved in the quotations
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<int>> BookingQuotationExists(List<int> bookingIds)
        {
            return await _context.QuQuotationInsps.Where(x => bookingIds.Contains(x.IdBooking) &&
                                      x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                                    Select(x => x.IdBooking).ToListAsync();
        }

        /// <summary>
        /// Get the picking id involved for the booking.
        /// </summary>
        /// <param name="poTransactionIds"></param>
        /// <returns></returns>
        public async Task<List<int>> GetPickingExists(List<int> poTransactionIds)
        {
            return await _context.InspTranPickings.Where(x => x.Active
                            && poTransactionIds.Contains(x.PoTranId) && (x.LabAddressId != null || x.CusAddressId != null)).
                            Select(x => x.PoTranId).ToListAsync();
        }
        /// <summary>
        /// Get the reportid involved for the poids in the booking.
        /// </summary>
        /// <param name="poTransactionIds"></param>
        /// <returns></returns>
        public async Task<List<int>> GetProductReport(List<int> poTransactionIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value && poTransactionIds.Contains(x.Id)
                                                                && (x.ProductRef.FbReportId != null || x.ContainerRef.FbReportId != null)).
                            Select(x => x.Id).ToListAsync();
        }

        /// <summary>
        /// Get the booking product list (via product transaction) and report data by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionProductsExportData>> GetBookingProductPoList(IQueryable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingIds.Contains(y.InspectionId)).Select(z => new InspectionProductsExportData()
            {
                //Product Transaction Data
                ProductRefId = z.Id,
                BookingId = z.InspectionId,
                ProductName = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                ProductCategory = z.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.Product.ProductCategorySub2Navigation.Name,
                FactoryReference = z.Product.FactoryReference,
                CombineProductId = z.CombineProductId,
                Barcode = z.Product.Barcode,
                IsEcoPack = z.IsEcopack,
                ProductRemarks = z.Remarks,
                IsNewProduct = z.Product.IsNewProduct,
                //Report Data
                ServiceStartDate = z.FbReport.ServiceFromDate,
                ServiceEndDate = z.FbReport.ServiceToDate,
                FbReportId = z.FbReportId,
                ReportStatus = z.FbReport.FbReportStatusNavigation.StatusName,
                ReportResult = z.FbReport.Result.ResultName,
                InspectedQuantity = z.FbReport.FbReportQuantityDetails.Where(x => x.Active.Value).Sum(x => x.InspectedQuantity)

            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking product list (via purchase order transaction) and report data using container data by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionProductsExportData>> GetContainerBookingProductList(IQueryable<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && bookingIds.Contains(y.InspectionId) && y.ContainerRefId != null).Select(z => new InspectionProductsExportData()
            {
                //Product Transaction Data
                ProductRefId = z.ProductRefId,
                BookingId = z.InspectionId,
                ProductName = z.ProductRef.Product.ProductId,
                ProductDescription = z.ProductRef.Product.ProductDescription,
                ProductCategory = z.ProductRef.Product.ProductCategoryNavigation.Name,
                ProductSubCategory = z.ProductRef.Product.ProductSubCategoryNavigation.Name,
                ProductSubCategory2 = z.ProductRef.Product.ProductCategorySub2Navigation.Name,
                FactoryReference = z.ProductRef.Product.FactoryReference,
                CombineProductId = z.ProductRef.CombineProductId,
                Barcode = z.ProductRef.Product.Barcode,
                IsEcoPack = z.ProductRef.IsEcopack,
                ContainerId = z.ContainerRef.ContainerId,

                //Report Data
                ServiceStartDate = z.ContainerRef.FbReport.ServiceFromDate,
                ServiceEndDate = z.ContainerRef.FbReport.ServiceToDate,
                FbReportId = z.ContainerRef.FbReportId,
                ReportStatus = z.ContainerRef.FbReport.FbReportStatusNavigation.StatusName,
                ReportResult = z.ContainerRef.FbReport.Result.ResultName,
                InspectedQuantity = z.ContainerRef.FbReport.FbReportQuantityDetails.Where(x => x.Active.Value).Sum(x => x.InspectedQuantity)
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the dept booking ids by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingDeptAccess>> GetDeptBookingIdsByBookingQuery(IQueryable<int> bookingIds)
        {
            return await _context.InspTranCuDepartments.Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(x =>
                 new BookingDeptAccess
                 {
                     DeptId = x.Department.Id,
                     DeptName = x.Department.Name,
                     BookingId = x.InspectionId
                 }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the brand booking ids by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingBrandAccess>> GetBrandBookingIdsByBookingQuery(IQueryable<int> bookingIds)
        {
            return await _context.InspTranCuBrands.Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(x => new BookingBrandAccess
                {
                    BrandId = x.Brand.Id,
                    BrandName = x.Brand.Name,
                    BookingId = x.InspectionId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// /// Get the buyer booking ids by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingBuyerAccess>> GetBuyerBookingIdsByBookingQuery(IQueryable<int> bookingIds)
        {
            return await _context.InspTranCuBuyers.Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(x => new BookingBuyerAccess
                {
                    BuyerId = x.Buyer.Id,
                    BuyerName = x.Buyer.Name,
                    BookingId = x.InspectionId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the quotation details by booking query
        /// </summary>
        /// <param name="lstbookingid"></param>
        /// <returns></returns>
        public async Task<List<InspectionQuotationExportData>> GetBookingQuotationDetailsbybookingId(IQueryable<int> lstbookingid)
        {
            return await _context.QuQuotationInsps.Where(x => lstbookingid.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                .Select(x => new InspectionQuotationExportData
                {
                    QuotationStatus = x.IdQuotationNavigation.IdStatusNavigation.Label,
                    BookingId = x.IdBooking,
                    ManDay = x.NoOfManDay,
                    QuotationStatusId = x.IdQuotationNavigation.IdStatus
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking po list by booking query
        /// </summary>
        /// <param name="lstbookingid"></param>
        /// <returns></returns>
        public async Task<List<InspectionPOExportData>> GetBookingPoListByBookingQuery(IQueryable<int> lstbookingid)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value && lstbookingid.Contains(x.InspectionId))
                .Select(x => new InspectionPOExportData
                {
                    PONumber = x.Po.Pono,
                    InspectionId = x.InspectionId,
                    BookingQuantity = x.BookingQuantity,
                    ETD = x.Etd,
                    DestinationCountry = x.DestinationCountry.CountryName,
                    ProductRefId = x.ProductRef.Id,
                    ProductId = x.ProductRef.ProductId,
                    ContainerId = x.ContainerRef.ContainerId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the factory country data by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<FactoryCountry>> GetFactorycountryByBookingQuery(IQueryable<int> bookingIds)
        {
            return await (from fact in _context.SuAddresses
                          join insp in _context.InspTransactions on fact.SupplierId equals insp.FactoryId
                          where bookingIds.Contains(insp.Id) && fact.AddressTypeId == (int)Supplier_Address_Type.HeadOffice
                          select new FactoryCountry
                          {
                              FactoryCountryId = fact.CountryId,
                              FactoryProvinceId = fact.RegionId,
                              FactoryCityId = fact.CityId,
                              FactoryCountyId = fact.CountyId.GetValueOrDefault(),
                              FactoryZoneId = fact.County.ZoneId.GetValueOrDefault(),
                              BookingId = insp.Id,
                              FactoryAdress = fact.Address,
                              FactoryRegionalAddress = fact.LocalLanguage,
                              CountryName = fact.Country.CountryName,
                              ProvinceName = fact.Region.ProvinceName,
                              CityName = fact.City.CityName,
                              CountyName = fact.County.CountyName,
                              ZoneName = fact.County.Zone.Name,
                              TownName = fact.Town.TownName
                          }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the service type list by booking query
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<ServiceTypeList>> GetServiceTypeByBookingQuery(IQueryable<int> bookingId)
        {
            return await _context.InspTranServiceTypes
                  .Where(x => bookingId.Contains(x.InspectionId) && x.Active)
                  .Select(x => new ServiceTypeList
                  {
                      InspectionId = x.InspectionId,
                      serviceTypeId = x.ServiceTypeId,
                      serviceTypeName = x.ServiceType.Name
                  }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking involved customer contacts which in inactive in the master list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CustomerContact>> GetEditBookingCustomerContacts(int bookingId)
        {
            return await _context.InspTranCuContacts.Where(x => x.Active && !x.Contact.Active.Value && x.InspectionId == bookingId)
                .Select(x => new CustomerContact
                {
                    Id = x.Contact.Id,
                    ContactName = x.Contact.ContactName,
                    Email = x.Contact.Email
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// /// Get the booking involved supplier contacts which in inactive in the master list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<suppliercontact>> GetEditBookingSupplierContacts(int? bookingId)
        {
            return await _context.InspTranSuContacts.Where(x => x.Active && !x.Contact.Active.Value && x.InspectionId == bookingId)
                .Select(x => new suppliercontact
                {
                    ContactId = x.Contact.Id,
                    ContactName = x.Contact.ContactName,
                    ContactEmail = x.Contact.Mail,
                    Phone = x.Contact.Phone
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// /// Get the booking involved factory contacts which in inactive in the master list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<suppliercontact>> GetEditBookingFactoryContacts(int? bookingId)
        {
            return await _context.InspTranFaContacts.Where(x => x.Active && !x.Contact.Active.Value && x.InspectionId == bookingId)
                .Select(x => new suppliercontact
                {
                    ContactId = x.Contact.Id,
                    ContactName = x.Contact.ContactName,
                    ContactEmail = x.Contact.Mail,
                    Phone = x.Contact.Phone
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking involved office which in inactive in the master list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingOffice(int bookingId)
        {
            return await _context.RefLocations.Where(x => x.Active || (!x.Active && x.InspTransactions.Any(y => y.Id == bookingId)))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.LocationName
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking involved unit which in inactive in the master list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingUnit(int bookingId)
        {
            return await _context.RefUnits.Where(x => x.Active || (!x.Active && x.InspProductTransactions.Any(y => y.Active.Value && y.InspectionId == bookingId)))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().Distinct().ToListAsync();
        }


        /// <summary>
        /// Get the inspection hold reason types
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetHoldReasonTypes()
        {
            return await _context.InspRefHoldReasons.Where(x => x.Active.Value)
                                .OrderBy(x => x.Sort.Value)
                                .Select(x => new CommonDataSource()
                                {
                                    Id = x.Id,
                                    Name = x.Reason
                                }).ToListAsync();
        }

        /// <summary>
        /// Get the booking involved inspection locations and active location list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingInspectionLocations(int bookingId)
        {
            return await _context.InspRefInspectionLocations.Where(x => x.Active.Value || (!x.Active.Value && x.InspTransactions.Any(y => y.Id == bookingId)))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking involved shipment list and active shipment list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingShipmentTypes(int bookingId)
        {
            return await _context.InspRefShipmentTypes.Where(x => x.Active.Value || (!x.Active.Value && x.InspTranShipmentTypes.Any(y => y.InspectionId == bookingId)))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get booking involved business lines
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingBusinessLines(int bookingId)
        {
            return await _context.RefBusinessLines.Where(x => x.Active.Value || (!x.Active.Value && x.InspTransactions.Any(y => y.Id == bookingId)))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.BusinessLine
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking customer product category
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingCuProductCategory(int customerId, int bookingId)
        {
            return await _context.CuProductCategories.Where(x => (x.CustomerId == customerId && x.Active.Value) || (!x.Active.Value && x.InspTransactions.Any(y => y.Id == bookingId)))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the inspection booking detail by bookingid
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<InspectionBookingDetail> GetInspectionBookingDetails(int bookingId)
        {
            return await _context.InspTransactions.Where(x => x.Id == bookingId)
                        .Select(x => new InspectionBookingDetail()
                        {
                            InspectionId = x.Id,
                            InternalReferencePo = x.InternalReferencePo,
                            CustomerId = x.CustomerId,
                            SupplierId = x.SupplierId,
                            FactoryId = x.FactoryId,
                            StatusId = x.StatusId,
                            StatusName = x.Status.Status,
                            SeasonId = x.SeasonId,
                            SeasonYearId = x.SeasonYearId,
                            ServiceDateFrom = x.ServiceDateFrom,
                            ServiceDateTo = x.ServiceDateTo,
                            FirstServiceDateFrom = x.FirstServiceDateFrom,
                            FirstServiceDateTo = x.FirstServiceDateTo,
                            CusBookingComments = x.CusBookingComments,
                            ApiBookingComments = x.ApiBookingComments,
                            InternalComments = x.InternalComments,
                            QCBookingComments = x.QcbookingComments,
                            OfficeId = x.OfficeId,
                            CreatedBy = x.CreatedBy,
                            CreatedOn = x.CreatedOn,
                            EntityId = x.EntityId,
                            ApplicantName = x.ApplicantName,
                            ApplicantEmail = x.ApplicantEmail,
                            ApplicantPhoneNo = x.ApplicantPhoneNo,
                            PreviousBookingNo = x.PreviousBookingNo,
                            ReInspectionType = x.ReInspectionType,
                            IsPickingRequired = x.IsPickingRequired,
                            IsCombineRequired = x.IsCombineRequired,
                            CustomerBookingNo = x.CustomerBookingNo,
                            CreatedUserType = x.CreatedByNavigation.UserTypeId,
                            CollectionId = x.CollectionId,
                            PriceCategoryId = x.PriceCategoryId,
                            CollectionName = x.Collection.Name,
                            PriceCategoryName = x.PriceCategory.Name,
                            CompassBookingNo = x.CompassBookingNo,
                            BusinessLine = x.BusinessLine,
                            InspectionLocation = x.InspectionLocation,
                            ShipmentPort = x.ShipmentPort,
                            ShipmentDate = x.ShipmentDate,
                            EAN = x.Ean,
                            CuProductCategory = x.CuProductCategory,
                            CustomerName = x.Customer.CustomerName,
                            SupplierName = x.Supplier.SupplierName,
                            FactoryName = x.Factory.SupplierName,
                            GuidId = x.GuidId,
                            CustomerSeasonId = x.SeasonId,
                            SeasonYear = x.SeasonYear.Year,
                            SplitPreviousBookingNo = x.SplitPreviousBookingNo,
                            ProductCategoryName = x.ProductCategory.Name,
                            BookingType = x.BookingType,
                            PaymentOptions = x.PaymentOptions,
                            ReInspectionTypeName = x.ReInspectionTypeNavigation.Name,
                            IsEaqf = x.IsEaqf,
                            GAPDACorrelation = x.Gapdacorrelation,
                            GAPDAEmail = x.Gapdaemail,
                            GAPDAName = x.Gapdaname,
                            BookingTypeName = x.BookingTypeNavigation.Name
                        }).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the product transaction list by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<InspectionProductDetail>> GetProductTransactionList(int bookingId)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value && x.InspectionId == bookingId).
                Select(x => new InspectionProductDetail()
                {
                    Id = x.Id,

                    ProductName = x.Product.ProductId,

                    ProductId = x.Product.Id,

                    ProductDesc = x.Product.ProductDescription,

                    ProductCategoryId = x.Product.ProductCategory,

                    ProductSubCategoryId = x.Product.ProductSubCategory,

                    ProductCategorySub2Id = x.Product.ProductCategorySub2,

                    ProductCategorySub3Id = x.Product.ProductCategorySub3,

                    ProductCategoryName = x.Product.ProductCategoryNavigation.Name,

                    ProductSubCategoryName = x.Product.ProductSubCategoryNavigation.Name,

                    ProductCategorySub2Name = x.Product.ProductCategorySub2Navigation.Name,

                    ProductCategorySub3Name = x.Product.ProductCategorySub3Navigation.Name,

                    InspectionId = x.InspectionId,

                    Unit = x.Unit,

                    UnitCount = x.UnitCount,

                    TotalBookingQuantity = x.TotalBookingQuantity,

                    Remarks = x.Remarks,

                    Aql = x.Aql,

                    Critical = x.Critical,

                    Major = x.Major,

                    Minor = x.Minor,

                    CreatedBy = x.CreatedBy,

                    CreatedOn = x.CreatedOn,

                    DeletedBy = x.DeletedBy,

                    DeletedOn = x.DeletedOn,

                    Active = x.Active,

                    UnitName = x.UnitNavigation.Name,

                    AqlQuantity = x.AqlQuantity,

                    SampleType = x.SampleType,

                    CombineSamplingSize = x.CombineAqlQuantity,

                    CombineGroupId = x.CombineProductId,

                    ReportId = x.FbReportId,

                    FactoryReference = x.Product.FactoryReference,

                    Barcode = x.Product.Barcode,

                    IsEcopack = x.IsEcopack,

                    IsDisplayMaster = x.IsDisplayMaster,

                    FBTemplateId = x.FbtemplateId,

                    FbTemplateName = x.Fbtemplate.Name,

                    ParentProductId = x.ParentProductId,

                    ParentProductName = x.ParentProduct.ProductId,

                    IsNewProduct = x.Product.IsNewProduct,

                    AsReceivedDate = x.AsReceivedDate,

                    TfReceivedDate = x.TfReceivedDate,

                    ProductImageCount = x.Product.CuProductFileAttachments.Where(x => x.Active && x.FileTypeId.HasValue && x.FileTypeId.Value == (int)ProductRefFileType.ProductRefPictures).Select(x => x.Id).Count()
                }).ToListAsync();
        }

        public async Task<List<InspectionPODetail>> GetPurchaseOrderTransactionList(int bookingId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.Value && x.InspectionId == bookingId)
                            .Select(x => new InspectionPODetail()
                            {
                                Id = x.Id,
                                ProductRefId = x.ProductRefId,
                                PoName = x.Po.Pono,
                                PoId = x.PoId,
                                ProductId = x.ProductRef.ProductId,
                                InspectionId = x.InspectionId,
                                BookingQuantity = x.BookingQuantity,
                                PickingQuantity = x.PickingQuantity,
                                Remarks = x.Remarks,
                                DestinationCountryID = x.DestinationCountryId,
                                DestinationCountryName = x.DestinationCountry.CountryName,
                                ContainerId = x.ContainerRef.ContainerId,
                                ETDDate = x.Etd,
                                CreatedBy = x.CreatedBy,
                                CreatedOn = x.CreatedOn,
                                DeletedBy = x.DeletedBy,
                                DeletedOn = x.DeletedOn,
                                Active = x.Active,
                                CustomerReferencePo = x.CustomerReferencePo,
                                BaseCustomerReferencePo = x.CustomerReferencePo,
                                PoQuantity = x.Po.CuPurchaseOrderDetails.FirstOrDefault(y => y.Active.Value && y.ProductId == x.ProductRef.ProductId).Quantity
                            }).ToListAsync();
        }

        /// <summary>
        /// Get the inspection hold reasons by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<InspectionHoldReasons> GetInspectionHoldReasons(int bookingId)
        {
            return await _context.InspTranHoldReasons.Where(x => x.Active.Value && x.InspectionId == bookingId)
                    .Select(x => new InspectionHoldReasons()
                    {
                        Id = x.Id,
                        ReasonType = x.ReasonType,
                        Reason = x.ReasonTypeNavigation.Reason,
                        Comment = x.Comment,
                        InspectionId = x.InspectionId,
                        CreatedOn = x.CreatedOn
                    }).OrderByDescending(x => x.CreatedOn).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the booking mapped customer contacts
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingMappedCustomerContacts(int bookingId)
        {
            return await _context.InspTranCuContacts.Where(x => x.Active && x.Contact.Active.Value && x.InspectionId == bookingId)
                    .Select(x => x.ContactId).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking mapped supplier contacts
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingMappedSupplierContacts(int bookingId)
        {
            return await _context.InspTranSuContacts.Where(x => x.Active && x.Contact.Active.Value && x.InspectionId == bookingId)
                    .Select(x => x.ContactId).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking mapped factory contacts
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int?>> GetBookingMappedFactoryContacts(int bookingId)
        {
            return await _context.InspTranFaContacts.Where(x => x.Active && x.Contact.Active.Value && x.InspectionId == bookingId)
                    .Select(x => x.ContactId).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking mapped buyer list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingMappedBuyers(int bookingId)
        {
            return await _context.InspTranCuBuyers.Where(x => x.Active && x.InspectionId == bookingId)
                    .Select(x => x.BuyerId).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking mapped brand list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingMappedBrands(int bookingId)
        {
            return await _context.InspTranCuBrands.Where(x => x.Active && x.InspectionId == bookingId)
                    .Select(x => x.BrandId).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking mapped brand list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingMappedDepartments(int bookingId)
        {
            return await _context.InspTranCuDepartments.Where(x => x.Active && x.InspectionId == bookingId)
                    .Select(x => x.DepartmentId).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking mapped merchandiser list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingMappedMerchandisers(int bookingId)
        {
            return await _context.InspTranCuMerchandisers.Where(x => x.Active && x.InspectionId == bookingId)
                    .Select(x => x.MerchandiserId).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking mapped shipment type list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int?>> GetBookingMappedShipmentTypes(int bookingId)
        {
            return await _context.InspTranShipmentTypes.Where(x => x.Active.Value && x.InspectionId == bookingId)
                    .Select(x => x.ShipmentTypeId).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking mapped containers
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetBookingMappedContainers(int bookingId)
        {
            return await _context.InspContainerTransactions.Where(x => x.Active.Value && x.InspectionId == bookingId)
                    .Select(x => x.ContainerId).Distinct().ToListAsync();
        }
        /// <summary>
        /// Get the booking mapped dynamic transactions
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<InspectionDFTransactions>> GetBookingMappedDFTransactions(int bookingId)
        {
            return await _context.InspDfTransactions.Where(x => x.Active && x.BookingId == bookingId)
                    .Select(x => new InspectionDFTransactions()
                    {
                        Id = x.Id,
                        BookingId = x.BookingId,
                        ControlConfigurationId = x.ControlConfigurationId,
                        Value = x.Value
                    }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// get the booking mapped inspection files
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<BookingFileAttachment>> GetBookingMappedFiles(int bookingId)
        {
            return await _context.InspTranFileAttachments.Where(x => x.Active && x.InspectionId == bookingId)
                    .Select(x => new BookingFileAttachment()
                    {
                        FileName = x.FileName,
                        FileDescription = x.FileDescription,
                        BookingId = x.InspectionId,
                        Id = x.Id,
                        IsNew = false,
                        uniqueld = x.UniqueId,
                        FileUrl = x.FileUrl,
                        IsBookingEmailNotification = x.IsbookingEmailNotification,
                        IsReportSendToFB = x.IsReportSendToFb,
                        FileAttachmentCategoryId = x.FileAttachmentCategoryId
                    }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking mapped product file attachments
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<ProductFileAttachmentRepsonse>> GetProductFileAttachments(int bookingId)
        {
            return await _context.CuProductFileAttachments.Where(x => x.Active && x.BookingId == bookingId
                    && x.FileTypeId == (int)ProductRefFileType.ProductRefPictures
                     && x.Product.InspProductTransactionProducts.Any(y => y.Active.Value && y.InspectionId == bookingId))
                    .Select(x => new ProductFileAttachmentRepsonse()
                    {
                        FileName = x.FileName,
                        Id = x.Id,
                        IsNew = false,
                        FileUrl = x.FileUrl,
                        uniqueld = x.UniqueId,
                        MimeType = "",
                        ProductId = x.ProductId
                    }).AsNoTracking().Distinct().ToListAsync();
        }
        /// <summary>
        /// Get the product subcategory list by product category ids
        /// </summary>
        /// <param name="productCategoryIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionProductSubCategory>> GetProductSubCategoryList(List<int?> productCategoryIds)
        {
            return await _context.RefProductCategorySubs.Where(x => productCategoryIds.Contains(x.Id)).
                Select(x => new InspectionProductSubCategory()
                {
                    ProductCategoryId = x.ProductCategoryId,
                    ProductSubCategoryId = x.Id,
                    ProductSubCategoryName = x.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the product subcategory2 list by productsubcategoryids
        /// </summary>
        /// <param name="productSubCategoryIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionProductSubCategory2>> GetProductSubCategory2List(List<int?> productSubCategoryIds)
        {
            return await _context.RefProductCategorySub2S.Where(x => productSubCategoryIds.Contains(x.ProductSubCategoryId)).
                Select(x => new InspectionProductSubCategory2()
                {
                    ProductSubCategoryId = x.ProductSubCategoryId,
                    ProductSubCategory2Id = x.Id,
                    ProductSubCategory2Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<InspectionProductSubCategory3>> GetProductSubCategory3List(List<int> productSubCategory2Ids)
        {
            return await _context.RefProductCategorySub3S.Where(x => x.Active && productSubCategory2Ids.Contains(x.ProductSubCategory2Id)).
                Select(x => new InspectionProductSubCategory3()
                {
                    ProductSubCategory2Id = x.ProductSubCategory2Id,
                    ProductSubCategory3Id = x.Id,
                    ProductSubCategory3Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        public async Task<BookingDataRepo> GetBookingDetails(int bookingId)
        {
            return await _context.InspTransactions
                .Where(x => x.Id == bookingId)
                .Select(x => new BookingDataRepo
                {
                    BookingNo = x.Id,
                    ServiceDateFrom = x.ServiceDateFrom,
                    ServiceDateTo = x.ServiceDateTo,
                    CustomerName = x.Customer.CustomerName,
                    CustomerId = x.CustomerId,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryName = x.Factory.SupplierName,
                    BookingStatus = x.StatusId,
                    RegionalSupplierName = x.Supplier.LocalName,
                    RegionalFactoryName = x.Factory.LocalName,
                    Office = x.Office.LocationName,
                    Status = x.Status.Status
                }).FirstOrDefaultAsync();
        }
        /// <summary>
        /// get Product List by booking id 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingProductinfo>> GetProductItemByBooking(int bookingId)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.HasValue && y.Active.Value
            && y.InspectionId == bookingId).Select(z => new BookingProductinfo()
            {
                Id = z.Id,
                BookingId = z.InspectionId,
                ProductId = z.ProductId,
                ProductName = z.Product.ProductId,
                ProductDescription = z.Product.ProductDescription,
                BookingQuantity = z.TotalBookingQuantity,
                UnitName = z.UnitNavigation.Name
            }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// IQueryable GetInspectionNo
        /// </summary>

        /// <returns></returns>
        public IQueryable<int> GetInspectionNo()
        {
            return _context.InspTransactions.Select(x => x.Id);
        }
        /// <summary>
        /// Get Booking Products Po Data By ProductRefIds by  booking id
        /// </summary>
        /// <param name="productRefIds"></param>
        /// <returns></returns>
        public async Task<List<BookingProductPoRepo>> GetBookingProductsPoItemsByProductRefIds(int bookingId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.InspectionId == bookingId && x.Active.HasValue && x.Active.Value).
                            Select(x => new BookingProductPoRepo
                            {
                                PoName = x.Po.Pono,
                                ProductRefId = x.ProductRefId,
                                BookingId = x.InspectionId
                            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the product information(result,productcategoryid) by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionProductAndReport>> GetBookingProductAndReportResult(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(y => y.Active.Value && bookingIds.Contains(y.InspectionId)).
                            Select(y => new InspectionProductAndReport()
                            {
                                InspectionId = y.InspectionId,
                                ProductCategoryId = y.Product.ProductCategory,
                                ResultId = y.FbReport.ResultId
                            }).ToListAsync();
        }


        public async Task<List<InspectionPoNumberList>> GetPoNoListByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.Value
                                && bookingIds.Contains(y.InspectionId)).Select(y => new InspectionPoNumberList()
                                {
                                    InspectionId = y.InspectionId,
                                    ProductRefId = y.ProductRefId,
                                    ContainerRefId = y.ContainerRefId,
                                    PoNumber = y.Po.Pono
                                }).ToListAsync();
        }

        /// <summary>
        /// Get the supplier contacts by bookingids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionSupplierFactoryContacts>> GetSupplierContactsByBookingIds(List<int> bookingIds)
        {
            return await _context.InspTranSuContacts.Where(x => x.Active && bookingIds.Contains(x.InspectionId) && x.Contact.Active.Value)
                .Select(x => new InspectionSupplierFactoryContacts
                {
                    InspectionId = x.InspectionId,
                    ContactName = x.Contact.ContactName,
                    ContactEmail = x.Contact.Mail,
                    Phone = x.Contact.Phone
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the factory contacts by bookingids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionSupplierFactoryContacts>> GetFactoryContactsByBookingIds(List<int> bookingIds)
        {
            return await _context.InspTranFaContacts.Where(x => x.Active && x.InspectionId != null && bookingIds.Contains(x.InspectionId.Value) && x.Contact.Active.Value)
                .Select(x => new InspectionSupplierFactoryContacts
                {
                    InspectionId = x.InspectionId,
                    ContactName = x.Contact.ContactName,
                    ContactEmail = x.Contact.Mail,
                    Phone = x.Contact.Phone
                }).AsNoTracking().Distinct().ToListAsync();
        }
        /// <summary>
        /// select merchandiser list by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingMerchandiserContactList>> GetMerchandiserContactsByBookingIds(List<int> bookingIds)
        {
            return await _context.InspTranCuMerchandisers.Where(x => x.Active && bookingIds.Contains(x.InspectionId))
                .Select(x => new BookingMerchandiserContactList
                {
                    BookingId = x.InspectionId,
                    MerchandiserContactName = x.Merchandiser.ContactName,
                    MerchandiserContactEmail = x.Merchandiser.Email
                }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the po color transaction list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<InspectionPOColorTransaction>> GetPOColorTransactions(int bookingId)
        {
            return await _context.InspPurchaseOrderColorTransactions.Where(x => x.Active.Value && x.ProductRef.InspectionId == bookingId)
                        .Select(x => new InspectionPOColorTransaction()
                        {
                            ColorTransactionId = x.Id,
                            PoTransactionId = x.PoTransId,
                            ProductRefId = x.ProductRefId,
                            ColorCode = x.ColorCode,
                            ColorName = x.ColorName,
                            BookingQuantity = x.BookingQuantity,
                            PickingQuantity = x.PickingQuantity
                        }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the inspection transaction cs list
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetInspectionTransCSList(int bookingId)
        {
            return await _context.InspTranCs.Where(x => x.Active.Value && x.InspectionId == bookingId)
                        .Select(x => new CommonDataSource() { Id = x.CsId.Value, Name = x.Cs.FullName }).AsNoTracking().ToListAsync();
        }

        public async Task<bool> GetEntityFeatureIsExist(int factoryCountryId)
        {
            return await _context.EntFeatureDetails.AnyAsync(x => x.Active.Value && x.CountryId == factoryCountryId);
        }

        public async Task<InspTransaction> GetInspectionBaseTransaction(int bookingId)
        {
            return await _context.InspTransactions.
                        Include(x => x.InspProductTransactions).
                        Include(x => x.InspPurchaseOrderTransactions).
                        ThenInclude(x => x.InspPurchaseOrderColorTransactions).
                        FirstOrDefaultAsync(x => x.Id == bookingId);
        }


        public async Task<IEnumerable<InspTranStatusLog>> GetBookingStatusLogsByQuery(IQueryable<int> inspectionIdList)
        {
            return await _context.InspTranStatusLogs.Where(x => inspectionIdList.Contains(x.BookingId) && x.StatusId == (int)BookingStatus.ReportSent).AsNoTracking().ToListAsync();
        }

        public async Task<List<InspectionHoldReasons>> GetInspectionHoldReasons(IQueryable<int> bookingIds)
        {
            return await _context.InspTranHoldReasons.Where(x => x.Active.Value && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
                    .Select(x => new InspectionHoldReasons()
                    {
                        Id = x.Id,
                        ReasonType = x.ReasonType,
                        Reason = x.ReasonTypeNavigation.Reason,
                        Comment = x.Comment,
                        InspectionId = x.InspectionId,
                        CreatedOn = x.CreatedOn
                    }).OrderByDescending(x => x.CreatedOn).AsNoTracking().ToListAsync();
        }

        public async Task<List<InspectionHoldReasons>> GetInspectionHoldReasons(List<int> bookingIds)
        {
            return await _context.InspTranHoldReasons.Where(x => x.Active.Value && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
                    .Select(x => new InspectionHoldReasons()
                    {
                        Id = x.Id,
                        ReasonType = x.ReasonType,
                        Reason = x.ReasonTypeNavigation.Reason,
                        Comment = x.Comment,
                        InspectionId = x.InspectionId,
                        CreatedOn = x.CreatedOn
                    }).OrderByDescending(x => x.CreatedOn).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the inspection cancel reason data
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionCancelReason>> GetInspectionCancelReasons(IQueryable<int> bookingIds)
        {
            return await _context.InspTranCancels.Where(x => bookingIds.Contains(x.InspectionId))
                    .Select(x => new InspectionCancelReason()
                    {
                        InspectionId = x.InspectionId,
                        CancelReason = x.Comments,
                        CancelType = x.ReasonType.Reason,

                    }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the booking mapped inspection files
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<BookingFileAttachment>> GetBookingMappedFilesByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranFileAttachments.Where(x => x.Active && bookingIds.Contains(x.InspectionId) && x.IsReportSendToFb.Value)
                    .Select(x => new BookingFileAttachment()
                    {
                        FileName = x.FileName,
                        FileDescription = x.FileDescription,
                        Id = x.Id,
                        IsNew = false,
                        uniqueld = x.UniqueId,
                        FileUrl = x.FileUrl,
                        BookingId = x.InspectionId
                    }).AsNoTracking().ToListAsync();
        }

        public async Task<InspTransactionDraft> GetInspectionDraftById(int id)
        {
            return await _context.InspTransactionDrafts.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the inspection drafs data for the created user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<DraftInspectionRepo>> GetInspectionDraftByUserId(int userId)
        {
            return await _context.InspTransactionDrafts.Where(x => x.Active.Value && x.CreatedBy == userId && x.InspectionId == null).
                Select(x => new DraftInspectionRepo()
                {
                    Id = x.Id,
                    Customer = x.Customer.CustomerName,
                    Supplier = x.Supplier.SupplierName,
                    Factory = x.Factory.SupplierName,
                    ServiceDateFrom = x.ServiceDateFrom,
                    ServiceDateTo = x.ServiceDateTo,
                    Brand = x.Brand.Name,
                    Department = x.Department.Name,
                    BookingInfo = x.BookingInfo,
                    IsReInspectionDraft = x.IsReInspectionDraft,
                    IsReBookingDraft = x.IsReBookingDraft,
                    PreviousBookingNo = x.PreviousBookingNo,
                    CreatedOn = x.CreatedOn
                }).OrderByDescending(x => x.CreatedOn).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the ent page file access
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<EntPageFieldAccess>> GetEntPageFieldAccess(EntPageRequest request)
        {
            return await _context.EntPagesFields.Where(x => x.Active.Value && x.EntityId == request.EntityId
                                && x.UserTypeId == request.UserTypeId
                                && x.Entfield.Entpage.ServiceId == request.ServiceId && x.Entfield.Entpage.RightId == request.RightId).
                                Select(x => new EntPageFieldAccess()
                                {
                                    Id = x.Entfield.Id,
                                    Name = x.Entfield.Name,
                                    CustomerId = x.CustomerId
                                }).ToListAsync();
        }


        /// <summary>
        /// get the inspections by product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<List<InspTransaction>> GetInspectionByProductId(int productId)
        {
            return await _context.InspProductTransactions.Where(x => x.ProductId == productId && x.Active == true).Select(y => y.Inspection).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the po product data by poid,customerid,supplierid
        /// </summary>
        /// <param name="bookingPoSearchData"></param>
        /// <returns></returns>
        public async Task<List<PoProductData>> GetPoProductDataByPoAndCustomer(BookingPoSearchData bookingPoSearchData)
        {
            return await _context.CuPurchaseOrderDetails.Where(x => x.Active.HasValue && x.Active.Value
                            && bookingPoSearchData.PoList.Contains(x.PoId)
                            && x.Po.CustomerId == bookingPoSearchData.CustomerId).
                        Select(x => new PoProductData()
                        {
                            poId = x.PoId,
                            productId = x.ProductId
                        }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the file attachments by pending zip status and general file attachment category
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InspTranFileAttachment>> GetFileAttachmentsByBookingIdsAndZipStatus(int? inspectionId)
        {
            var fileAttachments = _context.InspTranFileAttachments.Where(x => x.Active
                                && x.ZipStatus == (int)ZipStatus.Pending
                                && x.FileAttachmentCategoryId == (int)FileAttachmentCategory.General
                                && x.ZipTryCount <= 3);

            if (inspectionId > 0)
                fileAttachments = fileAttachments.Where(x => x.InspectionId == inspectionId);

            return await fileAttachments.ToListAsync();
        }

        /// <summary>
        /// Get the inspection file attachment zip
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        public async Task<InspTranFileAttachmentZip> GetInspectionFileAttachmentZipData(int inspectionId)
        {
            return await _context.InspTranFileAttachmentZips.Where(x => x.Active.Value && x.InspectionId == inspectionId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the booking file attachment by booking id
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        public async Task<BookingFileZipAttachment> GetBookingFileAttachment(int inspectionId)
        {
            return await _context.InspTranFileAttachmentZips.Where(x => x.Active.Value
                    && x.InspectionId == inspectionId).Select(x => new BookingFileZipAttachment()
                    {
                        ZipFileUrl = x.FileUrl,
                        ZipFileName = x.FileName,
                        InspectionId = x.InspectionId
                    }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Check the po detail available by po product detail
        /// </summary>
        /// <param name="bookingPoProductSearchData"></param>
        /// <returns></returns>
        public async Task<bool> CheckPoDetailAvailableByPoProductDetail(BookingPoProductSearchData bookingPoProductSearchData)
        {
            return await _context.CuPurchaseOrderDetails.AnyAsync(x => x.Active.HasValue && x.Active.Value
                            && x.PoId == bookingPoProductSearchData.PoId
                            && x.ProductId == bookingPoProductSearchData.ProductId
                            && x.Po.CustomerId == bookingPoProductSearchData.CustomerId);

        }

        /// <summary>
        /// Get the service type ignore accepatance level
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        public async Task<bool> GetServiceTypeIgnoreAcceptanceLevel(int customerId, int serviceTypeId)
        {
            return await _context.CuServiceTypes.AnyAsync(x => x.CustomerId == customerId
                                                && x.ServiceTypeId == serviceTypeId && x.IgnoreAcceptanceLevel.Value);
        }

        public async Task<BookingCustomerServiceTypeData> GetBookingCustomerServiceTypes(int bookingId)
        {
            return await _context.InspTransactions.Where(x => x.Id == bookingId).Select(x => new
                                    BookingCustomerServiceTypeData()
            {
                CustomerId = x.CustomerId,
                ServiceTypeId = x.InspTranServiceTypes.FirstOrDefault(x => x.Active && x.InspectionId == bookingId).ServiceTypeId
            }).FirstOrDefaultAsync();
            //return await _context.InspTransactions.Where(x=>;
        }

        /// <summary>
        /// get the report result by inspection ids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<InvoicePreviewReportResult>> GetReportResultByInspectionId(List<int> inspectionIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => inspectionIds.Contains(x.InspectionId) && x.ProductRef.FbReport.ResultId.HasValue).Select(x => new InvoicePreviewReportResult()
            {
                InspectionId = x.InspectionId,
                ProductRefId = x.ProductRefId,
                POId = x.Id,
                Result = x.ProductRef.FbReport.Result.ResultName
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get product information 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProductTranData>> GetProductDetails(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.Active.Value)
                .Select(x => new ProductTranData
                {
                    BookingId = x.InspectionId,
                    Unit = x.UnitNavigation.Name,
                    ProductCode = x.Product.ProductId,
                    FbReportId = x.FbReportId,
                    ProductRefId = x.Id
                }).Distinct().AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<InspectionPOColorTransaction>> GetPOColorTransactionsByBookingIds(IEnumerable<int> bookingIds)
        {

            return await _context.InspPurchaseOrderColorTransactions.Where(x => x.Active.Value && bookingIds.Contains(x.ProductRef.InspectionId))
                        .Select(x => new InspectionPOColorTransaction()
                        {
                            ColorTransactionId = x.Id,
                            PoTransactionId = x.PoTransId,
                            ProductRefId = x.ProductRefId,
                            ProductId = x.ProductRef.ProductId,
                            ColorCode = x.ColorCode,
                            ColorName = x.ColorName,
                            BookingId = x.ProductRef.InspectionId,
                            BookingQuantity = x.BookingQuantity,
                            PickingQuantity = x.PickingQuantity
                        }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Po Data By booking id
        /// </summary>
        /// <param name="productRefIds"></param>
        /// <returns></returns>
        public async Task<List<BookingProductPoRepo>> GetPoDataByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.Active.Value).
                            Select(x => new BookingProductPoRepo
                            {
                                PoName = x.Po.Pono,
                                ProductRefId = x.ProductRefId,
                                BookingId = x.InspectionId,
                                ContainerRefId = x.ContainerRefId
                            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get cs name list
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<List<CSNameRepo>> GetInspectionTransCSList(IQueryable<int> bookingIdList)
        {
            return await _context.InspTranCs.Where(x => x.Active.Value && bookingIdList.Contains(x.InspectionId.GetValueOrDefault()))
                        .Select(x => new CSNameRepo()
                        {
                            Id = x.CsId.Value,
                            Name = x.Cs.FullName,
                            BookingId = x.InspectionId
                        }).AsNoTracking().ToListAsync();
        }

        public async Task<bool> GetPODataBypoId(string poid)
        {
            return await _context.InspPurchaseOrderTransactions.AnyAsync(x => x.Po.Pono == poid && x.Active.HasValue && x.Active.Value);
        }

        /// <summary>
        /// get customer report inspection details
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<CustomerReportInspectionRepo>> GetCustomerReportInspectionDetails(IQueryable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id)).Select(y => new CustomerReportInspectionRepo()
            {
                FactoryName = y.Factory.SupplierName,
                FactoryCode = y.Factory.GlCode,
                Season = y.Season.Season.Name,
                Year = y.SeasonYear.Year,
                SupplierName = y.Supplier.SupplierName,
                ReInspectionType = y.ReInspectionType,
                SupplierCode = y.Supplier.GlCode,
                InspectionId = y.Id,
                InspectionType = y.InspTranServiceTypes.FirstOrDefault().ServiceType.Name,
                FactoryId = y.FactoryId.GetValueOrDefault()
            }).AsNoTracking().ToListAsync();
        }

        public async Task<string> GetBoookingStatus(string poid)
        {
            var bookingStatus = string.Empty;
            var inspPoTransaction = _context.InspPurchaseOrderTransactions.Include(x => x.Inspection)
                .Where(x => x.Po.Pono == poid);

            var data = inspPoTransaction.Any(x => x.Inspection.StatusId != (int)BookingStatus.Inspected || x.Inspection.StatusId == (int)BookingStatus.ReportSent);

            if (data)
            {
                bookingStatus = "in progress";
            }
            else
            {
                var checkInspectedOrReportSent = inspPoTransaction.All(x => x.Inspection.StatusId == (int)BookingStatus.Inspected || x.Inspection.StatusId == (int)BookingStatus.ReportSent);
                if (checkInspectedOrReportSent)
                    bookingStatus = "completed";
            }
            return bookingStatus;
        }

        public async Task<RefUnit> GetUnitByName(string unitname)
        {
            return await _context.RefUnits.FirstOrDefaultAsync(x => x.Active && x.Name == unitname);
        }

        public async Task<bool> IsAnyProductReportInProgress(int productId)
        {
            return await _context.InspProductTransactions.AnyAsync(x => x.ProductId == productId && x.Active == true && x.FbReport.Active == true && x.FbReport.FbReportStatus == (int)FBStatus.ReportDraft);
        }
        public async Task<IEnumerable<InspectionCsData>> GetInspectionTransCsDetails(List<int> bookingIds)
        {
            return await _context.InspTranCs.Where(x => x.Active.Value && bookingIds.Contains(x.InspectionId.Value))
                    .Select(x => new InspectionCsData
                    {
                        InspectionId = x.InspectionId,
                        CsId = x.CsId,
                        CsName = x.Cs.FullName
                    }).Distinct().ToListAsync();
        }


        public async Task<bool> CheckInspectionStatusByBookingId(int bookingId, int statusId)
        {
            return await _context.InspTransactions.AsNoTracking().AnyAsync(x => x.Id == bookingId && x.StatusId == statusId);
        }

        public async Task<InspBookingEmailConfiguration> GetCCEmailConfigurationEmailsByCustomer(int customerId, int? factoryCountryId, int bookingStatusId)
        {
            return await _context.InspBookingEmailConfigurations
                .FirstOrDefaultAsync(x => x.Active && x.CustomerId == customerId && x.BookingStatusId == bookingStatusId && x.FactoryCountryId == factoryCountryId);
        }

        public async Task<bool> CheckInspectionBookingByCustomerId(int bookingId, int customerId)
        {
            return await _context.InspTransactions.AsNoTracking().AnyAsync(x => x.Id == bookingId && x.CustomerId == customerId);
        }

        /// <summary>
        /// get product information 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<ScheduleBookingDetailsRepoItem>> GetBookingsByServiceDates(DateTime yesterdayDate)
        {
            return await _context.InspTransactions.Where(x => x.StatusId != (int)BookingStatus.Cancel && x.ServiceDateFrom <= yesterdayDate && yesterdayDate <= x.ServiceDateTo)
                .Select(y => new ScheduleBookingDetailsRepoItem()
                {
                    InspectionId = y.Id,
                    CustomerName = y.Customer.CustomerName,
                    FactoryName = y.Factory.SupplierName,
                    SupplierName = y.Supplier.SupplierName,
                    BusinessLine = y.BusinessLine
                }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EaqfProductDetails>> GetEaqfBookingPOTransactionDetails(List<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.ProductRef.Active.HasValue && x.Po.Active && x.Active.HasValue && x.Active.Value && bookingIds.Contains(x.InspectionId)).
                Select(p => new EaqfProductDetails
                {
                    PoNo = p.Po.Pono,
                    ProductReference = p.ProductRef.Product.ProductId,
                    Description = p.ProductRef.Product.ProductDescription,
                    Unit = p.ProductRef.Unit,
                    Quantity = p.ProductRef.AqlQuantity,
                    AqlLevel = p.ProductRef.Aql,
                    AQLCritical = p.ProductRef.Critical,
                    AQLMajor = p.ProductRef.Major,
                    AQLMinor = p.ProductRef.Minor,
                    DestinationCountry = p.DestinationCountry.Alpha2Code
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking default comments
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<string> GetCustomerBookingDefaulComments(int customerId)
        {
            return await _context.CuCustomers.Where(x => x.Active.Value && x.Id == customerId).
                Select(x => x.BookingDefaultComments).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<ReportVersionData>> GetReportVersionDetails(List<int> ReportIdList)
        {
            return await _context.FbReportDetails
                .Where(x => ReportIdList.Contains(x.Id) && x.Active.Value)
                .Select(x => new ReportVersionData()
                {
                    ReportId = x.Id,
                    ReportRevision = x.ReportRevision,
                    ReportVersion = x.ReportVersion
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the inspection picking details by inspection id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<InspectionPickingDetails>> GetInspectionPicking(int bookingId)
        {
            return await _context.InspTranPickings.Where(x => x.Active && x.BookingId == bookingId).
                Select(x => new InspectionPickingDetails()
                {
                    Id = x.Id,
                    labId = x.LabId,
                    CustomerId = x.CustomerId,
                    labName = x.Lab.LabName,
                    labAddressId = x.LabAddressId,
                    labAddress = x.LabAddress.Address,
                    CustomerName = x.Customer.CustomerName,
                    CustomerAddressId = x.CusAddressId,
                    CustomerAddress = x.CusAddress.Address,
                    PoTransactionId = x.PoTranId,
                    InspectionId = x.BookingId,
                    PickingQuantity = x.PickingQty,
                    Remarks = x.Remarks
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the inspection picking contacts 
        /// </summary>
        /// <param name="pickingIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionPickingContactDetails>> GetInspectionPickingContacts(List<int> pickingIds)
        {
            return await _context.InspTranPickingContacts.Where(x => x.Active && pickingIds.Contains(x.PickingTranId)).
                Select(x => new InspectionPickingContactDetails()
                {
                    Id = x.Id,
                    LabContactId = x.LabContactId,
                    CustomerContactId = x.CusContactId,
                    InspectionPickingId = x.PickingTranId,
                    LabContactName = x.LabContact.ContactName,
                    CustomerContactName = x.CusContact.ContactName
                }).AsNoTracking().ToListAsync();
        }

        public async Task<InspTransaction> GetInspectionWithFileAttachment(int inspectionId)
        {
            return await _context.InspTransactions.Include(x => x.InspTranFileAttachments).IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == inspectionId);
        }

        public async Task<InspTransaction> GetBookingDataUptoInspected(int inspectionId)
        {
            var statusList = new List<int> { 7, 11 };// Validated & Report Sent
            return await _context.InspTransactions.
                FirstOrDefaultAsync(x => x.Id == inspectionId && !statusList.Contains(x.StatusId));
        }
    }
}
