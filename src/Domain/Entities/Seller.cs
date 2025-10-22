using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Seller : User
    {
        public string? BankAccountNumber { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        
    }
}
