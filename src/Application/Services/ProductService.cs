using Application.Dtos.Product;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Collections.Generic;

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

       
        public List<Product> GetAllProducts() => _repository.GetAllProducts();

      
        public Product GetProductById(int id)
        {
            return _repository.GetProductById(id)
                ?? throw new NotFoundException($"No se encontró el producto con ID {id}.");
        }

        
        public Product AddProduct(ProductRequest request)
        {
            if (request == null)
                throw new AppValidationException("La información del producto no puede ser nula.");

            var sellerId = _currentUser.SellerId
                ?? throw new UnauthorizedAccessException("Usuario no autenticado.");

            if (_currentUser.Role != UserRole.Seller && _currentUser.Role != UserRole.Admin)
                throw new AppValidationException("Solo un vendedor o administrador puede crear productos.");

            
            var product = new Product(
                request.Name,
                request.Description,
                request.ImageUrl,
                request.Category,
                request.Price,
                request.Stock,
                sellerId
            );

            return _repository.AddProduct(product);
        }

       
        public bool UpdateProduct(int id, ProductUpdateRequest request)
        {
            var product = _repository.GetProductById(id)
                ?? throw new NotFoundException($"No se encontró el producto con ID {id}.");

            if (_currentUser.Role == UserRole.Client)
                throw new AppValidationException("Los clientes no pueden modificar productos.");

            if (_currentUser.Role == UserRole.Seller && product.SellerId != _currentUser.SellerId)
                throw new UnauthorizedAccessException("No puedes modificar un producto que no te pertenece.");


            product.UpdatePrice(request.Price);

            if (request.Stock > product.Stock)
                product.IncreaseStock(request.Stock - product.Stock);
            else if (request.Stock < product.Stock)
                product.DecreaseStock(product.Stock - request.Stock);

            //Actualizar campos editables
            product.Name = request.Name;
            product.Description = request.Description;
            product.ImageUrl = request.ImageUrl;
            product.Category = request.Category;

            _repository.UpdateProduct(product);
            return true;
        }


        public void UpdateProductStock(int id, ProductStockUpdateDto dto)
        {
            var product = GetProductById(id);

            if (_currentUser.Role == UserRole.Client)
                throw new UnauthorizedAccessException("Los clientes no pueden modificar el stock.");

            if (_currentUser.Role == UserRole.Seller && product.SellerId != _currentUser.SellerId)
                throw new UnauthorizedAccessException("No puedes modificar un producto que no te pertenece.");

            var difference = dto.Stock - product.Stock;
            if (difference == 0)
                throw new AppValidationException("El nuevo stock es igual al actual.");

            if (difference > 0)
                product.IncreaseStock(difference);
            else
                product.DecreaseStock(-difference);

            _repository.UpdateProduct(product);
        }

        
        public void UpdateProductPrice(int id, ProductPriceUpdateDto dto)
        {
            var product = GetProductById(id);

            if (_currentUser.Role == UserRole.Client)
                throw new UnauthorizedAccessException("Los clientes no pueden modificar el precio.");

            if (_currentUser.Role == UserRole.Seller && product.SellerId != _currentUser.SellerId)
                throw new UnauthorizedAccessException("No puedes modificar un producto que no te pertenece.");

            product.UpdatePrice(dto.Price);
            _repository.UpdateProduct(product);
        }

     
        public bool DeleteProduct(int id)
        {
            var product = GetProductById(id);

            if (_currentUser.Role == UserRole.Client)
                throw new AppValidationException("Los clientes no pueden eliminar productos.");

            if (_currentUser.Role == UserRole.Seller && product.SellerId != _currentUser.SellerId)
                throw new UnauthorizedAccessException("No puedes eliminar un producto que no te pertenece.");

            _repository.DeleteProduct(id);
            return true;
        }
    }
}
