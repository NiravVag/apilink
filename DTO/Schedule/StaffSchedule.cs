using DTO.File;
using System;
using System.Collections.Generic;

namespace DTO.Schedule
{
    public class StaffSchedule
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public string EmailAddress { get; set; }
        public string EmergencyCall { get; set; }
        public string Location { get; set; }
        public int LocationId { get; set; }
        public int ZoneId { get; set; }
        public string BookingsAssigned { get; set; }
        public string BookingsAssignedShow { get; set; }
        public int? EmployeeType { get; set; }
        public int? AssignedBookingCount { get; set; }
        public FileResponse StaffImage { get; set; }
        public bool isLeader { get; set; }
        public int? StartPortId { get; set; }
        public string StartPortName { get; set; }

    }

    public class StaffScheduleRepo
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public string EmailAddress { get; set; }
        public string EmergencyCall { get; set; }
        public string Location { get; set; }
        public int LocationId { get; set; }
        public int? EmployeeType { get; set; }
        public bool isLeader { get; set; }
        public int BookingId { get; set; }
        public DateTime ServiceDate { get; set; }
    }

    public class StaffScheduleQcRepo
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public string EmailAddress { get; set; }
        public string EmergencyCall { get; set; }
        public string Location { get; set; }
        public int LocationId { get; set; }
        public int? EmployeeType { get; set; }
        public bool isLeader { get; set; }
        public int BookingId { get; set; }
        public DateTime ServiceDate { get; set; }
        public int QcType { get; set; }
        public bool IsQcVisibility { get; set; }
    }
}
