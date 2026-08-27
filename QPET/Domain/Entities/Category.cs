using QPET.Models;

namespace QPET.Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<SubCategory> SubCategories { get; set; }
            = new List<SubCategory>();
    }
}