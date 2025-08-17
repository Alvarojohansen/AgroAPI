using Application.Dtos.Product;
using Application.Dtos.User;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public List<Product> GetAllProducts()
        {
            return _repository.GetAllProducts();
        }

        public Product GetProductById(int id)
        {
            var products = _repository.GetAllProducts();
            return products.FirstOrDefault(p => p.Id == id);
        }
        public Product AddProduct(ProductRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                stock = request.Stock

            };
            return _repository.AddProduct(product);
        }
        public bool UpdateProduct(int id, ProductUpdateRequest request)
        {
            var product = _repository.GetProductById(id);
            if (product != null)
            {
                product.Name = request.Name;
                product.Description = request.Description;
                product.Category = request.Category;
                product.Price = request.Price;
                product.stock = request.stock;

                _repository.UpdateProduct(product);
                return true;
            }
            return false;

        }
        public bool DeleteProduct(int id)
        {
            var product = _repository.GetProductById(id);
            if (product != null)
            {
                _repository.DeleteProduct(product.Id);
                return true;
            }
            return false;
        }
    }   
}
