using Application.Dtos.SaleOrder;
using Domain.Entities;
using Domain.Enum;
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
        SaleOrder? GetSaleOrderById(int id);
        List<SaleOrder> GetSaleOrdersByClientId(int clientId);
        List<SaleOrder> GetSaleOrdersBySellerId(int sellerId);
        SaleOrder AddSaleOrder(SaleOrderCreateDto saleOrder);
        bool UpdateSaleOrder(int id, SaleOrderDto saleOrder);
        bool UpdateSaleOrderStatus(int id, SaleOrderStateUpdateDto saleOrderState);
        bool CompleteSaleOrder(int id);
        bool CancelSaleOrder(int id);
        bool RemoveProductFromOrder(int orderId, int productId);
        void DeleteSaleOrder(int id);
        
    }
}
