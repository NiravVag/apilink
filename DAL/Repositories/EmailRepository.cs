using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contracts.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Entities;
using Entities.Enums;
using DTO.Email;
using DTO.RepoRequest.Email;

namespace DAL.Repositories
{
    public class EmailRepository : Repository, IEmailRepository
    {

        public EmailRepository(API_DBContext context) : base(context)
        {
        }

        public Task<List<MidEmailRecipientsInternalDefault>> GetInternalDefaultRecipientByEmailType(EmailType emailtypeid)
        {
            return null;
           // return _context.MidEmailRecipientsInternalDefaults.Where(x => x.Active.Value && x.EmailTypeId == (int)emailtypeid).ToListAsync();
        }

        public async Task<EmailRecipientRepoResponse> GetEmailRecipient(EmailRecipientRepoRequest request)
        {
            var reponse = new EmailRecipientRepoResponse();

            //// get email configuration details as iquerable. 
            //var EmailRecipients = _context.MidEmailRecipientsConfigurations
            //                    .Where(x => x.Active.Value && x.EmailTypeId == (int)request.EmailtypeId)
            //                    .Include(x => x.MidEmailRecipientsCusContacts)
            //                    .Include(x => x.MidEmailRecipientsSupContacts)
            //                    .Include(x => x.MidEmailRecipientsFactContacts)
            //                    .Include(x => x.MidEmailRecipientsInternals)
            //                    .Include(x => x.MidEmailRecipientsCusBrands)
            //                    .Include(x => x.MidEmailRecipientsCusBuyers)
            //                    .Include(x => x.MidEmailRecipientsCusDepartments)
            //                    .Include(x => x.MidEmailRecipientsCustomers)
            //                    .Include(x => x.MidEmailRecipientsDestinationCountries)
            //                    .Include(x => x.MidEmailRecipientsFactories)
            //                    .Include(x => x.MidEmailRecipientsFactoryCountries)
            //                    .Include(x => x.MidEmailRecipientsOffices)
            //                    .Include(x => x.MidEmailRecipientsProductCategories)
            //                    .Include(x => x.MidEmailRecipientsProductSubCategories)
            //                    .Include(x => x.MidEmailRecipientsServices)
            //                    .Include(x => x.MidEmailRecipientsServiceTypes)
            //                    .Include(x => x.MidEmailRecipientsSuppliers).AsQueryable();

            ////filter email data based on brand
            //if (request.LstBrand!=null && request.LstBrand.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsCusBrands.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsCusBrands.Any(y => y.CusBrandId != null && request.LstBrand.Contains(y.CusBrandId.Value) && y.Active != null && y.Active.Value));
            //}
            ////filter email data based on buyer
            //if (request.LstBuyer != null && request.LstBuyer.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsCusBuyers.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsCusBuyers.Any(y => y.CusBuyerId != null && request.LstBuyer.Contains(y.CusBuyerId.Value) && y.Active != null && y.Active.Value));
            //}
            ////filter email data based on department
            //if (request.LstDepartment != null && request.LstDepartment.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsCusDepartments.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsCusDepartments.Any(y => y.CusDepartmentId != null && request.LstDepartment.Contains(y.CusDepartmentId.Value) && y.Active != null && y.Active.Value));
            //}
            //// filter email data based on customer id.
            //if (request.LstCustomer != null && request.LstCustomer.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsCustomers.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsCustomers.Any(y => y.CustomerId != null && request.LstCustomer.Contains(y.CustomerId.Value) && y.Active != null && y.Active.Value));
            //}
            //// filter email data based on po destination country id
            //if (request.LstPODestinationCountry != null && request.LstPODestinationCountry.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsDestinationCountries.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsDestinationCountries.Any(y => y.DestinationCountryId != null && request.LstPODestinationCountry.Contains(y.DestinationCountryId.Value) && y.Active != null && y.Active.Value));
            //}
            //if (request.LstFactory != null && request.LstFactory.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsFactories.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsFactories.Any(y => y.FactoryId != null && request.LstFactory.Contains(y.FactoryId.Value) && y.Active != null && y.Active.Value));
            //}
            //if (request.LstFactoryCountry != null && request.LstFactoryCountry.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsFactoryCountries.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsFactoryCountries.Any(y => y.FactoryCountryId != null && request.LstFactoryCountry.Contains(y.FactoryCountryId.Value) && y.Active != null && y.Active.Value));
            //}
            //if (request.LstOffice != null && request.LstOffice.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsOffices.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsOffices.Any(y => y.OfficeId != null && request.LstOffice.Contains(y.OfficeId.Value) && y.Active != null && y.Active.Value));
            //}
            //if (request.LstServiceType != null && request.LstServiceType.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsServiceTypes.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsServiceTypes.Any(y => y.ServiceTypeId != null && request.LstServiceType.Contains(y.ServiceTypeId.Value) && y.Active != null && y.Active.Value));
            //}
            //if (request.LstService != null && request.LstService.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsServices.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsServices.Any(y => y.ServiceId != null && request.LstService.Contains(y.ServiceId.Value) && y.Active != null && y.Active.Value));
            //}
            //if (request.LstSupplier != null && request.LstSupplier.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsSuppliers.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsSuppliers.Any(y => y.SupplierId != null && request.LstSupplier.Contains(y.SupplierId.Value) && y.Active != null && y.Active.Value));
            //}
            //if (request.LstProductcategory != null && request.LstProductcategory.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsProductCategories.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsProductCategories.Any(y => y.ProductCategoryId != null && request.LstProductcategory.Contains(y.ProductCategoryId.Value) && y.Active != null && y.Active.Value));
            //}
            //if (request.LstSubProductcategory != null && request.LstSubProductcategory.Any() && EmailRecipients.Any(x => x.MidEmailRecipientsProductSubCategories.Any()))
            //{
            //    EmailRecipients = EmailRecipients.Where(x => x.MidEmailRecipientsProductSubCategories.Any(y => y.ProductSubCategoryId != null && request.LstSubProductcategory.Contains(y.ProductSubCategoryId.Value) && y.Active != null && y.Active.Value));
            //}
            //reponse.RecipientConfigList = await EmailRecipients.ToListAsync();
            //reponse.Result = InspEmailRecipientRepoResponseResult.success;

            return reponse;

        }

        public Task<List<MidEmailRecipientsCusContactDefault>> GetCusDefaultRecipientByEmailType(EmailType emailtypeid)
        {
            return null;
           // return _context.MidEmailRecipientsCusContactDefaults.Where(x => x.Active && x.EmailTypeId == (int)emailtypeid).ToListAsync();
        }
    }
}
