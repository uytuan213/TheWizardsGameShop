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
    public class FavoriteCategoriesController : Controller
    {
        private readonly TheWizardsGameShopContext _context;

        public FavoriteCategoriesController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: FavoriteCategories
        public async Task<IActionResult> Index()
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            var sessionUserId = UserHelper.GetSessionUserId(this);
            var context = _context.FavoriteCategory
                .Include(f => f.GameCategory)
                .Where(f => f.UserId.Equals(sessionUserId));
            return View(await context.ToListAsync());
        }

        // GET: FavoriteCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoriteCategory = await _context.FavoriteCategory
                .Include(f => f.GameCategory)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (favoriteCategory == null)
            {
                return NotFound();
            }

            return View(favoriteCategory);
        }

        // GET: FavoriteCategories/Create
        public IActionResult Create()
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            var userId = UserHelper.GetSessionUserId(this);
            var favoriteCategoryContext = _context.FavoriteCategory
                .Include(f => f.GameCategory)
                .Where(f => f.UserId.Equals(userId));
            ViewData["UserId"] = userId;
            ViewData["FavoriteCategories"] = favoriteCategoryContext.ToList();
            ViewData["GameCategories"] = getCategoriesNotInFav(userId);

            return View();
        }

        // POST: FavoriteCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,GameCategoryId")] FavoriteCategory favoriteCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favoriteCategory);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                //return View(favoriteCategory);
            }

            var userId = UserHelper.GetSessionUserId(this);
            var favoriteCategoryContext = _context.FavoriteCategory
                .Include(f => f.GameCategory)
                .Where(f => f.UserId.Equals(userId));
            ViewData["UserId"] = userId;
            ViewData["FavoriteCategories"] = favoriteCategoryContext.ToList();
            ViewData["GameCategories"] = getCategoriesNotInFav(userId);

            return View(favoriteCategory);
        }

        // GET: FavoriteCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoriteCategory = await _context.FavoriteCategory.FindAsync(id);
            if (favoriteCategory == null)
            {
                return NotFound();
            }

            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", favoriteCategory.GameCategoryId);

            return View(favoriteCategory);
        }

        // POST: FavoriteCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,GameCategoryId")] FavoriteCategory favoriteCategory)
        {
            if (id != favoriteCategory.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favoriteCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteCategoryExists(favoriteCategory.UserId))
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
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", favoriteCategory.GameCategoryId);
            return View(favoriteCategory);
        }

        // GET: FavoriteCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = HttpContext.Session.GetInt32("userId");

            if (id == null || userId == null)
            {
                return NotFound();
            }

            var favoriteCategory = await _context.FavoriteCategory
                .Include(f => f.GameCategory)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserId == userId && m.GameCategoryId == id);
            if (favoriteCategory == null)
            {
                return NotFound();
            }

            await DeleteConfirmed(Convert.ToInt32(userId), Convert.ToInt32(favoriteCategory.GameCategoryId));
            return RedirectToAction(nameof(Create));
        }

        // POST: FavoriteCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId, int gameCategoryId)
        {
            var favoriteCategory = await _context.FavoriteCategory
                .Include(f => f.GameCategory)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserId == userId && m.GameCategoryId == gameCategoryId);
            _context.FavoriteCategory.Remove(favoriteCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create));
        }

        private bool FavoriteCategoryExists(int id)
        {
            return _context.FavoriteCategory.Any(e => e.UserId == id);
        }

        private IQueryable<GameCategory> getCategoriesNotInFav(int? userId)
        {
            if (userId == null)
            {
                return null;
            }
            var catsInFav = _context.FavoriteCategory.Where(f => f.UserId.Equals(userId))
                                                         .Select(fp => new { fp.GameCategoryId });

            var catsNotInFav = _context.GameCategory.Where(p => !catsInFav.Any(fp => fp.GameCategoryId.Equals(p.GameCategoryId)));

            return catsNotInFav;
        }
    }
}
