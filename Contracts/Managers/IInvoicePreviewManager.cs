using DTO.InvoicePreview;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IInvoicePreviewManager
    {
        /// <summary>
        /// get invoice type
        /// </summary>
        /// <returns></returns>
        IEnumerable<DataCommon> GetInvoiceType();

        /// <summary>
        /// get invoice preview
        /// </summary>
        /// <returns></returns>
        IEnumerable<DataCommon> GetInvoicePreview();

        /// <summary>
        /// get bank details with tax
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<InvoiceBankPreview>> GetBankInfo();

        /// <summary>
        /// get invoice details by invoice number and preview
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="invoicePreview"></param>
        /// <returns></returns>
        Task<IEnumerable<List<DataCommon>>> GetInvoiceDetails(string invoiceNo, int invoicePreview);

        /// <summary>
        /// save invoice pdf 
        /// </summary>
        /// <param name="invoicePdfUrl"></param>
        /// <returns></returns>
        Task<SaveInvoicePdfResponse> SaveInvoicePdfUrl(SaveInvoicePdfUrl invoicePdfUrl);

        Task<InvoiceDownloadResponse> GetInvoicePreviewFile(string invoiceNo, string invoiceFileName);
    }
}
