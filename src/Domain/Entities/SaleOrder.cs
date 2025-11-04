using Domain.Enum;
using Domain.Exceptions;
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
        public ICollection<SaleOrderLine> SaleOrderLines { get; set; } = new List<SaleOrderLine>();

        // Constructor protegido para EF
        protected SaleOrder() { }

        // Constructor de dominio
        public SaleOrder(int clientId, int sellerId, StatusOrderEnum initialStatus = StatusOrderEnum.Pending)
        {
            ClientId = clientId;
            SellerId = sellerId;
            Date = DateTime.UtcNow;
            SetStatus(initialStatus);

        }
        //  Métodos de dominio

        public void SetStatus(StatusOrderEnum status)
        {
            if (!StatusOrderEnum.IsDefined(typeof(StatusOrderEnum), status))
                throw new AppValidationException($"El estado '{status}' no es válido para {nameof(StatusOrderEnum)}.");

            OrderStatus = status;
        }

        public void ChangeStatus(StatusOrderEnum newStatus)
        {
            if (!StatusOrderEnum.IsDefined(typeof(StatusOrderEnum), newStatus))
                throw new AppValidationException($"Estado inválido: {newStatus}");

            if (OrderStatus == StatusOrderEnum.Cancelled)
                throw new AppValidationException("No se puede cambiar el estado de una orden cancelada.");

            if (OrderStatus == StatusOrderEnum.Completed && newStatus != StatusOrderEnum.Completed)
                throw new AppValidationException("Una orden completada no puede volver a otro estado.");

            OrderStatus = newStatus;
        }

        public void AddLine(Product product, int quantity)
        {
            if (OrderStatus != StatusOrderEnum.Pending)
                throw new AppValidationException("Solo se pueden agregar productos a órdenes pendientes.");

            if (quantity <= 0)
                throw new AppValidationException("La cantidad debe ser mayor a cero.");

            if (product.Stock < quantity)
                throw new AppValidationException($"No hay suficiente stock disponible para el producto {product.Name}.");

            var existingLine = SaleOrderLines.FirstOrDefault(l => l.ProductId == product.Id);

            if (existingLine != null)
            {
                existingLine.Quantity += quantity;
            }
            else
            {
                SaleOrderLines.Add(new SaleOrderLine
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });
            }

            // 🔹 Resta el stock del producto
            product.Stock -= quantity;

            RecalculateTotal();
        }

        public void RemoveLine(Product product)
        {
            if (OrderStatus != StatusOrderEnum.Pending)
                throw new AppValidationException("Solo se pueden eliminar líneas de órdenes pendientes.");

            var line = SaleOrderLines.FirstOrDefault(l => l.ProductId == product.Id);
            if (line == null)
                throw new AppValidationException("El producto no existe en la orden.");

            // 🔹 Devuelve el stock al producto
            product.Stock += line.Quantity;

            SaleOrderLines.Remove(line);
            RecalculateTotal();
        }

        public void Cancel()
        {
            if (OrderStatus == StatusOrderEnum.Completed)
                throw new AppValidationException("No se puede cancelar una orden completada.");

            OrderStatus = StatusOrderEnum.Cancelled;
        }

        public void Complete()
        {
            if (!SaleOrderLines.Any())
                throw new AppValidationException("No se puede completar una orden sin líneas.");

            OrderStatus = StatusOrderEnum.Completed;
        }

        // Método para recalcular el total de la orden
        public void RecalculateTotal()
        {
            Total = SaleOrderLines?.Sum(l => l.UnitPrice * l.Quantity) ?? 0;
        }
        
    } 
}
