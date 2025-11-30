using ChienVHShopOnline.ViewModels;

namespace ChienVHShopOnline.Interfaces
{
    public interface IOrderService
    {
        void ProcessCashOrder(CheckoutViewModel checkout);
    }
}
