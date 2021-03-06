﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop.Controllers
{
    public class CreditCardsController : Controller
    {
        private readonly TheWizardsGameShopContext _context;

        public CreditCardsController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: CreditCards
        public async Task<IActionResult> Index()
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            var creditCards = await _context.CreditCard.Where(c => c.UserId.Equals(HttpContext.Session.GetInt32("userId"))).ToListAsync();
            if (creditCards != null)
            {
                foreach (var item in creditCards)
                {
                    decodeData(item);
                }
            }

            return View(creditCards);
        }

        // GET: CreditCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard
                .FirstOrDefaultAsync(m => m.CreditCardId == id);
            if (creditCard == null)
            {
                return NotFound();
            }

            return View(creditCard);
        }

        // GET: CreditCards/Create
        public IActionResult Create()
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            return View();
        }

        // POST: CreditCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CreditCardId,UserId,CreditCardNumber,ExpiryDate,Cvc,CardHolder")] CreditCard creditCard)
        {
            if (ModelState.IsValid)
            {
                encodeData(creditCard);
                creditCard.ExpiryDate = creditCard.ExpiryDate.Replace("/", "");
                _context.Add(creditCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));   
            }

            //ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "UserName", creditCard.UserId);
            if (UserHelper.IsLoggedIn(this))
            {
                ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            }
            return View(creditCard);
        }

        // GET: CreditCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            if (id == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard.FindAsync(id);
            if (creditCard == null)
            {
                return NotFound();
            }

            //ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "UserName", creditCard.UserId);

            decodeData(creditCard);
            return View(creditCard);
        }

        // POST: CreditCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CreditCardId,UserId,CreditCardNumber,ExpiryDate,Cvc,CardHolder")] CreditCard creditCard)
        {
            if (id != creditCard.CreditCardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    creditCard.ExpiryDate = creditCard.ExpiryDate.Replace("/", "");
                    encodeData(creditCard);
                    _context.Update(creditCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditCardExists(creditCard.CreditCardId))
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

            //ViewData["UserId"] = new SelectList(_context.WizardsUser, "UserId", "UserName", creditCard.UserId);
            return View(creditCard);
        }

        // GET: CreditCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CreditCardId == id);
            if (creditCard == null)
            {
                return NotFound();
            }


            await DeleteConfirmed(Convert.ToInt32(id));
            return RedirectToAction(nameof(Index));
            //return View(creditCard);
        }

        // POST: CreditCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var creditCard = await _context.CreditCard.FindAsync(id);
            _context.CreditCard.Remove(creditCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditCardExists(int id)
        {
            return _context.CreditCard.Any(e => e.CreditCardId == id);
        }

        /// <summary>
        /// Encode Credit card number and Cvc using Base64Helper
        /// </summary>
        /// <param name="creditCard">The credit card to encode</param>
        private void encodeData(CreditCard creditCard)
        {
            creditCard.CreditCardNumber = Base64Helper.encode(creditCard.CreditCardNumber);
            creditCard.Cvc = Base64Helper.encode(creditCard.Cvc);
        }

        /// <summary>
        /// Decode Credit card number and Cvc using Base64Helper
        /// </summary>
        /// <param name="creditCard">The credit card to decode</param>
        private void decodeData(CreditCard creditCard)
        {
            creditCard.CreditCardNumber = Base64Helper.decode(creditCard.CreditCardNumber);
            creditCard.Cvc = Base64Helper.decode(creditCard.Cvc);
        }
    }
}
