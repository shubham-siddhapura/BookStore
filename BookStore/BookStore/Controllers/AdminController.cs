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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace BookStore.Controllers
{
    [Route("api/Admin")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        /*
        private readonly IConfiguration _configuration;
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }*/

        #region admin
        
        /// <summary>
        /// Get all user to admin.
        /// </summary>
        /// <param name="pageIndex">pageIndex as int. Starts from 0</param>
        /// <param name="pageSize">pageSize as int.</param>
        /// <param name="userName">userName as string.</param>
        /// <param name="roleId">role as int.</param>
        /// <param name="email">email as string.</param>
        /// <returns>Get All users </returns>
        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers(int pageIndex, int pageSize, string userName, int roleId, string email)
        {
            try
            {
                AdminRepository adminRepo = new AdminRepository();
                
                BaseList<User> users = adminRepo.GetAllUsers(pageIndex, pageSize, userName, roleId, email);
                
                ResultStateWithModel<BaseList<User>> usersResultState = new ResultStateWithModel<BaseList<User>>();
                usersResultState.Data = users;

                return StatusCode((int)HttpStatusCode.OK, usersResultState);
            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }

        }


        /// <summary>
        /// toggle user Activation.
        /// </summary>
        /// <param name="userId">userId as int.</param>
        /// <returns> Toggle User Activation </returns>

        [HttpPut]
        [Route("ToggleActivation")]
        [Authorize(Roles = "Admin")]
        public IActionResult ToggleUserActivation(User userId)
        {

            try
            {
                AdminRepository adminRepo = new AdminRepository();
                User user = adminRepo.ToggleActivation(userId.UserId);

                ResultStateWithModel<User> toggleResultState = new ResultStateWithModel<User>();
                toggleResultState.Data = user;

                return StatusCode((int)HttpStatusCode.OK, toggleResultState);
            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }


        /// <summary>
        /// Get All Book for Admin
        /// </summary>
        /// <param name="pageIndex">pageIndex as int. Starts from 0</param>
        /// <param name="pageSize">pageSize as int.</param>
        /// <param name="bookName">bookName as string.</param>
        /// <param name="authorId">role as int.</param>
        /// <param name="categoryId">categoryId as int.</param>
        /// <param name="publisherId">publisherId as int.</param>
        /// <return>Get all books</return>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllBooks")]
        public IActionResult GetAllBooks(int pageIndex, int pageSize, string bookName, int categoryId, int authorId, int publisherId)
        {
            try
            {
                AdminRepository adminRepo = new AdminRepository();

                BaseList<AdminBook> adminBook = adminRepo.GetAllBooks(pageIndex, pageSize, bookName, categoryId, publisherId, authorId);

                ResultStateWithModel<BaseList<AdminBook>> bookResultState = new ResultStateWithModel<BaseList<AdminBook>>();
                bookResultState.Data = adminBook;

                return StatusCode((int)HttpStatusCode.OK, bookResultState);
            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }
        #endregion
    }
}
