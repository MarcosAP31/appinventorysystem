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
        string defaultConnection = "server = localhost; database = inventory;User ID=sa;Password=marcos123;";
        public OrderController(SampleContext context_, cifrado cifrado_)
        {
            _context = context_;
            _cifrado = cifrado_;
        }
 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> getOrders(string token)
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
                    if (context.Order == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var orderList = await context.Order.ToListAsync();
                        return Ok(orderList);
                    }

                }
            }

        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> getOrderById(int orderId, string token)
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
                    if (context.Order == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var order = await context.Order.FindAsync(orderId);

                        if (order == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return order;
                        }
                    }
                }
            }
        }
        [HttpGet("{orderDate}")]
        public async Task<ActionResult<Order>> getOrderByOrderDate(DateTime orderDate, string token)
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
                    if (context.Order == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var order = await context.Order.FirstOrDefaultAsync(res => res.OrderDate.Equals(orderDate));

                        if (order == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return order;
                        }
                    }
                }
            }
        }
        [HttpGet("{receptionDate}")]
        public async Task<ActionResult<Order>> getOrderByReceptionDate(DateTime receptionDate, string token)
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
                    if (context.Order == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var order = await context.Order.FirstOrDefaultAsync(res => res.ReceptionDate.Equals(receptionDate));

                        if (order == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return order;
                        }
                    }
                }
            }
        }
        [HttpGet("{dispatchedDate}")]
        public async Task<ActionResult<Order>> getOrderByDispatchedDate(DateTime dispatchedDate, string token)
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
                    if (context.Order == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var order = await context.Order.FirstOrDefaultAsync(res => res.DispatchedDate.Equals(dispatchedDate));

                        if (order == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return order;
                        }
                    }
                }
            }
        }
        [HttpGet("{deliveryDate}")]
        public async Task<ActionResult<Order>> getOrderByDeliveryDate(DateTime deliveryDate, string token)
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
                    if (context.Order == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var order = await context.Order.FirstOrDefaultAsync(res => res.DeliveryDate.Equals(deliveryDate));

                        if (order == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return order;
                        }
                    }
                }
            }
        }
        [HttpGet("{seller}")]
        public async Task<ActionResult<Order>> getOrderBySeller(string seller, string token)
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
                    if (context.Order == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var order = await context.Order.FirstOrDefaultAsync(res => res.Seller.Equals(seller));

                        if (order == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return order;
                        }
                    }
                }
            }
        }
        [HttpGet("{deliveryMan}")]
        public async Task<ActionResult<Order>> getOrderByDeliveryMan(string deliveryMan, string token)
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
                    if (context.Order == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var order = await context.Order.FirstOrDefaultAsync(res => res.DeliveryMan.Equals(deliveryMan));

                        if (order == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return order;
                        }
                    }
                }
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
                    else
                    {
                        query=order;
                        context.SaveChanges();
                        return Ok(query);
                    }

                }
            }


        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<Order>> PostOrder(Order[] param, string token)
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
                        foreach (Order order in param)
                        {
                            context.Order.Add(order);
                            await context.SaveChangesAsync();
                        }
                        return Ok("Was updated successfully");
                    }
                }
            }

        }

        // DELETE: api/user/5
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> deleteUser(int orderId, string token)
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
                    if (context.Order == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var order = await context.Order.FindAsync(orderId);
                        if (order == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            context.Order.Remove(order);
                            await context.SaveChangesAsync();

                            return NoContent();
                        }


                    }

                }
            }

        }
    }
}
