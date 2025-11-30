using System.Collections.Generic;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Interfaces
{
    public interface IOrderRepository
    {
        void SaveOrder(Order order, IEnumerable<Cart> cartItems);
    }
}
