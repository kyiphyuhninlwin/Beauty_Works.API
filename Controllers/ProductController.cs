using Beauty_Works.Models.Domain;
using Beauty_Works.Models.DTO.Product;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty_Works.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ISubcategoryRepository subcategoryRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IBrandRepository brandRepository;

        public ProductController(IProductRepository productRepository, ISubcategoryRepository subcategoryRepository,
            IStatusRepository statusRepository, IBrandRepository brandRepository)
        {
            this.productRepository = productRepository;
            this.subcategoryRepository = subcategoryRepository;
            this.statusRepository = statusRepository;
            this.brandRepository = brandRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto request)
        {
            if ((request.SubcategoryID == null) || (request.StatusID == null) || (request.BrandID == null))
            {
                return BadRequest("Subcategory ID or Status ID or Brand ID is required.");
            }

            var subcategory = await subcategoryRepository.GetByID(request.SubcategoryID.Value);
            var status = await statusRepository.GetByID(request.StatusID.Value);
            var brand = await brandRepository.GetByID(request.BrandID.Value);

            // Convert Dto to Domain
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity,
                Desp = request.Desp,
                PublishedDate = request.PublishedDate,
                ExpiredDate = request.ExpiredDate,
                SubcategoryID = request.SubcategoryID,
                Subcategory = subcategory,
                StatusID = request.StatusID,
                Status = status,
                BrandID = request.BrandID,
                Brand = brand
            };

            product = await productRepository.CreateAsync(product);

            // Convert Domain to Dto
            var response = new ProductDto
            {
                ID = product.ID,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Desp = product.Desp,
                PublishedDate = product.PublishedDate,
                ExpiredDate = product.ExpiredDate,
                SubcategoryID = product.SubcategoryID,
                SubcategoryName = product.Subcategory?.Name,
                StatusID = product.StatusID,
                StatusName = product.Status?.Name,
                BrandID = product.BrandID,
                BrandName = product.Brand?.Name
            };

            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productRepository.GetAllAsync();

            // Convert Domain to Dto
            var response = new List<ProductDto>();
            foreach (var product in products)
            {
                response.Add(new ProductDto
                {
                    ID = product.ID,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Desp = product.Desp,
                    PublishedDate = product.PublishedDate,
                    ExpiredDate = product.ExpiredDate,
                    SubcategoryID = product.SubcategoryID,
                    SubcategoryName = product.Subcategory?.Name,
                    StatusID = product.StatusID,
                    StatusName = product.Status?.Name,
                    BrandID = product.BrandID,
                    BrandName = product.Brand?.Name
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{productID:int}")]
        public async Task<IActionResult> GetProductByID([FromRoute] int productID)
        {
            var existingProduct = await productRepository.GetByIdAsync(productID);
            if (existingProduct == null)
            {
                return NotFound();
            }

            //Map Domain to Dto
            var response = new ProductDto
            {
                ID = existingProduct.ID,
                Name = existingProduct.Name,
                Price = existingProduct.Price,
                Quantity = existingProduct.Quantity,
                Desp = existingProduct.Desp,
                PublishedDate = existingProduct.PublishedDate,
                ExpiredDate = existingProduct.ExpiredDate,
                SubcategoryID = existingProduct.SubcategoryID,
                SubcategoryName = existingProduct.Subcategory?.Name,
                StatusID = existingProduct.StatusID,
                StatusName = existingProduct.Status?.Name,
                BrandID = existingProduct.BrandID,
                BrandName = existingProduct.Brand?.Name
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{productID:int}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int productID, UpdateProductRequestDto request)
        {
            if ((request.SubcategoryID == null) || (request.StatusID == null) || (request.BrandID == null))
            {
                return BadRequest("Subcategory ID or Status ID or Brand ID is required.");
            }

            var subcategory = await subcategoryRepository.GetByID(request.SubcategoryID.Value);
            var status = await statusRepository.GetByID(request.StatusID.Value);
            var brand = await brandRepository.GetByID(request.BrandID.Value);

            if ((subcategory == null) || (status == null) || (brand == null))
            {
                return NotFound("Subcategory or status or brand not found.");
            }

            // Convert Dto to Domain
            var product = new Product
            {
                ID = productID,
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity,
                Desp = request.Desp,
                PublishedDate = request.PublishedDate,
                ExpiredDate = request.ExpiredDate,
                SubcategoryID = request.SubcategoryID,
                StatusID = request.StatusID,
                BrandID = request.BrandID
            };

            var updatedProduct = await productRepository.UpdateAsync(product);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            // Convert Domain to Dto
            var response = new ProductDto
            {
                ID = updatedProduct.ID,
                Name = updatedProduct.Name,
                Price = updatedProduct.Price,
                Quantity = updatedProduct.Quantity,
                Desp = updatedProduct.Desp,
                PublishedDate = updatedProduct.PublishedDate,
                ExpiredDate = updatedProduct.ExpiredDate,
                SubcategoryID = updatedProduct.SubcategoryID,
                SubcategoryName = updatedProduct.Subcategory?.Name,
                StatusID = updatedProduct.StatusID,
                StatusName = updatedProduct.Status?.Name,
                BrandID = updatedProduct.BrandID,
                BrandName = updatedProduct.Brand?.Name
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{productID:int}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int productID)
        {
            var deletedProduct = await productRepository.DeleteAsync(productID);

            if (deletedProduct == null)
            {
                return NotFound();
            }

            // Convert Domain to Dto
            var response = new ProductDto
            {
                ID = deletedProduct.ID,
                Name = deletedProduct.Name,
                Price = deletedProduct.Price,
                Quantity = deletedProduct.Quantity,
                Desp = deletedProduct.Desp,
                PublishedDate = deletedProduct.PublishedDate,
                ExpiredDate = deletedProduct.ExpiredDate,
                SubcategoryID = deletedProduct.SubcategoryID,
                SubcategoryName = deletedProduct.Subcategory?.Name,
                StatusID = deletedProduct.StatusID,
                StatusName = deletedProduct.Status?.Name,
                BrandID = deletedProduct.BrandID,
                BrandName = deletedProduct.Brand?.Name
            };

            return Ok(response);
        }
    }
}
