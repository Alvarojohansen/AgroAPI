using Application.Dtos.SaleOrder;
using Application.Interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.JsonPatch;

namespace web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SaleOrderController : ControllerBase
    {
        private readonly ISaleOrderService _saleOrderService;

        public SaleOrderController(ISaleOrderService saleOrderService)
        {
            _saleOrderService = saleOrderService;
        }

        private bool IsUserInRole(string role)
        {
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return roleClaim != null && roleClaim.Value == role;
        }

        private int? GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return null;
            return int.TryParse(userIdClaim.Value, out var userId) ? userId : null;
        }

        // 🔹 GET: Todas las órdenes
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_saleOrderService.GetSaleOrders());
        }

        // 🔹 GET: Orden por ID
        [HttpGet("{id}")]
        public IActionResult GetSaleOrderById([FromRoute] int id)
        {
            var saleOrder = _saleOrderService.GetSaleOrderById(id);
            if (saleOrder == null)
                return NotFound($"No se encontró una orden con ID {id}");

            return Ok(saleOrder);
        }

        // 🔹 GET: Órdenes por cliente
        [HttpGet("client/{clientId}")]
        public IActionResult GetAllByClientId([FromRoute] int clientId)
        {
            var userId = GetUserId();
            if (userId == null) return Forbid();

            if (IsUserInRole("Admin") || (IsUserInRole("Client") && userId == clientId))
                return Ok(_saleOrderService.GetSaleOrdersByClientId(clientId));

            return BadRequest("El Id del cliente no es válido.");
        }

        // 🔹 GET: Órdenes por vendedor
        [HttpGet("seller/{sellerId}")]
        public IActionResult GetAllBySellerId([FromRoute] int sellerId)
        {
            var userId = GetUserId();
            if (userId == null) return Forbid();

            if (IsUserInRole("Admin") || (IsUserInRole("Seller") && userId == sellerId))
                return Ok(_saleOrderService.GetSaleOrdersBySellerId(sellerId));

            return BadRequest("El Id del vendedor no es válido.");
        }

        // 🔹 POST: Crear nueva orden
        [HttpPost]
        public IActionResult AddSaleOrder([FromBody] SaleOrderCreateDto saleOrder)
        {
            if (saleOrder == null) return BadRequest("Sale order vacía.");

            var userId = GetUserId();
            if (userId == null) return Forbid();

            if (IsUserInRole("Client") && userId == saleOrder.ClientId)
            {
                var created = _saleOrderService.AddSaleOrder(saleOrder);
                return Ok(created.OrderCode);
            }

            return BadRequest("ClientId inválido, prueba ingresando otro.");
        }

        // 🔹 PUT: Actualizar orden completa
        [HttpPut("{id}")]
        public IActionResult UpdateSaleOrder([FromRoute] int id, [FromBody] SaleOrderDto saleOrder)
        {
            var userId = GetUserId();
            if (userId == null) return Forbid();

            var existing = _saleOrderService.GetSaleOrderById(id);
            if (existing == null)
                return NotFound($"No se encontró una orden con ID {id}");

            if (!(IsUserInRole("Admin")
                  || (IsUserInRole("Client") && userId == existing.ClientId)
                  || (IsUserInRole("Seller") && userId == existing.SellerId)))
                return BadRequest("No tienes permisos para modificar esta orden.");

            var updated = _saleOrderService.UpdateSaleOrder(id, saleOrder);
            if (!updated)
                return BadRequest("No se pudo actualizar la orden.");

            return Ok("Orden actualizada con éxito.");
        }

        // 🔹 PATCH: Actualizar solo el estado
        [HttpPatch("{id}/state")]
        public IActionResult UpdateState([FromRoute] int id, [FromBody] SaleOrderStateUpdateDto saleOrderState)
        {
            var userId = GetUserId();
            if (userId == null) return Forbid();

            var existing = _saleOrderService.GetSaleOrderById(id);
            if (existing == null)
                return NotFound($"No se encontró una orden con ID {id}");

            if (!IsUserInRole("Admin") && !IsUserInRole("Seller"))
                return BadRequest("No tienes permisos para cambiar el estado.");

            _saleOrderService.UpdateSaleOrderStatus(id, saleOrderState);
            return Ok("Estado actualizado correctamente.");
        }

        // 🔹 PATCH: Completar orden
        [HttpPatch("{id}/complete")]
        public IActionResult CompleteSaleOrder([FromRoute] int id)
        {
            if (!IsUserInRole("Admin") && !IsUserInRole("Seller"))
                return BadRequest("No tienes permisos para completar la orden.");

            _saleOrderService.CompleteSaleOrder(id);
            return Ok("Orden completada con éxito.");
        }

        // 🔹 PATCH: Cancelar orden
        [HttpPatch("{id}/cancel")]
        public IActionResult CancelSaleOrder([FromRoute] int id)
        {
            if (!IsUserInRole("Admin") && !IsUserInRole("Client"))
                return BadRequest("Solo un cliente o admin puede cancelar su orden.");

            _saleOrderService.CancelSaleOrder(id);
            return Ok("Orden cancelada con éxito.");
        }

        // 🔹 DELETE: Eliminar orden
        [HttpDelete("{id}")]
        public IActionResult DeleteSaleOrder([FromRoute] int id)
        {
            var userId = GetUserId();
            if (userId == null) return Forbid();

            var existing = _saleOrderService.GetSaleOrderById(id);
            if (existing == null)
                return NotFound($"No se encontró una orden con ID {id}");

            if (IsUserInRole("Admin") ||
                (IsUserInRole("Client") && userId == existing.ClientId) ||
                (IsUserInRole("Seller") && userId == existing.SellerId))
            {
                _saleOrderService.DeleteSaleOrder(id);
                return Ok("Orden eliminada correctamente.");
            }

            return BadRequest("No tienes permisos para eliminar esta orden.");
        }

        // 🔹 PATCH: Eliminar un producto dentro de una orden (solo cliente)
        [HttpPatch("{orderId}/products/{productId}")]
        public IActionResult RemoveProductFromOrder([FromRoute] int orderId, [FromRoute] int productId)
        {
            if (!IsUserInRole("Client"))
                return BadRequest("Solo los clientes pueden eliminar productos de sus órdenes.");

            _saleOrderService.RemoveProductFromOrder(orderId, productId);
            return Ok("Producto eliminado correctamente de la orden.");
        }
    }
}
