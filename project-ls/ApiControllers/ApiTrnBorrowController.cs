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
    [RoutePrefix("api/Library/Borrow")]
    public class ApiTrnBorrowController : ApiController
    {
        private Data.librarydbDataContext db = new Data.librarydbDataContext();

        // ===============
        // Add- Trn Borrow
        // ===============
        [HttpPost, Route("Add")]
        public HttpResponseMessage AddTrnBorrow(Entities.TrnBorrow objTrnBorrow)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetUserId == User.Identity.GetUserId()
                                  select d;

                if (!currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    Data.TrnBorrow newTrnBorrow = new Data.TrnBorrow
                    {
                        BorrowNumber = objTrnBorrow.BorrowNumber,
                        BorrowDate = Convert.ToDateTime(objTrnBorrow.BorrowDate),
                        ManualBorrowNumber = objTrnBorrow.ManualBorrowNumber,
                        BorrowerId = objTrnBorrow.BorrowerId,
                        LibraryCardId = objTrnBorrow.LibraryCardId,
                        PreparedByUser = objTrnBorrow.PreparedByUser,
                        CreatedByUserId = currentUserId,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = currentUserId,
                        UpdatedDate = DateTime.Now
                    };

                    db.TrnBorrows.InsertOnSubmit(newTrnBorrow);
                    db.SubmitChanges();

                    Int32 BorrowId = newTrnBorrow.Id;

                    if (objTrnBorrow.ListOfBooks.Any())
                    {
                        List<Data.TrnBorrowBook> newBookBorrows = new List<Data.TrnBorrowBook>();

                        foreach (var Book in objTrnBorrow.ListOfBooks)
                        {
                            newBookBorrows.Add(new Data.TrnBorrowBook
                            {
                                BorrowId = BorrowId,
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
