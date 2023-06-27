using Contracts.Repositories;
using DTO.CommonClass;
using DTO.Customer;
using DTO.FullBridge;
using DTO.Quotation;
using DTO.User;
using DTO.UserAccount;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerRepository : Repository, ICustomerRepository
    {
        public CustomerRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<List<CuServiceType>> GetCustomerAuditServiceType(int CustomerId)
        {
            return await _context.CuServiceTypes
                .Include(x => x.ServiceType)
                .Where(x => x.Active && x.CustomerId == CustomerId && (x.ServiceId == (int)Service.AuditId || x.ServiceId == (int)Service.SGTAudit) && (x.ServiceType.Active))
                .OrderBy(x => x.ServiceType.Name).ToListAsync();
        }

        public async Task<List<CuServiceType>> GetCustomerInspectionServiceType(int CustomerId)
        {
            return await _context.CuServiceTypes
                .Include(x => x.ServiceType)
                .Where(x => x.Active && x.CustomerId == CustomerId
                                        && x.ServiceId == (int)Service.InspectionId
                                        && (x.ServiceType.Active) && (x.ServiceType.IsReInspectedService == false))
                .OrderBy(x => x.ServiceType.Name).ToListAsync();
        }



        public async Task<List<CuBrand>> GetCustomerBrandsByUserId(int CustomerId)
        {
            return await _context.CuBrands
                 .Include(x => x.Customer)
                 .Include(x => x.ItUserCuBrands)
                 .Where(x => x.CustomerId == CustomerId && x.Active)
                 .OrderBy(x => x.Name).ToListAsync();
        }



        public async Task<List<CuDepartment>> GetCustomerDepartmentByUserId(int CustomerId)
        {
            return await _context.CuDepartments
                   .Include(x => x.Customer)
                   .Include(x => x.ItUserCuDepartments)
                   .Where(x => x.CustomerId == CustomerId && x.Active)
                   .OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<CuSeason>> GetCustomerSeason(int CustomerId)
        {
            return await _context.CuSeasons
                 .Include(x => x.Season)
                 .Where(x => x.CustomerId == CustomerId && x.Active)
                 .OrderBy(x => x.Season.Name).ToListAsync();
        }

        public async Task<List<CuCustomer>> GetCustomersItems()
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value).OrderBy(x => x.CustomerName).ToListAsync();
        }
        public async Task<List<CuCustomer>> GetCustomerbyId(int? id)
        {
            return await _context.CuCustomers.Where(x => x.Group == id && x.Active.HasValue && x.Active.Value).ToListAsync();
        }

        public async Task<List<CuCustomerGroup>> GetCustomerGroup()
        {
            return await _context.CuCustomerGroups.ToListAsync();
        }

        public async Task<List<Language>> GetLanguage()
        {
            return await _context.Languages.ToListAsync();
        }
        public async Task<List<RefProspectStatus>> GetProspectStatus()
        {
            return await _context.RefProspectStatuses.ToListAsync();
        }
        public async Task<List<RefMarketSegment>> GetMarketSegment()
        {
            return await _context.RefMarketSegments.ToListAsync();
        }
        public async Task<List<RefBusinessType>> GetBusinessType()
        {
            return await _context.RefBusinessTypes.ToListAsync();
        }
        public async Task<List<RefAddressType>> GetAddressType()
        {
            return await _context.RefAddressTypes.ToListAsync();
        }
        public async Task<List<RefInvoiceType>> GetInvoiceType()
        {
            return await _context.RefInvoiceTypes.ToListAsync();
        }

        public async Task<List<CuRefAccountingLeader>> GetAccountingLeader()
        {
            return await _context.CuRefAccountingLeaders.Where(x => x.Active.Value == 1).OrderBy(x => x.Sort).AsNoTracking().ToListAsync();
        }

        public async Task<List<CuRefActivitiesLevel>> GetActivitiesLevel()
        {
            return await _context.CuRefActivitiesLevels.Where(x => x.Active.Value == 1).OrderBy(x => x.Sort).AsNoTracking().ToListAsync();
        }

        public async Task<List<CuRefRelationshipStatus>> GetRelationshipStatus()
        {
            return await _context.CuRefRelationshipStatuses.Where(x => x.Active.Value == 1).OrderBy(x => x.Sort).AsNoTracking().ToListAsync();
        }

        public async Task<List<CuRefBrandPriority>> GetBrandPriority()
        {
            return await _context.CuRefBrandPriorities.Where(x => x.Active.Value == 1).OrderBy(x => x.Sort).AsNoTracking().ToListAsync();
        }

        public IEnumerable<CuCustomer> GetAllCustomersItems()
        {
            return _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
                 .Include(x => x.CuAddresses)
                 .Include(x => x.ItUserMasters)
                 .Include(x => x.CuCustomerBusinessCountries)
                 .ThenInclude(x => x.BusinessCountry);
        }

        public async Task<int> AddCustomer(CuCustomer entity)
        {
            _context.CuCustomers.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> AddCustomerAddress(CuAddress entity)
        {
            _context.CuAddresses.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> AddCustomerContact(CuContact entity)
        {
            _context.CuContacts.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public Task<int> EditCustomer(CuCustomer entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> EditCustomerAddress(CuAddress entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> EditCustomerContact(CuContact entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }


        public async Task<CuCustomer> GetCustomerDetails(int? id)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
                .Include(x => x.CuAddresses)
                .Include(y => y.CuCustomerBusinessCountries)
                .Include(y => y.CuCustomerSalesCountries)
                .Include(y => y.CuKams)
                .Include(y => y.CuSalesIncharges)
                .Include(y => y.CuBrandpriorities)
                .Include(y => y.CuApiServices)
                .Include(y => y.CuEntities)
                .Include(y => y.CuSisterCompanyCustomers)
                .Include(y => y.CuContacts)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

        }

        public async Task<bool> RemoveCustomer(int id)
        {
            var entity = await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value).
                FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;
            _context.Entry(entity).State = EntityState.Deleted;
            int numReturn = await _context.SaveChangesAsync();

            return numReturn > 0;
        }
        public async Task<List<CuCustomer>> GetCustomersByUserId(int UserId)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
                //.Include(x => x.CuCsConfigurations)
                //.Where(x => x.CuCsConfigurations.Any(y => y.Active && y.UserId == UserId))
                .OrderBy(x => x.CustomerName).ToListAsync();
        }

        public async Task<List<CuCustomer>> GetCustomersBySupplierId(int SupplierId)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
                 .Include(x => x.SuSupplierCustomers)
                 .Where(x => x.SuSupplierCustomers.Any(y => y.SupplierId == SupplierId)).OrderBy(x => x.CustomerName).ToListAsync();
        }

        public CuCustomer GetCustomerByID(int? customerID)
        {
            if (customerID == null)
                return null;

            return _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
                    .Include(x => x.CuCheckPoints)
                    .FirstOrDefault(x => x.Id == customerID.Value);
        }

        public async Task<List<CuBuyer>> GetCustomerBuyerByUserId(int CustomerId)
        {
            return await _context.CuBuyers
                   .Include(x => x.Customer)
                   .Where(x => x.CustomerId == CustomerId && x.Active)
                   .OrderBy(x => x.Name).ToListAsync();
        }


        /// <summary>
        /// Get customer list
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="serviceId"></param>
        /// <param name="billingMethodId"></param>
        /// <param name="billingPaidById"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DataSource>> GetCustomerListByCountryAndService(int countryId, int serviceId)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
           .Where(x => x.CuCustomerBusinessCountries.Any(y => y.BusinessCountryId == countryId)
                    && x.CuApiServices.Any(y => y.ServiceId == serviceId && y.Active.Value)
           ).Select(x => new DataSource
           {
               Id = x.Id,
               Name = x.CustomerName,
               IsForwardToManager = x.CuServiceTypes.Any(y => y.Active && y.Service != null && y.Service.CuCheckPoints.Any(z => z.CustomerId == x.Id && z.Active && z.ServiceId == serviceId && z.CheckpointTypeId == (int)CheckPointTypeEnum.QuotationApproveByManager)),
               InvoiceType = x.InvoiceType.GetValueOrDefault()
           })
           .OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<CuCustomer>> GetEditCustomerListByCountryAndService(int countryId, int serviceId)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
            .Include(x => x.CuCustomerBusinessCountries)
            .Include(x => x.CuServiceTypes)
            .ThenInclude(x => x.Service)
           .Where(x => x.CuCustomerBusinessCountries.Any(y => y.BusinessCountryId == countryId)
                    && x.CuServiceTypes.Any(y => y.ServiceId == serviceId)
           ).OrderBy(x => x.CustomerName).ToListAsync();
        }

        /// <summary>
        /// Get customer list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CuCustomer>> GetCustomerList()
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
           .OrderBy(x => x.CustomerName).ToListAsync();
        }


        public async Task<List<CuServiceType>> GetReInspectionServiceType(int CustomerId)
        {
            return await _context.CuServiceTypes
                .Include(x => x.ServiceType)
                .Where(x => x.Active && x.CustomerId == CustomerId
                                        && x.ServiceId == (int)Service.InspectionId && (x.ServiceType.Active) && (x.ServiceType.IsReInspectedService == true))
                .OrderBy(x => x.ServiceType.Name).ToListAsync();
        }

        //Fetch Customer contacts based on customer ID
        public async Task<List<Contact>> GetCustomerContacts(int customerId)
        {
            return await _context.CuContacts.Where(x => x.CustomerId == customerId && x.Active == true)
                .Select(x => new Contact
                {
                    ContactId = x.Id,
                    ContactName = x.ContactName
                }).OrderBy(x => x.ContactName).ToListAsync();
        }
        /// <summary>
        /// Get Customer data By Id
        /// </summary>
        /// <returns></returns>
        public async Task<CuCustomer> GetCustomerItemById(int customerId)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value).FirstOrDefaultAsync(x => x.Id == customerId);
        }

        /// <summary>
        /// Get Customer data By Id by Ignore global filter
        /// </summary>
        /// <returns></returns>
        public async Task<CuCustomer> GetCustomerItemByIdForCFL(int customerId)
        {
            return await _context.CuCustomers.IgnoreQueryFilters().Where(x => x.Active.HasValue && x.Active.Value).FirstOrDefaultAsync(x => x.Id == customerId);
        }

        /// <summary>
        /// Get Customer data By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ZohoCustomer> GetCustomerByZohoId(long zohoId)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value && x.ZohoCustomerId == zohoId).
                    Select(x =>
                            new ZohoCustomer
                            {
                                Id = x.Id,
                                Name = x.CustomerName,
                                GLCode = x.GlCode,
                                StartDate = x.StartDate.Value
                            }).FirstOrDefaultAsync();
        }

        public async Task<List<ZohoCustomer>> GetCustomerDetailsByName(string Name)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value
                            && x.CustomerName.Contains(Name)).
                            Select(x => new ZohoCustomer { Id = x.Id, Name = x.CustomerName, EmailId = x.Email, GLCode = x.GlCode }).
                            ToListAsync();

        }

        public async Task<List<ZohoCustomerAddress>> GetZohoCustomerAddressById(int customerId)
        {
            return await _context.CuAddresses.Where(x => x.Active.HasValue && x.Active.Value
                            && x.CustomerId == customerId).
                            Select(x => new ZohoCustomerAddress { Id = x.Id, Address = x.Address, AddressType = x.AddressType }).
                            ToListAsync();

        }

        public async Task<List<CustomerSource>> GetCustomerByName(string name)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value
                            && x.CustomerName.ToLower().Trim() == name.ToLower().Trim()).
                            Select(x => new CustomerSource { Id = x.Id, Name = x.CustomerName }).
                            ToListAsync();

        }

        public async Task<List<CustomerSource>> GetOtherAcountCustomerName(string name, int customerId)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value
                            && x.CustomerName.ToLower().Trim() == name.ToLower().Trim()
                            && x.Id != customerId).
                            Select(x => new CustomerSource { Id = x.Id, Name = x.CustomerName }).
                            ToListAsync();
        }

        public async Task<List<CustomerSource>> GetOtherAcountGLCode(string glCode, int customerId)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value
                            && x.GlCode.ToLower().Trim() == glCode.ToLower().Trim()
                            && x.Id != customerId).
                            Select(x => new CustomerSource { Id = x.Id, Name = x.CustomerName }).
                            ToListAsync();
        }

        public async Task<List<CustomerSource>> GetCustomerByGLCode(string glCode)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value
                            && x.GlCode.ToLower().Trim() == glCode.ToLower().Trim()).
                            Select(x => new CustomerSource { Id = x.Id, Name = x.CustomerName }).
                            ToListAsync();

        }

        public async Task<List<CustomerSource>> GetCustomerByEmail(string email)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value
                            && x.Email.ToLower().Trim() == email.ToLower().Trim()).
                            Select(x => new CustomerSource { Id = x.Id, Name = x.CustomerName }).
                            ToListAsync();

        }

        public async Task<List<CustomerSource>> GetOtherAccountEmail(string email, int customerId)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value
                            && x.Email.ToLower().Trim() == email.ToLower().Trim() && x.Id != customerId).
                            Select(x => new CustomerSource { Id = x.Id, Name = x.CustomerName }).
                            ToListAsync();

        }

        public Task<CuCustomer> GetCustomerDetailsByZohoID(long id)
        {
            return _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
                .Include(x => x.CuAddresses)
                .Include(y => y.CuCustomerBusinessCountries)
                .Include(y => y.CuCustomerSalesCountries)
                .Where(x => x.ZohoCustomerId == id).FirstOrDefaultAsync();

        }

        public Task<CuCustomer> GetCustomerDetailsByCustomerId(int id)
        {
            return _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
                .Include(x => x.CuAddresses)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<CuCustomer> GetEaqfCustomerDetailsByCustomerId(int customerId)
        {
            return _context.CuCustomers.Where(x => x.Active.HasValue
                    && x.Active.Value && x.IsEaqf.HasValue && x.IsEaqf.Value
                    && x.Id == customerId)
                   .FirstOrDefaultAsync();
        }

        public async Task<CuAddress> GetEaqfCustomerAddress(int customerId, int addressId)
        {
            return await _context.CuAddresses.
                 Where(x => x.Active.HasValue
                 && x.Active.Value
                 && x.Customer.IsEaqf.Value
                 && x.CustomerId == customerId && x.Id == addressId)
                .FirstOrDefaultAsync();
        }

        public async Task<CuCustomer> GetCustomerDetailsByGLCode(string glCode)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value)
                .Include(x => x.CuAddresses)
                .Include(y => y.CuCustomerBusinessCountries)
                .Include(y => y.CuCustomerSalesCountries)
                .Where(x => x.GlCode.ToLower().Trim() == glCode.ToLower().Trim()).
                            FirstOrDefaultAsync();

        }

        public async Task<List<CommonDataSource>> GetCustomerMerchandiserById(int CustomerId)
        {
            return await _context.CuCustomerContactTypes.Where(x => x.Contact.Active.Value && x.Contact.CustomerId == CustomerId && x.ContactTypeId == (int)CustomercontactType.MerchandiserManager)
                .Select(x => new CommonDataSource
                {
                    Id = x.ContactId,
                    Name = x.Contact.ContactName
                }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get Customer Headoffice by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<int> GetCustomerHeadOfficeAddressById(int customerId)
        {
            return await _context.CuAddresses.Where(x => x.CustomerId == customerId && x.Active.HasValue && x.Active.Value
                                    && x.AddressType == (int)RefAddressTypeEnum.HeadOffice).
                                    Select(x => x.Id).FirstOrDefaultAsync();

        }
        /// <summary>
        /// Get Customer address based on customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<CustomerAccountingAddress>> GetCustomerAddressByListCusId(List<int> lstcustomerId)
        {
            return await _context.CuAddresses.Where(x => x.Active.HasValue && x.Active.Value && x.CustomerId.HasValue
                            && lstcustomerId.Contains(x.CustomerId.Value)).
                            Select(x => new CustomerAccountingAddress { Id = x.Id, EnglishAddress = x.Address, CustomerId = x.CustomerId.Value, AddressType = x.AddressType }).
                            ToListAsync();

        }

        //Customer Address By Query Id
        public async Task<List<CustomerAccountingAddress>> GetCustomerAddressByCusIds(List<int> lstcustomerId)
        {
            return await _context.CuAddresses.Where(x => x.Active.HasValue && x.Active.Value && x.CustomerId.HasValue
                            && lstcustomerId.Contains(x.CustomerId.Value)).
                            Select(x => new CustomerAccountingAddress { Id = x.Id, EnglishAddress = x.Address, CustomerId = x.CustomerId.Value, AddressType = x.AddressType }).AsNoTracking().
                            ToListAsync();

        }

        //get customstatus name for customers
        public async Task<List<CustomerCustomStatus>> GetCustomStatusNameByCustomer(List<int> customerIds)
        {
            return await _context.InspCuStatuses.Where(x => customerIds.Contains(x.CustomerId) && x.Active == true)
                .Select(y => new CustomerCustomStatus
                {
                    CustomerId = y.CustomerId,
                    StatusId = y.StatusId,
                    CustomStatusName = y.CustomStatusName
                }).AsNoTracking().ToListAsync();
        }

        //get customer collection
        public async Task<List<CommonDataSource>> GetCustomerCollection(int CustomerId)
        {
            return await _context.CuCollections.Where(x => x.CustomerId == CustomerId && x.Active.Value)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        //get customer price category
        public async Task<List<CommonDataSource>> GetCustomerPriceCategory(int CustomerId)
        {
            return await _context.CuPriceCategories.Where(x => x.CustomerId == CustomerId && x.Active == true)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the customer brands by customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerBrands(int CustomerId)
        {
            return await _context.CuBrands.Where(x => x.CustomerId == CustomerId && x.Active == true)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the customer departments by customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerDepartments(int CustomerId)
        {
            return await _context.CuDepartments.Where(x => x.CustomerId == CustomerId && x.Active == true)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the customer buyers by customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerBuyers(int CustomerId)
        {
            return await _context.CuBuyers.Where(x => x.CustomerId == CustomerId && x.Active == true)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetCustomerProductCategoryList(int CustomerId)
        {
            return await _context.CuProductCategories.Where(x => x.CustomerId == CustomerId && x.Active.Value)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get product subcategory by customer id.
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerProductSubCategoryList(int CustomerId, List<int?> productCategory)
        {
            var subCategoryQuery = _context.RefProductCategorySubs.Where(x => x.ProductCategory.CuProducts.Any(x => x.CustomerId == CustomerId) && x.Active);

            if (productCategory.Any())
            {
                subCategoryQuery = subCategoryQuery.Where(x => productCategory.Contains(x.ProductCategoryId));
            }

            return await subCategoryQuery.Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get product sub category 2
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="productSubCategory"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerProductSub2CategoryList(List<int?> productCategory, List<int?> productSubCategory)
        {
            var subCategory2Query = _context.RefProductCategorySub2S.Where(x => x.Active);

            if (productCategory.Any())
            {
                subCategory2Query = subCategory2Query.Where(x => productCategory.Contains(x.ProductSubCategory.ProductCategoryId));
            }

            if (productSubCategory.Any())
            {
                subCategory2Query = subCategory2Query.Where(x => productSubCategory.Contains(x.ProductSubCategoryId));
            }

            return await subCategory2Query.Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().ToListAsync();
        }

        //get customer name by a substring
        public async Task<List<CommonDataSource>> GetCustomerByNameAutocomplete(string name)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active == true && EF.Functions.Like(x.CustomerName.ToLower().Trim(), $"%{name.ToLower().Trim()}%"))
                .Select(y => new CommonDataSource
                {
                    Id = y.Id,
                    Name = y.CustomerName
                }).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }

        //Get customer names by id list
        public async Task<List<CommonDataSource>> GetCustomerById(List<int> customerIds)
        {
            return await _context.CuCustomers.Where(x => x.Active.HasValue && x.Active == true && customerIds.Contains(x.Id))
                .Select(y => new CommonDataSource
                {
                    Id = y.Id,
                    Name = y.CustomerName
                }).AsNoTracking().ToListAsync();
        }

        //Get the customer brands by customer ids
        public async Task<IEnumerable<ParentDataSource>> GetCustomerBrands(IEnumerable<int> customerIds)
        {
            return await _context.CuBrands.Where(x => customerIds.Contains(x.CustomerId) && x.Active)
                .Select(x => new ParentDataSource
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.CustomerId
                }).ToListAsync();
        }

        //Get the customer departments by customer ids
        public async Task<IEnumerable<ParentDataSource>> GetCustomerDepartments(IEnumerable<int> customerIds)
        {
            return await _context.CuDepartments.Where(x => customerIds.Contains(x.CustomerId) && x.Active)
                .Select(x => new ParentDataSource
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.CustomerId
                }).ToListAsync();
        }

        /// <summary>
        /// Get the customer master data required for FB
        /// </summary>
        /// <returns></returns>
        public async Task<FBCustomerMasterData> GetFBCustomerDataById(int customerId)
        {
            return await _context.CuCustomers.
                Where(x => x.Id == customerId && x.Active.HasValue && x.Active.Value).
                   Select(x => new FBCustomerMasterData
                   {
                       CustomerId = x.Id,
                       CustomerName = x.CustomerName,
                       CustomerAddress = x.CuAddresses.Where(y => y.AddressType == (int)RefAddressTypeEnum.HeadOffice).Select(y => y.Address).FirstOrDefault(),
                       FbCountryId = x.CuAddresses.Where(y => y.AddressType == (int)RefAddressTypeEnum.HeadOffice).Select(y => y.Country.FbCountryId).FirstOrDefault(),
                       CityName = x.CuAddresses.Where(y => y.AddressType == (int)RefAddressTypeEnum.HeadOffice).Select(y => y.City.CityName).FirstOrDefault(),
                       FbCusId = x.FbCusId
                   }).IgnoreQueryFilters().FirstOrDefaultAsync();


        }


        public async Task<CustomerPriceData> GetCustomerPriceData(int customerId)
        {
            return await _context.CuCustomers.
                Where(x => x.Id == customerId && x.Active.HasValue && x.Active.Value).
                   Select(x => new CustomerPriceData
                   {
                       CustomerId = x.Id,
                       CustomerSegment = x.MargetSegmentNavigation.Name,
                       CountryName = x.CuAddresses.Where(y => y.AddressType == (int)RefAddressTypeEnum.HeadOffice).Select(y => y.Country.CountryName).FirstOrDefault(),
                   }).FirstOrDefaultAsync();
        }


        /// <summary>
        /// get Customer Address by customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonAddressDataSource>> GetCustomerAddress(int CustomerId)
        {
            return await _context.CuAddresses.Where(x => x.CustomerId == CustomerId && x.Active == true)
                .Select(x => new CommonAddressDataSource
                {
                    Id = x.Id,
                    Name = x.Address,
                    AddressType = x.AddressType,
                    CustomerId = x.CustomerId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get customer contacts by customer id
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerContactListbyCustomer(int CustomerId)
        {
            return await _context.CuContacts.Where(x => x.CustomerId == CustomerId && x.Active == true)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.ContactName
                }).OrderBy(x => x.Name).ToListAsync();
        }

        public IQueryable<CuCustomer> GetCustomerDataSource()
        {
            return _context.CuCustomers.Where(x => x.Active.Value).OrderBy(x => x.CustomerName);
        }

        #region Price Category        
        /// <summary>
        /// get active price category list by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IQueryable<CommonDataSource> GetPriceCategoryDataSource(int customerId)
        {
            return _context.CuPriceCategories.Where(x => x.Active.Value && x.CustomerId == customerId)
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name });
        }
        #endregion

        /// <summary>
        /// get customer list with supplier
        /// </summary>
        /// <returns></returns>
        public IQueryable<CustomerDataSource> GetCustomerDataSourceFromSupplier()
        {
            return _context.SuSupplierCustomers.Select(x => new CustomerDataSource
            {
                Id = x.CustomerId,
                CustomerName = x.Customer.CustomerName,
                SupplierId = x.SupplierId
            }).OrderBy(x => x.CustomerName);
        }

        /// <summary>
        /// Get the customer KAM Details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerKamDetail> GetCustomerKAMDetails(int customerId)
        {
            return await _context.CuKams.Where(x => x.Active.HasValue && x.Active.Value == 1 && x.CustomerId.HasValue && x.CustomerId.Value == customerId).
                        Select(x => new CustomerKamDetail()
                        {
                            Name = x.Kam.PersonName,
                            Email = x.Kam.CompanyEmail,
                            PhoneNumber = x.Kam.CompanyMobileNo
                        }).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the booking involved brands and active master brand list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingBrands(int CustomerId, int bookingId)
        {
            return await _context.CuBrands.Where(x => (x.CustomerId == CustomerId && x.Active) || x.InspTranCuBrands.Any(y => y.Active && y.InspectionId == bookingId))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking involved depts and active master depts list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingDepartments(int CustomerId, int bookingId)
        {
            return await _context.CuDepartments.Where(x => (x.CustomerId == CustomerId && x.Active) || x.InspTranCuDepartments.Any(y => y.Active && y.InspectionId == bookingId))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking involved buyers and active master buyers list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingBuyers(int CustomerId, int bookingId)
        {
            return await _context.CuBuyers.Where(x => (x.CustomerId == CustomerId && x.Active) || x.InspTranCuBuyers.Any(y => y.Active && y.InspectionId == bookingId))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking involved merchandise and active master merchandise list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingMerchandisers(int CustomerId, int bookingId)
        {
            return await _context.CuContacts.Where(x => (x.Active.Value && x.CustomerId == CustomerId && x.CuCustomerContactTypes.Any(y => y.ContactTypeId == (int)CustomercontactType.MerchandiserManager))
                                                    || (x.InspTranCuMerchandisers.Any(y => y.Active && y.InspectionId == bookingId)))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.ContactName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking involved price category and active master pricecategory list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingPriceCategory(int CustomerId, int bookingId)
        {
            return await _context.CuPriceCategories.Where(x => (x.CustomerId == CustomerId && x.Active.Value) || x.InspTransactions.Any(y => y.Id == bookingId))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the booking involved collection and active master collection list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingCollection(int CustomerId, int bookingId)
        {
            return await _context.CuCollections.Where(x => (x.CustomerId == CustomerId && x.Active.Value) || x.InspTransactions.Any(y => y.Id == bookingId))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the booking involved serviceType and active master serviceType list
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingServiceType(int customerId, int bookingId)
        {
            return await _context.RefServiceTypes.Where(x => x.Active && x.CuServiceTypes.Any(y => y.Active && y.CustomerId == customerId) || x.InspTranServiceTypes.Any(y => y.InspectionId == bookingId))
                .OrderBy(x => x.Sort)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the services configured for the customer contacts
        /// </summary>
        /// <param name="customerContactId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetCustomerContactServiceIds(int customerContactId, int primaryEntity)
        {
            return await _context.CuContactEntityServiceMaps.
                Where(x => x.EntityId == primaryEntity && x.ContactId == customerContactId)
                .Select(x => x.ServiceId.GetValueOrDefault()).ToListAsync();
        }

        public async Task<CommonDataSource> GetCustomerContactPrimaryEntity(int customerContactId)
        {
            return await _context.CuContacts.
                Where(x => x.Active.Value && x.Id == customerContactId).
                Select(x => new CommonDataSource()
                {
                    Id = x.PrimaryEntity.GetValueOrDefault(),
                    Name = x.PrimaryEntityNavigation.Name
                })
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// Get the customer product categories
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerProductCategories(int customerId)
        {
            return await _context.CuProductCategories.Where(x => x.Active.Value && x.CustomerId == customerId).OrderBy(x => x.Sort)
                            .Select(x => new CommonDataSource() { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the customer season config details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IQueryable<CuSeasonConfig> GetCustomerSeasonConfigQuery()
        {
            return _context.CuSeasonConfigs;
        }

        public async Task<string> GetCustomerContactEmailbyUserid(int Userid)
        {
            return await _context.ItUserMasters.Where(x => x.Id == Userid).Select(x => x.CustomerContact.Email).IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Product Category List by booking ids
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<List<ParentDataSource>> GetCustomerProductCategoryListByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id) && x.CuProductCategoryNavigation.Active.HasValue && x.CuProductCategoryNavigation.Active.Value)
                .Select(x => new ParentDataSource
                {
                    Id = x.CuProductCategory.Value,
                    Name = x.CuProductCategoryNavigation.Name,
                    ParentId = x.Id
                }).AsNoTracking().ToListAsync();
        }


        public async Task<List<CommonDataSource>> GetCustomerEntityByCustomerId(int customerId)
        {
            return await _context.CuEntities.Where(x => x.Active == true && x.CustomerId == customerId).Select(y => new CommonDataSource()
            {
                Id = y.EntityId,
                Name = y.Entity.Name,
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetCustomerSisterCompanyByCustomerId(int customerId)
        {
            return await _context.CuSisterCompanies.Where(x => x.Active.Value && x.CustomerId == customerId)
                .Select(y => new CommonDataSource()
                {
                    Id = y.SisterCompanyId,
                    Name = y.SisterCompany.CustomerName,
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<CuContactEntityMap>> GetCustomerContactEntityMapByCustomerId(IEnumerable<int> contactIds)
        {
            return await _context.CuContactEntityMaps.Where(x => contactIds.Contains(x.ContactId) && x.Contact.Active.Value).AsNoTracking().ToListAsync();
        }

        public async Task<List<CuContactEntityServiceMap>> GetCustomerContactEntityServiceMapByCustomerId(IEnumerable<int> contactIds)
        {
            return await _context.CuContactEntityServiceMaps.Where(x => x.ContactId.HasValue && contactIds.Contains(x.ContactId.Value) && x.Contact.Active == true).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the customer products by name
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productNameList"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerProductsByName(int customerId, List<string> productNameList)
        {
            return await _context.CuProducts.Where(x => x.Active && x.CustomerId == customerId && productNameList.Contains(x.ProductId.Trim().ToLower())).
                            Select(x => new CommonDataSource()
                            {
                                Id = x.Id,
                                Name = x.ProductId
                            }).ToListAsync();
        }

        public async Task<EmailEntityResponse> GetCustomerContactEmailEntityByUserId(int userId)
        {
            return await _context.ItUserMasters.Where(x => x.Id == userId).Select(x => new EmailEntityResponse()
            {
                EmailId = x.CustomerContact.Email,
                EntityId = x.Customer.CompanyId.GetValueOrDefault()
            }).IgnoreQueryFilters().FirstOrDefaultAsync();
        }
        /// <summary>
        /// get customer product sub category by product sub category ids
        /// </summary>
        /// <param name="productSubCategoryIds"></param>
        /// <returns></returns>
        public async Task<List<ParentDataSource>> GetCustomerProductCategoryByProductSubCategoryIds(List<int> productSubCategoryIds)
        {
            return await _context.CuProductCategories.Where(x => x.Active == true && productSubCategoryIds.Contains(x.LinkProductSubCategory.GetValueOrDefault()))
                .Select(y => new ParentDataSource()
                {
                    Id = y.Id,
                    Name = y.Name,
                    ParentId = y.LinkProductSubCategory.GetValueOrDefault()
                })
                .AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get customer product type by product sub category 2 ids
        /// </summary>
        /// <param name="productSubCategoryIds"></param>
        /// <returns></returns>
        public async Task<List<ParentDataSource>> GetCustomerProductTypeByProductCategoryIds(List<int> productSubCategory2Ids)
        {
            return await _context.CuProductTypes.Where(x => x.Active == true && productSubCategory2Ids.Contains(x.LinkProductType.GetValueOrDefault()))
                .Select(y => new ParentDataSource()
                {
                    Id = y.Id,
                    Name = y.Name,
                    ParentId = y.LinkProductType.GetValueOrDefault()
                })
                .AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsCustomerExists(Expression<Func<CuCustomer, bool>> predicate)
        {
            return await _context.CuCustomers.AnyAsync(predicate);
        }

        public async Task<bool> IsCustomerContactExists(Expression<Func<CuContact, bool>> predicate)
        {
            return await _context.CuContacts.AnyAsync(predicate);
        }

        public async Task<List<CuContactSisterCompany>> GetSisterCompaniesContactByCustomerContactIds(IEnumerable<int> contactIds)
        {
            return await _context.CuContactSisterCompanies.Where(x => contactIds.Contains(x.ContactId) && x.Active.Value).ToListAsync();
        }

        public async Task<List<int>> GetSisterCompanieIdsByCustomerContactId(int contactId)
        {
            return await _context.CuContactSisterCompanies.Where(x => x.ContactId == contactId && x.Active.Value).AsNoTracking().Select(y => y.SisterCompanyId).ToListAsync();
        }
        public async Task<List<LocationDto>> GetCustomerAddressByCustomerIds(List<int?> customerIds)
        {
            return await _context.CuAddresses.Where(x => customerIds.Contains(x.CustomerId) && x.Active.HasValue && x.Active.Value
                                    && x.AddressType == (int)RefAddressTypeEnum.HeadOffice)
                                    .Select(x => new LocationDto
                                    {
                                        CustomerId = x.CustomerId.GetValueOrDefault(),
                                        CountryId = x.CountryId,
                                        Country = x.Country.CountryName,
                                        ProvinceId = x.City.ProvinceId,
                                        Province = x.City.Province.ProvinceName,
                                        CityId = x.CityId,
                                        City = x.City.CityName
                                    }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the customer contacts by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<CustomerContactBaseData>> GetCustomerContact(int customerId)
        {
            return await _context.CuContacts.Where(x => x.Active.Value && x.CustomerId == customerId).
                            Select(x => new CustomerContactBaseData()
                            {
                                Id = x.Id,
                                Name = x.ContactName,
                                CustomerId = x.CustomerId
                            }).AsNoTracking().ToListAsync();
        }


        public async Task<CuCustomer> GetCustomerDataByCustomerIdAndEntityId(int customerId, int entityId)
        {
            return await _context.CuCustomers.Where(x => x.Id == customerId && x.CuEntities.Any(x => x.EntityId == entityId && x.Active.Value))
                    .Include(x => x.CuDepartments)
                    .Include(x => x.CuBuyers)
                    .Include(x => x.CuAddresses)
                     .ThenInclude(x => x.Country)
                    .Include(x => x.CuAddresses)
                     .ThenInclude(x => x.City)
                     .IgnoreQueryFilters()
                     .FirstOrDefaultAsync();
        }
    }
}
