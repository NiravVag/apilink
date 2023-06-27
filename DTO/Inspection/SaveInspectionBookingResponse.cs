using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.User;

namespace DTO.Inspection
{
    public class SaveInspectionBookingResponse
    {
        public int Id { get; set; }
        public SaveInspectionBookingResult Result { get; set; }
        public Guid TaskId { get; set; }
        public bool isCombineOrderDataChanged { get; set; }
        public bool IsTechincalDocumentsAddedOrRemoved { get; set; }
        public bool IsMissionCreated { get; set; }
        public bool IsTechnicalDoucmentSync { get; set; }

    }

    public enum SaveInspectionBookingResult
    {
        Success = 1,
        InspectionBookingIsNotSaved = 2,
        InspectionBookingIsNotFound = 3,
        InspectionBookingExists = 4,
        RequestIncorrect = 5,
        UnAuthorized = 6,
        InspectionBookingQuantityZero = 7,
        BookingSavedNotificationError = 8,
        BookingProductsNotAvailable = 9,
        BookingDatesIncorrect = 10,
        CombineInformationNotFound = 11,
        PickingInformationNotFound = 12,
        FactoryCountyTownNotFound = 13
    }
}
