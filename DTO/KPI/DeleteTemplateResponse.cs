using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.KPI
{
    public class DeleteTemplateResponse
    {
        public int Id { get; set;  }

        public DeleteTemplateResult Result { get; set;  }
    }

    public enum DeleteTemplateResult
    {
        Success = 1, 
        NotFound = 2
    }
}
