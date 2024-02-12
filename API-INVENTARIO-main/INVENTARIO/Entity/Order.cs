using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace INVENTARIO.Entity
{
    public class Order
    {
        [Key]
        public int OrderId { set; get; }
        public DateTime OrderDate { set; get; }
        public DateTime? ReceptionDate { set; get; }
        public DateTime? DispatchedDate { set; get; }
        public DateTime? DeliveryDate { set; get; }
        public double? TotalPrice { set; get; }
        public string? Seller { set; get; }
        public string? DeliveryMan { set; get; }
        public string? Status { set; get; }
    }
}
