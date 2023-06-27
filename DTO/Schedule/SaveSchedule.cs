using System.Collections.Generic;

namespace DTO.Schedule
{
    public class SaveScheduleResponse
    {
        public int BookingStatus { get; set; }
        public string[] StatusMessageList { get; set; }
        public SaveScheduleResponseResult Result { get; set; }
    }

    public enum SaveScheduleResponseResult
    {
        Success = 1,
        SaveUnsuccessful = 2,
        SaveFBDataFailure = 3,
        BookingProcessAlready = 4,
        ReportProcessedAlready = 5,
        NotFound = 6
    }
}
