using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Invoice;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class InvoiceBankManager : IInvoiceBankManager
    {
        private readonly IInvoiceBankRepository _repo = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly InvoiceBankMap _invoicemap = null;
        private ITenantProvider _filterService = null;
        public InvoiceBankManager(IInvoiceBankRepository repo, IAPIUserContext applicationContext, ITenantProvider filterService)
        {
            _repo = repo;
            _applicationContext = applicationContext;
            _invoicemap = new InvoiceBankMap();
            _filterService = filterService;
        }

        /// <summary>
        /// Save Invoice Bank details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<InvoiceBankSaveResponse> SaveInvoiceBankDetails(InvoiceBankSaveRequest request)
        {
            try
            {
                var response = new InvoiceBankSaveResponse();

                //check new record
                if (request.Id == 0)
                {
                    // check the bank account is already exist           
                    response = await CheckBankAccountIsNotExist(request);

                    if (response.Result == InvoiceBankSaveResult.InvoiceBankIsNotExist)
                    {
                        // map invoice bank request and entity
                        var bankEntity = _invoicemap.GetInvoiceBankSaveMap(request, _applicationContext.UserId, _filterService.GetCompanyId());

                        // add invoice bank tax if available
                        AddInvoiceBankTaxList(request, bankEntity);

                        _repo.AddEntity(bankEntity);

                        await _repo.Save();

                        response.Result = InvoiceBankSaveResult.Success;
                    }
                    else
                    {
                        response.Result = InvoiceBankSaveResult.InvoiceBankAccountIsAlreadyExist;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update Invoice Bank details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<InvoiceBankSaveResponse> UpdateInvoiceBankDetails(InvoiceBankSaveRequest request)
        {
            try
            {
                var response = new InvoiceBankSaveResponse();

                if (request.Id > 0)
                {
                    // get bank and bank tax details
                    var bankEntity = await _repo.GetInvoiceBankById(request.Id);

                    if (bankEntity == null)
                    {
                        return new InvoiceBankSaveResponse() { Result = InvoiceBankSaveResult.InvoiceBankIsNotExist };
                    }

                    if (request != null && bankEntity != null)
                    {
                        // map the bank entity from the request to update.
                        _invoicemap.GetInvoiceBankUpdateMap(request, bankEntity);
                        bankEntity.UpdatedBy = _applicationContext.UserId;
                    }

                    // update bank tax details
                    if (bankEntity.InvTranBankTaxes.Any())
                    {
                        // delete if the record is not exist in the request but from the db
                        DeleteInvoiceBankTaxList(request, bankEntity);

                        // update if the record is exist in both request and db
                        UpdateInvoiceBankTaxList(request, bankEntity);
                    }

                    // add if the record id is zero from the request
                    AddInvoiceBankTaxList(request, bankEntity);

                    await _repo.Save();
                }

                response.Result = InvoiceBankSaveResult.Success;

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the bank list
        /// </summary>
        /// <returns></returns>
        public async Task<InvoiceBankGetAllResponse> GetAllInvoiceBankDetails(InvoiceBankSummary request)
        {
            var response = new InvoiceBankGetAllResponse();

            if (request == null)
                return new InvoiceBankGetAllResponse() { Result = InvoiceBankGetAllResult.InvoiceBankNotFound };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value <= 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            var bankList = await _repo.GetInvoiceBankList();

            if (bankList.Any())
            {
                var count = bankList.Count();
                response.BankDetails = bankList.Skip(skip).Take(take).ToList();
                response.TotalCount = count;
                response.Index = request.Index.Value;
                response.PageSize = request.PageSize.Value;
                response.PageCount = (count / request.PageSize.Value) + (count % request.PageSize.Value > 0 ? 1 : 0);
                response.Result = InvoiceBankGetAllResult.Success;
            }
            else
            {
                response.Result = InvoiceBankGetAllResult.InvoiceBankNotFound;
                response.BankDetails = null;
            }
            return response;
        }

        /// <summary>
        /// Get Invoice bank details by bankId
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public async Task<InvoiceBankGetResponse> GetInvoiceBankDetails(int bankId)
        {
            var response = new InvoiceBankGetResponse();
            var bankTaxList = new List<InvoiceBankTax>();
            try
            {

                var bankInfo = await _repo.GetInvoiceBankById(bankId);

                if (bankInfo == null)
                {
                    return new InvoiceBankGetResponse()
                    {
                        BankDetails = null,
                        BankTaxDetails = null,
                        Result = InvoiceBankGetResult.InvoiceBankNotFound
                    };
                }

                foreach (var bankTax in bankInfo.InvTranBankTaxes.Where(x => x.Active))
                {
                    bankTaxList.Add(_invoicemap.GetInvoiceBankTaxDetailsMap(bankTax));
                }

                response.BankDetails = _invoicemap.GetInvoiceBankDetailsMap(bankInfo);
                response.BankTaxDetails = bankTaxList;
                response.Result = InvoiceBankGetResult.Success;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Remove Invoice bank details
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public async Task<InvoiceBankDeleteResponse> RemoveInvoiceBankDetails(int bankId)
        {
            var bankData = await _repo.GetInvoiceBankById(bankId);
            var bankTaxList = new List<InvTranBankTax>();

            if (bankData == null)
            {
                return new InvoiceBankDeleteResponse() { Result = InvoiceBankDeleteResult.NotFound };
            }

            // Inactive bank tax detail 
            if (bankData.InvTranBankTaxes.Any())
            {
                foreach (var tax in bankData.InvTranBankTaxes)
                {
                    tax.Active = false;
                    tax.DeletedBy = _applicationContext.UserId;
                    tax.DeletedOn = DateTime.Now;
                    bankTaxList.Add(tax);
                }
                _repo.EditEntities(bankTaxList);
            }
            // In active bank detail 
            bankData.Active = false;
            bankData.DeletedOn = DateTime.Now;
            bankData.DeletedBy = _applicationContext.UserId;

            _repo.EditEntity(bankData);

            await _repo.Save();

            return new InvoiceBankDeleteResponse { Id = bankId, Result = InvoiceBankDeleteResult.Success };

        }

        /// <summary>
        /// Add Invoice bank tax
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddInvoiceBankTaxList(InvoiceBankSaveRequest request, InvRefBank entity)
        {
            if (request.InvoiceBankTaxList != null)
            {
                foreach (var taxItem in request.InvoiceBankTaxList.Where(x => x.Id == 0))
                {
                    var bankTaxentity = _invoicemap.GetInvoiceBankTaxSaveMap(taxItem, _applicationContext.UserId);

                    entity.InvTranBankTaxes.Add(bankTaxentity);
                    _repo.AddEntity(bankTaxentity);
                }
            }
        }

        /// <summary>
        /// Delete Invoice bank tax
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void DeleteInvoiceBankTaxList(InvoiceBankSaveRequest request, InvRefBank entity)
        {
            if (request.InvoiceBankTaxList != null)
            {

                // Remove Logic for bank tax start
                var removebankTaxList = new List<InvTranBankTax>();

                var bankTaxesRemovedFromTheRequest = entity.InvTranBankTaxes.
                                                     Where(x => x.Active && !request.InvoiceBankTaxList.Where(y => y.Id > 0)
                                                    .Select(z => z.Id).Contains(x.Id));

                foreach (var tax in bankTaxesRemovedFromTheRequest)
                {

                    tax.DeletedOn = DateTime.Now;
                    tax.DeletedBy = _applicationContext.UserId;
                    tax.Active = false;
                    removebankTaxList.Add(tax);
                }
                _repo.EditEntities(removebankTaxList);

                // Remove Logic for bank tax end
            }
        }

        /// <summary>
        /// Update Invoice Bank taxex
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void UpdateInvoiceBankTaxList(InvoiceBankSaveRequest request, InvRefBank entity)
        {
            if (request.InvoiceBankTaxList != null)
            {
                // Update Logic for bank tax start
                var editbankTaxList = new List<InvTranBankTax>();

                foreach (var tax in request.InvoiceBankTaxList.Where(x => x.Id > 0))
                {
                    var taxData = entity.InvTranBankTaxes.Where(x => x.Active && x.Id == tax.Id).FirstOrDefault();

                    if (taxData != null)
                    {
                        taxData.TaxName = tax.TaxName;
                        taxData.TaxValue = tax.TaxValue;
                        taxData.FromDate = tax.FromDate.ToDateTime();
                        taxData.ToDate = tax.ToDate?.ToNullableDateTime();
                        taxData.UpdatedBy = _applicationContext.UserId;
                        taxData.UpdatedOn = DateTime.Now;

                        editbankTaxList.Add(taxData);
                    }
                }

                _repo.EditEntities(editbankTaxList);

                // Update Logic for bank tax end
            }
        }


        /// <summary>
        /// Check the input is valid or not
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private InvoiceBankSaveResponse CheckRequestIsValid(InvoiceBankSaveRequest request)
        {
            var response = new InvoiceBankSaveResponse() { Result = InvoiceBankSaveResult.InvoiceBankRequestIsValid };

            if (request == null)
            {
                response = new InvoiceBankSaveResponse { Result = InvoiceBankSaveResult.InvoiceBankRequestIsNotValid };
                return response;
            }

            return response;
        }

        /// <summary>
        /// Check the bank is already exist
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<InvoiceBankSaveResponse> CheckBankAccountIsNotExist(InvoiceBankSaveRequest request)
        {
            var response = new InvoiceBankSaveResponse() { Result = InvoiceBankSaveResult.InvoiceBankIsNotExist };

            if (!string.IsNullOrEmpty(request.BankName) && await _repo.CheckBankAccountNameIsAlreadyExist(request.BankName.Trim()))
            {
                return new InvoiceBankSaveResponse { Result = InvoiceBankSaveResult.InvoiceBankAccountIsAlreadyExist };
            }

            return response;
        }

        /// <summary>
        /// get tax details
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="maxInspectionDate"></param>
        /// <param name="minInspectionDate"></param>
        /// <returns></returns>
        public async Task<InvoiceBankGetResponse> GetTaxDetails(int bankId, DateTime toDate)
        {
            var bankInfo = await _repo.GetInvoiceBankById(bankId);
            if (bankInfo == null)
            {
                return new InvoiceBankGetResponse()
                {
                    BankDetails = null,
                    BankTaxDetails = null,
                    Result = InvoiceBankGetResult.InvoiceBankNotFound
                };
            }

            var taxes = await _repo.GetTaxDetails(bankId, toDate);
            return new InvoiceBankGetResponse()
            {
                BankDetails = _invoicemap.GetInvoiceBankDetailsMap(bankInfo),
                Result = InvoiceBankGetResult.Success,
                BankTaxDetails = taxes
            };
        }
    }
}
