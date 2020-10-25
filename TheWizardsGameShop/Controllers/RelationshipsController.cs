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
    public class RelationshipsController : Controller
    {
        private readonly TheWizardsGameShopContext _context;

        public RelationshipsController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: Relationships
        public async Task<IActionResult> Index()
        {
            var theWizardsGameShopContext = _context.Relationship.Include(r => r.UserId1Navigation).Include(r => r.UserId2Navigation);
            return View(await theWizardsGameShopContext.ToListAsync());
        }

        // GET: Relationships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relationship = await _context.Relationship
                .Include(r => r.UserId1Navigation)
                .Include(r => r.UserId2Navigation)
                .FirstOrDefaultAsync(m => m.UserId1 == id);
            if (relationship == null)
            {
                return NotFound();
            }

            return View(relationship);
        }

        // GET: Relationships/Create
        public IActionResult Create()
        {
            ViewData["UserId1"] = new SelectList(_context.WizardsUser, "UserId", "Email");
            ViewData["UserId2"] = new SelectList(_context.WizardsUser, "UserId", "Email");
            return View();
        }

        // POST: Relationships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId1,UserId2,IsFamily")] Relationship relationship)
        {
            if (ModelState.IsValid)
            {
                _context.Add(relationship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId1"] = new SelectList(_context.WizardsUser, "UserId", "Email", relationship.UserId1);
            ViewData["UserId2"] = new SelectList(_context.WizardsUser, "UserId", "Email", relationship.UserId2);
            return View(relationship);
        }

        // GET: Relationships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relationship = await _context.Relationship.FindAsync(id);
            if (relationship == null)
            {
                return NotFound();
            }
            ViewData["UserId1"] = new SelectList(_context.WizardsUser, "UserId", "Email", relationship.UserId1);
            ViewData["UserId2"] = new SelectList(_context.WizardsUser, "UserId", "Email", relationship.UserId2);
            return View(relationship);
        }

        // POST: Relationships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId1,UserId2,IsFamily")] Relationship relationship)
        {
            if (id != relationship.UserId1)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(relationship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelationshipExists(relationship.UserId1))
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
            ViewData["UserId1"] = new SelectList(_context.WizardsUser, "UserId", "Email", relationship.UserId1);
            ViewData["UserId2"] = new SelectList(_context.WizardsUser, "UserId", "Email", relationship.UserId2);
            return View(relationship);
        }

        // GET: Relationships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relationship = await _context.Relationship
                .Include(r => r.UserId1Navigation)
                .Include(r => r.UserId2Navigation)
                .FirstOrDefaultAsync(m => m.UserId1 == id);
            if (relationship == null)
            {
                return NotFound();
            }

            return View(relationship);
        }

        // POST: Relationships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var relationship = await _context.Relationship.FindAsync(id);
            _context.Relationship.Remove(relationship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RelationshipExists(int id)
        {
            return _context.Relationship.Any(e => e.UserId1 == id);
        }
    }
}
