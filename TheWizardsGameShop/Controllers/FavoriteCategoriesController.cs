using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var theWizardsGameShopContext = _context.FavoriteCategory.Include(f => f.GameCategory).Include(f => f.User);
            return View(await theWizardsGameShopContext.ToListAsync());
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
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1");
            ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "Email");
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", favoriteCategory.GameCategoryId);
            ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "Email", favoriteCategory.UserId);
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
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", favoriteCategory.GameCategoryId);
            ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "Email", favoriteCategory.UserId);
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
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", favoriteCategory.GameCategoryId);
            ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "Email", favoriteCategory.UserId);
            return View(favoriteCategory);
        }

        // GET: FavoriteCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: FavoriteCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favoriteCategory = await _context.FavoriteCategory.FindAsync(id);
            _context.FavoriteCategory.Remove(favoriteCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteCategoryExists(int id)
        {
            return _context.FavoriteCategory.Any(e => e.UserId == id);
        }
    }
}
