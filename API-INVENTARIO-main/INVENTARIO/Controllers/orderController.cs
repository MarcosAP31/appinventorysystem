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
    public class OrderController : ControllerBase
    {
        private cifrado _cifrado;
        string defaultConnection = "server = localhost; database = inventory;User ID=sa;Password=marcos123;";
        public OrderController(cifrado cifrado_)
        {
            _cifrado = cifrado_;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(string token)
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

                    var orderList = await context.Order.ToListAsync();
                    /*if (orderList == null || !orderList.Any())
                    {
                        return NotFound("No orders found");
                    }*/

                    return Ok(orderList);



                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }


        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetOrderById(int orderId, string token)
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

                    var order = await context.Order.FindAsync(orderId);

                    if (order == null)
                    {
                        return NotFound("No order found");
                    }
                    return Ok(order);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet("orderdate/{orderDate}")]
        public async Task<ActionResult<Order>> GetOrderByOrderDate(DateTime orderDate, string token)
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
                var orderList = await context.Order
                       .Where(order => order.OrderDate == orderDate)
                       .ToListAsync();
                

               

                return Ok(orderList);



            }
        }
        [HttpGet("receptiondate/{receptionDate}")]
        public async Task<ActionResult<Order>> GetOrderByReceptionDate(DateTime receptionDate, string token)
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
                var orderList = await context.Order
                        .Where(order => order.ReceptionDate == receptionDate)
                        .ToListAsync();

                return Ok(orderList);

            }
        }
        [HttpGet("dispatcheddate/{dispatchedDate}")]
        public async Task<ActionResult<Order>> GetOrderByDispatchedDate(DateTime dispatchedDate, string token)
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

                    var orderList = await context.Order
                       .Where(order => order.DispatchedDate == dispatchedDate)
                       .ToListAsync();
                    return Ok(orderList);

                }
            }
        }
        [HttpGet("deliverydate/{deliveryDate}")]
        public async Task<ActionResult<Order>> GetOrderByDeliveryDate(DateTime deliveryDate, string token)
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

                var orderList = await context.Order
                       .Where(order => order.DeliveryDate == deliveryDate)
                       .ToListAsync();

                return Ok(orderList);
            }
        }
        [HttpGet("seller/{seller}")]
        public async Task<ActionResult<Order>> GetOrderBySeller(string seller, string token)
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

                var orderList = await context.Order
                       .Where(order => order.Seller == seller)
                       .ToListAsync();

                return Ok(orderList);
            }
        }
        [HttpGet("deliveryman/{deliveryMan}")]
        public async Task<ActionResult<Order>> GetOrderByDeliveryMan(string deliveryMan, string token)
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

                var orderList = await context.Order
                       .Where(order => order.DeliveryMan == deliveryMan)
                       .ToListAsync();

                return Ok(orderList);
            }
        }

        [HttpGet("range/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDateRange(DateTime startDate, DateTime endDate, string token)
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

                    var ordersInRange = await context.Order
                        .Where(order => order.OrderDate.Date >= startDate.Date && order.OrderDate.Date <= endDate.Date)
                        .ToListAsync();

                    /*if (ordersInRange == null || !ordersInRange.Any())
                    {
                        return NotFound("No orders found within the specified date range");
                    }*/

                    return Ok(ordersInRange);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutOrder(Order order, string token)
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
                    var query = await context.Order.FirstOrDefaultAsync(res => res.OrderId.Equals(order.OrderId));
                    if (query == null)
                    {
                        return Problem("No record found");
                    }

                    query.OrderDate = order.OrderDate;
                    query.ReceptionDate = order.ReceptionDate;
                    query.DispatchedDate = order.DispatchedDate;
                    query.DeliveryDate = order.DeliveryDate;
                    query.TotalPrice=order.TotalPrice;
                    query.Seller = order.Seller;
                    query.DeliveryMan=order.DeliveryMan;
                    query.Status = order.Status;
                    context.SaveChanges();
                    return Ok(query);


                }
            }


        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<Order>> PostOrder(Order order, string token)
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


                context.Order.Add(order);
                await context.SaveChangesAsync();

                return Ok(order.OrderId);


            }

        }

        // DELETE: api/user/5
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteUser(int orderId, string token)
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

                    var order = await context.Order.FindAsync(orderId);
                    if (order == null)
                    {
                        return NotFound();
                    }

                    context.Order.Remove(order);
                    await context.SaveChangesAsync();

                    return NoContent();



                }
            }

        }
    }
}
