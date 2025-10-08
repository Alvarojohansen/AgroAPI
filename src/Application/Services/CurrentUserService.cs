using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        // Constructor (lo vamos a utilizar para abstraer el ID del JWT)
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? SellerId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")
                         ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

                if (claim != null && int.TryParse(claim.Value, out var id))
                    return id;

                return null;
            }
        }
        public int? ClientId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")
                         ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

                if (claim != null && int.TryParse(claim.Value, out var id))
                    return id;

                return null;
            }
        }
        public string? Name
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("username")
                         ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name);
                if (claim == null) return null;

                return claim.Value;
            }
        }
        
    }
}
