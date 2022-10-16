using DatabaseWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace DatabaseWebAPI.Controllers
{
    public class BookingController : ApiController
    {
        private CentreEntities db = new CentreEntities();

        // GET: Booking
        [ResponseType(typeof(void))]
        public IHttpActionResult PostSession(BookedSession session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Centre centre = db.Centres.First(c => c.Id == session.CentreId);
            centre.BookedSessions.Add(session);
            db.SaveChanges();

            return Ok();
        }
    }
}