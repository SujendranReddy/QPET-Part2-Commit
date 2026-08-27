namespace QPET.Domain.Entities
{
    public class Enquiry
    {
        public int EnquiryId { get; set; }

        public int CustomerId { get; set; }

        public int BranchId { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public EnquiryStatus Status { get; set; } = EnquiryStatus.New;

        public Customer Customer { get; set; } = null!;

        public Branch Branch { get; set; } = null!;

        public ICollection<EnquiryAttachment> Attachments { get; set; }
            = new List<EnquiryAttachment>();

        public void AddAttachment(EnquiryAttachment attachment)
        {
            Attachments.Add(attachment);
        }

        public void UpdateStatus(EnquiryStatus status)
        {
            Status = status;
        }
    }
}