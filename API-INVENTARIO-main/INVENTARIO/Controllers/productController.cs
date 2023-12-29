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
        private readonly SampleContext _context;
        private cifrado _cifrado;
        string defaultConnection = "server = localhost; database = inventory;Product ID=sa;Password=marcos123;";
        public ProductController(SampleContext context_, cifrado cifrado_)
        {
            _context = context_;
            _cifrado = cifrado_;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> getProducts(string token)
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
                else
                {
                    if (context.Product == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var userList = await context.Product.ToListAsync();
                        return Ok(userList);
                    }

                }
            }

        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> getProductById(int productId, string token)
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
                else
                {
                    if (context.Product == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var product = await context.Product.FindAsync(productId);

                        if (product == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return product;
                        }
                    }
                }
            }
        }
        [HttpGet("{sku}")]
        public async Task<ActionResult<Product>> getProductBySKU(string sku, string token)
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
                else
                {
                    if (context.Product == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var product = await context.Product.FirstOrDefaultAsync(res => res.SKU.Equals(sku));

                        if (product == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return product;
                        }
                    }
                }
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutProduct(Product product, string token)
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
                else
                {
                    var query = await context.Product.FirstOrDefaultAsync(res => res.ProductId.Equals(product.ProductId));
                    if (query == null)
                    {
                        return Problem("No record found");
                    }
                    else
                    {
                        query.SKU = product.SKU;
                        query.Name = product.Name;
                        query.Kind = product.Kind;
                        query.Label = product.Label;
                        query.UnitMeasurement = product.UnitMeasurement;
                        context.SaveChanges();
                        return Ok(query);
                    }

                }
            }


        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<Product>> PostProduct(Product product, string token)
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
                else
                {
                    context.Product.Add(product);
                    await context.SaveChangesAsync();
                    return Ok("Was updated successfully");
                }
            }

        }

        // DELETE: api/user/5
        [HttpDelete("{productId}")]
        public async Task<IActionResult> deleteProduct(int productId, string token)
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
                else
                {
                    if (context.Product == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var product = await context.Product.FindAsync(productId);
                        if (product == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            context.Product.Remove(product);
                            await context.SaveChangesAsync();

                            return NoContent();
                        }


                    }

                }
            }

        }

        private bool productExists(int id)
        {
            return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}

