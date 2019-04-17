using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_ls.Entities
{
    public class MstBorrower
    {
        public Int32 Id { get; set; }
        public Int32 BorrowerNumber { get; set; }
        public Int32 ManualBorrowerNumber { get; set; }
        public String FullName { get; set; }
        public String Department { get; set; }
        public String Course { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedDate { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedDate { get; set; }
        public Int32 BorrowerTypeId { get; set; }
        public Int32 LibraryCardId { get; set; }
    }
}