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
    public class ApiMstLibraryBooksController : ApiController
    {
        private Data.librarydbDataContext db = new Data.librarydbDataContext();

        // ===========
        // Post - Book
        // ===========
        [HttpPost, Route("api/library/book/add")]
        public HttpResponseMessage addLibraryBook(Entities.MtsLibraryBook objLibraryBook)
        {
            try
            {
                Data.MstLibraryBook newLibraryBook = new Data.MstLibraryBook
                {
                    BookNumber = objLibraryBook.BookNumber,
                    Title = objLibraryBook.Title,
                    Author = objLibraryBook.Author,
                    EditionNumber = objLibraryBook.EditionNumber,
                    PlaceOfPublication = objLibraryBook.PlaceOfPublication,
                    CopyRightDate = Convert.ToDateTime(objLibraryBook.CopyRightDate),
                    ISBN = objLibraryBook.ISBN,
                    UserId = objLibraryBook.UserId
                };

                db.MstLibraryBooks.InsertOnSubmit(newLibraryBook);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
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
        [HttpGet, Route("api/library/book/list")]
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
                            UserId = d.UserId
                        };

            return bookList.ToList();
        }

        // =============
        // Detail - Book
        // =============
        [HttpGet, Route("api/library/book/{id}")]
        public List<Entities.MtsLibraryBook> LibraryBookList(String id)
        {
            var book = from d in db.MstLibraryBooks
                        where d.Id == Convert.ToInt32(id)
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
                            UserId = d.UserId
                        };

            return book.ToList();
        }

        // =============
        // Update - Book
        // =============
        [HttpPut, Route("update/{id}/{bookId}")]
        public HttpResponseMessage UpdateBook(String id, String bookId, Entities.MtsLibraryBook objUpdateBook)
        {
            try
            {
                var currentUserId = from d in db.MstUsers
                                  where d.Id == Convert.ToInt32(id) && d.UserTypeId == 1
                                  select d.Id;

                var userIdCurrent = currentUserId.FirstOrDefault();

                if (currentUserId.Any())
                {
                    var currentBook = from d in db.MstLibraryBooks
                                        where d.Id == Convert.ToInt32(bookId) 
                                        select d;

                    if (currentBook.Any()) {

                        var currentBookId = from d in db.MstLibraryBooks
                                        where d.Id == Convert.ToInt32(bookId)
                                        select d.Id;

                        var bookIdCurrent = currentBookId.FirstOrDefault();

                        var updateBook = currentBook.FirstOrDefault();
                        updateBook.Id = bookIdCurrent;
                        updateBook.BookNumber = objUpdateBook.BookNumber;
                        updateBook.Title = objUpdateBook.Title;
                        updateBook.Author = objUpdateBook.Author;
                        updateBook.EditionNumber = objUpdateBook.EditionNumber;
                        updateBook.CopyRightDate = Convert.ToDateTime(objUpdateBook.CopyRightDate);
                        updateBook.ISBN = objUpdateBook.ISBN;
                        updateBook.UserId = userIdCurrent;

                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
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
        [HttpDelete, Route("api/library/book/delete/{id}")]
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
