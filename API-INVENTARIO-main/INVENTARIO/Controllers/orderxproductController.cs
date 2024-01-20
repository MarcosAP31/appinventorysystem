using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INVENTARIO.Entity;
using INVENTARIO.Services;
using INVENTARIO.Interfaces;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderXProductController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SampleContext _context;

        public OrderXProductController(ITokenService tokenService, SampleContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderXProduct>>> GetOrderXProducts()
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var orderXProductList = await _context.OrderXProduct.ToListAsync();

                if (orderXProductList == null || !orderXProductList.Any())
                {
                    return NotFound();
                }

                return Ok(orderXProductList);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{orderxproductId}")]
        public async Task<ActionResult<OrderXProduct>> GetOrderXProductById(int orderxproductId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);


                var orderXProduct = await _context.OrderXProduct.FindAsync(orderxproductId);

                if (orderXProduct == null)
                {
                    return NotFound();
                }

                return Ok(orderXProduct);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("orderid/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderXProduct>>> GetProductsByOrderId(int orderId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var orderXProductList = await _context.OrderXProduct
                    .Join(_context.Product, oxp => oxp.ProductId, p => p.ProductId, (oxp, p) => new
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
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("totalprice/{orderId}")]
        public async Task<ActionResult<decimal>> GetTotalPriceByOrderId(int orderId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var totalPrice = await _context.OrderXProduct
                    .Where(oxp => oxp.OrderId == orderId)
                    .Join(_context.Product, oxp => oxp.ProductId, p => p.ProductId, (oxp, p) => oxp.Quantity * p.Price)
                    .SumAsync();

                return Ok(totalPrice);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutOrderXProduct(OrderXProduct orderXProduct)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingOrderXProduct = await _context.OrderXProduct.FirstOrDefaultAsync(res => res.OrderXProductId.Equals(orderXProduct.OrderXProductId));

                if (existingOrderXProduct == null)
                {
                    return Problem("No record found");
                }

                _context.Entry(existingOrderXProduct).CurrentValues.SetValues(orderXProduct);
                await _context.SaveChangesAsync();

                return Ok(existingOrderXProduct);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<OrderXProduct>> PostOrderXProduct(OrderXProduct orderXProduct)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                _context.OrderXProduct.Add(orderXProduct);
                await _context.SaveChangesAsync();

                return Ok("Was updated successfully");

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{orderxproductId}")]
        public async Task<IActionResult> DeleteOrderXProduct(int orderXProductId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);


                var existingOrderXProduct = await _context.OrderXProduct.FindAsync(orderXProductId);

                if (existingOrderXProduct == null)
                {
                    return NotFound();
                }

                _context.OrderXProduct.Remove(existingOrderXProduct);
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