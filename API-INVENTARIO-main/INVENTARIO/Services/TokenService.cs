using INVENTARIO.Entity;
using INVENTARIO.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INVENTARIO.Services
{
    public class TokenService:ITokenService
    {
        private readonly cifrado _cifrado;
        private readonly SampleContext _context;

        public TokenService(cifrado cifrado, SampleContext context)
        {
            _cifrado = cifrado ?? throw new ArgumentNullException(nameof(cifrado));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Users> GetUserFromTokenAsync(HttpContext httpContext)
        {
            string token = httpContext.Request.Headers["Authorization"];
            Console.WriteLine(token);
            // Verificar si la cadena comienza con "Bearer "
            if (token != null && token.StartsWith("Bearer "))
            {
                // Quitar "Bearer " y obtener solo la parte del token
                token = token.Substring("Bearer ".Length);
                
            }

            

            var vtoken = _cifrado.validarToken(token);

            if (vtoken == null)
            {
                throw new UnauthorizedAccessException("The token isn't valid!");
            }

            return await _context.Users
                .FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
        }
        public string GenerateToken(string text)
        {
            return _cifrado.EncryptStringAES(text);
        }

        public Task<Users> GetUserFromToken(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }

        public string GenerateTokenAsync(string text)
        {
            throw new NotImplementedException();
        }
    }
}
