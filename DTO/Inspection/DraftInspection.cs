using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Inspection
{
    public class DraftInspectionData
    {
        public string BookingInfo { get; set; }
    }

    public class DraftInspectionResponse
    {
        public List<DraftInspection> InspectionData { get; set; }
        public DraftInspectionResult Result { get; set; }
    }

    public enum DraftInspectionResult
    {
        Success = 1,
        NotFound = 2
    }

    public class DeleteDraftInspectionResponse
    {
        public List<DraftInspection> InspectionData { get; set; }
        public DeleteDraftInspectionResult Result { get; set; }
    }

    public enum DeleteDraftInspectionResult
    {
        DeleteSuccess = 1,
        Error = 2,
        NotFound = 3
    }

    public class DraftInspection
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public string ServiceDate { get; set; }
        public string CreatedOn { get; set; }
        public string Brand { get; set; }
        public string Department { get; set; }
        public string BookingInfo { get; set; }

        public bool IsReInspectionDraft { get; set; }
        public bool IsReBookingDraft { get; set; }
        public int? PreviousBookingNo { get; set; }
    }

    public class DraftInspectionRepo
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public DateTime? ServiceDateFrom { get; set; }
        public DateTime? ServiceDateTo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Brand { get; set; }
        public string Department { get; set; }
        public string BookingInfo { get; set; }

        public bool? IsReInspectionDraft { get; set; }
        public bool? IsReBookingDraft { get; set; }
        public int? PreviousBookingNo { get; set; }
    }
}
