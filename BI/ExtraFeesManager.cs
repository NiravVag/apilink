using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.ExtraFees;
using DTO.Invoice;
using DTO.Quotation;
using DTO.RepoRequest.Enum;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class ExtraFeesManager : ApiCommonData, IExtraFeesManager
    {
        private readonly IExtraFeesRepository _extraFeesRepository = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly IInvoiceManager _invoiceManager = null;
        private ITenantProvider _filterService = null;
        private IInvoiceBankManager _invoiceBankManager;
        private readonly ExtraFeeMap _extrafee = null;

        public ExtraFeesManager(IExtraFeesRepository extraFeesRepository, IAPIUserContext applicationContext, IInvoiceBankManager invoiceBankManager,
            IInspectionBookingManager inspManager, IInvoiceManager invoiceManager, ITenantProvider filterService)
        {
            _extraFeesRepository = extraFeesRepository;
            _applicationContext = applicationContext;
            _inspManager = inspManager;
            _invoiceManager = invoiceManager;
            _extrafee = new ExtraFeeMap();
            _filterService = filterService;
            _invoiceBankManager = invoiceBankManager;
        }

        /// <summary>
        /// Get booking number list
        /// </summary>
        /// <returns></returns>
        public async Task<BookingDataResponse> GetBookingNoList(BookingDataSourceRequest request)
        {
            if (request == null)
                return new BookingDataResponse() { Result = DataSourceResult.RequestNotCorrectFormat };

            var response = new BookingDataResponse();
            IQueryable<BookingRepo> bookingList;
            if (request.ServiceId == (int)Service.InspectionId)
            {
                //get inspection booking details
                bookingList = _extraFeesRepository.GetBookingNoList();
            }
            else if (request.ServiceId == (int)Service.AuditId)
            {
                //get audit booking details
                bookingList = _extraFeesRepository.GetAuditBookingList();
            }
            else
            {
                return new BookingDataResponse() { Result = DataSourceResult.ServiceIdRequired };
            }


            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                bookingList = bookingList.Where(x => EF.Functions.Like(x.BookingId.ToString(), $"%{request.SearchText.Trim()}%"));
            }
            if (request.CustomerId > 0)
            {
                bookingList = bookingList.Where(x => x.CustomerId == request.CustomerId);
            }
            if (request.SupplierId > 0)
            {
                bookingList = bookingList.Where(x => x.SupplierId == request.SupplierId);
            }
            if (request.FactoryId > 0)
            {
                bookingList = bookingList.Where(x => x.FactoryId == request.FactoryId);
            }
            if (request.BookingId > 0)
            {
                bookingList = bookingList.Where(x => x.BookingId == request.BookingId);
            }

            var bookingNoList = await bookingList.Skip(request.Skip).Take(request.Take).ToListAsync();

            if (!bookingNoList.Any())
            {
                return new BookingDataResponse() { Result = DataSourceResult.CannotGetList };
            }
         
            response.Bookings = bookingNoList.Select(x => new BookingDataItem()
            {
                Id = x.BookingId,
                Name = x.BookingId.ToString(),
                CustomerId= x.CustomerId,
                SupplierId = x.SupplierId,
                FactoryId = x.FactoryId.GetValueOrDefault(),
                CustomerName = x.CustomerName,
                SupplierName = x.SupplierName,
                FactoryName = x.FactoryName,
                ServiceDate = x.ServiceDate.ToString(StandardDateFormat)
            }).ToList();

            return response;
        }

        /// <summary>
        /// save the details
        /// </summary>
        /// <returns></returns>
        public async Task<SaveResponse> Save(ExtraFees request)
        {
            if (request == null)
                return new SaveResponse() { Result = Result.RequestNotCorrectFormat };

            bool isExistsExtraFee = false;
            //exists check 
            if (request.BilledToId > 0 && request.BookingNumberId > 0 && request.ServiceId > 0)
            {
                isExistsExtraFee = await _extraFeesRepository.ExistsExtraFee(request.BilledToId, request.BookingNumberId, request.Id, request.ServiceId);
            }

            if (isExistsExtraFee)
            {
                return new SaveResponse() { Result = Result.Exists };
            }
            else
            {
                //update
                if (request.Id > 0)
                {
                    var extraFees = await _extraFeesRepository.GetExtraFeeData(request.Id);

                    //update only below fields, if condition statisfy
                    if (extraFees.StatusId == (int)ExtraFeeStatus.Invoiced && _applicationContext.RoleList.Contains((int)RoleEnum.Accounting))
                    {
                        extraFees.PaymentDate = request.PaymentDate?.ToDateTime();
                        extraFees.PaymentStatus = request.PaymentStatusId;
                        extraFees.Remarks = request.Remarks;
                        extraFees.UpdatedBy = _applicationContext.UserId;
                        extraFees.UpdatedOn = DateTime.Now;
                        extraFees.OfficeId = request.OfficeId;
                        extraFees.ExtraFeeInvoiceDate = request.ExtraFeeInvoiceDate?.ToDateTime();

                        _extraFeesRepository.RemoveEntities(extraFees.InvExfContactDetails);

                        if (request.BilledToId == (int)InvoiceTo.Customer && request.ContactIdList.Any())
                        {
                            //entity.InvExfContactDetails
                            foreach (var contactId in request.ContactIdList)
                            {
                                InvExfContactDetail contactEntity = new InvExfContactDetail
                                {
                                    CustomerContactId = contactId
                                };
                                extraFees.InvExfContactDetails.Add(contactEntity);
                                _extraFeesRepository.AddEntity(contactEntity);
                            }
                        }

                        else if (request.BilledToId == (int)InvoiceTo.Supplier && request.ContactIdList.Any())
                        {
                            //entity.InvExfContactDetails
                            foreach (var contactId in request.ContactIdList)
                            {
                                InvExfContactDetail contactEntity = new InvExfContactDetail
                                {
                                    SupplierContactId = contactId
                                };
                                extraFees.InvExfContactDetails.Add(contactEntity);
                                _extraFeesRepository.AddEntity(contactEntity);
                            }
                        }

                        else if (request.BilledToId == (int)InvoiceTo.Factory && request.ContactIdList.Any())
                        {
                            //entity.InvExfContactDetails
                            foreach (var contactId in request.ContactIdList)
                            {
                                InvExfContactDetail contactEntity = new InvExfContactDetail
                                {
                                    FactoryContactId = contactId
                                };
                                extraFees.InvExfContactDetails.Add(contactEntity);
                                _extraFeesRepository.AddEntity(contactEntity);
                            }
                        }

                        _extraFeesRepository.EditEntity(extraFees);

                        await _extraFeesRepository.Save();

                    }
                    else
                    {
                        await UpdateExtraFee(request, extraFees);
                    }
                    return new SaveResponse() { Id = extraFees.Id, Result = Result.Success };
                }
                else
                {
                    //add
                    InvExfTransaction entity = new InvExfTransaction()
                    {
                        Active = true,
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        CustomerId = request.CustomerId,
                        SupplierId = request.SupplierId,
                        FactoryId = request.FactoryId,
                        BankId = request.BankId,
                        BilledTo = request.BilledToId,
                        BilledName = request.BilledName,
                        BilledAddress = request.BilledAddress,
                        PaymentTerms = request.PaymentTerms,
                        PaymentDuration = request.PaymentDuration,
                        BillingEntityId = request.BillingEntityId,
                        CurrencyId = request.CurrencyId,
                        ExtraFeeInvoiceNo = request.ExtraFeeInvoiceNo,
                        InvoiceId = request.InvoiceNumberId,
                        PaymentDate = request.PaymentDate?.ToDateTime(),
                        PaymentStatus = request.PaymentStatusId,
                        Remarks = request.Remarks,
                        ServiceId = request.ServiceId,
                        StatusId = (int)ExtraFeeStatus.Pending,
                        Tax = request.TaxValue,
                        TaxAmount = request.TaxAmt,
                        ExtraFeeSubTotal = request.SubTotal,
                        TotalExtraFee = request.TotalFees,
                        OfficeId = request.OfficeId,
                        ExtraFeeInvoiceDate = request.ExtraFeeInvoiceDate?.ToDateTime(),
                        EntityId = _filterService.GetCompanyId(),
                        InvoiceCurrencyId = request.InvoiceCurrencyId,
                        ExchangeRate = request.ExchangeRate
                    };

                    if (request.ServiceId == (int)Service.InspectionId)
                    {
                        entity.InspectionId = request.BookingNumberId;
                    }
                    else if (request.ServiceId == (int)Service.AuditId)
                    {
                        entity.AuditId = request.BookingNumberId;
                    }

                    await AddExtraFee(request, entity);

                    return new SaveResponse() { Id = entity.Id, Result = Result.Success };
                }
            }
        }

        /// <summary>
        /// update the entity
        /// </summary>
        /// <param name="request"></param>
        /// <param name="extraFees"></param>
        private async Task UpdateExtraFee(ExtraFees request, InvExfTransaction extraFees)
        {
            if (request.InvoiceNumberId > 0 && extraFees.StatusId == (int)ExtraFeeStatus.Pending && !(extraFees.InvoiceId > 0))
            {
                extraFees.StatusId = (int)ExtraFeeStatus.Invoiced;

                //log insert
                AddExtraFeeLog(extraFees, (int)ExtraFeeStatus.Invoiced);
            }
            extraFees.UpdatedBy = _applicationContext.UserId;
            extraFees.UpdatedOn = DateTime.Now;
            extraFees.CustomerId = request.CustomerId;
            extraFees.SupplierId = request.SupplierId;
            extraFees.FactoryId = request.FactoryId;
            extraFees.BankId = request.BankId;
            extraFees.BilledTo = request.BilledToId;
            extraFees.BillingEntityId = request.BillingEntityId;
            extraFees.CurrencyId = request.CurrencyId;
            extraFees.ExtraFeeInvoiceNo = request.ExtraFeeInvoiceNo;
            extraFees.InvoiceId = request.InvoiceNumberId;
            extraFees.PaymentDate = request.PaymentDate?.ToDateTime();
            extraFees.PaymentStatus = request.PaymentStatusId;
            extraFees.Remarks = request.Remarks;
            extraFees.ServiceId = request.ServiceId;
            extraFees.Tax = request.TaxValue;
            extraFees.TaxAmount = request.TaxAmt;
            extraFees.ExtraFeeSubTotal = request.SubTotal;
            extraFees.TotalExtraFee = request.TotalFees;
            extraFees.OfficeId = request.OfficeId;
            extraFees.ExchangeRate = request.ExchangeRate;
            extraFees.InvoiceCurrencyId = request.InvoiceCurrencyId;
            extraFees.ExtraFeeInvoiceDate = request.ExtraFeeInvoiceDate?.ToDateTime();
            extraFees.BilledName = request.BilledName;
            extraFees.BilledAddress = request.BilledAddress;
            extraFees.PaymentTerms = request.PaymentTerms;
            extraFees.PaymentDuration = request.PaymentDuration;

            if (request.ServiceId == (int)Service.InspectionId)
            {
                extraFees.InspectionId = request.BookingNumberId;
                extraFees.AuditId = null;
            }
            else if (request.ServiceId == (int)Service.AuditId)
            {
                extraFees.AuditId = request.BookingNumberId;
                extraFees.InspectionId = null;
            }

            //actvie false existing extra fee type list
            foreach (var typesList in extraFees.InvExfTranDetails)
            {
                typesList.Active = false;
                typesList.DeletedBy = _applicationContext.UserId;
                typesList.DeletedOn = DateTime.Now;

                _extraFeesRepository.EditEntity(typesList);
            }

            //add extra fee type list
            foreach (var typesList in request.ExtraFeeTypeList)
            {
                InvExfTranDetail entityTypeList = new InvExfTranDetail()
                {
                    Active = true,
                    CreatedBy = _applicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    Remarks = typesList.Remarks,
                    ExtraFeeType = typesList.TypeId,
                    ExtraFees = typesList.Fees,
                };
                extraFees.InvExfTranDetails.Add(entityTypeList);
                _extraFeesRepository.AddEntity(entityTypeList);

            }

            //remove from tax table
            _extraFeesRepository.RemoveEntities(extraFees.InvExtTranTaxes);

            if (extraFees.BankId.HasValue && extraFees.InspectionId.HasValue)
            {
                await AddExtraFeeTaxes(extraFees);
            }

            _extraFeesRepository.RemoveEntities(extraFees.InvExfContactDetails);

            if (request.BilledToId == (int)InvoiceTo.Customer && request.ContactIdList.Any())
            {
                //entity.InvExfContactDetails
                foreach (var contactId in request.ContactIdList)
                {
                    InvExfContactDetail contactEntity = new InvExfContactDetail
                    {
                        CustomerContactId = contactId
                    };
                    extraFees.InvExfContactDetails.Add(contactEntity);
                    _extraFeesRepository.AddEntity(contactEntity);
                }
            }

            else if (request.BilledToId == (int)InvoiceTo.Supplier && request.ContactIdList.Any())
            {
                //entity.InvExfContactDetails
                foreach (var contactId in request.ContactIdList)
                {
                    InvExfContactDetail contactEntity = new InvExfContactDetail
                    {
                        SupplierContactId = contactId
                    };
                    extraFees.InvExfContactDetails.Add(contactEntity);
                    _extraFeesRepository.AddEntity(contactEntity);
                }
            }

            else if (request.BilledToId == (int)InvoiceTo.Factory && request.ContactIdList.Any())
            {
                //entity.InvExfContactDetails
                foreach (var contactId in request.ContactIdList)
                {
                    InvExfContactDetail contactEntity = new InvExfContactDetail
                    {
                        FactoryContactId = contactId
                    };
                    extraFees.InvExfContactDetails.Add(contactEntity);
                    _extraFeesRepository.AddEntity(contactEntity);
                }
            }
            _extraFeesRepository.EditEntity(extraFees);

            await _extraFeesRepository.Save();
        }

        /// <summary>
        /// add the extra fee tax logic
        /// </summary>
        /// <param name="extraFees"></param>
        /// <returns></returns>
        private async Task AddExtraFeeTaxes(InvExfTransaction extraFees)
        {
            var taxResponse = await GetTaxValue(extraFees.BankId.Value, extraFees.InspectionId.Value);
            if (taxResponse != null && taxResponse.Result == Result.Success && taxResponse.TaxDetail != null && taxResponse.TaxDetail.Any())
            {
                foreach (var tax in taxResponse.TaxDetail)
                {
                    InvExtTranTax extraFeeTaxEntity = new InvExtTranTax()
                    {
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        TaxId = tax.Id,
                    };
                    extraFees.InvExtTranTaxes.Add(extraFeeTaxEntity);
                    _extraFeesRepository.AddEntity(extraFeeTaxEntity);
                }
            }

        }

        /// <summary>
        /// add the record to entity
        /// </summary>
        /// <param name="request"></param>
        private async Task AddExtraFee(ExtraFees request, InvExfTransaction entity)
        {

            //extra fee type list
            foreach (var typesList in request.ExtraFeeTypeList)
            {
                InvExfTranDetail entityTypeList = new InvExfTranDetail()
                {
                    Active = true,
                    CreatedBy = _applicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    Remarks = typesList.Remarks,
                    ExtraFeeType = typesList.TypeId,
                    ExtraFees = typesList.Fees,
                };
                entity.InvExfTranDetails.Add(entityTypeList);
                _extraFeesRepository.AddEntity(entityTypeList);

            }

            if (entity.BankId.HasValue && entity.InspectionId.HasValue)
            {
                await AddExtraFeeTaxes(entity);
            }



            if (request.BilledToId == (int)InvoiceTo.Customer && request.ContactIdList.Any())
            {
                //entity.InvExfContactDetails
                foreach (var contactId in request.ContactIdList)
                {
                    InvExfContactDetail contactEntity = new InvExfContactDetail
                    {
                        CustomerContactId = contactId
                    };
                    entity.InvExfContactDetails.Add(contactEntity);
                    _extraFeesRepository.AddEntity(contactEntity);
                }
            }

            else if (request.BilledToId == (int)InvoiceTo.Supplier && request.ContactIdList.Any())
            {
                //entity.InvExfContactDetails
                foreach (var contactId in request.ContactIdList)
                {
                    InvExfContactDetail contactEntity = new InvExfContactDetail
                    {
                        SupplierContactId = contactId
                    };
                    entity.InvExfContactDetails.Add(contactEntity);
                    _extraFeesRepository.AddEntity(contactEntity);
                }
            }

            else if (request.BilledToId == (int)InvoiceTo.Factory && request.ContactIdList.Any())
            {
                //entity.InvExfContactDetails
                foreach (var contactId in request.ContactIdList)
                {
                    InvExfContactDetail contactEntity = new InvExfContactDetail
                    {
                        FactoryContactId = contactId
                    };
                    entity.InvExfContactDetails.Add(contactEntity);
                    _extraFeesRepository.AddEntity(contactEntity);
                }
            }

            //  --log insert
            AddExtraFeeLog(entity, (int)ExtraFeeStatus.Pending);

            _extraFeesRepository.AddEntity(entity);

            await _extraFeesRepository.Save();

        }

        /// <summary>
        /// add to extra fee log table 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="statusId"></param>
        private void AddExtraFeeLog(InvExfTransaction entity, int statusId)
        {
            InvExfTranStatusLog logTable = new InvExfTranStatusLog()
            {
                CreatedBy = _applicationContext.UserId,
                CreatedOn = DateTime.Now,
                InspectionId = entity.InspectionId,
                StatusId = statusId,
                EntityId = _filterService.GetCompanyId()
            };

            entity.InvExfTranStatusLogs.Add(logTable);
            _extraFeesRepository.AddEntity(logTable);
        }

        /// <summary>
        /// get the tax value by bankid and booking id
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<TaxResponse> GetTaxValue(int bankId, int bookingId)
        {
            var response = new TaxResponse();

            var bookingDates = await _inspManager.getInspBookingDateDetails(bookingId);

            var taxDetails = await _invoiceManager.GetTaxDetails(bankId, bookingDates.ServiceDateTo, bookingDates.ServiceDateFrom);

            if (taxDetails.Any())
            {
                response.TaxDetail = taxDetails.ToList();
                response.Result = Result.Success;
            }
            else
            {
                response.Result = Result.NotFound;
            }

            return response;
        }

        /// <summary>
        /// get the extra fee details by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<EditResponse> Edit(int extraFeeId)
        {
            var response = new EditResponse();

            var extraFees = await _extraFeesRepository.GetExtraFees(extraFeeId);

            if (extraFees != null)
            {
                extraFees.ExtraFeeTypeList = await _extraFeesRepository.GetExtraFeesTypeById(extraFeeId);
                extraFees.ContactIdList = await _extraFeesRepository.GetExtraContactsById(extraFeeId);
            }

            if (extraFees.Id > 0)
            {
                response.Data = _extrafee.ExtraFeeEditMap(extraFees);
                response.Result = Result.Success;
            }
            else
            {
                response.Result = Result.NotFound;
            }

            return response;
        }

        /// <summary>
        /// Get invoice extra type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInvoiceNoList(int bookingId, int billedToId, int serviceId)
        {
            IEnumerable<CommonDataSource> invoiceNumberList = Enumerable.Empty<CommonDataSource>();

            if (serviceId == (int)Service.AuditId)
            {
                invoiceNumberList = await _extraFeesRepository.GetInvoiceNoListByAudit(bookingId, billedToId, serviceId);

            }
            else if (serviceId == (int)Service.InspectionId)
            {
                invoiceNumberList = await _extraFeesRepository.GetInvoiceNoList(bookingId, billedToId, serviceId);

            }

            if (invoiceNumberList != null && invoiceNumberList.Any())
            {
                return new DataSourceResponse() { DataSourceList = invoiceNumberList, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }
        /// <summary>
        /// inactive the cancel extrafee
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<CancelResponse> Cancel(int extraFeeId)
        {
            var response = new CancelResponse();

            var extraFees = await _extraFeesRepository.GetExtraFeeData(extraFeeId);

            if (extraFees.Id > 0)
            {
                extraFees.DeletedBy = _applicationContext.UserId;
                extraFees.DeletedOn = DateTime.Now;
                extraFees.InvoiceId = null;
                extraFees.ExtraFeeInvoiceNo = null;
                extraFees.StatusId = (int)ExtraFeeStatus.Cancelled;

                _extraFeesRepository.EditEntity(extraFees);

                foreach (var item in extraFees.InvExfTranDetails)
                {
                    item.DeletedBy = _applicationContext.UserId;
                    item.DeletedOn = DateTime.Now;
                    _extraFeesRepository.EditEntity(item);
                }

                AddExtraFeeLog(extraFees, (int)ExtraFeeStatus.Cancelled);

                await _extraFeesRepository.Save();

                response.Result = Result.Success;
            }
            else
            {
                response.Result = Result.NotFound;
            }
            return response;
        }

        /// <summary>
        /// generate manual invoice number with status update
        /// </summary>
        /// <param name="extraFeeId"></param>
        /// <returns></returns>
        public async Task<ManualInvoiceResponse> GenerateManualInvoice(int extraFeeId)
        {
            try
            {
                InvExfTransaction extraFees = await _extraFeesRepository.GetExtraFeeData(extraFeeId);

                //id map check 
                if (extraFees.InvoiceId == null)
                {
                    //invoice number generate
                    string invoiceNumber = InvoiceNumberGenerate(extraFees);

                    //exists check
                    if (await _extraFeesRepository.IsExistsInvoiceNumber(invoiceNumber))
                    {
                        return new ManualInvoiceResponse() { Result = Result.DuplicateInvoice };
                    }
                    else
                    {
                        extraFees.ExtraFeeInvoiceNo = invoiceNumber;
                        extraFees.StatusId = (int)ExtraFeeStatus.Invoiced;
                        extraFees.UpdatedBy = _applicationContext.UserId;
                        extraFees.UpdatedOn = DateTime.Now;

                        _extraFeesRepository.EditEntity(extraFees);

                        AddExtraFeeLog(extraFees, (int)ExtraFeeStatus.Invoiced);

                        await _extraFeesRepository.Save();
                        return new ManualInvoiceResponse() { Id = extraFees.Id, Result = Result.Success };
                    }
                }
                else
                {
                    return new ManualInvoiceResponse() { Result = Result.InvoiceIdMapped };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// generate invoice number
        /// </summary>
        /// <param name="extraFees"></param>
        /// <returns></returns>
        private string InvoiceNumberGenerate(InvExfTransaction extraFees)
        {

            string FormatInvoiceNumber = string.Empty;
            int? BookingNumber = 0;

            //Inoive Format number Inspection - INS_EXF_CU_989, AUdit = AUD_EXF_SU_4848
            if (extraFees.ServiceId == (int)Service.InspectionId)
            {
                //if inspection INS 
                FormatInvoiceNumber = InvoiceInspWord;
                BookingNumber = extraFees.InspectionId;
            }
            else if (extraFees.ServiceId == (int)Service.AuditId)
            {
                //if audit AUD
                FormatInvoiceNumber = InvoiceAudWord;
                BookingNumber = extraFees.AuditId;
            }

            FormatInvoiceNumber = FormatInvoiceNumber + InvoiceUnderScore + InvoiceWord + InvoiceUnderScore;

            if (extraFees.BilledTo == (int)InvoiceTo.Customer)
            {
                FormatInvoiceNumber = FormatInvoiceNumber + InvoiceCustomer;
            }
            else if (extraFees.BilledTo == (int)InvoiceTo.Supplier)
            {
                FormatInvoiceNumber = FormatInvoiceNumber + InvoiceSupplier;
            }
            else if (extraFees.BilledTo == (int)InvoiceTo.Factory)
            {
                FormatInvoiceNumber = FormatInvoiceNumber + InvoiceFactory;
            }

            FormatInvoiceNumber = FormatInvoiceNumber + InvoiceUnderScore + BookingNumber;

            return FormatInvoiceNumber;
        }

        private IQueryable<ExtraFeeSummaryItem> FilterSummaryData(IQueryable<ExtraFeeSummaryItem> data, ExtraFeeRequest request)
        {
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 20;

            var cuslist = new List<int>();
            if (_applicationContext.UserType == UserTypeEnum.InternalUser)
            {
                if (request?.CustomerId > 0)
                    cuslist.Add(request.CustomerId.GetValueOrDefault());
            }
            else
            {
                if (request?.CustomerId > 0)
                    cuslist.Add(request.CustomerId.GetValueOrDefault());
            }

            if (request?.CustomerId > 0)
            {
                data = data.Where(x => x.CustomerId == request.CustomerId.GetValueOrDefault());
            }

            if (request?.SupplierId > 0)
            {
                data = data.Where(x => x.SupplierId == request.SupplierId.GetValueOrDefault());
            }
            if (request?.billedTo > 0)
            {
                data = data.Where(x => x.BilledToId == request.billedTo.GetValueOrDefault());
            }
            if (request.statuslst != null && request.statuslst.Any())
            {
                data = data.Where(x => x.StatusId.HasValue && request.statuslst.Contains(x.StatusId.Value));
            }
            if (request?.serviceId > 0)
            {
                data = data.Where(x => x.ServiceId == request.serviceId.GetValueOrDefault());
            }

            if (Enum.TryParse(request.DateTypeid.ToString(), out SearchType _datesearchtype))
            {
                if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
                {
                    switch (_datesearchtype)
                    {
                        case SearchType.ApplyDate:
                            {
                                data = data.Where(x => EF.Functions.DateDiffDay(request.FromDate.ToDateTime(), x.ApplyDate) >= 0 &&
                                                     EF.Functions.DateDiffDay(x.ApplyDate, request.ToDate.ToDateTime()) >= 0);
                                break;
                            }
                        case SearchType.ServiceDate:
                            {
                                data = data.Where(x => !((x.ServiceDateFrom > request.ToDate.ToDateTime()) || (x.ServiceDateTo < request.FromDate.ToDateTime())));
                                break;
                            }
                    }
                }
            }

            if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()))
            {
                if (Enum.TryParse(request.SearchTypeId.ToString(), out SearchType _seachtypeenum))
                {
                    switch (_seachtypeenum)
                    {
                        case SearchType.BookingNo:
                            {
                                if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()) && int.TryParse(request.SearchTypeText?.Trim(), out int bookid))
                                {
                                    data = data.Where(x => x.BookingId == bookid);
                                }
                                break;
                            }
                        case SearchType.CustomerBookingNo:
                            if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()))
                            {
                                data = data.Where(x => EF.Functions.Like(x.CustomerBookingNo.Trim(), $"%{request.SearchTypeText.Trim()}%"));
                            }
                            break;
                    }
                }
            }
            return data;
        }

        public async Task<ExtraFeeResponse> GetExFeeSummary(ExtraFeeRequest request)
        {
            if (request == null)
                return new ExtraFeeResponse() { Result = ExtraFeeSummaryResult.NotFound };

            var response = new ExtraFeeResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            var data = request.serviceId == (int)Service.InspectionId ? _extraFeesRepository.GetExFeeData() : _extraFeesRepository.GetAuditExFeeData();

            if (data == null)
                return new ExtraFeeResponse() { Result = ExtraFeeSummaryResult.NotFound };

            data = FilterSummaryData(data, request);

            var statuslist = data.Select(x => new { x.StatusId, x.StatusName })
                         .GroupBy(p => new { p.StatusId, p.StatusName }, p => p, (key, _data) =>
                       new ExtraFeeSummaryStatus
                       {
                           Id = key.StatusId.GetValueOrDefault(),
                           StatusName = key.StatusName,
                           TotalCount = _data.Count(),
                           StatusColor = ExtraFeeStatusColor.GetValueOrDefault(key.StatusId.GetValueOrDefault(), "")
                       }).ToList();

            var result = await data.Skip(skip).Take(take).ToListAsync();

            if (result == null || !result.Any())
                return new ExtraFeeResponse() { Result = ExtraFeeSummaryResult.NotFound };

            var exfeeTranIdList = result.Select(x => x.ExfTranId.GetValueOrDefault()).Distinct().ToList();

            var exFeeData = await _extraFeesRepository.GetExFeeTranDetails(exfeeTranIdList);

            var res = result.Select(x => _extrafee.ExtraFeeSummaryMap(x, exFeeData.Where(y => y.ExfTranId == x.ExfTranId).ToList()));

            return new ExtraFeeResponse
            {
                Result = ExtraFeeSummaryResult.Success,
                TotalCount = await data.CountAsync(),
                Index = request.Index.Value,
                PageSize = request.pageSize.Value,
                PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                Data = res.ToList(),
                StatusCountList = statuslist
            };
        }

        public async Task<List<ExtraFeeSummaryExportItem>> ExportExtrafeeSearchSummary(ExtraFeeRequest requestDto)
        {
            //int skip = (requestDto.Index.Value - 1) * requestDto.pageSize.Value;

            //int take = requestDto.pageSize.Value;

            var data = requestDto.serviceId == (int)Service.InspectionId ? _extraFeesRepository.GetExFeeDetailsData() : _extraFeesRepository.GetExtraFeeDetailsAuditData();

            if (data == null)
                return null;

            data = FilterSummaryData(data, requestDto);

            var result = await data.ToListAsync();

            var response = _extrafee.ExtraFeeSummaryExportMap(result);

            return response;
        }

        /// <summary>
        /// fetch all the invoice status
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetExtraFeeStatusList()
        {
            var data = await _extraFeesRepository.GetExtraFeeStatus();

            if (data == null || !data.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = data,
                Result = DataSourceResult.Success
            };
        }


        /// <summary>
        /// cancel extra fee invoice
        /// </summary>
        /// <returns></returns>
        public async Task<SaveResponse> CancelExtraFeeInvoice(int extraFeeId)
        {
            var extraFee = await _extraFeesRepository.GetExtraFeeData(extraFeeId);

            if (extraFee == null)
            {
                return new SaveResponse { Result = Result.NotFound };
            }

            extraFee.StatusId = (int)ExtraFeeStatus.Pending;
            extraFee.ExtraFeeInvoiceNo = null;
            extraFee.ExtraFeeInvoiceDate = null;

            _extraFeesRepository.EditEntity(extraFee);
            await _extraFeesRepository.Save();
            return new SaveResponse() { Result = Result.Success };
        }
    }
}

