using BusinessObjects;

namespace DataAccess
{
    public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                using (var context = new MyDBContext())
                {
                    listProducts = context.Products.ToList();
                    listProducts.ForEach(f =>
                    {
                        f.Category = context.Categories.Find(f.CategoryID);
                    });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listProducts;
        }
        public static List<Product> Search(string keyword)
        {
            var listProducts = new List<Product>();
            try
            {
                using (var context = new MyDBContext())
                {
                    listProducts = context.Products.Where(f => f.ProductName.Contains(keyword)).ToList();
                    listProducts.ForEach(f =>
                    {
                        f.Category = context.Categories.Find(f.CategoryID);
                    });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listProducts;
        }
        public static List<Product> FindProductsByCategoryId(int categoryId)
        {
            var listProducts = new List<Product>();
            try
            {
                using (var context = new MyDBContext())
                {
                    listProducts = context.Products.Where(f => f.CategoryID == categoryId).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listProducts;
        }
        public static Product FindProductById(int productId)
        {
            var product = new Product();
            try
            {
                using (var context = new MyDBContext())
                {
                    product = context.Products.SingleOrDefault(f => f.ProductID == productId);
                    product.Category = context.Categories.Find(product.CategoryID);
                    product.OrderDetails = context.OrderDetails.Where(o => o.ProductID == productId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }
        public static void SaveProduct(Product product)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    context.Products.Add(product);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateProduct(Product product)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    context.Entry(product).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteProduct(Product product)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var p = context
                        .Products
                        .SingleOrDefault(f => f.ProductID == product.ProductID);
                    context.Products.Remove(p);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
