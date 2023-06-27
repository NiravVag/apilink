using DTO.Common;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class SetStatusBusinessRequest
    {
        public int Id { get; set; }

        public QuotationStatus IdStatus { get; set; }

        public string CusComment { get; set; }

        public string ApiRemark { get; set; }

        public Action<SendEmailRequest> OnSendEmail { get; set; }

        public string Url { get; set; }

        public string ApiInternalRemark { get; set; }

        public DateObject ConfirmDate { get; set; }

        public bool IsOneBookingMapped { get; set; }
    }

    public class CancelInvoiceData
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public IEnumerable<DTO.User.User> InvoiceCancelUserEmail { get; set; }
    }
}
