using DTO.CommonClass;
using DTO.HumanResource;
using DTO.Report;
using DTO.Schedule;
using DTO.UserAccount;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IHumanResourceRepository : IRepository
    {
        /// <summary>
        /// get all departments
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrDepartment> GetDepartments();

        /// <summary>
        /// get all positions
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<HrPosition>> GetPositions();

        /// <summary>
        /// Get staff List
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrStaff> GetStaffList();

        /// <summary>
        /// Get staff by locations
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<HrStaff>> GetStaffListByLocations(IEnumerable<int> ids);

        Task<List<StaffInfo>> GetInternalStaffListByLocationList(IEnumerable<int> ids, int employeeTypeId);

        /// <summary>
        /// Get employee types
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<HrEmployeeType>> GetEmployeeTypes();

        /// <summary>
        /// Get qualifications
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrQualification> GetQualifications();

        /// <summary>
        /// get profiles
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrProfile> GetProfiles();

        /// <summary>
        /// get staff gender
        /// </summary>
        /// <returns></returns>
        Task<HrStaff> GetUserGender(int staffid);

        /// <summary>
        /// get Market segments
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefMarketSegment> GetMarketSegments();

        /// <summary>
        /// Get product categories
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefProductCategory> GetProductCategories();

        /// <summary>
        /// Get expertises
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RefExpertise>> GetExpertises();

        /// <summary>
        /// Get file types
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrFileType> GetFileTypes();

        /// <summary>
        /// Get staff details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        (HrStaff, bool) GetStaffDetails(int id);

        /// <summary>
        ///  Get Staff Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        HrStaff GetStaffAsync(int id);

        /// <summary>
        /// Get staff info
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<HrStaff> GetStaffByUserId(int userId);


        /// <summary>
        /// Remove staff
        /// </summary>
        /// <param name="id"></param>
        /// <param name="leavedate"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        StaffDeleteResult RemoveStaff(StaffDeleteRequest request);

        /// <summary>
        /// Get leave By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HrLeave> GetLeaveById(int id);


        /// <summary>
        /// get Leave data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<(int, IEnumerable<HrLeave>, IDictionary<HrLeaveStatus, int>)> GetLeaveData(LeaveSummaryDataRequest request);


        /// <summary>
        /// Add Staff
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddStaff(HrStaff entity);

        /// <summary>
        /// Save
        /// </summary>
        void Save(HrStaff entity);

        /// <summary>
        /// Get attached files
        /// </summary>
        /// <param name="idStaff"></param>
        /// <returns></returns>
        IEnumerable<HrAttachment> GetAttachedFiles(int idStaff);

        /// <summary>
        /// Save Attached files
        /// </summary>
        /// <param name=""></param>
        void SaveAttachedFiles(IEnumerable<HrAttachment> _entities);

        /// <summary>
        /// Getfile by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        HrAttachment GetFileAsync(int id);

        /// <summary>
        /// Get All entities
        /// </summary>
        /// <returns></returns>
        IEnumerable<ApEntity> GetEntities();

        // Get holidaylist 
        IEnumerable<HrHoliday> GetHolidays(int year, int countryId, int? locationId);

        /// <summary>
        /// Get Holiday 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HrHoliday> GetHoliday(int id);


        /// <summary>
        /// EditHolidayList
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityList"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> EditHolidayList(HrHoliday entity, IEnumerable<HrHoliday> entityList);


        /// <summary>
        /// Edit / Save
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> EditHoliday(HrHoliday entity);

        /// <summary>
        /// Add Holiday
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> AddHoliday(IEnumerable<HrHoliday> entityList);

        /// <summary>
        /// Delete holiday
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteHoliday(int id);


        /// <summary>
        /// removeHolidayTypes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> removeHolidayTypes(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrHolidayDayType> GetHolidayDayTypeList();

        /// <summary>
        /// GetOfficesByStaff
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<HrOfficeControl>> GetOfficesByStaff(int id);

        /// <summary>
        /// UpdateOfficesControl
        /// </summary>
        /// <param name="officeList"></param>
        /// <returns></returns>
        Task UpdateOfficesControl(IEnumerable<HrOfficeControl> officeList, int staffId);

        /// <summary>
        /// Get Booking rule
        /// </summary>
        /// <param name=""></param>
        /// <returns>int</returns>
        Task<List<HrHoliday>> GetHolidaysByRange(DateTime startdate, DateTime enddate, int countryId, int locationId);

        /// <summary>
        ///  Get staffs  By parent
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<IEnumerable<HrStaff>> GetstaffsByParentId(int parentId, bool includeCurrent);

        /// <summary>
        /// Get leave types
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<HrLeaveType>> GetLeaveTypes();

        /// <summary>
        ///  Get statuses
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<HrLeaveStatus>> GetLeaveStatuses();

        /// <summary>
        /// Get Auditor details
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<List<HrStaff>> GetAuditors();


        /// <summary>
        /// Get CS details
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<List<HrStaff>> GetAllCSByLocationCusId(int officeid,int customerid = 0);

        /// <summary>
        /// Get Photo
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<HrPhoto> GetPhoto(int staffId);
        /// <summary>
        /// Get Hr Profiles
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrProfile> GetHrProfiles();
        /// <summary>
        /// Get Hr Profiles
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrStaffProfile> GetHrStaffProfiles();

        /// <summary>
        /// Get staff list by role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<HrStaff>> GetStaffListByRole(int roleId, int locationId);

        /// <summary>
        /// Get STaff By Office And Profile
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        Task<IEnumerable<HrStaff>> GetStaffByOfficeIdAndProfile(List<int> lstprofileId);

        /// <summary>
        /// Get staff list by ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<HrStaff>> GetStaffListByIds(List<int> staffids);


        /// <summary>
        /// To get only staff details by staff id
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<HrStaff> GetStaffInfo(int staffId);

        /// <summary>
        /// get staff list by user id list
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<List<HrStaff>> GetStaffListByUserIds(List<int> userIds);

        /// <summary>
        /// get staff ids with qc profile
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="includeCurrent"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        Task<IEnumerable<HrStaff>> GetstaffsByParentIdwithQCProfile(int parentId, bool includeCurrent, int profileId);

        /// <summary>
        /// Get all staffs active+inactive
        /// </summary>
        /// <returns></returns>
        IQueryable<HrStaff> GetAllStaffList();
        /// <summary>
        /// Get Staff Details by id
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<StaffBaseData> GetStaffDataById(int staffId);

        /// <summary>
        /// get profile list
        /// </summary>
        /// <returns>get profile list</returns>
        Task<IEnumerable<HrProfile>> GetProfileList();

        /// <summary>
        /// get leave list
        /// </summary>
        /// <returns>get leave list</returns>
        Task<List<LeaveInfo>> GetStaffWithLeave(DateTime date, int locationId, int zoneid);

        /// <summary>
        /// get staff list
        /// </summary>
        /// <returns>staff name and id</returns>
        Task<List<CommonDataSource>> GetStaffList(List<int> userIds);

        /// <summary>
        /// get customer contact list
        /// </summary>
        /// <returns>staff name and id</returns>
        Task<List<CommonDataSource>> GetCusContactList(List<int> userIds);

        /// <summary>
        /// get sup contact list
        /// </summary>
        /// <returns>staff name and id</returns>
        Task<List<CommonDataSource>> GetSupContactList(List<int> userIds);

        /// <summary>
        /// get factory contact list
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetFactContactList(List<int> userIds);

        /// <summary>
        /// get dept list
        /// </summary>
        /// <param name="deptIdList"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetDepartmentList(List<int> deptIdList);

        IQueryable<HrStaff> GetStaffDataSource();

        Task<IEnumerable<int>> GetStaffIdsByProfileAndLocation(IEnumerable<int> locationIds);

        Task<IEnumerable<CommonDataSource>> GetStaffNameList();

        Task<int> GetUserIdByStaff(int staffId);

        Task<List<ItUserMaster>> GetUserIdByStaffIds(IEnumerable<int> staffIds);

        IQueryable<int> GetStaffIdsByProfileAndParentStaff(int parentId, int profileId);

        Task<List<CommonDataSource>> GetStaffStatusList();

        Task<List<CommonDataSource>> GetBandList();

        Task<List<CommonDataSource>> GetSocialInsuranceTypeList();

        IQueryable<RefCity> GetHukoLocationList();

        //get User list by active
        IQueryable<CommonDataSource> GetUserDataSource();

        Task<ItUserMaster> GetUserDetails(int staffId);

        /// <summary>
        /// Get the staff entity data
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<StaffEntity> GetStaffLocationEntity(int staffId);

        Task<bool> CheckEmailExists(string email, int userId);


        /// <summary>
        /// Get emails by positions and offices
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="offices"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetEmailsByPositionsAndOffices(IEnumerable<int> positions, IEnumerable<int> offices,IEnumerable<int> countryIds);

        Task<HrStaff> GetStaffByStaffId(int staffId);

        Task<List<CommonDataSource>> GetKAMStaff(List<int> staffIds);
        Task<List<int>> GetDepartmentIdsByName(string departname);
        Task<List<CommonDataSource>> GetStaffByDepartmentIds(List<int?> departmentIds);
        IQueryable<CommonDataSource> GetStaffUserDataSource();

        Task<List<int>> GetFBQCIdList(int bookingId);
        Task<List<string>> GetQCNameList(List<int> fbQCIds);
        Task<List<int>> GetFBCSIds(int bookingId);
        Task<List<string>> GetCSNameList(List<int> reviewerIds);
        Task<HrOutSourceCompany> GetHROutSourceCompanyById(int id);

        Task<bool> GetHROutSourceCompanyByName(SaveHROutSourceRequest request);
        IQueryable<HrOutSourceCompany> GetHROutSourceCompanyQuery();

        IQueryable<HrStaff> GetHRStaffQuery();

        Task<CommonDataSource> GetHROutSourceCompanyByUserId(int userId);

        Task<List<CommonDataSource>> GetStaffByHROutSourceCompany(List<int> companyIds);
        IQueryable<UserAccountSearchData> GetStaffData();

        /// <summary>
        /// get inspection occupancy query
        /// </summary>
        /// <returns></returns>
        IQueryable<InspectionOccupancyRepoItem> GetInspectionOccupanceQuery();

        /// <summary>
        /// get staff leaves
        /// </summary>
        /// <param name="staffIdList"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        Task<List<HrLeave>> GetStaffLeavesByStaffIdAndDateRange(IEnumerable<int> staffIdList, DateTime fromDate, DateTime toDate);

        Task<List<HrAttachment>> GetHrAttachmentList();
        Task<List<HrPhoto>> GetHrPhotoList();
        Task<List<HrPayrollCompany>> GetHRPayrollCompanyQuery();
        Task<int> GetHrStaffEntityId(int staffId);
        Task<List<StaffInfo>> GetInternalStaffListByLocationsAndTypes(IEnumerable<int> ids);
        Task<HrStaff> GetStaffDetailsByIdAndEntityId(int staffId, int entityId);
    }
}
