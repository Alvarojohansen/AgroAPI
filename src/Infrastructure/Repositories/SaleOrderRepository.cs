using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SaleOrderRepository : ISaleOrderRepository
    {
        private readonly ApplicationContext _context;
        public SaleOrderRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<SaleOrder> GetSaleOrders()
        {
            return _context.SaleOrders.ToList();
        }
        public SaleOrder? GetSaleOrderById(int id)
        {
            return _context.SaleOrders
                .Include(so => so.SaleOrderLines) // si querés traer las líneas también
                .FirstOrDefault(so => so.Id == id);
        }
        public List<SaleOrder> GetSaleOrderBySellerId(int sellerId)
        {
            return _context.SaleOrders
                .Where(o => o.SellerId == sellerId)
                .Include(o => o.SaleOrderLines) // opcional: incluir las líneas
                .ThenInclude(l => l.Product)    // opcional: incluir productos
                .ToList();
        }

        public List<SaleOrder> GetSaleOrderByClientId(int clientId)
        {
            return _context.SaleOrders
                .Where(o => o.ClientId == clientId)
                .Include(o => o.SaleOrderLines)
                .ThenInclude(l => l.Product)
                .ToList();
        }

        public SaleOrder AddSaleOrder(SaleOrder saleOrder)
        {
            _context.SaleOrders.Add(saleOrder);
            _context.SaveChanges();
            return saleOrder;
        }
        public bool UpdateSaleOrder(SaleOrder saleOrder)
        {
            _context.Entry(saleOrder).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }
        public void DeleteSaleOrder(int id) 
        {
            var saleOrder =  _context.SaleOrders.Find(id);
            if (saleOrder != null)
            {
                _context.SaleOrders.Remove(saleOrder);
                _context.SaveChanges();

            }
        }

    }
}
