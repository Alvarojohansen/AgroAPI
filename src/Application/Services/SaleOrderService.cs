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

        public SaleOrderService(ISaleOrderRepository repository, ICurrentUserService currentUser, IProductRepository productRepository)
        {
            _repository = repository;
            _currentUser = currentUser;
            _productRepository = productRepository;
        }
        public List<SaleOrder> GetSaleOrders()
        {
            return _repository.GetSaleOrders();
        }
        public SaleOrder? GetSaleOrderById(int id)
        {
            return _repository.GetSaleOrderById(id);
        }

        public List<SaleOrder> GetSaleOrdersByClientId(int clientId)
        {
            
            return _repository.GetSaleOrderByClientId(clientId);
        }
        public List<SaleOrder> GetSaleOrdersBySellerId(int sellerId)
        {
           
            return _repository.GetSaleOrderBySellerId(sellerId);
        }

        public SaleOrder AddSaleOrder(SaleOrderCreateDto saleOrder)
        {
            var clientId = _currentUser.ClientId
                ?? throw new UnauthorizedAccessException("Usuario no autenticado");

            // Generar código incremental
            var lastOrder = _repository.GetLastOrder();
            int nextNumber = 1;

            if (lastOrder != null && !string.IsNullOrEmpty(lastOrder.OrderCode))
            {
                var parts = lastOrder.OrderCode.Split('-');
                if (parts.Length > 1 && int.TryParse(parts[1], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            var newOrderCode = $"ORD-{nextNumber:D5}";

            // Crear la orden
            var newSaleOrder = new SaleOrder
            {
                OrderCode = newOrderCode,
                Date = DateTime.Now,
                OrderStatus = StatusOrderEnum.Pending,
                ClientId = clientId,
                SellerId = saleOrder.SellerId,
            };

            // Agregar líneas
            foreach (var lineDto in saleOrder.SaleOrderLines)
            {
                var product = _productRepository.GetProductById(lineDto.ProductId);
                if (product == null)
                    throw new AppValidationException($"Producto con ese ID no existe, intenta con otro");

                var line = new SaleOrderLine
                {
                    ProductId = product.Id,
                    Quantity = lineDto.Quantity,
                    UnitPrice = product.Price 
                };

                newSaleOrder.SaleOrderLines.Add(line);
            }

            // Calcular total antes de guardar
            newSaleOrder.RecalculateTotal();
            return _repository.AddSaleOrder(newSaleOrder);
        }

        public bool UpdateSaleOrder(int id, SaleOrderDto saleOrder)
        {
            var existingSaleOrder = _repository.GetSaleOrderById(id);
            if (existingSaleOrder != null)
            {
                existingSaleOrder.Date = saleOrder.Date;
                existingSaleOrder.ClientId = saleOrder.ClientId;
                existingSaleOrder.SellerId = saleOrder.SellerId;
                existingSaleOrder.OrderStatus = saleOrder.Status;

                // Mapeo correcto de DTO → Entidad
                existingSaleOrder.SaleOrderLines = saleOrder.SaleOrderLines
                    .Select(lineDto => new SaleOrderLine
                    {
                        ProductId = lineDto.ProductId,
                        Quantity = lineDto.Quantity,
                        // Si tu entidad SaleOrderLine tiene SaleOrderId o Id, no los pongas aquí,
                        // EF se encarga de relacionarlos al guardar.
                    })
                    .ToList();

                _repository.UpdateSaleOrder(existingSaleOrder);
                return true;
            }

            return false;
        }
        public void DeleteSaleOrder(int id)
        {
            _repository.DeleteSaleOrder(id);
        }

    }
}
