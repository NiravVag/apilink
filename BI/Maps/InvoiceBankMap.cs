using DTO.Common;
using DTO.Invoice;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BI.Maps
{
    public  class InvoiceBankMap: ApiCommonData
    {

        public  InvRefBank GetInvoiceBankSaveMap(InvoiceBankSaveRequest model, int userId,int _entityId)
        {
            return new InvRefBank()
            {
                CreatedOn = DateTime.Now,
                CreatedBy = userId,
                Id = model.Id,
                AccountName = model.AccountName,
                AccountCurrency = model.AccountCurrency,
                BillingEntity = model.BillingEntity,
                AccountNumber = model.AccountNumber,
                BankAddress = model.BankAddress,
                BankName = model.BankName,
                ChopFilename = model.ChopFileName,
                ChopFileUniqueId = model.ChopFileUniqueId,
                ChopFileUrl = model.ChopFileUrl,
                Active = true,
                Remarks = model.Remarks,
                SignatureFilename = model.SignatureFileName,
                SignatureFileUniqueId = model.SignatureFileUniqueId,
                SignatureFileUrl = model.SignatureFileUrl,
                SwiftCode = model.SwiftCode,
                EntityId= _entityId
            };
        }

        public  void GetInvoiceBankUpdateMap(InvoiceBankSaveRequest request, InvRefBank bankEntity)
        {
            bankEntity.AccountName = request.AccountName;
            bankEntity.AccountCurrency = request.AccountCurrency;
            bankEntity.BillingEntity = request.BillingEntity;
            bankEntity.AccountNumber = request.AccountNumber;
            bankEntity.BankAddress = request.BankAddress;
            bankEntity.BankName = request.BankName;
            bankEntity.ChopFilename = request.ChopFileName;
            bankEntity.ChopFileUniqueId = request.ChopFileUniqueId;
            bankEntity.ChopFileUrl = request.ChopFileUrl;
            bankEntity.Remarks = request.Remarks;
            bankEntity.SignatureFilename = request.SignatureFileName;
            bankEntity.SignatureFileUniqueId = request.SignatureFileUniqueId;
            bankEntity.SignatureFileUrl = request.SignatureFileUrl;
            bankEntity.SwiftCode = request.SwiftCode;
            bankEntity.UpdatedOn = DateTime.Now;
        }

        public  InvTranBankTax GetInvoiceBankTaxSaveMap(InvoiceBankTax model, int userId)
        {
            return new InvTranBankTax()
            {
                CreatedOn = DateTime.Now,
                CreatedBy = userId,
                Active = true,
                Id = model.Id,
                TaxName = model.TaxName,
                TaxValue = model.TaxValue,
                FromDate = model.FromDate.ToDateTime(),
                ToDate = model.ToDate?.ToNullableDateTime()
            };
        }

        public  InvoiceBank GetInvoiceBankDetailsMap(InvRefBank entity)
        {
            return new InvoiceBank()
            {
                Id = entity.Id,
                AccountName = entity.AccountName,
                AccountCurrency = entity.AccountCurrency,
                BillingEntity = entity.BillingEntity,
                AccountNumber = entity.AccountNumber,
                BankAddress = entity.BankAddress,
                BankName = entity.BankName,
                ChopFilename = entity.ChopFilename,
                ChopFileUniqueId = entity.ChopFileUniqueId,
                ChopFileUrl = entity.ChopFileUrl,
                Remarks = entity.Remarks,
                SignatureFilename = entity.SignatureFilename,
                SignatureFileUniqueId = entity.SignatureFileUniqueId,
                SignatureFileUrl = entity.SignatureFileUrl,
                SwiftCode = entity.SwiftCode
            };
        }

        public  InvoiceBankTax GetInvoiceBankTaxDetailsMap(InvTranBankTax model)
        {
            return new InvoiceBankTax()
            {
                Id = model.Id,
                TaxName = model.TaxName,
                TaxValue = model.TaxValue,
                FromDate = Static_Data_Common.GetCustomDate(model.FromDate),
                ToDate = Static_Data_Common.GetCustomDate(model.ToDate)
            };
        }
    }
}
