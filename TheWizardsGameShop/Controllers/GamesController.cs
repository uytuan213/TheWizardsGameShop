using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop.Controllers
{
    public class GamesController : Controller
    {
        public const int PAGE_SIZE = 15;
        public const int HOME_COUNT = 3;
        public const int SEARCH_SUGGESTIONS_COUNT = 10;
        private readonly TheWizardsGameShopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GamesController(TheWizardsGameShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
                                     .OrderByDescending(g => g.GameId)
                                     .Skip((pageNo - 1) * PAGE_SIZE)
                                     .Take(PAGE_SIZE);
            }
            else
            {
                totalGames = _context.Game
                    .Include(g => g.GameCategory)
                    .Where(g => g.GameCategory.GameCategoryId == categoryId)
                    .Count();
                games = _context.Game.Include(g => g.GameCategory)
                                     .Include(g => g.GameStatusCodeNavigation)
                                     .Include(g => g.GameImage)
                                     .Where(g => g.GameCategory.GameCategoryId == categoryId)
                                     .OrderByDescending(g => g.GameId)
                                     .Skip((pageNo - 1) * PAGE_SIZE)
                                     .Take(PAGE_SIZE);
            }

            ViewBag.totalPages = GetTotalPages(totalGames);
            ViewBag.Count = totalGames;
            ViewBag.PageNo = pageNo;
            ViewBag.CategoryId = categoryId;
            //if (games.Count() == 0)
            //{
            //    TempData["errorMessage"] = "This page does not exist";
            //    return NotFound();
            //}
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

            var games = _context.Game.Include(g => g.GamePlatform)
                                     .Include(g => g.GameCategory)
                                     .Include(g => g.GameStatusCodeNavigation)
                                     .Include(g => g.GameImage)
                                     .OrderByDescending(g => g.GameId)
                                     .Skip((pageNo - 1) * PAGE_SIZE)
                                     .Take(PAGE_SIZE);
            //if (games == null || games.Count() == 0)
            //{
            //    TempData["errorMessage"] = "This page does not exist";
            //    return NotFound();
            //}

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
                .Include(g => g.GamePlatform)
                .Include(g => g.GameStatusCodeNavigation)
                .Include(g => g.GameImage)
                .FirstOrDefaultAsync(m => m.GameId == id);


            if (game == null)
            {
                return NotFound();
            }
            var isPurchased = false;
            var gameReviews = _context.Review.Include(r => r.User).Where(r => r.GameId.Equals(id) && r.IsPublished).OrderByDescending(r => r.ReviewDate).ToList();
            if (UserHelper.IsLoggedIn(this))
            {
                var userId = UserHelper.GetSessionUserId(this);
                var userReview = await _context.Review.Include(r => r.User).FirstOrDefaultAsync(r => r.GameId.Equals(id) && r.UserId.Equals(userId));
                var userRating = await _context.Rating.FirstOrDefaultAsync(r => r.GameId.Equals(id) && r.UserId.Equals(userId));
                gameReviews = _context.Review.Include(r => r.User).Where(r => r.GameId.Equals(id) && r.IsPublished && r.UserId != userId).ToList();
                ViewData["UserReview"] = userReview;
                if (userRating != null) ViewData["UserRatingRate"] = userRating.Rate;

                isPurchased = _context.WizardsOrder.Join(_context.OrderDetail, o => o.OrderId, od => od.OrderId, (o, od) => new { order = o, detail = od }).Any(x => x.detail.GameId == id);
            }
            ViewData["IsPurchased"] = isPurchased;
            ViewData["AvgRating"] = CalculateAvgRating((int)id);
            ViewData["GameReviews"] = gameReviews;
            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            if (!UserHelper.IsEmployee(this))
            {
                return UserHelper.RequireEmployee(this);
            }
            ViewData["GamePlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName");
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1");
            ViewData["GameStatusCode"] = new SelectList(_context.GameStatus, "GameStatusCode", "GameStatus1");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,GameStatusCode,GamePlatformId,GameCategoryId,GameName,GameDescription,GamePrice,GameQty")] Game game,
            [FromForm(Name="gameFile")] IFormFile gameFile = null)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                if (gameFile != null)
                {
                    game.GameDigitalPath = UploadedFile(game.GameId, gameFile);
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Admin));
            }
            ViewData["GamePlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName");
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", game.GameCategoryId);
            ViewData["GameStatusCode"] = new SelectList(_context.GameStatus, "GameStatusCode", "GameStatus1", game.GameStatusCode);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!UserHelper.IsEmployee(this))
            {
                return UserHelper.RequireEmployee(this);
            }

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["GamePlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName");
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", game.GameCategoryId);
            ViewData["GameStatusCode"] = new SelectList(_context.GameStatus, "GameStatusCode", "GameStatus1", game.GameStatusCode);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,GameStatusCode,GamePlatformId,GameCategoryId,GameName,GameDescription,GamePrice,GameQty,GameDigitalPath")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*if (gameFile != null)
                    {
                        var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, $"download\\{game.GameId}");
                        // Delete the old game file if exists
                        if (Directory.EnumerateFileSystemEntries(folderPath).Any())
                        {
                            DirectoryInfo di = new DirectoryInfo(folderPath);
                            foreach(FileInfo file in di.GetFiles())
                            {
                                file.Delete();
                            }
                        }
                        game.GameDigitalPath = UploadedFile(game.GameId, gameFile);
                    }*/

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
                return RedirectToAction(nameof(Admin));
            }
            ViewData["GamePlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName");
            ViewData["GameCategoryId"] = new SelectList(_context.GameCategory, "GameCategoryId", "GameCategory1", game.GameCategoryId);
            ViewData["GameStatusCode"] = new SelectList(_context.GameStatus, "GameStatusCode", "GameStatus1", game.GameStatusCode);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!UserHelper.IsEmployee(this))
            {
                return UserHelper.RequireEmployee(this);
            }

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
            return RedirectToAction(nameof(Admin));
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
                                                .Include(g => g.GameCategory)
                                                .Include(g => g.GameStatusCodeNavigation)
                                                .Where(g => g.GameName.Contains(keyword))
                                                .Skip((pageNo - 1) * PAGE_SIZE)
                                                .Take(PAGE_SIZE);

                ViewData["SearchKeyword"] = keyword;
                ViewBag.Count = totalGames;
                return View(await searchResult.ToListAsync());
            }

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

                ViewData["SearchKeyword"] = keyword;
                return PartialView(await searchResult.ToListAsync());
            }

            ViewData["SearchKeyword"] = keyword;
            return PartialView();
        }

        public async Task<IActionResult> Home(int categoryId)
        {
            var searchResult = _context.Game.Include(g => g.GameImage)
                                                .Where(g => g.GameCategory.GameCategoryId == categoryId)
                                                .OrderByDescending(g => g.GameId)
                                                .Take(HOME_COUNT);

            return PartialView(await searchResult.ToListAsync());
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

        public FileContentResult Download(int id)
        {
            var downloadPath = Path.Combine(_webHostEnvironment.WebRootPath, $"download\\{id}");
            if (!Directory.Exists(downloadPath))
            {
                // TODO: return null will break the website, should be change
                return null;
            }

            var filePath = Directory.GetFiles(downloadPath).First();
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Text.Plain, fileName);
        }

        public double CalculateAvgRating(int gameId)
        {
            var avg = 0d;
            if (_context.Rating.Where(r => r.GameId.Equals(gameId)).Any())
                avg = _context.Rating.Where(r => r.GameId.Equals(gameId)).Select(r => r.Rate).Average();

            return avg;
        }

        public bool GameExists(int id)
        {
            return _context.Game.Any(e => e.GameId == id);
        }

        private int GetTotalPages(int totalItems)
        {
            return ((totalItems % PAGE_SIZE > 0) ? 1 : 0) + (totalItems / PAGE_SIZE);
        }

        private string UploadedFile(int gameId, IFormFile gameFile)
        {
            string filePath = null;

            if (gameFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, $"download\\{gameId}");
                string fileName = gameId + "_" + gameFile.FileName;
                filePath = Path.Combine(uploadsFolder, fileName);

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    gameFile.CopyTo(fileStream);
                }
            }
            return "\\" + Path.GetRelativePath(_webHostEnvironment.WebRootPath, filePath);
        }
    }
}
