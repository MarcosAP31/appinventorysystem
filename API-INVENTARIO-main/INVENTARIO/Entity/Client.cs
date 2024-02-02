using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace INVENTARIO.Entity
{
    public class Client
    {
        [Key]
        public int ClientId { set; get; }
        public string? Name { set; get; }
        public string? LastName { set; get; }
        public string? Birthday { set; get; }
        public string? Sex { set; get; }
        public string? Department { set; get; }
        public string? Province { set; get; }
        public string? District { set; get; }
        public string? Direction { set; get; }
        public string? Phone { set; get; }
        public string? Email { set; get; }
        public string? Image { set; get; }
    }
}
