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
    public class SaleOrderLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal LineTotal => UnitPrice * Quantity;

        [ForeignKey("SaleOrder")]
        public int SaleOrderId { get; set; }
       
    }
}
