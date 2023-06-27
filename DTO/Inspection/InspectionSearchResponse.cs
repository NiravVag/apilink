using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionSearchResponse
    {
        public IEnumerable<InspectionBookingDetails> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public InspectionSearchResponseResult Result { get; set; }
    }

    public enum InspectionSearchResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }
}
