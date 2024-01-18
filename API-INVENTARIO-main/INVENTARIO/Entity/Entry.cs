using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace INVENTARIO.Entity
{
    public class Entry
    {
        [Key]
        public int EntryId { set; get; }
        public string? Date { set; get; }
        public int? Amount { set; get; }
        public int? ProductId { set; get; }
        public int? UbicationId { set; get; }
        public int? UserId { set; get; }
    }
}