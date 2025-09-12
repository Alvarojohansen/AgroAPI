using Application.Dtos.SaleOrder;
using Application.Interfaces;
using Domain.Entities;
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

        public SaleOrderService(ISaleOrderRepository repository, ICurrentUserService currentUser)
        {            
            _repository = repository;
            _currentUser = currentUser;
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

        public SaleOrder AddSaleOrder(SaleOrderDto saleOrder)
        {
            var clientId = _currentUser.ClientId
                ?? throw new UnauthorizedAccessException("Usuario no autenticado");

            var newSaleOrder = new SaleOrder
            {
                OrderCode = saleOrder.OrderCode,
                Date = DateTime.Now,
                Total = saleOrder.Total,
                ClientId = clientId,
                SellerId = saleOrder.SellerId,
                SaleOrderLines = saleOrder.SaleOrderLines
                    .Select(lineDto => new SaleOrderLine
                    {
                        ProductId = lineDto.ProductId,
                        Quantity = lineDto.Quantity
                    }).ToList()
            };

            return _repository.AddSaleOrder(newSaleOrder);
        }

        public bool UpdateSaleOrder(int id, SaleOrderDto saleOrder)
        {
            var existingSaleOrder = _repository.GetSaleOrderById(id);
            if (existingSaleOrder != null)
            {
                existingSaleOrder.OrderCode = saleOrder.OrderCode;
                existingSaleOrder.SaleOrderLines = (ICollection<SaleOrderLine>)saleOrder.SaleOrderLines;
                existingSaleOrder.Date = saleOrder.Date;
                existingSaleOrder.Total = saleOrder.Total;
                existingSaleOrder.ClientId = saleOrder.ClientId;
                existingSaleOrder.SellerId = saleOrder.SellerId;
               
                _repository.UpdateSaleOrder(existingSaleOrder);
                return true;
            }
                return false;
        }
        public void DeleteSaleOrder(int id)
        {
            _repository.DeleteSaleOrder(id);
        }

        public bool UpdateSaleOrder(int id, SaleOrder saleOrder)
        {
            throw new NotImplementedException();
        }
    }
}
