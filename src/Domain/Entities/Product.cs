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
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CategoryEnum Category { get; set; }
        public decimal Price { get; set; }
        public int stock { get; set; }
        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public Seller Seller { get; set; }
    }
}
