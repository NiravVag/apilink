using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Core.entities.Emails
{
    public class ApismtpSettings : SmtpSettings { }
    public class SgtsmtpSettings : SmtpSettings { }
    public class AqfsmtpSettings : SmtpSettings { }

    public abstract class SmtpSettings
    {
        public bool SMTPSSL { get; set; }

        public string ServerName { get; set; }

        public string SenderEmail { get; set; }

        public int PortNumber { get; set; }

        public bool Dev { get; set; }

        public string RecepientDEV { get; set; }

        public string AccountName { get; set; }

        public string Password { get; set; }

        public string BCCEmail { get; set; }
    }
}
