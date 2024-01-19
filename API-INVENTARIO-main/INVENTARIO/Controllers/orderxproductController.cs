using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INVENTARIO.Entity;
using INVENTARIO.Services;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderXProductController : ControllerBase
    {
        private readonly cifrado _cifrado;
        private readonly string _defaultConnection = "server=localhost;database=inventory;User ID=marcos;Password=marcos123;";

        public OrderXProductController(cifrado cifrado)
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
        public async Task<ActionResult<IEnumerable<OrderXProduct>>> GetOrderXProducts(string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var orderXProductList = await context.OrderXProduct.ToListAsync();

                    if (orderXProductList == null || !orderXProductList.Any())
                    {
                        return NotFound();
                    }

                    return Ok(orderXProductList);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{orderxproductId}")]
        public async Task<ActionResult<OrderXProduct>> GetOrderXProductById(int orderxproductId, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var orderXProduct = await context.OrderXProduct.FindAsync(orderxproductId);

                    if (orderXProduct == null)
                    {
                        return NotFound();
                    }

                    return Ok(orderXProduct);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("orderid/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderXProduct>>> GetProductsByOrderId(int orderId, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var orderXProductList = await context.OrderXProduct
                        .Join(context.Product, oxp => oxp.ProductId, p => p.ProductId, (oxp, p) => new
                        {
                            oxp.OrderXProductId,
                            oxp.OrderId,
                            oxp.ProductId,
                            oxp.Quantity,
                            oxp.Subtotal,
                            p.SKU,
                            p.Name,
                            p.Price
                        })
                        .Where(oxp => oxp.OrderId == orderId)
                        .ToListAsync();

                    if (orderXProductList == null || !orderXProductList.Any())
                    {
                        return NotFound($"No order x products found for the specified orderId: {orderId}.");
                    }

                    return Ok(orderXProductList);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("totalprice/{orderId}")]
        public async Task<ActionResult<decimal>> GetTotalPriceByOrderId(int orderId, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var totalPrice = await context.OrderXProduct
                        .Where(oxp => oxp.OrderId == orderId)
                        .Join(context.Product, oxp => oxp.ProductId, p => p.ProductId, (oxp, p) => oxp.Quantity * p.Price)
                        .SumAsync();

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
        public async Task<ActionResult> PutOrderXProduct(OrderXProduct orderXProduct, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var existingOrderXProduct = await context.OrderXProduct.FirstOrDefaultAsync(res => res.OrderXProductId.Equals(orderXProduct.OrderXProductId));

                    if (existingOrderXProduct == null)
                    {
                        return Problem("No record found");
                    }

                    context.Entry(existingOrderXProduct).CurrentValues.SetValues(orderXProduct);
                    await context.SaveChangesAsync();

                    return Ok(existingOrderXProduct);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<OrderXProduct>> PostOrderXProduct(OrderXProduct orderXProduct, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    context.OrderXProduct.Add(orderXProduct);
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

        [HttpDelete("{orderxproductId}")]
        public async Task<IActionResult> DeleteOrderXProduct(int orderXProductId, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var existingOrderXProduct = await context.OrderXProduct.FindAsync(orderXProductId);

                    if (existingOrderXProduct == null)
                    {
                        return NotFound();
                    }

                    context.OrderXProduct.Remove(existingOrderXProduct);
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