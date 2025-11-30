using System.Linq;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Product GetById(int id)
        {
            using (var db = new ChienVHShopDBEntities())
            {
                return db.Products.FirstOrDefault(p => p.ProductId == id);
            }
        }
    }
}
