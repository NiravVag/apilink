using System.Collections.Generic;

namespace DTO.Schedule
{
    public class SaveScheduleRequest
    {
        public int BookingId { get; set; }
        public IEnumerable<SaveAllocationStaff> AllocationCSQCStaff { get; set; }
        public string Comment { get; set; }
    }

    public class FoodAllowanceData
    {
        public decimal FoodAllowance { get; set; }
        public int? FoodAllowanceCurrency { get; set; }
        public bool IsFoodAllowanceConfigured { get; set; }
    }

    public class TravelAllowanceData
    {
        public double? TravelAllowance { get; set; }
        public int? TravelAllowanceCurrency { get; set; }
        public bool? IsTravelAllowanceConfigured { get; set; }
    }

    public class DuplicateTravelAllowance
    {
        public int BookingId { get; set; }
        public string QcName { get; set; }
        public string ServiceDate { get; set; }
        public string StartPort { get; set; }
        public string FactoryTown { get; set; }
    }
}
