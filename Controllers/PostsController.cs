#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aspire.Data;
using Aspire.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Aspire.Areas.Identity.Data;

namespace Aspire.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly AppDBContext _context;
        private UserManager<AuthUser> _userManager;
        private readonly AuthContext _auth;
        private EmailSender _emailSender = new EmailSender(new EmailConfiguration());


        public PostsController(AppDBContext context, UserManager<AuthUser> userManager, AuthContext auth)
        {
            _context = context;
            _auth = auth;
            _userManager = userManager;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {          
            var appDBContext = _context.Posts.Include(p => p.PostStatus).Include(p=> p.PostType).Where(p => p.PostStatusId != 4 && p.PostStatusId != 5).OrderByDescending(p => p.PostId);
            
            return View(await appDBContext.ToListAsync());
        }

        // GET: Posts/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.PostStatus)
                .Include(p => p.PostType)
                .FirstOrDefaultAsync(m => m.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            ViewBag.User = _auth.Users.Find(post.UserId).FirstName + " " + _auth.Users.Find(post.UserId).LastName;

            var currentUserId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            List<UserPostSupport> userPostSupport = _context.UserPostSuport.Where(u => u.PostId == post.PostId && u.UserId == currentUserId).ToList();
            if(userPostSupport.Count == 0)
            {
                ViewBag.AllowSupport = true;
            }
            else
            {
                ViewBag.AllowSupport = false;
            }
            //string userId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            //List<UserPost> userPosts = _context.UserPosts.FirstOrDefaultAsync(u => u.PostId == post.PostId);

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Details(int? id, string txtMessage)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.PostStatus)
                .Include(p => p.PostType)
                .FirstOrDefaultAsync(m => m.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            var userName = _userManager.FindByNameAsync(User.Identity.Name).Result.FirstName + " " + _userManager.FindByNameAsync(User.Identity.Name).Result.LastName;
            var userEmail = _userManager.FindByNameAsync(User.Identity.Name).Result.Email;

            ViewBag.User = _auth.Users.Find(post.UserId).FirstName + " " + _auth.Users.Find(post.UserId).LastName;
            var AutherEmail = _auth.Users.Find(post.UserId).Email;

            string editedMessage = "Hi! This is Aspire Admin. I am here to let you know that your post titled <b>" + post.Title + "</b> posted on <b>" + post.PostedOn + "</b> have attrected <b>" + userName + "'s</b> attention so he has sent you a message using an email address that you can use to reply him/her <b>" + userEmail + "</b> the message is <br> <p>" + txtMessage + "</p>";

            Message emailMessage = new Message(AutherEmail, "Reply from student", editedMessage);
            _emailSender.SendEmail(emailMessage);

            var currentUserId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            List<UserPostSupport> userPostSupport = _context.UserPostSuport.Where(u => u.PostId == post.PostId && u.UserId == currentUserId).ToList();

            if (userPostSupport.Count == 0)
            {
                ViewBag.AllowSupport = true;
            }
            else
            {
                ViewBag.AllowSupport = false;
            }
            //string userId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            //List<UserPost> userPosts = _context.UserPosts.FirstOrDefaultAsync(u => u.PostId == post.PostId);
            TempData["Reply"] = "Email Sent!";
            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["PostStatusId"] = new SelectList(_context.PostStatuses.Take(4), "PostStatusId", "Name");
            ViewData["PostTypeId"] = new SelectList(_context.PostTypes, "PostTypeId", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Discription,Support,PostedOn,PostStatusId,PostTypeId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.UserId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
                _context.Add(post);
                await _context.SaveChangesAsync();

                //UserPost userPost = new UserPost();
                //userPost.PostId = post.PostId;
                //userPost.UserId = ;

                //_context.Add(userPost);
                //await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["PostStatusId"] = new SelectList(_context.PostStatuses.Take(4), "PostStatusId", "Name", post.PostStatusId);
            ViewData["PostTypeId"] = new SelectList(_context.PostTypes, "PostTypeId", "Name", post.PostTypeId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            ViewData["PostStatusId"] = new SelectList(_context.PostStatuses.Take(4), "PostStatusId", "Name", post.PostStatusId);
            ViewData["PostTypeId"] = new SelectList(_context.PostTypes, "PostTypeId", "Name", post.PostTypeId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Discription,Support,PostedOn,PostStatusId,PostTypeId")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // avoid certain fields to get updated like postedOn and Support.
                    var original = _context.Posts.Find(post.PostId);
                    original.Title = post.Title;
                    original.Discription = post.Discription;
                    original.PostStatusId = post.PostStatusId;
                    original.PostTypeId = post.PostTypeId;

                    _context.Update(original);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostStatusId"] = new SelectList(_context.PostStatuses.Take(4), "PostStatusId", "Name", post.PostStatusId);
            ViewData["PostTypeId"] = new SelectList(_context.PostTypes, "PostTypeId", "Name", post.PostTypeId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.PostStatus)
                .Include(p => p.PostType)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }

        // Add Suport to the post.
        public IActionResult Support(int id)
        {
            var post = _context.Posts.Find(id);
            post.Support += 1;

            UserPostSupport postSupport = new UserPostSupport();
            postSupport.UserId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            postSupport.PostId = id;

            _context.Update(post);
            _context.Add(postSupport);

            _context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id = id });
        }

        public IActionResult Report(int id)
        {
            var post = _context.Posts.Find(id);
            post.PostStatusId = 5;
            _context.Update(post);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
