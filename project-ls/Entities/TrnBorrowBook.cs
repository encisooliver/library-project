using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_ls.Entities
{
    public class TrnBorrowBook
    {
        public Int32 Id { get; set; }
        public Int32 BorrowId { get; set; }
        public Int32 BookId { get; set; }
        public Int32 Quantity { get; set; }
    }
}