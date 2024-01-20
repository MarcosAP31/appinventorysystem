using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using INVENTARIO.Entity;
using INVENTARIO.Services;
using INVENTARIO.Interfaces;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SampleContext _context;
        public ProductController(ITokenService tokenService, SampleContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var productList = await _context.Product.ToListAsync();

                return Ok(productList);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProductById(int productId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);


                var product = await _context.Product.FindAsync(productId);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("sku/{sku}")]
        public async Task<ActionResult<Product>> GetProductBySKU(string sku)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var product = await _context.Product.FirstOrDefaultAsync(res => res.SKU.Equals(sku));

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutProduct(Product product, string token)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);


                var existingProduct = await _context.Product.FindAsync(product.ProductId);

                if (existingProduct == null)
                {
                    return Problem("No record found");
                }

                // Update client properties
                _context.Entry(existingProduct).CurrentValues.SetValues(product);

                await _context.SaveChangesAsync();

                return Ok(existingProduct);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);
                
                    var existingProduct = await _context.Product.FirstOrDefaultAsync(res => res.Name.Equals(product.Name));

                    if (existingProduct != null)
                    {
                        return Problem("Product with the same name already exists");
                    }

                    _context.Product.Add(product);
                    await _context.SaveChangesAsync();

                    return Ok(product.ProductId);
                
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);
               
                    var existingProduct = await _context.Product.FindAsync(productId);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    _context.Product.Remove(existingProduct);
                    await _context.SaveChangesAsync();

                    return NoContent();
                
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}