using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        
        List<Product> GetAllProducts();
        Product? GetProductById(int id);
        Product? AddProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(int id);
        
    }
}
