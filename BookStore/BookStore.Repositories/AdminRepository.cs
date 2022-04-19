using BookStore.Models;
using BookStore.Models.Data;
using BookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Repositories
{
    public class AdminRepository
    {

        #region Admin

       public BaseList<User> GetAllUsers(int pageIndex, int pageSize, string userName, int role, string email)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                var query = db.Users.AsQueryable();


                BaseList<User> users = new BaseList<User>();

                if (userName != null)
                {
                    string[] names = userName.Split(" ");
                    if (names.Length > 1)
                    {
                        query = query.Where(x => (x.FirstName.Contains(names[0]) && x.LastName.Contains(names[1])) || (x.FirstName.Contains(names[1]) && x.LastName.Contains(names[0])));
                    }
                    else
                    {   
                        query = query.Where(x => x.FirstName.Contains(userName) || x.LastName.Contains(userName));
                    }

                }

                if(role != 0)
                {
                    query = query.Where(x => x.RoleId == role);
                }

                if(email != null)
                {
                    query = query.Where(x => x.Email.Contains(email));
                }

                users.TotalRecords = query.Count();

                if(pageSize!= 0)
                {
                    query = query.Skip(pageSize * pageIndex).Take(pageSize);
                }

                users.Records = query.ToList();

                return users;

            }
        }


        public User ToggleActivation(int userId)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                User user = db.Users.FirstOrDefault(x => x.UserId == userId);
                if (user == null)
                    throw new Exception("User not found!");

                user.IsActive = user.IsActive == true ? false : true;

                db.Users.Update(user);
                db.SaveChanges();

                return user;
            }
        }

        public BaseList<AdminBook> GetAllBooks(int pageIndex, int pageSize, string bookName, int categoryId, int publisherId, int authorId) 
        { 
            using(UnitOfWork db = new UnitOfWork())
            {
                var query = db.Books.AsQueryable().Where(x=> x.IsDeleted == false);
                
                BaseList<AdminBook> books = new BaseList<AdminBook>();

                if(bookName != null && bookName != "")
                {
                    query = query.Where(x => x.Name.Contains(bookName));
                }

                if(categoryId != 0)
                {
                    query = query.Where(x => x.CategoryId == categoryId);
                }

                if(publisherId != 0)
                {
                    query = query.Where(x => x.PublisherId == publisherId);
                }

                if(authorId != 0)
                {
                    query = query.Where(x => x.AuthorId == authorId);
                }

                books.TotalRecords = query.Count();

                if(pageSize != 0)
                {
                    query = query.Skip(pageSize * pageIndex).Take(pageSize);
                }

                List<Book> queryList = query.ToList();

                List<AdminBook> records = new List<AdminBook>();

                foreach(Book book in queryList)
                {
                    AdminBook adminBook = new AdminBook();
                    adminBook.BookId = book.BookId;
                    adminBook.Name = book.Name;
                    adminBook.Price = book.Price;
                    adminBook.CreatedOn = book.CreatedOn;
                    adminBook.Inventory = book.Inventory;
                    
                    User author = db.Users.FirstOrDefault(x => x.UserId == book.AuthorId);
                    adminBook.Author = author.FirstName + " " + author.LastName;
                    
                    User publisher = db.Users.FirstOrDefault(x => x.UserId == book.PublisherId);
                    adminBook.Publisher = publisher.FirstName + " " + publisher.LastName;

                    Category category = db.Categories.FirstOrDefault(x => x.CategoryId == book.CategoryId);
                    adminBook.Category = category.CategoryName;

                    /*Publisher publisher = db.Publishers.FirstOrDefault(x => x.PublisherId == book.PublisherId);
                    adminBook.Publisher = publisher.FirstName + " " + publisher.LastName;

                    Author author = db.Authors.FirstOrDefault(x => x.AuthorId == book.AuthorId);
                    adminBook.Author = author.AuthorName;
*/
         /*           Inventory inventory = db.Inventories.FirstOrDefault(x => x.BookId == book.BookId);
                    if (inventory != null)
                        adminBook.Inventory = inventory.Copies;
*/
                    records.Add(adminBook);
                }
                books.Records = records;

                return books;
            }
        }
        #endregion

    }
}
