using BusinessObjects;
using DataTransfer;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Ass1_WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient client = null;
        private string MemberApiUrl = "";

        public HomeController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "https://localhost:7033/api/Member";
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest loginRequest)
        {
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl);
            string stringData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

            Member admin = new Member
            {
                Email = config["Credentials:Email"],
                Password = config["Credentials:Password"],
                City = "no",
                Country = "no"
            };

            List<Member> listMembers = JsonSerializer.Deserialize<List<Member>>(stringData, options);
            listMembers.Add(admin);
            Member account = listMembers.Where(c => c.Email == loginRequest.Email && c.Password == loginRequest.Password).FirstOrDefault();

            if (account != null)
            {
                HttpContext.Session.SetInt32("USERID", account.MemberID);
                HttpContext.Session.SetString("EMAIL", account.Email);
                if (account.Email == "admin@gmail.com")
                    return RedirectToAction("Index", "Member");
                else
                    return RedirectToAction("Profile", "Member");
            }
            else
            {
                ViewData["ErrorMessage"] = "Email or password is invalid.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}