using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.CommonClass;
using DTO.Quotation;
using DTO.Schedule;
using DTO.UserAccount;
using Entities;

namespace Contracts.Repositories
{
    public interface IScheduleRepository : IRepository
    {
        /// <summary>
        /// Returns all the inspections to the schedule page
        /// </summary>        
        /// <returns></returns>
        IQueryable<ScheduleBookingInfo> GetAllInspections();

        /// <summary>
        /// Returns the staff list with profile QC
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        //Task<IEnumerable<HrStaffProfile>> GetQCStaffList(IEnumerable<int> location);
        /// <summary>
        /// Get the QC details for the booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="QcType"></param>
        /// <returns></returns>
        Task<List<SchScheduleQc>> GetQCDetails(int bookingId, int QcType);

        /// <summary>
        /// Get the CS details for the booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<List<SchScheduleC>> GetCSDetails(int bookingId);

        /// <summary>
        /// Get Inspected based on Inspection Id
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        Task<InspTransaction> GetInspectionByID(int inspectionID);
        /// <summary>
        /// get assigned qc list based on service date and qc id
        /// </summary>
        /// <param name="serviceDate"></param>
        /// <param name="QCType"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<int> AssignedQCList(DateTime serviceDate, int QCType, int staffId);
        /// <summary>
        /// get booking details show in schedule allocation
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<AllocationBookingItem> GetBookingDetails(int bookingId);
        /// <summary>
        /// get qc name based on qc id
        /// </summary>
        /// <param name="qcId"></param>
        /// <returns></returns>
        Task<List<int>> GetQCBooking(List<int> qcId);
        /// <summary>
        /// get product details from booking po transaction table
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<InspTransaction> GetBookingProduct(int bookingId);
        /// <summary>
        /// one QC manday get based on service date count
        /// </summary>
        /// <param name="qcId"></param>
        /// <param name="serviceDate"></param>
        /// <returns></returns>
        Task<int> GetQcManDay(int qcId, DateTime serviceDate);
        /// <summary>
        /// get one QC manday list based on service date
        /// </summary>
        /// <param name="qcId"></param>
        /// <param name="serviceDate"></param>
        /// <returns></returns>
        Task<IEnumerable<SchScheduleQc>> GetQcListManDay(List<int> qcIdlst, List<DateTime> serviceDatelst);
        /// <summary>
        /// get acutal manday based on qc and service date
        /// </summary>
        /// <param name="qcId"></param>
        /// <param name="serviceDate"></param>
        /// <returns></returns>
        //Task<double> GetQCManday(int qcId, DateTime serviceDate);
        /// <summary>
        /// save SchScheduleQc table
        /// </summary>
        /// <param name="schScheduleQc"></param>
        /// <returns></returns>
        Task<int> SaveQC(SchScheduleQc schScheduleQc);
        /// <summary>
        /// get count of QC profile based on location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        Task<int> GetQCCountbyLocation(IEnumerable<int> location);
        /// <summary>
        /// sum of QC manday based on service date
        /// </summary>
        /// <param name="serviceDate"></param>
        /// <returns></returns>
        Task<double> QcCountOnDate(DateTime serviceDate, List<int> locationList);
        /// <summary>
        /// get quotation manday list selected values which is not cancelled
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>manday list</returns>
        Task<IEnumerable<QuotationManday>> GetQuotationManDay(int bookingId);
        /// <summary>
        /// Get QC list with leave details based on location and profile. and schedule inserted list too for QC
        /// </summary>
        /// <param name="location"></param>
        /// <param name="serviceDates"></param>
        /// <returns>QC list with schedule QC lit</returns>
        IQueryable<HrStaff> GetQCList();
        /// <summary>
        /// get QC list active records by booking id
        /// </summary>
        /// <returns>get QC list</returns>
        Task<List<SchScheduleQc>> GetQCDetails(int bookingId);

        //Task<List<SchScheduleQc>> GetQCStaffList(DateTime fromdate, DateTime todate, int bookingId);

        //Task<List<SchScheduleC>> GetCSStaffList(DateTime fromdate, DateTime todate, int bookingId);

        Task<List<PoTransactionDetails>> GetBookingPo(List<int> poTransactionIds);

        Task<IEnumerable<QuotationManday>> GetQuotationManDayList(List<int> bookingId);

        Task<List<ScheduleStaffItem>> GetQCBookingDetails(List<int> bookingIds);

        Task<List<ScheduleStaffItem>> GetCSBookingDetails(List<int> bookingIds);

        Task<List<int>> GetQCAllocatedBookings(IEnumerable<int> staffIDs, IEnumerable<int> bookingIds);

        Task<IEnumerable<QCStaffInfo>> GetQCListByLocationForForecast(IEnumerable<int> location, IEnumerable<DateTime> serviceDates);

        Task<List<ScheduleStaffItem>> GetQCBookingDetails(List<int> bookingIds, DateTime todayDate);

        IQueryable<HrStaffProfile> GetQcDataSource();

        Task<List<ScheduleQuotationManDay>> GetQuotationManDay(List<int> bookingIdList, DateTime startDate, DateTime endDate);

        //Fetch the county for the zone
        Task<List<int>> GetCountyByZone(IEnumerable<int> zoneIds);

        Task<List<int>> GetQCBookingbyDate(int qcId, DateTime fromdate, DateTime todate);

        Task<List<QCBookings>> GetQCBookingbyServiceDate(int qcId, DateTime fromdate, DateTime todate);

        Task<IEnumerable<ActualManDayDateCount>> GetActualQcCountOnDate(DateTime fromDate, DateTime toDate, List<int> locationList);

        Task<IEnumerable<StaffLeavesDate>> GetQCStaffLeaves(IEnumerable<int> location);

        Task<List<StaffScheduleRepo>> GetCSListbyServiceDate(DateTime fromdate, DateTime todate, int bookingId);

        Task<List<StaffScheduleQcRepo>> GetQcListbyServiceDate(DateTime fromdate, DateTime todate, int bookingId);

        Task<IEnumerable<QcActualManDayRepo>> GetQCActualManDayByServiceDates(DateTime fromDate, DateTime toDate, int bookingId);

        Task<List<ActualManday>> GetQcManDayData(List<int> qcIdList, List<DateTime> serviceDateList);

        Task<List<QuotationManday>> GetQuotationManDays(IQueryable<int> bookingIds);

        Task<List<ScheduleQuotationManDay>> GetQuotationManDaybyBookingQuery(IQueryable<int> bookingIdList, DateTime startDate, DateTime endDate);
        Task<List<ScheduleStaffItem>> GetQCBookingDetailsByBookingQuery(IQueryable<int> bookingIds);
        Task<List<ScheduleStaffItem>> GetCSBookingDetailsByBookingQuery(IQueryable<int> bookingIds);
        Task<List<SchScheduleQc>> GetQCBookingVisibleDetails(List<int> bookingIds);
        Task<int?> GetFBEntityId(int entityId);
        Task<List<string>> GetQCNameList(int bookingId);
        Task<List<QcAutoExpense>> GetQCAutoTravelExpenseList(int inspectionId);
        //Task<List<EcAutQcExpense>> GetQCAutoExpenses(List<int?> QcIdList, DateTime startDate, DateTime endDate);
        Task<List<EcAutTravelTariff>> GetTravelTariffData(DateTime startDate, DateTime endDate);
        Task<List<EcFoodAllowance>> GetFoodAllowanceData(DateTime startDate, DateTime endDate);
        Task<int?> GetInspectionLocation(int inspectionId);
        Task<List<EcAutQcTravelExpense>> GetQCAutoTravelExpenses(List<int?> QcIdList, DateTime startDate, DateTime endDate);
        Task<List<EcAutQcFoodExpense>> GetQCAutoFoodExpenses(List<int?> QcIdList, DateTime startDate, DateTime endDate);
        Task<List<EcAutQcFoodExpense>> GetQCAutoFoodExpensesByInspectionId(int? inspectionId);
        Task<List<EcAutQcTravelExpense>> GetQCAutoTravelExpensesByInspectionId(int? inspectionId);

        Task<List<ActualManday>> GetSchuduleQcs(IEnumerable<int> qcIdList, DateTime fromDate, DateTime toDate);

        Task<List<ScheduleProductData>> GetProductPODetails(int bookingId);

        Task<List<SchedulePOData>> GetPODetails(int bookingId);

        Task<List<ScheduleQcCustomerFactory>> GetQCBookingCustomerFactory(IEnumerable<int> qcIds);

        Task<List<BookingCsItem>> GetBookingCSItemList(List<int> bookingIds);

        Task<List<ScheduleStaffItem>> GetCSBookingDetails(List<int> bookingIds, DateTime scheduleDate);
        Task<List<EcExpencesClaim>> GetAutoQcExpenseListByQcList(List<int?> qcList, int? inspectionId);
    }
}
