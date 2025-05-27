using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User
{
    public class UserUpdateRequest

    {
        public string? Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string City { get; set; }
        public string? Phone { get; set; }
        
    }
}
