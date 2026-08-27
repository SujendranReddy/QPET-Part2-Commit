namespace QPET.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string NeckFinish { get; set; } = string.Empty;

        public string CapacityVolume { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public Category Category { get; set; } = null!;

        public SubCategory SubCategory { get; set; } = null!;

        public ICollection<ProductImage> ProductImages { get; set; }
            = new List<ProductImage>();
    }
}