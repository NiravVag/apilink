using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.MobileApp
{
    public class CustomerDecisionMobileSaveRequest
    {
        [Required]
        public int reportId { get; set; }
        [Required]
        public int resultId { get; set; }
        public string resultComments { get; set; }
        public bool emailFlag { get; set; }
        public int createdBy { get; set; }
        [Required]
        public int bookingId { get; set; }
    }

    public class CustomerDecisionSaveMobileResponse
    {
        public MobileResult meta { get; set; }
        public Result data { get; set; }
    }
}
