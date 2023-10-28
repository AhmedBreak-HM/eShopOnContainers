using AutoMapper;
using Matgr.Products.Application.Responses;
using Matgr.Products.Core.Entities;
using Matgr.Products.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Matgr.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetListAsync()
        {
            // Fetch products from the repository.
            var products = await _repository.GetAllAsync();

            // Check if the products list is empty.
            if (products == null || !products.Any())
            {
                return NoContent();
            }

            // Map products to their DTO representation.
            var productDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            // Return the mapped products.
            return Ok(productDtos);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetByIdAsync(int id)
        {

            var product = await _repository.GetByIdAsync(id);

            // Ensure the product exists
            if (product == null) return NotFound("Product not found.");

            var productForReturn = _mapper.Map<ProductDto>(product);

            // Validate the mapping result
            if (productForReturn == null) return BadRequest("An error occurred while mapping the product data.");

            return Ok(productForReturn);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> AddAsync(ProductDto dto)
        {
            // Validate input
            if (dto == null) return BadRequest("Product data is required.");

            var product = _mapper.Map<Product>(dto);

            // Validate the mapping result
            if (product == null) return BadRequest("Invalid product data provided.");

            var productForCreate = await _repository.AddAsync(product);

            // Ensure successful creation
            if (productForCreate == null) return StatusCode(500, "An error occurred while adding the product.");

            return Ok(productForCreate);
        }

        [HttpPut]
        public async Task<ActionResult<ProductDto>> UpdateAsync(int id,ProductDto dto)
        {
            // Ensure ID in DTO matches ID in URL
            if (dto.Id != id)
            {
                return BadRequest("The ID in the URL does not match the ID in the provided data.");
            }

            // Fetch product from the repository.
            var productFromRepo = await _repository.GetByIdAsync(id);
            if (productFromRepo == null) return NotFound("Product not found.");

            // Map DTO to entity.
            var product = _mapper.Map<Product>(productFromRepo);
            if (product == null) return BadRequest("Invalid product data provided.");

            // Update product.
            var productForUpdate = await _repository.UpdateAsync(product);
            // Ensure successful creation
            if (productForUpdate == null) return StatusCode(500, "An error occurred while updating the product.");

            // Map updated entity back to DTO for return.
            var productDtoForReturn = _mapper.Map<ProductDto>(productForUpdate);

            return Ok(productDtoForReturn);

        }
        [HttpDelete]
        public async Task<ActionResult<ProductDto>> DeleteAsync(int id)
        {
            // Fetch product from the repository.
            var product = await _repository.GetByIdAsync(id);

            // Ensure the product exists
            if (product == null) return NotFound("Product not found.");

            // Delete product
            var productDeleted = await _repository.DeleteAsync(product);

            // Ensure successful deletion
            if (productDeleted == null) return StatusCode(500, "An error occurred while delete the product.");

            // Map deletion entity back to DTO for return.
            var productForReturn = _mapper.Map<ProductDto>(productDeleted);

            return Ok(productForReturn);

        }
    }
}
