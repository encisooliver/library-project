using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace project_ls.ApiControllers
{
    public class ApiTrnReturnController : ApiController
    {
        private Data.librarydbDataContext db = new Data.librarydbDataContext();

        // ===============
        // Add- Trn Borrow
        // ===============
        [HttpPost, Route("Add")]
        public HttpResponseMessage AddTrnBorrow(Entities.TrnReturn objTrnReturn)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                if (!currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    Data.TrnReturn newTrnReturn= new Data.TrnReturn
                    {
                        ReturnNumber = objTrnReturn.ReturnNumber,
                        ReturnDate = Convert.ToDateTime(objTrnReturn.ReturnDate),
                        ManualReturnNumber = objTrnReturn.ManualReturnNumber,
                        BorrowerId = objTrnReturn.BorrowerId,
                        PreparedByUser = objTrnReturn.PreparedByUser,
                        CreatedByUserId = currentUserId,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = currentUserId,
                        UpdatedDate = DateTime.Now
                    };

                    db.TrnReturns.InsertOnSubmit(newTrnReturn);
                    db.SubmitChanges();

                    Int32 ReturnId = newTrnReturn.Id;

                    if (objTrnReturn.ListOfReturnBooks.Any())
                    {
                        List<Data.TrnReturnBook> newReturnBooks= new List<Data.TrnReturnBook>();

                        foreach (var Book in objTrnReturn.ListOfReturnBooks)
                        {
                            newReturnBooks.Add(new Data.TrnReturnBook
                            {
                                ReturnId = ReturnId,
                                BookId = Book.BookId,
                                Quantity = Book.Quantity
                            });
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid transaction!");

                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

    }
}
