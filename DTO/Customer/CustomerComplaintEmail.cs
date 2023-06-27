
using DTO.Common;
using DTO.CommonClass;
using System;
using System.Collections.Generic;

namespace DTO.Customer
{
    public class CustomerComplaintEmailTemplate
    {
        public int ComplaintId { get; set; }
        public int StaffId { get; set; }
        public string Staffname { get; set; }
        public string StaffEmailID { get; set; }
        public string CurrentUserEmailID { get; set; }
        public string Customer { get; set; }
        public int BookingNo { get; set; }
        public int Service { get; set; }
        public string ServiceName { get; set; }
        public string ComplaintDate { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string SenderEmail { get; set; }
    }
    public class CustomerComplaintEmailRepo
    {
        public string Customer { get; set; }
        public int serviceId { get; set; }
        public string serviceName{ get; set; }
        public int BookingNo { get; set; }
        public int AuditNo { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public string Department { get; set; }
    }
    public class  ComplaintEmailUserRepo
    {
        public int userId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }

}
