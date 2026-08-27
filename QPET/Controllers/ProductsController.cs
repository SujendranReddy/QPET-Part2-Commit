using Microsoft.AspNetCore.Mvc;
using QPET.Application.Interfaces;
using QPET.Models;

namespace QPET.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(
            IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(
            int? categoryId = null,
            int? subCategoryId = null)
        {
            var model =
                new ProductCatalogueViewModel
                {
                    Products =
                        await _productService
                            .GetPublicProductsAsync(
                                categoryId,
                                subCategoryId),
                    Categories =
                        await _productService
                            .GetCategoriesAsync(),

                    SubCategories =
                        await _productService
                            .GetSubCategoriesAsync()
                };

            return View(model);
        }
    }
}