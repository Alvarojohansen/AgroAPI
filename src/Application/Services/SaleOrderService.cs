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
        public SaleOrderService(ISaleOrderRepository repository)
        {
            _repository = repository;
        }
        public List<SaleOrder> GetSaleOrders()
        {
            return _repository.GetSaleOrders();
        }

        public List<SaleOrder> GetSaleOrdersByClientId(int clientId)
        {
            var saleOrders = _repository.GetSaleOrders();
            return saleOrders.Where(so => so.ClientId == clientId).ToList();
        }
        public List<SaleOrder> GetSaleOrdersBySellerId(int sellerId)
        {
            var saleOrders = _repository.GetSaleOrders();
            return saleOrders.Where(so => so.SellerId == sellerId).ToList();
        }

        public SaleOrder AddSaleOrder(SaleOrderDto saleOrder)
        {
            var newSaleOrder = new SaleOrder
            {
                OrderCode = saleOrder.OrderCode,
                Date = saleOrder.Date,
                Total = saleOrder.Total,
                ClientId = saleOrder.ClientId,
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
