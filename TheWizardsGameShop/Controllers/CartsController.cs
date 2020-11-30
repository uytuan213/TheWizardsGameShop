using Microsoft.AspNetCore.Http;
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
            return View();
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
        public IActionResult Add(int id, int quantity = 1, bool goToCart = true)
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            var str = HttpContext.Session.GetString(SESSION_CART);
            List<CartItem> cart = CartHelper.getCartFromSession(this);

            if (cart.Any(c => c.Game.GameId == id))
            {
                // Add quantity by one if the game exists in the cart
                var item = cart.Where(c => c.Game.GameId == id).First();
                item.Quantity = item.Quantity + quantity;
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }
            else
            {
                // Add the game to the cart
                var item = new CartItem() { Game = _context.Game.Find(id), Quantity = quantity };
                if (quantity > 0) cart.Add(item);
            }

            str = JsonConvert.SerializeObject(cart);
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
