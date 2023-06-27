using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.InvoiceDataAccess;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class InvoiceDataAccessManager : IInvoiceDataAccessManager
    {
        private readonly IInvoiceDataAccessRepository _invoiceDataAccessRepository = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly ITenantProvider _filterService = null;

        public InvoiceDataAccessManager(IInvoiceDataAccessRepository invoiceDataAccessRepository, IAPIUserContext applicationContext, ITenantProvider filterService)
        {
            _invoiceDataAccessRepository = invoiceDataAccessRepository;
            _applicationContext = applicationContext;
            _filterService = filterService;
        }


        public async Task<SaveInvoiceDataAccessResponse> Save(SaveInvoiceDataAccessRequest request)
        {
            if (request == null)
                return new SaveInvoiceDataAccessResponse() { Result = SaveInvoiceDataAccessResult.RequestNotCorrectFormat };

            var response = new SaveInvoiceDataAccessResponse();

            try
            {
                if (request.Id == 0 && await _invoiceDataAccessRepository.IsStaffHasInvoiceDataAccess(request.StaffId))
                {
                    return new SaveInvoiceDataAccessResponse() { Result = SaveInvoiceDataAccessResult.Exists };
                }

                if (request.Id > 0)
                {
                    var invoiceAccessData = await _invoiceDataAccessRepository.GetInvoiceDataAccess(request.Id);
                    response.Id = await EditInvoiceDataAccess(request, invoiceAccessData);
                }
                else
                {
                    response.Id = await AddInvoiceDataAccess(request);
                }

                response.Result = SaveInvoiceDataAccessResult.Success;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<EditInvoiceDataAccessResponse> Edit(int id)
        {
            try
            {
                var response = new EditInvoiceDataAccessResponse();

                var invoiceDataAccessQuery = _invoiceDataAccessRepository.GetInvoiceDataAccessQuery();

                if (id > 0)
                {

                    var invoiceDataaccess = await invoiceDataAccessQuery.FirstOrDefaultAsync(x => x.Id == id);

                    var invoiceDataAccessIds = new[] { id }.ToList();

                    if (invoiceDataaccess != null)
                    {
                        var customerAccess = await _invoiceDataAccessRepository.GetInvoiceCustomerDataAccess(invoiceDataAccessIds);
                        var officeAccess = await _invoiceDataAccessRepository.GetInvoiceOfficeDataAccess(invoiceDataAccessIds);
                        var invoiceTypeAccess = await _invoiceDataAccessRepository.GetInvoiceTypeDataAccess(invoiceDataAccessIds);
                        response.InvoiceDataAccess = new InvoiceDataAccessEditSummaryItem()
                        {
                            Id = invoiceDataaccess.Id,
                            StaffId = invoiceDataaccess.StaffId,
                            StaffName = invoiceDataaccess.Staff.PersonName,
                            CustomerList = customerAccess.Select(x => x.CustomerId).ToList(),
                            OfficeList = officeAccess.Select(x => x.InvoiceOfficeId).ToList(),
                            InvoiceTypeList = invoiceTypeAccess.Select(x => x.InvoiceTypeId).ToList()
                        };
                        response.Result = InvoiceDataAccessResponseResult.Success;
                    }
                    else
                    {
                        response.Result = InvoiceDataAccessResponseResult.NotFound;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                return new EditInvoiceDataAccessResponse() { Result = InvoiceDataAccessResponseResult.Error };
            }
        }

        //summary data search based on request filters
        public async Task<InvoiceDataAccessSummaryResponse> GetSummaryData(InvoiceDataAccessSummaryRequest request)
        {

            if (request == null)
                return new InvoiceDataAccessSummaryResponse() { Result = InvoiceDataAccessSummaryResult.RequestNotCorrectFormat };

            var response = new InvoiceDataAccessSummaryResponse { Index = request.Index.GetValueOrDefault(), PageSize = request.pageSize.GetValueOrDefault() };
            try
            {
                if (request.Index == null || request.Index.Value <= 0)
                    request.Index = 1;

                if (request.pageSize == null || request.pageSize.Value == 0)
                    request.pageSize = 20;

                int skip = (request.Index.Value - 1) * request.pageSize.Value;

                int take = request.pageSize.Value;

                var data = _invoiceDataAccessRepository.GetInvoiceDataAccessQuery();

                if (request != null && request.StaffId > 0)
                {
                    data = data.Where(x => x.StaffId == request.StaffId);
                }

                if (request.CustomerIdList != null && request.CustomerIdList.Any())
                {
                    data = data.Where(x => x.InvDaCustomers.Any(y => y.Active && request.CustomerIdList.Contains(y.CustomerId)));
                }

                if (request.InvoiceTypeIdList != null && request.InvoiceTypeIdList.Any())
                {
                    data = data.Where(x => x.InvDaInvoiceTypes.Any(y => y.Active && request.InvoiceTypeIdList.Contains(y.InvoiceTypeId)));
                }

                if (request.OfficeIdList != null && request.OfficeIdList.Any())
                {
                    data = data.Where(x => x.InvDaOffices.Any(y => y.Active && request.OfficeIdList.Contains(y.OfficeId)));
                }

                // take total count after filter
                response.TotalCount = data.Count();

                if (response.TotalCount == 0)
                {
                    response.Result = InvoiceDataAccessSummaryResult.NotFound;
                    return response;
                }

                var result = await data.Select(x => new InvoiceDataAccessSummaryItem
                {
                    Id = x.Id,
                    StaffName = x.Staff.PersonName
                }).Skip(skip).Take(take).AsNoTracking().ToListAsync();

                var invoiceDataAccessIds = result.Select(x => x.Id).ToList();

                var customerNameList = await _invoiceDataAccessRepository.GetInvoiceCustomerDataAccess(invoiceDataAccessIds);
                var invoiceTypeList = await _invoiceDataAccessRepository.GetInvoiceTypeDataAccess(invoiceDataAccessIds);
                var officeList = await _invoiceDataAccessRepository.GetInvoiceOfficeDataAccess(invoiceDataAccessIds);

                foreach (var item in result)
                {
                    item.CustomerList = customerNameList.Where(x => x.DataAccessId == item.Id).ToList();
                    item.InvoiceTypeList = invoiceTypeList.Where(x => x.DataAccessId == item.Id).ToList();
                    item.OfficeList = officeList.Where(x => x.DataAccessId == item.Id).ToList();
                }

                return new InvoiceDataAccessSummaryResponse()
                {
                    Result = InvoiceDataAccessSummaryResult.Success,
                    TotalCount = response.TotalCount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                    InvoiceDataAccessSummaryList = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DeleteInvoiceDataAccessResponseResult> Delete(int id)
        {
            try
            {
                //get saved user data values by id
                var dataAccess = await _invoiceDataAccessRepository.GetInvoiceDataAccess(id);

                if (dataAccess != null)
                {
                    dataAccess.Active = false;
                    dataAccess.DeletedBy = _applicationContext.UserId;
                    dataAccess.DeletedOn = DateTime.Now;

                    if (dataAccess.InvDaCustomers != null && dataAccess.InvDaCustomers.Any())
                    {
                        foreach (var data in dataAccess.InvDaCustomers.Where(x => x.Active))
                        {
                            data.Active = false;
                            data.DeletedOn = DateTime.Now;
                            data.DeletedBy = _applicationContext.UserId;
                            _invoiceDataAccessRepository.EditEntity(data);
                        }
                    }

                    if (dataAccess.InvDaInvoiceTypes != null && dataAccess.InvDaInvoiceTypes.Any())
                    {
                        foreach (var data in dataAccess.InvDaInvoiceTypes.Where(x => x.Active))
                        {
                            data.Active = false;
                            data.DeletedOn = DateTime.Now;
                            data.DeletedBy = _applicationContext.UserId;
                            _invoiceDataAccessRepository.EditEntity(data);
                        }
                    }

                    if (dataAccess.InvDaOffices != null && dataAccess.InvDaOffices.Any())
                    {
                        foreach (var data in dataAccess.InvDaOffices.Where(x => x.Active))
                        {
                            data.Active = false;
                            data.DeletedOn = DateTime.Now;
                            data.DeletedBy = _applicationContext.UserId;
                            _invoiceDataAccessRepository.EditEntity(data);
                        }
                    }

                    _invoiceDataAccessRepository.EditEntity(dataAccess);
                    await _invoiceDataAccessRepository.Save();
                }

                return DeleteInvoiceDataAccessResponseResult.Success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check is invoice access exist
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<bool> IsInvoiceDataAccessExist(SaveInvoiceDataAccessRequest request)
        {
            var isDataExist = false;
            var invoiceAccessList = await _invoiceDataAccessRepository.GetInvoiceDataAccessListExist(request.Id);

            if (invoiceAccessList.Any())
            {
                if (request.StaffId > 0)
                {
                    invoiceAccessList = invoiceAccessList.Where(x => x.StaffId == request.StaffId).ToList();
                }

                if (request.CustomerIdList != null && request.CustomerIdList.Any() && invoiceAccessList.SelectMany(y => y.InvDaCustomers).Any(x => x.Active))
                {
                    invoiceAccessList = invoiceAccessList.Where(x => x.InvDaCustomers.Any(y => y.Active && request.CustomerIdList.Contains(y.CustomerId))).ToList();
                }

                if (request.InvoiceTypeIdList != null && request.InvoiceTypeIdList.Any() && invoiceAccessList.SelectMany(y => y.InvDaInvoiceTypes).Any(x => x.Active))
                {
                    invoiceAccessList = invoiceAccessList.Where(x => x.InvDaInvoiceTypes.Any(y => y.Active && request.InvoiceTypeIdList.Contains(y.InvoiceTypeId))).ToList();
                }

                if (request.OfficeIdList != null && request.OfficeIdList.Any() && invoiceAccessList.SelectMany(y => y.InvDaOffices).Any(x => x.Active))
                {
                    invoiceAccessList = invoiceAccessList.Where(x => x.InvDaOffices.Any(y => y.Active && request.OfficeIdList.Contains(y.OfficeId))).ToList();
                }

                if (invoiceAccessList.Any())
                {
                    isDataExist = true;
                }
            }
            return isDataExist;
        }



        private async Task<int> EditInvoiceDataAccess(SaveInvoiceDataAccessRequest request, InvDaTransaction invoiceDataAccess)
        {
            try
            {
                invoiceDataAccess.StaffId = request.StaffId;
                invoiceDataAccess.UpdatedBy = _applicationContext.UserId;
                invoiceDataAccess.UpdatedOn = DateTime.Now;

                // update child entity
                UpdateCustomerDataAccess(request.CustomerIdList, invoiceDataAccess);
                UpdateOfficeDataAccess(request.OfficeIdList, invoiceDataAccess);
                UpdateInvoiceTypeDataAccess(request.InvoiceTypeIdList, invoiceDataAccess);

                _invoiceDataAccessRepository.EditEntity(invoiceDataAccess);
                await _invoiceDataAccessRepository.Save();

                return invoiceDataAccess.Id;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateOfficeDataAccess(IEnumerable<int> requestOfficeAccessIdList, InvDaTransaction invDaTransaction)
        {
            //get to add a new data from request
            var addOfficeDataAccessIds = requestOfficeAccessIdList?.Except(invDaTransaction.InvDaOffices.Where(x => x.Active).
                                                            Select(x => x.OfficeId).ToList());
            //unselected data from request to remove from DB
            var removeOfficeList = invDaTransaction?.InvDaOffices?.Where(x => x.Active
                                                                && !requestOfficeAccessIdList.Contains(x.OfficeId));

            if (addOfficeDataAccessIds != null)
            {
                //add
                foreach (var id in addOfficeDataAccessIds)
                {
                    var officeAccess = new InvDaOffice()
                    {
                        OfficeId = id,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    invDaTransaction.InvDaOffices.Add(officeAccess);
                    _invoiceDataAccessRepository.AddEntity(officeAccess);
                }
            }

            if (removeOfficeList != null)
            {
                //remove
                foreach (var data in removeOfficeList)
                {
                    data.Active = false;
                    data.DeletedOn = DateTime.Now;
                    data.DeletedBy = _applicationContext.UserId;
                    _invoiceDataAccessRepository.EditEntity(data);
                }
            }
        }

        private void UpdateInvoiceTypeDataAccess(IEnumerable<int> requestInvoiceTypeAccessIdList, InvDaTransaction invDaTransaction)
        {
            //get to add a new data from request
            var addInvoicetypeDataAccessIds = requestInvoiceTypeAccessIdList?.Except(invDaTransaction.InvDaInvoiceTypes.Where(x => x.Active).
                                                            Select(x => x.InvoiceTypeId).ToList());
            //unselected data from request to remove from DB
            var removeInvoiceTypeList = invDaTransaction?.InvDaInvoiceTypes?.Where(x => x.Active
                                                                && !requestInvoiceTypeAccessIdList.Contains(x.InvoiceTypeId));

            if (addInvoicetypeDataAccessIds != null)
            {
                //add
                foreach (var id in addInvoicetypeDataAccessIds)
                {
                    var invoiceAccess = new InvDaInvoiceType()
                    {
                        InvoiceTypeId = id,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    invDaTransaction.InvDaInvoiceTypes.Add(invoiceAccess);
                    _invoiceDataAccessRepository.AddEntity(invoiceAccess);
                }
            }

            if (removeInvoiceTypeList != null)
            {
                //remove
                foreach (var data in removeInvoiceTypeList)
                {
                    data.Active = false;
                    data.DeletedOn = DateTime.Now;
                    data.DeletedBy = _applicationContext.UserId;
                    _invoiceDataAccessRepository.EditEntity(data);
                }
            }
        }

        private void UpdateCustomerDataAccess(IEnumerable<int> requestCustomerAccessIdList, InvDaTransaction invDaTransaction)
        {
            //get to add a new data from request
            var addCustomerDataAccessIds = requestCustomerAccessIdList?.Except(invDaTransaction.InvDaCustomers.Where(x => x.Active).
                                                            Select(x => x.CustomerId).ToList());
            //unselected data from request to remove from DB
            var removeCustomerIdList = invDaTransaction?.InvDaCustomers?.Where(x => x.Active
                                                                && !requestCustomerAccessIdList.Contains(x.CustomerId));

            if (addCustomerDataAccessIds != null)
            {
                //add
                foreach (var id in addCustomerDataAccessIds)
                {
                    var customerAccess = new InvDaCustomer()
                    {
                        CustomerId = id,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    invDaTransaction.InvDaCustomers.Add(customerAccess);
                    _invoiceDataAccessRepository.AddEntity(customerAccess);
                }
            }

            if (removeCustomerIdList != null)
            {
                //remove
                foreach (var productCategoryData in removeCustomerIdList)
                {
                    productCategoryData.Active = false;
                    productCategoryData.DeletedOn = DateTime.Now;
                    productCategoryData.DeletedBy = _applicationContext.UserId;
                    _invoiceDataAccessRepository.EditEntity(productCategoryData);
                }
            }
        }
        /// <summary>
        /// Add invoice data access
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<int> AddInvoiceDataAccess(SaveInvoiceDataAccessRequest request)
        {
            try
            {
                var entity = new InvDaTransaction()
                {
                    CreatedBy = _applicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    Active = true,
                    StaffId = request.StaffId,
                    EntityId = _filterService.GetCompanyId()
                };
                _invoiceDataAccessRepository.AddEntity(entity);
                // child access for staff
                AddInvoiceDataCustomerAccess(request, entity);
                AddInvoiceDataOfficeAccess(request, entity);
                AddInvoiceDataInvoiceTypeAccess(request, entity);
                await _invoiceDataAccessRepository.Save();
                return entity.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add invoice data customer access for staff
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInvoiceDataCustomerAccess(SaveInvoiceDataAccessRequest request, InvDaTransaction entity)
        {
            if (request.CustomerIdList != null)
            {
                foreach (var customerId in request.CustomerIdList)
                {
                    var daCustomerAccess = new InvDaCustomer()
                    {
                        CustomerId = customerId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    entity.InvDaCustomers.Add(daCustomerAccess);
                    _invoiceDataAccessRepository.AddEntity(daCustomerAccess);
                }
            }
        }

        /// <summary>
        /// Add invoice office access list
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInvoiceDataOfficeAccess(SaveInvoiceDataAccessRequest request, InvDaTransaction entity)
        {
            if (request.OfficeIdList != null)
            {
                foreach (var officeId in request.OfficeIdList)
                {
                    var daOfficeAccess = new InvDaOffice()
                    {
                        OfficeId = officeId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    entity.InvDaOffices.Add(daOfficeAccess);
                    _invoiceDataAccessRepository.AddEntity(daOfficeAccess);
                }
            }
        }

        private void AddInvoiceDataInvoiceTypeAccess(SaveInvoiceDataAccessRequest request, InvDaTransaction entity)
        {
            if (request.InvoiceTypeIdList != null)
            {
                foreach (var invoiceTypeId in request.InvoiceTypeIdList)
                {
                    var daInvoiceTypeAccess = new InvDaInvoiceType()
                    {
                        InvoiceTypeId = invoiceTypeId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    entity.InvDaInvoiceTypes.Add(daInvoiceTypeAccess);
                    _invoiceDataAccessRepository.AddEntity(daInvoiceTypeAccess);
                }
            }
        }

        public async Task<InvoiceDataAccess> GetStaffInvoiceDataAcesss(int staffId)
        {
            var invoiceDataAccess = await _invoiceDataAccessRepository.GetStaffInvoiceDataAccess(staffId);
            if (invoiceDataAccess == null)
                return null;
            return new InvoiceDataAccess()
            {
                StaffId = invoiceDataAccess.StaffId,
                CustomerIds = invoiceDataAccess.InvDaCustomers.Where(x => x.Active == true).Select(y => y.CustomerId).Distinct().ToList(),
                OfficeIds = invoiceDataAccess.InvDaOffices.Where(x => x.Active == true).Select(y => y.OfficeId).Distinct().ToList(),
                InvoiceTypes = invoiceDataAccess.InvDaInvoiceTypes.Where(x => x.Active == true).Select(y => y.InvoiceTypeId).Distinct().ToList(),
            };
        }
    }
}
