using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Application.Dtos.Product
{
    public class ProductUpdateRequest
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public CategoryEnum Category { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Price { get; set; }        

        public int Stock { get; set; }
    }
}

