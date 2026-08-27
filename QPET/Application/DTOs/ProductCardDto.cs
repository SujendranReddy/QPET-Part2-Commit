namespace QPET.Application.DTOs
{
    public class ProductCardDto
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string NeckFinish { get; set; } = string.Empty;

        public string CapacityVolume { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}