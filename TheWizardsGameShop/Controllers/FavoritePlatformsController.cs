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
            var theWizardsGameShopContext = _context.FavoritePlatform.Include(f => f.Platform).Where(f => f.UserId.Equals(HttpContext.Session.GetInt32("userId")));
            return View(await theWizardsGameShopContext.ToListAsync());
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

            ViewData["PlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName");
            ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName", favoritePlatform.PlatformId);
            if (UserHelper.IsLoggedIn(this))
            {
                ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            }
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

        // POST: FavoritePlatforms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favoritePlatform = await _context.FavoritePlatform.FindAsync(id);
            _context.FavoritePlatform.Remove(favoritePlatform);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoritePlatformExists(int id)
        {
            return _context.FavoritePlatform.Any(e => e.UserId == id);
        }
    }
}
