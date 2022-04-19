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
    
    [Route("api/BookStore")]
    [ApiController]
    public class BookStoreController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public BookStoreController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Register
        /// <summary>
        /// Registers user to database.
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>RegisterUserResponse</returns>
        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser([FromBody] RegisterUserRequest user)
        {
            User registerUserResponse = new User();
            try
            {
                UserRepository userRepo = new UserRepository();
                var userResult = userRepo.RegisterUser(user);
                ResultStateWithModel<UserDetail> resultState = new ResultStateWithModel<UserDetail>();
                resultState.Data = userResult;
                return StatusCode((int)HttpStatusCode.OK, resultState);
            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }

        }
        #endregion

        #region Login
        /// <summary>
        /// Validates login credentials
        /// </summary>
        /// <param name="loginRequest">loginRequest</param>
        /// <returns>LoginResponse</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                UserRepository userRepo = new UserRepository();

                var userResult = userRepo.Login(loginRequest);

                if(userResult != null)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, userResult.Email),
                        new Claim(ClaimTypes.Role, userResult.RoleName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddDays(7),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    userResult.Token = new JwtSecurityTokenHandler().WriteToken(token);

                    userResult.ExpireToken = token.ValidTo;

                    var cookieOption = new CookieOptions()
                    {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddDays(7)
                    };

                    Response.Cookies.Append("token", userResult.Token, cookieOption);
                }

                ResultStateWithModel<UserDetail> resultState = new ResultStateWithModel<UserDetail>();
                resultState.Data = userResult;
               
                return StatusCode((int)HttpStatusCode.OK, resultState);
            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }
        #endregion


        #region UserMaster

        /// <summary>
        /// Get User By Id.
        /// </summary>
        /// <param name="id">id as int.</param>
        /// <returns>user object response</returns>
        [Authorize]
        [HttpGet("GetById")]
        public IActionResult GetUserById([FromQuery] int id)
        {
            try
            {
                UserRepository userRepo = new UserRepository();
                User user = userRepo.GetUserById(id);
                ResultStateWithModel<User> resultState = new ResultStateWithModel<User>();
                resultState.Data = user;
                return StatusCode((int)HttpStatusCode.OK, resultState);
            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }


        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="user">UpdateUserRequest as user.</param>
        /// <returns>user object response.</returns>
        
        [HttpPut("UpdateUser")]
        [Authorize]
        public IActionResult UpdateUser([FromBody] UpdateUserRequest user)
        {
            try
            {
                UserRepository userRepo = new UserRepository();
                User existingUser = userRepo.GetUserById(user.Id);
                if(existingUser == null)
                {
                    ResultState resultState = new ResultState(Messages.UserNotFoundCode, "Failed", Messages.UserNotFoundMessage);
                    return StatusCode((int)HttpStatusCode.NotFound, resultState);
                }
                existingUser.LastName = user.LastName;
                existingUser.FirstName = user.FirstName;
                if(user.RoleId != 0)
                    existingUser.RoleId = user.RoleId;
                User result = userRepo.UpdateUser(existingUser);

                ResultStateWithModel<User> userUpdateResultState = new ResultStateWithModel<User>();

                userUpdateResultState.Data = result;
                return StatusCode((int)HttpStatusCode.OK, userUpdateResultState);
            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="id">id as int.</param>
        /// <returns>user object response.</returns>
        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                UserRepository userRepo = new UserRepository();
                
                User existingUser = userRepo.GetUserById(id);
                if(existingUser == null)
                {
                    ResultState resultState = new ResultState(Messages.UserNotFoundCode, "Failed", Messages.UserNotFoundMessage);
                    return StatusCode((int)HttpStatusCode.OK, resultState);
                }

                User result = userRepo.DeleteUser(existingUser);

                ResultStateWithModel<User> deleteUserResultState = new ResultStateWithModel<User>();

                deleteUserResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, deleteUserResultState);

            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// Returns list of existing users.
        /// </summary>
        /// <param name="pageIndex">pageIndex as int. Starts from 0</param>
        /// <param name="pageSize">pageSize as int.</param>
        /// <param name="keyword">Keyword as string.</param>
        /// <returns>List of Users.</returns>
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers(int pageIndex, int pageSize, string keyword)
        {
            try
            {

                UserRepository userRepo = new UserRepository();
                BaseList<User> users = userRepo.GetAllUser(pageIndex, pageSize, keyword);

                ResultStateWithModel<BaseList<User>> userAllResultState = new ResultStateWithModel<BaseList<User>>();
                userAllResultState.Data = users;

                return StatusCode((int)HttpStatusCode.OK, userAllResultState);

            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// Returns list of all Roles.
        /// </summary>
        /// <returns>List of Roles.</returns>
        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            try
            {
                UserRepository userRepo = new UserRepository();
                BaseList<Role> roles = userRepo.GetAllRoles();

                ResultStateWithModel<BaseList<Role>> resultState = new ResultStateWithModel<BaseList<Role>>();
                resultState.Data = roles;
                return StatusCode((int)HttpStatusCode.OK, resultState);
            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// Change password.
        /// </summary>
        /// <param name="changePwd">changePwd object of changePassword</param>
        /// <returns>change password</returns>
        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(ChangePassword changePwd)
        {
            try
            {
                UserRepository userRepo = new UserRepository();
                User user = userRepo.changePassword(changePwd);

                ResultStateWithModel<User> userResultState = new ResultStateWithModel<User>();
                userResultState.Data = user;

                return StatusCode((int)HttpStatusCode.OK, userResultState);
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
