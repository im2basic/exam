using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Exam.Models;

namespace Exam.Controllers
{
    public class ResponseController : Controller
    {
        private HomeContext dbContext;
        public ResponseController(HomeContext context)
        {
            dbContext = context;
        }

        [HttpGet("/rsvp/{EventId}/{Status}")]
        public IActionResult RSVP(int EventId, String Status)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction ("Logout", "Home");
            }
            else
            {
                if(Status == "add")
                {
                    Reservation NewRsvp = new Reservation();
                    NewRsvp.UserId = (int)HttpContext.Session.GetInt32("UserId");
                    NewRsvp.EventId = EventId;
                    dbContext.Rsvps.Add(NewRsvp);
                    dbContext.SaveChanges();
                    return RedirectToAction("Dashboard", "Event");
                }
                if(Status == "remove")
                {
                    Reservation remove = dbContext.Rsvps.FirstOrDefault(r => r.EventId == EventId && r.UserId == HttpContext.Session.GetInt32("UserId"));

                    dbContext.Rsvps.Remove(remove);
                    dbContext.SaveChanges();
                    return RedirectToAction("Dashboard", "Event");

                }
                return View("Dashboard");
            }
        }

    }
}