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
        SaleOrder? GetSaleOrderById(int id);
        SaleOrder AddSaleOrder(SaleOrder saleOrder);
        bool UpdateSaleOrder(SaleOrder saleOrder);
        void DeleteSaleOrder(int id);
    }
}
