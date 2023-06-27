using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class EditCustomerServiceConfigData
    {
        public int Id { get; set; }
        public int Service { get; set; }
        public int ServiceType { get; set; }
        public int? PickType { get; set; }
        public int? LevelPick1 { get; set; }
        public int? LevelPick2 { get; set; }
        public int? CriticalPick1 { get; set; }
        public int? CriticalPick2 { get; set; }
        public int? MajorTolerancePick1 { get; set; }
        public int? MajorTolerancePick2 { get; set; }
        public int? MinorTolerancePick1 { get; set; }
        public int? MinorTolerancePick2 { get; set; }
        public bool? AllowAQLModification { get; set; }
        public bool? IgnoreAcceptanceLevel { get; set; }
        public int? DefectClassification { get; set; }
        public bool? CheckMeasurementPoints { get; set; }
        public int? ReportUnit { get; set; }
        public int? ProductCategory { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool Active { get; set; }
        public string CustomServiceTypeName { get; set; }
        public double? CustomerRequirementIndex { get; set; }
        public int? DpPoint { get; set; }
    }
}


