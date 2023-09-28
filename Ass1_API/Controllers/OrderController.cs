using BusinessObjects;
using DataTransfer;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Impl;

namespace Ass1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderRepository repository = new OrderRepository();

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders() => repository.GetOrders();

        [HttpGet("member/{id}")]
        public ActionResult<IEnumerable<Order>> GetAllOrdersByMemberId(int id) => repository.GetAllOrdersByMemberId(id);

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(int id) => repository.GetOrderById(id);

        [HttpPost]
        public ActionResult<Order> PostOrder(OrderReq orderReq)
        {
            var order = new Order
            {
                OrderDate = orderReq.OrderDate,
                ShippedDate = null,
                Freight = orderReq.Freight,
                MemberID = orderReq.MemberID
            };
            return repository.SaveOrder(order);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var o = repository.GetOrderById(id);
            if (o == null)
            {
                return NotFound();
            }
            repository.DeleteOrder(o);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult PutOrder(int id, Order order)
        {
            var oTmp = repository.GetOrderById(id);
            if (oTmp == null)
            {
                return NotFound();
            }

            oTmp.OrderDate = order.OrderDate;
            oTmp.ShippedDate = order.ShippedDate;
            oTmp.Freight = order.Freight;

            repository.UpdateOrder(oTmp);
            return NoContent();
        }


    }
}
