using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.User
{
    public class UserUpdateRoleRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; }
    }
}
