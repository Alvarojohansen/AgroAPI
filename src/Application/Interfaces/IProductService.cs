using Application.Dtos.Product;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        Product AddProduct(ProductRequest request);
        bool UpdateProduct(int id, ProductUpdateRequest request);
        bool DeleteProduct(int id);
    }
}
