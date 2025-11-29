using System.Collections.Generic;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllOrdered();
        Category GetById(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);
        bool ExistsByName(string name, int? excludeId = null);
    }
}
