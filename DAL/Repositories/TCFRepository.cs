using Contracts.Repositories;
using DTO.TCF;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TCFRepository : Repository, ITCFRepository
    {

        public TCFRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// Get the customer Contacts required for the TCF
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public async Task<TCFCustomerContact> GetTCFCustomerContact(int contactId)
        {
            return await _context.CuContacts.
                Where(x => x.Id == contactId && x.Active.HasValue && x.Active.Value
                       && x.CuContactServices.Any(y => y.ServiceId == (int)Service.Tcf)).
                   Select(x => new TCFCustomerContact
                   {
                       apiCustomerContactId = x.Id,
                       contactName = x.ContactName,
                       email = x.Email,
                       phoneNumber = x.Phone,
                       mobileNumber = x.Mobile,
                       glCode = x.Customer.GlCode
                   }).IgnoreQueryFilters().FirstOrDefaultAsync();


        }

        /// Get the customers required for TCF
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<TCFCustomer> GetTCFCustomer(int customerId)
        {
            return await _context.CuCustomers.
                Where(x => x.Id == customerId && x.Active.HasValue && x.Active.Value && x.CuApiServices.Any(y => y.ServiceId == (int)Service.Tcf)).
                   Select(x => new TCFCustomer
                   {
                       customerName = x.CustomerName,
                       email = !string.IsNullOrEmpty(x.Email) ? x.Email : "",
                       phoneNumber = !string.IsNullOrEmpty(x.Phone) ? x.Phone : "",
                       mobileNumber = !string.IsNullOrEmpty(x.OtherPhone) ? x.OtherPhone : "",
                       glCode = x.GlCode
                   }).IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the Suppliers required for TCF
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<TCFSupplier> GetTCFSupplier(int supplierId)
        {
            return await _context.SuSuppliers.
                Where(x => x.Id == supplierId && x.Active && x.SuApiServices.Any(y => y.ServiceId == (int)Service.Tcf)).
                   Select(x => new TCFSupplier
                   {
                       apiSupplierId = x.Id,
                       supplierName = x.SupplierName
                   }).IgnoreQueryFilters().FirstOrDefaultAsync();

        }

        /// <summary>
        /// Get the Customer GL Codes by supplier id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<List<TCFSupplierCustomer>> GetCustomerGLCodesBySupplier(int supplierId)
        {
            return await _context.SuSupplierCustomers.Where(x => x.SupplierId == supplierId).
                                        Select(y => new TCFSupplierCustomer()
                                        {
                                            SupplierId = y.SupplierId,
                                            GlCode = y.Customer.GlCode.Trim()
                                        }).IgnoreQueryFilters().ToListAsync();
        }

        /// <summary>
        /// Get the supplier contacts to be pushed to tcf
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<List<Suppliercontact>> GetTCFSupplierContacts(int supplierId)
        {
            return await _context.SuContacts.
                Where(x => x.SupplierId == supplierId && x.Active.HasValue && x.Active.Value
                                            && x.SuContactApiServices.Any(y => y.ServiceId == (int)Service.Tcf)).
                   Select(x => new Suppliercontact
                   {
                       apiSupplierContactId = x.Id,
                       contactName = x.ContactName,
                       email = x.Mail,
                       phoneNumber = x.Phone,
                       mobileNumber = x.Mobile,
                       apiSupplierId = x.SupplierId
                   }).IgnoreQueryFilters().ToListAsync();

        }

        /// <summary>
        /// Get the users required for TCF
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<TCFUserRepo> GetTCFUser(int userId)
        {
            return await _context.ItUserMasters.
                Where(x => x.Id == userId && x.Active).
                   Select(x => new TCFUserRepo
                   {
                       apiLinkUserId = x.Id,
                       userName = x.LoginName,
                       email = x.Staff.CompanyEmail,
                       userType = x.UserType.Label,
                       userTypeId = x.UserTypeId,
                       customerId = x.CustomerId,
                       supplierId = x.SupplierId,
                       glCode = x.Customer.GlCode
                   }).FirstOrDefaultAsync();


        }

        /// <summary>
        /// Get the buyer list required for TCF
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<TCFBuyer>> GetTCFBuyerList(int customerId, int? entityId)
        {
            return await _context.CuBuyers.IgnoreQueryFilters().
                Where(x => x.Active && x.CustomerId == customerId && x.EntityId == entityId && x.CuBuyerApiServices.Any(y => y.ServiceId == (int)Service.Tcf)).
                   Select(x => new TCFBuyer
                   {
                       apiBuyerId = x.Id,
                       buyerName = x.Name,
                       glCode = x.Customer.GlCode
                   }).ToListAsync();

        }

        /// <summary>
        /// Get the Product list required for TCF
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<TCFProduct> GetTCFProduct(int productId, int? entityId)
        {
            return await _context.CuProducts.IgnoreQueryFilters().
                Where(x => x.Id == productId && x.Active && x.EntityId == entityId && x.CuProductApiServices.Any(y => y.ServiceId == (int)Service.Tcf)).
                   Select(x => new TCFProduct
                   {
                       apiProductId = x.Id,
                       productRef = x.ProductId,
                       productDescription = x.ProductDescription,
                       barCode = !string.IsNullOrEmpty(x.Barcode) ? x.Barcode : "",
                       productCategoryId = x.ProductCategory,
                       productSubCategoryId = 0
                   }).FirstOrDefaultAsync();
        }

    }
}
