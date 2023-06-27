using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.BookingRuleContact;
using DTO.Common;
using DTO.DataAccess;
using DTO.Inspection;
using DTO.MasterConfig;
using DTO.OfficeLocation;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class InspBookingRuleContactManager : IInspBookingRuleContactManager
    {
        #region declare
        private readonly ICacheManager _cache = null;
        private readonly IInspBookingRuleContactRepository _bookingRuleContactRepository = null;
        private readonly ISupplierManager _suppliermanager = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly IHumanResourceManager _humanresourcemanager = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly IUserRightsManager _userManager = null;
        private readonly BookingMap _bookingamap = null;
        private readonly IUserConfigRepository _userConfigRepository = null;

        #endregion declare

        #region Constructor
        public InspBookingRuleContactManager(IInspBookingRuleContactRepository repository,
            ISupplierManager suppliermanager, IOfficeLocationManager office, IHumanResourceManager humanresourcemanager,
            ICacheManager cache, IAPIUserContext applicationContext, IUserRightsManager userManager, IUserConfigRepository userConfigRepository)
        {
            _cache = cache;
            _bookingRuleContactRepository = repository;
            _suppliermanager = suppliermanager;
            _office = office;
            _humanresourcemanager = humanresourcemanager;
            _applicationContext = applicationContext;
            _userManager = userManager;
            _userConfigRepository = userConfigRepository;
            _bookingamap = new BookingMap();

        }
        #endregion Constructor

        #region Booking contact

        public async Task<BookingContactResponse> GetBookingContactInformation(int factoryId, int customerId)
        {
            try
            {
                var response = new BookingContactResponse() { };
                var office = await _office.GetOfficeByFactoryidInspection(factoryId);
                if (office == null)
                    return new BookingContactResponse() { Result = BookingContactResult.CannotGetOfficeList };
                var factdetails = await _suppliermanager.GetSupplierHeadOfficeAddress(factoryId);
                var FactoryCountryId = factdetails?.countryId ?? 0;

                var userAccessFilter = new UserAccess
                {
                    OfficeId = office.Id,
                    ServiceId = (int)Service.InspectionId,
                    CustomerId = customerId,
                    RoleId = (int)RoleEnum.InspectionVerified
                };
                //Get the user details based on DA_UserCustomer and DA_UserRoleNotificationByOffice tables
                var data = _userManager.GetCSBookingContact(userAccessFilter).Result.Take(2);
                if (data == null || data.Count() == 0)
                    return new BookingContactResponse() { Result = BookingContactResult.CannotGetContactDetails };

                response.BookingContact = _bookingamap.GetBookingContactModel(data, office);

                response.Result = BookingContactResult.GetContactDetailsSuccess;
                return response;
            }
            catch (Exception ex)
            {
                return new BookingContactResponse() { Result = BookingContactResult.CannotGetOfficeList };
            }

        }

        public async Task<InspBookingContactResponse> GetInspBookingContactInformation(UserAccess userAccessFilter)
        {
            try
            {
                var response = new InspBookingContactResponse() { };
                var data = await _userManager.GetCustomerServiceBookingContacts(userAccessFilter);
                if (data == null || data.Count() == 0)
                    return new InspBookingContactResponse() { Result = BookingContactResult.CannotGetContactDetails };
                response.BookingContactList = data;
                return response;

            }
            catch (Exception ex)
            {
                return new InspBookingContactResponse() { Result = BookingContactResult.CannotGetOfficeList };
            }

        }


        #endregion Booking contact

        #region Booking Rule
        public async Task<InspBookingRuleResponse> GetInspBookingRules(int customerId, int? factoryId)
        {
            try
            {
                int? factoryCountryid = null;
                Office office = null;

                if (factoryId > 0)
                {
                    var factdetails = await _suppliermanager.GetSupplierHeadOfficeAddress(factoryId.GetValueOrDefault());
                    factoryCountryid = factdetails?.countryId ?? 0;
                    office = await _office.GetOfficeByFactoryid(factoryId.GetValueOrDefault());
                }

                var response = new InspBookingRuleResponse() { BookingRuleList = new InspBookingRules(), Result = new BookingRuleResult() };
               
                // holidays
                var data = await _bookingRuleContactRepository.GetInspBookingRule();
                if (factoryCountryid > 0)
                {
                    var holidays = await _humanresourcemanager.GetHolidaysByRange(DateTime.Now, DateTime.Now.AddYears(1), factoryCountryid.GetValueOrDefault(), office?.Id ?? 0);
                    response.BookingRuleList.Holidays = holidays;
                }
                
                // getting audit lead days and bookingRule
                if (data.Any(x => x.CustomerId == customerId && x.FactoryCountryId == factoryCountryid && x.Active))
                {
                    var ruledetails = data.Where(x => x.CustomerId == customerId && x.FactoryCountryId == factoryCountryid && x.Active).FirstOrDefault();
                    if (ruledetails == null || ruledetails.BookingRule == string.Empty || ruledetails.LeadDays == 0)
                        return new InspBookingRuleResponse() { Result = BookingRuleResult.BookingRuleNotFound };
                    response.BookingRuleList.BookingRule = ruledetails?.BookingRule;
                    response.BookingRuleList.LeadDays = ruledetails?.LeadDays ?? 0;
                    response.Result = BookingRuleResult.Success;
                }
                else if (data.Any(x => x.IsDefault && x.FactoryCountryId == factoryCountryid && x.Active))
                {
                    var ruledetails = data.Where(x => x.IsDefault && x.FactoryCountryId == factoryCountryid && x.Active).FirstOrDefault();
                    if (ruledetails == null || ruledetails.BookingRule == string.Empty || ruledetails.LeadDays == 0)
                        return new InspBookingRuleResponse() { Result = BookingRuleResult.BookingRuleNotFound };
                    response.BookingRuleList.BookingRule = ruledetails?.BookingRule;
                    response.BookingRuleList.LeadDays = ruledetails?.LeadDays ?? 0;
                    response.Result = BookingRuleResult.Success;
                }
                else if (data.Any(x => x.IsDefault && x.Active))
                {
                    var ruledetails = data.Where(x => x.IsDefault && x.Active).FirstOrDefault();
                    if (ruledetails == null || ruledetails.BookingRule == string.Empty || ruledetails.LeadDays == 0)
                        return new InspBookingRuleResponse() { Result = BookingRuleResult.BookingRuleNotFound };
                    response.BookingRuleList.BookingRule = ruledetails?.BookingRule;
                    response.BookingRuleList.LeadDays = ruledetails?.LeadDays ?? 0;
                    response.Result = BookingRuleResult.Success;
                }
                else
                {
                    return new InspBookingRuleResponse() { Result = BookingRuleResult.BookingRuleNotFound };
                }

                //add service lead day message
                var masterConfigQuery = _userConfigRepository.GetMasterConfigurationQuery();

                var leadTimeMsg = await masterConfigQuery.Where(x => x.Type == (int)EntityConfigMaster.LeadTimeMessage).Select(x => x.Value).FirstOrDefaultAsync();

                response.BookingRuleList.ServiceLeadDaysMessage = String.Format(leadTimeMsg, response.BookingRuleList.LeadDays, response.BookingRuleList.LeadDays);

                return response;


            }
            catch (Exception ex)
            {
                return new InspBookingRuleResponse() { Result = BookingRuleResult.BookingRuleNotFound };
            }
        }

        public async Task<InspectionHolidaySummaryList> GetInspBookingHolidaysDate(int factoryCountryId, int factoryId)
        {
            try
            {
                var response = new InspectionHolidaySummaryList() { Result = new HolidayResult() };
                // getting holidays + week ends
                var tdyDate = DateTime.Now.Date;
                var office = await _office.GetOfficeByFactoryid(factoryId);

                var holidays = await _humanresourcemanager.GetHolidaysDateByRange(tdyDate, tdyDate.AddYears(1), factoryCountryId, office?.Id ?? 0);
                if (holidays != null && holidays.Count() > 0)
                {
                    response.HolidaysDate = holidays;
                    response.Result = HolidayResult.Success;
                    return response;
                }
                else
                {
                    return new InspectionHolidaySummaryList() { Result = HolidayResult.NotFound };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Booking Rule
    }
}
