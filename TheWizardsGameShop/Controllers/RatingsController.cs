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
    public class RatingsController : Controller
    {
        private readonly TheWizardsGameShopContext _context;

        public RatingsController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);
            var theWizardsGameShopContext = _context.Rating.Include(r => r.Game).Where(c => c.UserId.Equals(HttpContext.Session.GetInt32("userId")));
            return View(await theWizardsGameShopContext.ToListAsync());
        }

        public async Task<IActionResult> Index(int gameId)
        {
            var ratings = _context.Rating.Include(r => r.User).Where(r => r.GameId.Equals(gameId));
            return View(await ratings.ToListAsync());
        }

        // GET: Ratings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                .Include(r => r.Game)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // GET: Ratings/Create
        //public IActionResult Create(int gameId, int rate)
        //{
        //    if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

        //    ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
        //    return View();
        //}

        // POST: Ratings/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int gameId, int rate) //[Bind("RatingId,Rate,UserId,GameId")] Rating rating
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);
            var userId = UserHelper.GetSessionUserId(this);
            Rating rating = new Rating();
            rating.Rate = (double)rate;
            rating.UserId = (int)userId;
            rating.GameId = gameId;

            _context.Add(rating);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Games", new { id = gameId });
            //return View(rating);
        }

        // GET: Ratings/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var rating = await _context.Rating.FindAsync(id);
        //    if (rating == null)
        //    {
        //        return NotFound();
        //    }

        //    if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

        //    ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
        //    return View(rating);
        //}

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int gameId, int rate) //int id, [Bind("RatingId,Rate,UserId,GameId")] Rating rating
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);
            var userId = UserHelper.GetSessionUserId(this);
            var rating = await _context.Rating.FirstOrDefaultAsync(r => r.GameId.Equals(gameId) && r.UserId.Equals(userId));

            if (rating == null)
            {
                return NotFound();
            }

            try
            {
                rating.Rate = rate;

                _context.Update(rating);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(rating.GameId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", "Games", new { id = gameId });
        }

        // GET: Ratings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                .Include(r => r.Game)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rating = await _context.Rating.FindAsync(id);
            _context.Rating.Remove(rating);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RatingExists(int id)
        {
            return _context.Rating.Any(e => e.GameId == id);
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
