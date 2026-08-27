using QPET.Application.DTOs;
using QPET.Domain.Entities;

namespace QPET.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();

        Task<List<ProductCardDto>> GetPublicProductsAsync(
            int? categoryId,
            int? subCategoryId);

        Task<Product?> GetByIdAsync(int productId);

        Task<List<Category>> GetCategoriesAsync();

        Task<List<SubCategory>> GetSubCategoriesAsync(
            int? categoryId = null);

        Task<int> CreateAsync(SaveProductRequest request);

        Task<bool> UpdateAsync(SaveProductRequest request);

        Task<bool> DeleteAsync(int productId);
    }
}