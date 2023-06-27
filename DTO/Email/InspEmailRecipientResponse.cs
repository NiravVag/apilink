﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Email
{
   public class InspEmailRecipientResponse
    {

        public EmailRecipient EmailDetails { get; set; }

        public IEnumerable<EmailContact> CusBookingContact { get; set; }

        public IEnumerable<EmailContact> SupBookingContact { get; set; }

        public IEnumerable<EmailContact> FactBookingContact { get; set; }

        public InspEmailRecipientResponseResult Result { get; set; }
    }
    public enum InspEmailRecipientResponseResult
    {
        success = 1,
        NoBookingFound = 2,
        NoEmailRuleFound = 3,
        MultipleEmailRuleFound = 3
    }
}
