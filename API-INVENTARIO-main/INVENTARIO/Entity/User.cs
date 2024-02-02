using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace INVENTARIO.Entity
{
    public class Users
    {
        [Key]
        public int UserId { set; get; }
        public string? Code { set; get; }
        public string? Name { set; get; }
        public string? LastName { set; get; }
        public int? Phone { set; get; }
        public string? Position { set; get; }
        public string? Role { set; get; }
        public string? Email { set; get; }
        public string? Password { set; get; }
        
 
    }
}
