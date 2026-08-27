using QPET.Models;

namespace QPET.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public int SubCategoryId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string NeckFinish { get; set; } = string.Empty;

        public string CapacityVolume { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public SubCategory SubCategory { get; set; } = null!;

        public ICollection<ProductImage> Images { get; set; }
            = new List<ProductImage>();
    }
}