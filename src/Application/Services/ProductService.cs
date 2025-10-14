using Application.Dtos.Product;
using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICurrentUserService _currentUser;
        public ProductService(IProductRepository repository, ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
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
            // Obtengo el ID del vendedor desde el servicio de usuario actual
            var sellerId = _currentUser.SellerId
                           ?? throw new UnauthorizedAccessException("Usuario no autenticado");
            var Name = _currentUser.Name
                           ?? throw new UnauthorizedAccessException("Usuario no autenticado");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                Stock = request.Stock,
                SellerId = sellerId
            };
           
            return _repository.AddProduct(product);
            
        }
        public bool UpdateProduct(int id, ProductUpdateRequest request)
        {
            if (request == null)
                throw new AppValidationException("La información del producto no puede ser nula.");

            // Obtenemos el producto existente
            var existingProduct = _repository.GetProductById(id);
            if (existingProduct == null)
                throw new AppValidationException($"No se encontró el producto con ID.");

            // Creamos una nueva instancia del producto actualizada
            var updatedProduct = new Product
            {
                Id = existingProduct.Id,
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                Stock = request.Stock
            };

            // Intentamos actualizar mediante el repositorio
            var result = _repository.UpdateProduct(updatedProduct);

            if (!result)
                throw new AppValidationException("No se pudo actualizar el producto. Intente nuevamente.");

            return true;
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
