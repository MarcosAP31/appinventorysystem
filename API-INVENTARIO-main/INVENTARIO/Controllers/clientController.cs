using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using INVENTARIO.Entity;
using INVENTARIO.Services;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly cifrado _cifrado;
        private readonly string _defaultConnection = "server=localhost;database=inventory;User ID=marcos;Password=marcos123;";

        public ClientController(cifrado cifrado)
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

            return await context.User.FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients(string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var clientList = await context.Client.ToListAsync();

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
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

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
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var existingClient = await context.Client.FirstOrDefaultAsync(res => res.ClientId.Equals(client.ClientId));
                    if (existingClient == null)
                    {
                        return Problem("No record found");
                    }

                    // Update client properties
                    context.Entry(existingClient).CurrentValues.SetValues(client);

                    await context.SaveChangesAsync();
                    return Ok(existingClient);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Client>> PostClient(Client client, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var existingClient = await context.Client.FirstOrDefaultAsync(res => res.Name.Equals(client.Name) && res.LastName.Equals(client.LastName));
                    if (existingClient != null)
                    {
                        return Problem("Client with the same name already exists");
                    }

                    context.Client.Add(client);
                    await context.SaveChangesAsync();

                    return Ok(client.ClientId);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteUser(int clientId, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

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
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }
    }
}