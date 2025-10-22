using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SaleOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string OrderCode { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusOrderEnum OrderStatus { get; set; } = StatusOrderEnum.Pending;

        public decimal Total { get; private set; }

        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
        public User Client { get; set; }


        [ForeignKey("SellerId")]
        public int SellerId { get; set; }
        public Seller Seller { get; set; }
        public ICollection<SaleOrderLine> SaleOrderLines { get; set; }
        
        // Método para recalcular el total de la orden
        public void RecalculateTotal()
        {
            Total = SaleOrderLines.Sum(l => l.UnitPrice * l.Quantity);
        }
    }
}
