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
    public class UserController : ControllerBase
    {
        private readonly SampleContext _context;
        private cifrado _cifrado;
        string defaultConnection = "server = localhost; database = inventory;User ID=marcos;Password=marcos123;";
        public UserController(SampleContext context_, cifrado cifrado_)
        {
            _context = context_;
            _cifrado = cifrado_;
        }
        [HttpPost("login")]
        public async Task<IActionResult> GetUserAsync(User user)
        {
            using (var context = new SampleContext(defaultConnection))
            {
                var result = await context.User.FirstOrDefaultAsync(res => res.Email.Equals(user.Email) && res.Password.Equals(user.Password));
                if (result == null)
                {
                    return Problem("No user found");
                }
                var cifrado = _cifrado.EncryptStringAES(defaultConnection.Replace(" ", "") + " " + user.Email + " " + user.Password);

                return Ok(cifrado);
            }
        }
        [HttpPost("validatelogin")]
        public async Task<IActionResult> ValidateLogin(string token)
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
                return Ok(user);
            }

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string token)
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
                    if (context.User == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var userList = await context.User.ToListAsync();
                        return Ok(userList);
                    }

                }
            }

        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId, string token)
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
                    if (context.User == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var _user = await context.User.FindAsync(userId);

                        if (_user == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return _user;
                        }
                    }
                }
            }
        }
        [HttpGet("code/{code}")]
        public async Task<ActionResult<User>> GetUserByCode(string code, string token)
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
                    if (context.User == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var _user = await context.User.FirstOrDefaultAsync(res => res.Code.Equals(code));

                        if (_user == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return _user;
                        }
                    }
                }
            }
        }
        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email, string token)
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
                    if (context.User == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var _user = await context.User.FirstOrDefaultAsync(res => res.Email.Equals(email));

                        if (_user == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return _user;
                        }
                    }
                }
            }
        }
        [HttpGet("role/{role}")]
        public async Task<ActionResult<User>> GetUserByRole(string role, string token)
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
                    var userList = await context.User
                        .Where(u => u.Role == role)
                        .ToListAsync();

                    if (userList == null || !userList.Any())
                    {
                        return NotFound("No order x products found for the specified orderId.");
                    }

                    return Ok(userList);
                }
            }
        }
        [HttpPut("update")]
        public async Task<ActionResult> PutUser(User _user, string token)
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
                    var query = await context.User.FindAsync(_user.UserId);
                    if (query == null)
                    {
                        return Problem("No record found");
                    }
                    else
                    {
                        query=_user;
                        context.SaveChanges();
                        return Ok(query);
                    }

                }
            }


        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<User>> PostUser(User _user, string token)
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
                    context.User.Add(_user);
                    await context.SaveChangesAsync();
                    return Ok("Was updated successfully");
                }
            }

        }

        // DELETE: api/user/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId, string token)
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
                    if (context.User == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var _user = await context.User.FindAsync(userId);
                        if (_user == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            context.User.Remove(_user);
                            await context.SaveChangesAsync();

                            return NoContent();
                        }


                    }

                }
            }

        }

    }
}
