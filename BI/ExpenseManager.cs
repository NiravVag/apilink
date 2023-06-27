using BI.Maps;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.DataAccess;
using DTO.Expense;
using DTO.File;
using DTO.HumanResource;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace BI
{
    public class ExpenseManager : ApiCommonData, IExpenseManager
    {
        private readonly ILocationManager _locManager = null;
        private readonly IExpenseRepository _expenseRepository = null;
        private readonly IHumanResourceManager _hrManager = null;
        private readonly IFileManager _fileManager = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly IUserRightsManager _userManager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ILogger<ExpenseManager> _logger = null;
        private readonly IReferenceManager _referencemanager = null;
        private readonly IOfficeLocationRepository _officeRepo = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IHumanResourceRepository _humanResourceRepository = null;
        private readonly IInspectionBookingManager _inspectionBookingManager = null;
        private readonly IAuditManager _auditManager = null;
        private readonly ExpenseMap _expensemap = null;
        private IDictionary<ExpenseClaimStatus, Func<int, Task<SetExpenseStatusResponse>>> _dictStatuses = null;

        public ExpenseManager(ILocationManager locManager, IExpenseRepository expenseRepository,
            IHumanResourceManager hrManager, IFileManager fileManager,
            IOfficeLocationManager office, IUserRightsManager userManager, IAPIUserContext applicationContext, ILogger<ExpenseManager> logger, IReferenceManager referencemanager,
            IOfficeLocationRepository officeRepo, ITenantProvider filterService, IHumanResourceRepository humanResourceRepository,
            IInspectionBookingManager inspectionBookingManager, IAuditManager auditManager)
        {
            _locManager = locManager;
            _expenseRepository = expenseRepository;
            _hrManager = hrManager;
            _fileManager = fileManager;
            _office = office;
            _userManager = userManager;
            _ApplicationContext = applicationContext;
            _logger = logger;
            _referencemanager = referencemanager;
            _officeRepo = officeRepo;
            _expensemap = new ExpenseMap();
            _filterService = filterService;
            _humanResourceRepository = humanResourceRepository;
            _inspectionBookingManager = inspectionBookingManager;
            _auditManager = auditManager;

            _dictStatuses = new Dictionary<ExpenseClaimStatus, Func<int, Task<SetExpenseStatusResponse>>>() {
                                    { ExpenseClaimStatus.Checked, ToCheckExpense },
                                    { ExpenseClaimStatus.Approved, ToApproveExpense },
                                    { ExpenseClaimStatus.Paid, ToPayExpense },
                                    { ExpenseClaimStatus.Cancelled, ToCancelExpense }
                               };
        }

        public async Task<ClaimCitiesResponse> GetClaimCities(string term)
        {
            var citiesresponse = await _locManager.GetCitiesByTerm(term);

            return new ClaimCitiesResponse { Items = citiesresponse.Data };
        }

        public async Task<ExpenseClaimResponse> GetExpenseClaim(string name, int? id)
        {
            var response = new ExpenseClaimResponse();

            if (id != null)
            {
                var expenses = await _expenseRepository.GetExpenseClaim(id.Value);
                var expenseClaim = expenses.Item1;

                if (expenseClaim == null)
                    return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotFindCurrentExpenseClaim };

                response.expenseBookingDetailAccess = setExpenseBookingAccess(false, false, false, false);

                //for accounting & manager 
                if (expenseClaim.StaffId != _ApplicationContext.StaffId)
                {
                    bool Accountingaccess = false;
                    if (_ApplicationContext.RoleList.Contains((int)RoleEnum.ExpenseClaim))
                    {
                        if (expenseClaim.Staff == null || expenseClaim.Staff.LocationId == null)
                            return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotFindLocation };

                        if (_ApplicationContext.LocationList != null && !_ApplicationContext.LocationList.Any(x => expenseClaim.Staff.LocationId.Value == x))
                            return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotShowThisExpense };

                        if (expenseClaim.ClaimTypeId != (int)ClaimTypeEnum.NonInspection)
                            response.expenseBookingDetailAccess = setExpenseBookingAccess(false, false, true, false);

                        response.CanEdit = expenseClaim.StatusId == (int)ExpenseClaimStatus.Pending;
                        response.CanCheck = true;
                        Accountingaccess = true;
                    }
                    //Expense claim edit scenario for outsource accounting user role
                    if (_ApplicationContext.RoleList.Contains((int)RoleEnum.OutsourceAccounting))
                    {
                        response.expenseBookingDetailAccess = setExpenseBookingAccess(false, false, true, false);
                        response.CanEdit = expenseClaim.StatusId == (int)ExpenseClaimStatus.Pending || expenseClaim.StatusId == (int)ExpenseClaimStatus.Rejected;
                        response.CanCheck = false;
                        response.CanApprove = false;
                    }
                    //for manager access
                    else if (_ApplicationContext.RoleList.Contains((int)RoleEnum.Management))
                    {
                        var staffList = await _hrManager.GetStaffListByParentId(_ApplicationContext.StaffId);

                        if (staffList == null || !staffList.Any(x => x.Id == expenseClaim.StaffId))// check the manager access
                        {
                            if (!Accountingaccess) //if no manager access & no accounting access , don't show the expense details
                            {
                                return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotShowThisExpense };
                            }
                            else // if no manager access but have accouting access , then show expense details without manager access
                            {
                                response.CanApprove = false;
                            }
                        }
                        else if (staffList != null && staffList.Any(x => x.Id == expenseClaim.StaffId))
                        {
                            if (expenseClaim.ClaimTypeId != (int)ClaimTypeEnum.NonInspection)
                                response.expenseBookingDetailAccess = setExpenseBookingAccess(false, false, true, false);
                            response.CanApprove = true;
                        }

                    }

                }
                // for accounting team checking own expense
                else if (expenseClaim.StaffId == _ApplicationContext.StaffId && _ApplicationContext.RoleList.Contains((int)RoleEnum.ExpenseClaim))
                {
                    response.CanEdit = expenseClaim.StatusId == (int)ExpenseClaimStatus.Pending || expenseClaim.StatusId == (int)ExpenseClaimStatus.Rejected;
                    response.CanCheck = true;
                    response.CanApprove = false;
                }
                // normal user
                else
                {
                    if (expenseClaim.StaffId == _ApplicationContext.StaffId && (_ApplicationContext.UserProfileList.Contains((int)HRProfile.Auditor)
                        || _ApplicationContext.UserProfileList.Contains((int)HRProfile.Inspector)) && expenseClaim.ClaimTypeId != (int)ClaimTypeEnum.NonInspection
                        && (expenseClaim.StatusId == (int)ExpenseClaimStatus.Pending || expenseClaim.StatusId == (int)ExpenseClaimStatus.Rejected))
                    {
                        response.expenseBookingDetailAccess = setExpenseBookingAccess(true, true, true, false);
                    }
                    else if (expenseClaim.StaffId == _ApplicationContext.StaffId && (_ApplicationContext.UserProfileList.Contains((int)HRProfile.Auditor)
                        || _ApplicationContext.UserProfileList.Contains((int)HRProfile.Inspector)) && expenseClaim.ClaimTypeId != (int)ClaimTypeEnum.NonInspection
                        && (expenseClaim.StatusId != (int)ExpenseClaimStatus.Pending || expenseClaim.StatusId != (int)ExpenseClaimStatus.Rejected))
                    {
                        response.expenseBookingDetailAccess = setExpenseBookingAccess(false, false, true, false);
                    }
                    response.CanEdit = expenseClaim.StatusId == (int)ExpenseClaimStatus.Pending || expenseClaim.StatusId == (int)ExpenseClaimStatus.Rejected;
                    response.CanCheck = false;
                    response.CanApprove = false;
                }

                var travelPortList = await GetExpenseTravelQCPort(expenseClaim.EcExpensesClaimDetais);

                response.ExpenseClaim = _expensemap.GetExpenseClaim(expenseClaim, expenses.Item2, (x) => _fileManager.GetMimeType(x),
                    travelPortList);

                //var notification = await _userManager.EditNotification(x => !x.IsRead && x.LinkId == id.Value && x.UserId == _ApplicationContext.UserId, x => { x.IsRead = true; });
            }
            else
            {
                response.ExpenseClaim = new ExpenseClaim();
                response.CanEdit = true;
                response.CanCheck = false;
                response.CanApprove = false;
                response.expenseBookingDetailAccess = setExpenseBookingAccess(false, false, false, false);

                var currentStaff = await _hrManager.GetStaffByUserId(_ApplicationContext.UserId);

                // Current staff name
                if (currentStaff == null)
                    return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotFindCurrentStaff };

                // get current HR 

                response.ExpenseClaim.Name = currentStaff.StaffName;
                response.ExpenseClaim.StaffId = currentStaff.Id;
                response.ExpenseClaim.StaffEmail = currentStaff.Email;

                if (currentStaff.LocationId == null)
                    return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotFindLocation };

                // Location
                response.ExpenseClaim.LocationId = currentStaff.LocationId.Value;
                response.ExpenseClaim.LocationName = currentStaff.LocationName;
                response.ExpenseClaim.CountryId = currentStaff.CountryId;

                //Currency
                if (currentStaff.CurrencyId == null)
                    return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotFindCurrencies };

                response.ExpenseClaim.CurrencyId = currentStaff.CurrencyId.Value;
                response.ExpenseClaim.CurrencyName = currentStaff.CurrencyName;

                response.ExpenseClaim.ClaimDate = DateTime.Now.GetCustomDate();
                response.ExpenseClaim.ClaimNo = $"{(currentStaff.StaffName.Length >= 4 ? currentStaff.StaffName.Substring(0, 4) : currentStaff.StaffName)}-{currentStaff.Id}-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}";

                if (_ApplicationContext.UserProfileList.Contains((int)HRProfile.Auditor) || _ApplicationContext.UserProfileList.Contains((int)HRProfile.Inspector))
                {
                    response.expenseBookingDetailAccess = setExpenseBookingAccess(false, false, false, true);
                }

                response.ExpenseClaim.UserTypeId = currentStaff.UserTypeId;

            }

            //get the hroutsource company details by user id
            var hrOutSourceCompany = await _humanResourceRepository.GetHROutSourceCompanyByUserId(_ApplicationContext.UserId);

            if (hrOutSourceCompany != null)
                response.ExpenseClaim.OutSourceCompanyName = hrOutSourceCompany.Name;

            // CountryList
            response.CountryList = _locManager.GetCountries();

            if (response.CountryList == null || !response.CountryList.Any())
                return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotFindCountries };

            //CurrencyList 
            response.CurrencyList = _referencemanager.GetCurrencies();

            if (!response.CurrencyList.Any())
                return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotFindCurrencies };

            //get the expense type list
            response.ExpenseTypeList = await GetExpenseTypes();

            if (!response.ExpenseTypeList.Any())
                return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotFindExpenseTypes };


            //Get Status List By roles
            var data = await _expenseRepository.GetStatusList();

            if (data == null || !data.Any())
                return new ExpenseClaimResponse { Result = ExpenseClaimResult.CannotFindStatsList };

            response.StatusList = data.Select(_expensemap.GetStatus);

            response.Result = ExpenseClaimResult.Success;

            return response;
        }

        /// <summary>
        /// Get the expense type list
        /// </summary>
        /// <returns></returns>
        private async Task<List<ExpenseTypeSource>> GetExpenseTypes()
        {
            var expenseTypeList = new List<ExpenseTypeSource>();
            //get the expense types queryable
            var expenseTypesQuery = _expenseRepository.GetExpenseTypes();

            //if it is outsource user then get outsource expense types
            if (_ApplicationContext.UserType == UserTypeEnum.OutSource)
                expenseTypesQuery = expenseTypesQuery.Where(x => x.IsOutsource.HasValue
                && x.IsOutsource.Value);
            //if it is permanent user then get permanent expense types
            else if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
                expenseTypesQuery = expenseTypesQuery.Where(x => x.IsPermanent.HasValue
                && x.IsPermanent.Value);
            //execute and get the expense type list
            expenseTypeList = await expenseTypesQuery.Select(x => new ExpenseTypeSource()
            {
                Id = x.Id,
                Label = x.TypeTransId.GetTranslation(x.Description),
                IsTravel = x.IsTravel != null ? x.IsTravel.Value : false
            }).AsNoTracking().ToListAsync();
            return expenseTypeList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expenseClaimDetails"></param>
        /// <returns></returns>
        private async Task<List<ExpenseQCPort>> GetExpenseTravelQCPort(ICollection<EcExpensesClaimDetai> expenseClaimDetails)
        {
            var qcTravelExpenseIdList = expenseClaimDetails.Where(x => x.Active.Value).Select(x => x.QcTravelExpenseId).Distinct().ToList();

            var travelPortList = await _expenseRepository.GetExpenseTravelPortList(qcTravelExpenseIdList);

            return travelPortList;
        }

        public async Task<FileResponse> GetFile(int id)
        {
            var file = await _expenseRepository.GetFile(id);

            if (file == null)
                return new FileResponse { Result = FileResult.NotFound };

            return new FileResponse
            {
                Content = file.File,
                MimeType = _fileManager.GetMimeType(Path.GetExtension(file.FullFileName)),
                Result = FileResult.Success
            };
        }

        /// <summary>
        /// Save the expense claim data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveExpenseClaimResponse> SaveExpenseClaim(ExpenseClaim request)
        {
            var response = new SaveExpenseClaimResponse();

            //check if request is null and staffid is not zero
            if (request != null && request.StaffId != 0)
            {
                //get the entity of the location mapped with the given staff
                var staffEntity = await _hrManager.GetStaffLocationEntity(request.StaffId);
                //if location entity id is null (which means location filtered with globally entity id) then send the entitynotmatched status
                if (staffEntity != null && (staffEntity.entityId == null || staffEntity.entityId != _filterService.GetCompanyId()))
                {
                    response.Result = SaveExpenseClaimResult.StaffEntityNotMatched;
                }
                //if location entity matches with the global entity then do the expense claim
                else if (staffEntity != null && staffEntity.entityId == _filterService.GetCompanyId())
                {
                    if (request.Id == 0)
                        response = await AddExpenseClaim(request);
                    else
                        response = await UpdateExpenseClaim(request);
                }

            }

            return response;
        }

        public async Task<SaveExpenseClaimResponse> AddOutSourceQCExpenseClaim(ExpenseClaim request)
        {
            var response = new SaveExpenseClaimResponse();

            //Get the qc list from the request
            var qcList = request.ExpenseList.Where(x => x.QcId != null).Select(x => x.QcId.GetValueOrDefault()).Distinct().ToList();

            List<EcExpencesClaim> expenseClaimList = new List<EcExpencesClaim>();

            //loop through each qc
            foreach (var qcId in qcList)
            {
                //get the expense details for the qc
                var qcExpenseList = request.ExpenseList.Where(x => x.QcId == qcId).ToList();

                var qcName = qcExpenseList.Select(x => x.QcName).FirstOrDefault();

                //generate the claim no dynamically
                var claimNo = $"{(qcName.Length >= 4 ? qcName.Substring(0, 4) : qcName)}-{qcId}-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}";

                var expenseClaim = AddNewExpenseClaimForOutsourceQc(request, qcId, claimNo);

                //add the expense details
                AddExpenseDetails(qcExpenseList, expenseClaim, request.ClaimTypeId);

                expenseClaimList.Add(expenseClaim);
            }

            _expenseRepository.SaveList(expenseClaimList, false);

            await AddOutSourceQcNotification(request, expenseClaimList);

            // get all users with ClaimAccess and have office control current office control de notify them through email
            var users = await _userManager.GetUserListByRole((int)RoleEnum.ExpenseClaimNotification, _ApplicationContext.LocationId);

            //take the expense ids
            var expenseIds = expenseClaimList.Select(x => x.Id).Distinct().ToList();

            //get the expense details for email
            var expenseClaimDetails = await _expenseRepository.GetExpenseClaimListForEmail(expenseIds);

            if (expenseClaimDetails.Any())
            {
                response.Result = SaveExpenseClaimResult.Success;
                response.ExpenseClaimList = expenseClaimDetails;
                response.UserList = users;
            }


            return response;

        }

        /// <summary>
        /// manage the EcExpencesClaim object for the outsource qc
        /// </summary>
        /// <param name="request"></param>
        /// <param name="qcId"></param>
        /// <param name="claimNo"></param>
        /// <returns></returns>
        private EcExpencesClaim AddNewExpenseClaimForOutsourceQc(ExpenseClaim request, int qcId, string claimNo)
        {
            //create the expense claim
            return new EcExpencesClaim
            {
                LocationId = request.LocationId,
                StaffId = qcId,
                Active = true,
                CountryId = request.CountryId,
                CreatedDate = DateTime.Now,
                ExpensePurpose = request.ExpensePuropose?.Trim(),
                ClaimDate = DateTime.Now,
                ClaimNo = claimNo,
                StatusId = (int)ExpenseClaimStatus.Pending,
                PaymentTypeId = 1,
                EntityId = _filterService.GetCompanyId(),
                ClaimTypeId = request.ClaimTypeId
            };
        }

        /// <summary>
        /// Add notification for the outsource qc claims 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="expenseClaimList"></param>
        /// <returns></returns>
        private async Task AddOutSourceQcNotification(ExpenseClaim request, List<EcExpencesClaim> expenseClaimList)
        {
            //get the user list for notification by role and office
            var userAccessFilter = new UserAccess
            {
                OfficeId = request.LocationId,
                RoleId = (int)RoleEnum.ExpenseClaimNotification
            };

            var userListByRoleAccess = await _userManager.GetUserListByRoleOffice(userAccessFilter);

            foreach (var expenseClaim in expenseClaimList)
            {
                // Add new Task
                await _userManager.AddTask(TaskType.ExpenseToCheck, expenseClaim.Id, userListByRoleAccess.Select(x => x.Id));
            }
        }

        private async Task<SetExpenseStatusResponse> ToApproveExpense(int id)
        {

            // Get expense
            var expenses = await _expenseRepository.GetExpenseClaim(id);
            var expenseClaim = expenses.Item1;

            bool isAutoExpense = expenseClaim.EcExpensesClaimDetais.Any(x => x.IsAutoExpense.GetValueOrDefault());

            //Check role
            if (_ApplicationContext.RoleList == null || (!isAutoExpense && !_ApplicationContext.RoleList.Contains((int)RoleEnum.Management)))
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            if (expenseClaim == null)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.ExpenseNotFound };

            //check staff
            if (expenseClaim.Staff == null || expenseClaim.Staff.ParentStaffId == null)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };


            if (!isAutoExpense && expenseClaim.Staff.ParentStaffId.Value != _ApplicationContext.StaffId)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            //Check old status must be Checked
            if (!isAutoExpense && expenseClaim.StatusId != (int)ExpenseClaimStatus.Checked)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            //check current status when approve and the previous status should be pending
            if (isAutoExpense && expenseClaim.StatusId != (int)ExpenseClaimStatus.Pending)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            //Update status 
            bool result = await _expenseRepository.SetStatus(id, (int)ExpenseClaimStatus.Approved);

            if (!result)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.CannotUpdateStatus };

            var response = new SetExpenseStatusResponse { Result = SetExpenseStatusResult.Success };

            // Get expense
            expenses = await _expenseRepository.GetExpenseClaim(id);
            expenseClaim = expenses.Item1;

            var travelPortList = await GetExpenseTravelQCPort(expenseClaim.EcExpensesClaimDetais);

            response.Data = _expensemap.GetExpenseClaim(expenseClaim, expenses.Item2, (x) => _fileManager.GetMimeType(x), travelPortList);

            // Update task
            var task = await _userManager.EditTask(x => x.LinkId == id
                        && x.ReportTo == _ApplicationContext.UserId
                        && !x.IsDone,
                        (x) =>
                        {
                            x.IsDone = true;
                            x.UpdatedBy = _ApplicationContext?.UserId;
                            x.UpdatedOn = DateTime.Now;
                        });

            // Add new notification for user request
            if (expenseClaim.Staff != null && expenseClaim.Staff.ItUserMasters != null && expenseClaim.Staff.ItUserMasters.Any())
            {
                var notification = new MidNotification
                {
                    Id = Guid.NewGuid(),
                    IsRead = false,
                    LinkId = expenseClaim.Id,
                    UserId = expenseClaim.Staff.ItUserMasters.FirstOrDefault() != null ? expenseClaim.Staff.ItUserMasters.FirstOrDefault().Id : 0,
                    NotifTypeId = (int)NotificationType.ExpenseApproved,
                    CreatedOn = DateTime.Now,
                    EntityId = _filterService.GetCompanyId()
                };

                _expenseRepository.Save(notification, false);

                response.UserIds = new List<int>() { expenseClaim.Staff.ItUserMasters.FirstOrDefault() != null ?
                                expenseClaim.Staff.ItUserMasters.FirstOrDefault().Id : 0};
            }

            var userAccessFilter = new UserAccess
            {
                OfficeId = expenseClaim.LocationId,
                RoleId = (int)RoleEnum.ExpenseClaimNotification
            };

            var userListByRoleAccess = await _userManager.GetUserListByRoleOffice(userAccessFilter);

            await _userManager.AddTask(TaskType.ExpenseToPay, expenseClaim.Id, userListByRoleAccess.Select(x => x.Id));

            // get users to brodcast task
            response.ClaimUserList = await _userManager.GetUserListByRole((int)RoleEnum.ExpenseClaimNotification, expenseClaim.Staff.LocationId.Value);

            return response;
        }

        private async Task<SetExpenseStatusResponse> ToCheckExpense(int id)
        {
            var isDone = false;

            //Check role : claimaccess
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Contains((int)RoleEnum.ExpenseClaim))
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            if (_ApplicationContext.LocationList == null || !_ApplicationContext.LocationList.Any())
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            // Get expense
            var expenses = await _expenseRepository.GetExpenseClaim(id);
            var expenseClaim = expenses.Item1;

            if (expenseClaim == null)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.ExpenseNotFound };

            //check location
            if (expenseClaim.Staff == null || expenseClaim.Staff.LocationId == null)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            if (!_ApplicationContext.LocationList.Contains(expenseClaim.Staff.LocationId.Value))
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            // Check old status ; must be pending 
            if (expenseClaim.StatusId != (int)ExpenseClaimStatus.Pending)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            // update  Status
            bool result = await _expenseRepository.SetStatus(id, (int)ExpenseClaimStatus.Checked);

            if (!result)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.CannotUpdateStatus };

            var response = new SetExpenseStatusResponse { Result = SetExpenseStatusResult.Success };

            // Get expense
            expenses = await _expenseRepository.GetExpenseClaim(id);
            expenseClaim = expenses.Item1;

            var travelPortList = await GetExpenseTravelQCPort(expenseClaim.EcExpensesClaimDetais);

            response.Data = _expensemap.GetExpenseClaim(expenseClaim, expenses.Item2, (x) => _fileManager.GetMimeType(x), travelPortList);

            // Update task
            await _userManager.UpdateTask(id, new[] { (int)TaskType.ExpenseToCheck }, isDone);

            // Add new notification for user request
            if (expenseClaim.Staff != null && expenseClaim.Staff.ItUserMasters != null && expenseClaim.Staff.ItUserMasters.Any())
            {
                var notification = new MidNotification
                {
                    Id = Guid.NewGuid(),
                    IsRead = false,
                    LinkId = expenseClaim.Id,
                    UserId = expenseClaim.Staff.ItUserMasters.FirstOrDefault() != null ? expenseClaim.Staff.ItUserMasters.FirstOrDefault().Id : 0,
                    NotifTypeId = (int)NotificationType.ExpenseChecked,
                    CreatedOn = DateTime.Now,
                    EntityId = _filterService.GetCompanyId()
                };

                _expenseRepository.Save(notification, false);

                response.UserIds = new List<int>() { expenseClaim.Staff.ItUserMasters.FirstOrDefault() != null ?
                                expenseClaim.Staff.ItUserMasters.FirstOrDefault().Id : 0};
            }

            // Add Task  for  Report to
            var staff = _hrManager.GetBasicStaff(expenseClaim.StaffId);

            if (staff.ParentStaff != null && staff.ParentStaff.ItUserMasters != null && staff.ParentStaff.ItUserMasters.Any())
            {
                // Add Task 
                var taskApprove = new MidTask
                {
                    Id = Guid.NewGuid(),
                    IsDone = false,
                    LinkId = expenseClaim.Id,
                    UserId = expenseClaim.Staff.ItUserMasters.FirstOrDefault() != null ? expenseClaim.Staff.ItUserMasters.FirstOrDefault().Id : 0,
                    ReportTo = staff.ParentStaff.ItUserMasters.FirstOrDefault() != null ? staff.ParentStaff.ItUserMasters.FirstOrDefault().Id : 0, //manager
                    TaskTypeId = (int)TaskType.ExpenseToApprove,
                    CreatedBy = _ApplicationContext?.UserId,
                    CreatedOn = DateTime.Now,
                    EntityId = _filterService.GetCompanyId()
                };

                _expenseRepository.Save(taskApprove, false);

                response.ManagerUserId = staff.ParentStaff.ItUserMasters.FirstOrDefault().Id;
                response.ManagerName = staff.ParentStaff.PersonName;
                response.ManagerEmail = staff.ParentStaff.CompanyEmail;
            }

            return response;
        }

        private async Task<SetExpenseStatusResponse> ToPayExpense(int id)
        {
            var isDone = false;

            //Check role : claimaccess
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Contains((int)RoleEnum.ExpenseClaim))
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            if (_ApplicationContext.LocationList == null || !_ApplicationContext.LocationList.Any())
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            // Get expense
            var expenses = await _expenseRepository.GetExpenseClaim(id);
            var expenseClaim = expenses.Item1;

            if (expenseClaim == null)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.ExpenseNotFound };

            //check location
            if (expenseClaim.Staff == null || expenseClaim.Staff.LocationId == null)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            if (!_ApplicationContext.LocationList.Contains(expenseClaim.Staff.LocationId.Value))
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            //Check Old status must be   APproved
            if (expenseClaim.StatusId != (int)ExpenseClaimStatus.Approved)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            // update  Status
            bool result = await _expenseRepository.SetStatus(id, (int)ExpenseClaimStatus.Paid);

            if (!result)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.CannotUpdateStatus };

            var response = new SetExpenseStatusResponse { Result = SetExpenseStatusResult.Success };

            // Get expense
            expenses = await _expenseRepository.GetExpenseClaim(id);
            expenseClaim = expenses.Item1;

            var travelPortList = await GetExpenseTravelQCPort(expenseClaim.EcExpensesClaimDetais);

            response.Data = _expensemap.GetExpenseClaim(expenseClaim, expenses.Item2, (x) => _fileManager.GetMimeType(x), travelPortList);

            // Update task

            await _userManager.UpdateTask(id, new[] { (int)TaskType.ExpenseToPay }, isDone);

            // Add new notification for user request
            if (expenseClaim.Staff != null && expenseClaim.Staff.ItUserMasters != null && expenseClaim.Staff.ItUserMasters.Any())
            {
                var notification = new MidNotification
                {
                    Id = Guid.NewGuid(),
                    IsRead = false,
                    LinkId = expenseClaim.Id,
                    UserId = expenseClaim.Staff.ItUserMasters.FirstOrDefault() != null ? expenseClaim.Staff.ItUserMasters.FirstOrDefault().Id : 0,
                    NotifTypeId = (int)NotificationType.ExpensePaid,
                    CreatedOn = DateTime.Now,
                    EntityId = _filterService.GetCompanyId()
                };

                _expenseRepository.Save(notification, false);

                response.UserIds = new List<int>() { expenseClaim.Staff.ItUserMasters.FirstOrDefault() != null ?
                                expenseClaim.Staff.ItUserMasters.FirstOrDefault().Id : 0};
            }

            response.ClaimUserList = await _userManager.GetUserListByRole((int)RoleEnum.ExpenseClaimNotification, expenseClaim.Staff.LocationId.Value);

            return response;
        }

        private async Task<SetExpenseStatusResponse> ToCancelExpense(int id)
        {
            var isDone = false;

            // Get expense
            var expenses = await _expenseRepository.GetExpenseClaim(id);
            var expenseClaim = expenses.Item1;

            if (expenseClaim == null)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.ExpenseNotFound };

            //check staff
            if (expenseClaim.StaffId != _ApplicationContext.StaffId)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            // Check old status: must be Pending or Rejected to cancel
            if (expenseClaim.StatusId != (int)ExpenseClaimStatus.Pending && expenseClaim.StatusId != (int)ExpenseClaimStatus.Rejected)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.NoAccess };

            bool result = await _expenseRepository.SetStatus(id, (int)ExpenseClaimStatus.Cancelled);

            if (!result)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.CannotUpdateStatus };

            // Get expense
            expenses = await _expenseRepository.GetExpenseClaim(id);
            expenseClaim = expenses.Item1;

            var response = new SetExpenseStatusResponse { Result = SetExpenseStatusResult.Success };

            var travelPortList = await GetExpenseTravelQCPort(expenseClaim.EcExpensesClaimDetais);

            response.Data = _expensemap.GetExpenseClaim(expenseClaim, expenses.Item2, (x) => _fileManager.GetMimeType(x), travelPortList);

            // Update task
            await _userManager.UpdateTask(id, new[] { (int)TaskType.ExpenseToCheck, (int)TaskType.ExpenseToPay, (int)TaskType.ExpenseToApprove }, isDone);


            response.ClaimUserList = await _userManager.GetUserListByRole((int)RoleEnum.ExpenseClaimNotification, _ApplicationContext.LocationId);

            return response;
        }

        public async Task<SetExpenseStatusResponse> SetExpenseStatus(int id, int statusId)
        {
            if (_dictStatuses.TryGetValue((ExpenseClaimStatus)statusId, out Func<int, Task<SetExpenseStatusResponse>> func))
                return await func(id);

            return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.CannotUpdateStatus };
        }

        public async Task<SetExpenseStatusResponse> Reject(int id, string comment)
        {
            var isDone = false;

            bool result = await _expenseRepository.Reject(id, comment);

            if (!result)
                return new SetExpenseStatusResponse { Result = SetExpenseStatusResult.CannotUpdateStatus };

            var response = new SetExpenseStatusResponse { Result = SetExpenseStatusResult.Success };

            try
            {
                var gettasks = await _userManager.UpdateTask(id, new[] { (int)TaskType.ExpenseToCheck, (int)TaskType.ExpenseToPay, (int)TaskType.ExpenseToApprove }, isDone);

                if (gettasks != null && gettasks.Any())
                {
                    foreach (var userId in gettasks.Where(x => x.UserId > 0).Select(x => x.UserId).Distinct())
                    {
                        var notification = new MidNotification
                        {
                            Id = Guid.NewGuid(),
                            IsRead = false,
                            LinkId = id,
                            UserId = userId,
                            NotifTypeId = (int)NotificationType.ExpenseRejected,
                            CreatedOn = DateTime.Now,
                            EntityId = _filterService.GetCompanyId()
                        };
                        _expenseRepository.Save(notification, false);
                    }
                    response.UserIds = gettasks.Select(x => x.UserId).Distinct();
                }

                // Update task 
                var expenseResponse = await GetExpenseClaim("", id);

                if (expenseResponse.Result == ExpenseClaimResult.Success)
                    response.Data = expenseResponse.ExpenseClaim;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return response;
        }

        public async Task<ExpenseClaimSummaryResponse> GetExpenseSummary()
        {
            var response = new ExpenseClaimSummaryResponse();
            List<CommonDataSource> hrOutSourceCompanyList = new List<CommonDataSource>();

            // Get locations 
            response.LocationList = _office.GetOffices();

            if (response.LocationList == null || !response.LocationList.Any())
                return new ExpenseClaimSummaryResponse { Result = ExpenseClaimSummaryResult.CannotFindLocationList };

            //Get Employee list
            var employeeList = new List<StaffInfo>() { await _hrManager.GetStaffById(_ApplicationContext.StaffId) };

            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.ExpenseClaim))
            {
                var staffinfo = await _hrManager.GetInternalStaffListByLocationList((int)EmployeeTypeEnum.Permanent);
                if (staffinfo != null)
                    employeeList.AddRange(staffinfo);
            }

            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.Management))
            {
                var staffinfo = await _hrManager.GetStaffListByParentId(_ApplicationContext.StaffId);
                if (staffinfo != null)
                    employeeList.AddRange(staffinfo);
            }

            if (employeeList == null || !employeeList.Any())
                return new ExpenseClaimSummaryResponse { Result = ExpenseClaimSummaryResult.CannotFindEmployeeList };

            //Get the hr outsource company details in which user 'OutSourceAccounting' Role belongs to
            //and take the staff infos for the company id
            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.OutsourceAccounting))
            {
                //get the hr outsource company 
                var hrOutSourceCompany = await _humanResourceRepository.GetHROutSourceCompanyByUserId(_ApplicationContext.UserId);
                if (hrOutSourceCompany != null)
                {
                    //add to the datasource list
                    hrOutSourceCompanyList.Add(hrOutSourceCompany);
                    response.HrOutSourceCompanyList = hrOutSourceCompanyList;
                    //get the staff info which belongs to the hr outsource company
                    var staffinfo = await _humanResourceRepository.GetStaffByHROutSourceCompany(new[] { hrOutSourceCompany.Id }.ToList());
                    if (staffinfo != null)
                        employeeList.AddRange(staffinfo.Select(x => new StaffInfo() { Id = x.Id, StaffName = x.Name }).ToList());
                }
            }

            //distinct 
            response.EmployeeList = employeeList.GroupBy(x => x.Id).Select(x => x.First());

            //Get Status List By roles
            var data = await _expenseRepository.GetStatusList();

            if (data == null || !data.Any())
                return new ExpenseClaimSummaryResponse { Result = ExpenseClaimSummaryResult.CannotFindStatsList };

            response.StatusList = data.Select(_expensemap.GetStatus);

            response.Result = ExpenseClaimSummaryResult.Success;

            return response;
        }

        public async Task<ExpenseClaimListResponse> GetExpenseClaimList(ExpenseClaimListRequest request)
        {
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 20;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            //Get Employee list
            var employeeList = new List<int>();

            var staffInfo = await _hrManager.GetStaffInfoByStaffId(_ApplicationContext.StaffId);

            // Check Login user emplyee type with request filter type
            if (staffInfo != null && staffInfo.EmployeeTypeId == request.EmployeeTypeId && (_ApplicationContext.RoleList.Contains((int)RoleEnum.ExpenseClaim) || _ApplicationContext.RoleList.Contains((int)RoleEnum.Management) || _ApplicationContext.RoleList.Contains((int)RoleEnum.OutsourceAccounting)))
            {
                employeeList.AddRange(new List<int>() { _ApplicationContext.StaffId });
            }
            else
            {
                employeeList.AddRange(new List<int>() { _ApplicationContext.StaffId });
            }

            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.ExpenseClaim))
            {
                if (request.EmployeeTypeId == (int)EmployeeTypeEnum.Permanent)
                {
                    var locations = await _hrManager.GetInternalStaffListByLocationList(request.EmployeeTypeId);

                    if (locations != null && locations.Any())
                        employeeList.AddRange(locations.Select(x => x.Id));
                }
                else if (_ApplicationContext.LocationList != null && _ApplicationContext.LocationList.Any())
                {
                    var locations = await _humanResourceRepository.GetInternalStaffListByLocationsAndTypes(_ApplicationContext.LocationList);

                    if (locations != null && locations.Any())
                        employeeList.AddRange(locations.Select(x => x.Id).ToList());
                }
            }

            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.Management))
            {
                var staffList = await _hrManager.GetStaffListByParentId(_ApplicationContext.StaffId);
                staffList = staffList.Where(x => x.EmployeeTypeId == request.EmployeeTypeId).ToList();
                employeeList.AddRange(staffList.Select(x => x.Id).ToList());
            }


            //get the staff details belongs to user 'OutSourceAccounting' Role
            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.OutsourceAccounting))
            {
                var hrOutSourceCompany = await _humanResourceRepository.GetHROutSourceCompanyByUserId(_ApplicationContext.UserId);
                if (hrOutSourceCompany != null)
                {
                    var outSourceCompanyStaffList = await _humanResourceRepository.GetStaffByHROutSourceCompany(new[] { hrOutSourceCompany.Id }.ToList());
                    if (outSourceCompanyStaffList != null && outSourceCompanyStaffList.Any())
                        employeeList.AddRange(outSourceCompanyStaffList.Select(x => x.Id).ToList());
                }
            }
            else if (request.CompanyIds != null && request.CompanyIds.Any())
            {
                employeeList = new List<int>();
                var outSourceCompanyStaffList = await _humanResourceRepository.GetStaffByHROutSourceCompany(request.CompanyIds);
                if (outSourceCompanyStaffList != null && outSourceCompanyStaffList.Any())
                    employeeList.AddRange(outSourceCompanyStaffList.Select(x => x.Id).ToList());
            }

            if (employeeList == null || !employeeList.Any())
                return new ExpenseClaimListResponse { Result = ExpenseClaimListResult.NotFound };

            IEnumerable<int> staffIds = null;

            if (request.EmployeeValues == null || !request.EmployeeValues.Any())
                staffIds = employeeList.Distinct();
            else
                staffIds = request.EmployeeValues?.Select(x => x.Id).Where(x => employeeList.Contains(x));

            var locIds = new List<int>();

            if (_ApplicationContext.LocationList != null)
                locIds = _ApplicationContext.LocationList.ToList();

            locIds.Add(_ApplicationContext.LocationId);

            if (request.LocationValues != null && request.LocationValues.Any())
                locIds = locIds.Where(x => request.LocationValues.Any(y => y.Id == x)).ToList();

            var expenseClaimDataRequest = new ExpenseClaimDataRequest()
            {
                StartDate = request.StartDate.ToDateTime(),
                EndDate = request.EndDate.ToDateTime(),
                StatusIds = request.StatusValues?.Select(x => x.Id),
                StaffIds = staffIds,
                LocIds = locIds,
                IsClaimDate = request.IsClaimDate,
                Take = request.pageSize.Value,
                Skip = skip,
                ClaimTypeIds = request.ClaimTypeIds,
                PayrollCompanyIds = request.PayrollCompanyIds,
                IsAutoExpense = request.IsAutoExpense
            };

            if (expenseClaimDataRequest.StaffIds != null && expenseClaimDataRequest.StaffIds.Any())
            {
                var expenseQuerable = _expenseRepository.GetIqueryableExpense();

                var expenseFilteredQueryable = GetFilterExpenseData(expenseQuerable, expenseClaimDataRequest);

                int total = await expenseFilteredQueryable.CountAsync();

                if (total <= 0)
                    return new ExpenseClaimListResponse { Result = ExpenseClaimListResult.NotFound };

                var statusList = await GetExpenseStatusList(expenseFilteredQueryable);

                var expenseIds = await expenseFilteredQueryable.Select(x => x.Id).ToListAsync();

                var expenseItemList = await GetExpenseSummaryList(expenseFilteredQueryable, expenseClaimDataRequest.Skip, expenseClaimDataRequest.Take);

                var expenseIdList = expenseItemList.Select(x => x.Id).ToList();

                var expenseDetailItemList = await _expenseRepository.GetExpenseDetailsListByExpenseId(expenseIdList);

                var staffList = await _hrManager.GetStaffListByParentId(_ApplicationContext.StaffId);
                var claimTypeList = await _expenseRepository.GetExpenseClaimTypeList();

                return new ExpenseClaimListResponse
                {
                    Result = ExpenseClaimListResult.Success,
                    ExpenseClaimGroupList = expenseItemList.GroupBy(x => x.StaffId).Select(x =>
                    _expensemap.GetExpenseClaimGroup(x, _ApplicationContext.StaffId, _ApplicationContext.RoleList, staffList, claimTypeList, expenseDetailItemList)),
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    TotalCount = total,
                    CanCheck = _ApplicationContext.RoleList.Contains((int)RoleEnum.ExpenseClaim),
                    ExpenseStatusList = statusList,
                    ExpenseIdList = expenseIds
                };

            }
            else
            {
                return new ExpenseClaimListResponse { Result = ExpenseClaimListResult.NotFound };
            }
        }

        private async Task<SaveExpenseClaimResponse> AddExpenseClaim(ExpenseClaim request)
        {

            if (!string.IsNullOrEmpty(request.ClaimNo) && await _expenseRepository.CheckExpenseClaimNoExist(request.ClaimNo))
            {
                return new SaveExpenseClaimResponse
                {
                    Result = SaveExpenseClaimResult.ClaimNumberAlreadyExist,
                };
            }

            //validate the new booking
            var bookingValidationResponse = await ValidateDuplicateBooking(request.Id, request.StaffId,
                request.ClaimTypeId.GetValueOrDefault(), request.ExpenseList.ToList());

            if (bookingValidationResponse.Result == SaveExpenseClaimResult.ClaimAlreadyDoneForInspection)
                return bookingValidationResponse;

            var entity = new EcExpencesClaim
            {
                LocationId = request.LocationId,
                StaffId = request.StaffId,
                Active = true,
                CountryId = request.CountryId,
                CreatedDate = DateTime.Now,
                ExpensePurpose = request.ExpensePuropose?.Trim(),
                ClaimDate = request.ClaimDate.ToDateTime(),
                ClaimNo = request.ClaimNo,
                StatusId = (int)ExpenseClaimStatus.Pending,
                PaymentTypeId = 1,
                EntityId = _filterService.GetCompanyId(),
                ClaimTypeId = request.ClaimTypeId
            };

            AddExpenseDetails(request.ExpenseList, entity, request.ClaimTypeId);

            _expenseRepository.Save(entity, false);

            var userAccessFilter = new UserAccess
            {
                OfficeId = entity.LocationId,
                RoleId = (int)RoleEnum.ExpenseClaimNotification
            };

            var userListByRoleAccess = await _userManager.GetUserListByRoleOffice(userAccessFilter);

            // Add new Task
            await _userManager.AddTask(TaskType.ExpenseToCheck, entity.Id, userListByRoleAccess.Select(x => x.Id));

            // get all users with ClaimAccess and have office control current office control de notify them
            var users = await _userManager.GetUserListByRole((int)RoleEnum.ExpenseClaimNotification, _ApplicationContext.LocationId);

            var expenseClaimResponse = await GetExpenseClaim("", entity.Id);


            return new SaveExpenseClaimResponse
            {
                Result = SaveExpenseClaimResult.Success,
                UserList = users,
                ExpenseClaim = expenseClaimResponse.ExpenseClaim,
            };

        }

        /// <summary>
        /// Validate the duplicate booking on save expense claim
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="claimTypeId"></param>
        /// <param name="expenseList"></param>
        /// <returns></returns>
        private async Task<SaveExpenseClaimResponse> ValidateDuplicateBooking(int expenseClaimId, int staffId, int claimTypeId, List<ExpenseClaimDetails> expenseList)
        {
            var response = new SaveExpenseClaimResponse();
            //if claim type is inspection check if any duplicate booking id is there
            if (claimTypeId == (int)ClaimTypeEnum.Inspection)
            {
                var bookingNoList = expenseList.Where(x => x.BookingNo != null).Select(x => x.BookingNo.GetValueOrDefault()).Distinct().ToList();
                var bookingIds = await _expenseRepository.GetExpenseBookingIdsForQC(expenseClaimId, staffId, bookingNoList);

                if (bookingIds.Any())
                    response = new SaveExpenseClaimResponse { Result = SaveExpenseClaimResult.ClaimAlreadyDoneForInspection, CreatedExpenseBookingIds = bookingIds };
            }
            return response;
        }

        /// <summary>
        /// Validate duplicate booking in edit scenario
        /// </summary>
        /// <param name="expenseClaimId"></param>
        /// <param name="staffId"></param>
        /// <param name="claimTypeId"></param>
        /// <param name="expenseList"></param>
        /// <param name="isAutoQCExpense"></param>
        /// <returns></returns>
        private async Task<SaveExpenseClaimResponse> ValidateEditDuplicateBooking(int expenseClaimId, int staffId, int claimTypeId, List<ExpenseClaimDetails> expenseList, bool? isAutoQCExpense)
        {
            var response = new SaveExpenseClaimResponse();
            //if claim type is inspection check if any duplicate booking id is there
            if (claimTypeId == (int)ClaimTypeEnum.Inspection)
            {
                var bookingNoList = expenseList.Where(x => x.BookingNo != null).Select(x => x.BookingNo.GetValueOrDefault()).Distinct().ToList();
                var bookingIds = await _expenseRepository.GetEditExpenseBookingIdsForQC(expenseClaimId, staffId, bookingNoList, isAutoQCExpense.GetValueOrDefault());

                if (bookingIds.Any())
                    response = new SaveExpenseClaimResponse { Result = SaveExpenseClaimResult.ClaimAlreadyDoneForInspection, CreatedExpenseBookingIds = bookingIds };
            }
            return response;
        }

        ///// <summary>
        ///// Validate the duplicate booking on edit expense claim
        ///// </summary>
        ///// <param name="expenseClaimId"></param>
        ///// <param name="staffId"></param>
        ///// <param name="claimTypeId"></param>
        ///// <param name="expenseList"></param>
        ///// <returns></returns>
        //private async Task<SaveExpenseClaimResponse> ValidateEditDuplicateBooking(int expenseClaimId, int staffId, int? claimTypeId, List<ExpenseClaimDetails> expenseList)
        //{
        //    var response = new SaveExpenseClaimResponse();
        //    //if claim type is inspection check if any duplicate booking id is there
        //    if (claimTypeId == (int)ClaimTypeEnum.Inspection)
        //    {
        //        var bookingNoList = expenseList.Where(x => x.BookingNo != null).Select(x => x.BookingNo.GetValueOrDefault()).Distinct().ToList();
        //        var bookingIds = await _expenseRepository.GetEditExpenseBookingIdsForQC(expenseClaimId, staffId, bookingNoList);

        //        if (bookingIds.Any())
        //            response = new SaveExpenseClaimResponse { Result = SaveExpenseClaimResult.ClaimAlreadyDoneForInspection, CreatedExpenseBookingIds = bookingIds };
        //    }
        //    return response;
        //}

        public async Task<SaveExpenseClaimResponse> UpdateExpenseClaim(ExpenseClaim request)
        {
            var expenses = await _expenseRepository.GetExpenseClaim(request.Id);
            var expenseClaim = expenses.Item1;

            if (expenseClaim == null)
                return new SaveExpenseClaimResponse { Result = SaveExpenseClaimResult.CurrentExpenseClaimNotFound };

            if (request.StatusId != (int)ExpenseClaimStatus.Rejected && request.StatusId != (int)ExpenseClaimStatus.Pending)
                return new SaveExpenseClaimResponse { Result = SaveExpenseClaimResult.UnAuthorized };

            // only created staff or OutsourceAccounting role or expense role can update the expense
            if (expenseClaim.StaffId == _ApplicationContext.StaffId || ((expenseClaim.StaffId != _ApplicationContext.StaffId) &&
                (_ApplicationContext.RoleList.Contains((int)RoleEnum.OutsourceAccounting) ||
                (_ApplicationContext.RoleList.Contains((int)RoleEnum.ExpenseClaim) &&
                 (request.IsAutoExpense.GetValueOrDefault() || expenseClaim.StatusId == (int)ExpenseClaimStatus.Pending)
                ))))
            {
                IEnumerable<DTO.User.User> userList = null;

                // Status 
                if (request.StatusId == (int)ExpenseClaimStatus.Rejected)
                {
                    expenseClaim.StatusId = (int)ExpenseClaimStatus.Pending;

                    // get all users with ClaimAccess and have office control current office control de notify them
                    userList = await _userManager.GetUserListByRole((int)RoleEnum.ExpenseClaimNotification, _ApplicationContext.LocationId);

                    var userAccessFilter = new UserAccess
                    {
                        OfficeId = expenseClaim.LocationId,
                        RoleId = (int)RoleEnum.ExpenseClaimNotification
                    };

                    var userListByRoleAccess = await _userManager.GetUserListByRoleOffice(userAccessFilter);

                    // Add new Task
                    await _userManager.AddTask(TaskType.ExpenseToCheck, expenseClaim.Id, userListByRoleAccess.Select(x => x.Id));

                }

                // Details 
                if (request.ExpenseList == null)
                    request.ExpenseList = new HashSet<ExpenseClaimDetails>();

                //Removed 
                var exIds = request.ExpenseList.Where(x => x.Id > 0).Select(x => x.Id);
                var lstexpclaimToremove = new List<EcExpensesClaimDetai>();
                var lstExpReceiptAttachmentToremove = new List<EcReceiptFileAttachment>();
                var expensesToRemove = expenseClaim.EcExpensesClaimDetais.Where(x => !exIds.Contains(x.Id));

                foreach (var item in expensesToRemove.ToList())
                {
                    //remove file
                    var lstremovefiles = new List<EcReceiptFile>();
                    foreach (var fileItem in item.EcReceiptFiles.ToList())
                    {
                        lstremovefiles.Add(fileItem);
                    }
                    _expenseRepository.RemoveEntities(lstremovefiles);

                    foreach (var fileItem in item.EcReceiptFileAttachments.ToList())
                    {
                        fileItem.Active = false;
                        fileItem.DeletedBy = _ApplicationContext.UserId;
                        fileItem.DeletedOn = DateTime.Now;

                        lstExpReceiptAttachmentToremove.Add(fileItem);
                    }
                    _expenseRepository.RemoveEntities(lstExpReceiptAttachmentToremove);
                    lstexpclaimToremove.Add(item);
                }

                _expenseRepository.RemoveEntities(lstexpclaimToremove);

                // New
                var newDetails = request.ExpenseList.Where(x => x.Id == 0);

                //validate the new expense claim booking is duplicate
                var bookingValidationResponse = await ValidateDuplicateBooking(request.Id, request.StaffId, request.ClaimTypeId.GetValueOrDefault(), newDetails.ToList());

                if (bookingValidationResponse.Result == SaveExpenseClaimResult.ClaimAlreadyDoneForInspection)
                    return bookingValidationResponse;

                AddExpenseDetails(newDetails, expenseClaim, request.ClaimTypeId);

                //Updated
                var expensesToUpdate = request.ExpenseList.Where(x => x.Id > 0);

                //validate the existing expense claim booking is duplicated
                bookingValidationResponse = await ValidateEditDuplicateBooking(request.Id, request.StaffId, request.ClaimTypeId.GetValueOrDefault(), expensesToUpdate.ToList(),request.IsAutoExpense);

                if (bookingValidationResponse.Result == SaveExpenseClaimResult.ClaimAlreadyDoneForInspection)
                    return bookingValidationResponse;



                foreach (var item in expensesToUpdate)
                {
                    var expense = expenseClaim.EcExpensesClaimDetais.FirstOrDefault(x => x.Id == item.Id);

                    if (expense != null)
                    {
                        expense.AmmountHk = item.ActualAmount;
                        expense.Amount = item.Amount;
                        expense.ArrivalCityId = item.DestCity?.Id;
                        expense.CurrencyId = item.CurrencyId;
                        expense.Receipt = item.Receipt;
                        expense.StartCityId = item.StartCity?.Id;
                        expense.ExpenseDate = item.ExpenseDate.ToDateTime();
                        expense.Description = item.Description?.Trim();
                        expense.ExchangeRate = item.ExchangeRate;
                        expense.ExpenseTypeId = item.ExpenseTypeId;
                        expense.TripType = item.TripMode;
                        expense.Tax = item.Tax;
                        expense.TaxAmount = item.TaxAmount;
                        expense.ManDay = item.ManDay;

                        if (request.ClaimTypeId == (int)ClaimTypeEnum.Audit && item.BookingNo != null)
                            expense.AuditId = item.BookingNo;
                        else if (request.ClaimTypeId == (int)ClaimTypeEnum.Inspection && item.BookingNo != null)
                            expense.InspectionId = item.BookingNo;

                        //removed files
                        var fiIds = item.Files.Where(x => x.Id > 0).Select(x => x.Id);
                        var filesToremove = expense.EcReceiptFiles.Where(x => !fiIds.Contains(x.Id) && x.ExpenseId == item.Id).ToArray();

                        foreach (var fileItem in filesToremove.ToList())
                            expense.EcReceiptFiles.Remove(fileItem);
                        _expenseRepository.RemoveEntities(filesToremove);

                        var filesToremoveFromNewTable = expense.EcReceiptFileAttachments.Where(x => !fiIds.Contains(x.Id) && x.ExpenseId == item.Id).ToArray();

                        foreach (var fileItem in filesToremoveFromNewTable.ToList())
                        {
                            fileItem.Active = false;
                            fileItem.DeletedBy = _ApplicationContext.UserId;
                            fileItem.DeletedOn = DateTime.Now;
                            expense.EcReceiptFileAttachments.Add(fileItem);
                        }

                        // New Files
                        if (item.Files == null)
                            item.Files = new HashSet<ExpenseClaimReceipt>();

                        var files = item.Files.Where(x => x.Id == 0);

                        AddFiles(expense, files);

                    }
                }
                expenseClaim.ExpensePurpose = request.ExpensePuropose?.Trim();
                _expenseRepository.Save(expenseClaim, true);

                var travelPortList = await GetExpenseTravelQCPort(expenseClaim.EcExpensesClaimDetais);

                return new SaveExpenseClaimResponse
                {
                    Result = SaveExpenseClaimResult.Success,
                    UserList = userList,
                    ExpenseClaim = _expensemap.GetExpenseClaim(expenseClaim, expenses.Item2, (x) => _fileManager.GetMimeType(x),
                    travelPortList)
                };
            }
            else
            {
                return new SaveExpenseClaimResponse { Result = SaveExpenseClaimResult.UnAuthorized };
            }
        }

        private void AddExpenseDetails(IEnumerable<ExpenseClaimDetails> expenseList, EcExpencesClaim expenseClaim, int? claimTypeId)
        {
            if (expenseList != null)
            {
                foreach (var item in expenseList)
                {
                    var details = new EcExpensesClaimDetai
                    {
                        Active = true,
                        AmmountHk = item.ActualAmount,
                        Amount = item.Amount,
                        ArrivalCityId = item.DestCity?.Id,
                        CurrencyId = item.CurrencyId,
                        Receipt = item.Receipt,
                        StartCityId = item.StartCity?.Id,
                        ExpenseDate = item.ExpenseDate.ToDateTime(),
                        Description = item.Description?.Trim(),
                        ExchangeRate = item.ExchangeRate,
                        ExpenseTypeId = item.ExpenseTypeId,
                        TripType = item.TripMode,
                        EntityId = _filterService.GetCompanyId(),
                        IsAutoExpense = item.IsAutoExpense != null ? item.IsAutoExpense : false,
                        ManDay = item.ManDay,
                        Tax = item.Tax,
                        TaxAmount = item.TaxAmount
                    };

                    AddFiles(details, item.Files);
                    if (claimTypeId == (int)ClaimTypeEnum.Audit && item.BookingNo != null)
                        details.AuditId = item.BookingNo;
                    else if (claimTypeId == (int)ClaimTypeEnum.Inspection && item.BookingNo != null)
                        details.InspectionId = item.BookingNo;
                    expenseClaim.EcExpensesClaimDetais.Add(details);
                    _expenseRepository.AddEntity(details);
                }

            }
        }

        private void AddFiles(EcExpensesClaimDetai details, IEnumerable<ExpenseClaimReceipt> files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var ecFile = new EcReceiptFileAttachment
                    {
                        FileName = file.FileName,
                        FileUrl = file.FileUrl,
                        UniqueId = file.Uniqueld,
                        CreatedOn = DateTime.Now,
                        Createdby = _ApplicationContext.UserId,
                        Active = true
                    };

                    details.EcReceiptFileAttachments.Add(ecFile);
                    _expenseRepository.AddEntity(ecFile);
                }
            }

        }

        public async Task UploadFiles(Dictionary<Guid, byte[]> fileList)
        {
            var guidList = fileList.Select(x => x.Key);
            var data = await _expenseRepository.GetReceptFiles(guidList);

            foreach (var item in data)
                item.File = fileList[item.GuidId];

            await _expenseRepository.Save();

        }

        public async Task<ExpenseClaimTypeResponse> GetExpenseClaimTypeList()
        {
            ExpenseClaimTypeResponse response = new ExpenseClaimTypeResponse();
            var data = await _expenseRepository.GetExpenseClaimTypeList();
            if (data != null)
            {
                var claimTypeList = data.Select(x => _expensemap.GetExpenseClaimType(x));
                response.expenseClaimTypeList = claimTypeList;
                response.expenseClaimTypeResult = ExpenseClaimTypeResult.Success;
            }
            response.expenseClaimTypeResult = ExpenseClaimTypeResult.NotFound;
            return response;
        }

        /// <summary>
        /// Get the Booking Details for outsource qc accounting role
        /// </summary>
        /// <param name="expenseId"></param>
        /// <returns></returns>
        public async Task<ExpenseBookingDetailResponse> GetOutSourceQCBookingDetails(int? expenseId)
        {
            #region Declaration
            List<ExpenseBookingDetail> bookingDetails = null;
            DateTime minDate = DateTime.Now.AddMonths(-6);
            #endregion

            ExpenseBookingDetailResponse response = new ExpenseBookingDetailResponse() { Result = ExpenseBookingDetailResult.NotFound };

            response.expenseBookingDetailAccess = setExpenseBookingAccess(false, false, false, true);

            //get the hr outsource company details
            var hrOutSourceCompany = await _humanResourceRepository.GetHROutSourceCompanyByUserId(_ApplicationContext.UserId);

            var qcId = await _expenseRepository.GetQCByExpenseId(expenseId.GetValueOrDefault());

            //if hr outsourcecompany is null
            if (hrOutSourceCompany != null)
            {
                if (expenseId > 0)
                    bookingDetails = await _expenseRepository.GetEditOutSourceQCBookingDetails(qcId, hrOutSourceCompany.Id, minDate);
                else
                    bookingDetails = await _expenseRepository.GetOutSourceQCBookingDetails(hrOutSourceCompany.Id, minDate);
            }

            //assign the bookingdetails
            if (bookingDetails != null && bookingDetails.Any())
            {
                response.expenseBookingDetailAccess = setExpenseBookingAccess(true, true, true, true);
                response.expenseBookingDetailList = bookingDetails;
                response.Result = ExpenseBookingDetailResult.Success;
            }

            return response;
        }

        public async Task<ExpenseBookingDetailResponse> GetExpenseBookingDetails(int claimTypeId, int? expenseId, bool isEdit)
        {
            ExpenseBookingDetailResponse response = new ExpenseBookingDetailResponse();
            var staffId = _ApplicationContext.StaffId;
            int? statusId = null;
            response.expenseBookingDetailAccess = setExpenseBookingAccess(false, false, false, true);
            if (expenseId != null)
            {
                var expenseClaim = _expenseRepository.GetExpenseClaimById(expenseId).Result;
                if (expenseClaim != null)
                {
                    staffId = expenseClaim.StaffId;
                    statusId = expenseClaim.StatusId;
                }

            }

            var data = await GetBookingDetails(claimTypeId, staffId, isEdit, expenseId, statusId);
            response.Result = ExpenseBookingDetailResult.NotFound;
            if (data != null && data.Any())
            {
                if (_ApplicationContext.UserProfileList.Contains((int)HRProfile.Auditor) || _ApplicationContext.UserProfileList.Contains((int)HRProfile.Inspector))
                {
                    response.expenseBookingDetailAccess = setExpenseBookingAccess(true, true, true, true);
                }
                response.expenseBookingDetailList = data;
                response.Result = ExpenseBookingDetailResult.Success;
            }

            return response;
        }

        public async Task<List<ExpenseBookingDetail>> GetBookingDetails(int claimTypeId, int staffId, bool isEdit, int? expenseId, int? statusId)
        {
            List<ExpenseBookingDetail> expenseBookingList = new List<ExpenseBookingDetail>();

            var expstatus = new int[] { (int)ExpenseClaimStatus.Pending, (int)ExpenseClaimStatus.Rejected };

            if ((staffId != _ApplicationContext.StaffId) || (statusId.HasValue && !expstatus.Contains(statusId.Value)))
            {
                if (claimTypeId == (int)ExpenseBookingDetailEnum.Audit)
                {
                    var data = await _expenseRepository.GetAuditbookingByExpenseId(expenseId.Value);
                    expenseBookingList = data;
                }
                else if (claimTypeId == (int)ExpenseBookingDetailEnum.Inspection)
                {
                    var data = await _expenseRepository.GetInspbookingsByExpenseId(expenseId.Value);
                    expenseBookingList = data;
                }
            }
            else
            {
                //getting only 3 month data.
                DateTime minDate = DateTime.Now.AddMonths(-6);


                if (claimTypeId == (int)ExpenseBookingDetailEnum.Audit)
                {
                    var data = await _expenseRepository.GetAuditorAssignedBookings(staffId, minDate);
                    expenseBookingList = data;
                }
                else if (claimTypeId == (int)ExpenseBookingDetailEnum.Inspection)
                {
                    if (!isEdit)
                    {
                        var data = await _expenseRepository.GetQCAssignedBookings(staffId, minDate);
                        expenseBookingList = data;
                    }
                    else
                    {
                        var data = await _expenseRepository.GetEditExpenseQCAssignedBookings(staffId, minDate);
                        expenseBookingList = data;
                    }
                }

            }
            return expenseBookingList;
        }

        public ExpenseBookingDetailAccess setExpenseBookingAccess(bool bookingDetailVisible, bool bookingNoEnabled, bool bookingNoVisible, bool claimTypeEnabled)
        {
            ExpenseBookingDetailAccess expenseBookingDetailAccess = new ExpenseBookingDetailAccess();
            expenseBookingDetailAccess.BookingDetailVisible = bookingDetailVisible;
            expenseBookingDetailAccess.BookingNoEnabled = bookingNoEnabled;
            expenseBookingDetailAccess.BookingNoVisible = bookingNoVisible;
            expenseBookingDetailAccess.ClaimTypeEnabled = claimTypeEnabled;
            return expenseBookingDetailAccess;
        }

        //Export Voucher data based on employee and claim type
        public async Task<ExpenseClaimVoucherData> ExportVocherSummary(ExpenseClaimListRequest request)
        {
            //fetch the expense claim data
            var data = await _expenseRepository.ExpenseClaimDetails(request.ClaimIdList);

            //filter the data for the first employee -- validation applied in client side as well
            data = data.Where(x => x.StaffId == request.EmployeeValues.Select(y => y.Id).FirstOrDefault()).ToList();

            //get the distinct claim detail Id
            var claimDetailIdList = data.Select(x => x.ClaimDetailId).Distinct().ToList();

            ////get the audit data
            var auditData = await _expenseRepository.AuditDataByExpenseId(claimDetailIdList);

            //get the inspection data 
            var bookingData = await _expenseRepository.BookingDataByExpenseId(claimDetailIdList);

            var res = _expensemap.ExportExpenseVoucherMap(request, data, bookingData, auditData);

            return res;

        }

        public async Task<ExportExpenseClaimSummaryKpiResponse> ExportExpenseKpiSummary(ExpenseClaimListRequest request)
        {
            var auditData = new List<ExpenseAuditData>();
            var bookingData = new List<ExpenseBookingData>();

            var _expenseExportKPIMap = new ExpenseExportKPIMap();

            _expenseExportKPIMap.request = request;

            //fetch the expense claim data
            _expenseExportKPIMap.expenseClaimList = await _expenseRepository.ExpenseClaimDetails(request.ClaimIdList);

            //get the claim expense and allowance data
            //var res = _expensemap.ExportExpenseVoucherMap(request, data, bookingData, auditData);

            var deptIds = _expenseExportKPIMap.expenseClaimList.Select(x => x.DeptId.GetValueOrDefault()).Distinct().ToList();

            var deptData = await _hrManager.GetDepartmentList(deptIds);

            if (deptData.Result == DataSourceResult.Success)
            {
                _expenseExportKPIMap.deptData = deptData.DataSourceList;
            }

            var inspectionIds = _expenseExportKPIMap.expenseClaimList.Where(x => x.InspectionId > 0).Select(x => x.InspectionId.GetValueOrDefault()).ToList();

            var auditIds = _expenseExportKPIMap.expenseClaimList.Where(x => x.AuditId > 0).Select(x => x.AuditId.GetValueOrDefault()).ToList();

            if (inspectionIds != null && inspectionIds.Any())
            {
                _expenseExportKPIMap.InspectionBookingList = await _inspectionBookingManager.GetBookingDetails(inspectionIds);

                _expenseExportKPIMap.InspFactoryAddressList = await _inspectionBookingManager.GetFactorycountryId(inspectionIds);
            }
            if (auditIds != null && auditIds.Any())
            {
                _expenseExportKPIMap.AuditFactoryAddressList = await _auditManager.GetFactoryAddressDetailsByAuditIds(auditIds);

                _expenseExportKPIMap.AuditBookingList = await _auditManager.GetAuditDetails(auditIds);
            }

            var response = _expensemap.ExportExpenseClaimSummaryKpisMap(_expenseExportKPIMap);

            return response;
        }

        /// <summary>
        /// Check any pending expense exist or not
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> CheckPendingExpenseExist(PendingExpenseRequest request)
        {
            bool isPendingExpense = false;

            if (request != null)
            {
                var isTravelExist = await _expenseRepository.CheckPendingTravelExpenseExist(request.QcId, request.BookingList);

                var isFoodExist = await _expenseRepository.CheckPendingFoodExpenseExist(request.QcId, request.BookingList);

                isPendingExpense = (isTravelExist || isFoodExist);
            }

            return isPendingExpense;
        }

        /// <summary>
        /// get expense food allowance 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ExpenseFoodClaimResponse> GetExpenseFoodAmount(ExpenseFoodClaimRequest request)
        {
            var _ExpenseDate = request.ExpenseDate.ToNullableDateTime();
            var expenseDate = _ExpenseDate.HasValue ? _ExpenseDate.Value.ToString(StandardDateFormat) : string.Empty;

            if (request != null)
            {
                if (!DateTime.TryParseExact(expenseDate.ToString(), StandardDateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ExpDate))
                {
                    return new ExpenseFoodClaimResponse() { Result = ExpenseResponseResult.DateFormatIsNotValid };
                }
                else
                {
                    var response = new ExpenseFoodClaimResponse
                    {
                        ActualAmount = await _expenseRepository.GetExpenseFoodAllowance(request)
                    };

                    if (response.ActualAmount > 0)
                    {
                        response.Result = ExpenseResponseResult.Success;
                    }
                    else
                    {
                        response.Result = ExpenseResponseResult.NoDataFound;
                    }
                    return response;
                }
            }
            else
            {
                return new ExpenseFoodClaimResponse() { Result = ExpenseResponseResult.RequestNotCorrectFormat };
            }

        }

        /// <summary>
        /// expense summary detail kpi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ExportExpenseSummaryKpiResponse> ExportExpenseDetailKpiSummary(ExpenseClaimListRequest request)
        {
            var auditData = new List<ExpenseAuditData>();
            var bookingData = new List<ExpenseBookingData>();
            var response = new ExportExpenseSummaryKpiResponse();

            var _expenseExportKPIMap = new ExpenseExportKPIMap
            {
                request = request,
                //fetch the expense claim data
                expenseClaimList = await _expenseRepository.ExpenseClaimDetails(request.ClaimIdList)
            };

            var inspectionIds = _expenseExportKPIMap.expenseClaimList.Where(x => x.InspectionId > 0).Select(x => x.InspectionId.GetValueOrDefault()).ToList();

            var auditIds = _expenseExportKPIMap.expenseClaimList.Where(x => x.AuditId > 0).Select(x => x.AuditId.GetValueOrDefault()).ToList();

            if (inspectionIds != null && inspectionIds.Any())
            {
                _expenseExportKPIMap.InspectionBookingList = await _inspectionBookingManager.GetBookingDetails(inspectionIds);

                _expenseExportKPIMap.InspFactoryAddressList = await _inspectionBookingManager.GetFactorycountryId(inspectionIds);

                _expenseExportKPIMap.InspServiceTypeList = await _inspectionBookingManager.GetServiceType(inspectionIds);
            }
            if (auditIds != null && auditIds.Any())
            {
                _expenseExportKPIMap.AuditFactoryAddressList = await _auditManager.GetFactoryAddressDetailsByAuditIds(auditIds);

                _expenseExportKPIMap.AuditBookingList = await _auditManager.GetAuditDetails(auditIds);

                _expenseExportKPIMap.AuditServiceTypeList = await _auditManager.GetAuditServiceTypeList(auditIds);
            }

            response.ExpenseSummaryDetailKpiList = _expensemap.ExportExpenseDetailsKpiMap(_expenseExportKPIMap);

            response.Result = ExpenseSummaryDetailKpiListResult.Success;

            return response;
        }
        /// <summary>
        /// get booking id list food or travel expense is pending
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PendingBookingExpenseResponse> GetFoodOrTravelPendingExpenseBookingIdList(List<PendingBookingExpenseRequest> request)
        {
            var response = new PendingBookingExpenseResponse();
            var notConfigurebookingIdList = new List<int?>();

            if (request != null)
            {
                foreach (var item in request)
                {
                    if (item.BookingIdList.Any() && item.QcId > 0)
                    {
                        var bookingIdList = item.BookingIdList.Distinct().ToList();

                        //get pending food expense booking id list
                        var pendingFoodBookingIdList = await _expenseRepository.GetPendingFoodExpenseBookingIds(item.QcId, bookingIdList);

                        //get pending travel expense booking id list
                        var pendingTravelBookingIdList = await _expenseRepository.GetPendingTravelExpenseBookingIds(item.QcId, bookingIdList);

                        pendingTravelBookingIdList.AddRange(pendingFoodBookingIdList);

                        notConfigurebookingIdList.AddRange(pendingTravelBookingIdList);
                    }
                }

                //get pending expense food or travel booking ids 
                response.BookingIdList = notConfigurebookingIdList.Select(x => x.GetValueOrDefault()).Distinct().ToList();

                if (response.BookingIdList.Any())
                {
                    response.Result = PendingBookingExpenseResponseResult.notConfigure;
                }
                else
                {
                    response.Result = PendingBookingExpenseResponseResult.configure;
                }
            }
            return response;
        }

        /// <summary>
        /// get status list
        /// </summary>
        /// <param name="expencesClaimsQuerable"></param>
        /// <returns></returns>
        private async Task<List<ExpenseStatus>> GetExpenseStatusList(IQueryable<EcExpencesClaim> expencesClaimsQuerable)
        {
            return await expencesClaimsQuerable.Select(x => new { x.StatusId, x.Status.Description, x.Id })
                   .GroupBy(p => new { p.StatusId, p.Description }, p => p, (key, _data) =>
                 new ExpenseStatus
                 {
                     Id = key.StatusId,
                     Label = key.Description,
                     TotalCount = _data.Count(),
                     LabelColor = ExpenseStatusColor.GetValueOrDefault(key.StatusId, "")
                 }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// select the expense items from expense querable
        /// </summary>
        /// <param name="expenseFilteredQueryable"></param>
        /// <returns></returns>
        public async Task<List<ExpenseDataRepo>> GetExpenseSummaryList(IQueryable<EcExpencesClaim> expenseFilteredQueryable, int skip, int take)
        {
            return await expenseFilteredQueryable.Select(x => new ExpenseDataRepo()
            {
                ApproveByFullName = x.Approved.FullName,
                ApprovedDate = x.ApprovedDate,
                CancelByFullName = x.Cancel.FullName,
                CancelDate = x.CancelDate,
                CheckedByFullName = x.Checked.FullName,
                CheckedDate = x.CheckedDate,
                ClaimDate = x.ClaimDate,
                ClaimNo = x.ClaimNo,
                ClaimTypeId = x.ClaimTypeId,
                ClaimTypeName = x.ClaimType.Name,
                CountryId = x.CountryId,
                CreatedDate = x.CreatedDate,
                ExpensePurpose = x.ExpensePurpose,
                Id = x.Id,
                LocationId = x.LocationId,
                PaidByFullName = x.Paid.FullName,
                PaidDate = x.PaidDate,
                RejectByFullName = x.Reject.FullName,
                RejectDate = x.RejectDate,
                StaffId = x.StaffId,
                StatusId = x.StatusId,
                PayrollCompanyName = x.Staff.PayrollCompanyNavigation.CompanyName,
                EmployeeTypeId = x.Staff.EmployeeTypeId,
                EmployeeTypeName = x.Staff.EmployeeType.EmployeeTypeName,
                HrOutSourceCompanyName = x.Staff.HroutSourceCompany.Name,
                PayrollCurrencyId = x.Staff.PayrollCurrencyId,
                PayrollCurrencyName = x.Staff.PayrollCurrency.CurrencyName,
                PersonName = x.Staff.PersonName,
                LocationName = x.Location.LocationName,
                Comment = x.Comment,
                StatusName = x.Status.Description,
                IsAutoExpense = x.IsAutoExpense
            }).Skip(skip).Take(take).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// filter data by request from expense
        /// </summary>
        /// <param name="expenseQuerable"></param>
        /// <param name="expenseClaimDataRequest"></param>
        /// <returns></returns>
        private IQueryable<EcExpencesClaim> GetFilterExpenseData(IQueryable<EcExpencesClaim> expenseQuerable, ExpenseClaimDataRequest expenseClaimDataRequest)
        {
            var expenseQuerableData = expenseQuerable;

            if (expenseClaimDataRequest.StaffIds != null && expenseClaimDataRequest.StaffIds.Any())
                expenseQuerableData = expenseQuerableData.Where(x => expenseClaimDataRequest.StaffIds.Contains(x.StaffId));

            if (expenseClaimDataRequest.LocIds != null && expenseClaimDataRequest.LocIds.Any())
                expenseQuerableData = expenseQuerableData.Where(x => expenseClaimDataRequest.LocIds.Contains(x.LocationId));

            if (expenseClaimDataRequest.IsClaimDate)
                expenseQuerableData = expenseQuerableData.Where(x => x.ClaimDate >= expenseClaimDataRequest.StartDate && x.ClaimDate <= expenseClaimDataRequest.EndDate);
            else
                expenseQuerableData = expenseQuerableData.Where(x => x.EcExpensesClaimDetais.Any(y => y.ExpenseDate >= expenseClaimDataRequest.StartDate && y.ExpenseDate <= expenseClaimDataRequest.EndDate));

            if (expenseClaimDataRequest.StatusIds != null && expenseClaimDataRequest.StatusIds.Any())
                expenseQuerableData = expenseQuerableData.Where(x => expenseClaimDataRequest.StatusIds.Contains(x.StatusId));

            if (expenseClaimDataRequest.ClaimTypeIds != null && expenseClaimDataRequest.ClaimTypeIds.Any())
                expenseQuerableData = expenseQuerableData.Where(x => expenseClaimDataRequest.ClaimTypeIds.Contains(x.ClaimTypeId.Value));

            if (expenseClaimDataRequest.PayrollCompanyIds != null && expenseClaimDataRequest.PayrollCompanyIds.Any())
                expenseQuerableData = expenseQuerableData.Where(x => expenseClaimDataRequest.PayrollCompanyIds.Contains(x.Staff.PayrollCompany.Value));

            if (expenseClaimDataRequest.IsAutoExpense)
                expenseQuerableData = expenseQuerableData.Where(x => x.IsAutoExpense == expenseClaimDataRequest.IsAutoExpense);

            return expenseQuerableData;
        }

    }
}
