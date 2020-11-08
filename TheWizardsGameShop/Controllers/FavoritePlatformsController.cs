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
    public class FavoritePlatformsController : Controller
    {
        private readonly TheWizardsGameShopContext _context;

        public FavoritePlatformsController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: FavoritePlatforms
        public async Task<IActionResult> Index()
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            var sessionUserId = UserHelper.GetSessionUserId(this);
            var context = _context.FavoritePlatform
                .Include(f => f.Platform)
                .Where(f => f.UserId.Equals(sessionUserId));
            return View(await context.ToListAsync());
        }

        // GET: FavoritePlatforms/List
        public async Task<IActionResult> List()
        {
            var sessionUserId = UserHelper.GetSessionUserId(this);
            var context = _context.FavoritePlatform
                .Include(f => f.Platform)
                .Where(f => f.UserId.Equals(sessionUserId));
            var platformContext = _context.Platform;
            return View(await context.ToListAsync());
        }

        // GET: FavoritePlatforms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoritePlatform = await _context.FavoritePlatform
                .Include(f => f.Platform)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (favoritePlatform == null)
            {
                return NotFound();
            }

            return View(favoritePlatform);
        }

        // GET: FavoritePlatforms/Create
        public IActionResult Create()
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            var userId = UserHelper.GetSessionUserId(this);
            var favoritePlatformContext = _context.FavoritePlatform
                .Include(f => f.Platform)
                .Where(f => f.UserId.Equals(userId));
            ViewData["UserId"] = userId;
            ViewData["FavoritePlatforms"] = favoritePlatformContext.ToList();

            
            ViewData["Platforms"] = getPlatformsNotInFav(userId);

            return View();
        }

        // POST: FavoritePlatforms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,PlatformId")] FavoritePlatform favoritePlatform)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favoritePlatform);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                //return View(favoritePlatform);
            }
            //ViewData["PlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName", favoritePlatform.PlatformId);

            var userId = UserHelper.GetSessionUserId(this);
            var favoritePlatformContext = _context.FavoritePlatform
                .Include(f => f.Platform)
                .Where(f => f.UserId.Equals(userId));
            ViewData["UserId"] = userId;
            ViewData["FavoritePlatforms"] = favoritePlatformContext.ToList();
            ViewData["Platforms"] = getPlatformsNotInFav(userId);

            return View(favoritePlatform);
        }

        // GET: FavoritePlatforms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoritePlatform = await _context.FavoritePlatform.FindAsync(id);
            if (favoritePlatform == null)
            {
                return NotFound();
            }

            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            ViewData["PlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName", favoritePlatform.PlatformId);
            
            return View(favoritePlatform);
        }

        // POST: FavoritePlatforms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,PlatformId")] FavoritePlatform favoritePlatform)
        {
            if (id != favoritePlatform.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favoritePlatform);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoritePlatformExists(favoritePlatform.UserId))
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
            ViewData["PlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName", favoritePlatform.PlatformId);
            
            return View(favoritePlatform);
        }

        // GET: FavoritePlatforms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = HttpContext.Session.GetInt32("userId");

            if (id == null || userId == null)
            {
                return NotFound();
            }

            var favoritePlatform = await _context.FavoritePlatform
                .Include(f => f.Platform)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserId == userId && m.PlatformId == id);
            if (favoritePlatform == null)
            {
                return NotFound();
            }


            await DeleteConfirmed(Convert.ToInt32(userId), Convert.ToInt32(favoritePlatform.PlatformId));
            return RedirectToAction(nameof(Create));
            //return View(favoritePlatform);
        }

        // POST: FavoritePlatforms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId, int platformId)
        {
            var favoritePlatform = await _context.FavoritePlatform
                .Include(f => f.Platform)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserId == userId && m.PlatformId == platformId);
            _context.FavoritePlatform.Remove(favoritePlatform);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create));
        }

        private bool FavoritePlatformExists(int id)
        {
            return _context.FavoritePlatform.Any(e => e.UserId == id);
        }

        private IQueryable<Platform> getPlatformsNotInFav(int? userId)
        {
            if (userId == null)
            {
                return null;
            }
            var platformsInFav = _context.FavoritePlatform.Where(f => f.UserId.Equals(userId))
                                                         .Select(fp => new { fp.PlatformId });

            var platformsNotInFav = _context.Platform.Where(p => !platformsInFav.Any(fp => fp.PlatformId.Equals(p.PlatformId)));

            return platformsNotInFav;
        }
    }
}
