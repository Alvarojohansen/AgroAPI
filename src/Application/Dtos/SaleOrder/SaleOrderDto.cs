using Application.Dtos.SaleOrderLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.SaleOrder
{
    public class SaleOrderDto
    {
        public int SellerId { get; set; }
        public ICollection<SaleOrderLineDto> SaleOrderLines { get; set; }
        public decimal Total { get; set; }
        public int ClientId { get; set; }
        public string OrderCode { get; internal set; }
        public DateTime Date { get; internal set; }
    }
}
