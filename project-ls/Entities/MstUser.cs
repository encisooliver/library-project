using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_ls.Entities
{
    public class MstUser
    {
        public Int32 Id { get; set; }
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public Int32 UserTypeId { get; set; }

    }
}