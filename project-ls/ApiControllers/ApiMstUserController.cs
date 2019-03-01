using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;



namespace project_ls.ApiControllers
{
    public class ApiMstUserController : ApiController
    {
        private Data.librarydbDataContext db = new Data.librarydbDataContext();

        // ==========
        // Add - User
        // ==========
        [HttpPost, Route("api/user/add")]
        public HttpResponseMessage AddUser(Entities.MstUser objMstUser)
        {
            try
            {
                Data.MstUser mstUser = new Data.MstUser
                {
                    UserName = objMstUser.UserName,
                    FirstName = objMstUser.FirstName,
                    LastName = objMstUser.LastName,
                    Email = objMstUser.Email,
                    Password = objMstUser.Password,
                    UserTypeId = objMstUser.UserTypeId
                };
                db.MstUsers.InsertOnSubmit(mstUser);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }
        // ===============
        // List UserTypeId
        // ===============
        [HttpGet, Route("api/user/UserType")]
        public List<Entities.MstUserType> UserTypeList()
        {
            var userType = from d in db.MstUserTypes
                           select new Entities.MstUserType
                           {
                               Id = d.Id,
                               UserType = d.UserType
                           };

            return userType.ToList();
        }

        // ===========
        // List - User
        // ===========
        [HttpGet, Route("api/user/list")]
        public List<Entities.MstUser> UserList()
        {
            var userList = from d in db.MstUsers
                       select new Entities.MstUser
                       {
                           Id = d.Id,
                           UserName = d.UserName,
                           FirstName = d.FirstName,
                           LastName = d.LastName,
                           Email = d.Email,
                           Password = d.Password,
                           UserTypeId = d.UserTypeId
                       };

            return userList.ToList();
        }

        // =============
        // Detail - User
        // =============
        [HttpGet, Route("api/user/detail/{id}")]
        public List<Entities.MstUser> IndividualUser(String id)
        {
            var user = from d in db.MstUsers
                       where d.Id == Convert.ToInt32(id)
                       select new Entities.MstUser
                       {
                           Id = d.Id,
                           UserName = d.UserName,
                           FirstName = d.FirstName,
                           LastName = d.LastName,
                           Email = d.Email,
                           Password = d.Password,
                           UserTypeId = d.UserTypeId
                       };

            return user.ToList();
        }

        // =============
        // Update - User
        // =============
        [HttpPut, Route("api/user/update/{id}")]
        public HttpResponseMessage UpdateUser(Entities.MstUser objUpdateUser, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = from d in db.MstUsers
                                        where d.Id == Convert.ToInt32(id)
                                        select d.Id;

                    var userId = currentUserId.FirstOrDefault();

                    var updateUser = currentUser.FirstOrDefault();
                    updateUser.Id = userId;
                    updateUser.UserName = objUpdateUser.UserName;
                    updateUser.FirstName = objUpdateUser.FirstName;
                    updateUser.LastName = objUpdateUser.LastName;
                    updateUser.Email = objUpdateUser.Email;
                    updateUser.Password = objUpdateUser.Password;
                    updateUser.UserTypeId = objUpdateUser.UserTypeId;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // =============
        // Delete - User
        // =============
        [HttpDelete, Route("api/user/delete/{id}")]
        public HttpResponseMessage DeleteUser(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (currentUser.Any())
                {
                    db.MstUsers.DeleteOnSubmit(currentUser.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
