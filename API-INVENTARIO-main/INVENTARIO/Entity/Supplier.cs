using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace INVENTARIO.Entity
{
    public class Supplier
    {
        [Key]
        public int SupplierId { set; get; }
        public int? RUC { set; get; }
        public string? BusinessName { set; get; }
        public string? TradeName { set; get; }
        public string? Kind { set; get; }
        public string? Department { set; get; }
        public string? Province { set; get; }
        public string? District { set; get; }
        public string? Direction { set; get; }
        public int? Phone { set; get; }
        public string? Email { set; get; }
    }
}