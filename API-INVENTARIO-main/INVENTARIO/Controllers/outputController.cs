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
using static Grpc.Core.Metadata;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutputController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SampleContext _context;

        public OutputController(ITokenService tokenService, SampleContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Output>>> GetOutputs()
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var outputList = await _context.Output.ToListAsync();

                if (outputList == null || !outputList.Any())
                {
                    return NotFound();
                }

                return Ok(outputList);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{outputId}")]
        public async Task<ActionResult<Output>> GetOutputById(int outputId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);


                var output = await _context.Output.FindAsync(outputId);

                if (output == null)
                {
                    return NotFound();
                }

                return Ok(output);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutOutput(Output output, string token)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingOutput = await _context.Output.FirstOrDefaultAsync(res => res.OutputId.Equals(output.OutputId));

                if (existingOutput == null)
                {
                    return Problem("No record found");
                }

                // Update entry properties
                _context.Entry(existingOutput).CurrentValues.SetValues(output);

                await _context.SaveChangesAsync();

                return Ok(existingOutput);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Output>> PostOutput(Output output)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);
                
                    _context.Output.Add(output);
                    await _context.SaveChangesAsync();

                    return Ok(output.OutputId);
                
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{outputId}")]
        public async Task<IActionResult> DeleteOutput(int outputId, string token)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);
               
                    var existingOutput = await _context.Output.FindAsync(outputId);

                    if (existingOutput == null)
                    {
                        return NotFound();
                    }

                    _context.Output.Remove(existingOutput);
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