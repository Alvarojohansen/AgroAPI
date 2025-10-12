using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISaleOrderRepository
    {
        List<SaleOrder> GetSaleOrders();
        List<SaleOrder> GetSaleOrderByClientId(int customerId);
        List<SaleOrder> GetSaleOrderBySellerId(int sellerId);
        SaleOrder? GetSaleOrderById(int id);
        SaleOrder AddSaleOrder(SaleOrder saleOrder);
        bool UpdateSaleOrder(SaleOrder saleOrder);
        void DeleteSaleOrder(int id);
        SaleOrder? GetLastOrder();
    }
}
