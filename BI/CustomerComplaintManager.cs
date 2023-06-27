using AutoMapper;
using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Customer;
using DTO.Inspection;
using DTO.RepoRequest.Enum;
using DTO.Report;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class CustomerComplaintManager : ApiCommonData, ICustomerComplaintManager
    {
        private readonly IReferenceRepository _referenceRepo = null;
        private readonly ICustomerComplaintRepository _repo = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly IMapper _mapper;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly IAuditManager _auditManager = null;
        private readonly ComplaintMap _complaintMap = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        public CustomerComplaintManager(
           ICustomerComplaintRepository repo,
           IReferenceRepository referenceRepo, IMapper mapper, IAPIUserContext applicationContext, IInspectionBookingManager inspManager, IAuditManager auditManager, IInspectionBookingRepository inspRepo)
        {
            _repo = repo;
            _referenceRepo = referenceRepo;
            _mapper = mapper;
            _applicationContext = applicationContext;
            _inspManager = inspManager;
            _auditManager = auditManager;
            _complaintMap = new ComplaintMap();
            _inspRepo = inspRepo;
        }
        //Get All Complaint Type
        public async Task<ComplaintDataResponse> GetComplaintType()
        {
            var response = new ComplaintDataResponse();
            var data = await _repo.GetComplaintType();
            if (data == null || data.Count == 0)
                response.ComplaintDataList = null;
            else
            {
                response.ComplaintDataList = data.ToList();
                response.Result = ComplaintResult.Success;
            }
            return response;
        }

        //Get All Complaint Category
        public async Task<ComplaintDataResponse> GetComplaintCategory()
        {
            var response = new ComplaintDataResponse();
            var data = await _repo.GetComplaintCategory();
            if (data == null || data.Count == 0)
                response.ComplaintDataList = null;
            else
            {
                response.ComplaintDataList = data.ToList();
                response.Result = ComplaintResult.Success;
            }
            return response;
        }
        //Get All Complaint RecipientType
        public async Task<ComplaintDataResponse> GetComplaintRecipientType()
        {
            var response = new ComplaintDataResponse();
            var data = await _repo.GetComplaintRecipientType();
            if (data == null || data.Count == 0)
                response.ComplaintDataList = null;
            else
            {
                response.ComplaintDataList = data.ToList();
                response.Result = ComplaintResult.Success;
            }
            return response;
        }
        //Get All Complaint Department
        public async Task<ComplaintDataResponse> GetComplaintDepartment()
        {
            var response = new ComplaintDataResponse();
            var data = await _repo.GetComplaintDepartment();
            if (data == null || data.Count == 0)
                response.ComplaintDataList = null;
            else
            {
                response.ComplaintDataList = data.ToList();
                response.Result = ComplaintResult.Success;
            }
            return response;
        }
        //get all booking no data source
        public async Task<BookingNoDataSourceResponse> GetBookingNoDataSource(BookingNoDataSourceRequest request)
        {
            var response = new BookingNoDataSourceResponse();
            if (request.ServiceId == (int)Service.InspectionId)
                response = await _inspManager.GetBookingNoDataSource(request);
            else if (request.ServiceId == (int)Service.AuditId)
                response = await _auditManager.GetBookingNoDataSource(request);
            return response;
        }

        //get Audit data by id
        public async Task<ComplaintBookingDataResponse> GetAuditDetails(int AuditId)
        {
            ComplaintBookingDataResponse response = new ComplaintBookingDataResponse();

            if (AuditId <= 0)
            {
                response.Result = ComplaintResult.Failed;
                return response;
            }
            //Get booking Details based on booking number
            var bookingDetails = await _auditManager.GetAuditDetails(AuditId);

            //Get serviceType details
            var serviceType = await _auditManager.GetServiceTypeDataByAudit(AuditId);

            //Get factory details
            var factoryDetails = await _auditManager.GetFactoryCountryDataByAudit(AuditId);

            var _data = new ComplaintBookingData
            {
                BookingNo = bookingDetails.BookingNo,
                CustomerName = bookingDetails.CustomerName,
                CustomerId = bookingDetails.CustomerId,
                SupplierName = bookingDetails.SupplierName.Equals(bookingDetails.RegionalSupplierName, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(bookingDetails.RegionalSupplierName) ? bookingDetails.SupplierName : bookingDetails.SupplierName + " (" + bookingDetails.RegionalSupplierName + ")",
                FactoryName = bookingDetails.FactoryName.Equals(bookingDetails.RegionalFactoryName, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(bookingDetails.RegionalFactoryName) ? bookingDetails.FactoryName : bookingDetails.FactoryName + " (" + bookingDetails.RegionalFactoryName + ")",
                ServiceDateFrom = bookingDetails.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = bookingDetails.ServiceDateTo.ToString(StandardDateFormat),
                ServiceType = serviceType.FirstOrDefault().ServiceTypeName,
                BookingQty = 0,
                CountryName = factoryDetails.FirstOrDefault().FactoryCountryName,
                OfficeName = bookingDetails.Office,
                BookingStatus = bookingDetails.BookingStatus,
            };
            response.Data = _data;
            response.Result = ComplaintResult.Success;
            return response;
        }
        /// <summary>
        /// Get the complaint Data By ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ComplaintDetailedInfoResponse> GetComplaintDetailsById(int? Id)
        {
            var response = new ComplaintDetailedInfoResponse();
            if (Id != null)
            {
                var complaintData = await _repo.GetComplaintById(Id);
                var complaintDetailsData = await _repo.GetComplaintDetailsById(Id);
                var complaintPersonIncharge = await _repo.GetComplaintPersonInchargeById(Id);

                if (complaintData == null || complaintDetailsData == null || complaintPersonIncharge == null)
                {
                    response.Result = ComplaintResult.Failed;
                    return response;
                }
                response.Data = _complaintMap.GetComplaintDetailsData(complaintData, complaintDetailsData, complaintPersonIncharge);
                response.Result = ComplaintResult.Success;
                return response;
            }
            response.Result = ComplaintResult.Failed;
            return response;
        }

        //Get product details by booking
        public async Task<ComplaintBookingProductDataResponse> GetProductItemByBooking(int bookingId)
        {
            var response = new ComplaintBookingProductDataResponse();
            var data = await _inspManager.GetProductItemByBooking(bookingId);
            var poList = await _inspManager.GetBookingProductsPoItemsByProductRefIds(bookingId);
            if (data == null || data.Count() == 0)
                response.Data = null;
            else
            {
                response.Data = data.Select(x => _complaintMap.GetBookingProductPoMap(x, poList));
                response.Result = ComplaintResult.Success;
            }
            return response;
        }

        //Get email data by complaint
        public async Task<CustomerComplaintEmailTemplate> GetComplaintEmailData(ComplaintDetailedInfo request, int complaitId)
        {

            var mailData = await _repo.GetComplaintEmailData(complaitId);
            List<string> lstcategory = await _repo.GetComplaintCategoryData(complaitId);
            var userInfo = await _repo.GetUserData(request.UserIds.ToList());
            var _bookingNo = 0;
            if (mailData.serviceId > 0)
                _bookingNo = mailData.serviceId == 1 ? mailData.BookingNo : mailData.AuditNo;

            var _data = new CustomerComplaintEmailTemplate
            {
                StaffEmailID = userInfo.Select(x => x.EmailAddress ?? "").Distinct().Aggregate((x, y) => x + "," + y).ToString(),
                Staffname = userInfo.Select(x => x.Name).Distinct().Aggregate((x, y) => x + "," + y).ToString(),
                CurrentUserEmailID = _applicationContext?.EmailId,
                ComplaintId = complaitId,
                Customer = mailData.Customer,
                BookingNo = _bookingNo,
                Service = mailData.serviceId,
                ServiceName = mailData.serviceName,
                ComplaintDate = mailData.ComplaintDate?.ToString(StandardDateFormat),
                Category = lstcategory.Distinct().Aggregate((x, y) => x + "," + y).ToString(),
                Department = mailData.Department,
            };


            return _data;
        }

        //save complaint
        public async Task<SaveComplaintResponse> SaveComplaints(ComplaintDetailedInfo request)
        {
            try
            {

                var response = new SaveComplaintResponse();

                if (request == null)
                {
                    return new SaveComplaintResponse { Result = ComplaintResult.RequestNotCorrectFormat };
                }
                if (request.Id == 0)
                {
                    CompComplaint entity = CreateComplaintEntity(request);
                    AddPersonIncharge(request, entity);
                    AddComplaintDetails(request, entity);
                    await _repo.AddComplaints(entity);
                    response.Id = entity.Id;

                    if (response.Id == 0)
                    {
                        return new SaveComplaintResponse { Result = ComplaintResult.Failed };
                    }
                    response.Result = ComplaintResult.Success;
                    return response;
                }
                else
                {
                    var entity = await _repo.GetComplaintsItemsById(request.Id);

                    if (entity == null)
                        return new SaveComplaintResponse { Result = ComplaintResult.RequestNotCorrectFormat };
                    UpdateComplaintEntity(request, entity);
                    UpdatePersonIncharge(request.UserIds, entity);
                    UpdateComplaintDetails(request.ComplaintDetails, entity);

                    await _repo.EditComplaints(entity);
                    response.Id = entity.Id;
                    response.Result = ComplaintResult.Success;
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// Remove Complaint Detail
        public async Task<RemoveComplaintDetailResponse> RemoveComplaintDetail(int? id)
        {
            var response = new RemoveComplaintDetailResponse();
            if (id != null && id >= 0)
            {
                var entity = _repo.GetComplaintDetailItemById(id).FirstOrDefault();
                if (entity == null)
                {
                    response.Result = ComplaintResult.Failed;
                    return response;
                }
                entity.Active = false;
                entity.DeletedBy = _applicationContext.UserId;
                entity.DeletedOn = DateTime.Now;
                _repo.EditEntity(entity);
                await _repo.Save();

            }
            else
            {
                response.Result = ComplaintResult.Failed;
                return response;
            }

            response.Result = ComplaintResult.Success;
            return response;
        }
        /// Create Complaint entity
        private CompComplaint CreateComplaintEntity(ComplaintDetailedInfo req)
        {

            var _complaint = new CompComplaint()
            {
                Active = true,
                Type = req.ComplaintTypeId,
                ComplaintDate = req.ComplaintDate.ToDateTime(),
                RecipientType = req.RecipientTypeId,
                Department = req.DepartmentId,
                Remarks = req.Remarks,
                CreatedBy = _applicationContext.UserId,
                CreatedOn = DateTime.Now,
                Service = req.ServiceId,
                CustomerId = req.CustomerId,
                Country = req.CountryId,
                Office = req.OfficeId,
                InspectionId = req.ServiceId == (int)Service.InspectionId ? req.BookingNo : null,
                AuditId = req.ServiceId == (int)Service.AuditId ? req.BookingNo : null,
            };
            return _complaint;

        }
        /// Create ComplaintDetails entity
        private void AddComplaintDetails(ComplaintDetailedInfo request, CompComplaint entity)
        {
            if (request.ComplaintDetails != null && request.ComplaintDetails.Any())
            {
                foreach (var complaint in request.ComplaintDetails)
                {
                    var _complaintDetail = new CompTranComplaintsDetail()
                    {
                        ProductId = complaint.ProductId,
                        ComplaintCategory = complaint.CategoryId,
                        ComplaintDescription = complaint.Description,
                        CorrectiveAction = complaint.CorrectiveAction,
                        AnswerDate = complaint.AnswerDate?.ToDateTime(),
                        Remarks = complaint.Remarks,
                        Title = complaint.Title,
                        Active = true,
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };
                    entity.CompTranComplaintsDetails.Add(_complaintDetail);
                    _repo.AddEntity(_complaintDetail);
                }
            }
        }
        /// Create Personincharge entity
        private void AddPersonIncharge(ComplaintDetailedInfo request, CompComplaint entity)
        {
            if (request.UserIds != null && request.UserIds.Any())
            {
                foreach (var userId in request.UserIds)
                {
                    var _personIncharge = new CompTranPersonInCharge()
                    {
                        Active = true,
                        PsersonInCharge = userId,
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };
                    entity.CompTranPersonInCharges.Add(_personIncharge);
                    _repo.AddEntity(_personIncharge);
                }
            }
        }
        /// Update Complaint entity
        private CompComplaint UpdateComplaintEntity(ComplaintDetailedInfo req, CompComplaint entity)
        {
            entity.Active = true;
            entity.Type = req.ComplaintTypeId;
            entity.ComplaintDate = req.ComplaintDate.ToDateTime();
            entity.RecipientType = req.RecipientTypeId;
            entity.Department = req.DepartmentId;
            entity.Remarks = req.Remarks;
            entity.Service = req.ServiceId;
            entity.CustomerId = req.CustomerId;
            entity.Country = req.CountryId;
            entity.Office = req.OfficeId;
            entity.InspectionId = req.ServiceId == (int)Service.InspectionId ? req.BookingNo : null;
            entity.AuditId = req.ServiceId == (int)Service.AuditId ? req.BookingNo : null;
            return entity;
        }
        /// Update PersonIncharge entity
        private void UpdatePersonIncharge(IEnumerable<int> UserIds, CompComplaint entity)
        {
            if (UserIds != null)
            {
                //remove PersonInCharge
                var UsersToRemove = entity.CompTranPersonInCharges.Where(x => !UserIds.Contains(x.PsersonInCharge) && x.Active);
                var lstpersonToremove = new List<CompTranPersonInCharge>();
                foreach (var item in UsersToRemove)
                {
                    item.DeletedOn = DateTime.Now;
                    item.DeletedBy = _applicationContext.UserId;
                    item.Active = false;
                    lstpersonToremove.Add(item);
                }
                if (lstpersonToremove.Count > 0)
                    _repo.EditEntities(lstpersonToremove);

                //Add PersonInCharge
                var UsersToAdd = UserIds.Where(x => !entity.CompTranPersonInCharges.Select(y => y.PsersonInCharge).Contains(x));
                foreach (var userId in UsersToAdd)
                {
                    var _personIncharge = new CompTranPersonInCharge()
                    {
                        Active = true,
                        PsersonInCharge = userId,
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };
                    entity.CompTranPersonInCharges.Add(_personIncharge);
                    _repo.AddEntity(_personIncharge);
                }
            }
        }
        /// Update ComplaintDetails entity
        private void UpdateComplaintDetails(IEnumerable<ComplaintDetail> complaintDetails, CompComplaint entity)
        {
            if (complaintDetails != null)
            {
                //remove details
                var complaintDetailIds = complaintDetails.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
                var complaintDetailsToRemove = entity.CompTranComplaintsDetails.Where(x => !complaintDetailIds.Contains(x.Id));
                var lstDetailsToremove = new List<CompTranComplaintsDetail>();
                foreach (var item in complaintDetailsToRemove)
                {
                    item.DeletedOn = DateTime.Now;
                    item.DeletedBy = _applicationContext.UserId;
                    item.Active = false;
                    lstDetailsToremove.Add(item);
                }
                if (lstDetailsToremove.Count > 0)
                    _repo.EditEntities(lstDetailsToremove);

                //Add new details
                foreach (var item in complaintDetails.Where(x => x.Id <= 0))
                {
                    var _complaintDetail = new CompTranComplaintsDetail()
                    {
                        ProductId = item.ProductId,
                        ComplaintCategory = item.CategoryId,
                        ComplaintDescription = item.Description,
                        CorrectiveAction = item.CorrectiveAction,
                        AnswerDate = item.AnswerDate?.ToDateTime(),
                        Remarks = item.Remarks,
                        Title = item.Title,
                        Active = true,
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };
                    entity.CompTranComplaintsDetails.Add(_complaintDetail);
                    _repo.AddEntity(_complaintDetail);
                }

                //Update details
                var lstDetailsToUpdate = new List<CompTranComplaintsDetail>();
                var removeIds = complaintDetailsToRemove.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
                foreach (var item in complaintDetails.Where(x => x.Id > 0))
                {
                    var detailsToUpdate = entity.CompTranComplaintsDetails.Where(x => !removeIds.Contains(x.Id) && x.Id == item.Id).FirstOrDefault();
                    if (detailsToUpdate != null)
                    {
                        detailsToUpdate.ProductId = item.ProductId;
                        detailsToUpdate.ComplaintCategory = item.CategoryId;
                        detailsToUpdate.ComplaintDescription = item.Description;
                        detailsToUpdate.CorrectiveAction = item.CorrectiveAction;
                        detailsToUpdate.AnswerDate = item.AnswerDate?.ToDateTime();
                        detailsToUpdate.Remarks = item.Remarks;
                        detailsToUpdate.Title = item.Title;
                        lstDetailsToUpdate.Add(detailsToUpdate);
                    }
                }
                if (lstDetailsToUpdate.Count > 0)
                    _repo.EditEntities(lstDetailsToUpdate);

            }
        }

        /// <summary>
        /// get the complaint summary data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ComplaintSummaryResponse> GetComplaintSummary(ComplaintSummaryRequest request)
        {
            if (request == null)
                return new ComplaintSummaryResponse() { Result = ComplaintResult.CannotGetList };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            var response = new ComplaintSummaryResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            //get the IQueryable data
            var res = _repo.GetComplaintData();

            //filter the data based on request DTO
            res = GetComplaintQuerywithRequestFilters(request, res);

            //get the inspection, audit and general
            var result = await GetComplaintBookingData(res, skip, take);
            if (result != null && result.Count < take)
            {
                var auditdata = await GetComplaintAuditData(res, skip, take);
                if (auditdata != null && auditdata.Any())
                {
                    result = result.Concat(auditdata).ToList();
                }
            }
            if (result != null && result.Count < take)
            {
                var generaldata = await GetComplaintGeneralData(res, skip, take);
                if (generaldata != null && generaldata.Any())
                {
                    result = result.Concat(generaldata).ToList();
                }

            }

            ////apply skip and take
            // var data = await GetComplaintData(result, skip, take);

            if (result == null || !result.Any())
            {
                return new ComplaintSummaryResponse { Result = ComplaintResult.CannotGetList };
            }

            response.TotalCount = await res.CountAsync();

            var resultSet = result.Select(x => _complaintMap.GetComplaintSummaryData(x));

            return new ComplaintSummaryResponse
            {
                TotalCount = response.TotalCount,
                Index = request.Index.Value,
                PageSize = request.pageSize.Value,
                PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                Data = resultSet.ToList(),
                Result = ComplaintResult.Success
            };
        }

        /// <summary>
        /// Filter the data based on the Reques
        /// </summary>
        /// <param name="request"></param>
        /// <param name="inspectionQuery"></param>
        /// <returns></returns>
        public IQueryable<CompComplaint> GetComplaintQuerywithRequestFilters(ComplaintSummaryRequest request, IQueryable<CompComplaint> inspectionQuery)
        {
            //search by inspection booking no/ customer booking no
            if (!string.IsNullOrWhiteSpace(request.SearchTypeText?.Trim()) && (Enum.TryParse(request.SearchTypeId.ToString(), out SearchType _numberSearchTypeEnum)))
            {
                if (request.ServiceId > 0)
                {
                    switch (_numberSearchTypeEnum)
                    {
                        case SearchType.BookingNo:
                            {
                                if (int.TryParse(request.SearchTypeText?.Trim(), out int bookid))
                                    inspectionQuery = request.ServiceId.GetValueOrDefault() == (int)Service.InspectionId ? inspectionQuery.Where(x => x.InspectionId.HasValue && x.InspectionId == bookid) :
                                    inspectionQuery.Where(x => x.AuditId.HasValue && x.AuditId == bookid);
                                break;
                            }
                        case SearchType.CustomerBookingNo:
                            {
                                inspectionQuery = request.ServiceId.GetValueOrDefault() == (int)Service.InspectionId ? inspectionQuery.Where(x => EF.Functions.Like(x.Inspection.CustomerBookingNo.Trim(), $"%{request.SearchTypeText.Trim()}%"))
                                    : inspectionQuery.Where(x => EF.Functions.Like(x.Audit.CustomerBookingNo.Trim(), $"%{request.SearchTypeText.Trim()}%"));
                                break;
                            }
                    }
                }
            }

            //filter by creation date or service date or complaint date
            if (Enum.TryParse(request.Datetypeid.ToString(), out SearchType _datesearchtype) && (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null))
            {
                switch (_datesearchtype)
                {
                    case SearchType.ApplyDate:
                        {
                            inspectionQuery = inspectionQuery.Where(x => EF.Functions.DateDiffDay(request.FromDate.ToDateTime(), x.CreatedOn) >= 0 &&
                                                 EF.Functions.DateDiffDay(x.CreatedOn, request.ToDate.ToDateTime()) >= 0);
                            break;
                        }
                    case SearchType.ServiceDate:
                        {
                            inspectionQuery = request.ServiceId.GetValueOrDefault() == (int)Service.InspectionId ? inspectionQuery.Where(x => !((x.Inspection.ServiceDateFrom > request.ToDate.ToDateTime()) || (x.Inspection.ServiceDateTo < request.FromDate.ToDateTime())))
                                : inspectionQuery.Where(x => !((x.Audit.ServiceDateFrom > request.ToDate.ToDateTime()) || (x.Audit.ServiceDateTo < request.FromDate.ToDateTime())));
                            break;
                        }
                    case SearchType.ComplaintDate:
                        {
                            inspectionQuery = inspectionQuery.Where(x => EF.Functions.DateDiffDay(request.FromDate.ToDateTime(), x.ComplaintDate) >= 0 &&
                                                 EF.Functions.DateDiffDay(x.ComplaintDate, request.ToDate.ToDateTime()) >= 0);
                            break;
                        }
                }
            }

            //advance search type (ponmber,product name)
            if ((!string.IsNullOrEmpty(request.AdvancedsearchtypeText?.Trim())) && (Enum.TryParse(request.AdvancedSearchtypeId.ToString(), out AdvanceSearchType _advanceseachtypeenum)))
            {
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    switch (_advanceseachtypeenum)
                    {
                        case AdvanceSearchType.ProductName:
                            {
                                inspectionQuery = inspectionQuery.Where(x => x.Inspection.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && EF.Functions.Like(y.Product.ProductId.Trim(), $"%{request.AdvancedsearchtypeText.Trim()}%")));
                                break;
                            }
                        case AdvanceSearchType.PoNo:
                            {
                                inspectionQuery = inspectionQuery.Where(x => x.Inspection.InspPurchaseOrderTransactions.Any(y => y.Active.HasValue && y.Active.Value && EF.Functions.Like(y.Po.Pono, $"%{request.AdvancedsearchtypeText.Trim()}%")));
                                break;
                            }
                    }
                }

            }

            if (request.ComplaintTypeId > 0)
            {
                inspectionQuery = inspectionQuery.Where(x => x.Type == request.ComplaintTypeId);
            }

            if (request.ServiceId > 0)
            {
                inspectionQuery = inspectionQuery.Where(x => x.Service == request.ServiceId);
            }

            if (request.CustomerId > 0)
            {
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    inspectionQuery = inspectionQuery.Where(x => x.Inspection.CustomerId == request.CustomerId);
                }
                else if (request.ServiceId == (int)Service.AuditId)
                {
                    inspectionQuery = inspectionQuery.Where(x => x.Audit.CustomerId == request.CustomerId);
                }
                else
                {
                    inspectionQuery = inspectionQuery.Where(x => x.CustomerId == request.CustomerId);
                }
            }

            return inspectionQuery;
        }

        /// <summary>
        /// execute the complaints with booking complaint type - inspection
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private async Task<List<ComplaintSummaryRepoResult>> GetComplaintBookingData(IQueryable<CompComplaint> bookingData, int skip, int take)
        {
            return await bookingData.Where(x => x.Service == (int)Service.InspectionId).Select(x => new ComplaintSummaryRepoResult
            {
                Id = x.Id,
                BookingId = x.InspectionId,
                CustomerId = x.Inspection.CustomerId,
                CustomerName = x.Inspection.Customer.CustomerName,
                ServiceId = x.Service,
                ServiceName = x.ServiceNavigation.Name,
                ComplaintTypeId = x.Type,
                ComplaintTypeName = x.TypeNavigation.Name,
                ComplaintDate = x.ComplaintDate,
                ServiceDateFrom = x.Inspection.ServiceDateFrom,
                ServiceDateTo = x.Inspection.ServiceDateTo,
                CustomerBookingNo = x.Inspection.CustomerBookingNo,
                CreatedOn = x.CreatedOn,
                CreatedBy = x.CreatedByNavigation.FullName
            }).OrderByDescending(z => z.Id).Skip(skip).Take(take).AsNoTracking().ToListAsync();

        }
        /// <summary>
        /// execute the complaints with booking complaint type - audits
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private async Task<List<ComplaintSummaryRepoResult>> GetComplaintAuditData(IQueryable<CompComplaint> auditDataData, int skip, int take)
        {
            return await auditDataData.Where(x => x.Service == (int)Service.AuditId).Select(x => new ComplaintSummaryRepoResult
            {
                Id = x.Id,
                BookingId = x.AuditId,
                CustomerId = x.Audit.CustomerId,
                CustomerName = x.Audit.Customer.CustomerName,
                ServiceId = x.Service,
                ServiceName = x.ServiceNavigation.Name,
                ComplaintTypeId = x.Type,
                ComplaintTypeName = x.TypeNavigation.Name,
                ComplaintDate = x.ComplaintDate,
                ServiceDateFrom = x.Audit.ServiceDateFrom,
                ServiceDateTo = x.Audit.ServiceDateTo,
                CustomerBookingNo = x.Audit.CustomerBookingNo,
                CreatedOn = x.CreatedOn,
                CreatedBy = x.CreatedByNavigation.FullName
            }).OrderByDescending(z => z.Id).Skip(skip).Take(take).AsNoTracking().ToListAsync();

        }
        /// <summary>
        /// execute the complaints with general complaint type
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private async Task<List<ComplaintSummaryRepoResult>> GetComplaintGeneralData(IQueryable<CompComplaint> bookingData, int skip, int take)
        {
            return await bookingData.Where(x => x.Service == null).Select(x => new ComplaintSummaryRepoResult
            {
                Id = x.Id,
                BookingId = x.InspectionId,
                CustomerId = x.CustomerId,
                CustomerName = x.Customer.CustomerName,
                ServiceId = x.Service,
                ServiceName = x.ServiceNavigation.Name,
                ComplaintTypeId = x.Type,
                ComplaintTypeName = x.TypeNavigation.Name,
                ComplaintDate = x.ComplaintDate,
                CreatedOn = x.CreatedOn,
                CreatedBy = x.CreatedByNavigation.FullName
            }).OrderByDescending(z => z.Id).Skip(skip).Take(take).AsNoTracking().ToListAsync(); ;

        }

        /// <summary>
        /// execute the query
        /// </summary>
        /// <param name="data"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private async Task<List<ComplaintSummaryRepoResult>> GetComplaintData(IQueryable<ComplaintSummaryRepoResult> data, int skip, int take)
        {
            return await data.Select(x => new ComplaintSummaryRepoResult
            {
                Id = x.Id,
                BookingId = x.BookingId,
                CustomerId = x.CustomerId,
                CustomerName = x.CustomerName,
                ServiceId = x.ServiceId,
                ServiceName = x.ServiceName,
                ComplaintTypeId = x.ComplaintTypeId,
                ComplaintTypeName = x.ComplaintTypeName,
                ComplaintDate = x.ComplaintDate,
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo,
                CustomerBookingNo = x.CustomerBookingNo,
                CreatedOn = x.CreatedOn,
                CreatedBy = x.CreatedBy
            }).OrderByDescending(z => z.Id).Skip(skip).Take(take).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// Delete the complaint
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteComplaintResponse> DeleteComplaint(int id)
        {
            var response = new DeleteComplaintResponse();

            var entity = await _repo.GetComplaintsItemsById(id);

            if (entity == null)
                return new DeleteComplaintResponse { Result = ComplaintResult.RequestNotCorrectFormat };

            //update COMP_TRAN_PersonInCharge table data
            foreach (var item in entity.CompTranPersonInCharges)
            {
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = _applicationContext.UserId;
                item.Active = false;
            }

            //update COMP_TRAN_ComplaintsDetails table data
            foreach (var item in entity.CompTranComplaintsDetails)
            {
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = _applicationContext.UserId;
                item.Active = false;
            }

            //update comp_complaints table data
            entity.Active = false;
            entity.DeletedBy = _applicationContext.UserId;
            entity.DeletedOn = DateTime.Now;

            await _repo.EditComplaints(entity);
            response.Id = entity.Id;
            response.Result = ComplaintResult.Success;

            return response;
        }

        public async Task<ExportComplaintSummaryResponse> GetComplaintDetails(ComplaintSummaryRequest request)
        {
            if (request == null)
                return new ExportComplaintSummaryResponse() { Result = ComplaintResult.CannotGetList };

            IEnumerable<ServiceTypeList> serviceTypeList = new List<ServiceTypeList>();

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            var response = new ComplaintSummaryResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            //get the IQueryable data
            var res = _repo.GetComplaintData();

            //filter the data based on request DTO
            res = GetComplaintQuerywithRequestFilters(request, res);

            //get the inspection, audit and general
            var result = await GetComplaintBookingDataForExport(res, skip, take);
            if (result != null && result.Count < take)
            {
                var auditdata = await GetComplaintAuditDataForExport(res, skip, take);
                if (auditdata != null && auditdata.Any())
                {
                    result = result.Concat(auditdata).ToList();
                }
            }
            if (result != null && result.Count < take)
            {
                var generaldata = await GetComplaintGeneralDataForExport(res, skip, take);
                if (generaldata != null && generaldata.Any())
                {
                    result = result.Concat(generaldata).ToList();
                }

            }

            if (result == null || !result.Any())
            {
                return new ExportComplaintSummaryResponse { Result = ComplaintResult.CannotGetList };
            }

            response.TotalCount = await res.CountAsync();
            var allComplaintIds = result.Select(x => x.Id).ToList();
            if (request.ServiceId == (int)Service.InspectionId)
            {
                var allBookingIds = result.Select(x => x.BookingId.GetValueOrDefault());
                serviceTypeList = await _inspRepo.GetServiceType(allBookingIds);
            }
            else if(request.ServiceId == (int)Service.AuditId)
            {
                var allBookingIds = result.Select(x => x.AuditId.GetValueOrDefault());
                serviceTypeList = await _inspRepo.GetAuditServiceType(allBookingIds);
            }

            var complaintDetailsData = await _repo.GetComplaintDetailItemByComplaintIds(allComplaintIds);
            var complaintPersonIncharge = await _repo.GetComplaintPersonInchargeByComplaintIds(allComplaintIds);

            var resultSet = result.Select(x => _complaintMap.GetComplaintExportSummaryData(x, request.ServiceId, serviceTypeList, complaintDetailsData, complaintPersonIncharge));

            return new ExportComplaintSummaryResponse
            {
                TotalCount = response.TotalCount,
                Index = request.Index.Value,
                PageSize = request.pageSize.Value,
                PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                Data = resultSet.ToList(),
                Result = ComplaintResult.Success
            };
        }

        private async Task<List<ExportComplaintSummaryRepoResult>> GetComplaintBookingDataForExport(IQueryable<CompComplaint> bookingData, int skip, int take)
        {
            return await bookingData.Where(x => x.Service == (int)Service.InspectionId).Select(x => new ExportComplaintSummaryRepoResult
            {
                Id = x.Id,
                BookingId = x.InspectionId,
                AuditId = x.AuditId,
                CustomerName = x.Inspection.Customer.CustomerName,
                ComplaintTypeName = x.TypeNavigation.Name,
                ComplaintOffice = x.OfficeNavigation.LocationName,
                ComplaintCountry = x.CountryNavigation.CountryName,
                ComplaintDate = x.ComplaintDate,
                ServiceName = x.ServiceNavigation.Name,
                ServiceDateFrom = x.Inspection.ServiceDateFrom,
                ServiceDateTo = x.Inspection.ServiceDateTo,
                SupplierName = x.Inspection.Supplier.SupplierName,
                Factory = x.Inspection.Factory.SupplierName,
                Department = x.DepartmentNavigation.Name,
                RecipientType = x.RecipientTypeNavigation.Name,
                Remarks = x.Remarks
            }).OrderByDescending(z => z.Id).Skip(skip).Take(take).AsNoTracking().ToListAsync();

        }

        private async Task<List<ExportComplaintSummaryRepoResult>> GetComplaintAuditDataForExport(IQueryable<CompComplaint> auditDataData, int skip, int take)
        {
            return await auditDataData.Where(x => x.Service == (int)Service.AuditId).Select(x => new ExportComplaintSummaryRepoResult
            {
                Id = x.Id,
                BookingId = x.InspectionId,
                AuditId = x.AuditId,
                CustomerName = x.Audit.Customer.CustomerName,
                ComplaintTypeName = x.TypeNavigation.Name,
                ComplaintOffice = x.OfficeNavigation.LocationName,
                ComplaintCountry = x.CountryNavigation.CountryName,
                ComplaintDate = x.ComplaintDate,
                ServiceName = x.ServiceNavigation.Name,
                ServiceDateFrom = x.Audit.ServiceDateFrom,
                ServiceDateTo = x.Audit.ServiceDateTo,
                SupplierName = x.Audit.Supplier.SupplierName,
                Factory = x.Audit.Factory.SupplierName,
                Department = x.DepartmentNavigation.Name,
                RecipientType = x.RecipientTypeNavigation.Name,
                Remarks = x.Remarks
            }).OrderByDescending(z => z.Id).Skip(skip).Take(take).AsNoTracking().ToListAsync();

        }
        /// <summary>
        /// execute the complaints with general complaint type
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private async Task<List<ExportComplaintSummaryRepoResult>> GetComplaintGeneralDataForExport(IQueryable<CompComplaint> bookingData, int skip, int take)
        {
            return await bookingData.Where(x => x.Service == null).Select(x => new ExportComplaintSummaryRepoResult
            {
                Id = x.Id,
                BookingId = x.InspectionId,
                AuditId = x.AuditId,
                CustomerName = x.Customer.CustomerName,
                ComplaintTypeName = x.TypeNavigation.Name,
                ComplaintOffice = x.OfficeNavigation.LocationName,
                ComplaintCountry = x.CountryNavigation.CountryName,
                ComplaintDate = x.ComplaintDate,
                ServiceName = x.ServiceNavigation.Name,
                Department = x.DepartmentNavigation.Name,
                RecipientType = x.RecipientTypeNavigation.Name,
                Remarks = x.Remarks
            }).OrderByDescending(z => z.Id).Skip(skip).Take(take).AsNoTracking().ToListAsync();

        }

        public async Task<List<ExportComplaintSummaryResult>> ExportCompalintSummary(IEnumerable<ExportComplaintSummaryRepoResult> data)
        {
            return _complaintMap.MapExportCompalintSummary(data);
        }
    }
}
