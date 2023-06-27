using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Email
{
   public class DeafultEmailRecipientResponse
    {
        public IEnumerable<EmailContact> DefaultRecipients { get; set; }

        public DeafultEmailRecipientResponseResult Result { get; set; }
    }

    public enum DeafultEmailRecipientResponseResult
    {
        Success=1,
        NoData=2,
        NoEmail=3
    }
}
