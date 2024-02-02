using INVENTARIO.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace INVENTARIO
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options)
        {
        }
        private readonly string? _connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString != null)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {

        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Entry> Entry { get; set; }
        public DbSet<Output> Output { get; set; }
        public DbSet<Supplier> Supplier { get; set; }

        public DbSet<Order> Order { get; set; }
        public DbSet<Ubication> Ubication { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<OrderXProduct> OrderXProduct { get; set; }
    }

}
