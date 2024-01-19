using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using INVENTARIO.Entity;
using INVENTARIO.Services;
using INVENTARIO.Interfaces;

namespace INVENTARIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly string _defaultConnection = "server=localhost;database=inventory;User ID=marcos;Password=marcos123;";

        public SupplierController(ITokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                using (var context = new SampleContext(_defaultConnection))
                {
                    var supplierList = await context.Supplier.ToListAsync();

                    if (supplierList == null || supplierList.Count == 0)
                    {
                        return NotFound();
                    }

                    return Ok(supplierList);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{supplierId}")]
        public async Task<ActionResult<Supplier>> GetSupplierById(int supplierId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                using (var context = new SampleContext(_defaultConnection))
                {

                    var supplier = await context.Supplier.FindAsync(supplierId);

                    if (supplier == null)
                    {
                        return NotFound();
                    }

                    return Ok(supplier);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutSupplier(Supplier supplier, string token)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                using (var context = new SampleContext(_defaultConnection))
                {
                    

                    var existingSupplier = await context.Supplier.FirstOrDefaultAsync(res => res.SupplierId.Equals(supplier.SupplierId));

                    if (existingSupplier == null)
                    {
                        return Problem("No record found");
                    }

                    existingSupplier.RUC = supplier.RUC;
                    existingSupplier.BusinessName = supplier.BusinessName;
                    existingSupplier.TradeName = supplier.TradeName;
                    existingSupplier.Kind = supplier.Kind;
                    existingSupplier.Department = supplier.Department;
                    existingSupplier.Province = supplier.Province;
                    existingSupplier.District = supplier.District;
                    existingSupplier.Direction = supplier.Direction;
                    existingSupplier.Phone = supplier.Phone;
                    existingSupplier.Email = supplier.Email;

                    await context.SaveChangesAsync();

                    return Ok(existingSupplier);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier, string token)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                using (var context = new SampleContext(_defaultConnection))
                {

                    var existingSupplier = await context.Supplier.FirstOrDefaultAsync(res => res.BusinessName.Equals(supplier.BusinessName));

                    if (existingSupplier != null)
                    {
                        return Problem("Supplier with the same name already exists");
                    }

                    context.Supplier.Add(supplier);
                    await context.SaveChangesAsync();

                    return Ok(supplier.SupplierId);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{supplierId}")]
        public async Task<IActionResult> DeleteSupplier(int supplierId, string token)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                using (var context = new SampleContext(_defaultConnection))
                {

                    var existingSupplier = await context.Supplier.FindAsync(supplierId);

                    if (existingSupplier == null)
                    {
                        return NotFound();
                    }

                    context.Supplier.Remove(existingSupplier);
                    await context.SaveChangesAsync();

                    return NoContent();
                }
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
