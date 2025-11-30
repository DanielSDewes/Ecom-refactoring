using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Services;
using ChienVHShopOnline.ViewModels;

namespace ChienVHShopOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IPaymentStrategy _paymentStrategy;

        public ShoppingCartController(
            IShoppingCartService cartService,
            IOrderService orderService,
            IPaymentStrategy paymentStrategy)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _paymentStrategy = paymentStrategy ?? throw new ArgumentNullException(nameof(paymentStrategy));
        }

        private ShoppingCartViewModel BuildCartViewModel()
        {
            var cartItems = _cartService.GetCart();

            return new ShoppingCartViewModel
            {
                Items = cartItems.Select(c => new CartItemViewModel
                {
                    ProductId = c.Product.ProductId,
                    ProductName = c.Product.ProductName,
                    Price = c.Product.Price,
                    Quantity = c.Quantity
                }).ToList()
            };
        }

        public ActionResult Index()
        {
            var viewModel = BuildCartViewModel();
            return View(viewModel);
        }

        public ActionResult OrderNow(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _cartService.AddProduct(id.Value);

            var viewModel = BuildCartViewModel();
            return View("Index", viewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _cartService.RemoveProduct(id.Value);

            var viewModel = BuildCartViewModel();
            return View("Index", viewModel);
        }

        [HttpPost]
        public ActionResult UpdateCart(FormCollection frc)
        {
            string[] quantities = frc.GetValues("quantity");
            if (quantities != null)
            {
                int[] parsed = quantities.Select(q =>
                {
                    int.TryParse(q, out int value);
                    return value;
                }).ToArray();

                _cartService.UpdateQuantities(parsed);
            }

            var viewModel = BuildCartViewModel();
            return View("Index", viewModel);
        }

        public ActionResult CheckOut()
        {
            var viewModel = new CheckoutViewModel();
            return View("CheckOut", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessOrder(CheckoutViewModel checkout)
        {
            if (!ModelState.IsValid)
            {
                return View("CheckOut", checkout);
            }

            try
            {
                _orderService.ProcessCashOrder(checkout);
                return View("OrderSuccess");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("CheckOut", checkout);
            }
        }

        public ActionResult PaymentWithPaypal()
        {
            var cartItems = _cartService.GetCart();
            var result = _paymentStrategy.ProcessPayment(this, cartItems);

            if (result is ViewResult viewResult &&
                (viewResult.ViewName == "Success" || viewResult.ViewName == "Failure"))
            {
                _cartService.ClearCart();
            }

            return result;
        }
    }
}
