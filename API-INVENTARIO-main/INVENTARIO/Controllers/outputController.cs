using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INVENTARIO.Entity;
using INVENTARIO.Services;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutputController : ControllerBase
    {
        private readonly cifrado _cifrado;
        private readonly string _defaultConnection = "server=localhost;database=inventory;User ID=marcos;Password=marcos123;";

        public OutputController(cifrado cifrado)
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

            return await context.User
                .FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Output>>> GetOutputs(string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var outputList = await context.Output.ToListAsync();

                    if (outputList == null || !outputList.Any())
                    {
                        return NotFound();
                    }

                    return Ok(outputList);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{outputId}")]
        public async Task<ActionResult<Output>> GetOutputById(int outputId, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var output = await context.Output.FindAsync(outputId);

                    if (output == null)
                    {
                        return NotFound();
                    }

                    return Ok(output);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutOutput(Output output, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var existingOutput = await context.Output.FirstOrDefaultAsync(res => res.OutputId.Equals(output.OutputId));

                    if (existingOutput == null)
                    {
                        return Problem("No record found");
                    }

                    existingOutput.Date = output.Date;
                    existingOutput.Amount = output.Amount;
                    existingOutput.ProductId = output.ProductId;
                    existingOutput.UbicationId = output.UbicationId;
                    existingOutput.UserId = output.UserId;

                    await context.SaveChangesAsync();

                    return Ok(existingOutput);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Output>> PostOutput(Output output, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    context.Output.Add(output);
                    await context.SaveChangesAsync();

                    return Ok(output.OutputId);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{outputId}")]
        public async Task<IActionResult> DeleteOutput(int outputId, string token)
        {
            try
            {
                using (var context = new SampleContext(_defaultConnection))
                {
                    var user = await ValidateTokenAndGetUser(token, context);

                    var existingOutput = await context.Output.FindAsync(outputId);

                    if (existingOutput == null)
                    {
                        return NotFound();
                    }

                    context.Output.Remove(existingOutput);
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