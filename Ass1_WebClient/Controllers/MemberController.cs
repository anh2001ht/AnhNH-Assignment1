using Ass1_WebClient.Utils;
using BusinessObjects;
using DataTransfer;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Ass1_WebClient.Controllers
{
    public class MemberController : Controller
    {
        private readonly HttpClient client = null;
        private string MemberApiUrl = "";

        public MemberController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "https://localhost:7033/api/member";
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
/*            string Role = HttpContext.Session.GetString("EMAIL");

            if (Role == null)
            {
                TempData["ErrorMessage"] = "You must login to access this page.";
                return RedirectToAction("Index", "Home");
            }
            else if (Role != "Admin")
            {
                TempData["ErrorMessage"] = "You don't have permission to access this page.";
                return RedirectToAction("Profile", "Member");
            }*/

            List<Member> listMembers = await ApiHandler.DeserializeApiResponse<List<Member>>(MemberApiUrl, HttpMethod.Get);
/*
            if (TempData != null)
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            }*/

            return View(listMembers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            string Role = HttpContext.Session.GetString("EMAIL");

            if (Role == null)
            {
                TempData["ErrorMessage"] = "You must login to access this page.";
                return RedirectToAction("Index", "Home");
            }
            else if (Role != "Admin")
            {
                TempData["ErrorMessage"] = "You don't have permission to access this page.";
                return RedirectToAction("Profile", "Member");
            }

            if (TempData != null)
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MemberRequest memberRequest)
        {
            Member member = await ApiHandler.DeserializeApiResponse<Member>(MemberApiUrl + "/Email/" + memberRequest.Email, HttpMethod.Get);
            if (memberRequest.Email.Equals("admin") ||
                (member != null && member.MemberID != 0))
            {
                TempData["ErrorMessage"] = "Email already exists.";
                return RedirectToAction("Create");
            }

            await ApiHandler.DeserializeApiResponse(MemberApiUrl, HttpMethod.Post, memberRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string Role = HttpContext.Session.GetString("EMAIL");

            if (Role == null)
            {
                TempData["ErrorMessage"] = "You must login to access this page.";
                return RedirectToAction("Index", "Home");
            }
            else if (Role != "Admin")
            {
                TempData["ErrorMessage"] = "You don't have permission to access this page.";
                return RedirectToAction("Profile", "Member");
            }

            Member member = await ApiHandler.DeserializeApiResponse<Member>(MemberApiUrl + "/" + id, HttpMethod.Get);

            if (TempData != null)
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            }

            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MemberRequest memberRequest)
        {
            await ApiHandler.DeserializeApiResponse(MemberApiUrl + "/" + memberRequest.MemberID, HttpMethod.Put, memberRequest);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string Role = HttpContext.Session.GetString("EMAIL");

            if (Role == null)
            {
                TempData["ErrorMessage"] = "You must login to access this page.";
                return RedirectToAction("Index", "Home");
            }
            else if (Role != "Admin")
            {
                TempData["ErrorMessage"] = "You don't have permission to access this page.";
                return RedirectToAction("Profile", "Member");
            }

            Member member = await ApiHandler.DeserializeApiResponse<Member>(MemberApiUrl + "/" + id, HttpMethod.Get);

            if (TempData != null)
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            }

            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(MemberRequest memberRequest)
        {
            HttpResponseMessage response = await client.DeleteAsync(MemberApiUrl + "/" + memberRequest.MemberID);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            else
                return View();
        }

        // Member
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            int userId = HttpContext.Session.GetInt32("USERID").Value;

            Member member = await ApiHandler.DeserializeApiResponse<Member>(MemberApiUrl + "/" + userId, HttpMethod.Get);

            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            string Role = HttpContext.Session.GetString("EMAIL");
            if (Role == null)
            {
                TempData["ErrorMessage"] = "You must login to access this page.";
                return RedirectToAction("Index", "Home");
            }
            else if (Role != "Member")
            {
                TempData["ErrorMessage"] = "You don't have permission to access this page.";
                return RedirectToAction("Index", "Member");
            }
            int userId = HttpContext.Session.GetInt32("USERID").Value;

            Member member = await ApiHandler.DeserializeApiResponse<Member>(MemberApiUrl + "/" + userId, HttpMethod.Get);

            if (TempData != null)
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            }

            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(MemberRequest memberRequest)
        {
            int? userId = HttpContext.Session.GetInt32("USERID");
            if (userId == null)
                return RedirectToAction("Index", "Home");

            memberRequest.MemberID = userId.Value;
            await ApiHandler.DeserializeApiResponse(MemberApiUrl + "/" + memberRequest.MemberID, HttpMethod.Put, memberRequest);

            TempData["SuccessMessage"] = "Edit profile information successfully.";

            return RedirectToAction("Profile", TempData);
        }
    }
}
