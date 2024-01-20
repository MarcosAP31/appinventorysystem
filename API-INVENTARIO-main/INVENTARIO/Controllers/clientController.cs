using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using INVENTARIO.Entity;
using INVENTARIO.Services;
using INVENTARIO.Interfaces;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SampleContext _context;

        public ClientController(ITokenService tokenService, SampleContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {

            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var clientList = await _context.Client.ToListAsync();
                /*if (clientList == null || clientList.Count == 0)
                {
                    return NotFound();
                }¨*/

                return Ok(clientList);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{clientId}")]
        public async Task<ActionResult<Client>> GetClientById(int clientId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var client = await _context.Client.FindAsync(clientId);

                if (client == null)
                {
                    return NotFound("No client found");
                }
                return Ok(client);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutClient(Client client, string token)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingClient = await _context.Client.FirstOrDefaultAsync(res => res.ClientId.Equals(client.ClientId));
                if (existingClient == null)
                {
                    return Problem("No record found");
                }

                // Update client properties
                _context.Entry(existingClient).CurrentValues.SetValues(client);

                await _context.SaveChangesAsync();
                return Ok(existingClient);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Client>> PostClient(Client client, string token)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingClient = await _context.Client.FirstOrDefaultAsync(res => res.Name.Equals(client.Name) && res.LastName.Equals(client.LastName));
                if (existingClient != null)
                {
                    return Problem("Client with the same name already exists");
                }

                _context.Client.Add(client);
                await _context.SaveChangesAsync();

                return Ok(client.ClientId);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteUser(int clientId, string token)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var client = await _context.Client.FindAsync(clientId);
                if (client == null)
                {
                    return NotFound();
                }

                _context.Client.Remove(client);
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