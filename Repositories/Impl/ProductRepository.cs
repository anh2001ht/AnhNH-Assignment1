using DataAccess;
using BusinessObjects;

namespace Repositories.Impl
{
    public class ProductRepository : IProductRepository
    {
        public void DeleteProduct(Product product) => ProductDAO.DeleteProduct(product);
        public List<OrderDetail> GetOrderDetails(int productId) => OrderDetailDAO.FindOrderDetailsByProductId(productId);
        public Product GetProductById(int id) => ProductDAO.FindProductById(id);
        public List<Product> GetProducts() => ProductDAO.GetProducts();
        public void SaveProduct(Product product) => ProductDAO.SaveProduct(product);
        public List<Product> Search(string keyword) => ProductDAO.Search(keyword);
        public void UpdateProduct(Product product) => ProductDAO.UpdateProduct(product);
    }
}
