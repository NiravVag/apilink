using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Inspection
{
    public class BookingFileZipResponse
    {
        public BookingFileZipAttachment FileAttachment { get; set; }
        public BookingFileZipResult Result { get; set; }
    }

    public class BookingFileZipAttachment
    {
        public int? InspectionId { get; set; }
        public string ZipFileUrl { get; set; }
        public string ZipFileName { get; set; }
    }

    public enum BookingFileZipResult
    {
        Success = 1,
        NotFound = 2
    }
}
