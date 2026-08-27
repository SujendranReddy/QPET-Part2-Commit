using QPET.Domain.Entities;

namespace QPET.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(bool includeInactive = false);

        Task<Product?> GetByIdAsync(int productId);

        Task<List<Product>> FilterAsync(
            int? categoryId,
            int? subCategoryId);

        Task<List<Category>> GetCategoriesAsync();

        Task<List<SubCategory>> GetSubCategoriesAsync(
            int? categoryId = null);

        Task<Product> AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task<bool> DeleteAsync(int productId);
    }
}