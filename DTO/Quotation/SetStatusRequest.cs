using DTO.Common;
using DTO.File;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class SetStatusRequest
    {

        public int Id { get; set;  }

        public int StatusId { get; set;  }

        public string CusComment { get; set;  }
        
        public string ApiRemark { get; set;  }

        public string ApiInternalRemark { get; set; }

        public DateObject ConfirmDate { get; set; }
        public DateTime ValidatedOn { get; set; }
        public int ValidatedById { get; set; }
    }


    public class SendEmailRequest
    {
        public string Subject { get; set;  }
            
        public IEnumerable<string> RecepientList { get; set;  }

        public IEnumerable<string> CcList { get; set;  }

        public IEnumerable<FileResponse> FileList { get; set;  }
        public QuotationDetails Model { get; set;  }

        public string RecepitName { get; set;  }
        public string SenderEmail { get; set; }
    }


    
}
