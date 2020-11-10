using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop.Controllers
{
    public class GameImagesController : Controller
    {
        private readonly TheWizardsGameShopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GameImagesController(TheWizardsGameShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Create(int gameId)
        {
            //ViewData["GameId"] = new SelectList(_context.Game, "GameId", "GameName");
            ViewData["GameId"] = gameId;
            return View();
        }

        // POST: GameImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm(Name = "imageFile")] IFormFile imageFile ,[Bind("GameImageId,GameId")] GameImage gameImage, bool isThumbnail = false)
        {
            if (true) //ModelState.IsValid
            {
                string filePath = UploadedFile(gameImage.GameId, imageFile, isThumbnail);
                gameImage.GameImagePath = filePath;
                _context.Add(gameImage);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Admin", "Games");
            }
            //ViewData["GameId"] = new SelectList(_context.Game, "GameId", "GameName", gameImage.GameId);
            ViewData["GameId"] = gameImage.GameId;
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
            ViewData["GameId"] = new SelectList(_context.Game, "GameId", "GameName", gameImage.GameId);
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

        private string UploadedFile(int gameId, IFormFile imageFile, bool isThumbnail)
        {
            string filePath = null;

            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, $"images\\{gameId}");
                //string fileName = isThumbnail ? "Thumbnail_" + imageFile.FileName : Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string fileName = isThumbnail ? "00.png" : "01.png";
                filePath = Path.Combine(uploadsFolder, fileName);

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
            }
            return filePath;
        }
    }
}
