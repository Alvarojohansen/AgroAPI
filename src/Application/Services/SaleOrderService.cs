using Application.Dtos.SaleOrder;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SaleOrderService : ISaleOrderService
    {
        private readonly ISaleOrderRepository _repository;
        private readonly ICurrentUserService _currentUser;
        private readonly IProductRepository _productRepository;

        public SaleOrderService(ISaleOrderRepository repository,
                                ICurrentUserService currentUser,
                                IProductRepository productRepository)
        {
            _repository = repository;
            _currentUser = currentUser;
            _productRepository = productRepository;
        }

        public List<SaleOrder> GetSaleOrders() => _repository.GetSaleOrders();

        public SaleOrder? GetSaleOrderById(int id) => _repository.GetSaleOrderById(id);

        public List<SaleOrder> GetSaleOrdersByClientId(int clientId)
            => _repository.GetSaleOrderByClientId(clientId);

        public List<SaleOrder> GetSaleOrdersBySellerId(int sellerId)
            => _repository.GetSaleOrderBySellerId(sellerId);

        public SaleOrder AddSaleOrder(SaleOrderCreateDto saleOrder)
        {
            if (saleOrder == null)
                throw new AppValidationException("Los datos de la orden no pueden ser nulos.");

            var clientId = _currentUser.ClientId
                ?? throw new UnauthorizedAccessException("Usuario no autenticado.");

            if (saleOrder.SaleOrderLines == null || saleOrder.SaleOrderLines.Count == 0)
                throw new AppValidationException("La orden debe contener al menos un producto.");

            // Generar código incremental
            var lastOrder = _repository.GetLastOrder();
            int nextNumber = 1;

            if (lastOrder != null && !string.IsNullOrEmpty(lastOrder.OrderCode))
            {
                var parts = lastOrder.OrderCode.Split('-');
                if (parts.Length > 1 && int.TryParse(parts[1], out int lastNumber))
                    nextNumber = lastNumber + 1;
            }

            var newOrderCode = $"ORD-{nextNumber:D5}";

            
            var newSaleOrder = new SaleOrder(clientId, saleOrder.SellerId, StatusOrderEnum.Pending)
            {
                OrderCode = newOrderCode
            };

            
            foreach (var lineDto in saleOrder.SaleOrderLines)
            {
                var product = _productRepository.GetProductById(lineDto.ProductId)
                    ?? throw new NotFoundException($"Producto con ID {lineDto.ProductId} no existe.");

                newSaleOrder.AddLine(product, lineDto.Quantity);

                
                _productRepository.UpdateProduct(product);
            }

            
            return _repository.AddSaleOrder(newSaleOrder);
        }

        public bool UpdateSaleOrder(int id, SaleOrderDto saleOrder)
        {
            var existingSaleOrder = _repository.GetSaleOrderById(id)
                ?? throw new NotFoundException($"No se encontró la orden con ID {id}.");

            if (existingSaleOrder.OrderStatus != StatusOrderEnum.Pending)
                throw new AppValidationException("Solo se pueden modificar órdenes pendientes.");

            
            foreach (var oldLine in existingSaleOrder.SaleOrderLines)
            {
                var oldProduct = _productRepository.GetProductById(oldLine.ProductId);
                if (oldProduct != null)
                {
                    oldProduct.IncreaseStock(oldLine.Quantity);
                    _productRepository.UpdateProduct(oldProduct);
                }
            }

            
            existingSaleOrder.SaleOrderLines.Clear();

            
            foreach (var lineDto in saleOrder.SaleOrderLines)
            {
                var product = _productRepository.GetProductById(lineDto.ProductId)
                    ?? throw new NotFoundException($"Producto con ID {lineDto.ProductId} no existe.");

                existingSaleOrder.AddLine(product, lineDto.Quantity);
                _productRepository.UpdateProduct(product);
            }

            
            existingSaleOrder.Date = saleOrder.Date;
            existingSaleOrder.ChangeStatus(saleOrder.Status);

            _repository.UpdateSaleOrder(existingSaleOrder);
            return true;
        }

        public bool UpdateSaleOrderStatus(int id, SaleOrderStateUpdateDto status)
        {
            var existingSaleOrder = _repository.GetSaleOrderById(id)
                ?? throw new NotFoundException($"No se encontró la orden con ID {id}.");

            existingSaleOrder.ChangeStatus(status.OrderStatus);
            _repository.UpdateSaleOrder(existingSaleOrder);
            return true;
        }

        public void DeleteSaleOrder(int id)
        {
            _repository.DeleteSaleOrder(id);
        }
        // Método para eliminar un producto de una orden
        public bool RemoveProductFromOrder(int orderId, int productId)
        {
            var existingSaleOrder = _repository.GetSaleOrderById(orderId)
                ?? throw new NotFoundException($"No se encontró la orden con ID {orderId}.");

            if (existingSaleOrder.OrderStatus != StatusOrderEnum.Pending)
                throw new AppValidationException("Solo se pueden modificar órdenes pendientes.");

            var product = _productRepository.GetProductById(productId)
                ?? throw new NotFoundException($"No se encontró el producto con ID {productId}.");
            
            existingSaleOrder.RemoveLine(product);          
            _productRepository.UpdateProduct(product);           
            _repository.UpdateSaleOrder(existingSaleOrder);

            return true;
        }

        public bool CancelSaleOrder(int id)
        {
            var existingSaleOrder = _repository.GetSaleOrderById(id)
                ?? throw new NotFoundException($"No se encontró la orden con ID {id}.");

            if (existingSaleOrder.OrderStatus == StatusOrderEnum.Completed)
                throw new AppValidationException("No se puede cancelar una orden completada.");

            if (existingSaleOrder.OrderStatus == StatusOrderEnum.Cancelled)
                throw new AppValidationException("La orden ya está cancelada.");

            
            foreach (var line in existingSaleOrder.SaleOrderLines)
            {
                var product = _productRepository.GetProductById(line.ProductId);
                if (product != null)
                {
                    product.IncreaseStock(line.Quantity); 
                    _productRepository.UpdateProduct(product);
                }
            }

            
            existingSaleOrder.ChangeStatus(StatusOrderEnum.Cancelled);

            _repository.UpdateSaleOrder(existingSaleOrder);

            return true;
        }
        public bool CompleteSaleOrder(int id)
        {
            var existingSaleOrder = _repository.GetSaleOrderById(id)
                ?? throw new NotFoundException($"No se encontró la orden con ID {id}.");

            existingSaleOrder.Complete(); 
            _repository.UpdateSaleOrder(existingSaleOrder);

            return true;
        }
    }
}
