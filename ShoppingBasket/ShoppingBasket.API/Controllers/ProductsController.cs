using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using ShoppingBasket.Core.DTOs;
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productRepository.GetAllAsync();
        // Map Product entities to ProductDTO
        var productDTOs = products.Select(p => new ProductDTO
        {
            Name = p.Name,
            Price = p.Price
        }).ToList();

        return Ok(productDTOs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Map Product entity to ProductDTO
        var productDTO = new ProductDTO
        {
            Name = product.Name,
            Price = product.Price
        };

        return Ok(productDTO);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductDTO productDTO)
    {
        if (productDTO == null || !ModelState.IsValid)
        {
            return BadRequest(new { errorCode = "VALIDATION_ERROR", message = "Invalid product data." });
        }

        // Map ProductDTO to Product entity
        var product = new Product
        {
            Name = productDTO.Name,
            Price = productDTO.Price
        };

        await _productRepository.AddAsync(product);

        // Map the saved Product entity back to ProductDTO for the response
        var responseDTO = new ProductDTO
        {
            Name = product.Name,
            Price = product.Price
        };

        return Ok(responseDTO);
    }
}