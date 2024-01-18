using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INVENTARIO.Entity;
using Microsoft.EntityFrameworkCore;
using INVENTARIO.Services;
using NuGet.Common;
using Newtonsoft.Json.Linq;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private cifrado _cifrado;
        string defaultConnection = "server = localhost; database = inventory;User ID=marcos;Password=marcos123;";
        public ProductController(cifrado cifrado_)
        {
            _cifrado = cifrado_;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string token)
        {
            try
            {
                var vtoken = _cifrado.validarToken(token);

                if (vtoken == null)
                {
                    return Problem("The token isn't valid!");
                }
                using (var context = new SampleContext(defaultConnection))
                {
                    var user = await context.User.FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
                    if (user == null)
                    {
                        return Problem("The user enterd isn't valid");
                    }
                    
                    var productList = await context.Product.ToListAsync();
                    /*if (productList == null || !productList.Any())
                    {
                        return NotFound("No products found");
                    }*/

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
                var vtoken = _cifrado.validarToken(token);

                if (vtoken == null)
                {
                    return Problem("The token isn't valid!");
                }
                using (var context = new SampleContext(defaultConnection))
                {
                    var user = await context.User.FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
                    if (user == null)
                    {
                        return Problem("The user entered isn't valid");
                    }

                    var product = await context.Product.FindAsync(productId);

                    if (product == null)
                    {
                        return NotFound("No product found");
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
                var vtoken = _cifrado.validarToken(token);

                if (vtoken == null)
                {
                    return Problem("The token isn't valid!");
                }
                using (var context = new SampleContext(defaultConnection))
                {
                    var user = await context.User.FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
                    if (user == null)
                    {
                        return Problem("The user entered isn't valid");
                    }

                    var product = await context.Product.FirstOrDefaultAsync(res => res.SKU.Equals(sku));

                    if (product == null)
                    {
                        return NotFound("No product found");
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
                var vtoken = _cifrado.validarToken(token);

                if (vtoken == null)
                {
                    return Problem("The token isn't valid!");
                }
                using (var context = new SampleContext(defaultConnection))
                {
                    var user = await context.User.FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
                    if (user == null)
                    {
                        return Problem("The user entered isn't valid");
                    }

                    var query = await context.Product.FindAsync(product.ProductId);
                    if (query == null)
                    {
                        return Problem("No record found");
                    }

                    query.SKU = product.SKU;
                    query.Name = product.Name;
                    query.Kind = product.Kind;
                    query.Label = product.Label;
                    query.Price=product.Price;
                    query.UnitMeasurement = product.UnitMeasurement;
                    context.SaveChanges();
                    return Ok(query);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }



        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<Product>> PostProduct(Product product, string token)
        {
            try
            {
                var vtoken = _cifrado.validarToken(token);

                if (vtoken == null)
                {
                    return Problem("The token isn't valid!");
                }
                using (var context = new SampleContext(defaultConnection))
                {
                    var user = await context.User.FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
                    if (user == null)
                    {
                        return Problem("The user entered isn't valid");
                    }

                    var existingProduct = await context.Product.FirstOrDefaultAsync(res => res.Name.Equals(product.Name));
                    if (existingProduct != null)
                    {
                        return Problem("Product with the same name already exists");
                    }

                    context.Product.Add(product);
                    await context.SaveChangesAsync();
                    return Ok("Was updated successfully");


                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }


        }

        // DELETE: api/user/5
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId, string token)
        {
            try
            {
                var vtoken = _cifrado.validarToken(token);

                if (vtoken == null)
                {
                    return Problem("The token isn't valid!");
                }
                using (var context = new SampleContext(defaultConnection))
                {
                    var user = await context.User.FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
                    if (user == null)
                    {
                        return Problem("The user entered isn't valid");
                    }

                    var product = await context.Product.FindAsync(productId);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    context.Product.Remove(product);
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

