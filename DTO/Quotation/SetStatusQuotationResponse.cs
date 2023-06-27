using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class SetStatusQuotationResponse
    {

        public QuotationDetails Item { get; set; }

        public Guid TaskId { get; set; }

        public IEnumerable<DTO.User.User> ManagerList { get; set; }

        public IEnumerable<DTO.File.FileResponse> FileList { get; set; }

        public SetStatusQuotationResult Result { get; set; }

        public BookingDateChangeInfo ServiceDateChangeInfo { get; set; }
    }

    public enum SetStatusQuotationResult
    {
        Success = 1,
        CannotUpdateStatus = 2,
        NoAccess = 3,
        StatusNotConfigued = 4,
        QuotationNotFound = 5,
        SuccessButErrorBrodcast = 6,
        ServiceDateChanged = 7,
        BookingStatusNotConfirmed = 8,
        BookingIsCancelled = 9,
        BookingIsHold = 10
    }

    public class QuotationFileResponse
    { 
        public string FilePath { get; set; }
    }
}
