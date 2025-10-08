using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICurrentUserService
    {
        int? SellerId { get; }
        int? ClientId { get; }
        string? Name { get; }
        int? ClientId { get; }

    }
}
