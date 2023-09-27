using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IProductRepository
    {
        void SaveProduct(Product product);
        Product GetProductById(int id);
        List<Product> GetProducts();
        List<Product> Search(string keyword);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        List<OrderDetail> GetOrderDetails(int productId);
    }
}
