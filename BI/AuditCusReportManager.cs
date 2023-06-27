using BI.Maps;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.AuditReport;
using DTO.Common;
using DTO.RepoRequest.Enum;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class AuditCusReportManager : ApiCommonData, IAuditCusReportManager
    {
        public readonly IAPIUserContext _ApplicationContext = null;
        public readonly IOfficeLocationManager _officemanager = null;
        public readonly IAuditCusReportRepository _repo = null;
        public readonly IAuditRepository _auditrepo = null;
        private readonly IFileManager _fileManager = null;
        private readonly AuditMap _auditmap = null;
        public AuditCusReportManager(IAPIUserContext ApplicationContext, IOfficeLocationManager officemanager,
            IAuditCusReportRepository repo, IAuditRepository auditrepo, IFileManager fileManager)
        {
            _ApplicationContext = ApplicationContext;
            _officemanager = officemanager;
            _repo = repo;
            _auditrepo = auditrepo;
            _fileManager = fileManager;
            _auditmap = new AuditMap();
        }

        public async Task<AuditCusReportBookingDetailsResponse> SearchAuditCusReport(AuditCusReportBookingDetailsRequest request)
        {
            try
            {
                //filter data based on user type
                switch (_ApplicationContext.UserType)
                {
                    case UserTypeEnum.Customer:
                        {
                            request.CustomerId = request?.CustomerId != null && request?.CustomerId != 0 ? request?.CustomerId : _ApplicationContext.CustomerId;
                            break;
                        }
                    case UserTypeEnum.Factory:
                        {
                            request.FactoryIdlst = request.FactoryIdlst != null && request.FactoryIdlst.Count() > 0 ? request.FactoryIdlst : new List<int>().Append(_ApplicationContext.FactoryId);
                            break;
                        }
                    case UserTypeEnum.Supplier:
                        {
                            request.SupplierId = request.SupplierId != null && request.SupplierId != 0 ? request.SupplierId.Value : _ApplicationContext.SupplierId;
                            break;
                        }
                }

                if (request.Index == null || request.Index.Value <= 0)
                    request.Index = 1;

                if (request.pageSize == null || request.pageSize.Value == 0)
                    request.pageSize = 20;

                int skip = (request.Index.Value - 1) * request.pageSize.Value;

                int take = request.pageSize.Value;

                // set the customer list based on user access
                var cuslist = new List<int>();
                if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
                {
                    if (request.Officeidlst != null && request.Officeidlst.Count() == 0)
                    {
                        var _cusofficelist = _officemanager.GetOfficesByUserId(_ApplicationContext.StaffId);
                        request.Officeidlst = _cusofficelist == null || _cusofficelist.Count() == 0 ? request.Officeidlst : _cusofficelist.Select(x => (int?)x.Id);
                    }

                    if (request?.CustomerId != null)
                        cuslist.Add(request.CustomerId.Value);
                }
                else
                {
                    if (request?.CustomerId != null)
                        cuslist.Add(request.CustomerId.Value);
                }

                //get audit booking details
                var auditbookinglst =  _auditrepo.GetAuditMainData();

                //filter the data

                if (request != null && cuslist.Count() > 0)
                {
                    auditbookinglst = auditbookinglst.Where(x => cuslist.Contains(x.CustomerId));
                }

                if (request != null && request.SupplierId!=null && request.SupplierId != 0)
                {
                    auditbookinglst = auditbookinglst.Where(x => x.SupplierId == request.SupplierId.Value);
                }

                if (request != null && request.FactoryIdlst != null && request.FactoryIdlst.Count() > 0)
                {
                    auditbookinglst = auditbookinglst.Where(x => request.FactoryIdlst.ToList().Contains(x.FactoryId));
                }

                if (request != null && request.Officeidlst != null && request.Officeidlst.Count() > 0)
                {
                    auditbookinglst = auditbookinglst.Where(x => x.OfficeId.HasValue && request.Officeidlst.ToList().Contains(x.OfficeId.Value));
                }

                if (request != null && request.StatusIdlst != null && request.StatusIdlst.Count() > 0)
                {
                    auditbookinglst = auditbookinglst.Where(x => request.StatusIdlst.ToList().Contains(x.StatusId));
                }

                //Filter based on Service type
                if (request.ServiceTypelst != null && request.ServiceTypelst.Count() > 0)
                {
                    auditbookinglst = auditbookinglst.Where(x => x.AudTranServiceTypes.Any(y => y.Active && request.ServiceTypelst.Contains(y.ServiceTypeId)));
                }

                //apply factory country filter
                if (request.FactoryCountryIdList != null && request.FactoryCountryIdList.Any())
                {
                    auditbookinglst = auditbookinglst.Where(x => x.Factory.SuAddresses.Any(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice && request.FactoryCountryIdList.Contains(y.CountryId)));
                }

                //if the user role is qc then we are filter the qc data
                if (_ApplicationContext.RoleList.Any(x => x == (int)RoleEnum.Inspector))
                {
                    var staffId = _ApplicationContext.StaffId;
                    auditbookinglst = auditbookinglst.Where(x => x.AudTranAuditors.Any(y => y.StaffId == staffId && y.Active));
                }

                if (Enum.TryParse(request.SearchTypeId.ToString(), out SearchType _seachtypeenum))
                {
                    switch (_seachtypeenum)
                    {
                        case SearchType.BookingNo:
                            {
                                if (!string.IsNullOrEmpty(request.SearchTypeText) && int.TryParse(request.SearchTypeText, out int bookid))
                                {
                                    auditbookinglst = auditbookinglst.Where(x => x.Id == bookid);
                                }
                                break;
                            }
                        case SearchType.ReportNo:
                            {
                                if (!string.IsNullOrEmpty(request.SearchTypeText))
                                {
                                    auditbookinglst = auditbookinglst.Where(x => x.ReportNo.Contains(request.SearchTypeText));
                                }
                                break;
                            }
                        case SearchType.CustomerBookingNo:
                            {
                                if (!string.IsNullOrEmpty(request.SearchTypeText))
                                {
                                    auditbookinglst = auditbookinglst.Where(x => x.CustomerBookingNo.Contains(request.SearchTypeText));
                                }
                                break;
                            }

                    }
                    if (Enum.TryParse(request.DateTypeid.ToString(), out SearchType _datesearchtype))
                    {
                        switch (_datesearchtype)
                        {
                            case SearchType.ApplyDate:
                                {
                                    if (request.FromDate != null && request.ToDate != null)
                                    {
                                        auditbookinglst = auditbookinglst.Where(x =>x.CreatedOn.HasValue && EF.Functions.DateDiffDay(request.FromDate.ToDateTime(), x.CreatedOn) >= 0 &&
                                                        EF.Functions.DateDiffDay(x.CreatedOn, request.ToDate.ToDateTime()) >= 0);
                                    }
                                    break;
                                }
                            case SearchType.ServiceDate:
                                {
                                    if (request.FromDate != null && request.ToDate != null)
                                    {
                                        auditbookinglst = auditbookinglst.Where(x => !((x.ServiceDateFrom > request.ToDate.ToDateTime()) || (x.ServiceDateTo < request.FromDate.ToDateTime())));
                                    }
                                    break;
                                }
                        }
                    }
                }

                var Audstatuslst = await auditbookinglst.Select(x => new { x.StatusId, StatusName = x.Status.Status }).GroupBy(p => new { p.StatusId, p.StatusName }, p => p, (key, _data) =>
                                  new DTO.Audit.AuditStatus
                                  {
                                      Id = key.StatusId,
                                      StatusName = key.StatusName,
                                      TotalCount = _data.Count(),
                                      StatusColor = AuditStatusColor.GetValueOrDefault(key.StatusId, "")
                                  }).AsNoTracking().ToListAsync();



                var result = await auditbookinglst.OrderByDescending(x => x.ServiceDateTo).Skip(skip).Take(take)
                    .Select(x => new AuditRepoCusReportBookingDetails
                    {
                        AuditId = x.Id,
                        Customer = x.Customer.CustomerName,
                        Factory = x.Factory.SupplierName,
                        officeName = x.Office.LocationName,
                        ReportNo = x.ReportNo,
                        CustomerBookingNo = x.CustomerBookingNo,
                        ServiceFromDate = x.ServiceDateFrom,
                        ServiceToDate = x.ServiceDateTo,
                        Supplier = x.Supplier.SupplierName,
                        CustomerId = x.CustomerId,
                        FactoryId = x.FactoryId,
                        SupplierId = x.SupplierId,
                        officeId = x.OfficeId,
                        StatusId = x.StatusId,
                        StatusName = x.Status.Status,
                        CreatedOn = x.CreatedOn,
                        ReportId = x.FbreportId,
                        ReportUrl = x.FinalReportPath
                    }).AsNoTracking().ToListAsync();

                if (result == null || !result.Any())
                    return new AuditCusReportBookingDetailsResponse() { Result = AuditCusReportBookingDetailsResult.NotFound };

                //get all bookingid 
                var lstbookingid = result.Select(x => x.AuditId).Distinct().ToList();

                //  fetch service type list
                var lstauditservicetype = await _repo.GetAuditserviceType(lstbookingid);

                var _auditFactoryCountryList = await _repo.GetFactorycountryByAuditIds(lstbookingid);

                //get the booking count after filter the data
                var lstauditcountafterfilter = await auditbookinglst.Select(x => x.StatusId).ToListAsync();
                var totalcount = lstauditcountafterfilter.Count();

                //  fetch audit report type list
                var lstsuditexists = await _repo.IsAuditReportExists(lstbookingid);

                //map the details
                var mapauditlst = result.Select(x => _auditmap.GetAuditCusReport(x, lstsuditexists, lstauditservicetype, _auditFactoryCountryList, (y) => _fileManager.GetMimeType(y)));

                return new AuditCusReportBookingDetailsResponse()
                {
                    Result = AuditCusReportBookingDetailsResult.Success,
                    TotalCount = totalcount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (totalcount / request.pageSize.Value) + (totalcount % request.pageSize.Value > 0 ? 1 : 0),
                    Data = mapauditlst,
                    AuditStatuslst = Audstatuslst,
                };

            }
            catch(Exception ex)
            {
                return new AuditCusReportBookingDetailsResponse() { Result = AuditCusReportBookingDetailsResult.Error };
            }
        }
    }
}
