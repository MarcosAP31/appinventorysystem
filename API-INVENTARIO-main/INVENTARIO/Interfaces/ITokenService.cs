using INVENTARIO.Entity;

namespace INVENTARIO.Interfaces
{
    public interface ITokenService
    {
        Task<Users> GetUserFromTokenAsync(HttpContext httpContext);
        public string GenerateToken(string text);
    }
}
