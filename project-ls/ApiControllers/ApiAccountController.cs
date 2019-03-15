using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;

namespace project_ls.ApiControllers
{
    public class ApiAccountController : ApiController
    {
        private Data.librarydbDataContext db = new Data.librarydbDataContext();

        //[HttpPost, Route("api/user/login/{userName}/{password}")]
       
        //public List<Entities.MstUser> UserLogin(String userName, String password)
        //{
        //    var user = from d in db.MstUsers
        //               where d.UserName == userName && d.Password == password
        //               select new Entities.MstUser
        //               {
        //                   Id = d.Id,
        //                   FirstName = d.FirstName,
        //                   LastName = d.LastName,
        //                   Password = d.Password,
        //                   UserTypeId = d.UserTypeId
        //               };

        //    return user.ToList();
        //}
    }
}
