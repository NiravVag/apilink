using DTO.File;
using DTO.Quotation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Managers
{
    public interface IQuotationPDF
    {
        /// <summary>
        /// Create PDF Document
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        FileResponse CreateDocument(QuotationDetails model);
    }
}
