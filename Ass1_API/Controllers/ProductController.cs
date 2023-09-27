using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using DataTransfer;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Impl;

namespace Ass1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository repository = new ProductRepository();
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts() => repository.GetProducts();

        [HttpGet("Search/{keyword}")]
        public ActionResult<IEnumerable<Product>> Search(string keyword) => repository.Search(keyword);

        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id) => repository.GetProductById(id);

        [HttpPost]
        public IActionResult PostProduct(ProductRequest productRequest)
        {
            var f = new Product
            {
                ProductName = productRequest.ProductName,
                UnitPrice = productRequest.UnitPrice,
                UnitsInStock = productRequest.UnitsInStock,
                CategoryID = productRequest.CategoryID,
                Weight = productRequest.Weight,
            };
            repository.SaveProduct(f);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var f = repository.GetProductById(id);
            if (f == null)
            {
                return NotFound();
            }
            if (f.OrderDetails != null && f.OrderDetails.Count > 0)
            {
                repository.UpdateProduct(f);
            }
            else
            {
                repository.DeleteProduct(f);
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, ProductRequest productRequest)
        {
            var product = repository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            product.ProductName = productRequest.ProductName;
            product.UnitPrice = productRequest.UnitPrice;
            product.UnitsInStock = productRequest.UnitsInStock;
            product.CategoryID = productRequest.CategoryID;
            product.Weight = productRequest.Weight;

            repository.UpdateProduct(product);
            return NoContent();
        }
    }
}
