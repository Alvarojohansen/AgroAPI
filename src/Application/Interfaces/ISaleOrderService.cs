using Application.Dtos.SaleOrder;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISaleOrderService
    {
        List<SaleOrder> GetSaleOrders();
        List<SaleOrder> GetSaleOrdersByClientId(int clientId);
        List<SaleOrder> GetSaleOrdersBySellerId(int sellerId);
        SaleOrder AddSaleOrder(SaleOrderDto saleOrder);
        bool UpdateSaleOrder(int id, SaleOrderDto saleOrder);
        void DeleteSaleOrder(int id);
    }
}
