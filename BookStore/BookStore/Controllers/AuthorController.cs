using BookStore.Models;
using BookStore.Models.Data;
using BookStore.Models.ViewModels;
using BookStore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/Author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {

        #region author
        /// <summary>
        /// Add author to database
        /// </summary>
        /// <params name="author">author as Object of Author</params>
        /// <return>Added author</return>
        /*[HttpPost]
       [Route ("Add")]
       public IActionResult AddAuthor(Author author)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();
                Author result = authorAndPublisherRepository.AddAuthor(author);

                ResultStateWithModel<Author> authorResultState = new ResultStateWithModel<Author>();
                authorResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, authorResultState);

            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
       }
*/
        /// <summary>
        /// Update author to database
        /// </summary>
        /// <params name="author">author as Object of Author</params>
        /// <return>Updated author</return>
  /*      [HttpPut]
        [Route("Update")]
        public IActionResult UpdateAuthor(Author author)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();

                Author isAvailable = authorAndPublisherRepository.GetAuthorById(author.AuthorId);

                Author result = authorAndPublisherRepository.UpdateAuthor(author);

                ResultStateWithModel<Author> authorResultState = new ResultStateWithModel<Author>();
                authorResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, authorResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }
*/
        /// <summary>
        /// Update author to database
        /// </summary>
        /// <params name="id">id as int</params>
        /// <return>Updated author</return>
  /*      [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteAuthor(int id)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();

                Author author = authorAndPublisherRepository.GetAuthorById(id);

                Author result = authorAndPublisherRepository.DeleteAuthor(author);

                ResultStateWithModel<Author> authorResultState = new ResultStateWithModel<Author>();
                authorResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, authorResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }
*/
        /// <summary>
        /// get all author
        /// </summary>
        /// <params name="pageIndex">pageIndex as int</params>
        /// <params name="pageSize">pageSize as int</params>
        /// <params name="keyword">keyword as string</params>
        /// <return>Get all author</return>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllAuthor(int pageIndex, int pageSize, string keyword)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();

                
                BaseList<AuthorPublisher> result = authorAndPublisherRepository.GetAllAuthor(pageIndex, pageSize, keyword);

                ResultStateWithModel<BaseList<AuthorPublisher>> authorResultState = new ResultStateWithModel<BaseList<AuthorPublisher>>();
                authorResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, authorResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// Get author by book id
        /// </summary>
        /// <params name="id">id as int</params>
        /// <return>Get author by book id</return>
        /*[HttpGet]
        [Route("GetAuthorByBook")]
        public IActionResult GetAuthorByBook(int id)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();

                BookRepository bookRepo = new BookRepository();
                int authorId = bookRepo.GetBookById(id).AuthorId;

                Author result = authorAndPublisherRepository.GetAuthorById(authorId);

                ResultStateWithModel<Author> authorResultState = new ResultStateWithModel<Author>();
                authorResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, authorResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }
*/
        /// <summary>
        /// get all books by author
        /// </summary>
        /// <params name="id">id as int</params>
        /// <params name="pageIndex">pageIndex as int</params>
        /// <params name="pageSize">pageSize as int</params>
        /// <params name="keyword">keyword as string</params>
        /// <return>Get all books by author</return>
        /*[HttpGet]
        [Route("GetAllBooksByAuthor")]
        public IActionResult GetAllBooksByAuthor(int id, int pageIndex, int pageSize, string keyword)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();


                BaseList<Book> result = authorAndPublisherRepository.GetBooksByAuthor(id, pageIndex, pageSize, keyword);

                ResultStateWithModel<BaseList<Book>> authorResultState = new ResultStateWithModel<BaseList<Book>>();
                authorResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, authorResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }*/
        #endregion
    }
}
