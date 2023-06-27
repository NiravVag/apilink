using Contracts.Repositories;
using DTO.Invoice;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class InvoiceBankRepository : Repository, IInvoiceBankRepository
    {
        public InvoiceBankRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// get Invoice active Bank List
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<InvoiceBank>> GetInvoiceBankList()
        {
            return await _context.InvRefBanks.Where(x => x.Active.HasValue && x.Active.Value)
                .Select(x => new InvoiceBank
                {
                    Id = x.Id,
                    AccountName = x.AccountName,
                    BankName = x.BankName,
                    AccountCurrencyName = x.AccountCurrencyNavigation.CurrencyName,
                    AccountNumber = x.AccountNumber,
                    BankAddress = x.BankAddress,
                    ChopFilename = x.ChopFilename,
                    ChopFileUniqueId = x.ChopFileUniqueId,
                    ChopFileUrl = x.ChopFileUrl,
                    SignatureFilename = x.SignatureFilename,
                    SignatureFileUniqueId = x.SignatureFileUniqueId,
                    SignatureFileUrl = x.SignatureFileUrl,
                    Remarks = x.Remarks,
                    SwiftCode = x.SwiftCode
                }).ToListAsync();
        }

        /// <summary>
        /// get invoice bank details by bank id
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public async Task<InvRefBank> GetInvoiceBankById(int bankId)
        {
            return await _context.InvRefBanks.
                   Include(x => x.InvTranBankTaxes).
                   Where(x => x.Active.HasValue && x.Active.Value && x.Id == bankId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// check account number is exist
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public async Task<bool> CheckBankAccountNameIsAlreadyExist(string accountNumber)
        {
            return await _context.InvRefBanks.
                   Where(x => x.Active.HasValue && x.Active.Value && x.AccountName == accountNumber).CountAsync() > 0;
        }


        /// <summary>
        /// get the bank tax
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="maxInspectionDate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InvoiceBankTax>> GetTaxDetails(int bankId, DateTime toDate)
        {
            return await _context.InvTranBankTaxes.
                 Where(x => x.Active == true && x.BankId == bankId && x.FromDate <= toDate && x.ToDate >= toDate)
                .Select(x => new InvoiceBankTax
                {
                    TaxName = x.TaxName,
                    TaxValue = x.TaxValue,
                    Id = x.Id
                }).ToListAsync();
        }


    }
}
