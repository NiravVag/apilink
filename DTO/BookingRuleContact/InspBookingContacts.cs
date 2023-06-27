using DTO.Common;
using DTO.HumanResource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DTO.BookingRuleContact
{
    public class InspBookingContacts
    {
        public int Id { get; set; }

        public string FactoryCountry { get; set; }

        public string OfficeTelNo { get; set; }

        public int? UserId { get; set; }

        public bool Default { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string ContactInformation { get; set; }

        public string OfficeFax { get; set; }

        public string OfficeName { get; set; }

        public string OfficeAddress { get; set; }

        public string FullName { get; set; }

        public int? StaffId { get; set; }

        public string StaffName { get; set; }

        public string CompanyEmail { get; set; }

        public string CompanyPhone { get; set; }
    }

    public class BookingContactModel
    {
        public string OfficeAddress { get; set; }
        public string OfficeFax { get; set; }
        public string OfficeName { get; set; }
        public string OfficeTelNo { get; set; }
        public string PlanningEmailTo { get; set; }

    }

}
