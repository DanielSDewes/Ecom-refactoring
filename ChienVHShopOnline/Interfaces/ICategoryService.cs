using System.Collections.Generic;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllOrdered();
        Category GetById(int id);
        ServiceResult Create(Category category);
        ServiceResult Update(Category category);
        ServiceResult Delete(int id);
    }

    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
