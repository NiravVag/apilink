
namespace DTO.Customer
{
    public enum CheckPointResult
    {
        Success = 1,
        CannotGet = 2
    }
    public enum CheckPointGetResult
    {
        Success = 1,
        CannotGetCheckPointList = 2,
        NotFound = 3,
        CheckPointListNoData = 4
    }
    public enum CPSaveResult
    {
        Success = 1,
        IsNotSaved = 2,
        CannotMapRequestToEntites=3,
        CurrentCustomerCPNotFound=4,
        RecordExists=5
    }
    public enum CustomerCPDeleteResult
    {
        Success = 1,
        NotFound = 2
    }
}
