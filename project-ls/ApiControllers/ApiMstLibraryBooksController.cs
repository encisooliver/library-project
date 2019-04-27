using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Reflection;

namespace project_ls.ApiControllers
{
    [Authorize, RoutePrefix("api/Library/Book")]
    public class ApiMstLibraryBooksController : ApiController
    {
        private Data.librarydbDataContext db = new Data.librarydbDataContext();

        // ===========
        // Post - Book
        // ===========
        [HttpPost, Route("Add")]
        public HttpResponseMessage addLibraryBook(Entities.MtsLibraryBook objLibraryBook)
        {
            try
            {
                var currentUserType = from d in db.MstUsers
                                      where d.AspNetUserId == User.Identity.GetUserId()
                                      select d.MstUserType.UserType;

                var userType = currentUserType.FirstOrDefault();

                if (userType == "Admin")
                {
                    var bookNumber = from d in db.MstLibraryBooks
                                     where d.BookNumber == objLibraryBook.BookNumber
                                     select d.BookNumber;

                    if (bookNumber.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Book No. already taken!");
                    }
                    else
                    {
                        var currentUser = from d in db.MstUsers
                                          where d.AspNetUserId == User.Identity.GetUserId()
                                          select d;

                        Data.MstLibraryBook newLibraryBook = new Data.MstLibraryBook
                        {
                            BookNumber = objLibraryBook.BookNumber,
                            Title = objLibraryBook.Title,
                            Author = objLibraryBook.Author,
                            EditionNumber = objLibraryBook.EditionNumber,
                            PlaceOfPublication = objLibraryBook.PlaceOfPublication,
                            CopyRightDate = Convert.ToDateTime(objLibraryBook.CopyRightDate),
                            ISBN = objLibraryBook.ISBN,
                            CreatedByUserId = currentUser.FirstOrDefault().Id,
                            CreatedBy = currentUser.FirstOrDefault().FirstName,
                            CreatedDate = DateTime.Now,
                            UpdatedByUserId = currentUser.FirstOrDefault().Id,
                            UpdatedBy = currentUser.FirstOrDefault().FirstName,
                            UpdatedDate = DateTime.Now,
                        };
                        db.MstLibraryBooks.InsertOnSubmit(newLibraryBook);
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You're not authorize!");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // ===========
        // List - Book
        // ===========
        [HttpGet, Route("List")]
        public List<Entities.MtsLibraryBook> LibraryBookList()
        {

            var bookList = from d in db.MstLibraryBooks
                           select new Entities.MtsLibraryBook
                           {
                               Id = d.Id,
                               BookNumber = d.BookNumber,
                               Title = d.Title,
                               Author = d.Author,
                               EditionNumber = d.EditionNumber,
                               PlaceOfPublication = d.PlaceOfPublication,
                               CopyRightDate = d.CopyRightDate.ToShortDateString(),
                               ISBN = d.ISBN,
                               CreatedByUserId = d.CreatedByUserId,
                               CreatedBy = d.CreatedBy,
                               CreatedDate = d.CreatedDate.ToLongDateString(),
                               UpdatedByUserId = d.UpdatedByUserId,
                               UpdatedBy = d.UpdatedBy,
                               UpdatedDate = d.UpdatedDate.ToLongDateString()
                           };

            return bookList.ToList();
        }

        // =============
        // Update - Book
        // =============
        [HttpPut, Route("Update/{id}")]
        public HttpResponseMessage UpdateBook(Entities.MtsLibraryBook objUpdateBook, String id)
        {
            try
            {
                var currentBook = from d in db.MstLibraryBooks
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (currentBook.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetUserId == User.Identity.GetUserId()
                                      select d;

                    var updateBook = currentBook.FirstOrDefault();
                    updateBook.BookNumber = objUpdateBook.BookNumber;
                    updateBook.Title = objUpdateBook.Title;
                    updateBook.Author = objUpdateBook.Author;
                    updateBook.EditionNumber = objUpdateBook.EditionNumber;
                    updateBook.PlaceOfPublication = objUpdateBook.PlaceOfPublication;
                    updateBook.CopyRightDate = Convert.ToDateTime(objUpdateBook.CopyRightDate);
                    updateBook.ISBN = objUpdateBook.ISBN;
                    updateBook.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    updateBook.UpdatedBy = currentUser.FirstOrDefault().FirstName;
                    updateBook.UpdatedDate = DateTime.Now;

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
        // Delete - Book
        // =============
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteLibraryBook(String id)
        {
            try
            {
                var currentBook = from d in db.MstLibraryBooks
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (currentBook.Any())
                {
                    db.MstLibraryBooks.DeleteOnSubmit(currentBook.First());
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
