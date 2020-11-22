using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheWizardsGameShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TheWizardsGameShop.Controllers
{
    public class OrdersController : Controller
    {
        private TheWizardsGameShopContext _context;

        public OrdersController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: OrdersController
        public async Task<ActionResult> Index()
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var orders = _context.WizardsOrder.Include(o => o.OrderStatus)
                                              .Include(o => o.CreditCard)
                                              .Include(o => o.MailingAddress)
                                              .Include(o => o.ShippingAddress)
                                              .Where(o => o.UserId == UserHelper.GetSessionUserId(this));
            return View(await orders.ToListAsync());
        }

        // GET: OrdersController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var order = await _context.WizardsOrder.Include(o => o.OrderStatus)
                                             .Include(o => o.CreditCard)
                                             .Include(o => o.MailingAddress)
                                             .Include(o => o.ShippingAddress)
                                             .Where(o => o.OrderId == id)
                                             .FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // GET: OrdersController/Create
        public ActionResult Create()
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }
            ViewData["CreditCards"] = new SelectList(_context.CreditCard.Where(c => c.UserId == UserHelper.GetSessionUserId(this)), "CreditCardId", "CreditCardNumber");
            ViewData["MailingAddresses"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 1), "AddressId", "Street1");
            ViewData["ShippingAddresses"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 2), "AddressId", "Street1");
            return View();
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("OrderId, UserId, CreditCardId, MailingAddressId, ShippingAddressId")] WizardsOrder order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["CreditCards"] = new SelectList(_context.CreditCard.Where(c => c.UserId == UserHelper.GetSessionUserId(this)), "CreditCardId", "CreditCardNumber");
            ViewData["MailingAddresses"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 1), "AddressId", "Street1");
            ViewData["ShippingAddresses"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 2), "AddressId", "Street1");

            return View(order);
        }

        // GET: OrdersController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var order = await _context.WizardsOrder.FindAsync(id);
            ViewData["CreditCards"] = new SelectList(_context.CreditCard.Where(c => c.UserId == UserHelper.GetSessionUserId(this)), "CreditCardId", "CreditCardNumber");
            ViewData["MailingAddresses"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 1), "AddressId", "Street1");
            ViewData["ShippingAddresses"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 2), "AddressId", "Street1");

            return View(order);
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("OrderId,CreditCardId, MailingAddressId, ShippingAddressId")] WizardsOrder order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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

            ViewData["CreditCards"] = new SelectList(_context.CreditCard.Where(c => c.UserId == UserHelper.GetSessionUserId(this)), "CreditCardId", "CreditCardNumber");
            ViewData["MailingAddresses"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 1), "AddressId", "Street1");
            ViewData["ShippingAddresses"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 2), "AddressId", "Street1");

            return View(order);
        }

        // GET: OrdersController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var order = await _context.WizardsOrder
                                      .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            await DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: OrdersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.WizardsOrder.FindAsync(id);
            _context.WizardsOrder.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public bool OrderExists(int id)
        {
            return _context.WizardsOrder.Any(e => e.OrderId== id);
        }
    }
}
