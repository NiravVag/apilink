using DTO.ExtraFees;
using DTO.Invoice;
using DTO.InvoicePreview;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IInvoicePreivewRepository
    {
        /// <summary>
        /// get bank details with tax
        /// </summary>
        /// <returns></returns>
        Task<List<InvoiceBankRepo>> GetBankDetails();

        /// <summary>
        /// get invoice details by invoice number
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        Task<List<InvoiceDetailsRepo>> GetInvoiceDetails(string invoiceNumber);

        /// <summary>
        /// get booking details by booking id list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<InvoiceBookingPDFDetail>> GetInvoiceBookingData(IEnumerable<int> bookingIds);
        /// <summary>
        /// get booking details by booking id list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<InvoiceBookingProductsData>> GetBookingProductData(IEnumerable<int> bookingIds);
        /// <summary>
        /// get billed contacts by invoice ids
        /// </summary>
        /// <param name="invoiceIds"></param>
        /// <returns></returns>
        Task<List<BilledContactsName>> GetInvoiceBilledContacts(IEnumerable<int> invoiceIds);

        Task<List<ExtraFeeData>> GetExtraFeeByBooking(List<int> bookingIds);

        Task<List<ExtraFeeTypeData>> GetExtraFeeTypeByBooking(IEnumerable<int> extraFeeIds);

        Task<List<InvoiceBookingPDFDetail>> GetInvoiceAuditBookingData(IEnumerable<int> auditIds);

        Task<List<ExtraFeeData>> GetExtraFeeByAuditBooking(List<int> bookingIds);

        Task<List<InvoiceDetailsRepo>> GetExtraFeeInvoiceDetails(string invoiceNumber);

        Task<List<BilledContactsName>> GetExtraFeeBilledContacts(IEnumerable<int> ExtraFeeIds);

        Task<List<BookingQuantity>> GetInspectionQuantities(List<int> bookingIds);
        Task<List<InvoiceBankTaxRepo>> GetBankTaxDetails(List<int> BankIdList);

        Task<List<InvoiceTaxData>> GetInvoiceTaxDetails(List<int> InvoiceIds);

        Task<List<ExtraFeesTaxData>> GetExtraFeesBankTaxList(List<int> extraFeeIds);
    }
}
