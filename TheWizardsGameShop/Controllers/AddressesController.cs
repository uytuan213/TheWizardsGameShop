using System;
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
    public class AddressesController : Controller
    {
        private const string NOT_LOGGED_IN_MESSAGE = "Please log in to proceed.";

        private readonly TheWizardsGameShopContext _context;

        public AddressesController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        private RedirectToActionResult RequireLogin(Controller controller)
        {
            string actionName = controller.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = controller.ControllerContext.RouteData.Values["controller"].ToString();

            TempData["LoginMessage"] = NOT_LOGGED_IN_MESSAGE;
            TempData["RequestedActionName"] = actionName;
            TempData["RequestedControllerName"] = controllerName;

            return RedirectToAction("Login", "Users");
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
            {
                return RequireLogin(this);
            }

            var theWizardsGameShopContext = _context.Address.Include(a => a.ProvinceCodeNavigation).Include(a => a.User);
            return View(await theWizardsGameShopContext.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .Include(a => a.ProvinceCodeNavigation)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            if (!IsLoggedIn())
            {
                return RequireLogin(this);
            }

            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceName");
            //ViewData["UserId"] = new SelectList(_context.Users, "UserName", "Email");
            ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressId,UserId,Street1,Street2,City,ProvinceCode,PostalCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceName", address.ProvinceCode);
            
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", address.UserId);
            if (IsLoggedIn())
            {
                ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            }
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsLoggedIn())
            {
                return RequireLogin(this);
            }

            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", address.ProvinceCode);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserName", address.UserId);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AddressId,UserId,Street1,Street2,City,ProvinceCode,PostalCode")] Address address)
        {
            if (id != address.AddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.AddressId))
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
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", address.ProvinceCode);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", address.UserId);
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
            {
                return RequireLogin(this);
            }

            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .Include(a => a.ProvinceCodeNavigation)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }

            await DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
            //return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Address.FindAsync(id);
            _context.Address.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
            return _context.Address.Any(e => e.AddressId == id);
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("userId") != null;
        }
    }
}
