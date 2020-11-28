﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop.Controllers
{
    public class CartsController : Controller
    {
        private TheWizardsGameShopContext _context;

        public CartsController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: CartsController
        public ActionResult Index()
        {
            if (!UserHelper.IsLoggedIn(this))
            {
                return UserHelper.RequireLogin(this);
            }

            var cart = CartHelper.getCartFromSession(this);
            return View(cart);
        }

        // GET: CartsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CartsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartsController/Create
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

        // GET: CartsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartsController/Edit/5
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

        // GET: CartsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartsController/Delete/5
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
    }
}
