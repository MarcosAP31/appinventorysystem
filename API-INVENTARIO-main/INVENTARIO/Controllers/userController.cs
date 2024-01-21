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
    public class UserController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SampleContext _context;
        public UserController(ITokenService tokenService, SampleContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {

            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var userList = await _context.User.ToListAsync();
                /*if (userList == null || userList.Count == 0)
                {
                    return NotFound();
                }¨*/

                return Ok(userList);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var _user = await _context.User.FindAsync(userId);

                if (_user == null)
                {
                    return NotFound("No user found");
                }
                return Ok(_user);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutUser(User _user)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingUser = await _context.User.FirstOrDefaultAsync(res => res.UserId.Equals(_user.UserId));
                if (existingUser == null)
                {
                    return Problem("No record found");
                }

                // Update user properties
                _context.Entry(existingUser).CurrentValues.SetValues(_user);

                await _context.SaveChangesAsync();
                return Ok(existingUser);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<User>> PostUser(User _user)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingUser = await _context.User.FirstOrDefaultAsync(res => res.Name.Equals(_user.Name) && res.LastName.Equals(_user.LastName));
                if (existingUser != null)
                {
                    return Problem("User with the same name already exists");
                }

                _context.User.Add(_user);
                await _context.SaveChangesAsync();

                return Ok(_user.UserId);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var _user = await _context.User.FindAsync(userId);
                if (_user == null)
                {
                    return NotFound();
                }

                _context.User.Remove(_user);
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

        [HttpGet("fullname/{fullName}")]
        public async Task<ActionResult<User>> GetUserByFullName(string fullName)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var _user = await _context.User.FirstOrDefaultAsync(res => (res.Name + " " + res.LastName).Equals(fullName));

                if (_user == null)
                {
                    return NotFound();
                }

                return Ok(_user);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<User>> GetUserByCode(string code)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var _user = await _context.User.FirstOrDefaultAsync(res =>res.Code.Equals(code));

                if (_user == null)
                {
                    return NotFound();
                }

                return Ok(_user);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var _user = await _context.User.FirstOrDefaultAsync(res =>res.Email.Equals(email));

                if (_user == null)
                {
                    return NotFound();
                }

                return Ok(_user);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet("role/{role}")]
        public async Task<ActionResult<User>> GetUserByRole(string role)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var userList = await _context.User
                        .Where(u => u.Role == role)
                        .ToListAsync();

                /*if (userList == null || !userList.Any())
                {
                    return NotFound("No user found for the specified userId.");
                }*/

                return Ok(userList);

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
