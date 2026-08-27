using QPET.Models;

namespace QPET.Domain.Entities
{
    public class Enquiry
    {
        public int EnquiryId { get; set; }

        public int CustomerId { get; set; }

        public int BranchId { get; set; }

        public int EnquiryStatusId { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public Customer Customer { get; set; } = null!;

        public Branch Branch { get; set; } = null!;

        public EnquiryStatus EnquiryStatus { get; set; } = null!;

        public ICollection<EnquiryAttachment> Attachments { get; set; }
            = new List<EnquiryAttachment>();
    }
}