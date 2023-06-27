using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingFileAttachment
    {
        public int Id { get; set; }

        public string uniqueld { get; set; }

        public string FileName { get; set; }

        public string FileDescription { get; set; }

        public string FileUrl { get; set; }

        public bool IsNew { get; set; }

        public string MimeType { get; set; }
        public int BookingId { get; set; }

        public int? FileAttachmentCategoryId { get; set; }

        public bool? IsBookingEmailNotification { get; set; }
        public bool? IsReportSendToFB { get; set; }
    }

    public class InspectionGAPFileAttachment
    {
        public int Id { get; set; }

        public string uniqueld { get; set; }

        public string FileName { get; set; }

        public string FileDescription { get; set; }

        public string FileUrl { get; set; }

        public bool IsNew { get; set; }

        public string MimeType { get; set; }
        public int InspectionId { get; set; }

        public int DocumentTypeId { get; set; }

    }

    public class ProductFileAttachment
    {
        public int Id { get; set; }

        public string uniqueld { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public int ProductId { get; set; }

        public bool IsNew { get; set; }

        public string MimeType { get; set; }
    }

    public class ProductFileAttachmentRepsonse
    {
        public int Id { get; set; }

        public string uniqueld { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public int ProductId { get; set; }

        public bool IsNew { get; set; }

        public string MimeType { get; set; }
    }

    public class FileAttachmentToZipRequest
    {
        public int InspectionId { get; set; }

        public int EntityId { get; set; }
    }

}

    

   
