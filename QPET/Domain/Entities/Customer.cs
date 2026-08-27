namespace QPET.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<Enquiry> Enquiries { get; set; }
            = new List<Enquiry>();

        public void UpdateContactDetails(
            string fullName,
            string phoneNumber)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }
    }
}