﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop.Controllers
{
    public class CartsController : Controller
    {
        private const string SESSION_CART = "cart";
        private TheWizardsGameShopContext _context;

        public CartsController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: Carts
        public ActionResult Index()
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var cart = CartHelper.getCartFromSession(this);
            ViewBag.TotalAmount = CartHelper.getTotalAmount(this);
            return View(cart);
        }

        // GET: Carts/Checkout
        public ActionResult Checkout()
        {
            //ViewData["Address"] = new SelectList(_context.Province, "AddressId", "Address1");
            return View();
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Carts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int id)
        {
            List<CartItem> cart = CartHelper.getCartFromSession(this);
            var item = cart.Where(c => c.Game.GameId == id).FirstOrDefault();
            if (item != null)
            {
                cart.Remove(item);

                //Update cart session
                var str = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString(SESSION_CART, str);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Carts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Carts/Add/5
        public IActionResult Add(int id, int quantity = 1, bool isDigital = false, bool goToCart = true)
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            List<CartItem> cart = CartHelper.getCartFromSession(this);
            var qtyInStock = _context.Game.Find(id).GameQty;

            if (cart.Any(c => c.Game.GameId == id))
            {
                // Add quantity by one if the game exists in the cart
                var item = cart.Where(c => c.Game.GameId == id).First();
                item.Quantity = item.Quantity + quantity;
                if (!isDigital && qtyInStock < item.Quantity)
                {
                    HttpContext.Session.SetString("modalTitle", "Error!");
                    HttpContext.Session.SetString("modalMessage", $"There are only {qtyInStock} item(s) in stock.");
                    return RedirectToAction("Index");
                }
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }
            else
            {
                if (!isDigital && qtyInStock < quantity)
                {
                    HttpContext.Session.SetString("modalTitle", "Error!");
                    HttpContext.Session.SetString("modalMessage", "Does not have enough of this game in stock.");
                    return RedirectToAction("Index");
                }

                // Add the game to the cart
                var item = new CartItem() { Game = _context.Game.Find(id), Quantity = quantity, IsDigital = isDigital };
                if (quantity > 0) cart.Add(item);
            }

            var str = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(SESSION_CART, str);

            if (goToCart)
            {
                return RedirectToAction("Index");
            } else
            {
                HttpContext.Session.SetString("modalTitle", "Game Added");
                HttpContext.Session.SetString("modalMessage", "The game has been added to your cart.");
                return RedirectToAction("Details", "Games", new { id = id });
            }
        }
    }
}
