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
    public class GameStatusesController : Controller
    {
        private readonly TheWizardsGameShopContext _context;

        public GameStatusesController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: GameStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.GameStatus.ToListAsync());
        }

        // GET: GameStatus/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameStatus = await _context.GameStatus
                .FirstOrDefaultAsync(m => m.GameStatusCode == id);
            if (gameStatus == null)
            {
                return NotFound();
            }

            return View(gameStatus);
        }

        // GET: GameStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameStatusCode,GameStatus1")] GameStatus gameStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameStatus);
        }

        // GET: GameStatus/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameStatus = await _context.GameStatus.FindAsync(id);
            if (gameStatus == null)
            {
                return NotFound();
            }
            return View(gameStatus);
        }

        // POST: GameStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("GameStatusCode,GameStatus1")] GameStatus gameStatus)
        {
            if (id != gameStatus.GameStatusCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameStatusExists(gameStatus.GameStatusCode))
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
            return View(gameStatus);
        }

        // GET: GameStatus/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameStatus = await _context.GameStatus
                .FirstOrDefaultAsync(m => m.GameStatusCode == id);
            if (gameStatus == null)
            {
                return NotFound();
            }

            return View(gameStatus);
        }

        // POST: GameStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gameStatus = await _context.GameStatus.FindAsync(id);
            _context.GameStatus.Remove(gameStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameStatusExists(string id)
        {
            return _context.GameStatus.Any(e => e.GameStatusCode == id);
        }
    }
}
