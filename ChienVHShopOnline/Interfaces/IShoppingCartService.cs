using System.Collections.Generic;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Interfaces
{
    public interface IShoppingCartService
    {
        IList<Cart> GetCart();
        void AddProduct(int productId);
        void RemoveProduct(int productId);
        void UpdateQuantities(int[] quantities);
        void ClearCart();
    }
}
