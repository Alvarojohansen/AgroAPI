using Application.Dtos.SaleOrderLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.SaleOrder
{
    public class SaleOrderCreateDto
    {
        public int ClientId { get; set; }
        public int SellerId { get; set; }
        public ICollection<SaleOrderLineDto> SaleOrderLines { get; set; } = new List<SaleOrderLineDto>();
    }
}
