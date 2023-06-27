namespace DTO.EmailSend
{
    public class InvoiceEmailAttachment
    {
        public int? InvoiceId { get; set; }
        public string FileName { get; set; }
        public int? FileType { get; set; }
        public string UniqueId { get; set; }
        public string FilePath { get; set; }
        public int UserId { get; set; }
    }
}
