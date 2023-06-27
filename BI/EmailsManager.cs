using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.Email;
using Entities.Enums;
using Contracts.Repositories;
using System.Linq;
using BI.Maps;
using DTO.RepoRequest;
using DTO.RepoRequest.Email;

namespace BI
{
    public class EmailsManager : IEmailsManager
    {
        private readonly IEmailRepository _emailrepo = null;
        private readonly IHumanResourceRepository _hrrepo = null;
        private readonly ICustomerContactRepository _cuscontactrepo = null;
        private readonly ISupplierRepository _suprepo = null;
        private readonly IInspectionBookingRepository _insprepo = null;
        private readonly IAuditRepository _auditrepo = null;
        private readonly EmailMap _emailmap = null;
        public EmailsManager(IEmailRepository emailrepo, IHumanResourceRepository hrrepo, ICustomerContactRepository cuscontactrepo, ISupplierRepository suprepo,
            IInspectionBookingRepository insprepo, IAuditRepository auditrepo)
        {
            _emailrepo = emailrepo;
            _hrrepo = hrrepo;
            _cuscontactrepo = cuscontactrepo;
            _suprepo = suprepo;
            _insprepo = insprepo;
            _auditrepo = auditrepo;
            _emailmap = new EmailMap();
        }

        public async Task<AudEmailRecipientResponse> GetAudEmailRecipient(AudEmailRecipientRequest request)
        {
            var AuditData = await _auditrepo.GetAuditDetailsByListId(request.LstAuditIds);
            if (AuditData != null && AuditData.Any())
            {
                var reporequest = new EmailRecipientRepoRequest();
                reporequest.EmailtypeId = request.EmailtypeId;
                reporequest.LstBrand = AuditData.Any(x => x.BrandId != null) ? AuditData.Where(y => y.BrandId != null).Select(x => x.BrandId.Value).ToList() : null;
                reporequest.LstDepartment = AuditData.Any(x => x.DepartmentId != null) ? AuditData.Where(y => y.DepartmentId != null).Select(x => x.DepartmentId.Value).ToList() : null;
                reporequest.LstCustomer = AuditData.Select(x => x.CustomerId).ToList();
                reporequest.LstFactory = AuditData.Select(x => x.FactoryId).ToList();
                reporequest.LstSupplier = AuditData.Select(x => x.SupplierId).ToList();
                reporequest.LstFactoryCountry = AuditData.Any(x => x.Factory.SuAddresses.Any()) ? AuditData.Select(x => x.Factory).SelectMany(y => y.SuAddresses).Select(z => z.CountryId).ToList() : null;
                reporequest.LstService = new List<int>((int)Service.AuditId);
                reporequest.LstOffice = AuditData.Select(x => x.OfficeId.Value).ToList();
                reporequest.LstServiceType = AuditData.SelectMany(x => x.AudTranServiceTypes).Select(y => y.ServiceTypeId).ToList();

                var Reporesponse = await _emailrepo.GetEmailRecipient(reporequest);

                if (Reporesponse != null && Reporesponse.RecipientConfigList != null && Reporesponse.RecipientConfigList.Any())
                {
                    var response = new AudEmailRecipientResponse();

                    //get cus booking contacts
                    var cusbookingcontact = Reporesponse.RecipientConfigList.Where(x => x.IsCusBookingContact != null && x.IsCusBookingContact.Value).Count() > 0 ? await _cuscontactrepo.GetCustomerContactsList(AuditData.SelectMany(x => x.AudTranCuContacts).Select(y => y.ContactId).ToList()) : null;
                    response.CusBookingContact = cusbookingcontact != null && cusbookingcontact.Any() ? cusbookingcontact.Select(_emailmap.GetCusEmailContact) : null;
                    //get sup booking contacts
                    var supbookingcontact = Reporesponse.RecipientConfigList.Where(x => x.IsSupBookingContact.Value).Count() > 0 ? await _suprepo.GetSupplierContactsList(AuditData.SelectMany(x => x.AudTranSuContacts).Select(y => y.ContactId).ToList()) : null;
                    response.FactBookingContact = supbookingcontact != null && supbookingcontact.Any() ? supbookingcontact.Select(_emailmap.GetSupplierEmailContact) : null;
                    //get fact booking contacts
                    var factbookingcontact = Reporesponse.RecipientConfigList.Where(x => x.IsFactBookingContact.Value).Count() > 0 ? await _suprepo.GetSupplierContactsList(AuditData.SelectMany(x => x.AudTranFaContacts).Select(y => y.ContactId).ToList()) : null;
                    response.SupBookingContact = factbookingcontact != null && factbookingcontact.Any() ? factbookingcontact.Select(_emailmap.GetSupplierEmailContact) : null;

                    //get recipient contacts
                    response.EmailDetails = await GetEmailRecipient(Reporesponse, request.EmailtypeId);
                    response.Result = response.EmailDetails.Result == EmailRecipientResult.success ? AudEmailRecipientResponseResult.success : AudEmailRecipientResponseResult.Error;
                    return response;
                }
                else
                {
                    return new AudEmailRecipientResponse { Result = AudEmailRecipientResponseResult.NoEmailRuleFound };
                }
            }
            else
            {
                return new AudEmailRecipientResponse { Result = AudEmailRecipientResponseResult.NoBookingFound };
            }
        }

        public async Task<DeafultEmailRecipientResponse> GetInternalDefaultRecipientByEmailType(EmailType emailtypeid)
        {
            var response = new DeafultEmailRecipientResponse();
            var emailrepo = await _emailrepo.GetInternalDefaultRecipientByEmailType(emailtypeid);
            if (emailrepo == null || emailrepo.Count == 0)
                return new DeafultEmailRecipientResponse { Result = DeafultEmailRecipientResponseResult.NoData };

            var hrstaff = await _hrrepo.GetStaffListByIds(emailrepo.Select(x => x.InternalContactId.Value).ToList());
            if (hrstaff == null || hrstaff.Count == 0)
                return new DeafultEmailRecipientResponse { Result = DeafultEmailRecipientResponseResult.NoEmail };
            response.DefaultRecipients = hrstaff != null && hrstaff.Any() ? hrstaff.Select(_emailmap.GetInternalEmailContact) : null;
            response.Result = DeafultEmailRecipientResponseResult.Success;
            return response;
        }
        public async Task<DeafultEmailRecipientResponse> GetCustomerDefaultRecipientByEmailType(EmailType emailtypeid)
        {
            var response = new DeafultEmailRecipientResponse();
            var emailrepo = await _emailrepo.GetCusDefaultRecipientByEmailType(emailtypeid);
            if (emailrepo == null || emailrepo.Count == 0)
                return new DeafultEmailRecipientResponse { Result = DeafultEmailRecipientResponseResult.NoData };

            var cuscontact = await _cuscontactrepo.GetCustomerContactsList(emailrepo.Select(x => x.CusContactId).ToList());
            if (cuscontact == null || cuscontact.Count == 0)
                return new DeafultEmailRecipientResponse { Result = DeafultEmailRecipientResponseResult.NoEmail };
            response.DefaultRecipients = cuscontact != null && cuscontact.Any() ? cuscontact.Select(_emailmap.GetCusEmailContact) : null;
            response.Result = DeafultEmailRecipientResponseResult.Success;
            return response;
        }
        public async Task<InspEmailRecipientResponse> GetInspEmailRecipient(InspEmailRecipientRequest request)
        {
            //get all inspections details based on Booking id
            var InspData = await _insprepo.GetInspectionList(request.LstInspectionIds);
            if (InspData != null && InspData.Any())
            {
                var reporequest = new EmailRecipientRepoRequest();
                reporequest.EmailtypeId = request.EmailtypeId;
                reporequest.LstBrand = InspData.Any(x => x.InspTranCuBrands.Any()) ? InspData.SelectMany(x => x.InspTranCuBrands).Select(x => x.BrandId).Distinct().ToList() : null;
                reporequest.LstBuyer = InspData.Any(x => x.InspTranCuBuyers.Any()) ? InspData.SelectMany(x => x.InspTranCuBuyers).Select(x => x.BuyerId).Distinct().ToList() : null;
                reporequest.LstDepartment = InspData.Any(x => x.InspTranCuDepartments.Any()) ? InspData.SelectMany(x => x.InspTranCuDepartments).Select(x => x.DepartmentId).Distinct().ToList() : null;
                reporequest.LstCustomer = InspData.Select(x => x.CustomerId).ToList();
                reporequest.LstFactory = InspData.Where(x => x.FactoryId>0).Select(x => x.FactoryId.GetValueOrDefault()).ToList();
                reporequest.LstSupplier = InspData.Select(x => x.SupplierId).ToList();
                reporequest.LstPODestinationCountry = InspData.Any(x => x.InspPurchaseOrderTransactions.Where(t => t.Active.HasValue && t.Active.Value).
                                                Any(y => y?.DestinationCountryId != null)) ? InspData.SelectMany(x => x.InspPurchaseOrderTransactions).
                                                Select(z => z.DestinationCountryId.Value).Distinct().ToList() : null;
                reporequest.LstFactoryCountry = InspData.Any(x => x.Factory.SuAddresses.Any()) ? InspData.Select(x => x.Factory).SelectMany(y => y.SuAddresses).Select(z => z.CountryId).Distinct().ToList() : null;
                reporequest.LstService = new List<int>((int)Service.InspectionId);
                reporequest.LstOffice = InspData.Where(y => y.OfficeId != null).Select(x => x.OfficeId.Value).ToList();
                reporequest.LstServiceType = InspData.SelectMany(x => x.InspTranServiceTypes).Select(y => y.ServiceTypeId).ToList();
                reporequest.LstProductcategory = InspData.SelectMany(x => x.InspProductTransactions).Where(z => z.Product.ProductCategory != null).Select(y => y.Product.ProductCategory.Value).Distinct().ToList();
                reporequest.LstSubProductcategory = InspData.SelectMany(x => x.InspProductTransactions).Where(z => z.Product.ProductCategorySub2 != null).Select(y => y.Product.ProductCategorySub2.Value).Distinct().ToList();
                var Reporesponse = await _emailrepo.GetEmailRecipient(reporequest);

                if (Reporesponse != null && Reporesponse.RecipientConfigList != null && Reporesponse.RecipientConfigList.Any())
                {
                    var response = new InspEmailRecipientResponse();

                    //get cus booking contacts
                    var cusbookingcontact = Reporesponse.RecipientConfigList.Where(x => x.IsCusBookingContact.Value).Count() > 0 ? await _cuscontactrepo.GetCustomerContactsList(InspData.SelectMany(x => x.InspTranCuContacts).Select(y => y.ContactId).ToList()) : null;
                    response.CusBookingContact = cusbookingcontact != null && cusbookingcontact.Any() ? cusbookingcontact.Select(_emailmap.GetCusEmailContact) : null;
                    //get sup booking contacts
                    var supbookingcontact = Reporesponse.RecipientConfigList.Where(x => x.IsSupBookingContact.Value).Count() > 0 ? await _suprepo.GetSupplierContactsList(InspData.SelectMany(x => x.InspTranSuContacts).Select(y => y.ContactId).ToList()) : null;
                    response.FactBookingContact = supbookingcontact != null && supbookingcontact.Any() ? supbookingcontact.Select(_emailmap.GetSupplierEmailContact) : null;
                    //get fact booking contacts
                    var factbookingcontact = Reporesponse.RecipientConfigList.Where(x => x.IsFactBookingContact.Value).Count() > 0 ? await _suprepo.GetSupplierContactsList(InspData.SelectMany(x => x.InspTranFaContacts).Select(y => y.ContactId.Value).ToList()) : null;
                    response.SupBookingContact = factbookingcontact != null && factbookingcontact.Any() ? factbookingcontact.Select(_emailmap.GetSupplierEmailContact) : null;

                    //get recipient contacts
                    response.EmailDetails = await GetEmailRecipient(Reporesponse, request.EmailtypeId);
                    response.Result = InspEmailRecipientResponseResult.success;

                    return response;
                }
                else
                {
                    return new InspEmailRecipientResponse { Result = InspEmailRecipientResponseResult.NoEmailRuleFound };
                }
            }
            else
            {
                return new InspEmailRecipientResponse { Result = InspEmailRecipientResponseResult.NoBookingFound };
            }
        }

        private async Task<EmailRecipient> GetEmailRecipient(EmailRecipientRepoResponse Reporesponse, EmailType emailtypeid)
        {
            var response = new EmailRecipient();

            var cusrecipients = Reporesponse.RecipientConfigList.Any(x => x.MidEmailRecipientsCusContacts.Any()) ?
            await _cuscontactrepo.GetCustomerContactsList(Reporesponse.RecipientConfigList.SelectMany(x => x.MidEmailRecipientsCusContacts).Select(y => y.CusContactId).ToList()) : null;
            response.LstCusRecipient = cusrecipients != null && cusrecipients.Any() ? cusrecipients.Select(_emailmap.GetCusEmailContact) : null;

            var suprecipients = Reporesponse.RecipientConfigList.Any(x => x.MidEmailRecipientsSupContacts.Any()) ?
            await _suprepo.GetSupplierContactsList(Reporesponse.RecipientConfigList.SelectMany(x => x.MidEmailRecipientsSupContacts).Select(y => y.SupContactId).ToList()) : null;
            response.LstSupRecipient = suprecipients != null && suprecipients.Any() ? suprecipients.Select(_emailmap.GetSupplierEmailContact) : null;

            var factrecipients = Reporesponse.RecipientConfigList.Any(x => x.MidEmailRecipientsFactContacts.Any()) ?
            await _suprepo.GetSupplierContactsList(Reporesponse.RecipientConfigList.SelectMany(x => x.MidEmailRecipientsFactContacts).Select(y => y.FactContactId).ToList()) : null;
            response.LstFactRecipient = factrecipients != null && factrecipients.Any() ? factrecipients.Select(_emailmap.GetSupplierEmailContact) : null;


            var internalrecipients = Reporesponse.RecipientConfigList.Any(x => x.MidEmailRecipientsInternals.Any()) ?
            await _hrrepo.GetStaffListByIds(Reporesponse.RecipientConfigList.SelectMany(x => x.MidEmailRecipientsInternals).Select(y => y.InternalContactId).ToList()) : null;
            response.LstInternalRecipient = internalrecipients != null && internalrecipients.Any() ? internalrecipients.Select(_emailmap.GetInternalEmailContact) : null;

            var internalaccountrecipients = Reporesponse.RecipientConfigList.Any(x => x.MidEmailRecipientsInternals.Any(y => y.IsAccounting)) ?
            await _hrrepo.GetStaffListByIds(Reporesponse.RecipientConfigList.SelectMany(x => x.MidEmailRecipientsInternals).Where(z => z.IsAccounting).Select(y => y.InternalContactId).ToList()) : null;
            response.LstInternalAccountingRecipient = internalaccountrecipients != null && internalaccountrecipients.Any() ? internalaccountrecipients.Select(_emailmap.GetInternalEmailContact) : null;

            var defaultcusrecipient = await GetCustomerDefaultRecipientByEmailType(emailtypeid);
            response.LstDefaultCusRecipient = defaultcusrecipient != null && defaultcusrecipient.Result == DeafultEmailRecipientResponseResult.Success ? defaultcusrecipient.DefaultRecipients : null;

            var defaultinternalrecipient = await GetInternalDefaultRecipientByEmailType(emailtypeid);
            response.LstDefaultInternalRecipient = defaultinternalrecipient != null && defaultinternalrecipient.Result == DeafultEmailRecipientResponseResult.Success ? defaultinternalrecipient.DefaultRecipients : null;
            response.Result = EmailRecipientResult.success;
            return response;
        }
    }
}
