using BookStore.Models;
using BookStore.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Repositories
{
    public class CategoryRepository
    {

        public BaseList<Category> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var query = db.Categories.AsQueryable();
                BaseList<Category> result = new BaseList<Category>();
                result.TotalRecords = query.Count();

                if (pageSize != 0)
                {
                    query = query.Skip(pageIndex * pageSize).Take(pageSize);
                }

                result.Records = query.ToList();
                return result;
            }
        }


        public Category Add(Category category)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return category;
            }
        }

        public Category Update(Category category)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.Categories.Update(category);
                db.SaveChanges();
                return category;
            }
        }

        public Category Delete(int categoryId)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                Category category = db.Categories.FirstOrDefault(x => x.CategoryId == categoryId);

                if(category == null)
                {
                    throw new Exception($"Category not found with Id {categoryId}");
                }

                db.Categories.Remove(category);
                db.SaveChanges();
                return category;
            }
        }

    }
}
