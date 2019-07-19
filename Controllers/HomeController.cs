using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Exam.Models;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext dbContext;
        public HomeController(HomeContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("signin")]
        public IActionResult Signin()
        {
            return View("Login");
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Signin");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(Signin logUser)
        { 
            
            if(ModelState.IsValid)
            {
                User userInDb = dbContext.Users.FirstOrDefault( u => u.Email == logUser.SigninEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("SigninEmail", "Your account cannot be found");
                    return View("Login");

                }
                var haser = new PasswordHasher<Signin>();
                var result = haser.VerifyHashedPassword(logUser, userInDb.Password, logUser.SigninPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("SigninPassword", "Wrong Password");
                    return View("Login");

                }
                
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                //** Change template to match your new controller **
                return RedirectToAction("Dashboard", "Event");
            }
            else
            {
                return View ("Login");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
