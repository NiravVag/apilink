using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.MobileApp
{
    public class QuotationValidateRequest
    {
        [Required]
        public int quotationId { get; set; }
        [Required]
        public int statusId { get; set; }
        public string comments { get; set; }
    }

    public class QuotationValidateResponse
    {
        public MobileResult meta { get; set; }

        public Result data { get; set; }
    }

    public enum Result
    {
        success = 1,
        fail = 2,
        noemailconfiguration = 3,
        noroleconfiguration = 4
    }
}

