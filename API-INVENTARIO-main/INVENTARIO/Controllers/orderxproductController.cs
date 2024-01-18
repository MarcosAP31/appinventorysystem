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
        private cifrado _cifrado;
        string defaultConnection = "server = localhost; database = inventory;User ID=marcos;Password=marcos123;";
        public OrderXProductController(cifrado cifrado_)
        {
            _cifrado = cifrado_;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderXProduct>>> GetOrderXProducts(string token)
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
                        var orderxproductList = await context.OrderXProduct.ToListAsync();
                        return Ok(orderxproductList);
                    }

                }
            }

        }


        [HttpGet("{orderxproductId}")]
        public async Task<ActionResult<OrderXProduct>> GetOrderXProductById(int orderxproductId, string token)
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

        [HttpGet("orderid/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderXProduct>>> GetProductsByOrderId(int orderId, string token)
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
                    var user = await context.User
                        .FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));

                    if (user == null)
                    {
                        return Problem("The user entered isn't valid");
                    }

                    var orderxproductList = await (from orderxproduct in context.OrderXProduct

                                                   join product in context.Product on orderxproduct.ProductId equals product.ProductId
                                                   where orderxproduct.OrderId==orderId
                                                   select new
                                                   {
                                                       orderxproduct.OrderXProductId,
                                                       orderxproduct.OrderId,
                                                       orderxproduct.ProductId,
                                                       orderxproduct.Quantity,
                                                       orderxproduct.Subtotal,
                                                       product.SKU,
                                                       product.Name,
                                                       product.Price
                                                   }).ToListAsync();

                    if (orderxproductList == null || !orderxproductList.Any())
                    {
                        return NotFound("No order x products found for the specified orderId.");
                    }

                    return Ok(orderxproductList);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("totalprice/{orderId}")]
        public async Task<ActionResult<float>> GetTotalPriceByOrderId(int orderId, string token)
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
                    var user = await context.User
                        .FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));

                    if (user == null)
                    {
                        return Problem("The user entered isn't valid");
                    }

                    var orderxproductList = await (from orderxproduct in context.OrderXProduct

                                         join product in context.Product on orderxproduct.ProductId equals product.ProductId
                                         where orderxproduct.OrderId==orderId
                                         select new
                                         {
                                             orderxproduct.OrderXProductId,
                                             orderxproduct.OrderId,
                                             orderxproduct.ProductId,
                                             orderxproduct.Quantity,
                                             orderxproduct.Subtotal,
                                             product.Name,
                                             product.Price
                                         }).ToListAsync();

                    if (orderxproductList == null || !orderxproductList.Any())
                    {
                        return NotFound("No order x products found for the specified orderId.");
                    }

                    // Calculate total price
                    decimal? totalPrice = orderxproductList.Sum(op => op.Price * op.Quantity);

                    return Ok(totalPrice);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
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
                        query.OrderId = orderxproduct.OrderId;
                        query.ProductId = orderxproduct.ProductId;
                        query.Quantity = orderxproduct.Quantity;
                        query.Subtotal = orderxproduct.Subtotal;
                        context.SaveChanges();
                        return Ok(query);
                    }
                }
            }
        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<OrderXProduct>> PostOrderXProduct(OrderXProduct orderxproduct, string token)
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
                    context.OrderXProduct.Add(orderxproduct);
                    await context.SaveChangesAsync();
                    return Ok("Was updated successfully");
                }
            }

        }

        // DELETE: api/user/5
        [HttpDelete("{orderxproductId}")]
        public async Task<IActionResult> DeleteOrderXProduct(int orderxproductId, string token)
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
