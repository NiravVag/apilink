using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.EmailSend;
using DTO.EmailSendingDetails;
using DTO.InspectionCustomerDecision;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class EmailSendingDetailsManager : IEmailSendingDetailsManager
    {

        private readonly IEmailSendingDetailsRepository _repo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IEmailSendManager _emailSendManager = null;
        private readonly IEmailSendRepository _emailSendRepo = null;
        private readonly IInspectionBookingRepository _inspRepo = null;

        public EmailSendingDetailsManager(IEmailSendingDetailsRepository repo, IAPIUserContext ApplicationContext, IEmailSendManager emailSendManager,
            IEmailSendRepository emailSendRepo, IInspectionBookingRepository inspRepo)
        {
            _repo = repo;
            _ApplicationContext = ApplicationContext;
            _emailSendManager = emailSendManager;
            _emailSendRepo = emailSendRepo;
            _inspRepo = inspRepo;
        }

        /// <summary>
        /// get customer configuration email contacts
        /// </summary>
        /// <param name="emailBookingData"></param>
        /// <returns></returns>
        public async Task<EmailPreviewResponse> GetCustomerDecisionEmailConfigurationContacts(List<int> reportIdList, int bookingId, bool sendEmailToFactoryContacts, int customerResultId)
        {
            var emailConfigList = new List<EmailSendConfigBooking>();
            EmailPreviewResponse emailData = new EmailPreviewResponse();

            var bookingIdList = new[] { bookingId }.ToList();
            var bookingList = await _emailSendRepo.GetBookingDetails(bookingIdList);

            //get the booking brands
            var bookingBrandsIdList = await _inspRepo.GetBrandBookingIdsByBookingIds(bookingIdList);
            //get the booking buyers
            var bookingBuyersIdList = await _inspRepo.GetBuyerBookingIdsByBookingIds(bookingIdList);
            //get the booking departments
            var bookingDepartmentIdList = await _inspRepo.GetDeptBookingIdsByBookingIds(bookingIdList);
            //get the booking product (productcategory,report result)
            var bookingProductReportResult = await _inspRepo.GetBookingProductAndReportResult(bookingIdList);
            //get the booking container report results
            var bookingContainerReportResult = await _inspRepo.GetContainerReportDataByBooking(bookingIdList);
            //get the booking service type list
            var bookingServiceTypeList = await _inspRepo.GetServiceType(bookingIdList);

            //get the customer list
            var customerList = bookingList.Select(x => x.CustomerId).ToList();

            var bookingItem = bookingList.FirstOrDefault();

            if (bookingItem != null)
            {
                //get the email config base data
                var emailConfigBaseData = await _emailSendRepo.GetEmailConfigurationBaseDetails((int)EmailSendingType.CustomerDecision, customerList, (int)Service.InspectionId);

                //take the email send details id
                var esDetailsId = emailConfigBaseData.Select(x => x.Id).ToList();

                //get the email send customer config data
                var esCustomerConfigData = await _emailSendRepo.GetESCustomerConfigDetails(esDetailsId);

                //get the email send customer contact data
                var esCustomerContactData = await _emailSendRepo.GetESCustomerContactDetails(esDetailsId);

                //get the email send service type data
                var esServiceTypeData = await _emailSendRepo.GetESServiceTypeDetails(esDetailsId);

                //get the email send factory country config data
                var esFactoryCountryData = await _emailSendRepo.GetESFactoryCountryDetails(esDetailsId);

                //get the email send supplier or factory config data
                var esSupplierOrFactoryData = await _emailSendRepo.GetESSupplierOrFactoryDetails(esDetailsId);

                //get the email send office config data
                var esOfficeDetailsData = await _emailSendRepo.GetESOfficeDetails(esDetailsId);

                //get the email send api contact config data
                var esApiContactData = await _emailSendRepo.GetESApiContactDetails(esDetailsId);

                //get the email send report result config data
                var esReportResultData = await _emailSendRepo.GetESReportResultDetails(esDetailsId);

                //get the email send product category config data
                var esProductCategoryData = await _emailSendRepo.GetESProductCategoryDetails(esDetailsId);

                //get the email send special rule config data
                var esSpecialRuleData = await _emailSendRepo.GetESSpecialRuleDetails(esDetailsId);

                var emailConfigurationList = _emailSendManager.GetEmailConfigurationList(bookingItem.CustomerId, emailConfigBaseData, esCustomerConfigData, esCustomerContactData,
                    esServiceTypeData, esFactoryCountryData, esSupplierOrFactoryData, esOfficeDetailsData, esApiContactData, esReportResultData,
                    esProductCategoryData, esSpecialRuleData, (int)EmailSendingType.CustomerDecision);

                bookingItem.BrandIdList = bookingBrandsIdList.Where(x => x.BookingId == bookingItem.BookingId).Select(x => x.BrandId).ToList();

                //assign the buyer list

                bookingItem.BuyerIdList = bookingBuyersIdList.Where(x => x.BookingId == bookingItem.BookingId).Select(x => x.BuyerId).ToList();

                //assign the department list

                bookingItem.DepartmentIdList = bookingDepartmentIdList.Where(x => x.BookingId == bookingItem.BookingId).Select(x => x.DeptId).ToList();

                //assign the service type list

                bookingItem.ServiceTypeIdList = bookingServiceTypeList.Where(x => x.InspectionId == bookingItem.BookingId).Select(x => x.serviceTypeId).ToList();

                //assign the report result list

                bookingItem.ProductCategoryIdList = bookingProductReportResult.Where(x => x.InspectionId == bookingItem.BookingId && x.ProductCategoryId != null).Select(x => x.ProductCategoryId.GetValueOrDefault(0)).ToList();

                bookingItem.NonContainerReportResultIds = bookingProductReportResult.Where(x => x.InspectionId == bookingItem.BookingId && x.ResultId != null).Select(x => x.ResultId.GetValueOrDefault(0)).ToList();

                //assign the container report result list

                bookingItem.ContainerReportResultIds = bookingProductReportResult.Where(x => x.InspectionId == bookingItem.BookingId && x.ResultId != null).Select(x => x.ResultId.GetValueOrDefault(0)).ToList();

                emailConfigList.Add(_emailSendManager.GetEmailConfigurationListByBookingData(bookingItem, emailConfigurationList, customerResultId));

                var ruleData = _emailSendManager.GetEmailRuleData(emailConfigList, new[] { bookingId }.ToList());

                if (ruleData.Result == EmailSendResult.MoreThanOneRuleFound)
                {
                    return new EmailPreviewResponse { Result = EmailPreviewResponseResult.multipleRuleFound };
                }

                var bookingReportList = new List<EmailReportPreviewDetail>();

                foreach (var item in reportIdList)
                {
                    bookingReportList.Add(new EmailReportPreviewDetail() { ReportId = item, BookingId = bookingId });
                }

                EmailPreviewRequest req = new EmailPreviewRequest
                {
                    EmailRuleId = ruleData.RuleId,
                    EmailReportPreviewData = bookingReportList,
                    EsTypeId = (int)EmailSendingType.CustomerDecision
                };


                emailData = await _emailSendManager.FetchEmaildetailsbyEmailRule(req);

                if (sendEmailToFactoryContacts && emailData.EmailCCList != null && emailData.EmailCCList.Any())
                {
                    var factoryContacts = await _repo.GetBookingFactoryEmailContacts(bookingId);

                    foreach (var newItem in factoryContacts.Select(x => x))
                    {
                        if (newItem != null && new EmailAddressAttribute().IsValid(newItem))
                        {
                            emailData.EmailCCList.Add(newItem);
                        }
                    }
                }
            }
            return emailData;
        }
    }
}
