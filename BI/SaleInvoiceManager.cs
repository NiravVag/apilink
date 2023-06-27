using BI.Maps;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Invoice;
using DTO.RepoRequest.Enum;
using DTO.SaleInvoice;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class SaleInvoiceManager : ApiCommonData, ISaleInvoiceManager
    {
        private readonly ISaleInvoiceRepository _repo = null;
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly SaleInvoiceMap _saleInvoiceMap = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IInvoiceDataAccessRepository _invoiceDataAccessRepository = null;
        private readonly IOfficeLocationManager _officeLocationManager = null;
        private readonly IUserRepository _userRepository = null;
        private readonly ICustomerRepository _customerRepository = null;

        public SaleInvoiceManager(ISaleInvoiceRepository repo, IInvoiceRepository invoiceRepository, IAPIUserContext ApplicationContext,
            IInvoiceDataAccessRepository invoiceDataAccessRepository, IOfficeLocationManager officeLocationManager, IUserRepository userRepository,
            ICustomerRepository customerRepository)
        {
            _repo = repo;
            _invoiceRepository = invoiceRepository;
            _saleInvoiceMap = new SaleInvoiceMap();
            _ApplicationContext = ApplicationContext;
            _invoiceDataAccessRepository = invoiceDataAccessRepository;
            _officeLocationManager = officeLocationManager;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
        }

        public async Task<SaleInvoiceSummaryResponse> GetSaleInvoiceSummary(SaleInvoiceSummaryRequest request)
        {
            if (request == null)
                return new SaleInvoiceSummaryResponse() { Result = SaleInvoiceSummaryResult.RequestNotCorrectFormat };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            request = await GetInvoiceSummaryRequest(request);

            var data = await GetAllInvoiceSummary(request);

            if (data.InvoiceDataList == null || !data.InvoiceDataList.Any())
                return new SaleInvoiceSummaryResponse() { Result = SaleInvoiceSummaryResult.NotFound };
            var invoiceIdList = data.InvoiceDataList.Select(x => x.Id).ToList();

            var invoiceFiles = await _repo.GetInvoiceTransactionFiles(invoiceIdList);
            var result = _saleInvoiceMap.MapSaleInvoiceSummary(data.InvoiceDataList, data.InvoiceExtraFeeList, invoiceFiles);

            var response = new SaleInvoiceSummaryResponse()
            {
                Result = SaleInvoiceSummaryResult.Success,
                Data = result,
                TotalCount = data.TotalCount,
                PageSize = request.pageSize.Value,
                Index = request.Index.Value,
                PaymentStatusCountList = data.PaymentStatusCountList,
                PageCount = (result.Count / request.pageSize.Value) + (result.Count % request.pageSize.Value > 0 ? 1 : 0)
            };

            return response;
        }

        public async Task<dynamic> ExportSaleInvoiceSearchSummary(SaleInvoiceSummaryRequest request)
        {
            request = await GetInvoiceSummaryRequest(request);
            var data = await GetAllInvoiceSummary(request);

            if (data.InvoiceDataList == null || !data.InvoiceDataList.Any())
                return null;

            if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
                return _saleInvoiceMap.MapExportSalesInvoiceForInternalUserSummary(data.InvoiceDataList, data.InvoiceExtraFeeList);
            else
                return _saleInvoiceMap.MapExportSalesInvoiceForExternalUsersSummary(data.InvoiceDataList, data.InvoiceExtraFeeList);
        }

        private async Task<SaleInvoiceSummaryRequest> GetInvoiceSummaryRequest(SaleInvoiceSummaryRequest request)
        {
            request.CustomerIdList = new List<int>();
            //filter data based on user type
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.InternalUser:
                    {
                        var _cusofficelist = await _officeLocationManager.GetOnlyOfficeIdsByUser(_ApplicationContext.StaffId);

                        if (_cusofficelist.Any())
                        {
                            if (request.OfficeIdList != null && request.OfficeIdList.Any())
                            {
                                request.OfficeIdList = _cusofficelist.Where(x => request.OfficeIdList.Contains(x)).Select(x => (int?)x).ToList();
                            }
                            else
                            {
                                request.OfficeIdList = _cusofficelist.Select(x => (int?)x).ToList();
                            }
                        }
                        break;
                    }
                case UserTypeEnum.Customer:
                    {
                        if (!(request.CustomerId > 0))
                        {
                            var contactId = await _userRepository.GetCustomerContactIdByUserId(_ApplicationContext.UserId);
                            var sisterCompanyIds = await _customerRepository.GetSisterCompanieIdsByCustomerContactId(contactId);
                            request.CustomerIdList.Add(_ApplicationContext.CustomerId);
                            request.CustomerIdList.AddRange(sisterCompanyIds);
                        }
                        request.InvoiceTo = (int)QuotationPaidBy.customer;
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        request.SupplierId = request.SupplierId > 0 ? request.SupplierId.Value : _ApplicationContext.SupplierId;
                        request.InvoiceTo = (int)QuotationPaidBy.supplier;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        request.FactoryIdList = request.FactoryIdList!= null && request.FactoryIdList.Any() ? request.FactoryIdList : new List<int>().Append(_ApplicationContext.FactoryId);
                        request.InvoiceTo = (int)QuotationPaidBy.factory;
                        break;
                    }
            }
            if (request.CustomerId > 0)
                request.CustomerIdList.Add(request.CustomerId.GetValueOrDefault());
            
            return request;
        }

        private async Task<SaleInvoiceSummarySearchResult> GetAllInvoiceSummary(SaleInvoiceSummaryRequest request)
        {
            var response = new SaleInvoiceSummarySearchResult();
            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            var invoiceData = _invoiceRepository.GetInvoiceDetailsByServiceType(request.ServiceId);

            //filter based on service date from and to / invoice date
            if (Enum.TryParse(request.DateTypeId.ToString(), out SearchType _datesearchtype) && request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
            {
                switch (_datesearchtype)
                {
                    case SearchType.InvoiceDate:
                        {
                            invoiceData = invoiceData.Where(x => (x.InvoiceDate.HasValue && x.InvoiceDate.Value.Date >= request.FromDate.ToDateTime().Date) &&
                                        (x.InvoiceDate.HasValue && x.InvoiceDate.Value.Date <= request.ToDate.ToDateTime().Date));
                            break;
                        }
                    case SearchType.ServiceDate:
                        {
                            if (request.ServiceId == (int)Service.InspectionId)
                            {
                                invoiceData = invoiceData.Where(x => !((x.Inspection.ServiceDateFrom.Date > request.ToDate.ToDateTime().Date) || (x.Inspection.ServiceDateTo.Date < request.FromDate.ToDateTime().Date)));
                            }
                            else if (request.ServiceId == (int)Service.AuditId)
                            {
                                invoiceData = invoiceData.Where(x => !((x.Audit.ServiceDateFrom.Date > request.ToDate.ToDateTime().Date) || (x.Audit.ServiceDateTo.Date < request.FromDate.ToDateTime().Date)));
                            }
                            break;
                        }
                }
            }

            //filter by invoice To
            if (request.InvoiceTo > 0)
            {
                if (request.InvoiceTo == (int)QuotationPaidBy.customer)
                {
                    invoiceData = invoiceData.Where(x => x.InvoiceTo == (int)QuotationPaidBy.customer);
                }
                else if (request.InvoiceTo == (int)QuotationPaidBy.supplier)
                {
                    invoiceData = invoiceData.Where(x => x.InvoiceTo == (int)QuotationPaidBy.supplier);
                }
                else
                {
                    invoiceData = invoiceData.Where(x => x.InvoiceTo == (int)QuotationPaidBy.factory);
                }
            }

            //filter by customerId
            if (request.CustomerIdList.Any())
            {
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    invoiceData = invoiceData.Where(x => request.CustomerIdList.Contains(x.Inspection.CustomerId));
                }
                else
                {
                    invoiceData = invoiceData.Where(x => request.CustomerIdList.Contains(x.Audit.CustomerId));
                }
            }

            //filter by supplierId
            if (request.SupplierId > 0)
            {
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    invoiceData = invoiceData.Where(x => x.Inspection.SupplierId == request.SupplierId);
                }
                else
                {
                    invoiceData = invoiceData.Where(x => x.Audit.SupplierId == request.SupplierId);
                }
            }

            //filter by factoryId
            if (request.FactoryIdList != null && request.FactoryIdList.Any())
            {
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    invoiceData = invoiceData.Where(x => request.FactoryIdList.Contains(x.Inspection.FactoryId.GetValueOrDefault()));
                }
                else
                {
                    invoiceData = invoiceData.Where(x => request.FactoryIdList.Contains(x.Audit.FactoryId));
                }
            }

            //apply office list filter
            if (request.OfficeIdList != null && request.OfficeIdList.Any())
            {
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    invoiceData = invoiceData.Where(x => x.Inspection.OfficeId != null && request.OfficeIdList.Contains(x.Inspection.OfficeId.GetValueOrDefault()));
                }
                else
                {
                    invoiceData = invoiceData.Where(x => x.Audit.OfficeId != null && request.OfficeIdList.Contains(x.Audit.OfficeId.GetValueOrDefault()));
                }
            }

            //filter by Booking Id
            if (request.SearchTypeId == (int)SearchType.BookingNo && !string.IsNullOrEmpty(request.SearchTypeText?.Trim()) && int.TryParse(request.SearchTypeText?.Trim(), out int bookid))
            {
                invoiceData = invoiceData.Where(x => x.InspectionId == bookid);
            }

            //filter by invoice No
            if (request.SearchTypeId == (int)SearchType.InvoiceNo && !string.IsNullOrEmpty(request.SearchTypeText?.Trim()))
            {
                invoiceData = invoiceData.Where(x => x.InvoiceNo == request.SearchTypeText.Trim());
            }

            // check invoice data access and display the data based onn that filter
            if (_ApplicationContext.StaffId > 0)
            {
                var invoiceAccessData = await _invoiceDataAccessRepository.GetStaffInvoiceDataAccess(_ApplicationContext.StaffId);

                var customerIdAccess = invoiceAccessData.InvDaCustomers.Where(x => x.Active).Select(x => x.CustomerId).Distinct().ToList();

                var officeIdAccess = invoiceAccessData.InvDaOffices.Where(x => x.Active).Select(x => x.OfficeId).Distinct().ToList();

                var invoiceTypeAccess = invoiceAccessData.InvDaInvoiceTypes.Where(x => x.Active).Select(x => x.InvoiceTypeId).Distinct().ToList();

                if (customerIdAccess.Any())
                {
                    if (request.ServiceId == (int)Service.InspectionId)
                    {
                        invoiceData = invoiceData.Where(x => customerIdAccess.Contains(x.Inspection.CustomerId));
                    }
                    else
                    {
                        invoiceData = invoiceData.Where(x => customerIdAccess.Contains(x.Audit.CustomerId));
                    }
                }

                if (officeIdAccess.Any())
                {
                    if (request.ServiceId == (int)Service.InspectionId)
                    {
                        invoiceData = invoiceData.Where(x => officeIdAccess.Contains(x.Inspection.OfficeId.GetValueOrDefault()));
                    }
                    else
                    {
                        invoiceData = invoiceData.Where(x => officeIdAccess.Contains(x.Audit.OfficeId.GetValueOrDefault()));
                    }
                }

                if (invoiceTypeAccess.Any())
                {
                    invoiceData = invoiceData.Where(x => invoiceTypeAccess.Contains(x.InvoiceType.GetValueOrDefault()));
                }
            }

            if (request.PaymentStatusIdList != null && request.PaymentStatusIdList.Any())
            {
                invoiceData = invoiceData.Where(x => request.PaymentStatusIdList.Contains(x.InvoicePaymentStatus.GetValueOrDefault()));
            }

            invoiceData = invoiceData.OrderByDescending(x => x.InvoiceNo);

            response.PaymentStatusCountList = await invoiceData.Where(x => x.InvoicePaymentStatus.HasValue)
                .GroupBy(p => new { PaymentStatusId = p.InvoicePaymentStatus.GetValueOrDefault(), PaymentStatusName = p.InvoicePaymentStatusNavigation.Name }, p => p, (key, _data) =>
                           new PaymentStatus
                           {
                               Id = key.PaymentStatusId,
                               StatusName = key.PaymentStatusName,
                               TotalCount = _data.Select(x=>x.InvoiceNo).Distinct().Count(),
                               StatusColor = InvoicePaymentStatusColor.GetValueOrDefault(key.PaymentStatusId, DefaultPaymentStatusColor)
                           }).ToListAsync();

            response.TotalCount = await invoiceData.Select(x => x.InvoiceNo).Distinct().CountAsync();

            IQueryable<string> invoiceNoList = null;
            if (!request.IsExport)
                invoiceNoList = invoiceData.Select(x => x.InvoiceNo).Distinct().Skip(skip).Take(request.pageSize.GetValueOrDefault());
            else
                invoiceNoList = invoiceData.Select(x => x.InvoiceNo).Distinct();

            IQueryable<InvoiceSummaryItem> invoiceResultData = null;

            var dataList = new List<InvoiceSummaryItem>();
            if (request.ServiceId == (int)Service.InspectionId)
            {
                //fetch the booking and invoice details
                invoiceResultData = _invoiceRepository.GetQueryableInvoiceDetailsByInvoiceNo(invoiceNoList);

                dataList = await invoiceResultData.AsNoTracking().ToListAsync();
            }
            else if (request.ServiceId == (int)Service.AuditId)
            {
                //fetch the Audit and invoice details
                invoiceResultData = _invoiceRepository.GetQueryableAuditInvoiceDetailsByInvoiceNo(invoiceNoList);

                dataList = await invoiceResultData.AsNoTracking().ToListAsync();
            }

            var distinctInvoiceIds = invoiceData.Select(x => x.Id).Distinct();
            var invoiceExtraFees = await _invoiceRepository.GetQueryableInvoiceExtraFeeItem(distinctInvoiceIds);
            response.InvoiceDataList = dataList;
            response.InvoiceExtraFeeList = invoiceExtraFees.ToList();

            return response;
        }
    }
}
