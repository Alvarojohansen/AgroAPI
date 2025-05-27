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
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .Property(d =>d.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());
            modelBuilder
                .Entity<Product>()
                .Property(p => p.Category)
                .HasConversion(new EnumToStringConverter<CategoryEnum>());
        }
    }
}
