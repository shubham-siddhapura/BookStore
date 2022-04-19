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
    [Route("api/Publisher")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        #region Publisher
        /// <summary>
        /// Add publisher to database
        /// </summary>
        /// <params name="publisher">publisher as Object of Publisher</params>
        /// <return>Added publisher</return>
        /*[HttpPost]
        [Route("Add")]
        public IActionResult AddPublisher(Publisher publisher)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();
                Publisher result = authorAndPublisherRepository.AddPublisher(publisher);

                ResultStateWithModel<Publisher> publisherResultState = new ResultStateWithModel<Publisher>();
                publisherResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, publisherResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }*/

        /// <summary>
        /// Update publisher to database
        /// </summary>
        /// <params name="publisher">publisher as Object of Publisher</params>
        /// <return>Updated publisher</return>
        /*[HttpPut]
        [Route("Update")]
        public IActionResult UpdatePublisher(Publisher publisher)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();

                Publisher isAvailable = authorAndPublisherRepository.GetPublisherById(publisher.PublisherId);

                Publisher result = authorAndPublisherRepository.UpdatePublisher(publisher);

                ResultStateWithModel<Publisher> publisherResultState = new ResultStateWithModel<Publisher>();
                publisherResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, publisherResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }*/

        /// <summary>
        /// Delete Publisher
        /// </summary>
        /// <params name="id">id as int</params>
        /// <return>Delete Publisher</return>
        /*[HttpDelete]
        [Route("Delete")]
        public IActionResult DeletePublisher(int id)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();

                Publisher publisher = authorAndPublisherRepository.GetPublisherById(id);

                Publisher result = authorAndPublisherRepository.DeletePublisher(publisher);

                ResultStateWithModel<Publisher> publisherResultState = new ResultStateWithModel<Publisher>();
                publisherResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, publisherResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }*/

        /// <summary>
        /// get all publisher
        /// </summary>
        /// <params name="pageIndex">pageIndex as int</params>
        /// <params name="pageSize">pageSize as int</params>
        /// <params name="keyword">keyword as string</params>
        /// <return>Get all publisher</return>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllPublisher(int pageIndex, int pageSize, string keyword)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();


                BaseList<AuthorPublisher> result = authorAndPublisherRepository.GetAllPublisher(pageIndex, pageSize, keyword);

                ResultStateWithModel<BaseList<AuthorPublisher>> publisherResultState = new ResultStateWithModel<BaseList<AuthorPublisher>>();
                publisherResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, publisherResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// Get Publisher by book id
        /// </summary>
        /// <params name="id">id as int</params>
        /// <return>Get publisher by book id</return>
        /*[HttpGet]
        [Route("GetPublisherByBook")]
        public IActionResult GetPublisherByBook(int id)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();

                BookRepository bookRepo = new BookRepository();
                int publisherId = bookRepo.GetBookById(id).PublisherId;

                Publisher result = authorAndPublisherRepository.GetPublisherById(publisherId);

                ResultStateWithModel<Publisher> publisherResultState = new ResultStateWithModel<Publisher>();
                publisherResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, publisherResultState);

            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }*/

        /// <summary>
        /// get all books by publisher
        /// </summary>
        /// <params name="id">id as int</params>
        /// <params name="pageIndex">pageIndex as int</params>
        /// <params name="pageSize">pageSize as int</params>
        /// <params name="keyword">keyword as string</params>
        /// <return>Get all books by publisher</return>
        /*[HttpGet]
        [Route("GetAllBooksByPublisher")]
        public IActionResult GetAllBooksByPublisher(int id, int pageIndex, int pageSize, string keyword)
        {
            try
            {
                AuthorAndPublisherRepository authorAndPublisherRepository = new AuthorAndPublisherRepository();


                BaseList<Book> result = authorAndPublisherRepository.GetBooksByPublisher(id, pageIndex, pageSize, keyword);

                ResultStateWithModel<BaseList<Book>> publisherResultState = new ResultStateWithModel<BaseList<Book>>();
                publisherResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, publisherResultState);

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
