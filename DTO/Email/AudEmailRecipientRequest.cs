using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Email
{
   public class AudEmailRecipientRequest
    {
        public IEnumerable<int> LstAuditIds { get; set; }

        public EmailType EmailtypeId { get; set; }
    }
}
