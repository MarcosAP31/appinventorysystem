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
    public class ClientController : ControllerBase
    {
        private cifrado _cifrado;
        string defaultConnection = "server = localhost; database = inventory;User ID=marcos;Password=marcos123;";
        public ClientController(cifrado cifrado_)
        {
            _cifrado = cifrado_;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients(string token)
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

                    var clientList = await context.Client.ToListAsync();
                    /*if (clientList == null || !clientList.Any())
                    {
                        return NotFound("No clients found");
                    }*/

                    return Ok(clientList);



                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }


        }

        [HttpGet("{clientId}")]
        public async Task<ActionResult<Client>> GetClientById(int clientId, string token)
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

                    var client = await context.Client.FindAsync(clientId);

                    if (client == null)
                    {
                        return NotFound("No client found");
                    }
                    return Ok(client);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPut("update")]
        public async Task<ActionResult> PutClient(Client client, string token)
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
                    var query = await context.Client.FirstOrDefaultAsync(res => res.ClientId.Equals(client.ClientId));
                    if (query == null)
                    {
                        return Problem("No record found");
                    }

                    query.Name = client.Name;
                    query.LastName = client.LastName;
                    query.Birthday = client.Birthday;
                    query.Sex = client.Sex;
                    query.Department = client.Department;
                    query.Province = client.Province;
                    query.DeliveryMan = client.DeliveryMan;
                    query.Status = client.Status;
                    context.SaveChanges();
                    return Ok(query);


                }
            }


        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<Client>> PostClient(Client client, string token)
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


                context.Client.Add(client);
                await context.SaveChangesAsync();

                return Ok(client.ClientId);


            }

        }

        // DELETE: api/user/5
        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteUser(int clientId, string token)
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

                    var client = await context.Client.FindAsync(clientId);
                    if (client == null)
                    {
                        return NotFound();
                    }

                    context.Client.Remove(client);
                    await context.SaveChangesAsync();

                    return NoContent();



                }
            }

        }
    }
}
