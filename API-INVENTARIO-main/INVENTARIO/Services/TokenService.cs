using INVENTARIO.Entity;
using Microsoft.EntityFrameworkCore;

namespace INVENTARIO.Services
{
    public class TokenService
    {
        private readonly cifrado _cifrado;
        private readonly SampleContext _context;

        public TokenService(cifrado cifrado, SampleContext context)
        {
            _cifrado = cifrado ?? throw new ArgumentNullException(nameof(cifrado));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> GetUserFromTokenAsync(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"];
            var vtoken = _cifrado.validarToken(token);

            if (vtoken == null)
            {
                throw new UnauthorizedAccessException("The token isn't valid!");
            }

            return await _context.User
                .FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
        }
    }
}
