using BookStore.Models.Data;
using BookStore.Models.ViewModels;
using BookStore.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BookStore.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;


namespace BookStore.Controllers
{
    [Route("api/Order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        ///<summary>
        ///Add Order
        /// </summary>
        /// <param name="order">order as object of order</param>
        /// <return>Added order object</return>
        [HttpPost]
        [Route("Add")]
        public IActionResult AddOrder(Order order)
        {
            try
            {
                OrderRepository orderRepo = new OrderRepository();

                order.UpdatedOn = DateTime.Now;
                order.OrderDate = DateTime.Now;
                order.Status = 1;
                Order add = orderRepo.AddOrder(order);

                ResultStateWithModel<Order> orderResultState = new ResultStateWithModel<Order>();
                orderResultState.Data = add;

                return StatusCode((int)HttpStatusCode.OK, orderResultState);
            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }


        ///<summary>
        ///Cancel order
        ///</summary>
        ///<param name="order">order as object of cancelorder</param>
        ///<return>cancelled order</return>
        [HttpPost]
        [Route("Cancel")]
        public IActionResult CancelOrder(CancelOrder order)
        {
            try
            {
                OrderRepository orderRepo = new OrderRepository();
                Order orderForCancel = orderRepo.OrderById(order.OrderId);

                UserRepository userRepo = new UserRepository();
                User user = userRepo.GetUserById(orderForCancel.OrderBy);

                if (user.Password != order.Password)
                    throw new Exception("Password was incorrect! try again");

                orderForCancel.Status = 4;

                Order cancelledOrder = orderRepo.UpdateOrder(orderForCancel);

                ResultStateWithModel<Order> orderResultState = new ResultStateWithModel<Order>();
                orderResultState.Data = cancelledOrder;

                return StatusCode((int)HttpStatusCode.OK, orderResultState);
            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }


        [HttpPost]
        [Route("ChangeStatus")]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeStatus(Order order)
        {
            try
            {
                OrderRepository orderRepo = new OrderRepository();
                Order myOrder = orderRepo.OrderById(order.OrderId);
                myOrder.Status = order.Status;
                Order updatedOrder = orderRepo.UpdateOrder(myOrder);

                ResultStateWithModel<Order> orderResultState = new ResultStateWithModel<Order>();
                orderResultState.Data = updatedOrder;

                return StatusCode((int)HttpStatusCode.OK, orderResultState);
            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        ///<summary>
        ///Get all Order for admin page
        ///</summary>
        ///<param name="pageIndex">pageIndex as int</param>
        ///<param name="pageSize">pageSize as int</param>
        ///<param name="bookName">bookName as string</param>
        ///<param name="userName">userName as string</param>
        ///<param name="status">status as int</param>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllOrders(int pageIndex, int pageSize, string bookName, string userName, int status)
        {
            try
            {
                OrderRepository orderRepo = new OrderRepository();
                BaseList<Order> orders = orderRepo.GetAllorders(pageIndex, pageSize, bookName, userName, status);

                ResultStateWithModel<BaseList<Order>> orderResultState = new ResultStateWithModel<BaseList<Order>>();
                orderResultState.Data = orders;

                return StatusCode((int)HttpStatusCode.OK, orderResultState);
            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        ///<summary>
        ///Get ordered books by user
        /// </summary>
        /// <param name="userId">userId as int</param>
        /// <return>ordered books by user</return>
        [HttpGet]
        [Route("OrderedBook")]
        public IActionResult GetOrderedBookById(int userId)
        {
            try
            {
                OrderRepository orderRepo = new OrderRepository();
                BaseList<Book> books = orderRepo.OrderedBookByUser(userId);

                foreach(Book book in books.Records)
                {
                    string imageName = book.Image;
                    string path = "BookCover/" + imageName;
                    if (System.IO.File.Exists(path))
                    {
                        byte[] b = System.IO.File.ReadAllBytes(path);
                        book.GetImage = File(b, "image/jpg");
                    }
                }

                ResultStateWithModel<BaseList<Book>> orderResultState = new ResultStateWithModel<BaseList<Book>>();
                orderResultState.Data = books;

                return StatusCode((int)HttpStatusCode.OK, orderResultState);
            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }


    }
}
