using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Application.Dtos.Product
{
    public class ProductUpdateRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? ImageUrl { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public CategoryEnum Category { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "El stock debe ser mayor que 0.")]
        public int Stock { get; set; }
    }
}

