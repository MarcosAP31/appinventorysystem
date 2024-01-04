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
        private cifrado _cifrado;
        string defaultConnection = "server = localhost; database = inventory;User ID=sa;Password=marcos123;";
        public UserController(cifrado cifrado_)
        {
            _cifrado = cifrado_;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            try
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
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpPost("validatelogin")]
        public async Task<IActionResult> ValidateLogin(string token)
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
                        return Problem("The user isn't valid");
                    }
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }


        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string token)
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

                    var userList = await context.User.ToListAsync();
                    /*if (userList == null || !userList.Any())
                    {
                        return NotFound("No users found");
                    }*/

                    return Ok(userList);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }


        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId, string token)
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

                    var _user = await context.User.FindAsync(userId);

                    if (_user == null)
                    {
                        return NotFound("No user found");
                    }

                    return Ok(_user);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("fullname/{fullName}")]
        public async Task<ActionResult<User>> GetUserByFullName(string fullName, string token)
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

                    var _user = await context.User.FirstOrDefaultAsync(res => (res.Name+" "+res.LastName).Equals(fullName));

                    if (_user == null)
                    {
                        return NotFound();
                    }

                    return Ok(_user);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<User>> GetUserByCode(string code, string token)
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

                    var _user = await context.User.FirstOrDefaultAsync(res => res.Code.Equals(code));

                    if (_user == null)
                    {
                        return NotFound();
                    }

                    return Ok(_user);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email, string token)
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
                    var _user = await context.User.FirstOrDefaultAsync(res => res.Email.Equals(email));

                    if (_user == null)
                    {
                        return NotFound();
                    }

                    return Ok(_user);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet("role/{role}")]
        public async Task<ActionResult<User>> GetUserByRole(string role, string token)
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

                    var userList = await context.User
                        .Where(u => u.Role == role)
                        .ToListAsync();

                    /*if (userList == null || !userList.Any())
                    {
                        return NotFound("No user found for the specified userId.");
                    }*/

                    return Ok(userList);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpPut("update")]
        public async Task<ActionResult> PutUser(User _user, string token)
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

                    var query = await context.User.FindAsync(_user.UserId);
                    if (query == null)
                    {
                        return Problem("No record found");
                    }

                    query.Code = _user.Code;
                    query.Name = _user.Name;
                    query.LastName = _user.LastName;
                    query.Phone = _user.Phone;
                    query.Position=_user.Position;
                    query.Role = _user.Role;
                    query.Email=_user.Email;
                    query.Password = _user.Password;
                    context.SaveChanges();
                    return Ok(query);



                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }



        }

        // POST: api/user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult<User>> PostUser(User _user, string token)
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

                    var existingUser = await context.User.FirstOrDefaultAsync(res => res.Name.Equals(_user.Name) && res.LastName.Equals(_user.LastName));
                    if (existingUser != null)
                    {
                        return Problem("User with the same name already exists");
                    }

                    context.User.Add(_user);
                    await context.SaveChangesAsync();
                    return Ok("Was updated successfully");

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }


        }

        // DELETE: api/user/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId, string token)
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
                    else
                    {

                        var _user = await context.User.FindAsync(userId);
                        if (_user == null)
                        {
                            return NotFound();
                        }


                        context.User.Remove(_user);
                        await context.SaveChangesAsync();

                        return NoContent();



                    }
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
