using BusinessObjects;
using DataAccess;

namespace Repositories.Impl
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void SaveOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.SaveOrderDetail(orderDetail);
        public OrderDetail GetOrderDetailByOrderIdAndProductId(int orderId, int flowerBouquetId) => OrderDetailDAO.FindOrderDetailByOrderIdAndProductId(orderId, flowerBouquetId);
        public List<OrderDetail> GetOrderDetails() => OrderDetailDAO.GetOrderDetails();
        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId) => OrderDetailDAO.FindAllOrderDetailsByOrderId(orderId);
        public void UpdateOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.UpdateOrderDetail(orderDetail);
        public void DeleteOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.DeleteOrderDetail(orderDetail);
    }
}
