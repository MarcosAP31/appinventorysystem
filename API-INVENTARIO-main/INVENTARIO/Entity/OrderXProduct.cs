using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace INVENTARIO.Entity
{
    public class OrderXProduct
    {
        [Key]
        public int OrderXProductId { set; get; }
        public int? OrderId { set; get; }
        public int? ProductId { set; get; }
        public int? Quantity { set; get; }
        public double? Subtotal { set; get; }
    }
}