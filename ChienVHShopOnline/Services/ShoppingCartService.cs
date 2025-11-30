using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private const string CartSessionKey = "Cart";
        private readonly IProductRepository _productRepository;

        public ShoppingCartService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public IList<Cart> GetCart()
        {
            var session = HttpContext.Current.Session;
            if (session[CartSessionKey] == null)
            {
                session[CartSessionKey] = new List<Cart>();
            }

            return (List<Cart>)session[CartSessionKey];
        }

        public void AddProduct(int productId)
        {
            var cart = GetCart();
            var index = cart.FindIndex(c => c.Product.ProductId == productId);

            if (index == -1)
            {
                var product = _productRepository.GetById(productId);
                if (product == null) return;

                cart.Add(new Cart(product, 1));
            }
            else
            {
                cart[index].Quantity++;
            }
        }

        public void RemoveProduct(int productId)
        {
            var cart = GetCart();
            var index = cart.FindIndex(c => c.Product.ProductId == productId);
            if (index >= 0)
            {
                cart.RemoveAt(index);
            }
        }

        public void UpdateQuantities(int[] quantities)
        {
            if (quantities == null) return;

            var cart = GetCart();
            for (int i = 0; i < cart.Count && i < quantities.Length; i++)
            {
                cart[i].Quantity = quantities[i];
            }
        }

        public void ClearCart()
        {
            var session = HttpContext.Current.Session;
            session.Remove(CartSessionKey);
        }
    }
}
