using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Interfaces
{
    public interface IProductRepository
    {
        Product GetById(int id);
    }
}
