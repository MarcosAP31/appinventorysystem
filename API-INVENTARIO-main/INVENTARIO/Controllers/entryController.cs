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
    public class EntryController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SampleContext _context;


        public EntryController(ITokenService tokenService, SampleContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntries()
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var entryList = await _context.Entry.ToListAsync();

                return Ok(entryList);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{entryId}")]
        public async Task<ActionResult<Entry>> GetEntryById(int entryId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var entry = await _context.Entry.FindAsync(entryId);

                if (entry == null)
                {
                    return NotFound("No entry found");
                }
                return Ok(entry);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutEntry(Entry entry)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingEntry = await _context.Entry.FirstOrDefaultAsync(res => res.EntryId.Equals(entry.EntryId));
                if (existingEntry == null)
                {
                    return Problem("No record found");
                }

                // Update entry properties
                _context.Entry(existingEntry).CurrentValues.SetValues(entry);

                await _context.SaveChangesAsync();
                return Ok(existingEntry);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Entry>> PostEntry(Entry entry)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);
                _context.Entry.Add(entry);
                await _context.SaveChangesAsync();

                return Ok(entry.EntryId);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{entryId}")]
        public async Task<IActionResult> DeleteEntry(int entryId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var entry = await _context.Entry.FindAsync(entryId);
                if (entry == null)
                {
                    return NotFound();
                }

                _context.Entry.Remove(entry);
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