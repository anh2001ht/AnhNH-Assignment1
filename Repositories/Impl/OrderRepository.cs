using BusinessObjects;
using DataAccess;

namespace Repositories.Impl
{
    public class OrderRepository : IOrderRepository
    {
        public Order SaveOrder(Order order) => OrderDAO.SaveOrder(order);
        public Order GetOrderById(int id) => OrderDAO.FindOrderById(id);
        public List<Order> GetOrders() => OrderDAO.GetOrders();
        public List<Order> GetAllOrdersByMemberId(int memberId) => OrderDAO.FindAllOrdersByMemberId(memberId);
        public void UpdateOrder(Order order) => OrderDAO.UpdateOrder(order);
        public void DeleteOrder(Order order) => OrderDAO.DeleteOrder(order);
        public List<OrderDetail> GetOrderDetails(int orderId) => OrderDetailDAO.FindAllOrderDetailsByOrderId(orderId);
    }
}
