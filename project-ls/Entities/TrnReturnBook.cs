using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_ls.Entities
{
    public class TrnReturnBook
    {
        public Int32 Id { get; set; }
        public Int32 ReturnId { get; set; }
        public Int32 BookId { get; set; }
        public Int32 Quantity { get; set; }
    }
}