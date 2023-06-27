using BI.Maps;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Customer;
using DTO.Eaqf;
using DTO.Inspection;
using DTO.Invoice;
using DTO.InvoicePreview;
using DTO.MasterConfig;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static BI.TenantProvider;

namespace BI
{
    public class ManualInvoiceManager : ApiCommonData, IManualInvoiceManager
    {
        private readonly IInvoiceManager _invoiceManager = null;
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly IInspectionBookingRepository _inspectionBookingRepo = null;
        private readonly IAuditRepository _auditBookingRepo = null;
        private readonly IManualInvoiceRepository _repo = null;
        private readonly IInvoiceBankManager _invoiceBankManager = null;
        private readonly ManualInvoiceMap _map = null;
        private readonly IAPIUserContext _apiUserContext = null;
        private readonly ICustomerManager _customerManager;
        private readonly ITenantProvider _filterService = null;
        private readonly IReferenceRepository _referenceRepository;
        private readonly ISupplierRepository _supplierRepository = null;
        private readonly IUserConfigRepository _userConfigRepository;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly IClaimRepository _claimRepository;
        private readonly IExtraFeesRepository _extraFeesRepository;
        private readonly IUserRepository _userRepository;
        private readonly InvoiceMap _invoiceMap;
        private readonly IReferenceManager _referenceManager;

        public ManualInvoiceManager(IInvoiceManager invoiceManager, IManualInvoiceRepository repo, IUserConfigRepository userConfigRepository, IUserRepository userRepository,
            IAPIUserContext apiUserContext, IReferenceRepository referenceRepository, IInvoiceBankManager invoiceBankManager, IClaimRepository claimRepository,
            ICustomerManager customerManager, ITenantProvider filterService, IInvoiceRepository invoiceRepository, IExtraFeesRepository extraFeesRepository,
            IInspectionBookingRepository inspectionBookingRepo, ISupplierRepository supplierRepository, ICustomerRepository customerRepository,
            IReferenceManager referenceManager, IAuditRepository auditBookingRepo)
        {
            _invoiceManager = invoiceManager;
            _repo = repo;
            _invoiceBankManager = invoiceBankManager;
            _map = new ManualInvoiceMap();
            _apiUserContext = apiUserContext;
            _customerManager = customerManager;
            _filterService = filterService;
            _referenceRepository = referenceRepository;
            _invoiceRepository = invoiceRepository;
            _inspectionBookingRepo = inspectionBookingRepo;
            _supplierRepository = supplierRepository;
            _userConfigRepository = userConfigRepository;
            _customerRepository = customerRepository;
            _claimRepository = claimRepository;
            _extraFeesRepository = extraFeesRepository;
            _userRepository = userRepository;
            _invoiceMap = new InvoiceMap();
            _referenceManager = referenceManager;
            _auditBookingRepo = auditBookingRepo;
        }
        public async Task<SaveManualInvoiceResponse> SaveManualInvoice(SaveManualInvoice request)
        {
            var response = new SaveManualInvoiceResponse();
            var isInvoiceNumberExist = await this.CheckInvoiceNumberExist(request.InvoiceNo);
            if (isInvoiceNumberExist)
            {
                response.Result = ManualInvoiceResult.InvoiceNoAlreadyExist;
                return response;
            }
            var userId = _apiUserContext.UserId > 0 ? _apiUserContext.UserId : request.UserId;
            var entityId = _filterService.GetCompanyId();
            var manualInvoice = _map.MapManualInvoiceEntity(request, userId, entityId);
            double subTotal = 0;

            foreach (var invoiceItem in request.InvoiceItems)
            {
                var invoiceItemTranDetails = _map.MapManualInvoiceItemEntity(invoiceItem, userId);
                _repo.AddEntity(invoiceItemTranDetails);
                manualInvoice.InvManTranDetails.Add(invoiceItemTranDetails);

                subTotal = subTotal + invoiceItemTranDetails.Subtotal.Value;
            }

            await AddManualInvoiceTaxes(manualInvoice, subTotal);

            _repo.AddEntity(manualInvoice);
            await _repo.Save();
            response.ManaualInvoiceId = manualInvoice.Id;
            response.Result = ManualInvoiceResult.Success;
            return response;
        }

        public async Task<SaveManualInvoiceResponse> UpdateManualInvoice(SaveManualInvoice request)
        {
            var response = new SaveManualInvoiceResponse();
            var manualInvoice = await _repo.GetManualInvoice(request.Id);
            if (manualInvoice == null)
            {
                response.Result = ManualInvoiceResult.NotFound;
                return response;
            }

            if (manualInvoice.InvoiceNo != request.InvoiceNo)
            {
                var isInvoiceNumberExist = await this.CheckInvoiceNumberExist(request.InvoiceNo);
                if (isInvoiceNumberExist)
                {
                    response.Result = ManualInvoiceResult.InvoiceNoAlreadyExist;
                    return response;
                }
            }

            var userId = _apiUserContext.UserId;
            var dbBankId = manualInvoice.BankId;
            _map.MapManualInvoiceEntity(manualInvoice, request, userId);


            var invoiceItems = await _repo.GetManualInvoiceItemsByManualInvoiceIds(new List<int>() { manualInvoice.Id });
            var invoiceItemIds = request.InvoiceItems.Where(x => x.Id > 0).Select(x => x.Id);

            //Deleting Invoice items
            var deleteInvoiceItems = invoiceItems.Where(x => !invoiceItemIds.Contains(x.Id));
            if (deleteInvoiceItems.Any())
            {
                deleteInvoiceItems.ToList().ForEach(x =>
                {
                    x.Active = false;
                    x.DeletedBy = userId;
                    x.DeletedOn = DateTime.Now;
                });
                _repo.EditEntities<InvManTranDetail>(deleteInvoiceItems);
            }

            double? subTotal = 0;
            //Add New Items 
            var newItems = request.InvoiceItems.Where(x => x.Id <= 0);
            foreach (var invoiceItem in newItems)
            {
                var invoiceItemTranDetails = _map.MapManualInvoiceItemEntity(invoiceItem, userId);
                _repo.AddEntity(invoiceItemTranDetails);
                manualInvoice.InvManTranDetails.Add(invoiceItemTranDetails);
                subTotal = subTotal + invoiceItemTranDetails.Subtotal.Value;
            }

            var updateItems = new List<InvManTranDetail>();
            foreach (var item in request.InvoiceItems.Where(x => x.Id > 0))
            {
                var invoiceItem = invoiceItems.FirstOrDefault(x => x.Id == item.Id);
                if (invoiceItem == null)
                {
                    response.Result = ManualInvoiceResult.InvoiceItemNotFound;
                    return response;
                }
                _map.MapManualInvoiceItemEntity(invoiceItem, item, userId);
                updateItems.Add(invoiceItem);
                subTotal = subTotal + invoiceItem.Subtotal;
            }

            if (updateItems.Any())
                _repo.EditEntities<InvManTranDetail>(updateItems);

            //if requested bankid and db bank id different 
            if (dbBankId != request.BankId)
            {
                var manualInvoiceTransTaxes = await _repo.GetManualInvoiceTransTaxes(request.Id);
                if (manualInvoiceTransTaxes.Any())
                    _repo.RemoveEntities(manualInvoiceTransTaxes);
                await AddManualInvoiceTaxes(manualInvoice, subTotal);
            }

            _repo.EditEntity(manualInvoice);
            await _repo.Save();
            response.ManaualInvoiceId = manualInvoice.Id;
            response.Result = ManualInvoiceResult.Success;
            return response;
        }
        /// <summary>
        /// for adding the manual invoice taxes
        /// </summary>
        /// <param name="manualInvoice"></param>
        private async Task AddManualInvoiceTaxes(InvManTransaction manualInvoice, double? subTotal)
        {
            var invoiceBankGetResponse = await _invoiceBankManager.GetTaxDetails(manualInvoice.BankId, manualInvoice.ToDate.Value);
            if (invoiceBankGetResponse != null && invoiceBankGetResponse.BankTaxDetails != null)
            {
                var taxDetails = invoiceBankGetResponse.BankTaxDetails;
                if (taxDetails.Any())
                {
                    foreach (var item in taxDetails)
                    {
                        var invManTranTax = new InvManTranTax() { TaxId = item.Id, CreatedOn = DateTime.Now, CreatedBy = _apiUserContext.UserId };
                        _repo.AddEntity(invManTranTax);
                        manualInvoice.InvManTranTaxes.Add(invManTranTax);
                    }
                    var totalTax = taxDetails.Sum(x => Decimal.ToDouble(x.TaxValue));
                    manualInvoice.Tax = totalTax;
                    manualInvoice.TaxAmount = Math.Round((subTotal * manualInvoice.Tax).GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
                }
                else
                {
                    manualInvoice.Tax = 0;
                    manualInvoice.TaxAmount = 0;
                }
                manualInvoice.TotalAmount = Math.Round(subTotal.GetValueOrDefault() + manualInvoice.TaxAmount.GetValueOrDefault(), InvoiceRoundUpValue, MidpointRounding.AwayFromZero);
            }

        }
        public async Task<ManualInvoiceSummaryResponse> GetManualInvoiceSummary(ManualInvoiceSummaryRequest request)
        {
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;
            var data = _repo.GetManualInvoices();

            if (request != null)
            {
                if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
                {
                    data = data.Where(x => x.InvoiceDate >= request.FromDate.ToDateTime() && x.InvoiceDate <= request.ToDate.ToDateTime());
                }
                if (request.InvoiceNo != null && !string.IsNullOrWhiteSpace(request.InvoiceNo))
                {
                    data = data.Where(x => x.InvoiceNo.Contains(request.InvoiceNo));
                }
                if (request.CustomerId > 0)
                    data = data.Where(x => x.CustomerId == request.CustomerId);

                if (request.InvoiceTo > 0)
                    data = data.Where(x => x.InvoiceTo == request.InvoiceTo);

                if (request.InvoiceStatusId != null && request.InvoiceStatusId.Any())
                {
                    data = data.Where(x => request.InvoiceStatusId.Contains(x.StatusId));
                }
                if (request.InvoiceStatusId != null && request.InvoiceStatusId.Any())
                {
                    data = data.Where(x => request.InvoiceStatusId.Contains(x.StatusId));
                }
                if (request.IsEAQF.GetValueOrDefault())
                {
                    data = data.Where(x => x.IsEAQF == request.IsEAQF);
                }
            }
            var totalCount = await data.AsNoTracking().CountAsync();
            if (totalCount == 0)
            {
                return new ManualInvoiceSummaryResponse()
                {
                    Result = ManualInvoiceResult.NotFound
                };
            }
            var res = await data.AsNoTracking().Skip(skip).Take(take).ToListAsync();


            var entityId = _filterService.GetCompanyId();
            IEnumerable<EaqfInvoiceDetails> invoiceFiles = null;
            if (entityId == (int)Company.aqf)
            {
                invoiceFiles = await _inspectionBookingRepo.GetEaqfBookingInvoiceFileDetails(res.Select(y => y.InvoiceNo).ToList());
            }

            var invoiceItems = await _repo.GetManualInvoiceItemsByManualInvoiceIds(res.Select(x => x.Id));
            var result = res.Select(x => _map.MapManualInvoiceSummary(x, invoiceItems, invoiceFiles));

            var invoiceStatusList = res
                          .GroupBy(p => new { p.StatusId, p.StatusName }, p => p, (key, _data) =>
                            new InspectionStatus
                            {
                                Id = key.StatusId,
                                StatusName = key.StatusName,
                                TotalCount = _data.Count(),
                                StatusColor = InvoiceStatusColor.GetValueOrDefault(key.StatusId, "")
                            }).ToList();
            return new ManualInvoiceSummaryResponse
            {
                Data = result.ToList(),
                StatusCountList = invoiceStatusList,
                Result = ManualInvoiceResult.Success,
                PageCount = (totalCount / request.PageSize.Value) + (totalCount % request.PageSize.Value > 0 ? 1 : 0),
                PageSize = request.PageSize.GetValueOrDefault(),
                TotalCount = totalCount,
                Index = request.Index.GetValueOrDefault()
            };
        }

        public async Task<DeleteManualInvoiceResponse> DeleteManualInvoice(int id)
        {
            var response = new DeleteManualInvoiceResponse();

            var manualInvoice = await _repo.GetManualInvoice(id);
            if (manualInvoice == null)
            {
                response.Result = ManualInvoiceResult.NotFound;
                return response;
            }
            var items = await _repo.GetManualInvoiceItemsByManualInvoiceIds(new List<int> { manualInvoice.Id });
            items.ToList().ForEach(x =>
            {
                x.Active = false;
                x.DeletedBy = _apiUserContext.UserId;
                x.DeletedOn = DateTime.Now;
            });

            _repo.EditEntities<InvManTranDetail>(items);

            manualInvoice.Status = (int)InvoiceStatus.Cancelled;
            manualInvoice.DeletedBy = _apiUserContext.UserId;
            manualInvoice.DeletedOn = DateTime.Now;

            _repo.EditEntity<InvManTransaction>(manualInvoice);
            await _repo.Save();

            response.Result = ManualInvoiceResult.Success;
            return response;


        }

        public async Task<GetManualInvoiceResponse> GetManualInvoice(int id)
        {
            var response = new GetManualInvoiceResponse();

            var manualInvoice = await _repo.GetManualInvoice(id);
            if (manualInvoice == null)
            {
                response.Result = ManualInvoiceResult.NotFound;
                return response;
            }
            var items = await _repo.GetManualInvoiceItemsByManualInvoiceIds(new List<int> { manualInvoice.Id });
            response.ManualInvoice = _map.MapGetManualInvoice(manualInvoice, items);
            response.Result = ManualInvoiceResult.Success;
            return response;


        }

        public async Task<int> GetManualInvoiceIdbyInvoiceNumber(string invoiceNo)
        {
            var response = new GetManualInvoiceResponse();

            var manualInvoice = await _repo.GetManualInvoicebyInvoiceNo(invoiceNo);
            if (manualInvoice == null)
            {
                response.Result = ManualInvoiceResult.NotFound;
                return 0;
            }
            response.Result = ManualInvoiceResult.Success;
            return manualInvoice.Id;
        }

        public async Task<bool> CheckInvoiceNumberExist(string invoiceNumber)
        {
            return await _repo.IsInvoiceNumberExists(invoiceNumber);
        }

        public async Task<ManualInvoiceSummaryExportResponse> ExportManualInvoiceSummary(ManualInvoiceSummaryRequest request)
        {
            var response = new ManualInvoiceSummaryExportResponse();
            if (request == null)
            {
                response.Result = ManualInvoiceResult.NotFound;
                return response;
            }
            var data = _repo.GetManualInvoiceExports();

            if (request != null)
            {
                if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
                {
                    data = data.Where(x => x.InvoiceDate >= request.FromDate.ToDateTime() && x.InvoiceDate <= request.ToDate.ToDateTime());
                }
                if (request.InvoiceNo != null && string.IsNullOrWhiteSpace(request.InvoiceNo))
                {
                    data = data.Where(x => x.InvoiceNo.Contains(request.InvoiceNo));
                }
                if (request.CustomerId > 0)
                    data = data.Where(x => x.CustomerId == request.CustomerId);

                if (request.InvoiceTo > 0)
                    data = data.Where(x => x.InvoiceTo == request.InvoiceTo);
            }
            var result = await data.AsNoTracking().OrderBy(x => x.InvoiceNo).Select(x => _map.MapManualInvoiceExportSummary(x)).ToListAsync();
            response.ManualInvoices = result;
            response.RequestFilters = await SetExportFilter(request);
            response.Result = ManualInvoiceResult.Success;
            return response;
        }
        private async Task<ManualInvoiceExportSummaryRequestFilter> SetExportFilter(ManualInvoiceSummaryRequest request)
        {
            var filter = new ManualInvoiceExportSummaryRequestFilter();
            if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
            {
                filter.FromDate = request.FromDate.ToDateTime().ToString(StandardDateFormat);
                filter.ToDate = request.ToDate.ToDateTime().ToString(StandardDateFormat);
            }
            if (request.InvoiceNo != null && !string.IsNullOrWhiteSpace(request.InvoiceNo))
            {
                filter.InvoiceNo = request.InvoiceNo;
            }
            if (request.CustomerId > 0)
            {
                var customers = await _customerManager.GetCustomerByCustomerId(request.CustomerId);
                filter.CustomerName = customers.DataSourceList != null && customers.DataSourceList.Any() ? string.Join(", ", customers.DataSourceList.Select(x => x.Name)) : "";
            }

            if (request.InvoiceTo > 0)
            {
                var invoiceTos = await _referenceRepository.GetBillingTosByIds(new List<int> { request.InvoiceTo });
                filter.InvoiceTo = invoiceTos != null && invoiceTos.Any() ? string.Join(", ", invoiceTos) : "";
            }
            return filter;
        }

        public async Task<object> SaveEAQFManualInvoice(SaveQuotationEaqfRequest request, bool isNewCreate = false)
        {
            // Add validation 
            if (request == null)
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Request can not be empty" });
            }

            if (request.Service != (int)(Service.InspectionId) && request.Service != (int)(Service.AuditId))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Service is invalid" });
            }

            if (string.IsNullOrEmpty(request.CurrencyCode))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Currency Code is not valid" });
            }

            if (request.BookingId <= 0)
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Booking Id is not valid." });

            if (request.OrderDetails == null || !request.OrderDetails.Any())
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Order details is Required" });
            }

            if (request.OrderDetails.Any(x => string.IsNullOrWhiteSpace(x.Description)))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Description is required in order details" });
            }

            if (request.OrderDetails.Any(x => x.OrderType == "orderfee" && x.Manday <= 0))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Manday is required in order details" });
            }

            if (request.OrderDetails.Any(x => x.OrderType == "orderfee" && x.Amount <= 0))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Order fee is required in order details" });
            }

            if (request.OrderDetails.Any(x => x.OrderType == "extrafee" && x.Amount <= 0))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Extra fee is required in order details" });
            }

            if (request.OrderDetails.Any(x => x.OrderType == "discount" && x.Amount <= 0))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Discount is required in order details" });
            }

            double totalFees = 0;
            double orderFees = 0;
            double otherFees = 0;
            double discount = 0;

            if (request.OrderDetails.Any(x => x.OrderType == "orderfee"))
            {
                orderFees = request.OrderDetails.Where(x => x.OrderType == "orderfee").Select(x => x.Manday * x.Amount).Sum();
            }
            if (request.OrderDetails.Any(x => x.OrderType == "otherfee"))
            {
                otherFees = request.OrderDetails.Where(x => x.OrderType == "otherfee").Select(x => x.Amount).Sum();
            }

            if (request.OrderDetails.Any(x => x.OrderType == "discount"))
            {
                discount = request.OrderDetails.Where(x => x.OrderType == "discount").Select(x => x.Amount).Sum();
            }

            totalFees = orderFees + otherFees;

            if (totalFees < discount)
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Total discount is not greater than the total fees in order details" });
            }

            if (request.PaymentMode <= 0)
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Payment mode is not valid" });
            }

            if (string.IsNullOrWhiteSpace(request.PaymentRef))
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Payment reference is not valid" });
            }


            if (request.Service == (int)(Service.InspectionId))
            {
                var invoice = await _repo.GetManualInvoiceByBookingId(request.BookingId);
                if (invoice != null && !isNewCreate)
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = "Success",
                        statusCode = HttpStatusCode.OK,

                        data = new EAQFInvoiceResponse()
                        {
                            InvoiceNo = invoice.InvoiceNo,
                            InvoiceId = invoice.Id
                        }
                    };
                }

                var bookingData = await _inspectionBookingRepo.GetInspectionBookingDetails(request.BookingId); // insp details 
                if (bookingData == null)
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Inspection data not found" });

                var currencyData = await _referenceManager.GetCurrencyData(request.CurrencyCode);

                if (currencyData == null)
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Currency data not found" });
                }

                var customerContact = await _userRepository.GetCustomerContactByUserId(request.UserId);
                if (customerContact == null)
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "User id is not mapped with this Booking customer" });
                }

                var masterConfigurations = await _userConfigRepository.GetMasterConfigurationByTypeIds(new List<int>() { (int)EntityConfigMaster.DefaultEAQFBank, (int)EntityConfigMaster.DefaultEAQFBillingEntity, (int)EntityConfigMaster.DefaultEAQFInvoiceOffice });
                if (!int.TryParse(masterConfigurations?.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.DefaultEAQFBank)?.Value, out int bankId))
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Default bank is not set for eaqf" });
                }

                if (!int.TryParse(masterConfigurations?.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.DefaultEAQFBillingEntity)?.Value, out int billingEntity))
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Default billing entity is not set for eaqf" });
                }

                if (!int.TryParse(masterConfigurations?.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.DefaultEAQFInvoiceOffice)?.Value, out int invoiceOfficeId))
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Default invoice office is not set for eaqf" });
                }

                var factoryAddress = await _supplierRepository.GetSupplierHeadOfficeAddress(bookingData.FactoryId.GetValueOrDefault()); // FactoryCountry                                                                                                                
                if (factoryAddress == null)
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Factory head office address not available" });

                var bookingIds = new[] { request.BookingId };

                var invoiceNumber = await GetInvoiceNumber(request.BookingId);
                if (string.IsNullOrWhiteSpace(invoiceNumber))
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invoice number is not generated" });

                var customerAddresses = await _customerRepository.GetZohoCustomerAddressById((bookingData.CustomerId)); // BillAddress 

                var serviceTypes = await _inspectionBookingRepo.GetServiceType(bookingIds); // serviceType

                var manualInvoice = _map.MapManualInvoiceDetailsForEAQF(bookingData, request, customerContact, serviceTypes, factoryAddress, bankId, currencyData.Id, billingEntity, invoiceOfficeId, customerAddresses, invoiceNumber);
                var response = await this.SaveManualInvoice(manualInvoice);
                if (response.Result == ManualInvoiceResult.Success)
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = "Success",
                        statusCode = HttpStatusCode.OK,

                        data = new EAQFInvoiceResponse()
                        {
                            InvoiceNo = invoiceNumber,
                            InvoiceId = response.ManaualInvoiceId
                        }
                    };
                }
                else
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Failed" });
                }
            }
            else if (request.Service == (int)(Service.AuditId))
            {

                var invoice = await _repo.GetAuditManualInvoiceByBookingId(request.BookingId);

                if (invoice != null && !isNewCreate)
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = "Success",
                        statusCode = HttpStatusCode.OK,

                        data = new EAQFInvoiceResponse()
                        {
                            InvoiceNo = invoice.InvoiceNo,
                            InvoiceId = invoice.Id
                        }
                    };
                }

                var bookingData = await _auditBookingRepo.GetAuditBookingDetails(request.BookingId); // insp details 
                if (bookingData == null)
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Audit data not found" });

                var currencyData = await _referenceManager.GetCurrencyData(request.CurrencyCode);

                if (currencyData == null)
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Currency data not found" });
                }

                var customerContact = await _userRepository.GetCustomerContactByUserId(request.UserId);
                if (customerContact == null)
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "User id is not mapped with this Booking customer" });
                }

                var masterConfigurations = await _userConfigRepository.GetMasterConfigurationByTypeIds(new List<int>() { (int)EntityConfigMaster.DefaultEAQFBank, (int)EntityConfigMaster.DefaultEAQFBillingEntity, (int)EntityConfigMaster.DefaultEAQFInvoiceOffice });
                if (!int.TryParse(masterConfigurations?.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.DefaultEAQFBank)?.Value, out int bankId))
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Default bank is not set for eaqf" });
                }

                if (!int.TryParse(masterConfigurations?.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.DefaultEAQFBillingEntity)?.Value, out int billingEntity))
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Default billing entity is not set for eaqf" });
                }

                if (!int.TryParse(masterConfigurations?.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.DefaultEAQFInvoiceOffice)?.Value, out int invoiceOfficeId))
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Default invoice office is not set for eaqf" });
                }

                var factoryAddress = await _supplierRepository.GetSupplierHeadOfficeAddress(bookingData.FactoryId.GetValueOrDefault()); // FactoryCountry                                                                                                                
                if (factoryAddress == null)
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Factory head office address not available" });

                var bookingIds = new[] { request.BookingId };

                var invoiceNumber = await GetAuditInvoiceNumber(request.BookingId);
                if (string.IsNullOrWhiteSpace(invoiceNumber))
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invoice number is not generated" });

                var customerAddresses = await _customerRepository.GetZohoCustomerAddressById((bookingData.CustomerId)); // BillAddress 

                var serviceTypes = await _auditBookingRepo.GetServiceType(bookingIds); // serviceType

                var manualInvoice = _map.MapManualInvoiceDetailsForEAQF(bookingData, request, customerContact, serviceTypes, factoryAddress, bankId, currencyData.Id, billingEntity, invoiceOfficeId, customerAddresses, invoiceNumber);
              
                var response = await this.SaveManualInvoice(manualInvoice);

                if (response.Result == ManualInvoiceResult.Success)
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = "Success",
                        statusCode = HttpStatusCode.OK,

                        data = new EAQFInvoiceResponse()
                        {
                            InvoiceNo = invoiceNumber,
                            InvoiceId = response.ManaualInvoiceId
                        }
                    };
                }
                else
                {
                    return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Failed" });
                }
            }
            else
            {
                return BuildCommonEaqfErrorResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Failed" });
            }
        }

        public EaqfErrorResponse BuildCommonEaqfErrorResponse(HttpStatusCode statusCode, string message, List<string> errors)
        {
            return new EaqfErrorResponse()
            {
                errors = errors,
                statusCode = statusCode,
                message = message
            };
        }

        private async Task<string> GetInvoiceNumber(int bookingId)
        {
            var totalInvoice = await _repo.GetManualInvoiceCountByBookingId(bookingId);
            string invoiceNumber = string.Format(DefaultEAQFInvoiceNumber, bookingId, totalInvoice + 1);
            if (await CheckInvoiceNumberExists(invoiceNumber))
            {
                await GetInvoiceNumber(bookingId);
            }

            return invoiceNumber;
        }

        private async Task<string> GetAuditInvoiceNumber(int bookingId)
        {
            var totalInvoice = await _repo.GetAuditManualInvoiceCountByBookingId(bookingId);
            string invoiceNumber = string.Format(DefaultEAQFInvoiceNumber, bookingId, totalInvoice + 1);
            if (await CheckInvoiceNumberExists(invoiceNumber))
            {
                await GetAuditInvoiceNumber(bookingId);
            }

            return invoiceNumber;
        }

        /// <summary>
        /// check the invoice number invoice, manual invoice, credit note and extra free
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        private async Task<bool> CheckInvoiceNumberExists(string invoiceNumber)
        {
            var invoiceExists = await _invoiceRepository.CheckInvoiceNumberExist(invoiceNumber);
            if (!invoiceExists)
                invoiceExists = await this.CheckInvoiceNumberExist(invoiceNumber);
            if (!invoiceExists)
                invoiceExists = await _claimRepository.IsCreditNoExist(invoiceNumber);
            if (!invoiceExists)
                invoiceExists = await _extraFeesRepository.IsExistsInvoiceNumber(invoiceNumber);

            return invoiceExists;
        }


        public async Task<EAQFManualInvoiceFastReport> GetEaqfManualInvoice(int invoiceId)
        {
            var manualInvoice = await _repo.GetEaqfManualInvoiceDataById(invoiceId);
            if (manualInvoice == null)
                return null;

            foreach (var item in manualInvoice)
            {
                item.InvoiceDate = item.InvoiceDateTime?.ToString(StandardDateFormat5);
            }

            var invoiceItems = await _repo.GetManualInvoiceItemsByManualInvoiceIds(new List<int>() { invoiceId });

            return new EAQFManualInvoiceFastReport()
            {
                Invoice = manualInvoice,
                InvoiceItems = invoiceItems.OrderByDescending(x => x.ServiceFee).ToList()
            };
        }


        public async Task<SaveInvoicePdfResponse> SaveInvoicePdfUrl(int manualInvoiceId, string filePath, string uniqueId, int createdBy)
        {
            //fetch the invoice from invocie no
            var invoice = await _repo.GetManualInvoice(manualInvoiceId);
            if (invoice == null)
                return new SaveInvoicePdfResponse() { Result = SaveInvoicePdfResult.InvoiceNumberNotFound };

            //get the invoice transaction file list
            var invoiceFiles = await _invoiceRepository.GetInvoiceTransactionFiles(invoice.Id, (int)InvRefFileTypeEnum.ManualInvoice);
            //deactive the existing invoice preview file
            if (invoiceFiles != null && invoiceFiles.Any())
            {
                invoiceFiles.ForEach(x =>
                {
                    x.Active = false;
                    x.DeletedBy = createdBy;
                    x.DeletedOn = DateTime.Now;
                });
                _invoiceRepository.EditEntities(invoiceFiles);
            }

            //map the request to entity
            var invTranFile = _invoiceMap.MapInvoiceTranFile(new DTO.InvoicePreview.SaveInvoicePdfUrl()
            {
                CreatedBy = createdBy,
                FilePath = filePath,
                FileType = (int)InvRefFileTypeEnum.ManualInvoice,
                InvoiceNo = invoice.InvoiceNo,
                UniqueId = uniqueId
            }, manualInvoiceId);
            //add invoice transaction file
            _invoiceRepository.AddEntity(invTranFile);

            await _invoiceRepository.Save();
            return new SaveInvoicePdfResponse() { Result = SaveInvoicePdfResult.Success };
        }
    }
}
