using BookStore.Models;
using BookStore.Models.Data;
using BookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Repositories
{
    public class AuthorAndPublisherRepository
    {

        #region publisher
/*
        public Publisher AddPublisher(Publisher publisher)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var result = db.Publishers.Add(publisher);
                db.SaveChanges();
                return result.Entity;
            }
        }

        public Publisher UpdatePublisher(Publisher publisher)
        {
            using (UnitOfWork db = new UnitOfWork())
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
*/
        /*public Publisher GetPublisherById(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                Publisher publisher = db.Publishers.FirstOrDefault(x => x.PublisherId == id && x.IsDeleted == false);
                if (publisher == null)
                    throw new Exception($"Publisher not found with id {id}");

                return publisher;
            }
        }
*/
        public BaseList<AuthorPublisher> GetAllPublisher(int pageIndex, int pageSize, string keyword)
        {
            using (UnitOfWork db = new UnitOfWork())
            {

                var query = db.Users.AsQueryable().Where(x => x.RoleId == 4)
                    .Select(x => new AuthorPublisher{ 
                                    UserId = x.UserId,
                                    FirstName =  x.FirstName,
                                    LastName = x.LastName, 
                                    IsDeleted = x.IsDeleted,
                                    IsActive = x.IsActive
                    });

                BaseList<AuthorPublisher> result = new BaseList<AuthorPublisher>();
                result.TotalRecords = query.Count();

                if (pageSize != 0)
                {
                    keyword = keyword != null ? keyword : string.Empty;

                    query = query.Where(x => x.IsDeleted == false && x.IsActive == true && (x.FirstName.Contains(keyword) || x.LastName.Contains(keyword))).Skip(pageIndex * pageSize).Take(pageSize);
                }

                result.Records = query.ToList();

                return result;

            }
        }

       /* public BaseList<Book> GetBooksByPublisher(int id, int pageIndex, int pageSize, string keyword)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var query = db.Books.AsQueryable();

                BaseList<Book> result = new BaseList<Book>();
                result.TotalRecords = query.Count();

                if (pageSize != 0)
                {
                    keyword = keyword != null ? keyword : string.Empty;

                    query = query.Where(x => x.PublisherId == id && (x.Name.Contains(keyword) || x.Category.CategoryName.Contains(keyword) || x.Publisher.FirstName.Contains(keyword) || x.Publisher.LastName.Contains(keyword) || x.Author.AuthorName.Contains(keyword))).Skip(pageIndex * pageSize).Take(pageSize);
                }

                result.Records = query.ToList();

                return result;

            }
        }*/
        #endregion

        #region Author

        public BaseList<AuthorPublisher> GetAllAuthor(int pageIndex, int pageSize, string keyword)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                var query = db.Users.AsQueryable().Where(x => x.RoleId == 3)
                    .Select(x => new AuthorPublisher { 
                        UserId = x.UserId,
                        FirstName = x.FirstName,
                        LastName = x.LastName, 
                        IsActive = x.IsActive,
                        IsDeleted = x.IsDeleted
                    });

                BaseList<AuthorPublisher> result = new BaseList<AuthorPublisher>();
                result.TotalRecords = query.Count();

                if (pageSize != 0)
                {
                    keyword = keyword != null ? keyword : string.Empty;

                    query = query.Where(x =>x.IsDeleted == false && x.IsActive == true && (x.FirstName.Contains(keyword)||x.LastName.Contains(keyword))).Skip(pageIndex * pageSize).Take(pageSize);
                }

                result.Records = query.ToList();

                return result;

            }
        }

/*       public Author AddAuthor(Author author)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                var result = db.Authors.Add(author);
                db.SaveChanges();
                return result.Entity;
            }
        }

        public Author UpdateAuthor(Author author)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                db.Authors.Update(author);
                db.SaveChanges();
                return author;
            }
        }

        public Author DeleteAuthor(Author author)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                author.IsDeleted = true;
                db.Authors.Update(author);
                db.SaveChanges();
                return author;
            }
        }

        public Author GetAuthorById(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                Author author = db.Authors.FirstOrDefault(x => x.IsDeleted == false && x.AuthorId == id);
                if (author == null)
                    throw new Exception($"Author not found by id {id}");

                return author;
            }
        }

        public BaseList<Book> GetBooksByAuthor(int id, int pageIndex, int pageSize, string keyword)
        {
            using(UnitOfWork db = new UnitOfWork())
            {

                var query = db.Books.AsQueryable();

                BaseList<Book> result = new BaseList<Book>();
                result.TotalRecords = query.Count();

                if (pageSize != 0)
                {
                    keyword = keyword != null ? keyword : string.Empty;

                    query = query.Where(x => x.AuthorId == id && (x.Name.Contains(keyword) || x.Category.CategoryName.Contains(keyword) || x.Author.AuthorName.Contains(keyword) || x.Publisher.FirstName.Contains(keyword) || x.Publisher.LastName.Contains(keyword))).Skip(pageIndex * pageSize).Take(pageSize);
                }

                result.Records = query.ToList();

                return result;
            }
        }
*/        
        #endregion
    }
}
