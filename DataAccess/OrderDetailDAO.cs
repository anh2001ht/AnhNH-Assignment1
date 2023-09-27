using BusinessObjects;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        public static List<OrderDetail> GetOrderDetails()
        {
            var listOrderDetails = new List<OrderDetail>();
            try
            {
                using (var context = new MyDBContext())
                {
                    listOrderDetails = context.OrderDetails.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listOrderDetails;
        }
        public static List<OrderDetail> FindOrderDetailsByProductId(int productId)
        {
            var listOrderDetails = new List<OrderDetail>();
            try
            {
                using (var context = new MyDBContext())
                {
                    listOrderDetails = context
                        .OrderDetails
                        .Where(o => o.ProductID == productId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listOrderDetails;
        }
        public static List<OrderDetail> FindAllOrderDetailsByOrderId(int orderId)
        {
            var listOrderDetails = new List<OrderDetail>();
            try
            {
                using (var context = new MyDBContext())
                {
                    listOrderDetails = context
                        .OrderDetails
                        .Where(o => o.OrderID == orderId)
                        .ToList();
                    listOrderDetails.ForEach(o =>
                        o.Product = context.Products.SingleOrDefault(f => f.ProductID == o.ProductID)
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listOrderDetails;
        }

        public static OrderDetail FindOrderDetailByOrderIdAndProductId(int orderId, int productId)
        {
            var orderDetail = new OrderDetail();
            try
            {
                using (var context = new MyDBContext())
                {
                    orderDetail = context
                        .OrderDetails
                        .SingleOrDefault(o => o.OrderID == orderId && o.ProductID == productId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetail;
        }

        public static void SaveOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    context.OrderDetails.Add(orderDetail);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    context.Entry(orderDetail).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var od = context
                        .OrderDetails
                        .SingleOrDefault(o => o.OrderID == orderDetail.OrderID && o.ProductID == orderDetail.ProductID);
                    context.OrderDetails.Remove(od);
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
