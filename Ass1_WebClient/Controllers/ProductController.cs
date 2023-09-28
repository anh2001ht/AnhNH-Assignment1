using Ass1_WebClient.Utils;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Ass1_WebClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        private string CategoryApiUrl = "";
        private string SupplierApiUrl = "";

        public ProductController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7033/api/Product";
            CategoryApiUrl = "https://localhost:7033/api/Category";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string Email = HttpContext.Session.GetString("EMAIL");

            if (Email == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (Email != "admin@estore.com")
            {
                return RedirectToAction("Profile", "Member");
            }

            List<Product> listProducts = await ApiHandler.DeserializeApiResponse<List<Product>>(ProductApiUrl, HttpMethod.Get);

            return View(listProducts);
        }

        public async Task<IActionResult> Search(string keyword)
        {
            List<Product> listProducts = await ApiHandler.DeserializeApiResponse<List<Product>>(ProductApiUrl + "/Search/" + keyword, HttpMethod.Get);

            ViewData["keyword"] = keyword;

            return View("Index", listProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            string Email = HttpContext.Session.GetString("EMAIL");

            if (Email == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (Email != "admin@estore.com")
            {
                return RedirectToAction("Profile", "Member");
            }

            List<Category> listCategories = await ApiHandler.DeserializeApiResponse<List<Category>>(CategoryApiUrl, HttpMethod.Get);

            ViewData["Categories"] = listCategories;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(ProductApiUrl, product);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string Email = HttpContext.Session.GetString("EMAIL");

            if (Email == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (Email != "admin@estore.com")
            {
                return RedirectToAction("Profile", "Member");
            }

            Product product = await ApiHandler.DeserializeApiResponse<Product>(ProductApiUrl + "/" + id, HttpMethod.Get);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            List<Category> listCategories = await ApiHandler.DeserializeApiResponse<List<Category>>(CategoryApiUrl, HttpMethod.Get);

            ViewData["Categories"] = listCategories;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            await ApiHandler.DeserializeApiResponse(ProductApiUrl + "/" + product.ProductID, HttpMethod.Put, product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string Email = HttpContext.Session.GetString("EMAIL");

            if (Email == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (Email != "admin@estore.com")
            {
                return RedirectToAction("Profile", "Member");
            }

            Product product = await ApiHandler.DeserializeApiResponse<Product>(ProductApiUrl + "/" + id, HttpMethod.Get);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            await ApiHandler.DeserializeApiResponse<Product>(ProductApiUrl + "/" + product.ProductID, HttpMethod.Delete);
            return RedirectToAction("Index");
        }
    }
}
