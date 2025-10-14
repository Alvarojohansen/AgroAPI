
using Application.Dtos.SaleOrder;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SaleOrderController : Controller
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

            if (userIdClaim == null)
                return null;

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return null;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var saleOrders = _saleOrderService.GetSaleOrders();
            return Ok(saleOrders);
        }

        [HttpGet("GetSaleOrder/{id}")]
        public IActionResult GetSaleOrderById([FromRoute] int id)
        {
            var saleOrder = _saleOrderService.GetSaleOrderById(id);
            if (saleOrder == null)
                return NotFound($"No se encontró una orden de venta con ID {id}");

            return Ok(saleOrder);
        }

        [HttpGet("client/{clientId}")]
        public IActionResult GetAllByClientId([FromRoute]int clientId)
        {
            var userId = GetUserId();
            if (userId == null) return Forbid();

            if (IsUserInRole("Admin") || (IsUserInRole("Client") && userId == clientId))
            {
                var saleOrders = _saleOrderService.GetSaleOrdersByClientId(clientId);
                return Ok(saleOrders);
            }
            return BadRequest("El Id del cliente no es valido.");
        }
        [HttpGet("seller/{sellerId}")]
        public IActionResult GetAllBySellerId([FromRoute] int sellerId)
        {
            var userId = GetUserId();
            if (userId == null) return Forbid();

            if (IsUserInRole("Admin") || (IsUserInRole("Seller") && userId == sellerId))
            {
                var saleOrders = _saleOrderService.GetSaleOrdersBySellerId(sellerId);
                return Ok(saleOrders);
            }
            return BadRequest("El Id del cliente no es valido.");
        }

        [HttpPost("AddSaleOrder/")]
        public IActionResult AddSaleOrder([FromBody] SaleOrderCreateDto saleOrder)
        {
            if (saleOrder == null)
            {
                return BadRequest("Sale order vacia.");
            }

            var userId = GetUserId();
            if (userId == null) return Forbid();

            if (IsUserInRole("Client") && userId == saleOrder.ClientId)
            {
                var createdSaleOrder = _saleOrderService.AddSaleOrder(saleOrder);
                
                return Ok(createdSaleOrder.OrderCode);
            }

            return BadRequest("CLientId invalido, prueba ingresando otro.");
        }

        [HttpPut("UpdateSaleOrder/{id}")]
        public IActionResult UpdateSaleOrder([FromRoute] int id, [FromBody] SaleOrderDto saleOrder)
        {
            var userId = GetUserId();
            if (userId == null) return Forbid();

            var existingSaleOrder = _saleOrderService.GetSaleOrderById(id);
            if (existingSaleOrder == null)
                return NotFound($"No se encontró una orden de venta con ID {id}");

            // Validar permisos: Admin o dueño (cliente/vendedor)
            if (!(IsUserInRole("Admin")
                  || (IsUserInRole("Client") && userId == existingSaleOrder.ClientId)
                  || (IsUserInRole("Seller") && userId == existingSaleOrder.SellerId)))
            {
                return BadRequest("El Id del cliente o vendedor no es válido.");
            }

            var updated = _saleOrderService.UpdateSaleOrder(id, saleOrder);
            if (!updated)
                return BadRequest("No se pudo actualizar la orden.");

            return Ok("Sale order actualizada con exito.");
        }


        [HttpDelete("DeleteSaleOrder/{id}")]
        public IActionResult DeleteSaleOrder([FromRoute] int id)
        {
            var userId = GetUserId();
            if (userId == null) return Forbid();
            var existingSaleOrders = _saleOrderService.GetSaleOrders();
            var existingSaleOrder = existingSaleOrders.FirstOrDefault(so => so.Id == id);
            if (existingSaleOrder == null)
            {
                return NotFound($"No se encontró una orden de venta con ID {id}");
            }
            if (IsUserInRole("Admin") || (IsUserInRole("Client") && userId == existingSaleOrder.ClientId) || (IsUserInRole("Seller") && userId == existingSaleOrder.SellerId))
            {
                _saleOrderService.DeleteSaleOrder(id);
                return Ok("Sale order deleted successfully.");
            }
            return BadRequest("El Id del cliente o vendedor no es valido.");
        }

    }
}
