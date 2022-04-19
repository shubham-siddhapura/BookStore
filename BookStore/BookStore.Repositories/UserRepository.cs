using BookStore.Models;
using BookStore.Models.Data;
using BookStore.Models.ViewModels;
using BookStore.Repositories;
using System;
using System.Linq;

namespace BookStore.Repositories
{
    public class UserRepository
    {

        public UserDetail RegisterUser(RegisterUserRequest user)
        {
            using (UnitOfWork db = new UnitOfWork())
            {

                User userExist = db.Users.FirstOrDefault(x => x.Email == user.Email);
                if (userExist != null)
                    throw new Exception("User is already exist");

                //Add user to database
                User objUser = new User();
                objUser.FirstName = user.FirstName;
                objUser.LastName = user.LastName;
                objUser.Email = user.Email;
                objUser.RoleId = Convert.ToInt32(user.RoleID);
                objUser.Password = user.Password;
                db.Users.Add(objUser);
                db.SaveChanges();

                //Prepare and return response 
                UserDetail userResult = new UserDetail();
                userResult.Id = objUser.UserId;
                userResult.FirstName = objUser.FirstName;
                userResult.LastName = objUser.LastName;
                userResult.Email = objUser.Email;
                userResult.Password = objUser.Password;
                userResult.RoleId = objUser.RoleId;
                return userResult;
            }
        }

        public UserDetail Login(LoginRequest loginRequest)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                User user = db.Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.Email == loginRequest.Email && x.Password == loginRequest.Password ).FirstOrDefault();

                if (user == null)
                {
                    throw new Exception(Messages.InvalidCredentialsMessage);
                }
                else
                {
                    UserDetail userResult = new UserDetail();
                    userResult.Id = user.UserId;
                    userResult.FirstName = user.FirstName;
                    userResult.LastName = user.LastName;
                    userResult.Email = user.Email;
                    userResult.Password = user.Password;
                    userResult.RoleId = user.RoleId;
                    
                    if(user.RoleId == 1)
                    {
                        userResult.RoleName = "Admin";
                    }
                    else if(user.RoleId == 2)
                    {
                        userResult.RoleName = "User";
                    }
                    else if(user.RoleId == 3)
                    {
                        userResult.RoleName = "Author";
                    }
                    else
                    {
                        userResult.RoleName = "Publisher";
                    }
                   
                    return userResult;
                }
            }
        }

        #region UserMaster
        public User GetUserById(int id)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                User user = db.Users.FirstOrDefault(x => x.UserId == id);
                if(user == null)
                {
                    throw new Exception(Messages.InvalidCredentialsMessage);
                }
                else
                {
                    return user;
                }
            }
        }

        public User UpdateUser(User user)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.Users.Update(user);
                db.SaveChanges();
                return user;
            }
        }
        
        public User DeleteUser(User user)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return user;
            }
        }

        public BaseList<User> GetAllUser(int pageIndex, int pageSize, string keyword)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                var query = db.Users.AsQueryable();

                BaseList<User> result = new BaseList<User>();
                result.TotalRecords = query.Count();

                if(pageSize != 0)
                {
                    keyword = keyword != null ? keyword : string.Empty;

                    //Fetch all the records where keyword is part of Firstname or Lastname or Email. Then skip first {{pageIndex * pageSize}} records and take next {{pageSize}} records.
                    query = query.Where(x => x.FirstName.Contains(keyword) || x.LastName.Contains(keyword) || x.Email.Contains(keyword)).Skip(pageIndex * pageSize).Take(pageSize);
                }

                result.Records = query.ToList();
                return result;
            }
        }

        public BaseList<Role> GetAllRoles()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var query = db.Roles.AsQueryable();

                BaseList<Role> result = new BaseList<Role>();
                result.TotalRecords = query.Count();
                result.Records = query.ToList();

                return result;

            }
        }

        public User changePassword(ChangePassword changePwd)
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                User user = db.Users.FirstOrDefault(x => x.UserId == changePwd.id);
                if (user == null)
                    throw new Exception("User does not exist");

                if (user.Password.TrimEnd() != changePwd.oldPwd)
                    throw new Exception("Invalid credential");

                user.Password = changePwd.newPwd;
                db.Users.Update(user);
                db.SaveChanges();

                return user;
            }
        }

        #endregion


    }
}
