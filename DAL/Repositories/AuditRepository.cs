using Contracts.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DTO.RepoRequest.Audit;
using DTO.RepoRequest.Enum;
using Entities.Enums;
using DTO.Common;
using DTO.Quotation;
using DTO.Audit;
using DTO.Inspection;
using DTO.FullBridge;
using DTO.CommonClass;
using DTO.Report;

namespace DAL.Repositories
{
    public class AuditRepository : Repository, IAuditRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;

        public AuditRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {
            _ApplicationContext = applicationContext;
        }

        public async Task<int> AddAudit(AudTransaction entity)
        {
            _context.AudTransactions.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity.Id;
            else
                return 0;

        }

        public Task<AudTransaction> GetAuditDetailsById(int id)
        {
            return _context.AudTransactions
                 .Include(x => x.AudTranCuContacts)
                 .Include(x => x.AudTranSuContacts)
                 .Include(x => x.AudTranFaContacts)
                 .Include(x => x.AudTranServiceTypes)
                 .Include(x => x.AudTranFaProfiles)
                 .Include(x => x.AudTranFileAttachments)
                 .Include(x => x.CreatedByNavigation)
                 .Include(x => x.AudTranAuditors)
                 .Include(x => x.AudTranCS)
                 .Include(x => x.AudTranWorkProcesses)
                 .Include(x => x.AudTranCancelReschedules)
                 .Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<AudTranFileAttachment> GetFile(int id)
        {
            return await _context.AudTranFileAttachments.FirstOrDefaultAsync(x => x.Id == id && x.Active);
        }
        public async Task<AudTranReport1> GetAuditReport(int id)
        {
            return await _context.AudTranReports1.FirstOrDefaultAsync(x => x.Id == id && x.Active);
        }
        public Task<List<AudEvaluationRound>> GetEvaluationRound()
        {
            return _context.AudEvaluationRounds
                .Where(x => x.Active)
                .OrderBy(x => x.Name).ToListAsync();
        }

        public Task<int> UpdateAudit(AudTransaction entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AudTranFileAttachment>> GetReceptFiles(int auditid, IEnumerable<string> GuidList)
        {
            return await _context.AudTranFileAttachments.Where(x => x.AuditId == auditid && GuidList.Contains(x.UniqueId) && x.Active).ToListAsync();
        }

        public async Task<IEnumerable<AudTranReport1>> GetReportFiles(int auditid, IEnumerable<Guid> GuidList)
        {
            return await _context.AudTranReports1.Where(x => x.AuditId == auditid && GuidList.Contains(x.GuidId) && x.Active).ToListAsync();
        }

        public Task<List<AudBookingRule>> GetAuditBookingRule()
        {
            return _context.AudBookingRules
                   .Where(x => x.Active).ToListAsync();
        }

        public Task<AudTransaction> GetCusLastReportNo(int customerid)
        {
            return _context.AudTransactions
                .Where(x => x.CustomerId == customerid).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        }

        public Task<bool> IsReportNoExists(string reportno, int customerid)
        {
            return _context.AudTransactions
                .AnyAsync(x => x.ReportNo == reportno && x.CustomerId == customerid);
        }

        public Task<List<AudBookingContact>> GetAuditBookingContacts(int OfficeId)
        {
            return _context.AudBookingContacts
                .Include(x => x.Office)
                  .Where(x => x.Active && x.OfficeId == OfficeId).ToListAsync();
        }
        public Task<AudTransaction> GetAuditCancelDetails(int auditid)
        {
            return _context.AudTransactions
                   .Include(x => x.Customer)
                   .Include(x => x.Supplier)
                   .Include(x => x.Factory)
                   .Include(x => x.AudTranServiceTypes)
                   .ThenInclude(x => x.ServiceType)
                   .Include(x => x.Office)
                   .Include(x => x.AudTranCancelReschedules)
                   .Where(x => x.Id == auditid).FirstOrDefaultAsync();
        }

        public async Task<List<AudStatus>> GetAuditStatus()
        {
            return await _context.AudStatuses
                  .Where(x => x.Active != null && x.Active.Value).ToListAsync();
        }

        /// <summary>
        /// get audit data as iqueryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<AudTransaction> GetAuditMainData()
        {
            return _context.AudTransactions;
        }

        /// <summary>
        /// get factory country details by audit ids
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<List<AuditFactoryCountryRepoResponse>> GetFactoryCountryDataByAudit(IQueryable<int> auditIds)
        {
            int addressTypeHeadOfficeId = (int)Supplier_Address_Type.HeadOffice;

            return await (from fact in _context.SuAddresses
                          join aud in _context.AudTransactions on fact.SupplierId equals aud.FactoryId
                          where auditIds.Contains(aud.Id) && fact.AddressTypeId == addressTypeHeadOfficeId
                          select new AuditFactoryCountryRepoResponse
                          {
                              AuditId = aud.Id,
                              FactoryCountryName = fact.Country.CountryName,
                              FactoryState = fact.City.Province.ProvinceName,
                              FactoryCity = fact.City.CityName
                          }).ToListAsync();
        }

        /// <summary>
        /// get quotation data by audit
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<List<AuditQuotationRepoResponse>> GetQuotationDataByAudit(IQueryable<int> auditIds)
        {
            int quotationCanceled = (int)QuotationStatus.Canceled;

            return await (from quot in _context.QuQuotations
                          join quAudit in _context.QuQuotationAudits on quot.Id equals quAudit.IdQuotation
                          join aud in _context.AudTransactions on quAudit.IdBooking equals aud.Id
                          where auditIds.Contains(aud.Id) && quot.IdStatus != quotationCanceled
                          select new AuditQuotationRepoResponse
                          {
                              StatusId = quot.IdStatusNavigation.Id,
                              AuditId = aud.Id,
                              StatusName = quot.IdStatusNavigation.Label,
                          }).ToListAsync();
        }

        /// <summary>
        /// get service type details by audit ids
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<List<AuditServiceTypeRepoResponse>> GetServiceTypeDataByAudit(IQueryable<int> auditIds)
        {
            return await _context.AudTranServiceTypes
                    .Where(x => auditIds.Contains(x.AuditId) && x.Active)
                    .Select(x => new AuditServiceTypeRepoResponse
                    {
                        AuditId = x.AuditId,
                        //ServiceTypeId = x.ServiceTypeId,
                        ServiceTypeName = x.ServiceType.Name
                    }).Distinct().ToListAsync();
        }

        /// <summary>
        /// get auditor details by audit id
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<List<AuditAuditorRepoResponse>> GetAuditorDataByAudit(IQueryable<int> auditIds)
        {
            return await _context.AudTranAuditors
                    .Where(x => auditIds.Contains(x.AuditId) && x.Active)
                    .Select(x => new AuditAuditorRepoResponse
                    {
                        AuditId = x.AuditId,
                        //AuditorId = x.Staff.Id,
                        AuditorName = x.Staff.PersonName
                    }).Distinct().ToListAsync();
        }

        public Task<List<AudCancelRescheduleReason>> GetAuditCancelRescheduleReasons(int? customerid, int optypeid)
        {
            var data = _context.AudCancelRescheduleReasons
                  .Where(x => x.Active);

            if (customerid != null && data.Any(x => x.CustomerId == customerid.Value))
            {
                data = data.Where(x => x.CustomerId == customerid.Value);
            }
            else
            {
                data = data.Where(x => x.IsDefault);
            }
            if (optypeid == (int)AuditBookingOperationType.Cancel)
            {
                data = data.Where(x => x.IsCancel);
            }
            else if (optypeid == (int)AuditBookingOperationType.Reschedule)
            {
                data = data.Where(x => x.IsReschedule);
            }

            if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
            {
                data = data.Where(x => x.IsSgT);
            }

            return data.ToListAsync();

        }

        public Task<List<AudType>> GetAuditType()
        {
            return _context.AudTypes.Where(x => x.Active).ToListAsync();
        }

        public async Task<List<AudWorkProcess>> GetAuditWorkProcess()
        {
            return await _context.AudWorkProcesses.Where(x => x.Active).ToListAsync();
        }
        public Task<AudTransaction> GetAuditBasicDetails(int auditid)
        {
            return _context.AudTransactions
                   .Include(x => x.Customer)
                   .Include(x => x.Supplier)
                   .Include(x => x.Factory)
                   .Include(x => x.AudTranServiceTypes)
                   .ThenInclude(x => x.ServiceType)
                   .Where(x => x.Id == auditid).FirstOrDefaultAsync();
        }

        public Task<List<AudTranAuditor>> GetScheduledAuditors(int auditid)
        {
            return _context.AudTranAuditors
                .Include(x => x.Staff)
                .Where(x => x.AuditId == auditid && x.Active).ToListAsync();
        }

        public Task<AudTransaction> GetAuditReportDetails(int auditid)
        {
            return _context.AudTransactions
                 .Include(x => x.AudTranReport1S)
                 .Include(x => x.AudTranReports)
                 .Include(x => x.AudTranAuditors)
                 .Include(x => x.AudTranReportDetails)
                 .Where(x => x.Id == auditid).FirstOrDefaultAsync();
        }

        public Task<List<AudTransaction>> GetAuditDetailsByListId(IEnumerable<int> ListAuditid)
        {
            return _context.AudTransactions
                  .Include(x => x.AudTranCuContacts)
                  .Include(x => x.AudTranSuContacts)
                  .Include(x => x.AudTranFaContacts)
                  .Include(x => x.AudTranServiceTypes)
                  .Where(x => ListAuditid.Contains(x.Id)).ToListAsync();
        }

        public async Task<BookingDate> getAuditBookingDateDetails(int bookingId)
        {
            return await _context.AudTransactions.Where(x => x.Id == bookingId).Select(x => new BookingDate
            {
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo
            }).FirstOrDefaultAsync();

        }

        public async Task<List<BookingDate>> getListAuditBookingDateDetails(List<int> lstbookingId)
        {
            return await _context.AudTransactions.Where(x => lstbookingId.Contains(x.Id)).Select(x => new BookingDate
            {
                BookingId = x.Id,
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo
            }).ToListAsync();

        }
        /// <summary>
        /// get service type details by audit id
        /// </summary>
        /// <param name="auditId"></param>
        /// <returns></returns>
        public async Task<List<AuditServiceTypeRepoResponse>> GetServiceTypeDataByAudit(int auditId)
        {
            return await _context.AudTranServiceTypes
                    .Where(x => x.AuditId == auditId && x.Active)
                    .Select(x => new AuditServiceTypeRepoResponse
                    {
                        AuditId = x.AuditId,
                        ServiceTypeName = x.ServiceType.Name
                    }).Distinct().ToListAsync();
        }
        /// <summary>
        /// get factory country detail by audit id
        /// </summary>
        /// <param name="auditId"></param>
        /// <returns></returns>
        public async Task<List<AuditFactoryCountryRepoResponse>> GetFactoryCountryDataByAudit(int auditId)
        {
            int addressTypeHeadOfficeId = (int)Supplier_Address_Type.HeadOffice;

            return await (from fact in _context.SuAddresses
                          join aud in _context.AudTransactions on fact.SupplierId equals aud.FactoryId
                          where aud.Id == auditId && fact.AddressTypeId == addressTypeHeadOfficeId
                          select new AuditFactoryCountryRepoResponse
                          {
                              AuditId = aud.Id,
                              FactoryCountryName = fact.Country.CountryName,
                          }).ToListAsync();
        }
        //get audit info
        public async Task<BookingDataRepo> GetAuditDetails(int AuditId)
        {
            return await _context.AudTransactions
                .Where(x => x.Id == AuditId)
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
                }).FirstOrDefaultAsync();
        }
        //queryable  audit no
        public IQueryable<int> GetAuditNo()
        {
            return _context.AudTransactions.Select(x => x.Id);
        }

        public async Task<List<AuditServiceTypeData>> GetAuditServiceTypeList(List<int> auditIds)
        {
            return await _context.AudTranServiceTypes
                    .Where(x => auditIds.Contains(x.AuditId) && x.Active)
                    .Select(x =>

                    new AuditServiceTypeData
                    {
                        AuditId = x.AuditId,
                        ServiceTypeId = x.ServiceTypeId,
                        ServiceTypeName = x.ServiceType.Name
                    }).Distinct().ToListAsync();
        }

        public async Task<List<AuditCustomerContactData>> GetAuditContacts(List<int> auditIds)
        {
            return await _context.AudTranCuContacts
                    .Where(x => auditIds.Contains(x.AuditId) && x.Active)
                    .Select(x => new AuditCustomerContactData
                    {
                        AuditId = x.AuditId,
                        ContactId = x.ContactId,
                        ContactName = x.Contact.ContactName
                    }).Distinct().ToListAsync();
        }

        public async Task<List<AuditSupplierCustomerRepoResponse>> GetSupplierCustomerDataByAudit(IEnumerable<int> auditIds)
        {
            return await (from aud in _context.AudTransactions.Where(x => auditIds.Contains(x.Id))
                          join sup in _context.SuSupplierCustomers on new { aud.SupplierId, aud.CustomerId } equals new { sup.SupplierId, sup.CustomerId }
                          select new AuditSupplierCustomerRepoResponse
                          {
                              AuditId = aud.Id,
                              SupplierCustomerCode = sup.Code
                          }).Distinct().ToListAsync();
        }

        public async Task<List<AuditFactoryCustomerRepoResponse>> GetFactoryCustomerDataByAudit(IEnumerable<int> auditIds)
        {
            return await (from aud in _context.AudTransactions.Where(x => auditIds.Contains(x.Id))
                          join sup in _context.SuSupplierCustomers on new { SupplierId = aud.FactoryId, aud.CustomerId } equals new { sup.SupplierId, sup.CustomerId }
                          select new AuditFactoryCustomerRepoResponse
                          {
                              AuditId = aud.Id,
                              FactoryCustomerCode = sup.Code
                          }).Distinct().ToListAsync();
        }
        public async Task<List<AudTranFaProfile>> GetAuditTranFaProfiles(List<int> auditIds)
        {
            return await _context.AudTranFaProfiles.Where(x => auditIds.Contains(x.AuditId) && x.Active)
                .ToListAsync();
        }

        /// <summary>
        /// get audit details
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AuditCusFactDetails>> GetAuditDetails(IEnumerable<int> auditIds)
        {
            return await _context.AudTransactions.Where(x => auditIds.Contains(x.Id)).
                Select(x => new AuditCusFactDetails
                {
                    AuditId = x.Id,
                    CustomerName = x.Customer.CustomerName,
                    FactoryName = x.Factory.SupplierName,
                    StatusName = x.Status.Status,
                    ServiceFromDate = x.ServiceDateFrom
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// factory details by audit ids
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<List<FactoryCountry>> GetFactoryAddressDetailsByAuditIds(IEnumerable<int> auditIds)
        {
            int addressTypeHeadOfficeId = (int)Supplier_Address_Type.HeadOffice;

            return await (from fact in _context.SuAddresses
                          join aud in _context.AudTransactions on fact.SupplierId equals aud.FactoryId
                          where auditIds.Contains(aud.Id) && fact.AddressTypeId == addressTypeHeadOfficeId
                          select new FactoryCountry
                          {
                              FactoryCountryId = fact.CountryId,
                              FactoryProvinceId = fact.RegionId,
                              FactoryCityId = fact.CityId,
                              FactoryCountyId = fact.CountyId.GetValueOrDefault(),
                              FactoryZoneId = fact.County.ZoneId.GetValueOrDefault(),
                              BookingId = aud.Id,
                              FactoryAdress = fact.Address,
                              FactoryRegionalAddress = fact.LocalLanguage,
                              CountryName = fact.Country.CountryName,
                              ProvinceName = fact.Region.ProvinceName,
                              CityName = fact.City.CityName,
                              CountyName = fact.County.CountyName,
                              ZoneName = fact.County.Zone.Name,
                              TownName = fact.Town.TownName,
                              TownId = fact.TownId.GetValueOrDefault()
                          }).AsNoTracking().ToListAsync();
        }

        public async Task<AudTransaction> GetAuditData(int auditId)
        {
            return await _context.AudTransactions.IgnoreQueryFilters().
                Include(x => x.CuProductCategoryNavigation).FirstOrDefaultAsync(x => x.Id == auditId);
        }

        public async Task<FbAuditData> GetAuditForFbReportDetails(int auditId)
        {
            return await _context.AudTransactions.Where(x => x.Id == auditId)
                .Select(x => new FbAuditData
                {
                    FbCustomerId = x.Customer.FbCusId,
                    FbSupplierId = x.Supplier.FbFactSupId,
                    FbFactoryId = x.Factory.FbFactSupId,
                    FbServiceId = x.AudTranServiceTypes.FirstOrDefault().ServiceType.FbServiceTypeId,
                    ServiceFromDate = x.ServiceDateFrom,
                    ServiceToDate = x.ServiceDateTo,
                    AuditId = x.Id,
                    Office = x.Office.LocationName,
                    AuditType = x.AuditType.Name,
                    Evalution = x.Evalution.Name,
                    CustomerId = x.CustomerId,
                    FactoryId = x.FactoryId,
                    SupplierId = x.SupplierId,
                }).AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        public async Task<List<AuditCustomerContactData>> GetAuditCustomerContacts(int auditId)
        {
            return await _context.AudTranCuContacts
                    .Where(x => x.AuditId == auditId && x.Active)
                    .Select(x => new AuditCustomerContactData
                    {
                        AuditId = x.AuditId,
                        ContactName = x.Contact.ContactName,
                        ContactEmail = x.Contact.Email
                    }).AsNoTracking().ToListAsync();
        }

        public async Task<List<AuditCustomerContactData>> GetAuditSupplierContacts(int auditId)
        {
            return await _context.AudTranSuContacts
                    .Where(x => x.AuditId == auditId && x.Active)
                    .Select(x => new AuditCustomerContactData
                    {
                        AuditId = x.AuditId,
                        ContactName = x.Contact.ContactName,
                        ContactEmail = x.Contact.Mail
                    }).AsNoTracking().ToListAsync();
        }

        public async Task<List<AudTranAuditor>> GetAuditorDetails(int auditid)
        {
            return await _context.AudTranAuditors
                .Where(x => x.AuditId == auditid && x.Active).AsNoTracking().ToListAsync();
        }

        public async Task<List<AudTranC>> GetAuditCSDetails(int auditid)
        {
            return await _context.AudTranCs
                .Where(x => x.AuditId == auditid && x.Active).AsNoTracking().ToListAsync();
        }

        public async Task<List<AudTranFileAttachment>> GetAuditTranFiles(int auditid)
        {
            return await _context.AudTranFileAttachments.Where(x => x.AuditId == auditid).ToListAsync();
        }

        public IQueryable<AudCuProductCategory> GetProductCategory()
        {
            return _context.AudCuProductCategories.Where(x => x.Active);
        }

        public async Task<AudBookingEmailConfiguration> GetCCEmailConfigurationEmailsByCustomer(int customerId, int? factoryCountryId, int bookingStatusId)
        {
            return await _context.AudBookingEmailConfigurations
                .FirstOrDefaultAsync(x => x.Active && x.CustomerId == customerId && x.AuditStatusId == bookingStatusId && x.FactoryCountryId == factoryCountryId);
        }

        public async Task<List<AudFbReportCheckpoint>> GetAuditFbReportCheckpointByAuditIds(IQueryable<int> auditIds)
        {
            return await _context.AudFbReportCheckpoints.Where(x => auditIds.Contains(x.AuditId.GetValueOrDefault())).AsNoTracking().ToListAsync();
        }

        public Task<ItUserMaster> GetUserName(int userId)
        {
            return _context.ItUserMasters.Where(x => x.Id == userId && x.Active).FirstOrDefaultAsync();
        }

        public async Task<List<BookingServiceType>> GetAuditBookingServiceTypes(IEnumerable<int> bookingIds)
        {
            return await _context.AudTranServiceTypes.Where(x => x.Active && bookingIds.Contains(x.AuditId)).
                    Select(x => new BookingServiceType
                    {
                        BookingNo = x.AuditId,
                        ServiceTypeId = x.ServiceTypeId,
                        ServiceTypeName = x.ServiceType.Name
                    }).ToListAsync();
        }

        public async Task<InspectionBookingDetail> GetAuditBookingDetails(int bookingId)
        {
            return await _context.AudTransactions.Where(x => x.Id == bookingId)
                        .Select(x => new InspectionBookingDetail()
                        {
                            InspectionId = x.Id,
                            InternalReferencePo = x.PoNumber,
                            CustomerId = x.CustomerId,
                            SupplierId = x.SupplierId,
                            FactoryId = x.FactoryId,
                            StatusId = x.StatusId,
                            StatusName = x.Status.Status,
                            SeasonId = x.SeasonId,
                            SeasonYearId = x.SeasonYearId,
                            ServiceDateFrom = x.ServiceDateFrom,
                            ServiceDateTo = x.ServiceDateTo,
                            CusBookingComments = x.CusBookingComments,
                            ApiBookingComments = x.ApiBookingComments,
                            InternalComments = x.InternalComments,
                            OfficeId = x.OfficeId,
                            CreatedBy = x.CreatedBy,
                            CreatedOn = x.CreatedOn,
                            EntityId = x.EntityId.GetValueOrDefault(),
                            ApplicantName = x.ApplicantName,
                            ApplicantEmail = x.ApplicantEmail,
                            ApplicantPhoneNo = x.ApplicantPhNo,
                            CustomerBookingNo = x.CustomerBookingNo,
                            CreatedUserType = x.CreatedByNavigation.UserTypeId,
                            CuProductCategory = x.CuProductCategory,
                            CustomerName = x.Customer.CustomerName,
                            SupplierName = x.Supplier.SupplierName,
                            FactoryName = x.Factory.SupplierName,
                            GuidId = x.GuidId,
                            CustomerSeasonId = x.SeasonId,
                            SeasonYear = x.SeasonYear.Year,
                            SeasonName = x.Season.Name,
                            SeasonYearName = x.SeasonYear.Id.ToString(),
                            BrandId = x.BrandId,
                            BrandName = x.Brand.Name,
                            DepartmentId = x.DepartmentId,
                            DepartmentName = x.Department.Name,
                            BookingType = x.AuditTypeId,
                            IsEaqf = x.IsEaqf,
                            BookingTypeName = x.AuditType.Name
                        }).AsNoTracking().FirstOrDefaultAsync();
        }



        public async Task<List<InspectionSupplierFactoryContacts>> GetFactoryContactsByBookingIds(List<int> bookingIds)
        {
            return await _context.AudTranFaContacts.Where(x => x.Active && bookingIds.Contains(x.AuditId) && x.Contact.Active.Value)
                .Select(x => new InspectionSupplierFactoryContacts
                {
                    InspectionId = x.AuditId,
                    ContactName = x.Contact.ContactName,
                    ContactEmail = x.Contact.Mail,
                    Phone = x.Contact.Phone
                }).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<InspectionSupplierFactoryContacts>> GetSupplierContactsByBookingIds(List<int> bookingIds)
        {
            return await _context.AudTranSuContacts.Where(x => x.Active && bookingIds.Contains(x.AuditId) && x.Contact.Active.Value)
                .Select(x => new InspectionSupplierFactoryContacts
                {
                    InspectionId = x.AuditId,
                    ContactName = x.Contact.ContactName,
                    ContactEmail = x.Contact.Mail,
                    Phone = x.Contact.Phone
                }).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<BookingFileAttachment>> GetAuditBookingMappedFiles(IEnumerable<int> bookingIds)
        {
            return await _context.AudTranFileAttachments.Where(x => x.Active && bookingIds.Contains(x.AuditId))
                    .Select(x => new BookingFileAttachment()
                    {
                        FileName = x.FileName,
                        FileDescription = "",
                        Id = x.Id,
                        BookingId = x.AuditId,
                        IsNew = false,
                        uniqueld = x.UniqueId,
                        FileUrl = x.FileUrl,
                        IsBookingEmailNotification = true,
                        IsReportSendToFB = true,
                    }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<MidTask>> GetTask(int bookingId, IEnumerable<int> typeIdList, bool isdone)
        {
            return await _context.MidTasks.Where(x => x.LinkId == bookingId && typeIdList.Contains(x.TaskTypeId) && x.IsDone == isdone).ToListAsync();
        }

        public async Task<AudTransaction> GetAuditInspectionCustomerContactByID(int inspectionID)
        {
            return await _context.AudTransactions.Where(x => x.Id == inspectionID)
                .Include(x => x.AudTranCuContacts)
                .ThenInclude(x => x.Contact)
                .Include(x => x.CreatedByNavigation)
                .FirstAsync();
        }

        public async Task<CuCheckPoint> GetAuditServiceCustomersByCustomerId(int customerId)
        {
            return await _context.CuCheckPoints
                .Include(x => x.CuCheckPointsBrands)
                .Include(x => x.CuCheckPointsDepartments)
                .Include(x => x.CuCheckPointsServiceTypes)
                .Where(x => x.Active && x.CustomerId == customerId && x.ServiceId == (int)Service.AuditId &&
                       x.CheckpointTypeId == (int)CheckPointTypeEnum.QuotationRequired).FirstOrDefaultAsync();
        }

        public async Task<List<CommonDataSource>> GetAuditCSByAuditIds(IEnumerable<int> auditids)
        {
            return await _context.AudTranCs.Where(x => auditids.Contains(x.AuditId) && x.Active)
                .Select(y => new CommonDataSource()
                {
                    Id = y.AuditId,
                    Name = y.Staff.PersonName
                }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ServiceTypeList>> GetServiceType(IEnumerable<int> bookingId)
        {
            return await _context.AudTranServiceTypes
                  .Where(x => bookingId.Contains(x.AuditId) && x.Active)
                  .Select(x => new ServiceTypeList
                  {
                      InspectionId = x.AuditId,
                      serviceTypeId = x.ServiceTypeId,
                      serviceTypeName = x.ServiceType.Name,
                      IsAutoQCExpenseClaim = x.ServiceType.IsAutoQcexpenseClaim.GetValueOrDefault()
                  }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EaqfInvoiceDetails>> GetAuditEaqfBookingInvoiceDetails(List<int> bookingIds)
        {
            return await _context.InvManTransactions.Where(x => x.Status != 4 && bookingIds.Contains(x.AuditId.GetValueOrDefault())).
                Select(x => new EaqfInvoiceDetails
                {
                    BookingId = x.AuditId.GetValueOrDefault(),
                    InvoiceTotal = x.TotalAmount.GetValueOrDefault(),
                    InvoiceNo = x.InvoiceNo
                }).AsSingleQuery().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EaqfAuditReportDetails>> GetAuditEaqfBookingReportDetails(List<int> bookingIds)
        {
            return await _context.AudTransactions
                .Where(x => bookingIds.Contains(x.Id)).
                Select(x => new EaqfAuditReportDetails
                {
                    BookingId = x.Id,
                    ReportLink = x.FinalReportPath,
                    ReportTitle = x.ReportNo,
                    ReportStatus = x.Status.Status,
                    Score = x.ScoreValue
                }).AsNoTracking().ToListAsync();
        }
    }
}
