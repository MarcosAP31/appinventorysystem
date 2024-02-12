using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace INVENTARIO.Entity
{
    public class Product
    {
        [Key]
        public int ProductId { set; get; }
        public string? SKU { set; get; }
        public string? Name { set; get; }
        public string? Kind { set; get; }
        public string? Label { set; get; }
        public double? Price { set; get; }
        public string? UnitMeasurement { set; get; }
    }
}