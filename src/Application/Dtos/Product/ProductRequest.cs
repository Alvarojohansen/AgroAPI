using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.Product
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CategoryEnum Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
