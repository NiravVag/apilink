using Contracts.Repositories;
using DTO.CommonClass;
using DTO.HumanResource;
using DTO.References;
using DTO.Report;
using DTO.Schedule;
using DTO.UserAccount;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class HumanResourceRespository : Repository, IHumanResourceRepository
    {

        public HumanResourceRespository(API_DBContext context) : base(context)
        {
        }


        public IEnumerable<HrDepartment> GetDepartments()
        {
            return _context.HrDepartments.Where(x => x.Active).OrderBy(x => x.DepartmentName);
        }

        public async Task<IEnumerable<HrEmployeeType>> GetEmployeeTypes()
        {
            return await _context.HrEmployeeTypes.OrderBy(x => x.EmployeeTypeName).ToListAsync();
        }

        public async Task<IEnumerable<HrPosition>> GetPositions()
        {
            return await _context.HrPositions.Where(x => x.Active).OrderBy(x => x.PositionName).ToListAsync();
        }

        public IEnumerable<HrStaff> GetStaffList()
        {
            return _context.HrStaffs
                        .Include(x => x.NationalityCountry)
                        .Include(x => x.Department)
                        .Include(x => x.Location)
                        .Include(x => x.Position)
                        .Include(x => x.EmployeeType)
                        .Include(x => x.HrStaffProfiles)
                        .Include(x => x.ItUserMasters)
                        .Where(x => x.Active.Value)
                .OrderBy(x => x.PersonName);
        }
        public IQueryable<HrStaff> GetAllStaffList()
        {
            return _context.HrStaffs
                .OrderByDescending(x => x.Active.Value)
                        .ThenBy(x => x.PersonName)
                        .Include(x => x.Status)
                        .Include(x => x.NationalityCountry)
                        .Include(x => x.Department)
                        .Include(x => x.Location)
                        .Include(x => x.Position)
                        .Include(x => x.EmployeeType)
                        .Include(x => x.HrStaffProfiles)
                        .Include(x => x.ItUserMasters);
        }

        public IEnumerable<HrProfile> GetHrProfiles()
        {
            return _context.HrProfiles
                        .Where(x => x.ProfileName == "KAM")
                .OrderBy(x => x.ProfileName);
        }

        public IEnumerable<HrStaffProfile> GetHrStaffProfiles()
        {
            return _context.HrStaffProfiles;
        }


        public async Task<IEnumerable<HrStaff>> GetStaffListByLocations(IEnumerable<int> ids)
        {
            return await _context.HrStaffs
            .Include(x => x.NationalityCountry)
            .Include(x => x.Department)
            .Include(x => x.Location)
            .Include(x => x.Position)
            .Include(x => x.EmployeeType)
            .Where(x => x.LocationId != null && ids.Contains(x.LocationId.Value))
            .ToListAsync();
        }

        /// <summary>
        /// Get the internal staff list by locations
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="employeeTypeId"></param>
        /// <returns></returns>
        public async Task<List<StaffInfo>> GetInternalStaffListByLocationList(IEnumerable<int> ids, int employeeTypeId)
        {
            return await _context.HrStaffs
            .Where(x => x.Active.HasValue && x.Active.Value && x.EmployeeTypeId == employeeTypeId
                    && x.LocationId != null && ids.Contains(x.LocationId.Value)).
             Select(x => new StaffInfo()
             {
                 Id = x.Id,
                 CurrencyId = x.PayrollCurrencyId,
                 CurrencyName = x.PayrollCurrency.CurrencyName,
                 LocationId = x.LocationId,
                 LocationName = x.Location.LocationName,
                 StaffName = x.PersonName,
                 CountryId = x.NationalityCountryId.Value,
                 Email = x.CompanyEmail,
                 UserTypeId = x.ItUserMasters.Where(x => x.Active).Select(x => x.UserTypeId).FirstOrDefault()
             })
            .ToListAsync();
        }

        public async Task<HrLeave> GetLeaveById(int id)
        {
            return await _context.HrLeaves
                .Include(x => x.StatusNavigation)
                .Include(x => x.LeaveType)
                .Include(x => x.Staff)
                .ThenInclude(x => x.ParentStaff)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(int, IEnumerable<HrLeave>, IDictionary<HrLeaveStatus, int>)> GetLeaveData(LeaveSummaryDataRequest request)
        {
            var data = _context.HrLeaves
                .Include(x => x.Staff)
                .ThenInclude(x => x.Location)
                .Include(x => x.Staff)
                .ThenInclude(x => x.Position)
                .Include(x => x.LeaveType)
                .Include(x => x.StatusNavigation)
                .Where(x => x.Status != null);


            if (request.StartDate != null)
                data = data.Where(x => x.DateBegin >= request.StartDate);

            if (request.EndDate != null)
                data = data.Where(x => x.DateEnd <= request.EndDate);

            if (request.Typeids != null && request.Typeids.Any())
                data = data.Where(x => request.Typeids.Contains(x.LeaveTypeId));

            if (!string.IsNullOrWhiteSpace(request.StaffName))
                data = data.Where(x => x.Staff != null && EF.Functions.Like(x.Staff.PersonName, $"%{request.StaffName.Trim()}%"));


            IQueryable<HrLeave> dataStaffs = null;

            if (request.Staffids != null)
                dataStaffs = data.Where(x => request.Staffids.Contains(x.StaffId));

            IQueryable<HrLeave> dataLocations = null;
            if (request.LocationIds != null)
                dataLocations = data.Where(x => x.Staff.LocationId != null && request.LocationIds.Contains(x.Staff.LocationId.Value));

            if (dataLocations != null)
                data = dataStaffs.Union(dataLocations);
            else
                data = dataStaffs;

            if (request.StatusIds != null && request.StatusIds.Any())
                data = data.Where(x => request.StatusIds.Contains(x.Status.Value));

            // var statusList = data.Where(x => x.StatusNavigation != null).GroupBy(x => x.StatusNavigation).ToDictionary(x => x.Key, y => y.Count());

            Dictionary<HrLeaveStatus, int> statusList = new Dictionary<HrLeaveStatus, int>();
            var statutIdList = data.Where(x => x.StatusNavigation != null).Select(x => x.Status.Value).Distinct();

            foreach (var item in statutIdList)
                statusList.Add(data.First(x => x.Status == item).StatusNavigation, data.Count(x => x.Status == item));

            int total = await data.CountAsync();

            if (total == 0)
                return (0, null, null);

            int skip = (request.Index - 1) * request.PageSize;

            var result = await data.Skip(skip).Take(request.PageSize).ToListAsync();

            return (total, result, statusList);
        }


        public StaffDeleteResult RemoveStaff(StaffDeleteRequest request)
        {
            var staff = _context.HrStaffs.FirstOrDefault(x => x.Id == request.Id);

            if (staff == null)
                return StaffDeleteResult.StaffNotFound;

            staff.Active = false;
            staff.LeaveDate = request.LeaveDate == null ? DateTime.Now : request.LeaveDate.ToDateTime();
            staff.Comments = request.Reason;
            staff.StatusId = request.StatusId;

            _context.SaveChanges();

            return StaffDeleteResult.Success;
        }

        public IEnumerable<HrQualification> GetQualifications()
        {
            return _context.HrQualifications.Where(x => x.Active).OrderBy(x => x.QualificationName);
        }

        public IEnumerable<HrProfile> GetProfiles()
        {
            return _context.HrProfiles.Where(x => x.Active).OrderBy(x => x.ProfileName);
        }

        public IEnumerable<RefMarketSegment> GetMarketSegments()
        {
            return _context.RefMarketSegments.Where(x => x.Active).OrderBy(x => x.Name);
        }

        public IEnumerable<RefProductCategory> GetProductCategories()
        {
            return _context.RefProductCategories.Where(x => x.Active).OrderBy(x => x.Name);
        }

        public async Task<IEnumerable<RefExpertise>> GetExpertises()
        {
            return await _context.RefExpertises.Where(x => x.Active).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }

        public IEnumerable<HrFileType> GetFileTypes()
        {
            return _context.HrFileTypes.Where(x => x.Active).OrderBy(x => x.FileTypeName);
        }

        public async Task<HrPhoto> GetPhoto(int staffId)
        {
            return await _context.HrPhotos.FirstOrDefaultAsync(x => x.StaffId == staffId && x.Active);
        }

        public async Task<HrStaff> GetStaffInfo(int staffId)
        {
            return await _context.HrStaffs

                .FirstOrDefaultAsync(x => x.Id == staffId);
        }


        public (HrStaff, bool) GetStaffDetails(int id)
        {
            var item = _context.HrStaffs
                .Include(x => x.Status)
                    .Include(x => x.HrAttachments.Where(x => x.Active))
                    .Include(x => x.HrPhotos.Where(x => x.Active))
                    .ThenInclude(x => x.User)
                    .Include(x => x.HrStaffExpertises)
                    .Include(x => x.HrStaffMarketSegments)
                    .Include(x => x.HrStaffOpCountries)
                    .Include(x => x.HrStaffProductCategories)
                    .Include(x => x.HrStaffProfiles)
                    .Include(x => x.HrRenews)
                    .Include(x => x.ItUserMasters)
                    .Include(x => x.HrStaffHistories)
                    .Include(x => x.HrStaffTrainings)
                    .Include(x => x.HomeCity)
                    .ThenInclude(x => x.Province)
                    .Include(x => x.CurrentCity)
                    .ThenInclude(x => x.Province)
                    .Include(x => x.Department)
                    .Include(x => x.HrEntityMaps)
                    .Include(x => x.HrStaffServices)
                    .Include(x => x.HrStaffEntityServiceMaps)
                      .ThenInclude(x => x.Entity)
                    .Include(x => x.HrStaffEntityServiceMaps)
                      .ThenInclude(x => x.Service)
                      .IgnoreQueryFilters()
                    .FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                bool hasPicture = item.HrPhotos.Any();
                return (item, hasPicture);
            }

            return (null, false);

        }

        public async Task<HrStaff> GetStaffByUserId(int userId)
        {
            return await _context.HrStaffs
                .Include(x => x.Location)
                .Include(x => x.PayrollCurrency)
                .Include(x => x.ItUserMasters)
                .FirstOrDefaultAsync(x => x.ItUserMasters.Any(y => y.Id == userId));
        }


        public async Task<HrStaff> GetStaffByStaffId(int staffId)
        {
            return await _context.HrStaffs
                .Include(x => x.Location)
                .Include(x => x.PayrollCurrency)
                .Include(x => x.ItUserMasters)
                .FirstOrDefaultAsync(x => x.Id == staffId);
        }

        public async Task<IEnumerable<HrStaff>> GetStaffByOfficeIdAndProfile(List<int> lstprofileId)
        {
            return await _context.HrStaffs
                  .Include(x => x.HrStaffProfiles)
                .Where(x => x.Active != null && x.Active.Value && x.HrStaffProfiles.Any(y => lstprofileId.Contains(y.ProfileId)))
                .ToListAsync();
        }


        public HrStaff GetStaffAsync(int id)
        {
            return _context.HrStaffs
                .Include(x => x.ParentStaff)
                .ThenInclude(x => x.ItUserMasters)
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<HrStaff>> GetstaffsByParentId(int parentId, bool includeCurrent)
        {
            if (includeCurrent)
                return await _context.HrStaffs.Where(x => x.Active != null && x.Active.Value && x.ParentStaffId == parentId || x.ManagerId == parentId || x.Id == parentId).ToListAsync();

            return await _context.HrStaffs.Where(x => x.Active != null && x.Active.Value && x.ParentStaffId == parentId || x.ManagerId == parentId).ToListAsync();
        }

        public async Task<IEnumerable<HrStaff>> GetstaffsByParentIdwithQCProfile(int parentId, bool includeCurrent, int profileId)
        {
            if (includeCurrent)
                return await _context.HrStaffs.
                    Include(x => x.HrStaffProfiles).
                    Where(x => x.ParentStaffId == parentId && x.Active != null && x.Active.Value
                    || x.ManagerId == parentId || x.Id == parentId
                    && x.HrStaffProfiles.Any(y => y.ProfileId == profileId)
                    ).ToListAsync();

            return await _context.HrStaffs.
                 Include(x => x.HrStaffProfiles).
                Where(x => x.ParentStaffId == parentId
            || x.ManagerId == parentId
             && x.HrStaffProfiles.Any(y => y.ProfileId == profileId)

            ).ToListAsync();
        }

        public async Task<IEnumerable<HrStaff>> GetStaffListByRole(int roleId, int locationId)
        {
            return await _context.ItUserMasters
                .Include(x => x.Staff)
                 .ThenInclude(x => x.ItUserMasters)
                 .Include(x => x.Staff)
                .ThenInclude(x => x.HrOfficeControls)
                .Where(x => x.ItUserRoles != null && x.Staff.Active != null && x.Staff.Active.Value
                            && x.ItUserRoles.Any(y => y.RoleId == roleId)
                            && x.Staff.HrOfficeControls.Any(y => y.LocationId == locationId))
                .Select(x => x.Staff)
                .ToListAsync();
        }

        public int AddStaff(HrStaff entity)
        {
            _context.HrStaffs.Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public void Save(HrStaff entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<HrAttachment> GetAttachedFiles(int idStaff)
        {
            return _context.HrAttachments.Where(x => x.StaffId == idStaff && x.Active);
        }

        public void SaveAttachedFiles(IEnumerable<HrAttachment> _entities)
        {
            foreach (var entity in _entities)
                _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public HrAttachment GetFileAsync(int id)
        {
            return _context.HrAttachments.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<ApEntity> GetEntities()
        {
            return _context.ApEntities.Where(x => x.Active);
        }



        public IEnumerable<HrHoliday> GetHolidays(int year, int countryId, int? locationId)
        {
            var data = _context.HrHolidays
                .Include(x => x.Country)
                .Include(x => x.Location) as IQueryable<HrHoliday>;

            if (locationId != null && locationId.Value > 0)
                data = data.Where(x => x.LocationId == locationId);

            data = data.Where(x => x.CountryId == countryId && ((x.StartDate != null && x.StartDate.Value.Year == year)
            || (x.EndDate != null && x.EndDate.Value.Year == year)));

            return data;
        }


        public async Task<List<HrHoliday>> GetHolidaysByRange(DateTime startdate, DateTime enddate, int countryId, int locationId)
        {
            var data = await _context.HrHolidays
                .Where(x => x.LocationId == locationId && x.CountryId == countryId && ((x.StartDate != null && x.StartDate.Value >= startdate)
            && (x.EndDate != null && x.EndDate.Value <= enddate))).ToListAsync();
            return data;
        }

        public IEnumerable<HrHolidayDayType> GetHolidayDayTypeList()
        {
            return _context.HrHolidayDayTypes;
        }

        public Task<HrHoliday> GetHoliday(int id)
        {
            return _context.HrHolidays.FirstOrDefaultAsync(x => x.Id == id);
        }



        public async Task<IEnumerable<int>> EditHolidayList(HrHoliday entity, IEnumerable<HrHoliday> entityList)
        {
            if (entity.HolidayId == null)
                entity.HolidayId = await _context.HrHolidays.MaxAsync(x => x.HolidayId);
            if (entity.HolidayId == null)
                entity.HolidayId = 1;

            // get all holidayTypes 
            var lst = _context.HrHolidays.Where(x => x.HolidayId == entity.HolidayId.Value);

            if (entity.StartDate != null)
                lst = lst.Where(x => x.StartDate != null && x.StartDate.Value >= entity.StartDate.Value);

            if (entity.EndDate != null)
                lst = lst.Where(x => x.EndDate != null && x.EndDate.Value >= entity.EndDate.Value);

            if (lst != null)
                _context.HrHolidays.RemoveRange(lst);

            foreach (var holiday in entityList)
                holiday.HolidayId = entity.HolidayId.Value;

            await _context.HrHolidays.AddRangeAsync(entityList);

            await _context.SaveChangesAsync();

            return entityList.Select(x => x.Id);
        }

        public async Task<int> EditHoliday(HrHoliday entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity.Id;
        }


        public async Task<IEnumerable<int>> AddHoliday(IEnumerable<HrHoliday> entityList)
        {
            int? holidayId = await _context.HrHolidays.MaxAsync(x => x.HolidayId);

            if (holidayId == null)
                holidayId = 1;
            else
                holidayId = holidayId.Value + 1;

            foreach (var item in entityList)
                item.HolidayId = holidayId;

            await _context.HrHolidays.AddRangeAsync(entityList);

            await _context.SaveChangesAsync();

            return entityList.Select(x => x.Id);
        }

        public async Task<bool> DeleteHoliday(int id)
        {
            var entity = await _context.HrHolidays.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<int>> removeHolidayTypes(int id)
        {
            var entity = await _context.HrHolidays.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return null;

            if (entity.RecurrenceType == (int)RecurrenceType.Manually || entity.HolidayId == null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

                return new List<int> { entity.Id };
            }
            else
            {
                var lst = _context.HrHolidays.Where(x => x.HolidayId == entity.HolidayId.Value && ((x.StartDate != null && x.StartDate.Value > DateTime.Now) || (x.EndDate != null && x.EndDate.Value > DateTime.Now)));
                var ids = await lst.Select(x => x.Id).ToArrayAsync();

                if (lst != null)
                {
                    _context.HrHolidays.RemoveRange(lst);
                    await _context.SaveChangesAsync();
                    return ids;
                }

                return null;
            }

        }

        public async Task<IEnumerable<HrOfficeControl>> GetOfficesByStaff(int id)
        {
            return await _context.HrOfficeControls.Include(x => x.Location).Where(x => x.StaffId == id).ToListAsync();
        }

        public async Task UpdateOfficesControl(IEnumerable<HrOfficeControl> officeList, int staffId)
        {
            var data = await _context.HrOfficeControls.Where(x => x.StaffId == staffId).ToListAsync();

            _context.RemoveRange(data);

            if (officeList != null)
                await _context.AddRangeAsync(officeList);

            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<HrLeaveType>> GetLeaveTypes()
        {
            return await _context.HrLeaveTypes.Where(x => x.Active).OrderBy(x => x.Description).ToListAsync();
        }


        public async Task<IEnumerable<HrLeaveStatus>> GetLeaveStatuses()
        {
            return await _context.HrLeaveStatuses.ToListAsync();
        }

        public Task<List<HrStaff>> GetAuditors()
        {
            return _context.HrStaffs
                .Include(x => x.HrStaffProfiles)
                .Where(x => x.Active != null && x.Active.Value && x.HrStaffProfiles.Any(y => y.ProfileId == (int)HRProfile.Auditor)).ToListAsync();
        }

        public Task<List<HrStaff>> GetAllCSByLocationCusId(int officeid, int customerid = 0)
        {
            var entity = _context.HrStaffs
                .Include(x => x.HrStaffProfiles)
                .Include(x => x.CuCsConfigurations)
                .Where(x => x.Active != null && x.Active.Value && x.HrStaffProfiles.Any(y => y.ProfileId == (int)HRProfile.CS));

            if (customerid != 0 && entity.Where(x => x.CuCsConfigurations.Any(y => y.CustomerId == customerid)).Count() > 0)
            {
                entity = entity.Where(x => x.CuCsConfigurations.Any(y => y.CustomerId == customerid));
            }
            else
            {
                entity = entity.Where(x => x.LocationId != null && x.LocationId.Value == officeid);
            }
            return entity.ToListAsync();
        }

        public Task<HrStaff> GetUserGender(int staffid)
        {
            return _context.HrStaffs
                .Include(x => x.HrPhotos.Where(y => y.Active))
                .Where(x => x.Id == staffid).FirstOrDefaultAsync();
        }

        public Task<List<HrStaff>> GetStaffListByIds(List<int> staffids)
        {
            return _context.HrStaffs.Where(x => x.Active.Value && staffids.Contains(x.Id)).ToListAsync();
        }

        public Task<List<HrStaff>> GetStaffListByUserIds(List<int> userIds)
        {
            return _context.ItUserMasters.Where(x => x.Active && userIds.Contains(x.Id)).Select(x => x.Staff).ToListAsync();
        }
        /// <summary>
        /// Get staff based data by id
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public async Task<StaffBaseData> GetStaffDataById(int staffId)
        {
            return await _context.HrStaffs.Select(x => new StaffBaseData
            {
                Id = x.Id,
                StaffName = x.PersonName,
                Email = x.CompanyEmail,
                HrStaffProfiles = x.HrStaffProfiles
            })

                .FirstOrDefaultAsync(x => x.Id == staffId);
        }

        //get hr profile list
        public async Task<IEnumerable<HrProfile>> GetProfileList()
        {
            return await _context.HrProfiles.Where(x => x.Active).OrderBy(x => x.ProfileName).ToListAsync();
        }

        //get the user list having leaves for a date
        public async Task<List<LeaveInfo>> GetStaffWithLeave(DateTime date, int locationId, int zoneid)
        {
            var qcStaffIds = _context.HrStaffProfiles.Where(x => x.ProfileId == (int)HRProfile.Inspector).Select(x => x.StaffId);

            var data = _context.HrLeaves.Where(x => x.Status != (int)Entities.Enums.LeaveStatus.Cancelled && x.Status != (int)Entities.Enums.LeaveStatus.Rejected);

            if (zoneid > 0)
            {
                data = data.Where(x => x.Staff.CurrentCounty.ZoneId.HasValue && x.Staff.CurrentCounty.ZoneId.Value == zoneid);
            }

            return await data.Where(x => x.Staff.IsForecastApplicable.Value &&
                        (x.DateBegin.Date == date || x.DateEnd.Date == date || (date > x.DateBegin.Date && date < x.DateEnd.Date)) &&
                        x.Staff.EmployeeTypeId == (int)StaffType.Permanent && x.Staff.LocationId == locationId && qcStaffIds.Contains(x.Staff.Id))
                .Select(x => new LeaveInfo
                {
                    StaffId = x.Staff.Id,
                    StaffName = x.Staff.PersonName,
                    LeaveTypeName = x.LeaveType.Description,
                    Remarks = x.Comments,
                    BeginDate = x.DateBegin,
                    EndDate = x.DateEnd,
                    LeaveStatus = x.StatusNavigation.Label
                }).AsNoTracking().ToListAsync();


        }

        //Get the staff name by user Id
        public async Task<List<CommonDataSource>> GetStaffList(List<int> userIds)
        {
            return await _context.ItUserMasters.Where(x => x.Active && userIds.Contains(x.Id)).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Staff.PersonName
            }).ToListAsync();
        }

        //Get the customer contact name by user Id
        public async Task<List<CommonDataSource>> GetCusContactList(List<int> userIds)
        {
            return await _context.ItUserMasters.Where(x => x.Active && userIds.Contains(x.Id)).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.CustomerContact.ContactName
            }).ToListAsync();
        }

        //Get the supplier contact name by user Id
        public async Task<List<CommonDataSource>> GetSupContactList(List<int> userIds)
        {
            return await _context.ItUserMasters.Where(x => x.Active && userIds.Contains(x.Id)).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.SupplierContact.ContactName
            }).ToListAsync();
        }

        //Get the factory contact name by user Id
        public async Task<List<CommonDataSource>> GetFactContactList(List<int> userIds)
        {
            return await _context.ItUserMasters.Where(x => x.Active && userIds.Contains(x.Id)).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.FactoryContact.ContactName
            }).ToListAsync();
        }

        //Get the department List by dept Id
        public async Task<List<CommonDataSource>> GetDepartmentList(List<int> deptIdList)
        {
            return await _context.HrDepartments.Where(x => x.Active && deptIdList.Contains(x.Id) && x.DeptParentId.HasValue).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.DepartmentName
            }).ToListAsync();
        }

        /// <summary>
        /// get staff list
        /// </summary>
        /// <returns></returns>
        public IQueryable<HrStaff> GetStaffDataSource()
        {
            return _context.HrStaffs.Include(x => x.Location).Where(x => x.Active.Value);
        }

        /// <summary>
        /// get staff id list by inspector role and location ids
        /// </summary>
        /// <param name="locationIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetStaffIdsByProfileAndLocation(IEnumerable<int> locationIds)
        {
            return await _context.HrStaffs.Where(x => x.Active.Value && x.HrStaffProfiles.Any(y => y.ProfileId == (int)HRProfile.Inspector)
            && locationIds.Contains(x.LocationId.GetValueOrDefault())).Select(x => x.Id).ToListAsync();
        }

        /// <summary>
        /// get hr staff name with id
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetStaffNameList()
        {
            return await _context.HrStaffs.Where(x => x.Active.Value)
                      .Select(x => new CommonDataSource { Id = x.Id, Name = x.PersonName }).ToListAsync();
        }

        /// <summary>
        /// Get the user id based on the staffid
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public async Task<int> GetUserIdByStaff(int staffId)
        {
            return await _context.ItUserMasters.Where(x => x.Active && x.StaffId == staffId).Select(x => x.Id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the user id s based on the staffids
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public async Task<List<ItUserMaster>> GetUserIdByStaffIds(IEnumerable<int> staffIds)
        {
            return await _context.ItUserMasters.Where(x => x.Active && x.StaffId.HasValue && staffIds.Contains(x.StaffId.Value)).ToListAsync();
        }

        /// <summary>
        /// Get staffids by parent staff(includes current staff) and profileid
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public IQueryable<int> GetStaffIdsByProfileAndParentStaff(int parentId, int profileId)
        {
            return _context.HrStaffs.
                    Where(x => (x.ParentStaffId == parentId && x.Active.Value || x.ManagerId == parentId || x.Id == parentId)
                        && x.HrStaffProfiles.Any(y => y.ProfileId == profileId)
                    ).Select(x => x.Id);
        }

        /// <summary>
        /// get staff status list
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetStaffStatusList()
        {
            return await _context.HrRefStatuses.Where(x => x.Active).OrderBy(x => x.Sort)
                .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get band list
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetBandList()
        {
            return await _context.HrRefBands.Where(x => x.Active).OrderBy(x => x.Sort)
                .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get social insurance type list
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetSocialInsuranceTypeList()
        {
            return await _context.HrRefSocialInsuranceTypes.Where(x => x.Active).OrderBy(x => x.Sort)
                .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get location list
        /// </summary>
        /// <returns></returns>
        public IQueryable<RefCity> GetHukoLocationList()
        {
            return _context.RefCities.Where(x => x.Active && x.Province.CountryId == (int)CountryEnum.China)
                .OrderBy(x => x.CityName);
        }
        //get User list by active
        public IQueryable<CommonDataSource> GetUserDataSource()
        {
            return _context.HrStaffs.Where(x => x.Active.HasValue).OrderBy(x => x.PersonName)
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.PersonName });
        }

        public async Task<ItUserMaster> GetUserDetails(int staffId)
        {
            return await _context.ItUserMasters.Where(x => x.Active && x.StaffId == staffId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the location entity mapped with the staff
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public async Task<StaffEntity> GetStaffLocationEntity(int staffId)
        {
            return await _context.HrStaffs.Where(x => x.Active.Value && x.Id == staffId).
                            Select(x => new StaffEntity()
                            {
                                staffId = x.Id,
                                locationId = x.LocationId,
                                entityId = x.Location.EntityId
                            }).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Check Staff exists by the email id
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CheckEmailExists(string email, int userId)
        {
            return await _context.HrStaffs.AnyAsync(x => x.Active.Value && x.CompanyEmail.ToLower().Trim() == email.ToLower().Trim() && x.Id != userId);
        }

        /// <summary>
        /// Get emails
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="offices"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetEmailsByPositionsAndOffices(IEnumerable<int> positions, IEnumerable<int> offices, IEnumerable<int> countryIds)
        {
            return await _context.HrStaffs
                .Where(x => x.Active.HasValue && x.Active.Value && x.PositionId != null && x.EmployeeTypeId == (int)EmployeeTypeEnum.Permanent
                            && x.LocationId != null
                            && positions.Contains(x.PositionId.Value)
                            && offices.Contains(x.LocationId.Value)
                            && x.HrStaffOpCountries.Any(y => countryIds.Contains(y.CountryId)))
                .Select(x => x.CompanyEmail).AsNoTracking().ToListAsync();
        }

        public IQueryable<CommonDataSource> GetStaffUserDataSource()
        {
            return _context.ItUserMasters.Where(x => x.Active && x.Staff.Active.HasValue && x.Staff.Active.Value).Select(x => new CommonDataSource()
            {
                Id = x.Id,
                Name = x.Staff.PersonName
            }).OrderBy(x => x.Name);
        }

        /// <summary>
        /// Get the HR outsource company by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HrOutSourceCompany> GetHROutSourceCompanyById(int id)
        {
            return await _context.HrOutSourceCompanies.FirstOrDefaultAsync(x => x.Id == id && x.Active.Value);
        }

        /// <summary>
        /// Get the HR outsource company by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> GetHROutSourceCompanyByName(SaveHROutSourceRequest request)
        {
            var isExists = false;
            if (request.Id == 0)
                isExists = await _context.HrOutSourceCompanies.AnyAsync(x => x.Name == request.Name);
            else if (request.Id > 0)
                isExists = await _context.HrOutSourceCompanies.AnyAsync(x => x.Name == request.Name && x.Id != request.Id);
            return isExists;
        }

        public IQueryable<HrOutSourceCompany> GetHROutSourceCompanyQuery()
        {
            return _context.HrOutSourceCompanies;
        }

        public IQueryable<HrStaff> GetHRStaffQuery()
        {
            return _context.HrStaffs;
        }

        public async Task<List<int>> GetFBQCIdList(int bookingId)
        {
            return await _context.FbReportQcdetails
                            .Where(x => x.Active.Value && x.FbReportDetail.InspectionId == bookingId)
                            .Select(x => x.QcId.Value).ToListAsync();
        }

        public async Task<List<string>> GetQCNameList(List<int> fbQCIds)
        {
            return await _context.ItUserMasters
                            .Where(x => x.FbUserId.HasValue && x.Active && fbQCIds.Contains(x.FbUserId.Value))
                            .Select(x => x.Staff.PersonName).AsNoTracking()
                            .ToListAsync();
        }

        public async Task<List<int>> GetFBCSIds(int bookingId)
        {
            return await _context.FbReportReviewers
                .Where(x => x.Active.Value && x.FbReport.InspectionId.Value == bookingId)
                .Select(x => x.ReviewerId.Value).ToListAsync();
        }

        public async Task<List<string>> GetCSNameList(List<int> reviewerIds)
        {
            return await _context.ItUserMasters
                .Where(x => x.FbUserId.HasValue && x.Active && reviewerIds.Contains(x.FbUserId.Value))
                .Select(x => x.Staff.PersonName).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Check Staff by StaffIDs
        /// </summary>
        /// <param name="staffIds"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetKAMStaff(List<int> staffIds)
        {
            return await _context.HrStaffs.Where(x => x.Active.Value && staffIds.Contains(x.Id)).OrderBy(x => x.PersonName)
                .Select(x => new CommonDataSource { Id = x.Id, Name = x.PersonName }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Department Ids By Name
        /// <param name="departname"></param>
        /// <returns></returns>
        public async Task<List<int>> GetDepartmentIdsByName(string departname)
        {
            return await _context.HrDepartments.Where(x => x.Active && EF.Functions.Like(x.DepartmentName.ToLower().Trim(), $"%{departname.ToLower().Trim()}%"))
                .Select(x => x.Id).ToListAsync();
        }
        /// <summary>
        /// Check Staff by departmentIds
        /// </summary>
        /// <param name="departmentIds"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetStaffByDepartmentIds(List<int?> departmentIds)
        {
            return await _context.HrStaffs.Where(x => x.Active.Value && departmentIds.Contains(x.DepartmentId)).OrderBy(x => x.PersonName)
                .Select(x => new CommonDataSource { Id = x.Id, Name = x.PersonName }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the hr outsource company by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CommonDataSource> GetHROutSourceCompanyByUserId(int userId)
        {
            return await _context.ItUserMasters.Where(x => x.Active && x.Id == userId && x.Staff.HroutSourceCompany.Active.Value)
                .Select(x => new CommonDataSource() { Id = x.Staff.HroutSourceCompany.Id, Name = x.Staff.HroutSourceCompany.Name })
                            .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the staff details by company ids
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetStaffByHROutSourceCompany(List<int> companyIds)
        {
            return await _context.HrStaffs.Where(x => x.Active.Value && companyIds.Contains(x.HroutSourceCompanyId.Value)).
                    Select(x => new CommonDataSource() { Id = x.Id, Name = x.PersonName }).ToListAsync();
        }

        public IQueryable<InspectionOccupancyRepoItem> GetInspectionOccupanceQuery()
        {
            return _context.HrStaffs.Where(x => x.HrStaffProfiles.Any(x => x.ProfileId == (int)HRProfile.Inspector))
                .Select(y => new InspectionOccupancyRepoItem()
                {
                    Id = y.Id,
                    Name = y.PersonName,
                    RegisnDate = y.LeaveDate,
                    OfficeCountryId = y.Location.City.Province.CountryId,
                    OfficeCountry = y.Location.City.Province.Country.CountryName,
                    EmployeeType = y.EmployeeType.EmployeeTypeName,
                    EmployeeTypeId = y.EmployeeTypeId,
                    JoinDate = y.JoinDate,
                    Office = y.Location.LocationName,
                    OfficeId = y.LocationId,
                    OutSourceCompany = y.HroutSourceCompany.Name,
                    OutSourceCompanyId = y.HroutSourceCompanyId
                });
        }

        public async Task<List<HrLeave>> GetStaffLeavesByStaffIdAndDateRange(IEnumerable<int> staffIdList, DateTime fromDate, DateTime toDate)
        {
            return await _context.HrLeaves.Where(x => staffIdList.Contains(x.StaffId) && x.DateBegin <= toDate && x.DateEnd >= fromDate).AsNoTracking().ToListAsync();
        }
        public IQueryable<UserAccountSearchData> GetStaffData()
        {
            return _context.HrStaffs.Where(x => x.Active.Value).
                            Select(x => new UserAccountSearchData()
                            {
                                Id = x.Id,
                                Name = x.PersonName,
                                Gender = x.Gender,
                                DepartmentName = x.Department.DepartmentName,
                                PositionName = x.Position.PositionName,
                                LocationName = x.Location.LocationName,
                                CountryName = x.NationalityCountry.CountryName,
                                EmployeeTypeId = x.EmployeeTypeId,
                                PersonName = x.PersonName,
                                NationalityCountryId = x.NationalityCountryId.GetValueOrDefault(),
                                HasAccount = x.ItUserMasters.Any()
                            });
        }


        public async Task<List<HrAttachment>> GetHrAttachmentList()
        {
            return await _context.HrAttachments.Where(x => x.Active).ToListAsync();
        }

        public async Task<List<HrPhoto>> GetHrPhotoList()
        {
            return await _context.HrPhotos.Where(x => x.Active).ToListAsync();
        }

        public async Task<List<HrPayrollCompany>> GetHRPayrollCompanyQuery()
        {
            return await _context.HrPayrollCompanies.Where(x => x.Active).ToListAsync();
        }

        public async Task<int> GetHrStaffEntityId(int staffId)
        {
            return await _context.HrStaffs.IgnoreQueryFilters().Where(x => x.Active.Value && x.Id == staffId).Select(x => x.CompanyId.GetValueOrDefault()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the internal staff list by locations and other than permanent employee
        /// </summary>
        /// <param name="LocationIds"></param>
        /// <param name="employeeTypeId"></param>
        /// <returns></returns>
        public async Task<List<StaffInfo>> GetInternalStaffListByLocationsAndTypes(IEnumerable<int> LocationIds)
        {
            return await _context.HrStaffs
            .Where(x => x.Active.HasValue && x.Active.Value && x.EmployeeTypeId != (int)EmployeeTypeEnum.Permanent
                    && x.LocationId != null && LocationIds.Contains(x.LocationId.Value)).
             Select(x => new StaffInfo()
             {
                 Id = x.Id,
                 CurrencyId = x.PayrollCurrencyId,
                 CurrencyName = x.PayrollCurrency.CurrencyName,
                 LocationId = x.LocationId,
                 LocationName = x.Location.LocationName,
                 StaffName = x.PersonName,
                 CountryId = x.NationalityCountryId.Value,
                 Email = x.CompanyEmail,
                 UserTypeId = x.ItUserMasters.Where(x => x.Active).Select(x => x.UserTypeId).FirstOrDefault()
             }).AsNoTracking()
            .ToListAsync();
        }

        public async Task<HrStaff> GetStaffDetailsByIdAndEntityId(int staffId, int entityId)
        {
            return await _context.HrStaffs.Include(x => x.ItUserMasters).IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == staffId && x.HrEntityMaps.Any(y => y.EntityId == entityId));
        }
    }
}
