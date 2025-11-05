using Domain.Enum;
using Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get;  set; } = string.Empty;

        [Url(ErrorMessage = "URL not valid.")]
        public string? ImageUrl { get;  set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CategoryEnum Category { get;  set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Price { get;  set; }

        [Range(0, double.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public Seller Seller { get; set; }

        //Constructor vacío 
        protected Product() { }

        //Constructor de dominio
        public Product(string name, string description, string? imageUrl, CategoryEnum category, decimal price, int stock, int sellerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new AppValidationException("El nombre del producto es obligatorio.");

            if (price <= 0)
                throw new AppValidationException("El precio debe ser mayor que cero.");

            if (stock < 0)
                throw new AppValidationException("El stock no puede ser negativo.");

            Name = name;
            Description = description ?? string.Empty;
            ImageUrl = imageUrl;
            Category = category;
            Price = price;
            Stock = stock;
            SellerId = sellerId;
        }

        //Métodos de dominio
        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new AppValidationException("La cantidad debe ser mayor que cero.");

            if (Stock < quantity)
                throw new AppValidationException($"Stock insuficiente. Disponible: {Stock}");

            Stock -= quantity;
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new AppValidationException("La cantidad debe ser mayor que cero.");

            Stock += quantity;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new AppValidationException("El precio debe ser mayor a cero.");

            Price = newPrice;
        }

        public bool HasStockFor(int quantity) => Stock >= quantity;
    }
}
