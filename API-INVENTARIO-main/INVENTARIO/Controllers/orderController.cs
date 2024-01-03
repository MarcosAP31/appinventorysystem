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
        private readonly SampleContext _context;
        private cifrado _cifrado;
        string defaultConnection = "server = localhost; database = inventory;User ID=marcos;Password=marcos123;";
        public OrderController(SampleContext context_, cifrado cifrado_)
        {
            _context = context_;
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

                var order = await context.Order.FindAsync(orderDate);

                if (order == null)
                {
                    return NotFound("No order found");
                }

                return Ok(order);



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
                var order = await context.Order.FirstOrDefaultAsync(res => res.ReceptionDate.Equals(receptionDate));

                if (order == null)
                {
                    return NotFound("No order found");
                }

                return Ok(order);



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

                    var order = await context.Order.FirstOrDefaultAsync(res => res.DispatchedDate.Equals(dispatchedDate));

                    if (order == null)
                    {
                        return NotFound("No order found");
                    }

                    return Ok(order);


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

                var order = await context.Order.FirstOrDefaultAsync(res => res.DeliveryDate.Equals(deliveryDate));

                if (order == null)
                {
                    return NotFound("No order found");
                }

                return Ok(order);


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

                var order = await context.Order.FirstOrDefaultAsync(res => res.Seller.Equals(seller));

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
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

                var order = await context.Order.FirstOrDefaultAsync(res => res.DeliveryMan.Equals(deliveryMan));

                if (order == null)
                {
                    return NotFound("No order found");
                }

                return Ok(order);

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
