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

        
        public HttpResponseMessage addLibraryBook(Entities.MtsLibraryBooks objLibraryBook)
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
         
    }
}
