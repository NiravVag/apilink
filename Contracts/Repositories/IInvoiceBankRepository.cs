using DTO.Invoice;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IInvoiceBankRepository : IRepository
    {
        Task<bool> CheckBankAccountNameIsAlreadyExist(string accountNumber);
        Task<IEnumerable<InvoiceBank>> GetInvoiceBankList();
        Task<InvRefBank> GetInvoiceBankById(int bankId);

        Task<IEnumerable<InvoiceBankTax>> GetTaxDetails(int bankId, DateTime toDate);
    }
}
