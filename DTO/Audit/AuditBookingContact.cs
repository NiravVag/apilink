using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
   public class AuditBookingContact
    {
        public int Id { get; set; }

        public string PlanningTeamEmailTo { get; set; }

        public string PlanningTeamEMailCC { get; set; }

        public string PenaltyEmail { get; set; }

        public string ContactInformation { get; set; }

        public string OfficeTelNo { get; set; }

        public string OfficeFax { get; set; }

        public string OfficeName { get; set; }

        public string OfficeAddress { get; set; }

        public string OfficeRegionalLanguageAddress { get; set; }


    }
}
