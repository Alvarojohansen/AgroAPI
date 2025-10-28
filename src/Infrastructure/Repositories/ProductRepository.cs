using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationContext _context;
        public ProductRepository(ApplicationContext context)
        {
            _context = context;
        }
        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
        public Product? GetProductById(int Id)
        {
            return _context.Products.Find(Id);
        }

        public Product? AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
           
            return product; 
        }

        public bool UpdateProduct(Product product)
        {
            if (product == null)
                return false;

            var existingProduct = _context.Products.Find(product.Id);
            if (existingProduct == null)
                return false;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Category = product.Category;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;

            return _context.SaveChanges() > 0;
        }

        public bool DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }

        
    }
}
