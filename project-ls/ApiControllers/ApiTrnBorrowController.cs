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

        // ================
        // List- Trn Borrow
        // ================
        [HttpGet, Route("List")]
        public List<Entities.TrnBorrow> GetListTrnBorrow()
        {

            var listTrnBorrow = from d in db.TrnBorrows
                                select new Entities.TrnBorrow
                                {
                                    Id = d.Id,
                                    BorrowNumber = d.BorrowerId,
                                    BorrowDate = d.BorrowDate.ToShortDateString(),
                                    ManualBorrowNumber = d.ManualBorrowNumber,
                                    BorrowerId = d.BorrowerId,
                                    LibraryCardId = d.LibraryCardId,
                                    PreparedByUser = d.PreparedByUser,
                                    CreatedByUserId = d.CreatedByUserId,
                                    CreatedDate = d.CreatedDate.ToShortDateString(),
                                    UpdatedByUserId = d.UpdatedByUserId,
                                    UpdatedDate = d.UpdatedDate.ToShortDateString()
                                };

            return listTrnBorrow.ToList();
        }

        // ==================
        // Update- Trn Borrow
        // ==================
        [HttpPut, Route("Update/{id}")]
        public HttpResponseMessage UpdateTrnBorrow(Entities.TrnBorrow objUpdateTrnBorrow, String id)
        {
            try
            {
                var currentTrnBorrow = from d in db.TrnBorrows
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (currentTrnBorrow.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetUserId == User.Identity.GetUserId()
                                      select d;

                    var updateTrnBorrow = currentTrnBorrow.FirstOrDefault();
                    updateTrnBorrow.BorrowNumber = objUpdateTrnBorrow.BookNumber;
                    updateTrnBorrow.BorrowDate = Convert.ToDateTime(objUpdateTrnBorrow.BorrowDate);
                    updateTrnBorrow.ManualBorrowNumber = objUpdateTrnBorrow.ManualBorrowNumber;
                    updateTrnBorrow.BorrowerId = objUpdateTrnBorrow.BorrowerId;
                    updateTrnBorrow.LibraryCardId = objUpdateTrnBorrow.LibraryCardId;
                    updateTrnBorrow.PreparedByUser = objUpdateTrnBorrow.PreparedByUser;
                    updateTrnBorrow.CreatedByUserId = objUpdateTrnBorrow.CreatedByUserId;
                    updateTrnBorrow.CreatedDate = Convert.ToDateTime(objUpdateTrnBorrow.CreatedDate);
                    updateTrnBorrow.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    updateTrnBorrow.UpdatedDate = DateTime.Now;

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
