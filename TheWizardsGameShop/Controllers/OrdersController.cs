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
        private const float TAX = 0.13f;
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
                                              .Where(o => o.UserId == UserHelper.GetSessionUserId(this))
                                              .OrderByDescending(o => o.OrderId);

            return View(await orders.ToListAsync());
        }

        // GET: OrdersController
        public async Task<ActionResult> Admin()
        {
            if (!UserHelper.IsEmployee(this)) return UserHelper.RequireEmployee(this);

            var orders = _context.WizardsOrder.Include(o => o.OrderStatus)
                                              .Include(o => o.CreditCard)
                                              .Include(o => o.MailingAddress)
                                              .Include(o => o.ShippingAddress)
                                              .Include(o => o.User)
                                              .Where(o => o.OrderStatusId == 1)
                                              .OrderByDescending(o => o.OrderId);
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
        public async Task<ActionResult> Create()
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var sessionUserId = UserHelper.GetSessionUserId(this);
            var cardList = _context.CreditCard.Where(c => c.UserId == sessionUserId);
            await cardList.ForEachAsync(c => c.CreditCardNumber = Base64Helper.decode(c.CreditCardNumber));
            ViewData["UserId"] = sessionUserId;
            ViewData["CreditCardId"] = new SelectList(cardList, "CreditCardId", "CreditCardNumber");
            ViewData["MailingAddressId"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 1 && a.UserId == sessionUserId), "AddressId", "Street1");
            ViewData["ShippingAddressId"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 2 && a.UserId == sessionUserId), "AddressId", "Street1");
            ViewData["OrderStatusId"] = "1"; // Default to Processing
            ViewData["Total"] = calculateTotal(CartHelper.getCartFromSession(this));
            return View();
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("OrderId, UserId, CreditCardId, MailingAddressId, ShippingAddressId, OrderStatusId, Total")] WizardsOrder order)
        {
            var sessionUserId = UserHelper.GetSessionUserId(this);

            if (ModelState.IsValid)
            {
                var cart = CartHelper.getCartFromSession(this);
                order.UserId = (int)sessionUserId;
                order.OrderStatusId = 1; //Default is processing
                order.Total = calculateTotal(cart);
                _context.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cart)
                {
                    // Update order details
                    OrderDetail od = new OrderDetail() { OrderId = order.OrderId, GameId = item.Game.GameId, Quantity = item.Quantity, IsDigital = item.IsDigital };
                    _context.Add(od);

                    // Update game quantity
                    if (!item.IsDigital)
                    {
                        var game = _context.Game.Find(item.Game.GameId);
                        game.GameQty = (short)(game.GameQty - item.Quantity);
                        _context.Update(game);
                    }
                }

                await _context.SaveChangesAsync();
                CartHelper.clearCart(this);
                return RedirectToAction("Index");
            }

            var cardList = _context.CreditCard.Where(c => c.UserId == sessionUserId);
            await cardList.ForEachAsync(c => c.CreditCardNumber = Base64Helper.decode(c.CreditCardNumber));
            ViewData["UserId"] = sessionUserId;
            ViewData["CreditCardId"] = new SelectList(cardList, "CreditCardId", "CreditCardNumber");
            ViewData["MailingAddressId"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 1 && a.UserId == sessionUserId), "AddressId", "Street1");
            ViewData["ShippingAddressId"] = new SelectList(_context.Address.Include(a => a.AddressType).Where(a => a.AddressTypeId == 2 && a.UserId == sessionUserId), "AddressId", "Street1");
            ViewData["OrderStatusId"] = 1; // Default to Processing
            ViewData["Total"] = calculateTotal(CartHelper.getCartFromSession(this));

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

        // GET: OrdersController/Complete/5
        /*[ValidateAntiForgeryToken]*/
        public async Task<ActionResult> Complete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await CompleteConfirmed(Convert.ToInt32(id));
            return RedirectToAction(nameof(Admin));
        }

        // POST: OrdersController/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteConfirmed(int id)
        {
            var order = await _context.WizardsOrder.FindAsync(id);
            order.OrderStatusId = 2;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }

        public bool OrderExists(int id)
        {
            return _context.WizardsOrder.Any(e => e.OrderId== id);
        }

        private decimal calculateTotal(List<CartItem> cart)
        {
            decimal total = 0;

            // calculate total before tax
            foreach(var item in cart)
            {
                total += item.Game.GamePrice * item.Quantity;
            }

            //total after tax
            total = total * (decimal)(1 + TAX);
            return total;
        }
    }
}
