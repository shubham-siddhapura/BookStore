using BookStore.Models;
using BookStore.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Repositories
{
    public class BookRepository
    {

        #region Book

        public BaseList<Book> GetAllBooks(int pageIndex, int pageSize, string keyword, int category)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var query = db.Books.AsQueryable();

                BaseList<Book> result = new BaseList<Book>();
                
                if(category != 0)
                {
                    query = query.Where(x => x.CategoryId == category);
                }

                result.TotalRecords = query.Count();


                if (pageSize != 0)
                {
                    keyword = keyword != null ? keyword : string.Empty;

                    query = query.Where(x=> x.Name.Contains(keyword) || x.Category.CategoryName.Contains(keyword) ||  x.Author.FirstName.Contains(keyword) || x.Author.LastName.Contains(keyword) || x.Publisher.FirstName.Contains(keyword) || x.Publisher.LastName.Contains(keyword)).Skip(pageIndex * pageSize).Take(pageSize);
                    
                }
                result.Records = query.ToList();


                return result;
            }
        }

        
        public Book GetBookById(int id)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                Book book = db.Books.FirstOrDefault(x => x.BookId == id);
                if (book == null)
                    throw new Exception($"book not found with id {id}");

                return book;
            }
        }
        
        public Book Add(Book book)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.Books.Add(book);
                db.SaveChanges();
                return book;
            }
        }

        public Book Update(Book book)
        {
            using(UnitOfWork db = new UnitOfWork())
            {                
                db.Books.Update(book);
                db.SaveChanges();
                return book;
            }
        }

        public Book Delete(int id)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                Book book = db.Books.FirstOrDefault(x=> x.BookId == id);
                if (book == null)
                    throw new Exception($"Book not found with Id {id}");

                db.Books.Remove(book);
                db.SaveChanges();
                return book;
            }
        }
        #endregion

        #region inventory
        /* public Inventory AddInventory(Inventory inventory)
         {
             using (UnitOfWork db = new UnitOfWork())
             {
                 Inventory result = db.Inventories.FirstOrDefault(x=> x.BookId == inventory.BookId);

                 if(result == null)
                 {
                     Inventory addInventory = new Inventory();
                     addInventory.BookId = inventory.BookId;
                     addInventory.Copies = inventory.Copies;

                     db.Inventories.Add(addInventory);
                     db.SaveChanges();
                     return addInventory;
                 }
                 else
                 {
                     result.Copies = inventory.Copies + result.Copies;
                     db.Inventories.Update(result);
                     db.SaveChanges();
                     return result;
                 }
             }
         }

         public Inventory GetInventoryByBook(int bookId)
         {
             using(UnitOfWork db = new UnitOfWork())
             {
                 Inventory inventory = db.Inventories.FirstOrDefault(x => x.BookId == bookId);

                 if (inventory == null)
                     throw new Exception($"Inventory not found for given book id {bookId}");

                 return inventory;
             }
         }*/

        public Book AddInventory(Book book)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                Book result = db.Books.FirstOrDefault(x => x.BookId == book.BookId);

                if (result == null)
                {
                    throw new Exception($"Book not found with {book.BookId} id.");
                }
                else
                {
                    result.Inventory = book.Inventory;
                    db.Books.Update(result);
                    db.SaveChanges();
                    return result;
                }
            }
        }

        /*public Inventory GetInventoryByBook(int bookId)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                Inventory inventory = db.Inventories.FirstOrDefault(x => x.BookId == bookId);

                if (inventory == null)
                    throw new Exception($"Inventory not found for given book id {bookId}");

                return inventory;
            }
        }*/

        #endregion

        #region publisher
        public Publisher AddPublisher(Publisher publisher)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                var result = db.Publishers.Add(publisher);
                db.SaveChanges();
                return result.Entity;
            }
        }
        
        public Publisher UpdatePublisher(Publisher publisher)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                var result = db.Publishers.Update(publisher);
                db.SaveChanges();
                return result.Entity;
            }
        }

        public Publisher DeletePublisher(Publisher publisher)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                publisher.IsDeleted = true;
                var result = db.Publishers.Update(publisher);
                db.SaveChanges();
                return result.Entity;
            }
        }

        public Publisher GetPublisherById(int id)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                Publisher publisher = db.Publishers.FirstOrDefault(x => x.PublisherId == id && x.IsDeleted == false);
                if (publisher == null)
                    throw new Exception($"Publisher not found with id {id}");

                return publisher;
            }
        }

        public BaseList<Publisher> GetAllPublisher(int pageIndex, int pageSize, string keyword)
        {
            using(UnitOfWork db = new UnitOfWork())
            {

                var query = db.Publishers.AsQueryable();

                BaseList<Publisher> result = new BaseList<Publisher>();
                result.TotalRecords = query.Count();

                if (pageSize != 0)
                {
                    keyword = keyword != null ? keyword : string.Empty;

                    query = query.Where(x =>x.IsDeleted == false && ( x.FirstName.Contains(keyword) || x.LastName.Contains(keyword))).Skip(pageIndex * pageSize).Take(pageSize);
                }

                result.Records = query.ToList();

                return result;

            }
        }

        public BaseList<Book> GetBooksByPublisher(int id, int pageIndex, int pageSize, string keyword)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var query = db.Books.AsQueryable();

                BaseList<Book> result = new BaseList<Book>();
                result.TotalRecords = query.Count();

                if (pageSize != 0)
                {
                    keyword = keyword != null ? keyword : string.Empty;

                    query = query.Where(x =>x.PublisherId == id && (x.Name.Contains(keyword) || x.Category.CategoryName.Contains(keyword))).Skip(pageIndex * pageSize).Take(pageSize);
                }

                result.Records = query.ToList();

                return result;

            }
        }
        #endregion
    }
}
