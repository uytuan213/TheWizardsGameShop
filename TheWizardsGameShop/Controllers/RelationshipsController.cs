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
            if (!UserHelper.IsLoggedIn(this)) UserHelper.RequireLogin(this);
            var id = UserHelper.GetSessionUserId(this);
            var theWizardsGameShopContext = _context.Relationship.Include(r => r.SenderNavigation).Include(r => r.ReceiverNavigation).Where(r => (bool)r.IsAccepted);

            ViewData["friendRequests"] = RelationshipHelper.getRequestsReceived(id, _context);
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
                .Include(r => r.SenderNavigation)
                .Include(r => r.ReceiverNavigation)
                .FirstOrDefaultAsync(m => m.Sender == id);
            if (relationship == null)
            {
                return NotFound();
            }

            return View(relationship);
        }

        // GET: Relationships/Create
        public IActionResult Create()
        {
            var sessionUserId = UserHelper.GetSessionUserId(this);
            ViewData["UserId"] = sessionUserId;
            return View();
        }

        // POST: Relationships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string receiverUserName, [Bind("Sender,Receiver,IsFamily,IsAccepted")] Relationship relationship)
        {
            var receiver = _context.WizardsUser.Where(u => u.UserName == receiverUserName).FirstOrDefault();
            if (receiver != null)
            {
                relationship.Receiver = receiver.UserId;
                /*if (ModelState.IsValid)
                {*/
                    relationship.IsAccepted = false;
                    _context.Add(relationship);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                //}
            }
            var sessionUserId = UserHelper.GetSessionUserId(this);
            ViewData["UserId"] = sessionUserId;
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
            ViewData["Sender"] = new SelectList(_context.WizardsUser, "UserId", "Email", relationship.Sender);
            ViewData["Receiver"] = new SelectList(_context.WizardsUser, "UserId", "Email", relationship.Receiver);
            return View(relationship);
        }

        // POST: Relationships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Sender,Receiver,IsFamily,IsAccepted")] Relationship relationship)
        {
            if (id != relationship.Sender)
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
                    if (!RelationshipExists(relationship.Sender))
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
            ViewData["Sender"] = new SelectList(_context.WizardsUser, "UserId", "Email", relationship.Sender);
            ViewData["Receiver"] = new SelectList(_context.WizardsUser, "UserId", "Email", relationship.Receiver);
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
                .Include(r => r.SenderNavigation)
                .Include(r => r.ReceiverNavigation)
                .FirstOrDefaultAsync(m => m.Sender == id);
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
            return _context.Relationship.Any(e => e.Sender == id);
        }
    }
}
