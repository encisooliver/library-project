using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_ls.Entities
{
    public class TrnReturn
    {
        public Int32 Id { get; set; }
        public Int32 ReturnNumber { get; set; }
        public String ReturnDate { get; set; }
        public Int32 ManualReturnNumber { get; set; }
        public Int32 BorrowerId { get; set; }
        public String PreparedByUser { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedDate { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedDate { get; set; }
        public List<TrnReturnBook> ListOfReturnBooks { get; set; }
    }
}