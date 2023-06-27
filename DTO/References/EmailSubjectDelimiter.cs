using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.References
{
    public class EmailSubjectDelimiter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsFile { get; set; }
    }

    public class EmailSubjectDelimiterResponse
    {
        public List<EmailSubjectDelimiter> delimiterList { get; set; }

        public EmailSubjectDelimiterResult Result { get; set; }
    }

    public enum EmailSubjectDelimiterResult
    {
        Success=1,
        CannotGetList = 2
    }
}
