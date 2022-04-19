using BookStore.Models;
using BookStore.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BookStore.Repositories
{
    public class OrderRepository
    {
        #region order
        public Order AddOrder(Order order)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                var result = db.Orders.Add(order);
                db.SaveChanges();
                return result.Entity;
            }
        }

        public Order UpdateOrder(Order order)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                db.Orders.Update(order);
                db.SaveChanges();
                return order;
            }
        }

        public Order OrderById(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                Order order = db.Orders.FirstOrDefault(x => x.OrderId == id);
                if (order == null)
                    throw new Exception($"Order was not found with given id {id}");

                return order;
            }
        }

        public BaseList<Book> OrderedBookByUser(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var query = db.Orders.AsQueryable().Where(x => x.OrderBy == id)
                    .Select(x => new Book
                    {
                        BookId = x.BookId,
                        Name = x.Book.Name,
                        Price = x.Book.Price,
                        Image = x.Book.Image,
                        OrderStatus = x.Status
                    });

                BaseList<Book> result = new BaseList<Book>();
                result.TotalRecords = query.Count();
                result.Records = query.ToList();

                return result;
            }
        }

        public BaseList<Order> GetAllorders(int pageIndex, int pageSize, string BookName, string userName, int status)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var query = db.Orders.AsQueryable();

                BaseList<Order> result = new BaseList<Order>();

                if (BookName != null)
                {
                    query = query.Where(x=> x.Book.Name.Contains(BookName));
                }

                if(userName != null)
                {
                    query = query.Where(x => x.OrderByNavigation.FirstName.Contains(userName) || x.OrderByNavigation.LastName.Contains(userName));
                }

                if(status != 0)
                {
                    query = query.Where(x => x.Status == status);
                }

                result.TotalRecords = query.Count();

                if(pageSize != 0)
                {
                    query = query.Skip(pageSize * pageIndex).Take(pageSize);
                }

                result.Records = query.ToList();

                foreach(Order record in result.Records)
                {
                    record.OrderByNavigation = db.Users.FirstOrDefault(x => x.UserId == record.OrderBy);
                    record.Book = db.Books.FirstOrDefault(x => x.BookId == record.BookId);
                }

                return result;
            }
        }

        #endregion
    }
}
