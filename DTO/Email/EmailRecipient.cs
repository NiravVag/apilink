using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Email
{
   public class EmailRecipient
    {
        public IEnumerable<EmailContact> LstCusRecipient { get; set; }

        public IEnumerable<EmailContact> LstSupRecipient { get; set; }

        public IEnumerable<EmailContact> LstFactRecipient { get; set; }

        public IEnumerable<EmailContact> LstInternalRecipient { get; set; }

        public IEnumerable<EmailContact> LstInternalManagerRecipient { get; set; }

        public IEnumerable<EmailContact> LstInternalAccountingRecipient { get; set; }

        public IEnumerable<EmailContact> LstDefaultInternalRecipient { get; set; }

        public IEnumerable<EmailContact> LstDefaultCusRecipient { get; set; }

        public EmailRecipientResult Result { get; set; }
    }
    public enum EmailRecipientResult
    {
        success=1,
        error=2
    }
}
