using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using INVENTARIO.Entity;
using INVENTARIO.Services;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly cifrado _cifrado;
        private readonly string _defaultConnection = "server=localhost;database=inventory;User ID=marcos;Password=marcos123;";

        public ProductController(cifrado cifrado)
        {
            _cifrado = cifrado ?? throw new ArgumentNullException(nameof(cifrado));
        }

        private async Task<User> ValidateTokenAndGetUser(string token, SampleContext context)
        {
            var vtoken = _cifrado.validarToken(token);

            if (vtoken == null)
            {
                throw new UnauthorizedAccessException("The token isn't valid!");
            }

            return await context.User
                .FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var productList = await context.Product.ToListAsync();

                    if (productList == null || productList.Count == 0)
                    {
                        return NotFound();
                    }

                    return Ok(productList);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProductById(int productId, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var product = await context.Product.FindAsync(productId);

                    if (product == null)
                    {
                        return NotFound();
                    }

                    return Ok(product);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("sku/{sku}")]
        public async Task<ActionResult<Product>> GetProductBySKU(string sku, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var product = await context.Product.FirstOrDefaultAsync(res => res.SKU.Equals(sku));

                    if (product == null)
                    {
                        return NotFound();
                    }

                    return Ok(product);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutProduct(Product product, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var existingProduct = await context.Product.FindAsync(product.ProductId);

                    if (existingProduct == null)
                    {
                        return Problem("No record found");
                    }

                    existingProduct.SKU = product.SKU;
                    existingProduct.Name = product.Name;
                    existingProduct.Kind = product.Kind;
                    existingProduct.Label = product.Label;
                    existingProduct.Price = product.Price;
                    existingProduct.UnitMeasurement = product.UnitMeasurement;

                    await context.SaveChangesAsync();

                    return Ok(existingProduct);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Product>> PostProduct(Product product, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var existingProduct = await context.Product.FirstOrDefaultAsync(res => res.Name.Equals(product.Name));

                    if (existingProduct != null)
                    {
                        return Problem("Product with the same name already exists");
                    }

                    context.Product.Add(product);
                    await context.SaveChangesAsync();

                    return Ok(product.ProductId);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var existingProduct = await context.Product.FindAsync(productId);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    context.Product.Remove(existingProduct);
                    await context.SaveChangesAsync();

                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }
    }
}