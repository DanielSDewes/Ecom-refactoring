using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> GetAllOrdered()
        {
            using (var db = new ChienVHShopDBEntities())
            {
                return db.Categories
                         .OrderBy(x => x.Name)
                         .ToList();
            }
        }

        public Category GetById(int id)
        {
            using (var db = new ChienVHShopDBEntities())
            {
                return db.Categories.Find(id);
            }
        }

        public void Add(Category category)
        {
            using (var db = new ChienVHShopDBEntities())
            {
                db.Categories.Add(category);
                db.SaveChanges();
            }
        }

        public void Update(Category category)
        {
            using (var db = new ChienVHShopDBEntities())
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(Category category)
        {
            using (var db = new ChienVHShopDBEntities())
            {
                var toDelete = db.Categories.Find(category.CategoryId);
                if (toDelete != null)
                {
                    db.Categories.Remove(toDelete);
                    db.SaveChanges();
                }
            }
        }

        public bool ExistsByName(string name, int? excludeId = null)
        {
            using (var db = new ChienVHShopDBEntities())
            {
                var query = db.Categories.Where(c => c.Name == name);

                if (excludeId.HasValue)
                {
                    query = query.Where(c => c.CategoryId != excludeId.Value);
                }

                return query.Any();
            }
        }
    }
}
