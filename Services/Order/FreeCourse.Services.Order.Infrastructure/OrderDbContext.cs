using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }

        public DbSet<Domain.Order.Aggragate.Order> Orders { get; set; }
        public DbSet<Domain.Order.Aggragate.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Order.Aggragate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
            modelBuilder.Entity<Domain.Order.Aggragate.OrderItem>().ToTable("Orders", DEFAULT_SCHEMA);

            modelBuilder.Entity<Domain.Order.Aggragate.OrderItem>().Property(i => i.Price).HasColumnType("decimal(18, 2)");

            // Domain içerisinde ORM araçları ile ilgili kod yazmıyoruz. Böylece sızıntı olmamış oluyor. Order içerisindeki Addresin owned olduğunu burada tanımlıyoruz.
            modelBuilder.Entity<Domain.Order.Aggragate.Order>().OwnsOne(i => i.Address).WithOwner();

            base.OnModelCreating(modelBuilder);
        }
    }
}
