namespace QPET.Application.DTOs
{
    public class CreateReviewRequest
    {
        public int BranchId { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public int Rating { get; set; }

        public string ReviewMessage { get; set; } = string.Empty;
    }
}