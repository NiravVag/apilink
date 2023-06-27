using System.Collections.Generic;
using DTO.References;
namespace DTO.CancelBooking
{
    public class CurrencyResponse
    {
        public IEnumerable<Currency> CurrencyList { get; set; }
        public CurrencyResult Result { get; set; }
    }
    public enum CurrencyResult
    {
        Success = 1,
        CurrencyNotFound = 2
    }
}
