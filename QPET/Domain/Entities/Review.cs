namespace QPET.Domain.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int BranchId { get; set; }

        public int? CustomerId { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public int Rating { get; set; }

        public string ReviewMessage { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Branch Branch { get; set; } = null!;

        public Customer? Customer { get; set; }
    }
}