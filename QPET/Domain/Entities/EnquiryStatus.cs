using QPET.Models;

namespace QPET.Domain.Entities
{
    public class EnquiryStatus
    {
        public int EnquiryStatusId { get; set; }

        public string StatusName { get; set; } = string.Empty;

        public ICollection<Enquiry> Enquiries { get; set; }
            = new List<Enquiry>();
    }
}