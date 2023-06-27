using DTO;
using DTO.Audit;
using DTO.File;
using DTO.HumanResource;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTO.Common;
using Entities;
using DTO.CommonClass;

namespace Contracts.Managers
{
    public interface IHumanResourceManager
    {
        /// <summary>
        /// Get Staff Summary
        /// </summary>
        /// <returns></returns>
       Task<StaffSummaryResponse> GetStaffSummary();

        /// <summary>
        /// GetStaffData
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<StaffSearchResponse> GetStaffData(StaffSearchRequest request);


        /// <summary>
        /// Remove staff
        /// </summary>
        /// <param name="id"></param>
        /// <param name="leaveDate"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<StaffDeleteResponse> DeleteStaff(StaffDeleteRequest request);

        /// <summary>
        /// Get Edit Staff details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EditStaffResponse> GetEditStaff(int? id);

        /// <summary>
        /// Get Staff gender
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<StaffGenderResponse> GetGender();


        /// <summary>
        /// Save Staff
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveStaffResponse> SaveStaff(StaffDetails request);

        /// <summary>
        ///  Get sub departments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SubDepartmentResponse GetSubDepartments(int id);

        /// <summary>
        /// Get Photo async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HrPhotoResponse> GetPhotoAsync(int id);

        /// <summary>
        /// Get holiday data master 
        /// </summary>
        /// <returns></returns>
        HolidayMasterResponse GetHolidayDataMaster();


        /// <summary>
        /// Get staff by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<StaffInfo> GetStaffByUserId(int userId);

        /// <summary>
        /// Get staff by id
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<StaffInfo> GetStaffById(int staffId);

        /// <summary>
        /// Get Holidays by Year/ country
        /// </summary>
        /// <param name="year"></param>
        /// <param name="countryId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        Task<HolidayDetailsResponse> GetHolidayDetails(HolidayDetaisRequest request);

        /// <summary>
        /// Add or Edit Hoiday
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<EditHolidayResponse> AddOrEditHoliday(EditHolidayRequest request);

        /// <summary>
        /// Delete holiday
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EditHolidayResponse> DeleteHoliday(DeleteHolidayRequest request);

        /// <summary>
        /// Get offices control
        /// </summary>
        /// <returns></returns>
        Task<OfficesControlResponse> GetOfficesControl();

        /// <summary>
        /// GetOfficesControlByStaffId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OfficeControlStaffResponse> GetOfficesControlByStaffId(int id);

        /// <summary>
        /// SaveOfficeControl
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveOfficeControlResponse> SaveOfficeControl(SaveOfficeControlRequest request);

        /// <summary>
        ///  Get staff list
        /// </summary>
        /// <returns></returns>
        IEnumerable<StaffInfo> GetStaffList();


        /// <summary>
        ///  Get staff list
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<StaffInfo>> GetStaffListByParentId(int staffId);

        /// <summary>
        ///  Get Holiday date
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DateObject>> GetHolidaysByRange(DateTime startdate, DateTime enddate, int countryId, int locationId);

        /// <summary>
        /// Get staffs by parent
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<int>> GetStaffIdsByParent();

        /// <summary>
        /// get leave request
        /// </summary>
        /// <returns></returns>
        Task<LeaveResponse> GetLeaveRequest(int ? id);

        /// <summary>
        /// Save leaverequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveLeaveRequestResponse> SaveLeaveRequest(LeaveRequest request);

        /// <summary>
        /// Get leave summary
        /// </summary>
        /// <returns></returns>
        Task<LeaveSummaryResponse> GetLeaveSummary();

        /// <summary>
        /// Get Leave data summary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<LeaveSummaryDataResponse> GetLeaveDataSummary(LeaveSummaryRequest request);

        /// <summary>
        /// Get leave days
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<LeaveDaysResponse> GetLeaveDays(LeaveDaysRequest request);

        /// <summary>
        /// Get Auditor List
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>AuditorResponse</returns>
        Task<AuditorResponse> GetAuditors();

        /// <summary>
        /// Get Auditor List
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>Auditor</returns>
        Task<IEnumerable<CustomerCS>> GetAllCSByLocationCusId(int officeid, int customerid=0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<LeaveApproveEmail> SetLeaveStatus(int id, Entities.Enums.LeaveStatus statusId, string comment);

        /// <summary>
        /// Get staffs by locations
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<StaffInfo>> GetStaffListByLocations();

        

        Task<IEnumerable<StaffInfo>> GetInternalStaffListByLocationList(int typeId);

        StaffKAMProfileResponse GetHRStaffWithKAMProfiles();

        /// <summary>
        /// Update Email State
        /// </summary>
        /// <param name="id"></param>
        /// <param name="emailState"></param>
        /// <param name="isTask"></param>
        void UpdateEmailState(Guid id, int emailState, bool isTask);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        HrStaff GetBasicStaff(int id);

        /// <summary>
        /// Get staff details for booking , only name, email, phone number
        /// </summary>
        /// <param name="staffId">staff</param>
        /// <returns></returns>
        Task<BookingStaffInfo> GetStaffInfoByStaffId(int staffId);
        /// <summary>
        ///  Get Holiday date
        /// </summary>
        /// <param name="startdate">staff</param>
        /// <param name="enddate">staff</param>
        /// <param name="countryId">staff</param>
        /// <param name="locationId">staff</param>
        /// <returns></returns>
        Task<IEnumerable<DateTime>> GetHolidaysDateByRange(DateTime startdate, DateTime enddate, int countryId, int locationId);
        /// <summary>
        /// Get staff details by id
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<StaffBaseData> GetStaffByStaffId(int staffId);
      
        /// <summary>
        /// get the profile list
        /// </summary>
        /// <returns>profile list</returns>
        Task<HRProfileResponse> GetProfileList();

        /// <summary>
        /// get the department details
        /// </summary>
        /// <param name="deptIdList"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetDepartmentList(List<int> deptIdList);

        Task<DataSourceResponse> GetStaffDataSource(StaffDataSourceRequest request);

        Task<IEnumerable<int>> GetStaffIdsByProfileAndLocation(IEnumerable<int> locationIds);

        Task<IEnumerable<CommonDataSource>> GetStaffNameList();

        Task<DataSourceResponse> GetStaffStatusList();

        Task<DataSourceResponse> GetBandList();

        Task<DataSourceResponse> GetSocialInsuranceTypeList();

        Task<DataSourceResponse> GetHukoLocationList(CommonDataSourceRequest request);

        /// <summary>
        /// Get the staff entity data
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<StaffEntity> GetStaffLocationEntity(int staffId);

        /// <summary>
        /// Get all positions
        /// </summary>
        /// <returns></returns>
        Task<StaffSummaryResponse> GetPositions();

        /// <summary>
        /// Get emails 
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="offices"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetEmailsByPositionsAndOffices(IEnumerable<int> positions, IEnumerable<int> offices, IEnumerable<int> countryIds);


        /// <summary>
        /// get staff list by id office
        /// </summary>
        /// <param name="idOffice"></param>
        /// <returns></returns>
        Task<StaffSearchResponse> GetStaffsByOffice(int idOffice); 

        Task<StaffInfo> GetStaffDetailsByStaffId(int userId);

        /// <summary>
        /// Get Staff Data with User Id 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetStaffUserDataSource(UserDataSourceRequest request);

        /// <summary>
        /// Save the HR Outsource company
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveHROutSourceResponse> SaveHROutSourceCompany(SaveHROutSourceRequest request);

        Task<HROutSourceCompanyListResponse> GetHROutSourceCompanyList(HROutSourceCompanyRequest request);

        Task<HROutSourceCompanyListResponse> GetHROutSourceCompanyList();

        Task<HRStaffResponse> GetStaffListByOutSourceCompanyIds(List<int> companyIds);

        Task<DataSourceResponse> GetKAMStaff();
        Task<List<int>> GetDepartmentIdsByName(string departname);
        Task<DataSourceResponse> GetSalesIncharge(List<int?> departmentIds);
        Task<HRPayrollCompanyListResponse> GetHRPayrollCompanyList();
        Task<List<int>> GetFBQCIdList(int bookingId);
        Task<List<int>> GetFBCSIds(int bookingId);
    }
}
