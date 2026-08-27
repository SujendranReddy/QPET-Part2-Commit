namespace QPET.Application.DTOs
{
    public class CreateEnquiryRequest
    {
        public int BranchId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public IEnumerable<UploadedFile> Attachments { get; set; }
            = Enumerable.Empty<UploadedFile>();
    }
}