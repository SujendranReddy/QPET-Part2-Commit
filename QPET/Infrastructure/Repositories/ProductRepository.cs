using Microsoft.EntityFrameworkCore;
using QPET.Application.Interfaces;
using QPET.Data;
using QPET.Domain.Entities;

namespace QPET.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync(
            bool includeInactive = false)
        {
            var query = BuildProductQuery();

            if (!includeInactive)
            {
                query =
                    query.Where(product => product.IsActive);
            }

            return await query
                .OrderBy(product => product.ProductName)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int productId)
        {
            return await _context.Products
                .Include(product => product.Category)
                .Include(product => product.SubCategory)
                .Include(product => product.ProductImages)
                .FirstOrDefaultAsync(
                    product => product.ProductId == productId);
        }

        public async Task<List<Product>> FilterAsync(
            int? categoryId,
            int? subCategoryId)
        {
            var query =
                BuildProductQuery()
                    .Where(product => product.IsActive);

            if (categoryId.HasValue)
            {
                query =
                    query.Where(
                        product =>
                            product.CategoryId ==
                            categoryId.Value);
            }

            if (subCategoryId.HasValue)
            {
                query =
                    query.Where(
                        product =>
                            product.SubCategoryId ==
                            subCategoryId.Value);
            }

            return await query
                .OrderBy(product => product.ProductName)
                .ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(category => category.CategoryName)
                .ToListAsync();
        }

        public async Task<List<SubCategory>>
            GetSubCategoriesAsync(
                int? categoryId = null)
        {
            var query =
                _context.SubCategories
                    .AsNoTracking()
                    .AsQueryable();

            if (categoryId.HasValue)
            {
                query =
                    query.Where(
                        subCategory =>
                            subCategory.CategoryId ==
                            categoryId.Value);
            }

            return await query
                .OrderBy(
                    subCategory =>
                        subCategory.SubCategoryName)
                .ToListAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            var product =
                await _context.Products
                    .Include(
                        item =>
                            item.ProductImages)
                    .FirstOrDefaultAsync(
                        item =>
                            item.ProductId ==
                            productId);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        private IQueryable<Product> BuildProductQuery()
        {
            return _context.Products
                .AsNoTracking()
                .Include(product => product.Category)
                .Include(product => product.SubCategory)
                .Include(product => product.ProductImages);
        }
    }
}