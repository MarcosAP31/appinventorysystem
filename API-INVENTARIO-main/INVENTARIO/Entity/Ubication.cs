using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace INVENTARIO.Entity
{
    public class Ubication
    {
        [Key]
        public int UbicationId { set; get; }
        public string? Name { set; get; }
        public int? Description { set; get; }
        public int? Amount { set; get; }
        public int? Capacity { set; get; }
    }
}
