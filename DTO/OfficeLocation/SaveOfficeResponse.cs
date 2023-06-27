namespace DTO.OfficeLocation
{
    public class SaveOfficeResponse
    {
        public int OfficeId { get; set; }
        public SaveOfficeResult Result { get; set; }
    }
    public enum SaveOfficeResult
    {
        Success = 1,
        CannotSaveOffice = 2,
        CurrentOfficeNotFound = 3,
        CannotMapRequestToEntites = 4
    }
}
