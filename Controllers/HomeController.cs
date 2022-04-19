using Aspire.Models;
using Aspire.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Aspire.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Aspire.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;
        private UserManager<AuthUser> _userManager;
        private readonly AuthContext _auth;
        private EmailSender _emailSender = new EmailSender(new EmailConfiguration());

        public HomeController(AppDBContext context, UserManager<AuthUser> userManager, AuthContext auth)
        {
            _context = context;
            _auth = auth;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var appDBContext = _context.Posts.Include(p => p.PostStatus).Include(p => p.PostType).Where(p => p.PostStatusId == 1).OrderByDescending(p => p.Support);
            return View(await appDBContext.ToListAsync());
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult SupportPage()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserInfo(string id)
        {
            if (id == _userManager.FindByNameAsync(User.Identity.Name).Result.Id)
            {
                return RedirectToAction("Profile");
            }

            if (id == null)
            {
                return NotFound();
            }

            UserProfile userProfile = new UserProfile();
            userProfile.User = _auth.Users.Find(id);
            userProfile.Posts = _context.Posts.Where(p => p.UserId == id && p.PostStatusId != 5).ToList();


            if (User == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        //[HttpPost]
        //public IActionResult UserInfo(string id, string txtMessage)
        //{
        //    if (id == _userManager.FindByNameAsync(User.Identity.Name).Result.Id)
        //    {
        //        return RedirectToAction("Profile");
        //    }

        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    UserProfile userProfile = new UserProfile();
        //    userProfile.User = _auth.Users.Find(id);
        //    userProfile.Posts = _context.Posts.Where(p => p.UserId == id).ToList();


        //    var userEmail = _userManager.FindByNameAsync(User.Identity.Name).Result.Email;
        //    var userName = _userManager.FindByNameAsync(User.Identity.Name).Result.FirstName + " " + _userManager.FindByNameAsync(User.Identity.Name).Result.LastName;
        //    Message emailMessage = new Message(userProfile.User.Email, "Reply from student", txtMessage);
        //    _emailSender.SendEmail(emailMessage, "", "", "");

        //    if (User == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(userProfile);
        //}

        [Authorize]
        public IActionResult Profile()
        {
            //var FirstName = _userManager.FindByNameAsync(User.Identity.Name).Result.FirstName;
            //var LastName = _userManager.FindByNameAsync(User.Identity.Name).Result.LastName;
            //var email = _userManager.FindByNameAsync(User.Identity.Name).Result.Email;
            //var phone = _userManager.FindByNameAsync(User.Identity.Name).Result.PhoneNumber;
            //var dob = _userManager.FindByNameAsync(User.Identity.Name).Result.DateOfBirth;
            //var userName = _userManager.FindByNameAsync(User.Identity.Name).Result.UserName;

            //ViewBag.AuthUser = new AuthUser()
            //{
            //    FirstName = FirstName,
            //    LastName = LastName,
            //    Email = email,
            //    PhoneNumber = phone,
            //    DateOfBirth = dob,
            //    UserName = userName
            //};


            var currentUserId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;

            UserProfile userProfile = new UserProfile();
            userProfile.User = _auth.Users.Find(currentUserId);
            userProfile.Posts = _context.Posts.Include(p => p.PostStatus).Include(p => p.PostType).Where(p => p.UserId == currentUserId).ToList();

            return View(userProfile);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Feedback(string rating, string comment, string username, string userEmail)
        {
            string message = "Aspire Admin! we have got a new feed back by a random user. his name is <b>" + username + "</b> his email Id is <b>" + userEmail + " Feedback is as below</b><br> <p><b>Ratings: </b>" + rating + " <br> <b>Comment:</b>" + comment + "</p>";

            EmailConfiguration configuration = new EmailConfiguration();
            Message emailMessage = new Message(configuration.Username, "FEEDBACK", message);
            _emailSender.SendEmail(emailMessage);
            TempData["Reply"] = "Feedback recived! Thank You!";
            return View();
        }
    }
}