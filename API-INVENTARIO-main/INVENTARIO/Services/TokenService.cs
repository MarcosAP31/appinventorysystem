using INVENTARIO.Entity;
using Microsoft.EntityFrameworkCore;

namespace INVENTARIO.Services
{
    public class TokenService
    {
        private readonly cifrado _cifrado;

        public TokenService(cifrado cifrado)
        {
            _cifrado = cifrado ?? throw new ArgumentNullException(nameof(cifrado));
        }

        public async Task<User> GetUserFromTokenAsync(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"];
            var vtoken = _cifrado.validarToken(token);

            if (vtoken == null)
            {
                throw new UnauthorizedAccessException("The token isn't valid!");
            }

            using (var context = new SampleContext("server = localhost; database = inventory;User ID=marcos;Password=marcos123;"))
            {
                return await context.User
                    .FirstOrDefaultAsync(res => res.Email.Equals(vtoken[1]) && res.Password.Equals(vtoken[2]));
            }
        }
    }
}
