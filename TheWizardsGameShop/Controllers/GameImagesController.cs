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
    public class GameImagesController : Controller
    {
        private readonly TheWizardsGameShopContext _context;

        public GameImagesController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: GameImages
        public async Task<IActionResult> Index()
        {
            var theWizardsGameShopContext = _context.GameImage.Include(g => g.Game);
            return View(await theWizardsGameShopContext.ToListAsync());
        }

        // GET: GameImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameImage = await _context.GameImage
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.GameImageId == id);
            if (gameImage == null)
            {
                return NotFound();
            }

            return View(gameImage);
        }

        // GET: GameImages/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Game, "GameId", "GameDigitalPath");
            return View();
        }

        // POST: GameImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameImageId,GameId,GameImagePath")] GameImage gameImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Game, "GameId", "GameDigitalPath", gameImage.GameId);
            return View(gameImage);
        }

        // GET: GameImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameImage = await _context.GameImage.FindAsync(id);
            if (gameImage == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Game, "GameId", "GameDigitalPath", gameImage.GameId);
            return View(gameImage);
        }

        // POST: GameImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameImageId,GameId,GameImagePath")] GameImage gameImage)
        {
            if (id != gameImage.GameImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameImageExists(gameImage.GameImageId))
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
            ViewData["GameId"] = new SelectList(_context.Game, "GameId", "GameDigitalPath", gameImage.GameId);
            return View(gameImage);
        }

        // GET: GameImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameImage = await _context.GameImage
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.GameImageId == id);
            if (gameImage == null)
            {
                return NotFound();
            }

            return View(gameImage);
        }

        // POST: GameImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameImage = await _context.GameImage.FindAsync(id);
            _context.GameImage.Remove(gameImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameImageExists(int id)
        {
            return _context.GameImage.Any(e => e.GameImageId == id);
        }
    }
}
