using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly TheWizardsGameShopContext _context;

        public ReviewsController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);
            var theWizardsGameShopContext = _context.Review.Include(r => r.Game).Where(c => c.UserId.Equals(HttpContext.Session.GetInt32("userId")));
            return View(await theWizardsGameShopContext.ToListAsync());
        }

        // GET: Reviews/Admin
        public async Task<IActionResult> Admin()
        {
            if (!UserHelper.IsEmployee(this)) return UserHelper.RequireEmployee(this);
            var theWizardsGameShopContext = _context.Review.Include(r => r.Game).Where(c => c.UserId.Equals(HttpContext.Session.GetInt32("userId")));
            return View(await theWizardsGameShopContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Game)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        public async Task<IActionResult> GetReviewsOnGame(int? gameId)
        {
            if (gameId == null)
            {
                return NotFound();
            }

            var reviews = await _context.Review.Include(r => r.User).Where(r => r.GameId.Equals(gameId)).ToListAsync();

            return View(reviews);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewId,ReviewContent,UserId,GameId")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;
                review.IsPublished = false;
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (UserHelper.IsLoggedIn(this))
            {
                ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            }

            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            if (!UserHelper.IsLoggedIn(this))
            {
                RequireLogin(this);
            }

            ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,ReviewContent,UserId,GameId,ReviewDate,IsPublished")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewId))
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

            if (UserHelper.IsLoggedIn(this))
            {
                ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Game)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Review.FindAsync(id);
            _context.Review.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.ReviewId == id);
        }
        private RedirectToActionResult RequireLogin(Controller controller)
        {
            string actionName = controller.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = controller.ControllerContext.RouteData.Values["controller"].ToString();

            TempData["LoginMessage"] = UserHelper.NOT_LOGGED_IN_MESSAGE;
            TempData["RequestedActionName"] = actionName;
            TempData["RequestedControllerName"] = controllerName;

            return RedirectToAction("Login", "Users");
        }
    }
}
