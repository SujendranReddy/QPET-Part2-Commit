using QPET.Models;

namespace QPET.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Enquiry> Enquiries { get; set; }
            = new List<Enquiry>();

        public ICollection<Review> Reviews { get; set; }
            = new List<Review>();
    }
}