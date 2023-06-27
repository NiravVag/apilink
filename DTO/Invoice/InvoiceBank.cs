using DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.Invoice
{
    public class InvoiceBank
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string BankAddress { get; set; }
        public int? AccountCurrency { get; set; }
        public int? BillingEntity { get; set; }
        public string AccountCurrencyName { get; set; }
        public string Remarks { get; set; }
        public string ChopFileUniqueId { get; set; }
        public string SignatureFileUniqueId { get; set; }
        public string ChopFilename { get; set; }
        public string SignatureFilename { get; set; }
        public string ChopFileUrl { get; set; }
        public string SignatureFileUrl { get; set; }
    }

    public class InvoiceBankTax
    {
        public int Id { get; set; }
        [Required]
        public string TaxName { get; set; }
        [Required]
        public decimal TaxValue { get; set; }
        [Required]
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
    }

    public class BankTaxData
    {
        public int BankId { get; set; }
        public string TaxName { get; set; }
        public decimal TaxValue { get; set; }
    }

    public class InvoiceBankSaveRequest
    {
        public int Id { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string SwiftCode { get; set; }
        [Required]
        public string BankAddress { get; set; }
        public int? AccountCurrency { get; set; }
        public int? BillingEntity { get; set; }
        public string Remarks { get; set; }
        public string ChopFileUniqueId { get; set; }
        public string SignatureFileUniqueId { get; set; }
        public string ChopFileName { get; set; }
        public string SignatureFileName { get; set; }
        public string ChopFileUrl { get; set; }
        public string SignatureFileUrl { get; set; }
        public IEnumerable<InvoiceBankTax> InvoiceBankTaxList { get; set; }
    }


    public class InvoiceBankSummary
    {
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }

    public class InvoiceBankSaveResponse
    {
        public int Id { get; set; }
        public InvoiceBankSaveResult Result { get; set; }
    }

    public enum InvoiceBankSaveResult
    {
        Success = 1,
        Failure = 2,
        InvoiceBankAccountIsAlreadyExist = 3,
        InvoiceBankIsNotExist = 4,
        InvoiceBankRequestIsNotValid = 5,
        InvoiceBankRequestIsValid = 6
    }

    public class InvoiceBankDeleteResponse
    {
        public int Id { get; set; }

        public InvoiceBankDeleteResult Result { get; set; }
    }
    public enum InvoiceBankDeleteResult
    {
        Success = 1,
        NotFound = 2,
        Failure = 3
    }

    public class InvoiceBankGetResponse
    {
        public InvoiceBank BankDetails { get; set; }
        public IEnumerable<InvoiceBankTax> BankTaxDetails { get; set; }
        public InvoiceBankGetResult Result { get; set; }
    }
    
    public class InvoiceBankGetAllResponse
    {
        public IEnumerable<InvoiceBank> BankDetails { get; set; }
        public InvoiceBankGetAllResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }

    public class InvoiceBankTaxRequest
    {
        [Required]
        public DateObject ToDate { get; set; }
    }

    public enum InvoiceBankGetAllResult
    {
        Success = 1,
        Failure = 2,
        InvoiceBankNotFound = 3
    }

    public enum InvoiceBankGetResult
    {
        Success = 1,
        Failure = 2,
        InvoiceBankNotFound = 3
    }
}
