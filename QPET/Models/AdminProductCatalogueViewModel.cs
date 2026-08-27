namespace QPET.Models
{
    public class AdminProductCatalogueViewModel
    {
        public List<QPET.Domain.Entities.Product> Products
        { get; set; }
            = new List<QPET.Domain.Entities.Product>();

        public List<QPET.Domain.Entities.Category> Categories
        { get; set; }
            = new List<QPET.Domain.Entities.Category>();

        public List<QPET.Domain.Entities.SubCategory> SubCategories
        { get; set; }
            = new List<QPET.Domain.Entities.SubCategory>();
    }
}