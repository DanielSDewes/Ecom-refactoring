using System.Collections.Generic;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void SaveOrder(Order order, IEnumerable<Cart> cartItems)
        {
            using (var db = new ChienVHShopDBEntities())
            {
                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var cart in cartItems)
                {
                    var detail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = cart.Product.ProductId,
                        Quantity = cart.Quantity,
                        Price = cart.Product.Price
                    };

                    db.OrderDetails.Add(detail);
                }

                db.SaveChanges();
            }
        }
    }
}
