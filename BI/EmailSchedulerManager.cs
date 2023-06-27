using Contracts.Managers;
using Contracts.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DTO.Schedule;
using DTO.Common;
using System.Threading.Tasks;
using Components.Core.entities.Emails;
using System.Net.Http;
using BI.Maps;
using Entities;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using DTO.Inspection;
using Entities.Enums;

namespace BI
{
    public class EmailSchedulerManager : ApiCommonData, IEmailScheduleManager
    {
        private readonly IEmailScheduleRepository _emailRepo = null;
        private readonly ILocationRepository _locRepo = null;
        private readonly ICombineOrdersManager _combineOrdermanager = null;
        private readonly ICustomerProductManager _customerProductManager = null;
        private static IConfiguration _Configuration = null;
        private static IUserRightsManager _userrightManager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IScheduleRepository _scheduleRepository = null;
        private readonly EmailSchedulerMap _emailschedulemap = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IInspectionBookingManager _bookingmanager = null;
        private readonly IKpiCustomRepository _kpiCustomRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IAuditRepository _auditRepository;

        public EmailSchedulerManager(IEmailScheduleRepository emailRepo, ICombineOrdersManager combineOrdermanager,
                        ICustomerProductManager customerProductManager, ILocationRepository locRepo, IConfiguration configuration,
                       IUserRightsManager userrightManager, IAPIUserContext applicationContext, IScheduleRepository scheduleRepository, IInspectionBookingManager bookingmanager,
                       IInspectionBookingRepository inspRepo, IKpiCustomRepository kpiCustomRepository, IReportRepository reportRepository, IAuditRepository auditRepository)
        {
            _emailRepo = emailRepo;
            _combineOrdermanager = combineOrdermanager;
            _customerProductManager = customerProductManager;
            _locRepo = locRepo;
            _Configuration = configuration;
            _userrightManager = userrightManager;
            _ApplicationContext = applicationContext;
            _scheduleRepository = scheduleRepository;
            _bookingmanager = bookingmanager;
            _emailschedulemap = new EmailSchedulerMap();
            _inspRepo = inspRepo;
            _kpiCustomRepository = kpiCustomRepository;
            _reportRepository = reportRepository;
            _auditRepository = auditRepository;
        }

        public async Task<List<ScheduleQCEmailTemplate>> scheduleQCEmail(bool isFromScheduler, List<int> bookingIdList, string offices)
        {
            List<ScheduleQCEmailTemplate> scheduleQCEmailTemplateList = new List<ScheduleQCEmailTemplate>();
            List<int> bookingIds = new List<int>();
            IEnumerable<ScheduleStaffItem> scheduledQCList = new List<ScheduleStaffItem>();

            List<int> auditIds = new List<int>();
            IEnumerable<ScheduleStaffItem> auditorQCList = new List<ScheduleStaffItem>();

            //Calculate tommorow date and day after tommorrow date
            var tommorowDateTime = DateTime.Now.AddDays(1);
            var dayAfterTommorowDateTime = DateTime.Now.AddDays(2);


            //if request is from the scheduled task, then fetch the bookings scheduled for tomorrow and day after
            if (isFromScheduler)
            {

                // get office ids from the request
                var officeList = string.IsNullOrEmpty(offices) ? new List<int>() : offices.Split(',').Select(int.Parse).ToList();

                //Get scheduled data for qcs between two dates
                scheduledQCList = await _emailRepo.GetScheduledDetailsForQCsByServiceDate(tommorowDateTime.Date, dayAfterTommorowDateTime.Date, officeList);

                //Get audit data for auditor between two dates
                auditorQCList = await _emailRepo.GetAuditorDetailsByServiceDate(tommorowDateTime.Date, dayAfterTommorowDateTime.Date, officeList);

                //Take all the bookingids from the scheduled data
                bookingIds = scheduledQCList.Select(x => x.BookingId).Distinct().ToList();

                //take all the auditids from the scheduled data
                auditIds = auditorQCList.Select(x => x.BookingId).Distinct().ToList();
            }

            //if request is from the UI, send the email for the selected bookings
            if (!isFromScheduler)
            {
                bookingIds = bookingIdList;

                //Get scheduled data for qcs by booking Ids
                scheduledQCList = await _scheduleRepository.GetQCBookingDetails(bookingIds, DateTime.Now.Date);
            }

            var missionUrl = _Configuration["EmailUserVerificationEmail"].ToString();

            List<ScheduleQCEmail> scheduleQCDetails = new List<ScheduleQCEmail>();


            if (bookingIds.Count > 0)
            {
                //Get the inspection list by bookingids
                var inspectionScheduledList = await _emailRepo.GetScheduledInspectionByInspectionList(bookingIds);

                var serviceTypeList = await _inspRepo.GetServiceType(bookingIds);
                var factoryDetails = await _inspRepo.GetFactorycountryId(bookingIds);
                var supplierContacts = await _inspRepo.GetSupplierContactsByBookingIds(bookingIds);
                var factoryContacts = await _inspRepo.GetFactoryContactsByBookingIds(bookingIds);
                var csNameList = await _emailRepo.GetCSName(bookingIds);

                //get CS name
                var cuslist = inspectionScheduledList.Select(x => x.CustomerId).Distinct().ToList();
                var AElist = await _userrightManager.GetAEbyCustomer(cuslist);
                
                var poColorList = await _inspRepo.GetPOColorTransactionsByBookingIds(bookingIds);

                var productTranList = await _bookingmanager.GetProductDetails(bookingIds);

                var poList = await _bookingmanager.GetPoDataByBookingIds(bookingIds);

                var inspectionProducts = await _reportRepository.GetProductListByBooking(bookingIds);

                var inspectionContainers = await _inspRepo.GetBookingContainer(bookingIds);

                var inspectionReportData = await _inspRepo.GetBookingReportQuantityDetails(bookingIds);

                //Map the scheduledQcDetail with scheduled data and inspectiondata
                var result = _emailschedulemap.GetInspectionScheduleQCEmails(scheduledQCList, inspectionScheduledList, missionUrl, AElist, serviceTypeList, factoryDetails, csNameList,
                supplierContacts, factoryContacts, poColorList, productTranList, poList, inspectionProducts, inspectionContainers, inspectionReportData);

                scheduleQCDetails.AddRange(result);
            }

            if (auditIds.Count > 0)
            {
                var auditData = await _emailRepo.GetAuditDataByAuditIds(auditIds);
                var auditFactoryContacts = await _auditRepository.GetFactoryContactsByBookingIds(auditIds);
                var auditSupplierContacts = await _auditRepository.GetSupplierContactsByBookingIds(auditIds);
                var auditFactoryLocations = await _auditRepository.GetFactoryAddressDetailsByAuditIds(auditIds);
                var auditServiceTypes = await _inspRepo.GetAuditServiceType(auditIds);
                var auditCS = await _auditRepository.GetAuditCSByAuditIds(auditIds);

                var auditScheduleQcEmail = _emailschedulemap.GetAuditScheduleQCEmails(auditorQCList, auditData, missionUrl, auditFactoryLocations, auditServiceTypes, auditCS, auditSupplierContacts, auditFactoryContacts);
                scheduleQCDetails.AddRange(auditScheduleQcEmail);
            }


            if (scheduleQCDetails.Count > 0)
            {
                //inspection files
                var inspectionFiles = await _inspRepo.GetBookingMappedFilesByBookingIds(bookingIds);

                //inspection files
                var auditFiles = await _auditRepository.GetAuditBookingMappedFiles(auditIds);

                //Take the distinct qcs
                var distinctQCs = scheduleQCDetails.Select(x => x.QCId).Distinct();

                //Loop through the qc and seperate the scheduled data for each qc
                foreach (var qc in distinctQCs)
                {
                    ScheduleQCEmailTemplate scheduleQCEmailTemplate = new ScheduleQCEmailTemplate();

                    //filter the scheduled data for the specific qc
                    var qcBasedscheduleQCDetails = scheduleQCDetails.Where(x => x.QCId == qc);

                    //take the distinct service date from the qc specific scheduled data
                    var distinctServiceDate = qcBasedscheduleQCDetails.Select(x => x.ServiceDate).Distinct();

                    scheduleQCEmailTemplate.ScheduleQcServiceDateList = new List<ScheduleQcServiceDate>();

                    //Loop through each service date and seperate the scheduled data for each service date
                    foreach (var serviceDate in distinctServiceDate)
                    {

                        ScheduleQcServiceDate scheduleQcServiceDate = new ScheduleQcServiceDate();
                        scheduleQcServiceDate.ServiceDate = serviceDate;
                        var scList = qcBasedscheduleQCDetails.Where(x => x.ServiceDate == serviceDate.Date).ToList();
                        scheduleQcServiceDate.IsShowSoftLineItems = scList.Any(x => x.BusinessLine == (int)BusinessLine.SoftLine);
                        if (scList != null && scList.Any())
                        {
                            scheduleQcServiceDate.ScheduleQCEmailDetail = new List<ScheduleQCEmail>();
                            scheduleQcServiceDate.ScheduleQCEmailDetail.AddRange(scList);
                        }

                        var inspectionIdList = scList.Where(x => x.ServiceId == (int)Service.InspectionId).Select(x => x.BookingId).ToList();

                        var auditIdList = scList.Where(x => x.ServiceId == (int)Service.AuditId).Select(x => x.BookingId).ToList();

                        var inspectionAttachments = inspectionFiles.Where(x => inspectionIdList.Contains(x.BookingId)).GroupBy(y => y.BookingId)
                                                .Select(z => new BookingAttachmentData()
                                                {
                                                    BookingId = z.Key,
                                                    ServiceDate = serviceDate,
                                                    Service = Service_Inspection,
                                                    Attachments = z.Select(x => x)
                                                }).OrderBy(x => x.ServiceDate).ThenBy(y => y.BookingId).ToList();

                        var auditAttachments = auditFiles.Where(x => auditIdList.Contains(x.BookingId)).GroupBy(y => y.BookingId)
                                               .Select(z => new BookingAttachmentData()
                                               {
                                                   BookingId = z.Key,
                                                   ServiceDate = serviceDate,
                                                   Service = Service_Audit,
                                                   Attachments = z.Select(x => x)
                                               }).OrderBy(x => x.ServiceDate).ThenBy(y => y.BookingId).ToList();

                        scheduleQcServiceDate.BookingAttachments = new List<BookingAttachmentData>();

                        scheduleQcServiceDate.BookingAttachments.AddRange(inspectionAttachments);

                        scheduleQcServiceDate.BookingAttachments.AddRange(auditAttachments);

                        scheduleQCEmailTemplate.ScheduleQcServiceDateList.Add(scheduleQcServiceDate);
                        //ScheduleQcServiceDate ScheduleQcServiceDate = new ScheduleQcServiceDate();
                    }


                    scheduleQCEmailTemplate.QCId = qc;
                    scheduleQCEmailTemplate.QCName = qcBasedscheduleQCDetails.Select(x => x.QCName).FirstOrDefault();
                    scheduleQCEmailTemplate.QCEmailID = qcBasedscheduleQCDetails.Select(x => x.QCEmail).FirstOrDefault();
                    scheduleQCEmailTemplate.IsChinaCountry = qcBasedscheduleQCDetails.Select(x => x.IsChinaCountry).FirstOrDefault();

                    //check for valid email
                    if (new EmailAddressAttribute().IsValid(_ApplicationContext?.EmailId))
                        scheduleQCEmailTemplate.CurrentUserEmailID = _ApplicationContext?.EmailId;

                    //set dates for the email subject
                    if (isFromScheduler)
                    {
                        scheduleQCEmailTemplate.ServiceFromDate = tommorowDateTime;
                        scheduleQCEmailTemplate.ServiceToDate = dayAfterTommorowDateTime;
                        scheduleQCEmailTemplate.TotalNumberofDays = Convert.ToInt32((dayAfterTommorowDateTime - tommorowDateTime).TotalDays) + 1;
                    }
                    //set the dates for the email subject with min - max dates
                    else
                    {
                        scheduleQCEmailTemplate.ServiceFromDate = scheduleQCEmailTemplate.ScheduleQcServiceDateList.Min(x => x.ServiceDate);
                        scheduleQCEmailTemplate.ServiceToDate = scheduleQCEmailTemplate.ScheduleQcServiceDateList.Max(x => x.ServiceDate);
                    }

                    scheduleQCEmailTemplate.IsFromScheduler = isFromScheduler;
                    scheduleQCEmailTemplate.ScheduleQcServiceDateList.Sort((x, y) => DateTime.Compare(x.ServiceDate, y.ServiceDate));

                    scheduleQCEmailTemplateList.Add(scheduleQCEmailTemplate);
                }
            }
            return scheduleQCEmailTemplateList;
        }

    }
}
