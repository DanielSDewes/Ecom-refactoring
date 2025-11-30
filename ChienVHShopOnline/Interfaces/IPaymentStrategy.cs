using System.Web.Mvc;
using System.Collections.Generic;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Interfaces
{
    public interface IPaymentStrategy
    {
        ActionResult ProcessPayment(Controller controller, IList<Cart> cartItems);
    }
}
