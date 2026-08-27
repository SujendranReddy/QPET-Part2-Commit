namespace QPET.Domain.Entities
{
    public class EnquiryAttachment
    {
        public int EnquiryAttachmentId { get; set; }

        public int EnquiryId { get; set; }

        public string OriginalFileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public Enquiry Enquiry { get; set; } = null!;
    }
}