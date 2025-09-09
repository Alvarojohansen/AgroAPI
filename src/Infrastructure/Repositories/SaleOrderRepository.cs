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
            return _context.SaleOrders.Find(id);
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
