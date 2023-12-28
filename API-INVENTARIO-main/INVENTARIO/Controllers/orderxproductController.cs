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
    public class OrderXProductController : ControllerBase
    {
        private readonly SampleContext _context;
        private cifrado _cifrado;
        string defaultConnection = "server = localhost; database = inventory;User ID=sa;Password=marcos123;";
        public OrderXProductController(SampleContext context_, cifrado cifrado_)
        {
            _context = context_;
            _cifrado = cifrado_;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderXProduct>>> getOrderXProducts(string token)
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
                    if (context.OrderXProduct == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var orderxproductList = await context.OrderXProduct.ToListAsync();
                        return Ok(orderxproductList);
                    }

                }
            }

        }

        [HttpGet("{orderxproductId}")]
        public async Task<ActionResult<OrderXProduct>> getOrderXProductById(int orderxproductId, string token)
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
                    if (context.OrderXProduct == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var orderxproduct = await context.OrderXProduct.FindAsync(orderxproductId);

                        if (orderxproduct == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return orderxproduct;
                        }
                    }
                }
            }
        }
        
        [HttpPut("update")]
        public async Task<ActionResult> PutOrderXProduct(OrderXProduct orderxproduct, string token)
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
                    var query = await context.OrderXProduct.FirstOrDefaultAsync(res => res.OrderXProductId.Equals(orderxproduct.OrderXProductId));
                    if (query == null)
                    {
                        return Problem("No record found");
                    }
                    else
                    {
                        query=orderxproduct;
                        context.SaveChanges();
                        return Ok(query);
                    }

                }
            }


        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<OrderXProduct>> PostOrderXProduct(OrderXProduct[] param, string token)
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
                    if (param.Length == 0)
                    {
                        return Problem("There is no user to insert");
                    }
                    else
                    {
                        foreach (OrderXProduct orderxproduct in param)
                        {
                            context.OrderXProduct.Add(orderxproduct);
                            await context.SaveChangesAsync();
                        }
                        return Ok("Was updated successfully");
                    }
                }
            }

        }

        // DELETE: api/user/5
        [HttpDelete("{orderxproductId}")]
        public async Task<IActionResult> deleteUser(int orderxproductId, string token)
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
                    if (context.OrderXProduct == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var orderxproduct = await context.OrderXProduct.FindAsync(orderxproductId);
                        if (orderxproduct == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            context.OrderXProduct.Remove(orderxproduct);
                            await context.SaveChangesAsync();

                            return NoContent();
                        }


                    }

                }
            }

        }
    }
}
