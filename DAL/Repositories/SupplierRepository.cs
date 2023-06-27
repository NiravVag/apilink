using Contracts.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Enums;
using DTO.Supplier;
using DTO.UserAccount;
using DTO.Quotation;
using DTO.FullBridge;
using DTO.CommonClass;
using DTO.DefectDashboard;
using DTO.User;

namespace DAL.Repositories
{
    public class SupplierRepository : Repository, ISupplierRepository
    {


        public SupplierRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<int> AddSupplier(SuSupplier entity)
        {
            _context.SuSuppliers.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> EditSupplier(SuSupplier entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public Task<SuSupplier> GetSupplierDetails(int id)
        {
            return _context.SuSuppliers
                .Include(x => x.SuAddresses)
                .Include(x => x.SuApiServices)
                .Include(x => x.SuContacts)
                .ThenInclude(x => x.SuContactApiServices)
                .Include(x => x.SuSupplierCustomers)
                .ThenInclude(x => x.Customer)
                .Include(x => x.SuSupplierCustomerContacts)
                .Include(x => x.AudTransactionFactories)
                .Include(x => x.AudTransactionSuppliers)
                .Include(x => x.InspTransactionFactories)
                .Include(x => x.InspTransactionSuppliers)
                .Include(x => x.CuPoFactories)
                .Include(x => x.CuPoSuppliers)
                .Include(x => x.SuSupplierFactorySuppliers)
                .Include(x => x.SuSupplierFactoryParents)
                .ThenInclude(x => x.Supplier)
                .Include(x => x.SuContacts)
                .ThenInclude(x => x.SuContactEntityMaps)
                .Include(x => x.SuContacts)
                .ThenInclude(x => x.SuContactEntityServiceMaps)
                .ThenInclude(x => x.Entity)
                .Include(x => x.SuContacts)
                .ThenInclude(x => x.SuContactEntityServiceMaps)
                .ThenInclude(x => x.Service)
                .Include(x => x.SuEntities)
                .FirstOrDefaultAsync(x => x.Id == id);

        }
        /// <summary>
        /// Get Supplier or factory details 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SuSupplier> GetSupplierORFactoryDetails(int id)
        {
            return await _context.SuSuppliers
                .Include(x => x.SuAddresses)
                .Include(x => x.SuContacts)
                .Include(x => x.SuSupplierCustomers)
                 .ThenInclude(x => x.Customer)
                .Include(x => x.SuSupplierCustomerContacts)
                .Include(x => x.SuSupplierFactorySuppliers)
                .Include(x => x.SuSupplierFactoryParents)
                .ThenInclude(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.Id == id);

        }
        public async Task<bool> IsSupplierExists(SupplierDetails request)
        {
            var reqHead = !request.IsFromBookingPage ? request.AddressList.FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice) :
                          request.AddressList.FirstOrDefault();
            return await _context.SuSuppliers.AnyAsync(x => x.SupplierName.Trim().ToUpper() == request.Name.Trim().ToUpper() && x.TypeId != null && x.TypeId.Value == request.TypeId
                    && x.SuAddresses.Any(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice && y.CountryId == reqHead.CountryId));
        }




        public Task<List<SuType>> GetTypes()
        {
            return _context.SuTypes.ToListAsync();
        }

        public Task<List<SuLevel>> GetLevels()
        {
            return _context.SuLevels.ToListAsync();
        }

        public Task<List<SuOwnlerShip>> GetOwners()
        {
            return _context.SuOwnlerShips.ToListAsync();
        }

        public async Task<bool> RemoveSupplier(int id)
        {
            var entity = await _context.SuSuppliers.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            _context.Entry(entity).State = EntityState.Deleted;
            int numReturn = await _context.SaveChangesAsync();

            return numReturn > 0;

        }

        public Task<List<SuAddressType>> GetAddressTypes()
        {
            return _context.SuAddressTypes.ToListAsync();
        }

        public Task<List<SuSupplier>> GetSupplierByCustomerId(int customerId)
        {
            return _context.SuSuppliers
                 .Include(x => x.SuSupplierCustomers)
                 .Where(x => x.Active && x.TypeId != (int)Supplier_Type.Factory && x.SuSupplierCustomers.Any(y => y.CustomerId == customerId)).OrderBy(x => x.SupplierName).ToListAsync();
        }

        public Task<List<SuSupplier>> GetFactoryByCustomerId(int customerId)
        {
            return _context.SuSuppliers
                .Include(x => x.SuSupplierCustomers)
                .Where(x => x.Active && x.TypeId == (int)Supplier_Type.Factory && x.SuSupplierCustomers.Any(y => y.CustomerId == customerId)).OrderBy(x => x.SupplierName).ToListAsync();
        }

        public Task<List<SuContact>> GetSuppliercontactById(int Supid, int cusid)
        {
            return _context.SuContacts
                .Include(x => x.SuSupplierCustomerContacts)
                .Where(x => x.Active.Value && x.SupplierId == Supid && x.SuSupplierCustomerContacts.Any(y => y.CustomerId == cusid)).ToListAsync();

        }

        public Task<List<SuContact>> GetSupplierContactBySupId(int supid)
        {
            return _context.SuContacts.Where(x => x.SupplierId == supid && x.Active == true).ToListAsync();
        }

        public Task<List<SuSupplier>> GetFactoryBySupplierId(int Supid)
        {
            return _context.SuSuppliers
                .Include(x => x.SuSupplierFactoryParents)
                .Include(x => x.SuAddresses)
                .Where(x => x.Active && x.TypeId == (int)Supplier_Type.Factory && x.SuSupplierFactoryParents.Any(z => z.SupplierId == Supid)).ToListAsync();

        }

        public Task<List<SuSupplier>> GetFactoryByCustomerIdSupplierId(int? customerId, int? supplierId)
        {
            return _context.SuSuppliers
               .Include(x => x.SuSupplierCustomers)
               .Include(x => x.SuSupplierFactoryParents)
               .Where(x => x.Active && x.SuSupplierFactoryParents.Any(z => z.SupplierId == supplierId)
               && x.TypeId == (int)Supplier_Type.Factory && x.SuSupplierCustomers.Any(y => y.CustomerId == customerId)).ToListAsync();
        }

        public Task<List<SuSupplier>> GetSupplierByfactId(int factid)
        {
            return _context.SuSuppliers
                  .Include(x => x.SuSupplierFactorySuppliers)
                  .Where(x => x.Active && x.SuSupplierFactorySuppliers.Any(y => y.ParentId == factid)).ToListAsync();
        }

        public IEnumerable<SuSupplier> GetAllSuppliers()
        {
            return _context.SuSuppliers
                                  .Where(x => x.Active).OrderBy(x => x.SupplierName).ToList();
        }

        public Task<List<SuContact>> GetSupplierContactsList(List<int> contactids)
        {
            return _context.SuContacts
               .Where(x => x.Active.Value && contactids.Contains(x.Id)).OrderBy(x => x.ContactName).ToListAsync();
        }

        public Task<List<SuCreditTerm>> GetCreditTerms()
        {
            return _context.SuCreditTerms.ToListAsync();
        }

        public Task<List<SuStatus>> GetStatus()
        {
            return _context.SuStatuses.ToListAsync();
        }

        public IQueryable<SuSupplier> GetAllSuppliersByName(string supName)
        {
            return _context.SuSuppliers
                .Include(x => x.SuSupplierCustomers)
                .Where(x => x.Active && x.SupplierName != null && EF.Functions.Like(x.SupplierName, $"%{supName.Trim()}%")).OrderBy(x => x.SupplierName);
        }

        /// <summary>
        /// get supplier or factory list with Address
        /// </summary>
        /// <param name="supplierName"></param>
        /// <returns></returns>
        public IQueryable<SuSupplier> GetAllSuppliersAndCountryList()
        {
            return _context.SuSuppliers
                .Include(x => x.SuSupplierCustomers)
                .Include(x => x.SuAddresses)
                .Where(x => x.Active);
        }


        /// <summary>
        /// Get the Supplier Data IQueryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<SuSupplier> GetSupplierDataSource()
        {
            return _context.SuSuppliers.Where(x => x.Active).OrderBy(x => x.SupplierName);
        }

        public async Task<SupplierAddress> GetSupplierHeadOfficeAddress(int supplierId)
        {
            return await _context.SuAddresses.Where(x => x.SupplierId == supplierId &&
            x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Select(x => new SupplierAddress
            {
                RegionalAddress = x.LocalLanguage,
                Address = x.Address,
                countryId = x.CountryId,
                Country = x.Country.CountryName,
                CityId = x.CityId,
                ProvinceId = x.RegionId,
                SupplierName = x.Supplier.SupplierName,
                RegionalSupplierName = x.Supplier.LocalName,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).FirstOrDefaultAsync();
        }

        //Fetch Supplier contacts based on supplier ID
        public Task<List<Contact>> GetSupplierContactById(int supid)
        {
            return _context.SuContacts.Where(x => x.SupplierId == supid && x.Active == true)
                .Select(x => new Contact
                {
                    ContactId = x.Id,
                    ContactName = x.ContactName,
                    ContactEmail = x.Mail,
                    ContactPhone = x.Phone
                }).ToListAsync();

        }

        //Fetch Supplier Code
        public async Task<List<SupplierCode>> GetSupplierCode(List<int> customerId, List<int> supplierIds)
        {
            return await _context.SuSupplierCustomers.Where(x => supplierIds.Contains(x.SupplierId) && customerId.Contains(x.CustomerId))
                .Select(y => new SupplierCode
                {
                    CustomerId = y.CustomerId,
                    Code = y.Code,
                    SupplierId = y.SupplierId
                }).ToListAsync();
        }

        /// <summary>
        /// fetch supplier code by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<SupplierCode>> GetSupplierCode(IQueryable<int> bookingIds)
        {
            return await _context.SuSupplierCustomers.Where(x => x.Supplier.Active && x.Supplier.InspTransactionSuppliers.Any(y => bookingIds.Contains(y.Id)))
                .Select(y => new SupplierCode
                {
                    CustomerId = y.CustomerId,
                    Code = y.Code,
                    SupplierId = y.SupplierId
                }).ToListAsync();
        }

        public async Task<string> GetSupplierCode(int Supid, int cusid)
        {
            return await _context.SuSupplierCustomers.Where(x => x.CustomerId == cusid && x.SupplierId == Supid)
                .Select(x => x.Code).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Get supplier head office by supplierid
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<string> GetSupplierHeadOfficeAddressById(int supplierId)
        {
            return await _context.SuAddresses.Where(x => x.SupplierId == supplierId
                                    && x.AddressTypeId == (int)SuAddressTypeEnum.Headoffice).
                                    Select(x => x.Address).FirstOrDefaultAsync();

        }

        /// <summary>
        /// Get supplier office by supplierId 
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<List<SupplierAddress>> GetSupplierOfficeAddressBylstId(List<int> lstsupplierId)
        {
            return await _context.SuAddresses.Where(x => lstsupplierId.Contains(x.SupplierId))
                .Select(y => new SupplierAddress
                {
                    Address = y.Address,
                    Id = y.Id,
                    RegionalAddress = y.LocalLanguage,
                    SupplierId = y.SupplierId,
                    SupplierAddresstype = y.AddressTypeId.Value
                }).ToListAsync();
        }
        /// <summary>
        /// Get supplier Address By Query Id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<List<SupplierAddress>> GetSupplierOfficeAddressBySupplierIds(List<int> lstsupplierId)
        {
            return await _context.SuAddresses.Where(x => lstsupplierId.Contains(x.SupplierId))
                .Select(y => new SupplierAddress
                {
                    Address = y.Address,
                    Id = y.Id,
                    RegionalAddress = y.LocalLanguage,
                    SupplierId = y.SupplierId,
                    SupplierAddresstype = y.AddressTypeId.Value
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get supplier Id by countryId
        /// </summary>
        /// <param name="countrylist"></param>
        /// <returns>supplierIds</returns>
        public async Task<List<int>> GetFactoryByCountryId(List<int> countrylist)
        {
            return await _context.SuAddresses.Where(x => x.Supplier.TypeId == (int)Supplier_Type.Factory && countrylist.Contains(x.CountryId)).AsNoTracking()
                .Select(x => x.SupplierId).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get supplier Id by provinceId
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns>supplierIds</returns>
        public async Task<List<int>> GetSupplierByProvinceId(int provinceId)
        {
            return await _context.SuAddresses.Where(x => x.RegionId == provinceId)
                .Select(x => x.SupplierId).ToListAsync();
        }

        /// <summary>
        /// Get supplier and address by supplier id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetSupplierById(List<int> supplierIdList)
        {
            return await _context.SuSuppliers.Where(x => x.Active && supplierIdList.Contains(x.Id))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.SupplierName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get supplier id by factoryid
        /// </summary>
        /// <param name="factoryId"></param>
        /// <returns></returns>
        public async Task<int> GetFactoryIdBySupplierId(int supplierId)
        {
            return await _context.SuSupplierFactories.Where(x => x.SupplierId == supplierId).
                                Select(x => x.ParentId).FirstOrDefaultAsync();
        }

        public async Task<FBSupplierMasterData> GetFBSupplierData(int supplierId)
        {
            return await _context.SuSuppliers.
                Where(x => x.Id == supplierId && x.Active).
                   Select(x => new FBSupplierMasterData
                   {
                       SupplierId = x.Id,
                       TypeId = x.TypeId,
                       SupplierName = x.SupplierName,
                       Address = x.SuAddresses.Select(y => y.Address).FirstOrDefault(),
                       RegionalAddress = x.SuAddresses.Select(y => y.LocalLanguage).FirstOrDefault(),
                       FbCountryId = x.SuAddresses.Select(y => y.Country.FbCountryId).FirstOrDefault(),
                       CityName = x.SuAddresses.Select(y => y.City.CityName).FirstOrDefault(),
                       FbFactSupId = x.FbFactSupId
                   }).IgnoreQueryFilters().FirstOrDefaultAsync();


        }

        public IQueryable<SuSupplier> GetSuppliersSearchData()
        {
            return _context.SuSuppliers.Where(x => x.Active);
        }

        public async Task<List<CommonDataSource>> GetAddressCountry(IQueryable<int> supplierid)
        {
            return await _context.SuAddresses.Where(x => supplierid.Contains(x.SupplierId))
                .Select(y => new CommonDataSource { Name = y.Country.CountryName, Id = y.SupplierId })
                .AsNoTracking().ToListAsync();
        }

        public IQueryable<SupplierSearchItemRepo> GetSuppliersSearchChildData(int id, int supplierType)
        {

            IQueryable<SupplierSearchItemRepo> resultQuery;

            if (supplierType == (int)Supplier_Type.Factory)
                resultQuery = _context.SuSupplierFactories.Where(x => x.ParentId == id).Select(y => new SupplierSearchItemRepo()
                {
                    Id = y.Supplier.Id,
                    Name = y.Supplier.SupplierName,
                    TypeName = y.Supplier.Type.Type,
                    TypeId = y.Supplier.TypeId,
                    LocalName = y.Supplier.LocalName
                });
            else
                resultQuery = _context.SuSupplierFactories.Where(x => x.SupplierId == id).Select(y => new SupplierSearchItemRepo()
                {
                    Id = y.Parent.Id,
                    Name = y.Parent.SupplierName,
                    TypeName = y.Parent.Type.Type,
                    TypeId = y.Parent.TypeId,
                    LocalName = y.Parent.LocalName
                });

            return resultQuery;
        }

        public async Task<IEnumerable<SupplierAddressData>> GetSupplierAddressDataList(IEnumerable<int> supplierIds)
        {
            return await _context.SuAddresses.Where(x => supplierIds.Contains(x.SupplierId)
            ).Select(x => new SupplierAddressData
            {
                SupplierId = x.SupplierId,
                CountryName = x.Country.CountryName,
                CityName = x.City.CityName,
                RegionName = x.Region.ProvinceName,
                RegionalLanguageName = x.LocalLanguage,
                TownName = x.Town.TownName,
                Address = x.Address,
                CountryId = x.CountryId,
                CityId = x.CityId,
                ProvinceId = x.RegionId,
                CountyName = x.County.CountyName,
                CountyId = x.CountyId,
                TownId = x.TownId
            }).ToListAsync();
        }

        //Factory Address By Query Id
        public async Task<IEnumerable<SupplierAddressData>> GetSupplierAddressDataByIds(List<int> supplierIds)
        {
            return await _context.SuAddresses.Where(x => supplierIds.Contains(x.SupplierId)
            ).Select(x => new SupplierAddressData
            {
                SupplierId = x.SupplierId,
                CountryName = x.Country.CountryName,
                CityName = x.City.CityName,
                RegionName = x.Region.ProvinceName,
                RegionalLanguageName = x.LocalLanguage,
                TownName = x.Town.TownName,
                Address = x.Address,
                CountryId = x.CountryId,
                CityId = x.CityId,
                ProvinceId = x.RegionId,
                CountyName = x.County.CountyName,
                ZipCode = x.ZipCode,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                OfficeType = x.AddressType.AddressType
            }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<SupplierAddressData>> GetSupplierAddressDataByIds(IQueryable<int> supplierIds)
        {
            return await _context.SuAddresses.Where(x => supplierIds.Contains(x.SupplierId)
            ).Select(x => new SupplierAddressData
            {
                SupplierId = x.SupplierId,
                CountryName = x.Country.CountryName,
                CityName = x.City.CityName,
                RegionName = x.Region.ProvinceName,
                RegionalLanguageName = x.LocalLanguage,
                TownName = x.Town.TownName,
                Address = x.Address,
                CountryId = x.CountryId,
                CityId = x.CityId,
                ProvinceId = x.RegionId,
                CountyName = x.County.CountyName,
                ZipCode = x.ZipCode,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                OfficeType = x.AddressType.AddressType
            }).AsNoTracking().ToListAsync();
        }





        public async Task<IEnumerable<SupplierInvolvedData>> GetSupplierInvolvedItemsCount(IEnumerable<int> supplierIds)
        {
            return await _context.SuSuppliers.Where(x => x.Active && supplierIds.Contains(x.Id))
                          .Select(x => new SupplierInvolvedData()
                          {
                              SupplierId = x.Id,
                              InspectionSupplierAvailable = x.InspTransactionSuppliers.Any(),
                              AuditSupplierAvailable = x.AudTransactionSuppliers.Any(),
                              PurchaseOrderSupplierAvailable = x.CuPoSuppliers.Any(x => x.Active.HasValue && x.Active.Value),
                              InspectionFactoryAvailable = x.InspTransactionFactories.Any(),
                              AuditFactoryAvailable = x.AudTransactionFactories.Any(),
                              PurchaseOrderFactoryAvailable = x.CuPoFactories.Any(y => y.Active.HasValue && y.Active.Value),
                              SupplierFactoryMapAvailable = x.SuSupplierFactorySuppliers.Any(),
                              SupplierParentAvailable = x.SuSupplierFactoryParents.Any()
                          }).ToListAsync();
        }

        /// <summary>
        /// get supplierId contacts by supplierId id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetSupplierContactListbySupplier(int supplierId)
        {
            return await _context.SuContacts.Where(x => x.SupplierId == supplierId && x.Active == true)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.ContactName
                }).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<SupplierGeoLocation>> GetSupplierGeoLocations(IEnumerable<int> lstsupplierId)
        {
            return await _context.SuAddresses.Where(x => lstsupplierId.Contains(x.SupplierId))
                .Select(y => new SupplierGeoLocation
                {
                    FactoryId = y.SupplierId,
                    Country = y.Country.CountryName,
                    Province = y.Region.ProvinceName,
                    City = y.City.CityName,
                    County = y.County.CountyName,
                    Town = y.Town.TownName,
                }).ToListAsync();
        }

        /// <summary>
        /// get supplier or factory address details
        /// </summary>
        /// <param name="lstsupplierId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SupplierGeoLocation>> GetSupplierOrFactoryLocations(IEnumerable<int?> lstSupplierId)
        {
            return await _context.SuAddresses.Where(x => lstSupplierId.Contains(x.SupplierId))
                .Select(y => new SupplierGeoLocation
                {
                    FactoryId = y.SupplierId,
                    Country = y.Country.CountryName,
                    CountryId = y.CountryId
                }).ToListAsync();
        }

        /// <summary>
        /// get the supplier contacts by booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetSupplierContactByBooking(int bookingId, int supType)
        {
            if (supType == (int)Supplier_Type.Supplier_Agent)
            {
                return await _context.InspTranSuContacts.Where(x => x.InspectionId == bookingId && x.Active)
                    .Select(x => new CommonDataSource
                    {
                        Id = x.ContactId,
                        Name = x.Contact.ContactName
                    }).AsNoTracking().ToListAsync();
            }

            else
            {
                return await _context.InspTranFaContacts.Where(x => x.InspectionId == bookingId && x.Active)
                    .Select(x => new CommonDataSource
                    {
                        Id = x.ContactId.GetValueOrDefault(),
                        Name = x.Contact.ContactName
                    }).AsNoTracking().ToListAsync();
            }
        }

        public async Task<List<CommonDataSource>> GetSupplierContactByBookingForAudit(int bookingId, int supType)
        {
            if (supType == (int)Supplier_Type.Supplier_Agent)
            {
                return await _context.AudTranSuContacts.Where(x => x.AuditId == bookingId && x.Active)
                    .Select(x => new CommonDataSource
                    {
                        Id = x.ContactId,
                        Name = x.Contact.ContactName
                    }).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _context.AudTranFaContacts.Where(x => x.AuditId == bookingId && x.Active)
                    .Select(x => new CommonDataSource
                    {
                        Id = x.ContactId,
                        Name = x.Contact.ContactName
                    }).AsNoTracking().ToListAsync();
            }
        }

        public IQueryable<SuSupplier> GetSupplierDataSourceList(int typeId)
        {
            return _context.SuSuppliers.Where(x => x.TypeId == typeId && x.Active).OrderBy(x => x.SupplierName);
            //.Select(x => new CommonDataSource
            //{
            //    Id = x.Id,
            //    Name = x.SupplierName,
            //    CustomerId = x.SuSupplierCustomers.
            //}).OrderBy(x => x.Name);
        }

        /// <summary>
        /// Get the facory country by id
        /// </summary>
        /// <param name="factoryIds"></param>
        /// <returns></returns>
        public async Task<List<CountryListModel>> GetFactoryCountryById(IEnumerable<int> factoryIds)
        {
            return await _context.SuAddresses.Where(x => factoryIds.Contains(x.SupplierId)).
                Select(x => new CountryListModel
                {
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,
                    FactoryId = x.SupplierId
                }).ToListAsync();
        }

        /// <summary>
        /// Get the edit booking involved suppliers and active suppliers in the master list(by customerId)
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingSuppliersByCustId(int? customerId, int bookingId)
        {
            return await _context.SuSuppliers.Where(x => x.TypeId == (int)Supplier_Type.Supplier_Agent && (x.Active && x.SuSupplierCustomers.Any(y => y.CustomerId == customerId)
                            || x.InspTransactionSuppliers.Any(y => y.Id == bookingId))).
                            Select(x => new CommonDataSource() { Id = x.Id, Name = x.SupplierName }).
                            OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// Get the edit booking involved suppliers(by supplierId)
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingSuppliersBySupId(int supplierId)
        {
            return await _context.SuSuppliers.Where(x => x.TypeId == (int)Supplier_Type.Supplier_Agent && x.Id == supplierId).
                            Select(x => new CommonDataSource() { Id = x.Id, Name = x.SupplierName }).
                            OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// Get the edit booking involved suppliers and active suppliers in the master list
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingSuppliersByfactId(int factoryId, int bookingId)
        {
            return await _context.SuSuppliers
                  .Where(x => (x.Active && x.SuSupplierFactorySuppliers.Any(y => y.ParentId == factoryId) || x.InspTransactionFactories.Any(y => y.Id == bookingId)))
                  .Select(x => new CommonDataSource() { Id = x.Id, Name = x.SupplierName })
                  .ToListAsync();
        }
        /// <summary>
        /// Get the services configured for the supplier contacts
        /// </summary>
        /// <param name="supplierContactId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetSupplierContactServiceIds(int supplierContactId, int primaryEntity)
        {
            return await _context.SuContactEntityServiceMaps.
                 Where(x => x.ContactId == supplierContactId && x.EntityId == primaryEntity)
                .Select(x => x.ServiceId.GetValueOrDefault()).ToListAsync();
        }

        public async Task<CommonDataSource> GetSupplierContactPrimaryEntity(int supplierContactId)
        {
            return await _context.SuContacts.Where(x => x.Active.Value && x.Id == supplierContactId).
                                         Select(x => new CommonDataSource()
                                         { Id = x.PrimaryEntity.GetValueOrDefault(), Name = x.PrimaryEntityNavigation.Name })
                                         .IgnoreQueryFilters()
                                         .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierIds"></param>
        /// <returns></returns>
        public async Task<List<SupplierCustomerRepo>> GetSupplierCustomer(IQueryable<int> supplierIds)
        {
            return await _context.SuSupplierCustomers.Where(x => supplierIds.Contains(x.SupplierId))
                .Select(x => new SupplierCustomerRepo()
                {
                    SupplierId = x.SupplierId,
                    Code = x.Code,
                    CustomerName = x.Customer.CustomerName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierIds"></param>
        /// <returns></returns>
        public async Task<List<SupplierContactRepo>> GetSupplierContactDetailsBySupplierIdQuery(IQueryable<int> supplierIds)
        {
            return await _context.SuContacts.Where(x => x.Active.Value && supplierIds.Contains(x.SupplierId))
                .Select(x => new SupplierContactRepo()
                {
                    SupplierId = x.SupplierId,
                    ContactName = x.ContactName,
                    Email = x.Mail,
                    PhoneNumber = x.Phone
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierIds"></param>
        /// <returns></returns>
        public async Task<List<SupplierExportRepo>> GetFactoryDetailsBySupplierIdQuery(IQueryable<int> supplierIds)
        {
            return await _context.SuSupplierFactories.Where(x => supplierIds.Contains(x.ParentId))
                .Select(x => new SupplierExportRepo()
                {
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryId = x.ParentId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get supplier api service  list
        /// </summary>
        /// <param name="supplierIds"></param>
        /// <returns></returns>
        public async Task<List<SupplierServiceExportRepo>> GetSuAPIServiceBySupplierIdQuery(IQueryable<int> supplierIds)
        {
            return await _context.SuApiServices.Where(x => supplierIds.Contains(x.SupplierId.GetValueOrDefault()))
                .Select(x => new SupplierServiceExportRepo
                {
                    SupplierId = x.SupplierId,
                    ServiceName = x.Service.Name
                }).AsNoTracking().ToListAsync();
        }

        public async Task<string> GetSupplierContactEmailbyUserid(int Userid)
        {
            return await _context.ItUserMasters.Where(x => x.Id == Userid).Select(x => x.SupplierContact.Mail).IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetContactEmailIds(List<string> emailIds, int typeId)
        {
            return await _context.SuContacts.
                Where(x => x.Supplier.TypeId == typeId && !string.IsNullOrEmpty(x.Mail) &&
                        emailIds.Contains(x.Mail.ToLower().Trim()) && x.Active.HasValue && x.Active.Value).
                Select(x => x.Mail).ToListAsync();
        }

        public async Task<List<suppliercontact>> GetBaseSupplierContactDataById(int supplierId)
        {
            return await _context.SuContacts.Where(x => x.Active.Value && x.SupplierId == supplierId).
                Select(x => new suppliercontact()
                {
                    ContactId = x.Id,
                    ContactName = x.ContactName,
                    ContactEmail = x.Mail,
                    Phone = x.Phone
                }).ToListAsync();
        }

        public async Task<List<SupplierData>> SupplierDetailsExists(SupplierDetails request)
        {
            var suContactEmail = request.SupplierContactList.Select(x => x.ContactEmail).Distinct().ToList();
            var suContactPhone = request.SupplierContactList.Where(x => x.Phone != null && x.Phone != "").Select(x => x.Phone).Distinct().ToList();

            var supplierContact = _context.SuContacts.IgnoreQueryFilters().Where(x => x.Supplier.TypeId == request.TypeId && x.Active.HasValue && x.Active.Value
            && (suContactEmail.Any() && suContactEmail.Contains(x.Mail)) || (suContactPhone.Any() && suContactPhone.Contains(x.Phone)));

            if (supplierContact != null && supplierContact.Any())
            {
                var suList = await _context.SuSuppliers.IgnoreQueryFilters().Where(x => x.TypeId == request.TypeId && supplierContact.Any(y => y.SupplierId == x.Id) && x.Active)
                   .Select(x => new SupplierData
                   {
                       Id = x.Id,
                       Type = x.Type.Type,
                       TypeId = x.TypeId,
                       Name = x.SupplierName,
                       RegionalName = x.LocalName,
                       Email = x.Email,
                       Phone = x.Phone,
                       Address = x.SuAddresses.Select(x => x.Address).FirstOrDefault(),
                       RegionalAddress = x.SuAddresses.Select(x => x.LocalLanguage).FirstOrDefault(),
                       SuEntities = x.SuEntities
                   }).ToListAsync();
                if (suList.Any())
                {
                    foreach (var supplier in suList.ToList())
                    {
                        var suContact = supplierContact.Where(x => x.SupplierId == supplier.Id).FirstOrDefault();
                        if (suContact != null)
                        {
                            supplier.ContactEmail = suContact.Mail;
                            supplier.ContactPhone = suContact.Phone;
                            supplier.ContactName = suContact.ContactName;
                        }
                    }
                }
                return suList;
            }
            else
            {
                var isEmailExist = !string.IsNullOrEmpty(request.Email?.Trim());
                var isPhoneExist = !string.IsNullOrEmpty(request.Phone?.Trim());
                var isContactEmailExist = suContactEmail.Any();
                var isContactPhoneExist = suContactPhone.Any();
                var existEmail = request.Email?.Trim().ToLower();
                return await _context.SuSuppliers.IgnoreQueryFilters().Where(x => x.TypeId == request.TypeId && x.Active && ((isEmailExist && x.Email.Trim().ToLower() == existEmail) ||
               (isPhoneExist && x.Phone == request.Phone) || (isContactEmailExist && x.SuContacts.Any(y => suContactEmail.Contains(y.Mail))) || (isContactPhoneExist && x.SuContacts.Any(y => suContactPhone.Contains(y.Phone)))))
                    .Select(x => new SupplierData
                    {
                        Id = x.Id,
                        Type = x.Type.Type,
                        TypeId = x.TypeId,
                        Name = x.SupplierName,
                        RegionalName = x.LocalName,
                        Email = x.Email,
                        Phone = x.Phone,
                        Address = x.SuAddresses.Select(x => x.Address).FirstOrDefault(),
                        RegionalAddress = x.SuAddresses.Select(x => x.LocalLanguage).FirstOrDefault(),
                        ContactEmail = x.SuContacts.Select(x => x.Mail).FirstOrDefault(),
                        ContactPhone = x.SuContacts.Select(x => x.Phone).FirstOrDefault(),
                        ContactName = x.SuContacts.Select(x => x.ContactName).FirstOrDefault(),
                        SuEntities = x.SuEntities
                    }).ToListAsync();
            }
        }

        public async Task<SuSupplier> GetSupplierDetailById(int id)
        {
            return await _context.SuSuppliers.IgnoreQueryFilters().Include(x => x.SuContacts).Include(x => x.SuSupplierCustomers).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<SupplierCustomerData>> GetSupplierCustomerData(List<int> customerIds)
        {
            return await _context.SuSupplierCustomers.Where(x => customerIds.Contains(x.CustomerId))
            .Select(x => new SupplierCustomerData()
            {
                CustomerId = x.CustomerId,
                Code = x.Code,
                SupplierId = x.SupplierId,
                SupplierName = x.Supplier.SupplierName
            }).AsNoTracking().ToListAsync();
        }

        public async Task<EmailEntityResponse> GetSupplierContactEmailEntityByUserId(int userId)
        {
            return await _context.ItUserMasters.Where(x => x.Id == userId).Select(x => new EmailEntityResponse()
            {
                EmailId = x.SupplierContact.Mail,
                EntityId = x.Supplier.CompanyId.GetValueOrDefault()
            }).IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        public async Task<int> GetSupplierIdByCode(string code)
        {
            return _context.SuSupplierCustomers.Where(x => x.Code.ToLower().Trim() == code.ToLower().Trim() && x.Supplier.Active).Select(x => x.SupplierId).FirstOrDefault();
        }

        /// <summary>
        /// get supplier level by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<SuLevelCustomDto>> GetSupplierLevelByCustomerId(int customerId)
        {
            //check any supplier level configured or not
            var isAnyCustomerConfigured = await _context.SuLevelCustoms.AsNoTracking().AnyAsync(x => x.CustomerId == customerId);
            IQueryable<SuLevelCustom> suLevelCustoms = _context.SuLevelCustoms;
            if (isAnyCustomerConfigured)
            {
                suLevelCustoms = suLevelCustoms.Where(x => x.CustomerId == customerId);
            }
            else
            {
                suLevelCustoms = suLevelCustoms.Where(x => x.IsDefault);
            }
            return await suLevelCustoms.Select(x => new SuLevelCustomDto()
            {
                Id = x.Id,
                CustomName = x.CustomName,
                Level = x.Level.Level
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the supplier grade details by supplier id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<List<SupplierGradeRepo>> GetSupplierGradeDetailsBySupplierId(int supplierId)
        {
            return await _context.SuGrades.Where(x => x.SupplierId == supplierId && x.Active)
                .Select(y => new SupplierGradeRepo()
                {
                    Id = y.Id,
                    CustomerName = y.Customer.CustomerName,
                    Level = y.Level.Level.Level,
                    CustomName = y.Level.CustomName,
                    CustomerId = y.CustomerId,
                    PeriodFrom = y.PeriodFrom,
                    PeriodTo = y.PeriodTo,
                    LevelId = y.LevelId
                }).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// get the supplier grades by supplierids
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<List<SuGrade>> GetSupplierGradesBySupplierId(int supplierId)
        {
            return await _context.SuGrades.Where(x => x.SupplierId == supplierId && x.Active).ToListAsync();
        }

        public async Task<List<SupplierGradeRepo>> GetGradeByCustomerSupplier(int customerId, int supplierId)
        {
            return await _context.SuGrades.Where(x => x.CustomerId == customerId && x.SupplierId == supplierId && x.Active)
                .Select(y => new SupplierGradeRepo()
                {
                    PeriodFrom = y.PeriodFrom,
                    PeriodTo = y.PeriodTo,
                    CustomName = y.Level.CustomName,
                    Level = y.Level.Level.Level
                }).ToListAsync();
        }

        public async Task<SuSupplier> GetSupplierById(int supplierId, int type)
        {
            return await _context.SuSuppliers.Include(i => i.SuContacts).Where(x => x.Active && x.Id == supplierId && x.TypeId == type).FirstOrDefaultAsync();
        }

        public async Task<int> GetAddressIdBySuppllierId(int supplierId)
        {
            return _context.SuAddresses.Where(x => x.SupplierId == supplierId).Select(x => x.Id).FirstOrDefault();
        }
        public async Task<SuSupplier> GetSupplierByName(string name, int type)
        {
            return await _context.SuSuppliers.FirstOrDefaultAsync(x => x.SupplierName.ToLower().Trim() == name.ToLower().Trim() && x.Active && x.TypeId == type);
        }
        public async Task<bool> IsSupplierExistsByCustomer(int supplierId, int clientId, int type)
        {
            return await _context.SuSupplierCustomers.AnyAsync(x => x.SupplierId == supplierId && x.CustomerId == clientId && x.Supplier.TypeId == type);
        }

        public async Task<SuSupplier> GetSupplierDataBySupplierIdAndEntityId(int supplierId, int entityId)
        {
            return await _context.SuSuppliers.Where(x => x.Id == supplierId && x.SuEntities.Any(x => x.EntityId == entityId && x.Active.Value))
                    .Include(x => x.SuAddresses)
                     .ThenInclude(x => x.Country)
                     .ThenInclude(x => x.RefProvinces)
                     .ThenInclude(x => x.RefCities)
                     .IgnoreQueryFilters()
                     .FirstOrDefaultAsync();
        }

        public async Task<List<SupplierAddressData>> GetSupplierAddressBySupplierIds(IEnumerable<int> supplierIds)
        {
            return await _context.SuAddresses.Where(x => supplierIds.Contains(x.SupplierId)
                                    && x.AddressTypeId == (int)RefAddressTypeEnum.HeadOffice)
                                    .Select(x => new SupplierAddressData
                                    {
                                        SupplierId = x.SupplierId,
                                        CountryId = x.CountryId,
                                        CountryName = x.Country.CountryName,
                                        ProvinceId = x.City.ProvinceId,
                                        ProvinceName = x.City.Province.ProvinceName,
                                        CityId = x.CityId,
                                        CityName = x.City.CityName
                                    }).AsNoTracking().ToListAsync();
        }

        public async Task<List<SupplierGradeRepo>> GetGradeByCustomersAndSuppliers(List<int> customerIds, List<int> supplierIds)
        {
            return await _context.SuGrades.Where(x => customerIds.Contains(x.CustomerId) && supplierIds.Contains(x.SupplierId) && x.Active)
                .Select(y => new SupplierGradeRepo()
                {
                    PeriodFrom = y.PeriodFrom,
                    PeriodTo = y.PeriodTo,
                    CustomName = y.Level.CustomName,
                    CustomerId = y.CustomerId,
                    Level = y.Level.Level.Level,
                    SupplierId = y.SupplierId
                }).AsNoTracking().ToListAsync();
        }
    }
}
