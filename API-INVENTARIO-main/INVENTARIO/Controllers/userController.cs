using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
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
using System.Net.Http;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly cifrado _cifrado;
        private readonly SampleContext _context;
        private string defaultConnection;
        public UsersController(ITokenService tokenService, SampleContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _context = context ?? throw new ArgumentNullException(nameof(context));

            defaultConnection= "Server=tcp:appservermarcos.database.windows.net,1433;Initial Catalog=inventory;Persist Security Info=False;User ID=marcos;Password=Amkl-572#$LCVy;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(Users user)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var result = await _context.Users.FirstOrDefaultAsync(res => res.Email.Equals(user.Email) && res.Password.Equals(user.Password));
                    if (result == null)
                    {
                        return Problem("No user found");
                    }
                    var cifrado = _tokenService.GenerateToken(defaultConnection.Replace(" ", "") + " " + user.Email + " " + user.Password);
                    
                    // Agregar el token cifrado al encabezado
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cifrado);


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
        public async Task<IActionResult> ValidateLogin()
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {

            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var userList = await _context.Users.ToListAsync();
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
        public async Task<ActionResult<Users>> GetUserById(int userId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var _user = await _context.Users.FindAsync(userId);

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
        public async Task<ActionResult> PutUsers(Users _user)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingUsers = await _context.Users.FirstOrDefaultAsync(res => res.UserId.Equals(_user.UserId));
                if (existingUsers == null)
                {
                    return Problem("No record found");
                }

                // Update user properties
                _context.Entry(existingUsers).CurrentValues.SetValues(_user);

                await _context.SaveChangesAsync();
                return Ok(existingUsers);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Users>> PostUsers(Users _user)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var existingUsers = await _context.Users.FirstOrDefaultAsync(res => res.Name.Equals(_user.Name) && res.LastName.Equals(_user.LastName));
                if (existingUsers != null)
                {
                    return Problem("Users with the same name already exists");
                }

                _context.Users.Add(_user);
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
        public async Task<IActionResult> DeleteUsers(int userId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var _user = await _context.Users.FindAsync(userId);
                if (_user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(_user);
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
        public async Task<ActionResult<Users>> GetUserByFullName(string fullName)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var _user = await _context.Users.FirstOrDefaultAsync(res => (res.Name + " " + res.LastName).Equals(fullName));

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
        public async Task<ActionResult<Users>> GetUserByCode(string code)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var _user = await _context.Users.FirstOrDefaultAsync(res => res.Code.Equals(code));

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
        public async Task<ActionResult<Users>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var _user = await _context.Users.FirstOrDefaultAsync(res => res.Email.Equals(email));

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
        public async Task<ActionResult<Users>> GetUserByRole(string role)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var userList = await _context.Users
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
