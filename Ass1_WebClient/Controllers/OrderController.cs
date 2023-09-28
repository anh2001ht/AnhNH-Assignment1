using Ass1_WebClient.Utils;
using BusinessObjects;
using DataTransfer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ass1_WebClient.Controllers
{
    public class OrderController : Controller
    {
        private const string ORDER_ITEMS_KEY = "ORDER_ITEMS";

        private readonly HttpClient client = null;
        private string OrderApiUrl = "";
        private string OrderDetailApiUrl = "";
        private string MemberApiUrl = "";
        private string ProductApiUrl = "";

        public OrderController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "https://localhost:7033/api/Order";
            OrderDetailApiUrl = "https://localhost:7033/api/OrderDetail";
            MemberApiUrl = "https://localhost:7033/api/Member";
            ProductApiUrl = "https://localhost:7033/api/Product";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("USERID");
            string email = HttpContext.Session.GetString("EMAIL");
            if (userId == null)
            {
                return RedirectToAction("Index", "Home", TempData);
            }
            else if (email != "admin@estore.com")
            {
                return RedirectToAction("Profile", "Member", TempData);
            }

            List<Order> listOrders = await ApiHandler.DeserializeApiResponse<List<Order>>(OrderApiUrl, HttpMethod.Get);
            return View(listOrders);
        }

        [HttpGet]
        public async Task<IActionResult> OrderHistory()
        {
            int? userId = HttpContext.Session.GetInt32("USERID");
            string email = HttpContext.Session.GetString("EMAIL");
            if (userId == null)
            {
                return RedirectToAction("Index", "Home", TempData);
            }

            List<Order> listOrders = await ApiHandler.DeserializeApiResponse<List<Order>>(OrderApiUrl + $"/member/{userId.Value}", HttpMethod.Get);

            return View("Index", listOrders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            int? userId = HttpContext.Session.GetInt32("USERID");
            string email = HttpContext.Session.GetString("EMAIL");
            if (userId == null)
            {
                return RedirectToAction("Index", "Home", TempData);
            }

            Order order = await ApiHandler.DeserializeApiResponse<Order>(OrderApiUrl + "/" + id, HttpMethod.Get);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";

                if (email == "admin@estore.com")
                    return RedirectToAction("Index", "Order", TempData);
                else
                    return RedirectToAction("OrderHistory", "Order", TempData);
            }

            List<OrderDetail> listOrderDetails = await ApiHandler.DeserializeApiResponse<List<OrderDetail>>(OrderDetailApiUrl + $"/order/{id}", HttpMethod.Get);

            ViewData["Order"] = order;
            ViewData["OrderDetails"] = listOrderDetails;

            return View("OrderDetail");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string email = HttpContext.Session.GetString("EMAIL");
            if (email == null)
            {
                return RedirectToAction("Index", "Home", TempData);
            }
            if (email != "admin@estore.com")
            {
                return RedirectToAction("OrderHistory", "Order", TempData);
            }

            Order order = await ApiHandler.DeserializeApiResponse<Order>(OrderApiUrl + "/" + id, HttpMethod.Get);
            if (order == null)
            {
                return RedirectToAction("Index", "Order", TempData);
            }

            List<OrderDetail> listOrderDetails = await ApiHandler.DeserializeApiResponse<List<OrderDetail>>(OrderDetailApiUrl + $"/order/{id}", HttpMethod.Get);


            ViewData["Order"] = order;
            ViewData["OrderDetails"] = listOrderDetails;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string orderIdStr)
        {
            string email = HttpContext.Session.GetString("EMAIL");
            if (email == null)
            {
                return RedirectToAction("Index", "Home", TempData);
            }
            if (email != "admin@estore.com")
            {
                return RedirectToAction("OrderHistory", "Order", TempData);
            }

            int orderId = int.Parse(orderIdStr);
            Order order = await ApiHandler.DeserializeApiResponse<Order>(OrderApiUrl + "/" + orderId, HttpMethod.Get);
            if (order == null)
            {
                return RedirectToAction("Index", "Order", TempData);
            }

            List<OrderDetail> listOrderDetails = await ApiHandler.DeserializeApiResponse<List<OrderDetail>>(OrderDetailApiUrl + $"/order/{order.OrderID}", HttpMethod.Get);

            foreach (OrderDetail orderDetail in listOrderDetails)
            {
                await ApiHandler.DeserializeApiResponse<OrderDetail>(OrderDetailApiUrl + "/" + orderDetail.OrderID + "/" + orderDetail.ProductID, HttpMethod.Delete);
            }

            await ApiHandler.DeserializeApiResponse<Order>(OrderApiUrl + "/" + order.OrderID, HttpMethod.Delete);

            return RedirectToAction("Index", "Order", TempData);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            int? userId = HttpContext.Session.GetInt32("USERID");
            if (userId == null)
            {
                return RedirectToAction("Index", "Home", TempData);
            }

            List<Member> listMembers = await ApiHandler.DeserializeApiResponse<List<Member>>(MemberApiUrl, HttpMethod.Get);
            List<Product> listProducts = await ApiHandler.DeserializeApiResponse<List<Product>>(ProductApiUrl, HttpMethod.Get);


            ViewData["OrderItems"] = GetOrderItems();
            ViewData["Members"] = listMembers;
            ViewData["Products"] = listProducts;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderRequest orderRequest)
        {
            List<OrderItemRequest> listItemsRequest = GetOrderItems();
            if (listItemsRequest.Count == 0)
            {
                return RedirectToAction("Create", TempData);
            }

            string email = HttpContext.Session.GetString("EMAIL");
            int memberID = orderRequest.MemberID;
            if (email != "admin@estore.com")
            {
                memberID = HttpContext.Session.GetInt32("USERID").Value;
            }

            Order order = new Order()
            {
                MemberID = memberID,
                OrderDate = DateTime.Now,
                Freight = orderRequest.Freight,
            };
            Order orderSaved = await ApiHandler.DeserializeApiResponse<Order>(OrderApiUrl, HttpMethod.Post, order);

            foreach (OrderItemRequest itemRequest in listItemsRequest)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderID = orderSaved.OrderID,
                    ProductID = itemRequest.Product.ProductID,
                    Quantity = itemRequest.Quantity,
                    UnitPrice = itemRequest.Product.UnitPrice,
                    Discount = 0
                };
                await client.PostAsJsonAsync(OrderDetailApiUrl, orderDetail);
            }

            ClearOrderItemsSession();

            if (email != "admin@estore.com")
            {
                return RedirectToAction("OrderHistory", TempData);
            }
            else
            {
                return RedirectToAction("Index", TempData);
            }
        }

        public async Task<IActionResult> AddOrderItem(OrderRequest orderRequest)
        {
            List<Product> listProducts = await ApiHandler.DeserializeApiResponse<List<Product>>(ProductApiUrl, HttpMethod.Get);

            Product product = listProducts.Where(p => p.ProductID == orderRequest.ProductID).FirstOrDefault();
            if (product == null)
            {
                return RedirectToAction("Create", TempData);
            }

            List<OrderItemRequest> listItemsRequest = GetOrderItems();
            OrderItemRequest itemRequest = listItemsRequest.Find(p => p.Product.ProductID == orderRequest.ProductID);
            if (itemRequest != null)
            {
                if (itemRequest.Quantity + orderRequest.Quantity > product.UnitsInStock)
                {
                    return RedirectToAction("Create", TempData);
                }
                itemRequest.Quantity += orderRequest.Quantity;
            }
            else
            {
                if (orderRequest.Quantity > product.UnitsInStock)
                {
                    return RedirectToAction("Create", TempData);
                }
                listItemsRequest.Add(new OrderItemRequest() { Quantity = orderRequest.Quantity, Product = product });
            }

            SaveOrderItemsSession(listItemsRequest);
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> RemoveOrderItem(OrderRequest orderRequest)
        {
            List<OrderItemRequest> listItemsRequest = GetOrderItems();
            listItemsRequest.RemoveAll(p => p.Product.ProductID == orderRequest.ProductID);
            SaveOrderItemsSession(listItemsRequest);
            return RedirectToAction("Create");
        }

        private List<OrderItemRequest> GetOrderItems()
        {
            var session = HttpContext.Session;
            string jsonOrderItems = session.GetString(ORDER_ITEMS_KEY);
            if (jsonOrderItems != null)
            {
                return JsonConvert.DeserializeObject<List<OrderItemRequest>>(jsonOrderItems);
            }
            return new List<OrderItemRequest>();
        }

        private void SaveOrderItemsSession(List<OrderItemRequest> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(ORDER_ITEMS_KEY, jsoncart);
        }

        private void ClearOrderItemsSession()
        {
            var session = HttpContext.Session;
            session.Remove(ORDER_ITEMS_KEY);
        }
    }
}
