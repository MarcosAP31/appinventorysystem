using INVENTARIO.Entity;

namespace INVENTARIO.Interfaces
{
    public interface ITokenService
    {
        Task<User> GetUserFromTokenAsync(HttpContext httpContext);
        public string GenerateToken(string text);
    }
}
