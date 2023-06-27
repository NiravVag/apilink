using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingPoProductResponse
    {
        public IEnumerable<BookingPoProduct> Data { get; set; }
        public BookingPoProductResult Result { get; set; }

        public enum BookingPoProductResult
        {
            Success = 1,
            NotFound = 2
        }
    }
}
