using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }       
        public DbSet<SaleOrder> SaleOrders  { get; set; }
        public DbSet<SaleOrderLine> SaleOrderLines { get; set; }
        

        //Acá estamos llamando al constructor de DbContext que es el que acepta las opciones
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasDiscriminator(u => u.Role)
                .HasValue<User>(UserRole.None)
                .HasValue<Client>(UserRole.Client)
                .HasValue<Seller>(UserRole.Seller)
                .HasValue<Admin>(UserRole.Admin);

            modelBuilder
                .Entity<User>()
                .Property(d =>d.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());
            modelBuilder
                .Entity<Product>()
                .Property(p => p.Category)
                .HasConversion(new EnumToStringConverter<CategoryEnum>());
            modelBuilder
                .Entity<SaleOrder>()
                .Property(so => so.OrderStatus)
                .HasConversion(new EnumToStringConverter<StatusOrderEnum>());

            // Relación entre Cliente y OrdenDeVenta (uno a muchos)
            modelBuilder.Entity<User>()
                .HasMany(c => c.SaleOrders)
                .WithOne(o => o.Client)
                .HasForeignKey(o => o.ClientId);

            // Relación entre OrdenDeVenta y LineaDeVenta (uno a muchos)
            modelBuilder.Entity<SaleOrder>()
                .HasMany(o => o.SaleOrderLines)
                .WithOne()
                .HasForeignKey(l => l.SaleOrderId);

            modelBuilder.Entity<SaleOrderLine>()
                .HasOne(sol => sol.Product)
                .WithMany() //vacío porque no me interesa establecer esa relación
                .HasForeignKey(sol => sol.ProductId);

            //Relación entre vendedor y producto
            modelBuilder.Entity<Seller>()
                .HasMany(u => u.Products)
                .WithOne(p => p.Seller)
                .HasForeignKey(f => f.SellerId);
            
        }
    }
}
