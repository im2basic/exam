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
    [Route("Event")]
    public class EventController : Controller
    {
        private HomeContext dbContext;
        //Change this controller to new name
        public EventController(HomeContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult DashBoard()
        {
            if(HttpContext.Session.GetInt32("UserId") == null )
            {
                return RedirectToAction ("Loguut","Home");
            }
            else 
            {
                //This code Compares Dates and does not display past dates
                List<Event> AllEvents = dbContext.Events.Include(w => w.GuestList)
                .ThenInclude(r => r.Guest).Where(c => c.EventTime > DateTime.Now).ToList();
                
                ViewBag.User = dbContext.Users.Include(u => u.CreatedActivities).Include(u => u.Activities).ThenInclude( r => r.Attending).FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));

                return View ("Dashboard",AllEvents);
                
            }
        }

        [HttpGet("new")]
        public IActionResult NewEvent()
        {
        ViewBag.User = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
        return View();
        }

        [HttpPost("create")]
        public IActionResult CreateEvent(Event NewEvent)
        {
            Console.WriteLine("Print 1");
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction ("Logout", "Home");
            }
            else
            {
                if(ModelState.IsValid)
                {
                    Console.WriteLine("Print 2");
                    dbContext.Events.Add(NewEvent);
                    dbContext.SaveChanges();
                    return RedirectToAction($"/Event/show/{NewEvent.EventId}");
                }
                else
                {
                    Console.WriteLine("Print 3");
                    ViewBag.User = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
                    return View("NewEvent");
                }
            }
        }

        [HttpGet("/Event/show/{EventId}")]
        public IActionResult ShowEvent(int EventId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction ("Logout", "Home");
            }
            else
            {                                                          
                Event myEvent = dbContext.Events.Include(w => w.GuestList).ThenInclude( r => r.Guest).FirstOrDefault(w => w.EventId == EventId);

                ViewBag.User = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
                return View("ShowEvent", myEvent);
            }
        }

        [HttpGet("/destroy/{EventId}")]
        public IActionResult Destroy(int EventId)
        {
            Console.WriteLine("Its working");
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction ("Logout", "Home");
            }
            else
            {
                Console.WriteLine("DESTROY");
                Event RemoveEvent = dbContext.Events.FirstOrDefault(w => w.EventId == EventId);
                if(RemoveEvent == null)
                {
                    return RedirectToAction("Dashboard");

                }
                dbContext.Events.Remove(RemoveEvent);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
        }

        [HttpGet("/test")]
        public string test(){
            return "It worked";
        }


    }
}