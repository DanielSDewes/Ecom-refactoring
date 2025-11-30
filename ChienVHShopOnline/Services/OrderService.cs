using System;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.ViewModels;

namespace ChienVHShopOnline.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartService _cartService;
        private readonly ICheckoutValidator _checkoutValidator;

        public OrderService(
            IOrderRepository orderRepository,
            IShoppingCartService cartService,
            ICheckoutValidator checkoutValidator)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _checkoutValidator = checkoutValidator ?? throw new ArgumentNullException(nameof(checkoutValidator));
        }

        public void ProcessCashOrder(CheckoutViewModel checkout)
        {
            var validation = _checkoutValidator.Validate(checkout);
            if (!validation.IsValid)
            {
                throw new InvalidOperationException(string.Join("; ", validation.Errors));
            }

            var cartItems = _cartService.GetCart();
            if (cartItems == null || cartItems.Count == 0)
            {
                throw new InvalidOperationException("O carrinho está vazio.");
            }

            var order = new Order
            {
                CustomerName = checkout.CustomerName,
                CustomerPhone = checkout.CustomerPhone,
                CustomerEmail = checkout.CustomerEmail,
                CustomerAddress = checkout.CustomerAddress,
                OrderDate = DateTime.Now,
                PaymentType = "Cash",
                Status = "Processing"
            };

            _orderRepository.SaveOrder(order, cartItems);
            _cartService.ClearCart();
        }
    }
}
