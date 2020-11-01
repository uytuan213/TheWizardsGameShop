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
    public class GamesController : Controller
    {
        public const int PAGE_SIZE = 15;
        public const int SEARCH_SUGGESTIONS_COUNT = 10;
        private readonly TheWizardsGameShopContext _context;

        public GamesController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index(int pageNo=1, int? categoryId=null)
        {
            if (pageNo <= 0)
            {
                return NotFound();
            }
            int totalGames;
            IQueryable<Game> games;
            if (categoryId == null)
            {
                totalGames = _context.Game.Count();
                games = _context.Game.Include(g => g.GameCategory)
                                     .Include(g => g.GameStatusCodeNavigation)
                                     .Include(g => g.GameImage)
                                     .Skip((pageNo - 1) * PAGE_SIZE)
                                     .Take(PAGE_SIZE);
            }
            else
            {
                totalGames = _context.Game.Where(g => g.GameCategoryId.Equals(categoryId)).Count();
                games = _context.Game.Include(g => g.GameCategory)
                                     .Include(g => g.GameStatusCodeNavigation)
                                     .Include(g => g.GameImage)
                                     .Where(g => g.GameCategoryId.Equals(categoryId))
                                     .Skip((pageNo - 1) * PAGE_SIZE)
                                     .Take(PAGE_SIZE);
            }

            ViewBag.totalPages = GetTotalPages(totalGames);
            if (games.Count() == 0)
            {
                TempData["errorMessage"] = "This page does not exist";
                return NotFound();
            }
            return View(await games.ToListAsync());
        }

        // GET: Games/Admin
        public async Task<IActionResult> Admin(int pageNo = 1)
        {
            if (!UserHelper.IsEmployee(this)) return UserHelper.RequireEmployee(this);

            if (pageNo <= 0)
            {
                return NotFound();
            }
            int totalGames = _context.Game.Count();
            ViewBag.TotalPages = GetTotalPages(totalGames);

            var games = _context.Game.Include(g => g.GameCategory)
                                     .Include(g => g.GameStatusCodeNavigation)
                                     .Skip((pageNo - 1) * PAGE_SIZE)
                                     .Take(PAGE_SIZE);
            if (games.Count() == 0)
            {
                TempData["errorMessage"] = "This page does not exist";
                return NotFound();
            }

            ViewBag.PageNo = pageNo;
            return View(await games.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.GameCategory)
                .Include(g => g.GameStatusCodeNavigation)
                .Include(g => g.GameImage)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            ViewBag.AvgRating = CalculateAvgRating((int)id);

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1");
            ViewData["GameStatusCode"] = new SelectList(_context.GameStatus, "GameStatusCode", "GameStatus1");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,GameStatusCode,GameCategoryId,GameName,GameDescription,GamePrice,GameQty,GameDigitalPath")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", game.GameCategoryId);
            ViewData["GameStatusCode"] = new SelectList(_context.GameStatus, "GameStatusCode", "GameStatus1", game.GameStatusCode);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", game.GameCategoryId);
            ViewData["GameStatusCode"] = new SelectList(_context.GameStatus, "GameStatusCode", "GameStatus1", game.GameStatusCode);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,GameStatusCode,GameCategoryId,GameName,GameDescription,GamePrice,GameQty,GameDigitalPath")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
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
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", game.GameCategoryId);
            ViewData["GameStatusCode"] = new SelectList(_context.GameStatus, "GameStatusCode", "GameStatus1", game.GameStatusCode);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.GameCategory)
                .Include(g => g.GameStatusCodeNavigation)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Games/Search
        //public IActionResult Search()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Search(string keyword, int pageNo=1)
        {
            if (pageNo <= 0)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                int totalGames = _context.Game.Where(g => g.GameName.Contains(keyword)).Count();
                ViewBag.totalPages = GetTotalPages(totalGames);

                var searchResult = _context.Game.Include(g => g.GameImage)
                                                .Where(g => g.GameName.Contains(keyword))
                                                .Skip((pageNo - 1) * PAGE_SIZE)
                                                .Take(PAGE_SIZE);

                return View(await searchResult.ToListAsync());
            }

            ViewData["SearchKeyword"] = keyword;
            return View();
        }

        public async Task<IActionResult> SearchSuggestions(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                int totalGames = _context.Game.Where(g => g.GameName.Contains(keyword)).Count();
                ViewBag.totalPages = GetTotalPages(totalGames);

                var searchResult = _context.Game.Include(g => g.GameImage)
                                                .Where(g => g.GameName.Contains(keyword))
                                                .OrderBy(g => g.GameName)
                                                .Take(SEARCH_SUGGESTIONS_COUNT);

                return PartialView(await searchResult.ToListAsync());
            }

            ViewData["SearchKeyword"] = keyword;
            return PartialView();
        }

        public IActionResult SearchByCategory()
        {
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchByCategory(int catId, int pageNo=1)
        {
            if (pageNo <= 0)
            {
                return NotFound();
            }
            var searchResult = _context.Game.Include(g => g.GameImage)
                                            .Where(g => g.GameCategoryId.Equals(catId))
                                            .Skip((pageNo - 1) * PAGE_SIZE)
                                            .Take(PAGE_SIZE);

            return View(await searchResult.ToListAsync());
        }

        public double CalculateAvgRating(int gameId)
        {
            var avg = 0d;
            if (_context.Rating.Where(r => r.GameId.Equals(gameId)).Any())
                avg = _context.Rating.Where(r => r.GameId.Equals(gameId)).Select(r => r.Rate).Average();

            return avg;
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.GameId == id);
        }

        private int GetTotalPages(int totalItems)
        {
            return ((totalItems % PAGE_SIZE > 0) ? 1 : 0) + (totalItems / PAGE_SIZE);
        }
    }
}
