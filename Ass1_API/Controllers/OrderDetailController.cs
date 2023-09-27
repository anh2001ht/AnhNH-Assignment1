using BusinessObjects;
using DataTransfer;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Impl;

namespace Ass1_API.Controllers
{
    public class OrderDetailController : ControllerBase
    {
        private IOrderDetailRepository repository = new OrderDetailRepository();

        [HttpGet]
        public ActionResult<IEnumerable<OrderDetail>> GetOrderDetails() => repository.GetOrderDetails();
        [HttpGet("order/{id}")]
        public ActionResult<IEnumerable<OrderDetail>> GetOrderDetailsByOrderId(int id) => repository.GetOrderDetailsByOrderId(id);

        [HttpGet("{orderId}/{productId}")]
        public ActionResult<OrderDetail> GetOrderDetailByOrderIdAndProductId(int orderId, int productId) => repository.GetOrderDetailByOrderIdAndProductId(orderId, productId);

        [HttpPost]
        public IActionResult PostOrderDetail(OrderDetailRequest OrderDetailRequest)
        {
            var orderDetail = new OrderDetail
            {
                OrderID = OrderDetailRequest.OrderID,
                ProductID = OrderDetailRequest.ProductID,
                UnitPrice = OrderDetailRequest.UnitPrice,
                Quantity = OrderDetailRequest.Quantity,
            };
            repository.SaveOrderDetail(orderDetail);
            return NoContent();
        }

        [HttpDelete("{orderId}/{productId}")]
        public IActionResult DeleteOrderDetailByOrderIdAndProductId(int orderId, int productId)
        {
            var o = repository.GetOrderDetailByOrderIdAndProductId(orderId, productId);
            if (o == null)
            {
                return NotFound();
            }
            repository.DeleteOrderDetail(o);
            return NoContent();
        }

        [HttpPut("{orderId}/{productId}")]
        public IActionResult PutOrderDetailByOrderIdAndProductId(int orderId, int productId, OrderDetail orderDetail)
        {
            var order = repository.GetOrderDetailByOrderIdAndProductId(orderId, productId);
            if (order == null)
            {
                return NotFound();
            }

            order.UnitPrice = orderDetail.UnitPrice;
            order.Quantity = orderDetail.Quantity;
            order.Discount = orderDetail.Discount;

            repository.UpdateOrderDetail(order);
            return NoContent();
        }
    }
}
