using QPET.Application.DTOs;
using QPET.Application.Interfaces;
using QPET.Domain.Entities;

namespace QPET.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileStorageService _fileStorageService;

        public ProductService(
            IProductRepository productRepository,
            IFileStorageService fileStorageService)
        {
            _productRepository = productRepository;
            _fileStorageService = fileStorageService;
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _productRepository.GetAllAsync(
                includeInactive: true);
        }

        public async Task<List<ProductCardDto>>
            GetPublicProductsAsync(
                int? categoryId,
                int? subCategoryId)
        {
            var products =
                await _productRepository.FilterAsync(
                    categoryId,
                    subCategoryId);

            return products
                .Select(MapToPublicDto)
                .ToList();
        }

        public Task<Product?> GetByIdAsync(int productId)
        {
            return _productRepository.GetByIdAsync(productId);
        }

        public Task<List<Category>> GetCategoriesAsync()
        {
            return _productRepository.GetCategoriesAsync();
        }

        public Task<List<SubCategory>> GetSubCategoriesAsync(
            int? categoryId = null)
        {
            return _productRepository
                .GetSubCategoriesAsync(categoryId);
        }

        public async Task<int> CreateAsync(
            SaveProductRequest request)
        {
            await ValidateSelectionAsync(request);

            var uploadedFiles =
                request.Images.ToList();

            ValidateProductImages(uploadedFiles);

            var savedPaths =
                new List<string>();

            try
            {
                var product =
                    new Product
                    {
                        ProductName = request.ProductName.Trim(),
                        CategoryId = request.CategoryId,
                        SubCategoryId = request.SubCategoryId,
                        NeckFinish = request.NeckFinish.Trim(),
                        CapacityVolume =
                            request.CapacityVolume.Trim(),
                        Description = request.Description.Trim(),
                        IsActive = request.IsActive
                    };

                for (var index = 0;
                     index < uploadedFiles.Count;
                     index++)
                {
                    var imagePath =
                        await _fileStorageService.SaveAsync(
                            uploadedFiles[index],
                            "products");

                    savedPaths.Add(imagePath);

                    product.ProductImages.Add(
                        new ProductImage
                        {
                            ImageUrl = imagePath,
                            IsPrimary = index == 0
                        });
                }

                var createdProduct =
                    await _productRepository.AddAsync(product);

                return createdProduct.ProductId;
            }
            catch
            {
                foreach (var path in savedPaths)
                {
                    await _fileStorageService.DeleteAsync(path);
                }

                throw;
            }
        }

        public async Task<bool> UpdateAsync(
            SaveProductRequest request)
        {
            if (!request.ProductId.HasValue)
            {
                return false;
            }

            var product =
                await _productRepository.GetByIdAsync(
                    request.ProductId.Value);

            if (product == null)
            {
                return false;
            }

            await ValidateSelectionAsync(request);

            var uploadedFiles =
                request.Images.ToList();

            ValidateProductImages(uploadedFiles);

            var newPaths =
                new List<string>();

            var oldPaths =
                product.ProductImages
                    .Select(image => image.ImageUrl)
                    .ToList();

            try
            {
                product.ProductName =
                    request.ProductName.Trim();

                product.CategoryId =
                    request.CategoryId;

                product.SubCategoryId =
                    request.SubCategoryId;

                product.NeckFinish =
                    request.NeckFinish.Trim();

                product.CapacityVolume =
                    request.CapacityVolume.Trim();

                product.Description =
                    request.Description.Trim();

                product.IsActive =
                    request.IsActive;

                if (uploadedFiles.Count > 0)
                {
                    product.ProductImages.Clear();

                    for (var index = 0;
                         index < uploadedFiles.Count;
                         index++)
                    {
                        var imagePath =
                            await _fileStorageService.SaveAsync(
                                uploadedFiles[index],
                                "products");

                        newPaths.Add(imagePath);

                        product.ProductImages.Add(
                            new ProductImage
                            {
                                ImageUrl = imagePath,
                                IsPrimary = index == 0
                            });
                    }
                }

                await _productRepository.UpdateAsync(product);

                if (uploadedFiles.Count > 0)
                {
                    foreach (var path in oldPaths)
                    {
                        await _fileStorageService.DeleteAsync(path);
                    }
                }

                return true;
            }
            catch
            {
                foreach (var path in newPaths)
                {
                    await _fileStorageService.DeleteAsync(path);
                }

                throw;
            }
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            var product =
                await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return false;
            }

            var imagePaths =
                product.ProductImages
                    .Select(image => image.ImageUrl)
                    .ToList();

            var deleted =
                await _productRepository.DeleteAsync(productId);

            if (!deleted)
            {
                return false;
            }

            foreach (var path in imagePaths)
            {
                await _fileStorageService.DeleteAsync(path);
            }

            return true;
        }

        private async Task ValidateSelectionAsync(
            SaveProductRequest request)
        {
            var categories =
                await _productRepository.GetCategoriesAsync();

            if (!categories.Any(
                    category =>
                        category.CategoryId ==
                        request.CategoryId))
            {
                throw new ArgumentException(
                    "The selected category is invalid.");
            }

            var subCategories =
                await _productRepository.GetSubCategoriesAsync(
                    request.CategoryId);

            if (!subCategories.Any(
                    subCategory =>
                        subCategory.SubCategoryId ==
                        request.SubCategoryId))
            {
                throw new ArgumentException(
                    "The selected subcategory is invalid.");
            }
        }

        private void ValidateProductImages(
            IEnumerable<UploadedFile> images)
        {
            foreach (var image in images)
            {
                if (!image.ContentType.StartsWith(
                        "image/",
                        StringComparison.OrdinalIgnoreCase) ||
                    !_fileStorageService.Validate(image))
                {
                    throw new ArgumentException(
                        "One or more product images are invalid.");
                }
            }
        }

        private static ProductCardDto MapToPublicDto(Product product)
        {
            var primaryImage =
                product.ProductImages
                    .FirstOrDefault(image => image.IsPrimary)
                ?? product.ProductImages.FirstOrDefault();

            return new ProductCardDto
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                ProductName = product.ProductName,
                NeckFinish = product.NeckFinish,
                CapacityVolume = product.CapacityVolume,
                Description = product.Description,
                ImageUrl =
                    primaryImage?.ImageUrl
                    ?? "/images/products/product-placeholder.jpg"
            };
        }
    }
}