using BI.Maps;
using BI.Utilities;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Audit;
using DTO.Common;
using DTO.CommonClass;
using DTO.File;
using DTO.HumanResource;
using DTO.MasterConfig;
using DTO.References;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BI
{
    public class HumanResourceManager : ApiCommonData, IHumanResourceManager
    {
        private IHumanResourceRepository _hrRepository = null;
        private ICacheManager _cache = null;
        private ILocationManager _locationManager = null;
        private IFileManager _fileManager = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly IConfiguration _configuration = null;
        private readonly IUserRepository _userRepo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ICSConfigManager _csmanager = null;
        private readonly IReferenceManager _referencemanager = null;
        private readonly HRMap _hrmap = null;
        private readonly LocationMap _locmap = null;
        private ITenantProvider _filterService = null;
        private readonly IUserConfigRepository _userConfigRepository = null;
        private readonly IHelper _helper = null;

        public HumanResourceManager(IHumanResourceRepository hrRepository, ICacheManager cache,
            ILocationManager locationManager, IFileManager fileManager, IOfficeLocationManager office,
            IConfiguration configuration, IUserRepository userRepo, IAPIUserContext applicationContext, ICSConfigManager csmanager,
            IReferenceManager referencemanager, ITenantProvider filterService, IUserConfigRepository userConfigRepository, IHelper helper)
        {
            _hrRepository = hrRepository;
            _cache = cache;
            _locationManager = locationManager;
            _fileManager = fileManager;
            _office = office;
            _configuration = configuration;
            _userRepo = userRepo;
            _ApplicationContext = applicationContext;
            _csmanager = csmanager;
            _referencemanager = referencemanager;
            _hrmap = new HRMap();
            _locmap = new LocationMap();
            _filterService = filterService;
            _userConfigRepository = userConfigRepository;
            _helper = helper;
        }

        public async Task<StaffSearchResponse> GetStaffData(StaffSearchRequest request)
        {
            if (request.Index == 0)
                request.Index = 1;

            var data = _hrRepository.GetAllStaffList();
            var response = new StaffSearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            var locIds = new List<int>();

            if (_ApplicationContext.LocationList != null && _ApplicationContext.RoleList.Contains((int)RoleEnum.HumanResource))
                locIds = _ApplicationContext.LocationList.ToList();


            var staffIds = new List<int>();

            //if no data selected , show default staff id.
            if (string.IsNullOrEmpty(request.EmployeeNumber) && string.IsNullOrEmpty(request.StaffName) && (
                !request.PositionValues.Any()) && (!request.OfficeValues.Any()) && (!request.CountryValues.Any())
                && (!request.DepartmentValues.Any()) && (request.EmployeeTypeValues == null || !request.EmployeeTypeValues.Any()))
            {
                staffIds.Add(_ApplicationContext.StaffId);

            }

            if (locIds != null && locIds.Any())
            {
                staffIds.Add(_ApplicationContext.StaffId);

                var staffList = await _hrRepository.GetStaffListByLocations(locIds);

                if (staffList != null && staffList.Any())
                    staffIds.AddRange(staffList.Select(x => x.Id).ToList());
            }

            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.Management))
            {
                staffIds.Add(_ApplicationContext.StaffId);

                var ids = await GetStaffIdsByParent();
                if (ids != null && ids.Any())
                    staffIds.AddRange(ids.ToList());
            }

            if (staffIds != null && staffIds.Count > 0)
                data = data.Where(x => staffIds.Any(y => y == x.Id));


            //if (request.IsLeft)
            //    data = data.Where(x => x.LeaveDate != null);
            //else
            //    data = data.Where(x => x.Active != null && x.Active.Value);

            if (!string.IsNullOrEmpty(request.StaffName?.Trim()))
            {
                var staffame = request.StaffName?.Trim().ToUpper() ?? "";
                data = data.Where(x => x.PersonName.ToUpper().Contains(staffame));
            }

            if (!string.IsNullOrEmpty(request.EmployeeNumber?.Trim()))
            {
                var employeenumbe = request.EmployeeNumber?.Trim() ?? "";
                data = data.Where(x => x.EmpNo == employeenumbe);
            }

            if (request.PositionValues != null && request.PositionValues.Any())
                data = data.Where(x => request.PositionValues.Select(z => z.Id).Contains(x.PositionId.GetValueOrDefault()));

            if (request.DepartmentValues != null && request.DepartmentValues.Any())
                data = data.Where(x => request.DepartmentValues.Select(z => z.Id).Contains(x.DepartmentId.GetValueOrDefault()));

            if (request.CountryValues != null && request.CountryValues.Any())
                data = data.Where(x => request.CountryValues.Select(z => z.Id).Contains(x.NationalityCountryId.GetValueOrDefault()));

            if (request.EmployeeTypeValues != null && request.EmployeeTypeValues.Any())
                data = data.Where(x => request.EmployeeTypeValues.Select(z => z.Id).Contains(x.EmployeeTypeId));

            if (request.OfficeValues != null && request.OfficeValues.Any())
                data = data.Where(x => request.OfficeValues.Select(z => z.Id).Contains(x.LocationId.GetValueOrDefault()));

            //not equal to manager role or HR role show only login profile
            if (!_ApplicationContext.RoleList.Contains((int)RoleEnum.Management) &&
                !_ApplicationContext.RoleList.Contains((int)RoleEnum.HumanResource))
            {
                data = data.Where(x => x.Id == _ApplicationContext.StaffId);
            }

            try
            {
                response.TotalCount = await data.CountAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            if (response.TotalCount == 0)
            {
                response.Result = StaffSearchResult.NotFound;
                return response;
            }

            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
            var result = await data.Skip(skip).Take(request.pageSize.Value).ToArrayAsync();
            response.Data = result.Select(_hrmap.MapStaffItem).ToArray();
            response.Result = StaffSearchResult.Success;

            return response;
        }


        public async Task<StaffSummaryResponse> GetStaffSummary()
        {
            var response = new StaffSummaryResponse();

            // Countries

            response.CountryList = _locationManager.GetCountries();

            if (response.CountryList == null)
                return new StaffSummaryResponse { Result = StaffSummaryResult.CannotGetCountryList };

            response.OfficeList = _office.GetOffices();

            if (response.OfficeList == null || !response.OfficeList.Any())
                return new StaffSummaryResponse { Result = StaffSummaryResult.CannotGetOfficeList };

            //Departments 
            var departments = _hrRepository.GetDepartments();

            if (departments == null || !departments.Any())
                return new StaffSummaryResponse { Result = StaffSummaryResult.CannotGetDepartmentList };

            response.DepartmentList = departments.Select(_hrmap.GetDepartment).ToArray();

            //Positions 
            var positions = await _hrRepository.GetPositions();

            if (positions == null || !positions.Any())
                return new StaffSummaryResponse { Result = StaffSummaryResult.CannotGetPositionList };

            response.PositionList = positions.Select(_hrmap.GetPosition).ToArray();

            var employeeTypes = await _hrRepository.GetEmployeeTypes();

            if (employeeTypes == null || !employeeTypes.Any())
                return new StaffSummaryResponse { Result = StaffSummaryResult.CannotGetEmployeeTypes };

            response.EmployeeTypeList = employeeTypes.Select(_hrmap.GetEmployeeType).ToArray();

            response.Result = StaffSummaryResult.Success;

            return response;

        }

        public async Task<StaffDeleteResponse> DeleteStaff(StaffDeleteRequest request)
        {
            var result = _hrRepository.RemoveStaff(request);
            var userData = await _hrRepository.GetUserDetails(request.Id);

            if (userData == null)
                return new StaffDeleteResponse { Result = StaffDeleteResult.UserAccountDeleteError };

            userData.DeletedBy = _ApplicationContext.UserId;
            userData.Active = false;
            userData.DeletedOn = DateTime.Now;
            _hrRepository.EditEntity(userData);
            await _hrRepository.Save();

            return new StaffDeleteResponse
            {
                Id = request.Id,
                Result = result
            };
        }

        public async Task<EditStaffResponse> GetEditStaff(int? id)
        {
            var response = new EditStaffResponse();

            //Staff
            if (id != null)
            {
                response.Staff = GetStaff(id.Value);

                if (response.Staff == null)
                    return new EditStaffResponse { Result = EditStaffResult.CannotGetStaff };
            }

            response.CountryList = _locationManager.GetCountries();

            if (response.CountryList == null || !response.CountryList.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetCountryList };

            //PositionList
            var positions = await _hrRepository.GetPositions();

            if (positions == null || !positions.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetPositionList };

            response.PositionList = positions.Select(_hrmap.GetPosition).ToArray();

            //QualificationList
            var qualifications = _hrRepository.GetQualifications();

            if (qualifications == null || !qualifications.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetQualificationList };

            response.QualificationList = qualifications.Select(_hrmap.GetQualification).ToArray();

            // ReportHeadList
            var reportHeads = _hrRepository.GetStaffList();

            if (reportHeads != null && reportHeads.Any())
            {
                if (id != null)
                    reportHeads = reportHeads.Where(x => x.Id != id.Value);

                response.ReportHeadList = reportHeads.Select(_hrmap.MapStaffItem).ToArray();
            }

            //EmployeeTypeList
            var employeeTypes = await _hrRepository.GetEmployeeTypes();

            if (employeeTypes == null || !employeeTypes.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetEmployeeTypes };

            response.EmployeeTypeList = employeeTypes.Select(_hrmap.GetEmployeeType).ToArray();

            //OfficeList
            response.OfficeList = _office.GetOffices();

            if (response.OfficeList == null || !response.OfficeList.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetOfficeList };


            //DepartmentList
            var departments = _hrRepository.GetDepartments().Where(x => x.DeptParentId == null);

            if (departments == null || !departments.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetDepartmentList };

            response.DepartmentList = departments.Select(_hrmap.GetDepartment).ToArray();

            //SubDepartmentList 
            if (response.Staff != null && response.Staff.DepartmentId != null)
            {
                var subDepartments = _hrRepository.GetDepartments().Where(x => x.DeptParentId == response.Staff.DepartmentId.Value);

                if (subDepartments != null && subDepartments.Any())
                    response.SubDepartmentList = subDepartments.Select(_hrmap.GetDepartment).ToArray();

            }
            //ProfileList
            var profiles = _hrRepository.GetProfiles();

            if (profiles == null || !profiles.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetProfileList };

            response.ProfileList = profiles.Select(_hrmap.GetProfile).ToArray();

            //CurrencyList
            response.CurrencyList = _referencemanager.GetCurrencies();

            if (response.CurrencyList == null || !response.CurrencyList.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetCurrencyList };

            //MarketSegmentList
            var msegments = _hrRepository.GetMarketSegments();

            if (msegments == null || !msegments.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetMarketSegmentList };

            response.MarketSegmentList = msegments.Select(_hrmap.GetMarketSegment).ToArray();

            //ProductCategoryList
            var pCatogries = _hrRepository.GetProductCategories();

            if (pCatogries == null || !pCatogries.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetProductCategoryList };

            response.ProductCategoryList = pCatogries.Select(_hrmap.GetProductCategory).ToArray();

            //ExpertiseList
            var expertises = await _hrRepository.GetExpertises();

            if (expertises == null || !expertises.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetExpertiseList };

            response.ExpertiseList = expertises.Select(_hrmap.GetExpertise).ToArray();

            //FileTypeList
            var fileTypes = _hrRepository.GetFileTypes();

            if (fileTypes == null || !fileTypes.Any())
                return new EditStaffResponse { Result = EditStaffResult.CannotGetFileTypeList };

            response.FileTypeList = fileTypes.Select(_hrmap.GetFileType).ToArray();


            // Home States
            if (response.Staff != null && response.Staff.HomeCountryId != null)
            {
                var stateResponse = _locationManager.GetStates(response.Staff.HomeCountryId.Value);

                if (stateResponse.Result == DTO.Location.StatesResult.Success)
                    response.HomeStateList = stateResponse.Data;
                else
                    return new EditStaffResponse { Result = EditStaffResult.CannotGetStateList };
            }

            // Current States
            if (response.Staff != null && response.Staff.CurrentCountryId != null)
            {
                var stateResponse = _locationManager.GetStates(response.Staff.CurrentCountryId.Value);

                if (stateResponse.Result == DTO.Location.StatesResult.Success)
                    response.CurrentStateList = stateResponse.Data;
                else
                    return new EditStaffResponse { Result = EditStaffResult.CannotGetStateList };
            }

            // Home Cities
            if (response.Staff != null && response.Staff.HomeStateId != null)
            {
                var citiesResponse = _locationManager.GetCities(response.Staff.HomeStateId.Value);

                if (citiesResponse.Result == DTO.Location.CitiesResult.Success)
                    response.HomeCityList = citiesResponse.Data;
                else
                    return new EditStaffResponse { Result = EditStaffResult.CannotGeCityList };
            }

            // Current Cities
            if (response.Staff != null && response.Staff.CurrentStateId != null)
            {
                var citiesResponse = _locationManager.GetCities(response.Staff.CurrentStateId.Value);

                if (citiesResponse.Result == DTO.Location.CitiesResult.Success)
                    response.CurrentCityList = citiesResponse.Data;
                else
                    return new EditStaffResponse { Result = EditStaffResult.CannotGeCityList };
            }


            response.Result = EditStaffResult.Success;
            return response;

        }

        public SubDepartmentResponse GetSubDepartments(int id)
        {
            var response = new SubDepartmentResponse();

            var departments = _hrRepository.GetDepartments().Where(x => x.DeptParentId == id);

            if (departments == null || !departments.Any())
                return new SubDepartmentResponse { Result = SubDepartmentResult.CannotFindSubDepartments };

            response.Data = departments.Select(_hrmap.GetDepartment).ToArray();
            response.Result = SubDepartmentResult.Success;

            return response;
        }

        public async Task<SaveStaffResponse> SaveStaff(StaffDetails request)
        {
            var response = new SaveStaffResponse();

            var staffExists = await _hrRepository.CheckEmailExists(request.CompanyEmail, request.Id);

            if (staffExists)
                return new SaveStaffResponse { Result = SaveStaffResult.StaffAlreadyExistsWithSameEmail };

            if (request.Id == 0)
            {
                HrStaff entity = _hrmap.MapStaffEntity(request, _filterService.GetCompanyId(), _ApplicationContext.UserId);

                if (entity == null)
                    return new SaveStaffResponse { Result = SaveStaffResult.CannotMapRequestToEntites };

                response.IdStaff = _hrRepository.AddStaff(entity);

                if (response.IdStaff == 0)
                    return new SaveStaffResponse { Result = SaveStaffResult.CannotAddStaff };

                response.Result = SaveStaffResult.Success;

                return response;
            }
            else
            {
                var element = _hrRepository.GetStaffDetails(request.Id);

                var entity = element.Item1;

                //if we update employee type id
                if (entity.EmployeeTypeId != request.EmployeeTypeId)
                {
                    var _activeUserList = entity.ItUserMasters.Where(x => x.Active).ToList();

                    foreach (var activeUser in _activeUserList)
                    {
                        if (activeUser != null)
                        {
                            //if employee type permanent - user type is internal user
                            if (request.EmployeeTypeId == (int)EmployeeTypeEnum.Permanent)
                                activeUser.UserTypeId = (int)UserTypeEnum.InternalUser;
                            else //other than permanent all usertype outsource 
                                activeUser.UserTypeId = (int)UserTypeEnum.OutSource;
                        }
                    }
                }

                if (entity == null)
                    return new SaveStaffResponse { Result = SaveStaffResult.CurrentSaffNotFound };

                // Remove Associations
                _hrRepository.RemoveEntities(entity.HrStaffTrainings);
                _hrRepository.RemoveEntities(entity.HrRenews);
                _hrRepository.RemoveEntities(entity.HrStaffHistories);
                _hrRepository.RemoveEntities(entity.HrStaffEntityServiceMaps);

                // Change Active flag for Remove some atteched files 
                if (request.AttachedList != null && request.AttachedList.Any())
                {
                    var hrAttachments = entity.HrAttachments.Where(x => !request.AttachedList.Any(y => y.UniqueId == x.UniqueId)).ToList();
                    hrAttachments.ForEach(x => { x.Active = false; x.DeletedBy = request.UserId; x.DeletedOn = DateTime.Now; });
                }
                else
                {
                    entity.HrAttachments.ToList().ForEach(x => { x.Active = false; x.DeletedBy = request.UserId; x.DeletedOn = DateTime.Now; });
                }

                // Change Active flag for Remove some atteched photos 
                if (request.HrPhoto != null && !string.IsNullOrEmpty(request.HrPhoto.UniqueId))
                {
                    var hrPhotos = entity.HrPhotos.Where(x => x.UniqueId != request.HrPhoto.UniqueId).ToList();
                    hrPhotos.ForEach(x => { x.Active = false; x.DeletedBy = request.UserId; x.DeletedOn = DateTime.Now; });
                }
                else
                {
                    entity.HrPhotos.ToList().ForEach(x => { x.Active = false; x.DeletedBy = request.UserId; x.DeletedOn = DateTime.Now; });
                }

                //if api entity ids available
                if (request.ApiEntityIds != null && request.ApiEntityIds.Any())
                {

                    await UpdateHrEntityMaps(entity, request);

                }


                _hrmap.UpdateEnity(entity, request, _filterService.GetCompanyId());
                _hrRepository.Save(entity);

                response.IdStaff = entity.Id;
                response.Result = SaveStaffResult.Success;
            }

            return response;
        }

        //update the hr entity maps
        private async Task UpdateHrEntityMaps(HrStaff entity, StaffDetails request)
        {
            //get the delete hr staff entity list
            var deleteHrStaffEntityList = entity.HrEntityMaps.Where(x => !request.ApiEntityIds.Contains(x.EntityId));

            //get the db staff entity id list
            var dbStaffEntityIds = entity.HrEntityMaps.Select(x => x.EntityId);

            //based on the db staff and request api entity ids, get the new api entity ids
            var newStaffEntityIds = request.ApiEntityIds.Where(x => !dbStaffEntityIds.Contains(x)).ToList();

            IEnumerable<ItUserRole> staffUserRoles = null;
            //if any entity removed or added
            if (deleteHrStaffEntityList.Any() || newStaffEntityIds.Any())
            {
                var staffUserIds = entity.ItUserMasters.Select(x => x.Id);
                //get the staff user roles
                staffUserRoles = await _userRepo.GetUserRolesByUserIdsIgnoreQueryFilter(staffUserIds);
            }
            //if any delete hr staff entity available
            if (deleteHrStaffEntityList.Any())
            {
                if (staffUserRoles != null && staffUserRoles.Any())
                {
                    //get the deleted entity ids
                    var deleteStaffEntityIds = deleteHrStaffEntityList.Select(x => x.EntityId);
                    //based on deleted entity ids select the roles

                    var deleteStaffUserRoles = staffUserRoles.Where(x => deleteStaffEntityIds.Contains(x.EntityId));
                    //remove roles from the IT_UserRole
                    if (deleteStaffUserRoles.Any())
                        _hrRepository.RemoveEntities(deleteStaffUserRoles);
                }
                //remove entity from the HR_Entity_Map
                _hrRepository.RemoveEntities(deleteHrStaffEntityList);
            }

            //if any new entity id added
            if (newStaffEntityIds.Any())
            {
                //new entity id loop
                newStaffEntityIds.ForEach(entityId =>
                {
                    //add the HR_Entity_Map table
                    var hrStaffEntityMap = new HrEntityMap()
                    {
                        EntityId = entityId
                    };
                    _hrRepository.AddEntity(hrStaffEntityMap);
                    entity.HrEntityMaps.Add(hrStaffEntityMap);


                    if (staffUserRoles != null && staffUserRoles.Any())
                    {
                        // if for existing data role already map with new entity then we want to skip this role
                        var newlyEntiyStaffUserRoles = staffUserRoles.Where(x => x.EntityId == entityId);
                        if (newlyEntiyStaffUserRoles.Any())
                        {
                            var newlyEntityRoles = newlyEntiyStaffUserRoles.Select(x => x.RoleId);
                            staffUserRoles = staffUserRoles.Where(x => !newlyEntityRoles.Contains(x.RoleId));
                        }

                        //get the user roles based on the primary entity
                        var primaryStaffUserRoles = staffUserRoles.Where(x => x.EntityId == entity.PrimaryEntity).ToList();
                        if (primaryStaffUserRoles.Any())
                        {
                            //primary entity user roles loop
                            primaryStaffUserRoles.ForEach(primaryUserRole =>
                            {
                                //Add the IT_UserRole
                                var itUserRole = new ItUserRole()
                                {
                                    EntityId = entityId,
                                    RoleId = primaryUserRole.RoleId,
                                    UserId = primaryUserRole.UserId,
                                };

                                _hrRepository.AddEntity(itUserRole);
                            });
                        }
                    }
                });
            }
        }

        public async Task<HrPhotoResponse> GetPhotoAsync(int id)
        {
            var hrPhoto = await _hrRepository.GetPhoto(id);

            if (hrPhoto == null)
                return new HrPhotoResponse { Result = HrPhotoResult.NotFound };

            return new HrPhotoResponse
            {
                GuidId = hrPhoto.GuidId,
                UniqueId = hrPhoto.UniqueId,
                StaffId = hrPhoto.StaffId,
                FileName = hrPhoto.FileName,
                FileUrl = hrPhoto.FileUrl,
                UserId = hrPhoto.UserId,
                UploadDate = hrPhoto.UploadDate,
                Active = hrPhoto.Active,
                Result = HrPhotoResult.Success
            };
        }

        public HolidayMasterResponse GetHolidayDataMaster()
        {
            var response = new HolidayMasterResponse();

            response.CountryList = _locationManager.GetCountries();

            if (response.CountryList == null || !response.CountryList.Any())
                return new HolidayMasterResponse { Result = HolidayMasterResult.CannotGetCountryList };

            //OfficeList
            response.OfficeList = _office.GetOffices();

            if (response.OfficeList == null || !response.OfficeList.Any())
                return new HolidayMasterResponse { Result = HolidayMasterResult.CannotGetOfficeList };

            // Holiday Day type List

            var holidayTypes = _hrRepository.GetHolidayDayTypeList();

            if (holidayTypes == null || !holidayTypes.Any())
                return new HolidayMasterResponse { Result = HolidayMasterResult.CannotGetHolidayDayTypes };

            response.HolidayDayTypeList = holidayTypes.Select(_hrmap.GetHolidayDayType);

            //Years
            var years = new List<int>();
            for (int i = 2000; i <= DateTime.Now.Year + 5; i++)
                years.Add(i);

            response.YearList = years;
            response.Result = HolidayMasterResult.Success;

            return response;
        }

        public async Task<HolidayDetailsResponse> GetHolidayDetails(HolidayDetaisRequest request)
        {
            var data = await (_hrRepository.GetHolidays(request.Year, request.CountryId, request.BranchId) as IQueryable<HrHoliday>).ToArrayAsync();

            if (data == null || !data.Any())
                return new HolidayDetailsResponse { Result = HolidayDetailsResult.NoDataFound };

            var response = new HolidayDetailsResponse();
            var lstHolidays = data.Select(x => new Holiday
            {
                Id = x.Id,
                Name = x.HolidayName,
                RecurrenceType = (RecurrenceType)x.RecurrenceType,
                StartDate = x.StartDate.GetCustomDate(),
                EndDate = x.EndDate.GetCustomDate(),
                StartDayType = x.StartDateType,
                EndDayType = x.EndDateType
            });

            //foreach (var item in data.Where(x => x.RecurrenceType == (int)RecurrenceType.EveryYear))
            //{
            //    lstHolidays.Add(new Holiday
            //    {
            //        Id = item.Id,
            //        Name = item.HolidayName,
            //        RecurrenceType = RecurrenceType.EveryYear,
            //        StartDate = item.StartDate == null ? null : new DateObject(request.Year, item.StartDate.Value.Month, item.StartDate.Value.Day),
            //        EndDate = item.EndDate == null ? null : new DateObject(request.Year, item.EndDate.Value.Month, item.EndDate.Value.Day)
            //    });
            //}

            //foreach (var item in data.Where(x => x.RecurrenceType == (int)RecurrenceType.EveryMonth))
            //{

            //    for (int i = 1; i <= 12; i++)
            //    {
            //        lstHolidays.Add(new Holiday
            //        {
            //            Id = item.Id,
            //            Name = item.HolidayName,
            //            RecurrenceType = RecurrenceType.EveryMonth,
            //            StartDate = item.StartDate == null ? null : new DateObject(request.Year, i, item.StartDate.Value.Day),
            //            EndDate = item.EndDate == null ? null : new DateObject(request.Year, i, item.EndDate.Value.Day)
            //        });
            //    }
            //}

            //foreach (var item in data.Where(x => x.RecurrenceType == (int)RecurrenceType.EveryWeek))
            //{
            //    var curDate = new DateTime(request.Year, 1, 1);

            //    var lst = new List<Holiday>();

            //    while (curDate != new DateTime(request.Year, 12, 31))
            //    {
            //        DateTime? startDate = null;
            //        DateTime? endDate = null;

            //        if (item.StartDay != null && (int)curDate.DayOfWeek == item.StartDay.Value)
            //        {
            //            startDate = curDate;

            //            lst.Add(new Holiday
            //            {
            //                Id = item.Id,
            //                Name = item.HolidayName,
            //                RecurrenceType = RecurrenceType.EveryWeek,
            //                StartDate = startDate.GetCustomDate(),
            //                StartDay = curDate.DayOfWeek,
            //            });
            //        }

            //        if (item.EndDay != null && (int)curDate.DayOfWeek == item.EndDay.Value)
            //        {
            //            endDate = curDate;

            //            // check last element
            //            var element = lst.LastOrDefault();
            //            if (element == null)
            //                lstHolidays.Add(new Holiday
            //                {
            //                    Id = item.Id,
            //                    Name = item.HolidayName,
            //                    RecurrenceType = RecurrenceType.EveryWeek,
            //                    EndDate = endDate.GetCustomDate(),
            //                    EndDay = curDate.DayOfWeek
            //                });
            //            else
            //            {
            //                element.EndDate = endDate.GetCustomDate();
            //                element.EndDay = curDate.DayOfWeek;

            //            }
            //        }

            //        curDate = curDate.AddDays(1);
            //    }

            //    lstHolidays.AddRange(lst);
            //}

            //var response = new HolidayDetailsResponse(); 

            //if (request.Index == null || request.Index == 0)
            //    request.Index = 1;


            //response.TotalCount = data.Count();
            //int skip = (request.Index.Value - 1) * request.PageSize.Value;

            //response.PageCount = (response.TotalCount / request.PageSize.Value) + (response.TotalCount % request.PageSize.Value > 0 ? 1 : 0);

            //response.Data = data.Skip(skip).Take(request.PageSize.Value).Select(x => LocationMap.GetHoliday(x, request.Year)).ToArray();

            response.Data = lstHolidays;
            response.Result = HolidayDetailsResult.Success;


            return response;

        }

        private StaffDetails GetStaff(int id)
        {
            var staff = _hrRepository.GetStaffDetails(id);

            if (staff.Item1 == null)
                return null;

            return _hrmap.GetStaffDetails(staff.Item1, (x) => _fileManager.GetMimeType(x), staff.Item2);
        }

        public async Task<EditHolidayResponse> AddOrEditHoliday(EditHolidayRequest request)
        {
            if (request.Id > 0)
            {
                var holiday = await _hrRepository.GetHoliday(request.Id);

                if (holiday == null)
                    return new EditHolidayResponse { Result = EditHolidayResult.HolidayIsNotFound };

                if (request.RecurrenceType != RecurrenceType.Manually && request.ForAllIterations)
                {
                    var holidayList = AddIterationHolidays(request);
                    var ids = await _hrRepository.EditHolidayList(holiday, holidayList);

                    return new EditHolidayResponse { Ids = ids, Result = EditHolidayResult.Success };
                }
                else
                {

                    if (holiday.RecurrenceType == (int)RecurrenceType.EveryWeek)
                    {
                        if (request.StartDay != null && holiday.StartDate != null)
                        {

                            var date = holiday.StartDate.Value;

                            if (request.StartDay == 0)
                                request.StartDay = 7;

                            int currentholidayDay = (int)holiday.StartDate.Value.DayOfWeek == 0 ? 7 : (int)holiday.StartDate.Value.DayOfWeek;

                            date = date.AddDays(request.StartDay.Value - currentholidayDay);
                            holiday.StartDate = date;

                        }

                        if (request.EndDay != null && holiday.EndDate != null)
                        {
                            var date = holiday.EndDate.Value;

                            if (request.EndDay == 0)
                                request.EndDay = 7;

                            int currentholidayDay = (int)holiday.EndDate.Value.DayOfWeek == 0 ? 7 : (int)holiday.EndDate.Value.DayOfWeek;

                            date = date.AddDays(request.EndDay.Value - currentholidayDay);
                            holiday.EndDate = date;
                        }

                    }
                    else
                    {
                        holiday.StartDate = request.StartDate.ToDateTime();
                        holiday.EndDate = request.EndDate.ToDateTime();
                    }

                    holiday.HolidayName = request.HolidayName;
                    holiday.RecurrenceType = (int)request.RecurrenceType;
                    holiday.StartDateType = request.StartDayType;
                    holiday.EndDateType = request.EndDayType;

                    int id = await _hrRepository.EditHoliday(holiday);

                    if (id <= 0)
                        return new EditHolidayResponse { Result = EditHolidayResult.HolidayIsNotSaved };

                    return new EditHolidayResponse { Ids = new List<int> { id }, Result = EditHolidayResult.Success };

                }
            }
            else
            {
                var lstHolidays = new List<HrHoliday>();

                if (request.RecurrenceType == RecurrenceType.Manually)
                {
                    lstHolidays.Add(new HrHoliday
                    {
                        HolidayName = request.HolidayName?.Trim(),
                        CountryId = request.CountryId,
                        LocationId = request.OfficeId != null && request.OfficeId == 0 ? null : request.OfficeId,
                        RecurrenceType = (int)request.RecurrenceType,
                        StartDate = request.StartDate.ToDateTime(),
                        EndDate = request.EndDate.ToDateTime(),
                        StartDateType = request.StartDayType,
                        EndDateType = request.EndDayType,
                        EntityId = _filterService.GetCompanyId()
                    });
                }
                else
                    lstHolidays = AddIterationHolidays(request).ToList();

                var ids = await _hrRepository.AddHoliday(lstHolidays);

                if (ids == null || !ids.Any())
                    return new EditHolidayResponse { Result = EditHolidayResult.HolidayIsNotSaved };

                return new EditHolidayResponse { Ids = ids, Result = EditHolidayResult.Success };
            }

        }

        public async Task<StaffInfo> GetStaffByUserId(int userId)
        {
            var staff = await _hrRepository.GetStaffByUserId(userId);

            if (staff == null)
                return null;

            return _hrmap.GetStaffInfo(staff);
        }

        public async Task<StaffInfo> GetStaffDetailsByStaffId(int userId)
        {
            var staff = await _hrRepository.GetStaffByStaffId(userId);

            if (staff == null)
                return null;

            return _hrmap.GetStaffInfo(staff);
        }

        public async Task<StaffInfo> GetStaffById(int staffId)
        {
            var staff = _hrRepository.GetStaffDetails(staffId);

            if (staff.Item1 == null)
                return null;

            return _hrmap.GetStaffInfo(staff.Item1);
        }

        public async Task<BookingStaffInfo> GetStaffInfoByStaffId(int staffId)
        {
            var staff = await GetStaffInfo(staffId);

            BookingStaffInfo objStaff = new BookingStaffInfo();

            if (staff != null)
            {
                objStaff.Id = staff.Id;
                objStaff.CompanyEmail = staff.CompanyEmail;
                objStaff.StaffName = staff.PersonName;
                objStaff.CompanyPhone = staff.CompanyMobileNo;
                objStaff.EmployeeTypeId = staff.EmployeeTypeId;
            }

            return objStaff;
        }


        public IEnumerable<StaffInfo> GetStaffList()
        {
            var staffs = _hrRepository.GetStaffList();

            if (staffs == null || !staffs.Any())
                return null;


            return staffs.Select(_hrmap.GetStaffInfo);
        }

        public async Task<IEnumerable<StaffInfo>> GetStaffListByLocations()
        {
            if (_ApplicationContext.LocationList == null || !_ApplicationContext.LocationList.Any())
                return null;

            var staffs = await _hrRepository.GetStaffListByLocations(_ApplicationContext.LocationList);

            if (staffs == null || !staffs.Any())
                return null;


            return staffs.Select(_hrmap.GetStaffInfo);
        }

        /// <summary>
        /// Get the internal staff list by location list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<StaffInfo>> GetInternalStaffListByLocationList(int typeId)
        {
            if (_ApplicationContext.LocationList == null || !_ApplicationContext.LocationList.Any())
                return null;
            //get the internal staff by locations
            return await _hrRepository.GetInternalStaffListByLocationList(_ApplicationContext.LocationList, typeId);
        }

        public async Task<IEnumerable<StaffInfo>> GetStaffListByParentId(int staffId)
        {
            var data = await _hrRepository.GetstaffsByParentId(staffId, true);

            if (data == null)
                return null;

            return data.Select(_hrmap.GetStaffInfo);
        }

        public async Task<IEnumerable<int>> GetStaffIdsByParent()
        {
            var data = await _hrRepository.GetstaffsByParentId(_ApplicationContext.StaffId, false);

            if (data == null)
                return null;

            return data.Select(x => x.Id);
        }

        public async Task<IEnumerable<int>> GetStaffIdsByParentwithProfile()
        {
            var data = await _hrRepository.GetstaffsByParentId(_ApplicationContext.StaffId, false);

            if (data == null)
                return null;

            return data.Select(x => x.Id);
        }

        private IEnumerable<HrHoliday> AddIterationHolidays(EditHolidayRequest request)
        {
            var lstHolidays = new List<HrHoliday>();

            var _entityId = _filterService.GetCompanyId();
            if (request.RecurrenceType == RecurrenceType.EveryYear)
            {

                for (var i = DateTime.Now.Year; i < DateTime.Now.Year + 20; i++)
                {
                    var startDate = request.StartDate;
                    var endDate = request.EndDate;

                    startDate.Year = i;
                    endDate.Year = i;

                    lstHolidays.Add(new HrHoliday
                    {
                        HolidayName = request.HolidayName?.Trim(),
                        CountryId = request.CountryId,
                        LocationId = request.OfficeId != null && request.OfficeId == 0 ? null : request.OfficeId,
                        RecurrenceType = (int)request.RecurrenceType,
                        StartDate = startDate.ToDateTime(),
                        EndDate = endDate.ToDateTime(),
                        StartDateType = request.StartDayType,
                        EndDateType = request.EndDayType,
                        EntityId = _entityId
                    });
                }
            }
            else if (request.RecurrenceType == RecurrenceType.EveryMonth)
            {
                for (var j = request.StartDate.Month; j <= 12; j++)
                {
                    var startDate = request.StartDate;
                    var endDate = request.EndDate;

                    startDate.Month = j;
                    endDate.Month = j;

                    lstHolidays.Add(new HrHoliday
                    {
                        HolidayName = request.HolidayName?.Trim(),
                        CountryId = request.CountryId,
                        LocationId = request.OfficeId != null && request.OfficeId == 0 ? null : request.OfficeId,
                        RecurrenceType = (int)request.RecurrenceType,
                        StartDate = startDate.ToDateTime(),
                        EndDate = endDate.ToDateTime(),
                        StartDateType = request.StartDayType,
                        EndDateType = request.EndDayType,
                        EntityId = _entityId
                    });
                }

            }
            else if (request.RecurrenceType == RecurrenceType.EveryWeek)
            {
                var curDate = request.StartDate.ToDateTime();
                int curYear = request.StartDate.Year;
                var holiday = new HrHoliday();

                DateTime endDate = request.EndDateWeek != null && request.EndDate.Year > 0 ? request.EndDateWeek.ToDateTime() : new DateTime(curYear, 12, 31);


                while (curDate <= endDate)
                {

                    if (request.StartDay != null && (int)curDate.DayOfWeek == request.StartDay.Value)
                    {
                        holiday = new HrHoliday
                        {
                            HolidayName = request.HolidayName?.Trim(),
                            CountryId = request.CountryId,
                            LocationId = request.OfficeId != null && request.OfficeId == 0 ? null : request.OfficeId,
                            RecurrenceType = (int)request.RecurrenceType,
                            StartDate = curDate,
                            EndDate = null,
                            StartDateType = request.StartDayType,
                            EndDateType = request.EndDayType,
                            EntityId = _entityId
                        };

                        lstHolidays.Add(holiday);
                    }

                    if (request.EndDay != null && (int)curDate.DayOfWeek == request.EndDay.Value)
                    {
                        holiday.EndDate = curDate;
                    }

                    curDate = curDate.AddDays(1);
                }


            }

            return lstHolidays;
        }


        /// <summary>
        /// Delete holiday
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EditHolidayResponse> DeleteHoliday(DeleteHolidayRequest request)
        {
            bool isDeleted = false;
            IEnumerable<int> ids = null;

            if (request.ForAllIterations)
            {

                ids = await _hrRepository.removeHolidayTypes(request.Id);
                isDeleted = ids != null && ids.Any();
            }
            else
            {
                isDeleted = await _hrRepository.DeleteHoliday(request.Id);
                ids = new List<int> { request.Id };
            }


            if (isDeleted)
                return new EditHolidayResponse { Ids = ids, Result = EditHolidayResult.Success };

            return new EditHolidayResponse { Ids = new List<int> { request.Id }, Result = EditHolidayResult.HolidayIsNotFound };
        }

        public async Task<OfficesControlResponse> GetOfficesControl()
        {
            var response = new OfficesControlResponse();

            response.LocationList = _office.GetAllOffices();

            if (response.LocationList == null || !response.LocationList.Any())
                return new OfficesControlResponse { Result = OfficesControlResult.CannotFindLocations };

            var staffs = _hrRepository.GetStaffList();

            if (staffs == null || !staffs.Any())
                return new OfficesControlResponse { Result = OfficesControlResult.CannotFindStaffList };

            response.StaffList = staffs.Select(_hrmap.MapStaffItem).ToArray();

            response.Result = OfficesControlResult.Success;

            return response;
        }

        public async Task<OfficeControlStaffResponse> GetOfficesControlByStaffId(int id)
        {
            var response = new OfficeControlStaffResponse();

            var data = await _hrRepository.GetOfficesByStaff(id);

            if (data != null && data.Any())
                return new OfficeControlStaffResponse { Result = OfficeControlStaffResult.Success, Data = data.Select(x => _locmap.GetOffice(x.Location)) };

            return new OfficeControlStaffResponse { Result = OfficeControlStaffResult.Success, Data = null };
        }

        public async Task<SaveOfficeControlResponse> SaveOfficeControl(SaveOfficeControlRequest request)
        {
            if (request.Data != null && request.StaffId > 0)
            {
                var EntityId = _filterService.GetCompanyId();
                var lst = request.Data.Select(x => new HrOfficeControl
                {
                    LocationId = x.Id,
                    StaffId = request.StaffId,
                    EntityId = EntityId
                });

                //logic for remove office access from cs configuration page
                var data = await _hrRepository.GetOfficesByStaff(request.StaffId);
                if (data != null && data.Any())
                {
                    var lstrequestloc = request.Data.Select(x => x.Id).ToList();
                    var removeaudlist = data.Where(x => !lstrequestloc.Contains(x.LocationId));
                    if (removeaudlist != null && removeaudlist.Any())
                        await _csmanager.DeleteCSConfigSummaryByLocation(request.StaffId, removeaudlist.Select(x => x.LocationId).ToList());
                }

                await _hrRepository.UpdateOfficesControl(lst, request.StaffId);

                return new SaveOfficeControlResponse { StaffId = request.StaffId, Result = SaveOfficeControlResult.Success };
            }

            return new SaveOfficeControlResponse { StaffId = request.StaffId, Result = SaveOfficeControlResult.CannotSave };

        }

        public async Task<IEnumerable<DateObject>> GetHolidaysByRange(DateTime _startdate, DateTime enddate, int countryId, int locationId)
        {
            var data = await _hrRepository.GetHolidaysByRange(_startdate, enddate, countryId, locationId);
            if (data == null)
                return null;
            var lstholiday = new List<DateTime>();
            foreach (var _holi in data)
            {
                if (_holi.EndDate != null && _holi.StartDate != null && _holi.EndDate.Value != _holi.StartDate.Value)
                {
                    var daterange = Enumerable.Range(0, _holi.EndDate.Value.Subtract(_holi.StartDate.Value).Days + 1).Select(d => _holi.StartDate.Value.AddDays(d));
                    if (daterange != null || daterange.Count() > 0)
                        lstholiday.AddRange(daterange);
                }
                else if (_holi.EndDate != null && _holi.StartDate != null && _holi.EndDate.Value == _holi.StartDate.Value)
                {
                    lstholiday.Add(_holi.StartDate.Value);
                }
            }
            return lstholiday.Distinct().Select(x => x.GetCustomDate());
        }

        public async Task<IEnumerable<DateTime>> GetHolidaysDateByRange(DateTime startdate, DateTime enddate, int countryId, int locationId)
        {
            var data = await _hrRepository.GetHolidaysByRange(startdate, enddate, countryId, locationId);
            if (data == null)
                return null;
            var lstholiday = new List<DateTime>();
            foreach (var _holi in data)
            {
                if (_holi.EndDate != null && _holi.StartDate != null && _holi.EndDate.Value != _holi.StartDate.Value)
                {
                    var daterange = Enumerable.Range(0, _holi.EndDate.Value.Subtract(_holi.StartDate.Value).Days + 1).Select(d => _holi.StartDate.Value.AddDays(d));
                    if (daterange != null || daterange.Count() > 0)
                        lstholiday.AddRange(daterange);
                }
                else if (_holi.EndDate != null && _holi.StartDate != null && _holi.EndDate.Value == _holi.StartDate.Value)
                {
                    lstholiday.Add(_holi.StartDate.Value);
                }
            }
            return lstholiday.Distinct().OrderBy(x => x.Date).Select(x => x.Date).ToList();
        }

        public async Task<LeaveResponse> GetLeaveRequest(int? id)
        {
            var response = new LeaveResponse();
            bool cancelLeave = true;
            if (id != null)
            {
                response.LeaveRequest = await GetLeave(id.Value);
                response.CanApprove = false;

                if (response.LeaveRequest == null)
                    return new LeaveResponse { Result = LeaveResult.CannotFindLeaveRequest };

                bool findStaff = false;

                if (_ApplicationContext.StaffId != response.LeaveRequest.StaffId)
                {
                    if (_ApplicationContext.RoleList.Contains((int)RoleEnum.Management))
                    {
                        var ids = (await GetStaffIdsByParent()).ToList();

                        if (!ids.Contains(response.LeaveRequest.StaffId))
                            findStaff = false;
                        else if (response.LeaveRequest.StatusId == (int)Entities.Enums.LeaveStatus.Request)
                        {
                            response.CanApprove = true;
                            findStaff = true;
                        }
                        else if (response.LeaveRequest.StatusId == (int)Entities.Enums.LeaveStatus.Cancelled || response.LeaveRequest.StatusId == (int)Entities.Enums.LeaveStatus.Approved)
                            findStaff = true;

                        response.CanEdit = false;
                    }

                    if (!findStaff && _ApplicationContext.RoleList.Contains((int)RoleEnum.LeaveHr) && _ApplicationContext.LocationList != null && _ApplicationContext.LocationList.Any())
                    {
                        var staffList = await _hrRepository.GetStaffListByLocations(_ApplicationContext.LocationList);

                        if (staffList != null && staffList.Any(x => x.Id == response.LeaveRequest.StaffId))
                        {
                            findStaff = true;
                            response.CanEdit = false;
                            response.CanApprove = false;
                        }
                    }

                    if (!findStaff)
                        return new LeaveResponse { Result = LeaveResult.CannotShowLeaveRequest };
                }
                else
                {
                    response.CanEdit = response.LeaveRequest.StatusId == (int)Entities.Enums.LeaveStatus.Request || response.LeaveRequest.StatusId == (int)Entities.Enums.LeaveStatus.Rejected;
                    //show cancel button after leave approval from 7 days after todate
                    response.CanCancel = !cancelLeave;
                    if (response.LeaveRequest.StatusId == (int)Entities.Enums.LeaveStatus.Request || response.LeaveRequest.StatusId == (int)Entities.Enums.LeaveStatus.Rejected)
                        response.CanCancel = cancelLeave;

                    else if (response.LeaveRequest.StatusId == (int)Entities.Enums.LeaveStatus.Approved)
                    {
                        double dateDifference = DateTime.Now.Date.Subtract(response.LeaveRequest.EndDate.ToDateTime().Date).TotalDays;
                        response.CanCancel = dateDifference < 7 ? cancelLeave : !cancelLeave;
                    }
                }
            }
            else
                response.CanEdit = true;

            // Leave Types
            var data = await _hrRepository.GetLeaveTypes();

            if (data == null || !data.Any())
                return new LeaveResponse { Result = LeaveResult.CannotFindTypes };

            response.LeaveTypeList = data.Select(_hrmap.GetLeaveType);

            // Day type List
            var dayTypes = _hrRepository.GetHolidayDayTypeList();

            if (dayTypes == null || !dayTypes.Any())
                return new LeaveResponse { Result = LeaveResult.CannotFinddayTypeList };

            response.DayTypeList = dayTypes.Select(_hrmap.GetHolidayDayType);

            response.Result = LeaveResult.Success;

            return response;
        }

        public void UpdateEmailState(Guid id, int emailState, bool isTask)
        {
            var task = _hrRepository.GetSingle<MidTask>(x => x.Id == id);

            // TODO TASK OR NOTIFICATION

            if (task != null)
            {
                task.EmailState = emailState;
                task.UpdatedBy = _ApplicationContext?.UserId;
                task.UpdatedOn = DateTime.Now;
                _hrRepository.Save(task);
            }
        }

        public async Task<SaveLeaveRequestResponse> SaveLeaveRequest(LeaveRequest request)
        {
            //using (var transaction = _hrRepository.BeginTransaction())
            //{

            var response = new SaveLeaveRequestResponse();
            bool sendNotification = false;

            HrLeave entity = null;

            //get the location entity mapped with the staff
            var staffEntity = await _hrRepository.GetStaffLocationEntity(_ApplicationContext.StaffId);
            //if location entity id is null (which means location filtered with globally entity id) then send the entitynotmatched status
            if (staffEntity != null && (staffEntity.entityId == null || staffEntity.entityId != _filterService.GetCompanyId()))
                response.Result = SaveLeaveRequestResult.StaffEntityNotMatched;
            //if location entity matches with the global entity then save the leave request
            else if (staffEntity != null && staffEntity.entityId == _filterService.GetCompanyId())
            {
                if (request.Id > 0)
                {
                    entity = await _hrRepository.GetLeaveById(request.Id);

                    if (entity == null)
                        return new SaveLeaveRequestResponse { Result = SaveLeaveRequestResult.NotFound };

                    if (entity.StaffId != _ApplicationContext.StaffId)
                        return new SaveLeaveRequestResponse { Result = SaveLeaveRequestResult.UnAuthorized };


                    sendNotification = entity.Status == (int)Entities.Enums.LeaveStatus.Rejected;


                    entity.DateBegin = request.StartDate.ToDateTime();
                    entity.DateEnd = request.EndDate.ToDateTime();
                    entity.Comments = request.Reason?.Trim();
                    entity.Status = (int)Entities.Enums.LeaveStatus.Request;
                    entity.LeaveTypeId = request.LeaveTypeId;
                    entity.NumberOfDays = request.LeaveDays;//(request.EndDate.ToDateTime() - request.StartDate.ToDateTime()).TotalDays + 1;
                    entity.LeaveApplicationDate = DateTime.Now;
                    entity.IdTypeStartDate = request.StartDayType;
                    entity.IdTypeEndDate = request.EndDayType;



                }
                else
                {
                    entity = new HrLeave
                    {
                        DateBegin = request.StartDate.ToDateTime(),
                        DateEnd = request.EndDate.ToDateTime(),
                        Comments = request.Reason?.Trim(),
                        StaffId = _ApplicationContext.StaffId,
                        Status = (int)Entities.Enums.LeaveStatus.Request,
                        LeaveTypeId = request.LeaveTypeId,
                        NumberOfDays = request.LeaveDays,// (request.EndDate.ToDateTime() - request.StartDate.ToDateTime()).TotalDays + 1 ,
                        LeaveApplicationDate = DateTime.Now,
                        IdTypeStartDate = request.StartDayType,
                        IdTypeEndDate = request.EndDayType,
                        EntityId = _filterService.GetCompanyId()
                    };

                    sendNotification = true;

                }

                _hrRepository.Save(entity, request.Id > 0);

                if (sendNotification)
                {

                    var masterConfigs = await _userConfigRepository.GetMasterConfiguration();
                    var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
                    var baseUrl = _configuration["BaseUrl"];
                    // Get reportTo Id : 
                    var staff = _hrRepository.GetStaffAsync(entity.StaffId);

                    if (staff.ParentStaff != null && staff.ParentStaff.ItUserMasters != null && staff.ParentStaff.ItUserMasters.Any())
                    {
                        // Add Task 
                        var task = new MidTask
                        {
                            Id = Guid.NewGuid(),
                            IsDone = false,
                            LinkId = entity.Id,
                            UserId = _ApplicationContext.UserId,
                            ReportTo = staff.ParentStaff.ItUserMasters.First().Id,
                            TaskTypeId = (int)TaskType.LeaveToApprove,
                            CreatedBy = _ApplicationContext?.UserId,
                            CreatedOn = DateTime.Now,
                            EntityId = _filterService.GetCompanyId()
                        };

                        _hrRepository.Save(task, false);

                        string leaveType = _hrRepository.GetSingle<HrLeaveType>(x => x.Id == entity.LeaveTypeId).Description;

                        // int recepientUserId = 0;

                        // if (staff.ParentStaff != null && staff.ParentStaff.ItUserMasters != null && staff.ParentStaff.ItUserMasters.Any())
                        int recepientUserId = staff.ParentStaff.ItUserMasters.First().Id;

                        return new SaveLeaveRequestResponse
                        {
                            Id = entity.Id,
                            TaskId = task.Id,
                            EmailModel = new LeaveEmailModel
                            {
                                ApplyDate = DateTime.Now.ToString(StandardDateFormat),
                                Days = entity.NumberOfDays,
                                EndDate = entity.DateEnd.ToString(StandardDateFormat),
                                LeaveId = entity.Id,
                                LeaveType = leaveType,
                                Reason = entity.Comments?.Trim(),
                                RecepientEmail = staff.ParentStaff?.CompanyEmail,
                                RecepientCCEmail = staff.CompanyEmail,
                                RecepientName = staff.ParentStaff?.PersonName,
                                RecepiendUserId = recepientUserId,
                                SenderName = staff.PersonName,
                                StartDate = entity.DateBegin.ToString(StandardDateFormat),
                                WebSite = baseUrl + string.Format(_configuration["UrlLeaveRequest"], entity.Id, entityName)
                            },
                            Result = SaveLeaveRequestResult.Success,
                            SendNotification = true
                        };
                    }
                }

                return new SaveLeaveRequestResponse
                {
                    Id = entity.Id,
                    Result = SaveLeaveRequestResult.Success,
                    SendNotification = sendNotification
                };
            }




            //transaction.Commit();


            //}

            return response;
        }

        public HrStaff GetBasicStaff(int id)
        {
            return _hrRepository.GetStaffAsync(id);
        }

        private async Task<HrStaff> GetStaffInfo(int staffId)
        {
            return await _hrRepository.GetStaffInfo(staffId);
        }




        public async Task<LeaveSummaryResponse> GetLeaveSummary()
        {

            var response = new LeaveSummaryResponse();

            //Get Leave Types
            var data = await _hrRepository.GetLeaveTypes();

            if (data == null || !data.Any())
                return new LeaveSummaryResponse { Result = LeaveSummaryResult.CannotFindTypes };

            response.LeaveTypeList = data.Select(_hrmap.GetLeaveType);

            // get statuses
            var datastatuses = await _hrRepository.GetLeaveStatuses();

            if (datastatuses == null || !datastatuses.Any())
                return new LeaveSummaryResponse { Result = LeaveSummaryResult.CannotFindStatuses };

            response.LeaveStatusList = datastatuses.Select(_hrmap.GetLeaveStatus);

            // Day type List
            var dayTypes = _hrRepository.GetHolidayDayTypeList();

            if (dayTypes == null || !dayTypes.Any())
                return new LeaveSummaryResponse { Result = LeaveSummaryResult.CannotFinddayTypeList };

            response.DayTypeList = dayTypes.Select(_hrmap.GetHolidayDayType);

            response.Result = LeaveSummaryResult.Success;

            return response;
        }

        public async Task<LeaveSummaryDataResponse> GetLeaveDataSummary(LeaveSummaryRequest request)
        {
            if (!_ApplicationContext.RoleList.Contains((int)RoleEnum.Management) && request.IsApproveSummary)
                return new LeaveSummaryDataResponse { Result = LeaveSummaryDataResult.CannotShow };

            if (request.Index == null || request.Index <= 0)
                request.Index = 1;
            if (request.PageSize <= 0)
                request.PageSize = 20;

            var repoRequest = new LeaveSummaryDataRequest
            {
                StaffName = request.StaffName,
                StartDate = request.StartDate == null ? (DateTime?)null : request.StartDate.ToDateTime(),
                EndDate = request.EndDate == null ? (DateTime?)null : request.EndDate.ToDateTime(),
                StatusIds = request.StatusValues == null ? null : request.StatusValues.Select(x => x.Id),
                Typeids = request.TypeValues == null ? null : request.TypeValues.Select(x => x.Id),
                Index = request.Index.Value,
                PageSize = request.PageSize,
                IsApproveSummary = request.IsApproveSummary
            };

            // staff ids : 
            var staffIds = new List<int>() { _ApplicationContext.StaffId };

            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.Management))
            {
                var ids = (await GetStaffIdsByParent()).ToList();
                ids.Add(_ApplicationContext.StaffId);
                staffIds.AddRange(ids);
            }

            repoRequest.Staffids = staffIds;

            //Locations list if HumanResource
            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.HumanResource))
                repoRequest.LocationIds = _ApplicationContext.LocationList;


            var data = await _hrRepository.GetLeaveData(repoRequest);

            if (data.Item1 == 0)
                return new LeaveSummaryDataResponse { Result = LeaveSummaryDataResult.NotFound };

            IEnumerable<DTO.HumanResource.LeaveStatus> lstStatusCount = null;

            if (data.Item3 != null && data.Item3.Any())
            {
                lstStatusCount = data.Item3.Select(x => new DTO.HumanResource.LeaveStatus
                {
                    Id = x.Key.Id,
                    Label = x.Key.IdTran.GetTranslation(x.Key.Label),
                    TotalCount = x.Value
                });
            }

            var leaveitems = data.Item2.Select(x => _hrmap.GetLeaveItem(x, _ApplicationContext.StaffId, _ApplicationContext.RoleList, staffIds));


            return new LeaveSummaryDataResponse
            {
                Data = leaveitems,
                Result = LeaveSummaryDataResult.Success,
                TotalCount = data.Item1,
                CanCheck = _ApplicationContext.RoleList.Contains((int)RoleEnum.HumanResource),
                Index = request.Index.Value,
                PageSize = request.PageSize,
                LeaveStatusList = lstStatusCount
            };

        }


        public async Task<LeaveDaysResponse> GetLeaveDays(LeaveDaysRequest request)
        {
            double days = 0;
            if (request != null && request.EndDate != null && request.StartDate != null)
            {
                // get Holidays between two dates
                days = (request.EndDate.ToDateTime() - request.StartDate.ToDateTime()).TotalDays + 1;

                if (request.StartDayType == DayType.AfterNoon)
                    days = days - 0.5;
                if (request.EndDayType == DayType.Morning)
                    days = days - 0.5;
            }

            return new LeaveDaysResponse { Days = days, Result = LeaveDaysResult.Success };
        }

        private async Task<LeaveRequest> GetLeave(int id)
        {
            var item = await _hrRepository.GetLeaveById(id);

            if (item == null)
                return null;

            return _hrmap.GetLeave(item);
        }

        public async Task<AuditorResponse> GetAuditors()
        {
            var response = new AuditorResponse();
            var data = await _hrRepository.GetAuditors();
            if (data == null)
                return new AuditorResponse() { Result = AuditorResponseResult.error };
            response.Auditors = data.Select(x => _hrmap.GetAuditor(x));
            response.Result = AuditorResponseResult.success;
            return response;
        }

        public async Task<IEnumerable<CustomerCS>> GetAllCSByLocationCusId(int officeid, int customerid = 0)
        {
            var data = await _hrRepository.GetAllCSByLocationCusId(officeid, customerid);

            if (data == null)
                return null;
            return data.Select(_hrmap.GetCustomerCS);

        }

        //set status updated date(approved,cancelled,rejected)
        private void SetStatusDate(HrLeave leave, Entities.Enums.LeaveStatus statusId)
        {
            switch (statusId)
            {
                case Entities.Enums.LeaveStatus.Approved:
                    leave.ApprovedOn = DateTime.Now;
                    break;
                case Entities.Enums.LeaveStatus.Cancelled:
                    leave.CancelledOn = DateTime.Now;
                    break;
                case Entities.Enums.LeaveStatus.Rejected:
                    leave.RejectedOn = DateTime.Now;
                    break;
            }
        }

        //allow cancel leave after approval
        private bool CheckCancelledAfterApproval(HrLeave leave, int oldStatus)
        {
            if (oldStatus == (int)Entities.Enums.LeaveStatus.Approved)
            {
                double dateDifference = DateTime.Now.Date.Subtract(leave.DateEnd.Date).TotalDays;
                if (dateDifference < 7)
                    return true;
            }
            return false;
        }

        public async Task<LeaveApproveEmail> SetLeaveStatus(int id, Entities.Enums.LeaveStatus statusId, string comment)
        {

            var leave = await _hrRepository.GetLeaveById(id);
            var oldStatus = (Entities.Enums.LeaveStatus)leave.Status;
            bool IsCancelledAfterApproval = false;

            SetStatusDate(leave, statusId);

            var masterConfigs = await _userConfigRepository.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            var baseUrl = _configuration["BaseUrl"];

            if (statusId == Entities.Enums.LeaveStatus.Cancelled)
            {
                if (CheckCancelledAfterApproval(leave, (int)oldStatus))
                    IsCancelledAfterApproval = true;

                if (leave.StaffId != _ApplicationContext.StaffId)
                    return null;

                leave.Status = (int)statusId;

                _hrRepository.Save(leave);

                if (!IsCancelledAfterApproval)
                {
                    // Update task related to this request
                    var task = await _userRepo.GetTask(x => x.TaskTypeId == (int)TaskType.LeaveToApprove && x.LinkId == id
                                && x.UserId == _ApplicationContext.UserId && !x.IsDone);

                    if (task != null)
                    {
                        task.IsDone = true;
                        task.UpdatedBy = _ApplicationContext?.UserId;
                        task.UpdatedOn = DateTime.Now;

                        _userRepo.Save(task);


                    }
                }

                var parentStaffId = (leave.Staff != null && leave.Staff.ParentStaff != null) ? leave.Staff.ParentStaff.Id : 0;

                var parentUserId = await _hrRepository.GetUserIdByStaff(parentStaffId);

                // Add new notification for user request
                var notification = new MidNotification
                {
                    Id = Guid.NewGuid(),
                    IsRead = false,
                    LinkId = leave.Id,
                    UserId = parentUserId,
                    NotifTypeId = (int)NotificationType.LeaveCancelled,
                    CreatedOn = DateTime.Now,
                    EntityId = _filterService.GetCompanyId()
                };

                _hrRepository.Save(notification, false);

                return new LeaveApproveEmail
                {
                    ApplyDate = leave.LeaveApplicationDate == null ? "" : leave.LeaveApplicationDate?.ToString(StandardDateFormat),
                    ApproveOn = leave.ApprovedOn == null ? "" : leave.ApprovedOn?.ToString(StandardDateFormat),
                    EndDate = leave.DateEnd.ToString(StandardDateFormat),
                    LeaveStatus = "Cancelled",
                    ReceipentEmail = leave.Staff?.ParentStaff?.CompanyEmail,
                    ReceipentCCEmail = leave.Staff?.CompanyEmail,
                    StaffName = leave.Staff?.ParentStaff.PersonName,
                    LeaveType = leave.LeaveType?.IdTran.GetTranslation(leave.LeaveType.Description),
                    StartDate = leave.DateBegin.ToString(StandardDateFormat),
                    TotalDays = leave.NumberOfDays,
                    Id = notification.Id,
                    UserId = parentUserId,
                    Url = baseUrl + string.Format(_configuration["UrlLeaveRequest"], leave.Id, entityName),
                    UserName = leave.Staff?.PersonName,
                    IsCancelledAfterApproval = IsCancelledAfterApproval
                };

            }

            if (!_ApplicationContext.RoleList.Contains((int)RoleEnum.Management))
                return null;

            if (leave == null)
                return null;

            if (leave.Staff == null || leave.Staff.ParentStaffId == null || leave.Staff.ParentStaffId.Value != _ApplicationContext.StaffId)
                return null;


            leave.Status = (int)statusId;

            if (!string.IsNullOrEmpty(comment))
            {
                switch (oldStatus)
                {
                    case Entities.Enums.LeaveStatus.Request:
                        leave.Comments1 = comment;
                        break;
                    case Entities.Enums.LeaveStatus.Checked:
                        leave.Comments2 = comment;
                        break;
                }
            }

            _hrRepository.Save(leave);

            try
            {
                // Update task related to this request
                var task = await _userRepo.GetTask(x => x.TaskTypeId == (int)TaskType.LeaveToApprove && x.LinkId == id && x.ReportTo == _ApplicationContext.UserId && !x.IsDone);

                if (task != null)
                {
                    task.IsDone = true;
                    task.UpdatedBy = _ApplicationContext?.UserId;
                    task.UpdatedOn = DateTime.Now;

                    _userRepo.Save(task);

                    // Add new notification for user request
                    var notification = new MidNotification
                    {
                        Id = Guid.NewGuid(),
                        IsRead = false,
                        LinkId = task.LinkId,
                        UserId = task.UserId,
                        NotifTypeId = statusId == Entities.Enums.LeaveStatus.Approved ? (int)NotificationType.LeaveApproved : (int)NotificationType.LeaveRejected,
                        CreatedOn = DateTime.Now,
                        EntityId = _filterService.GetCompanyId()
                    };

                    _hrRepository.Save(notification, false);
                    var lstHrLeaves = new List<HrLeaveStaff>();


                    if (statusId == Entities.Enums.LeaveStatus.Approved && task.User != null && task.User.Staff != null && task.User.Staff.LocationId != null)
                    {
                        //add manager in CC list
                        lstHrLeaves.Add(new HrLeaveStaff
                        {
                            UserIdList = new List<int>(),// no need approved boradcast to manager.
                            StaffId = task.User.Staff.ParentStaff.Id,
                            StaffEmail = task.User?.Staff?.ParentStaff.CompanyEmail,
                            StaffName = task.User?.Staff?.ParentStaff.PersonName
                        });

                        // get Hr Leave List
                        var hrStaffList = await _hrRepository.GetStaffListByRole((int)RoleEnum.LeaveHr, task.User.Staff.LocationId.Value);

                        if (hrStaffList != null && hrStaffList.Any())
                        {
                            var notificationList = new List<MidNotification>();

                            foreach (var item in hrStaffList)
                            {
                                lstHrLeaves.Add(new HrLeaveStaff
                                {
                                    UserIdList = item.ItUserMasters.Select(x => x.Id),
                                    StaffId = item.Id,
                                    StaffEmail = item.CompanyEmail,
                                    StaffName = item.PersonName
                                });

                                foreach (var user in item.ItUserMasters)
                                {
                                    notificationList.Add(new MidNotification
                                    {
                                        Id = Guid.NewGuid(),
                                        IsRead = false,
                                        LinkId = task.LinkId,
                                        NotifTypeId = (int)NotificationType.LeaveApprovedHrLeave,
                                        UserId = user.Id,
                                        CreatedOn = DateTime.Now,
                                        EntityId = _filterService.GetCompanyId()
                                    });
                                }
                            }

                            _hrRepository.SaveList(notificationList, false);
                        }
                    }


                    return new LeaveApproveEmail
                    {
                        ApplyDate = leave.LeaveApplicationDate == null ? "" : leave.LeaveApplicationDate?.ToString(StandardDateFormat),
                        ApproveOn = leave.ApprovedOn == null ? "" : leave.ApprovedOn?.ToString(StandardDateFormat),
                        Comment = leave.Comments1,
                        EndDate = leave.DateEnd.ToString(StandardDateFormat),
                        LeaveStatus = statusId == Entities.Enums.LeaveStatus.Approved ? "Approved" : "Rejected",
                        LeaveType = leave.LeaveType?.IdTran.GetTranslation(leave.LeaveType.Description),
                        ReceipentEmail = task.User?.Staff?.CompanyEmail,
                        StaffName = task.User.Staff?.PersonName,
                        StartDate = leave.DateBegin.ToString(StandardDateFormat),
                        TotalDays = leave.NumberOfDays,
                        UserId = task.UserId,
                        Id = notification.Id,
                        Url = baseUrl + string.Format(_configuration["UrlLeaveRequest"], leave.Id, entityName),
                        LeaveStaffList = lstHrLeaves
                    };
                }

                return null;

            }
            catch (Exception ex)
            {
                // TODO Log error but not block logic
            }

            return null;

        }

        public async Task<StaffGenderResponse> GetGender()
        {
            var response = new StaffGenderResponse();
            var data = await _hrRepository.GetUserGender(_ApplicationContext.StaffId);
            if (data != null)
            {
                response.Gender = data.Gender ?? "";
                response.IsPhotovailable = data.HrPhotos != null && data.HrPhotos.Select(x => x.FileUrl != null).Count() > 0 ? true : false;
                response.Result = StaffGenderResponseResult.success;
            }
            return response;
        }

        public StaffKAMProfileResponse GetHRStaffWithKAMProfiles()
        {
            int? HRProfileID = null;
            StaffKAMProfileResponse response = new StaffKAMProfileResponse();
            HRProfileID = _hrRepository.GetHrProfiles().Select(x => x.Id).SingleOrDefault();

            if (HRProfileID != null)
            {
                var StaffIds = _hrRepository.GetHrStaffProfiles().Where(x => x.ProfileId == HRProfileID).Select(x => x.StaffId).ToArray();
                var StaffList = _hrRepository.GetStaffList().Where(x => StaffIds.Contains(x.Id));
                if (StaffList == null)
                {
                    return new StaffKAMProfileResponse { Result = StaffKAMProfileResult.CannotGetStaffResult };
                }
                else
                {
                    var data = StaffList.Select(x => new StaffKAMLList { Id = x.Id, StaffName = x.PersonName }).ToList();
                    response.StaffList = data;
                    response.Result = StaffKAMProfileResult.Success;
                }
            }

            return response;
        }

        /// <summary>
        /// Get staff details by id
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public async Task<StaffBaseData> GetStaffByStaffId(int staffId)
        {
            var staff = await _hrRepository.GetStaffDataById(staffId);

            if (staff == null)
                return null;

            return staff;
        }

        //get profile list
        public async Task<HRProfileResponse> GetProfileList()
        {
            //ProfileList
            var profiles = await _hrRepository.GetProfileList();

            if (profiles == null || !profiles.Any())
                return new HRProfileResponse { Result = DataSourceResult.CannotGetList };

            return new HRProfileResponse
            {
                ProfileList = profiles.Select(_hrmap.GetProfile).ToArray(),
                Result = DataSourceResult.Success
            };
        }


        //get department list
        public async Task<DataSourceResponse> GetDepartmentList(List<int> deptIdList)
        {
            //ProfileList
            var deptData = await _hrRepository.GetDepartmentList(deptIdList);

            if (deptData == null || !deptData.Any())
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };

            return new DataSourceResponse
            {
                DataSourceList = deptData,
                Result = DataSourceResult.Success
            };
        }
        /// <summary>
        /// get staff list who as inspector
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetStaffDataSource(StaffDataSourceRequest request)
        {
            var response = new DataSourceResponse();

            var data = _hrRepository.GetHRStaffQuery();
            if (!request.ShowAllData)
                data = data.Where(x => x.Active == true);

            //filter by inspector
            data = data.Where(x => x.HrStaffProfiles.Any(y => y.ProfileId == (int)HRProfile.Inspector));

            //get data by location with login access
            if (_ApplicationContext.LocationList != null && _ApplicationContext.LocationList.Any())
            {
                data = data.Where(x => _ApplicationContext.LocationList.Contains(x.LocationId.GetValueOrDefault()));
            }

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.PersonName != null && EF.Functions.Like(x.PersonName, $"%{request.SearchText.Trim()}%"));
            }

            //selected office location filter
            if (request.LocationId > 0)
            {
                data = data.Where(x => x.LocationId == request.LocationId);
            }
            if (request.LocationIdList != null && request.LocationIdList.Any())
            {
                data = data.Where(x => request.LocationIdList.Contains(x.LocationId.GetValueOrDefault()));
            }
            
            //selected office location filter
            if (request.Id > 0)
            {
                data = data.Where(x => x.Id == request.Id);
            }


            if (request.EmployeeType.HasValue && request.EmployeeType > 0)
            {
                data = data.Where(x => x.EmployeeTypeId == request.EmployeeType);
            }

            if (request.EmployeeType.HasValue && request.EmployeeType == (int)EmployeeTypeEnum.OutSource && request.OutSourceCompanyId.HasValue && request.OutSourceCompanyId > 0)
            {
                data = data.Where(x => x.HroutSourceCompanyId == request.OutSourceCompanyId);
            }

            if (request.IdList != null && request.IdList.Any())
            {
                data = data.Where(x => request.IdList.Contains(x.Id));
            }
            //execute the data
            var staffList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

            if (staffList == null || !staffList.Any())
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = staffList.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.PersonName + (request.ShowLocation && !string.IsNullOrEmpty(x.Location?.LocationName) ? "-" + x.Location?.LocationName : "")
                }).OrderBy(x => x.Name);
                response.Result = DataSourceResult.Success;
            }
            return response;
        }
        /// <summary>
        /// get staff details by location ids
        /// </summary>
        /// <param name="locationIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetStaffIdsByProfileAndLocation(IEnumerable<int> locationIds)
        {
            return await _hrRepository.GetStaffIdsByProfileAndLocation(locationIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetStaffNameList()
        {
            return await _hrRepository.GetStaffNameList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetStaffStatusList()
        {
            var data = await _hrRepository.GetStaffStatusList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// get band list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetBandList()
        {
            var data = await _hrRepository.GetBandList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// get social insurance type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetSocialInsuranceTypeList()
        {
            var data = await _hrRepository.GetSocialInsuranceTypeList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// get city list by request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetHukoLocationList(CommonDataSourceRequest request)
        {
            var response = new DataSourceResponse();

            var data = _hrRepository.GetHukoLocationList();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.CityName != null && EF.Functions.Like(x.CityName, $"%{request.SearchText.Trim()}%"));
            }

            if (request.Id > 0)
            {
                data = data.Where(x => x.Id == request.Id);
            }

            if (request.IdList != null && request.IdList.Any())
            {
                data = data.Where(x => request.IdList.Contains(x.Id));
            }

            response.DataSourceList = await data.Skip(request.Skip).Take(request.Take).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.CityName
            }).OrderBy(x => x.Name).AsNoTracking().ToListAsync();

            if (!response.DataSourceList.Any())
                response.Result = DataSourceResult.CannotGetList;
            else
            {
                response.Result = DataSourceResult.Success;
            }
            return response;
        }


        /// <summary>
        /// Get the staff entity data
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public async Task<StaffEntity> GetStaffLocationEntity(int staffId)
        {
            return await _hrRepository.GetStaffLocationEntity(staffId);
        }



        /// <summary>
        /// Get all positions
        /// </summary>
        /// <returns></returns>

        public async Task<StaffSummaryResponse> GetPositions()
        {
            var response = new StaffSummaryResponse();

            //Positions 
            var positions = await _hrRepository.GetPositions();

            if (positions == null || !positions.Any())
                return new StaffSummaryResponse { Result = StaffSummaryResult.CannotGetPositionList };

            response.PositionList = positions.Select(_hrmap.GetPosition).ToArray();

            response.Result = StaffSummaryResult.Success;

            return response;

        }


        public async Task<IEnumerable<string>> GetEmailsByPositionsAndOffices(IEnumerable<int> positions, IEnumerable<int> offices, IEnumerable<int> countryIds)
        {
            return await _hrRepository.GetEmailsByPositionsAndOffices(positions, offices, countryIds);
        }

        public async Task<StaffSearchResponse> GetStaffsByOffice(int idOffice)
        {
            var data = await _hrRepository.GetListAsync<HrStaff>(x => x.LocationId == idOffice && x.Active != null && x.Active.Value);

            if (data == null || !data.Any())
                return new StaffSearchResponse
                {
                    Result = StaffSearchResult.NotFound
                };

            int count = data.Count();

            return new StaffSearchResponse
            {
                Result = StaffSearchResult.Success,
                Data = data.Select(_hrmap.MapStaffItem),
                Index = 1,
                PageCount = count,
                PageSize = count,
                TotalCount = count
            };
        }

        public async Task<DataSourceResponse> GetStaffUserDataSource(UserDataSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _hrRepository.GetStaffUserDataSource();

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                var userList = await data.Skip(request.Skip).Take(request.Take).AsNoTracking().ToListAsync();
                // filter by user ids
                if (request.IdList != null && request.IdList.Any())
                {
                    var staffs = await data.Where(x => request.IdList.Contains(x.Id)).AsNoTracking().ToListAsync();
                    userList.InsertRange(0, staffs);
                }
                if (userList == null || !userList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = userList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Save the HR Outsource company
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveHROutSourceResponse> SaveHROutSourceCompany(SaveHROutSourceRequest request)
        {
            //name is not available 
            if (string.IsNullOrEmpty(request.Name))
                return new SaveHROutSourceResponse() { Result = SaveHROutSourceResult.NameNotAvailable };

            //if duplicate name exists then return name already exists message
            if (await _hrRepository.GetHROutSourceCompanyByName(request))
                return new SaveHROutSourceResponse() { Result = SaveHROutSourceResult.NameAlreadyExists };

            if (request.Id == 0)
            {
                //map the hr outsource company
                var hrOutSourceCompanyEntity = _hrmap.MapHROutSourceCompany(request, _ApplicationContext.UserId, _filterService.GetCompanyId());

                _hrRepository.AddEntity(hrOutSourceCompanyEntity);

                await _hrRepository.Save();

                if (hrOutSourceCompanyEntity.Id > 0)
                    return new SaveHROutSourceResponse() { Result = SaveHROutSourceResult.Success };

            }
            else if (request.Id > 0)
            {
                //get the hr outsource company by id
                var hrOutSourceCompanyEntity = await _hrRepository.GetHROutSourceCompanyById(request.Id);

                //map the edit hr outsource company entity
                hrOutSourceCompanyEntity = _hrmap.MapEditHROutSourceCompany(hrOutSourceCompanyEntity, request, _ApplicationContext.UserId);

                _hrRepository.EditEntity(hrOutSourceCompanyEntity);

                await _hrRepository.Save();

                if (hrOutSourceCompanyEntity.Id > 0)
                    return new SaveHROutSourceResponse() { Result = SaveHROutSourceResult.Success };
            }

            return new SaveHROutSourceResponse() { Result = SaveHROutSourceResult.Failure };
        }

        /// <summary>
        /// Get the virtual hr outsource company list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HROutSourceCompanyListResponse> GetHROutSourceCompanyList(HROutSourceCompanyRequest request)
        {
            var hrOutSourceCompanies = _hrRepository.GetHROutSourceCompanyQuery();

            //apply id filter
            if (request.Id > 0)
            {
                hrOutSourceCompanies = hrOutSourceCompanies.Where(x => x.Id == request.Id);
            }

            //apply the search text filter
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                hrOutSourceCompanies = hrOutSourceCompanies.Where(x => EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
            }

            //execute the hr outsource company filter
            var hrOutSourceCompanyList = await hrOutSourceCompanies.Skip(request.Skip).Take(request.Take).
                                       Select(x => new CommonDataSource()
                                       {
                                           Id = x.Id,
                                           Name = x.Name
                                       }).AsNoTracking().ToListAsync();

            if (hrOutSourceCompanies != null && hrOutSourceCompanies.Any())
                return new HROutSourceCompanyListResponse()
                {
                    hrOutSourceCompanyList = hrOutSourceCompanyList,
                    Result = HROutSourceCompanyResult.Success
                };
            else
                return new HROutSourceCompanyListResponse()
                {
                    Result = HROutSourceCompanyResult.DataNotAvailable
                };

        }

        /// <summary>
        /// Get the HR OutSource Company List
        /// </summary>
        /// <returns></returns>
        public async Task<HROutSourceCompanyListResponse> GetHROutSourceCompanyList()
        {
            var hrOutSourceCompanies = _hrRepository.GetHROutSourceCompanyQuery();

            //execute the hr outsource company filter
            var hrOutSourceCompanyList = await hrOutSourceCompanies.
                                       Select(x => new CommonDataSource()
                                       {
                                           Id = x.Id,
                                           Name = x.Name
                                       }).AsNoTracking().ToListAsync();

            if (hrOutSourceCompanies != null && hrOutSourceCompanies.Any())
                return new HROutSourceCompanyListResponse()
                {
                    hrOutSourceCompanyList = hrOutSourceCompanyList,
                    Result = HROutSourceCompanyResult.Success
                };
            else
                return new HROutSourceCompanyListResponse()
                {
                    Result = HROutSourceCompanyResult.DataNotAvailable
                };

        }

        /// <summary>
        /// Get staff list by hr outsource company ids
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        public async Task<HRStaffResponse> GetStaffListByOutSourceCompanyIds(List<int> companyIds)
        {
            var response = new HRStaffResponse() { Result = HRStaffDetailResult.NotFound };

            if (companyIds != null && companyIds.Any())
            {
                var staffList = await _hrRepository.GetStaffByHROutSourceCompany(companyIds);

                if (staffList.Any())
                {
                    response.StaffList = staffList.Select(x => new HRStaffDetail()
                    {
                        Id = x.Id,
                        StaffName = x.Name
                    }).ToList();
                    response.Result = HRStaffDetailResult.Success;
                }
            }
            else
            {
                return await _referencemanager.GetOutSourceStaffList();
            }
            return response;
        }

        /// <summary>
        /// Check KAM Staff
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetKAMStaff()
        {
            int? HRProfileID = null;
            StaffKAMProfileResponse response = new StaffKAMProfileResponse();
            HRProfileID = _hrRepository.GetHrProfiles().Select(x => x.Id).SingleOrDefault();
            List<int> staffIds = null;
            if (HRProfileID != null)
            {
                staffIds = _hrRepository.GetHrStaffProfiles().Where(x => x.ProfileId == HRProfileID).Select(x => x.StaffId).ToList();
            }
            else
            {
                staffIds = new List<int> { 0 };
            }
            var data = await _hrRepository.GetKAMStaff(staffIds);
            if (data != null && data.Any())
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }
            return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.CannotGetList };

        }

        /// <summary>
        /// Get Department Ids By Name
        /// </summary>
        /// <param name="departname"></param>
        /// <returns></returns>
        public async Task<List<int>> GetDepartmentIdsByName(string departname)
        {
            return await _hrRepository.GetDepartmentIdsByName(departname);

        }
        /// <summary>
        /// Check Staff by departmentIds
        /// </summary>
        /// <param name="departmentIds"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetSalesIncharge(List<int?> departmentIds)
        {
            var data = await _hrRepository.GetStaffByDepartmentIds(departmentIds);
            if (data != null && data.Any())
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }
            return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.CannotGetList };
        }

        public async Task<HRPayrollCompanyListResponse> GetHRPayrollCompanyList()
        {
            var hrPayrollCompanies = await _hrRepository.GetHRPayrollCompanyQuery();

            if (hrPayrollCompanies == null && !hrPayrollCompanies.Any())
            {
                return new HRPayrollCompanyListResponse() { Result = HRPayrollCompanyListResult.DataNotAvailable };
            }

            //execute the hr payroll company filter
            var hrOutSourceCompanyList = hrPayrollCompanies.
                                   Select(x => new CommonDataSource()
                                   {
                                       Id = x.Id,
                                       Name = x.CompanyName
                                   }).ToList();

            return new HRPayrollCompanyListResponse()
            {
                HRPayrollCompanyList = hrOutSourceCompanyList,
                Result = HRPayrollCompanyListResult.Success
            };
        }

        private static string RandomString(int length)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            var chars = Enumerable.Range(0, length)
                .Select(x => Pool[random.Next(0, Pool.Length)]);
            return new string(chars.ToArray());
        }

        /// <summary>
        /// get fb qc id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetFBQCIdList(int bookingId)
        {
            return await _hrRepository.GetFBQCIdList(bookingId);
        }
        /// <summary>
        /// get fb cs id 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetFBCSIds(int bookingId)
        {
            return await _hrRepository.GetFBCSIds(bookingId);
        }
    }
}
