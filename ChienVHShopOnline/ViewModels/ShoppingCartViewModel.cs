using System.Collections.Generic;
using System.Linq;

namespace ChienVHShopOnline.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IList<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

        public decimal Subtotal => Items.Sum(i => i.Total);
    }
}
