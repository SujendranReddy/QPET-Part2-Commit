namespace QPET.Domain.Entities
{
    public class Branch
    {
        public int BranchId { get; set; }

        public string BranchName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string StreetAddress { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string? GoogleMapLink { get; set; }

        public ICollection<Enquiry> Enquiries { get; set; }
            = new List<Enquiry>();

        public ICollection<Review> Reviews { get; set; }
            = new List<Review>();

        public string GetFullAddress()
        {
            return $"{StreetAddress}, {City}, {Province}, {PostalCode}";
        }
    }
}