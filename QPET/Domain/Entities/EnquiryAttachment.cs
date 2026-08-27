namespace QPET.Domain.Entities
{
    public class EnquiryAttachment
    {
        public int AttachmentId { get; set; }

        public int EnquiryId { get; set; }

        public string OriginalFileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

        public Enquiry Enquiry { get; set; } = null!;

        public string GetFileExtension()
        {
            return Path.GetExtension(OriginalFileName);
        }
    }
}