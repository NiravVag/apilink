using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{

    public class SaveQuotationRequest
    {
        public QuotationDetails Model { get; set;  }

        public Action<SendEmailRequest> OnSendEmail { get; set;  }

        public string Url { get; set;  }
        public bool isCallFromEAQF { get; set; }
        public IEnumerable<FactoryBookingInfo> FactoryBookingInfoList { get; set; }
    }


    public class SaveQuotationResponse
    {
        public QuotationDetails Item { get; set;  }

        public SaveQuotationResult Result { get; set;  }

        public BookingDateChangeInfo ServiceDateChangeInfo { get; set; }

        public IEnumerable<int> BookingOrAuditNos { get; set; }
    }

    public enum SaveQuotationResult
    {
        Success = 1,
        CannotSave = 2,
        NotFound = 3,
        NoAccess = 4,
        CustomerNotFound = 5,
        SuccessWithBrodcastError = 6,
        ServiceDateChanged = 7,
        QuotationExists = 8,
        BookingIsCancelled = 9,
        BookingIsHold = 10,
        SkipSentToClientAndIsForwardToSelected = 11
    }

    public enum PriceCardTravelResult
    {
        Success = 1,
        NotFound = 2
    }

    public class PriceCardTravelResponse
    {
        public PriceCardTravelResult Result { get; set; }
        public bool IsTravelInclude { get; set; }
    }

}
