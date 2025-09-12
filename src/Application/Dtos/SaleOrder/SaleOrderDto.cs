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
        public string OrderCode { get; set; }
        public int ClientId { get; set; }
        public DateTime Date { get; set; }
        public int SellerId { get; set; }
        public ICollection<SaleOrderLineDto> SaleOrderLines { get; set; } = new List<SaleOrderLineDto>();
        public decimal Total { get; set; }
    }
}
