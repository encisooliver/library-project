using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using Microsoft.AspNet.Identity;

namespace project_ls.ApiControllers
{
    [RoutePrefix("api/Library/Borrower")]
    public class ApiMstBorrowerController : ApiController
    {
        private Data.librarydbDataContext db = new Data.librarydbDataContext();

        // ===========
        // Post - Book
        // ===========
        [HttpPost, Route("Add")]
        public HttpResponseMessage AddBorrwer(Entities.MstBorrower objLibraryBorrower)
        {
            try
            {
                var borrower = from d in db.MstBorrowers
                               where d.Id == objLibraryBorrower.Id
                               select d;



                if (borrower.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Book No. already taken!");
                }
                else
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetUserId == User.Identity.GetUserId()
                                      select d;

                    Data.MstBorrower newBorrower = new Data.MstBorrower
                    {
                        BorrowerNumber = objLibraryBorrower.BorrowerNumber,
                        ManualBorrowerNumber = objLibraryBorrower.ManualBorrowerNumber,
                        FullName = objLibraryBorrower.FullName,
                        Department = objLibraryBorrower.Department,
                        Course = objLibraryBorrower.Course,
                        // CreatedByUserId = objLibraryBorrower.UpdatedByUserId,
                        CreatedByUserId = currentUser.FirstOrDefault().Id,
                        CreatedDate = DateTime.Now,
                        //UpdatedByUserId = objLibraryBorrower.UpdatedByUserId,
                        UpdatedByUserId = currentUser.FirstOrDefault().Id,
                        UpdatedDate = DateTime.Now,
                        BorrowerTypeId = objLibraryBorrower.BorrowerTypeId,
                        LibraryCardId = objLibraryBorrower.LibraryCardId

                    };

                    db.MstBorrowers.InsertOnSubmit(newBorrower);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        [HttpPut, Route("Update/{id}/{librarycardid}")]
        public HttpResponseMessage UpdateBorrowerDetail(Entities.MstBorrower objUpdateBorrower, String id, String librarycardid)
        {
            try
            {
                var currentBorrowerDetail = from d in db.MstBorrowers
                                            where d.Id == Convert.ToInt32(id) && d.LibraryCardId == Convert.ToInt32(librarycardid)
                                            select d;

                if (currentBorrowerDetail.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetUserId == User.Identity.GetUserId()
                                      select d;

                    var updateBorrower = currentBorrowerDetail.FirstOrDefault();
                    updateBorrower.BorrowerNumber = objUpdateBorrower.BorrowerNumber;
                    updateBorrower.ManualBorrowerNumber = objUpdateBorrower.ManualBorrowerNumber;
                    updateBorrower.FullName = objUpdateBorrower.FullName;
                    updateBorrower.Department = objUpdateBorrower.Department;
                    updateBorrower.Course = objUpdateBorrower.Course;
                    // updateBorrower.UpdatedByUserId = objUpdateBorrower.UpdatedByUserId;
                    updateBorrower.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    updateBorrower.UpdatedDate = DateTime.Now;
                    updateBorrower.BorrowerTypeId = objUpdateBorrower.BorrowerTypeId;
                    updateBorrower.LibraryCardId = objUpdateBorrower.LibraryCardId;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Borrower does not exist!");

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // ===============
        // List - Borrower
        // ===============
        [HttpGet, Route("List")]
        public List<Entities.MstBorrower> GetMstBorrower()
        {
            var borrowerList = from d in db.MstBorrowers
                               select new Entities.MstBorrower
                               {
                                   Id = d.Id,
                                   BorrowerNumber = d.BorrowerNumber,
                                   ManualBorrowerNumber = d.BorrowerNumber,
                                   FullName = d.FullName,
                                   Department = d.Department,
                                   Course = d.Course,
                                   CreatedByUserId = d.CreatedByUserId,
                                   CreatedDate = d.CreatedDate.ToShortDateString(),
                                   UpdatedByUserId = d.UpdatedByUserId,
                                   UpdatedDate = d.UpdatedDate.ToShortDateString(),
                                   BorrowerTypeId = d.BorrowerTypeId,
                                   LibraryCardId = d.LibraryCardId,
                               };
            return borrowerList.ToList();
        }

        // ========================
        // Delete - Borrower Detail
        // ========================
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteLibraryBook(String id)
        {
            try
            {
                var currentBorrowerDetail = from d in db.MstBorrowers
                                            where d.Id == Convert.ToInt32(id)
                                            select d;

                if (currentBorrowerDetail.Any())
                {
                    db.MstBorrowers.DeleteOnSubmit(currentBorrowerDetail.First());
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
