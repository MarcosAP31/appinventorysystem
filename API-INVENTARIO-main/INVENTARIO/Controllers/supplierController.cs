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
        private readonly SampleContext _context;

        public SupplierController(ITokenService tokenService, SampleContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);

                var supplierList = await _context.Supplier.ToListAsync();

                return Ok(supplierList);

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


                var supplier = await _context.Supplier.FindAsync(supplierId);

                if (supplier == null)
                {
                    return NotFound();
                }

                return Ok(supplier);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutSupplier(Supplier supplier)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);


                var existingSupplier = await _context.Supplier.FirstOrDefaultAsync(res => res.SupplierId.Equals(supplier.SupplierId));

                if (existingSupplier == null)
                {
                    return Problem("No record found");
                }

                // Update client properties
                _context.Entry(existingSupplier).CurrentValues.SetValues(supplier);

                await _context.SaveChangesAsync();

                return Ok(existingSupplier);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);


                var existingSupplier = await _context.Supplier.FirstOrDefaultAsync(res => res.BusinessName.Equals(supplier.BusinessName));

                if (existingSupplier != null)
                {
                    return Problem("Supplier with the same name already exists");
                }

                _context.Supplier.Add(supplier);
                await _context.SaveChangesAsync();

                return Ok(supplier.SupplierId);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{supplierId}")]
        public async Task<IActionResult> DeleteSupplier(int supplierId)
        {
            try
            {
                var user = await _tokenService.GetUserFromTokenAsync(HttpContext);


                var supplier = await _context.Supplier.FindAsync(supplierId);

                if (supplier == null)
                {
                    return NotFound();
                }

                _context.Supplier.Remove(supplier);
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
