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
    public class UbicationController : ControllerBase
    {
        private cifrado _cifrado;
        string defaultConnection = "server = localhost; database = inventory;User ID=marcos;Password=marcos123;";
        public UbicationController(cifrado cifrado_)
        {
            _cifrado = cifrado_;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubication>>> GetUbications(string token)
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

                    var ubicationList = await context.Ubication.ToListAsync();
                    /*if (ubicationList == null || !ubicationList.Any())
                    {
                        return NotFound("No ubications found");
                    }*/

                    return Ok(ubicationList);



                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }


        }

        [HttpGet("{ubicationId}")]
        public async Task<ActionResult<Ubication>> GetUbicationById(int ubicationId, string token)
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

                    var ubication = await context.Ubication.FindAsync(ubicationId);

                    if (ubication == null)
                    {
                        return NotFound("No ubication found");
                    }
                    return Ok(ubication);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPut("update")]
        public async Task<ActionResult> PutUbication(Ubication ubication, string token)
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
                    var query = await context.Ubication.FirstOrDefaultAsync(res => res.UbicationId.Equals(ubication.UbicationId));
                    if (query == null)
                    {
                        return Problem("No record found");
                    }

                    query.Name = ubication.Name;
                    query.Description = ubication.Description;
                    query.Amount = ubication.Amount;
                    query.Capacity = ubication.Capacity;
                    context.SaveChanges();
                    return Ok(query);


                }
            }


        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<Ubication>> PostUbication(Ubication ubication, string token)
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
                var existingUbication = await context.Ubication.FirstOrDefaultAsync(res => res.Name.Equals(ubication.Name));
                if (existingUbication != null)
                {
                    return Problem("Ubication with the same name already exists");
                }

                context.Ubication.Add(ubication);
                await context.SaveChangesAsync();

                return Ok(ubication.UbicationId);


            }

        }

        // DELETE: api/user/5
        [HttpDelete("{ubicationId}")]
        public async Task<IActionResult> DeleteUbication(int ubicationId, string token)
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

                    var ubication = await context.Ubication.FindAsync(ubicationId);
                    if (ubication == null)
                    {
                        return NotFound();
                    }

                    context.Ubication.Remove(ubication);
                    await context.SaveChangesAsync();

                    return NoContent();



                }
            }

        }
    }
}
