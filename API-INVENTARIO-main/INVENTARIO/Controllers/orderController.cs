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
    public class OrderController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SampleContext _context;

        public OrderController(ITokenService tokenService, SampleContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var orderList = await _context.Order.ToListAsync();

                return Ok(orderList);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetOrderById(int orderId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var order = await _context.Order.FindAsync(orderId);

                if (order == null)
                {
                    return NotFound("No order found");
                }
                return Ok(order);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("orderdate/{orderDate}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderByOrderDate(DateTime orderDate)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var orderList = await _context.Order
                    .Where(order => order.OrderDate == orderDate)
                    .ToListAsync();

                return Ok(orderList);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        // Other date-specific methods (reception, dispatched, delivery) follow the same pattern

        [HttpGet("range/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var ordersInRange = await _context.Order
                    .Where(order => order.OrderDate.Date >= startDate.Date && order.OrderDate.Date <= endDate.Date)
                    .ToListAsync();

                return Ok(ordersInRange);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutOrder(Order order)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingOrder = await _context.Order.FirstOrDefaultAsync(res => res.OrderId.Equals(order.OrderId));
                if (existingOrder == null)
                {
                    return Problem("No record found");
                }

                // Update order properties
                _context.Entry(existingOrder).CurrentValues.SetValues(order);

                await _context.SaveChangesAsync();
                return Ok(existingOrder);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                return Ok(order.OrderId);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);
               
                    var order = await _context.Order.FindAsync(orderId);
                    if (order == null)
                    {
                        return NotFound();
                    }

                    _context.Order.Remove(order);
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