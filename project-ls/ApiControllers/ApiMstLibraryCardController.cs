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
    [RoutePrefix("api/Library/LibraryCard")]
    public class ApiMstLibraryCardController : ApiController
    {
        private Data.librarydbDataContext db = new Data.librarydbDataContext();

        // ===================
        // Add- Library Card
        // ===================
        [HttpPost, Route("Add")]
        public HttpResponseMessage AddLibraryCard(Entities.MstLibraryCard objLibraryCard)
        {
            try
            {
                var libraryCard = from d in db.MstLibaryCards
                                  where d.Id == objLibraryCard.Id && d.LibraryCardNumber == objLibraryCard.LibraryCardNumber
                                  select d;

                if (libraryCard.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Library card already exist!");
                }
                else
                {

                    var currentUser = from d in db.MstUsers
                                      where d.AspNetUserId == User.Identity.GetUserId()
                                      select d;

                    Data.MstLibaryCard newLibraryCard = new Data.MstLibaryCard
                    {
                        LibraryCardNumber = objLibraryCard.LibraryCardNumber,
                        ManualLibraryCardNumber = objLibraryCard.ManualLibraryCardNumber,
                        BorrowerId = objLibraryCard.BorrowerId,
                        IsPrinted = objLibraryCard.IsPrinted,
                        LibraryInChargeUserId = currentUser.FirstOrDefault().Id,
                        FootNote = objLibraryCard.FootNote,
                        CreatedByUserId = currentUser.FirstOrDefault().Id,
                        CreatedDate = DateTime.Now,
                        UpdatedByUserId = currentUser.FirstOrDefault().Id,
                        UpdatedDate = DateTime.Now,
                    };

                    db.MstLibaryCards.InsertOnSubmit(newLibraryCard);
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

        // =====================
        // Update - Library Card
        // =====================
        [HttpPut, Route("Update/{id}")]
        public HttpResponseMessage UpdateLibraryCardDetail(Entities.MstLibraryCard objUpdateLibraryCard, String id)
        {
            try
            {
                var currentLibraryCardDetail = from d in db.MstLibaryCards
                                               where d.Id == Convert.ToInt32(id)
                                               select d;

                if (currentLibraryCardDetail.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetUserId == User.Identity.GetUserId()
                                      select d;

                    var updateLibraryCard = currentLibraryCardDetail.FirstOrDefault();
                    updateLibraryCard.IsPrinted = objUpdateLibraryCard.IsPrinted;
                    updateLibraryCard.FootNote = objUpdateLibraryCard.FootNote;
                    updateLibraryCard.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    updateLibraryCard.UpdatedDate = DateTime.Now;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Library card does not exist!");

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // ===================
        // List - Library Card
        // ===================
        [HttpGet, Route("List")]
        public List<Entities.MstLibraryCard> GetLibraryCardList()
        {
            var libararyCardList = from d in db.MstLibaryCards
                                   select new Entities.MstLibraryCard
                                   {
                                       Id = d.Id,
                                       LibraryCardNumber = d.LibraryCardNumber,
                                       ManualLibraryCardNumber = d.LibraryCardNumber,
                                       BorrowerId = d.BorrowerId,
                                       IsPrinted = d.IsPrinted,
                                       LibraryInChargeUserId = d.LibraryInChargeUserId,
                                       FootNote = d.FootNote,
                                       CreatedByUserId = d.CreatedByUserId,
                                       CreatedDate = d.CreatedDate.ToString(),
                                       UpdatedByUserId = d.UpdatedByUserId,
                                       UpdatedDate = d.UpdatedDate.ToString(),
                                   };
            return libararyCardList.ToList();
        }

        // =====================
        // Delete - Library Card
        // =====================
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteLibraryCard(String id)
        {
            try
            {
                var currentLibraryCard = from d in db.MstBorrowers
                                         where d.Id == Convert.ToInt32(id)
                                         select d;

                if (currentLibraryCard.Any())
                {
                    db.MstBorrowers.DeleteOnSubmit(currentLibraryCard.First());
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
