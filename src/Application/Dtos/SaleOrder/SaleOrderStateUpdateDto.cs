using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.SaleOrder
{
    public class SaleOrderStateUpdateDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusOrderEnum OrderStatus { get; set; }
    }
}
