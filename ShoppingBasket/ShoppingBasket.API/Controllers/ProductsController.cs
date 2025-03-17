using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;

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
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        if (product == null || string.IsNullOrEmpty(product.Name) || product.Price <= 0)
        {
            return BadRequest(new { errorCode = "VALIDATION_ERROR", message = "Invalid product data." });
        }

        await _productRepository.AddAsync(product);
        return Ok(product);
    }
}