using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop.Controllers
{
    public class OrderDetailsController : Controller
    {
        private TheWizardsGameShopContext _context;

        public OrderDetailsController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: OrderDetailsController
        public ActionResult Index(int id)
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var order = _context.OrderDetail.Include(od => od.Game).Where(od => od.OrderId == id);
            return View(order);
        }

        // GET: OrderDetailsController/Details/5
        public async Task<ActionResult> Details(int orderId, int gameId)
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var orderDetails = _context.OrderDetail.Include(od => od.Game).Where(od => od.OrderId == orderId && od.GameId == gameId);
            return View(await orderDetails.ToListAsync());
        }

        // GET: OrderDetailsController/Create
        public ActionResult Create()
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            ViewData["Games"] = new SelectList(_context.Game, "GameId", "GameName");
            return View();
        }

        // POST: OrderDetailsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("OrderId, GameId, Quantity, IsDigital")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["Games"] = new SelectList(_context.Game, "GameId", "GameName");
            return View(orderDetail);
        }

        // GET: OrderDetailsController/Edit/5
        public async Task<ActionResult> Edit(int orderId, int gameId)
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var od = await _context.OrderDetail.Where(od => od.OrderId == orderId && od.GameId == gameId).FirstAsync();

            return View(od);
        }

        // POST: OrderDetailsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int orderId, int gameId, [Bind("OrderId, GameId, Quantity, IsDigital")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orderDetail.OrderId, orderDetail.GameId))
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

            return View(orderDetail);
        }

        // GET: OrderDetailsController/Delete/5
        public async Task<ActionResult> Delete(int orderId, int gameId)
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var order = await _context.OrderDetail
                                      .FirstOrDefaultAsync(m => m.OrderId == orderId && m.GameId == gameId);
            if (order == null)
            {
                return NotFound();
            }

            await DeleteConfirmed(orderId, gameId);
            return RedirectToAction(nameof(Index));
        }

        // POST: OrderDetailsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int orderId, int gameId)
        {
            var orderDetail = await _context.OrderDetail.Where(od => od.OrderId == orderId && od.GameId == gameId).FirstAsync();
            _context.OrderDetail.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public bool OrderExists(int orderId, int gameId)
        {
            return _context.OrderDetail.Any(e => e.OrderId == orderId && e.GameId == gameId);
        }
    }
}
