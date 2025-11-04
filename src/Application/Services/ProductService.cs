using Application.Dtos.Product;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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

        // 🔹 Obtener todos los productos
        public List<Product> GetAllProducts()
        {
            return _repository.GetAllProducts();
        }

        // 🔹 Obtener producto por ID
        public Product GetProductById(int id)
        {
            var product = _repository.GetProductById(id)
                ?? throw new NotFoundException($"No se encontró el producto con ID {id}.");

            return product;
        }

        // 🔹 Agregar nuevo producto (solo Seller o Admin)
        public Product AddProduct(ProductRequest request)
        {
            if (request == null)
                throw new AppValidationException("La información del producto no puede ser nula.");

            var sellerId = _currentUser.SellerId
                ?? throw new UnauthorizedAccessException("Usuario no autenticado.");

            if (_currentUser.Role != UserRole.Seller && _currentUser.Role != UserRole.Admin)
                throw new AppValidationException("Solo un vendedor o administrador puede crear productos.");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                ImageUrl = request.ImageUrl,
                Price = request.Price,
                Stock = request.Stock,
                SellerId = sellerId
            };

            return _repository.AddProduct(product);
        }

        // 🔹 Actualizar producto (solo Seller propietario o Admin)
        public bool UpdateProduct(int id, ProductUpdateRequest request)
        {
            if (request == null)
                throw new AppValidationException("La información del producto no puede ser nula.");

            var existingProduct = _repository.GetProductById(id)
                ?? throw new NotFoundException($"No se encontró el producto con ID {id}.");

            if (_currentUser.Role == UserRole.Client)
                throw new AppValidationException("Los clientes no pueden modificar productos.");

            if (_currentUser.Role == UserRole.Seller && existingProduct.SellerId != _currentUser.SellerId)
                throw new UnauthorizedAccessException("No puedes modificar un producto que no te pertenece.");

            // 🔹 Usar métodos de dominio
            existingProduct.UpdatePrice(request.Price);

            if (request.Stock > existingProduct.Stock)
                existingProduct.IncreaseStock(request.Stock - existingProduct.Stock);
            else if (request.Stock < existingProduct.Stock)
                existingProduct.DecreaseStock(existingProduct.Stock - request.Stock);

            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.ImageUrl = request.ImageUrl;
            existingProduct.Category = request.Category;

            _repository.UpdateProduct(existingProduct);
            return true;
        }
        //Actualizar Campos específicos del producto (solo Seller propietario o Admin)
        public void UpdateProductStock(int id, ProductStockUpdateDto dto)
        {
            var product = _repository.GetProductById(id)
                ?? throw new NotFoundException($"No se encontró el producto con ID {id}.");

            if (_currentUser.Role == UserRole.Client)
                throw new UnauthorizedAccessException("Los clientes no pueden modificar el stock.");

            if (_currentUser.Role == UserRole.Seller && product.SellerId != _currentUser.SellerId)
                throw new UnauthorizedAccessException("No puedes modificar un producto que no te pertenece.");

            // 🔹 Calcular la diferencia
            var difference = dto.Stock - product.Stock;

            if (difference == 0)
                throw new AppValidationException("El nuevo stock es igual al actual.");

            if (difference > 0)
                product.IncreaseStock(difference);
            else
                product.DecreaseStock(-difference);

            _repository.UpdateProduct(product);
            return;
        }

        public void UpdateProductPrice(int id, ProductPriceUpdateDto dto)
        {
            var product = _repository.GetProductById(id)
                ?? throw new NotFoundException($"No se encontró el producto con ID {id}.");

            if (_currentUser.Role == UserRole.Client)
                throw new AppValidationException("Los clientes no pueden modificar el precio.");

            if (_currentUser.Role == UserRole.Seller && product.SellerId != _currentUser.SellerId)
                throw new UnauthorizedAccessException("No puedes modificar un producto que no te pertenece.");

            product.UpdatePrice(dto.Price);

            _repository.UpdateProduct(product);
            return ;
        }

        // 🔹 Eliminar producto (solo Seller propietario o Admin)
        public bool DeleteProduct(int id)
        {
            var existingProduct = _repository.GetProductById(id)
                ?? throw new NotFoundException($"No se encontró el producto con ID {id}.");

            if (_currentUser.Role == UserRole.Client)
                throw new AppValidationException("Los clientes no pueden eliminar productos.");

            if (_currentUser.Role == UserRole.Seller && existingProduct.SellerId != _currentUser.SellerId)
                throw new UnauthorizedAccessException("No puedes eliminar un producto que no te pertenece.");

            _repository.DeleteProduct(id);
            return true;
        }
    }
}
