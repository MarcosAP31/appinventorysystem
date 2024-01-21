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
using INVENTARIO.Interfaces;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicationController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SampleContext _context;
        public UbicationController(ITokenService tokenService, SampleContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubication>>> GetUbications()
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var ubicationList = await _context.Ubication.ToListAsync();
                /*if (ubicationList == null || !ubicationList.Any())
                {
                    return NotFound("No ubications found");
                }*/

                return Ok(ubicationList);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }


        }

        [HttpGet("{ubicationId}")]
        public async Task<ActionResult<Ubication>> GetUbicationById(int ubicationId)
        {
            try
            {

                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var ubication = await _context.Ubication.FindAsync(ubicationId);

                    if (ubication == null)
                    {
                        return NotFound("No ubication found");
                    }
                    return Ok(ubication);

                
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPut("update")]
        public async Task<ActionResult> PutUbication(Ubication ubication)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingUbication = await _context.Ubication.FirstOrDefaultAsync(res => res.UbicationId.Equals(ubication.UbicationId));
                if (existingUbication == null)
                {
                    return Problem("No record found");
                }

                // Update ubication properties
                _context.Entry(existingUbication).CurrentValues.SetValues(ubication);

                await _context.SaveChangesAsync();
                return Ok(existingUbication);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<Ubication>> PostUbication(Ubication ubication)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingUbication = await _context.Ubication.FirstOrDefaultAsync(res => res.Name.Equals(ubication.Name));
                if (existingUbication != null)
                {
                    return Problem("Ubication with the same name already exists");
                }

                _context.Ubication.Add(ubication);
                await _context.SaveChangesAsync();

                return Ok(ubication.UbicationId);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        // DELETE: api/user/5
        [HttpDelete("{ubicationId}")]
        public async Task<IActionResult> DeleteUbication(int ubicationId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var ubication = await _context.Ubication.FindAsync(ubicationId);
                if (ubication == null)
                {
                    return NotFound();
                }

                _context.Ubication.Remove(ubication);
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
