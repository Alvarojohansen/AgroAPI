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

            // ------Data mocked------
            modelBuilder.Entity<Client>().HasData(
               new Client
               {
                   Id = 1,
                   Name = "Carlos Cliente",
                   Password = "client123",   
                   Email = "carlos@example.com",
                   Address = "Av. Siempre Viva 123",
                   Apartment = "1A",
                   Country = "Argentina",
                   City = "Rosario",
                   Phone = "3415551111",
                   Role = UserRole.Client
               },
               new Client
               {
                   Id = 2,
                   Name = "Sandra ",
                   Password = "seller123",
                   Email = "sandra@example.com",
                   Address = "Calle Comercio 45",
                   Apartment = null,
                   Country = "Argentina",
                   City = "Buenos Aires",
                   Phone = "113334444",
                   Role = UserRole.Client
               }
            );
            modelBuilder.Entity<Seller>().HasData(
              new Seller
              {
                  Id = 5,
                  Name = "juan ",
                  Password = "seller123",
                  Email = "andres@example.com",
                  Address = "Calle Central 9",
                  Apartment = "2B",
                  Country = "Argentina",
                  City = "Córdoba",
                  Phone = "351222333",
                  Role = UserRole.Seller
              }
           );
            modelBuilder.Entity<Admin>().HasData(
              new Admin
              {
                  Id = 4,
                  Name = "Andrés ",
                  Password = "admin123",
                  Email = "andres@example.com",
                  Address = "Calle Central 9",
                  Apartment = "2B",
                  Country = "Argentina",
                  City = "Córdoba",
                  Phone = "351222333",
                  Role = UserRole.Admin
              }
           );


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
