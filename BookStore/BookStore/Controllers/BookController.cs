using BookStore.Models;
using BookStore.Models.Data;
using BookStore.Models.ViewModels;
using BookStore.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
// using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Controllers
{
    [Route("api/Book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        #region Book
        /// <summary>
        /// Returns list of Books.
        /// </summary>
        /// <param name="pageIndex">pageIndex as int. Starts from 0</param>
        /// <param name="pageSize">pageSize as int.</param>
        /// <param name="keyword">keyword as string.</param>
        /// <returns>List of books.</returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllBooks(int pageIndex, int pageSize, string keyword, string category)
        {
            try
            {
                BookRepository bookRepository = new BookRepository();
                
                BaseList<Book> result = bookRepository.GetAllBooks(pageIndex, pageSize, keyword, Convert.ToInt32(category));

                for(int i = 0; i< result.Records.Count(); i++)
                {
                    var fileName = result.Records[i].Image;
                    var path = "BookCover/" + fileName;
                    if (System.IO.File.Exists(path))
                    {
                        byte[] b = System.IO.File.ReadAllBytes(path);
                        result.Records[i].GetImage = File(b, "image/jpg");
                    }
                }

                ResultStateWithModel<BaseList<Book>> allResultState = new ResultStateWithModel<BaseList<Book>>();
                allResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, allResultState);
            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// Returns Added Book.
        /// </summary>
        /// <param name="book">book as object of Book</param>
        /// <returns>Added book</returns>
        [HttpGet]
        [Route("GetById")]
        public IActionResult GetBookById(int id)
        {
            try
            {
                BookRepository bookRepository = new BookRepository();
                Book result = bookRepository.GetBookById(id);

                ResultStateWithModel<Book> getResultState = new ResultStateWithModel<Book>();
                getResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, getResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }


        /// <summary>
        /// Returns Added Book.
        /// </summary>
        /// <param name="book">book as object of Book</param>
        /// <returns>Added book</returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")]     
        public IActionResult AddBook([FromForm]Book book)
        {
            try
            {
                BookRepository bookRepository = new BookRepository();

                var image = book.ImageFile;

                if(image.Length > 0)
                {
                    book.Image = Guid.NewGuid().ToString() + image.FileName;

                    var path = Path.Combine("BookCover/", book.Image);
                    using(var filestream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(filestream);
                    }
                }

                
                Book result = bookRepository.Add(book);


                ResultStateWithModel<Book> addResultState = new ResultStateWithModel<Book>();
                addResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, addResultState);
            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// Returns updated Book.
        /// </summary>
        /// <param name="book">book as object of Book</param>
        /// <returns>updated book</returns>
        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateBook([FromForm] Book book)
        {
            try
            {
                BookRepository bookRepository = new BookRepository();

                var image = book.ImageFile;

                Book isBookExist = bookRepository.GetBookById(book.BookId);

                if(image == null)
                {
                    book.Image = isBookExist.Image;
                }
                else if (image.Length > 0)
                {
                    book.Image = Guid.NewGuid().ToString() + image.FileName;

                    var path = Path.Combine("BookCover/", book.Image);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(filestream);
                    }
                }

                Book result = bookRepository.Update(book);

                ResultStateWithModel<Book> updateResultState = new ResultStateWithModel<Book>();
                updateResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, updateResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// Returns deleted Book.
        /// </summary>
        /// <param name="id">id as int</param>
        /// <returns>deleted book</returns>
        [HttpDelete]
        [Route("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                BookRepository bookRepository = new BookRepository();

                Book result = bookRepository.Delete(id);

                ResultStateWithModel<Book> deleteResultState = new ResultStateWithModel<Book>();
                deleteResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, deleteResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }
        #endregion

        #region inventory
        /// <summary>
        /// add inventory for perticular book.
        /// </summary>
        /// <param name="inventory">inventory as object of Inventory</param>
        /// <returns>Add Inventory for book</returns>
        [HttpPost]
        [Route("AddInventory")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddInventory(Book inventory)
        {
            try
            {
                BookRepository bookRepository = new BookRepository();

                Book result = bookRepository.AddInventory(inventory);

                ResultStateWithModel<Book> inventoryResultState = new ResultStateWithModel<Book>();
                inventoryResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, inventoryResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// get inventory for perticular book.
        /// </summary>
        /// <param name="bookId">bookId as int</param>
        /// <returns>get Inventory for book</returns>
        [HttpGet]
        [Route("GetInventory")]
        public IActionResult GetInventoryByBook(int bookId)
        {
            try
            {
                BookRepository bookRepository = new BookRepository();

                Book result = bookRepository.GetBookById(bookId);

                ResultStateWithModel<Book> inventoryResultState = new ResultStateWithModel<Book>();
                inventoryResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, inventoryResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }
        #endregion

    }
}
