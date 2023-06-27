using DTO.Customer;
using DTO.HumanResource;
using DTO.Location;
using DTO.References;
using DTO.Supplier;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;

namespace DTO.Audit
{
    public class EditCancelRescheduleAuditResponse
    {
        public AuditSaveCancelRescheduleItem ItemDetails { get; set; }

        public AuditCancelRescheduleItem Data { get; set; }

        public CancelAuditResponseResult Result { get; set; }
    }

    public enum CancelAuditResponseResult
    {
        success = 1,
        CannotGetAuditDetails = 2
    }

    public class AuditCancelRescheduleItem
    {
        public int AuditId { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string FactoryName { get; set; }

        public string ServiceType { get; set; }

        public string ServiceDateFrom { get; set; }

        public string ServiceDateTo { get; set; }

        public string PoNumber { get; set; }

        public string ReportNo { get; set; }

        public string Office { get; set; }

        public int StatusId { get; set; }

        public int LeadTime { get; set; }

        public IEnumerable<AuditCancelRescheduleReasons> ReasonTypes { get; set; }

        public IEnumerable<Currency> CurrencyLst { get; set; }

        public IEnumerable<DateObject> HolidayDates { get; set; }

    }

    public class AuditSaveCancelRescheduleItem
    {
        public int AuditId { get; set; }

        public int Optypeid { get; set; }//operation type . cancel or reschedule

        public int? Reasontypeid { get; set; }

        public string Comment { get; set; }

        public string Internalcomment { get; set; }

        public int? Cancelrescheduletimetype { get; set; }

        public double? Travelexpense { get; set; }

        public int? CurrencyId { get; set; }

        public bool Isemailtoaccounting { get; set; }

        public DateObject Servicedatefrom { get; set; }

        public DateObject Servicedateto { get; set; }
    }
}
