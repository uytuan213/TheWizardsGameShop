﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly TheWizardsGameShopContext _context;

        public UserRolesController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: UserRoles
        public async Task<IActionResult> Index()
        {
            var theWizardsGameShopContext = _context.UserRole.Include(u => u.Role).Include(u => u.User);
            return View(await theWizardsGameShopContext.ToListAsync());
        }

        // GET: UserRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRole
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // GET: UserRoles/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.WizardsRole, "RoleId", "RoleName");
            ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "Email");
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.WizardsRole, "RoleId", "RoleName", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "Email", userRole.UserId);
            return View(userRole);
        }

        // GET: UserRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRole.FindAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.WizardsRole, "RoleId", "RoleName", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "Email", userRole.UserId);
            return View(userRole);
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,RoleId")] UserRole userRole)
        {
            if (id != userRole.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRoleExists(userRole.UserId))
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
            ViewData["RoleId"] = new SelectList(_context.WizardsRole, "RoleId", "RoleName", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "Email", userRole.UserId);
            return View(userRole);
        }

        // GET: UserRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRole
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userRole = await _context.UserRole.FindAsync(id);
            _context.UserRole.Remove(userRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRoleExists(int id)
        {
            return _context.UserRole.Any(e => e.UserId == id);
        }
    }
}
