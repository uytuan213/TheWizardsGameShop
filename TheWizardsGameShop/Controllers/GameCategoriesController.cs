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
    public class GameCategoriesController : Controller
    {
        private readonly TheWizardsGameShopContext _context;

        public GameCategoriesController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: GameCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.GameCategory.ToListAsync());
        }

        // GET: GameCategories/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameCategory = await _context.GameCategory
                .FirstOrDefaultAsync(m => m.GameCategoryId == id);
            if (gameCategory == null)
            {
                return NotFound();
            }

            return View(gameCategory);
        }

        // GET: GameCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameCategoryId,GameCategory1")] GameCategory gameCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameCategory);
        }

        // GET: GameCategories/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameCategory = await _context.GameCategory.FindAsync(id);
            if (gameCategory == null)
            {
                return NotFound();
            }
            return View(gameCategory);
        }

        // POST: GameCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("GameCategoryId,GameCategory1")] GameCategory gameCategory)
        {
            if (id != gameCategory.GameCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameCategoryExists(gameCategory.GameCategoryId))
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
            return View(gameCategory);
        }

        // GET: GameCategories/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameCategory = await _context.GameCategory
                .FirstOrDefaultAsync(m => m.GameCategoryId == id);
            if (gameCategory == null)
            {
                return NotFound();
            }

            return View(gameCategory);
        }

        // POST: GameCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var gameCategory = await _context.GameCategory.FindAsync(id);
            _context.GameCategory.Remove(gameCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameCategoryExists(short id)
        {
            return _context.GameCategory.Any(e => e.GameCategoryId == id);
        }
    }
}
