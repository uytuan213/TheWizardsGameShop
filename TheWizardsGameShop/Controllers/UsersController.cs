using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop.Controllers
{
    public class UsersController : Controller
    {
        private const int LIMIT_LOGIN_ATTEMPT = 3;
        private const int TOTAL_WAIT_IN_SECOND = 30;
        private readonly TheWizardsGameShopContext _context;

        public UsersController(TheWizardsGameShopContext context)
        {
            _context = context;
        }

        // GET: User/Menu
        public IActionResult Menu()
        {
            return View();
        }

        // GET: User/ResetPassword
        public IActionResult ResetPassword()
        {
            return View();
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var theWizardsGameShopContext = _context.Users.Include(u => u.GenderNavigation);
            return View(await theWizardsGameShopContext.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .Include(u => u.GenderNavigation)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            ViewData["Gender"] = new SelectList(_context.Gender, "Gender1", "Gender1");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string passwordConfirm, [Bind("UserId,UserName,PasswordHash,FirstName,Dob,LastName,Phone,Email,Gender,ReceivePromotionalEmails")] Users users)
        {
            Boolean isValid = true;

            if (!string.IsNullOrEmpty(users.PasswordHash) && passwordConfirm != users.PasswordHash)
            {
                isValid = false;
                TempData["PasswordConfirmMessage"] = "Password does not match.";
            }
            if (_context.Users.Where(u => u.UserName.Equals(users.UserName)).Any())
            {
                isValid = false;
                TempData["UserExistedMessage"] = "Username is used by another user.";
            }
            if (isValid && ModelState.IsValid)
            {
                users.PasswordHash = HashHelper.ComputeHash(users.PasswordHash);
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction("index", "home");
            }
            ViewData["Gender"] = new SelectList(_context.Gender, "Gender1", "Gender1", users.Gender);
            return View(users);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["Gender"] = new SelectList(_context.Gender, "Gender1", "Gender1", users.Gender);
            return View(users);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,PasswordHash,FirstName,Dob,LastName,Phone,Email,Gender,ReceivePromotionalEmails")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
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
            ViewData["Gender"] = new SelectList(_context.Gender, "Gender1", "Gender1", users.Gender);
            return View(users);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .Include(u => u.GenderNavigation)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.FindAsync(id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        // GET: User/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("loginAttempts") == null)
            {
                HttpContext.Session.SetInt32("loginAttempts", 1);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("UserName, PasswordHash")] Users users)
        {
            // If user is blocked because of going over the login attempts
            if (HttpContext.Session.GetString("isBlock") != null)
            {
                TimeSpan t = DateTime.Parse(HttpContext.Session.GetString("isBlock")) - DateTime.Now;
                if (t.TotalSeconds > TOTAL_WAIT_IN_SECOND)
                {
                    HttpContext.Session.Remove("isBlock");

                    //Reset loginAttempts in session
                    HttpContext.Session.SetInt32("loginAttempts", 1);
                }
                else
                {
                    TempData["LoginBlockMessage"] = $"Please be back after {TOTAL_WAIT_IN_SECOND - t.TotalSeconds} seconds";
                    return RedirectToAction("Index", "Home");
                }
            }

            // If user already logged in
            if (HttpContext.Session.GetInt32("userId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            var username = users.UserName;
            var password = users.PasswordHash;
            var passwordHash = HashHelper.ComputeHash(password);
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var userResult = _context.Users.Where(u => u.UserName.Equals(username)).FirstOrDefault();
                var isMatch = userResult != null && userResult.PasswordHash.Equals(passwordHash);

                // User logged in
                if (isMatch)
                {
                    var role = _context.UserRole.Where(us => us.UserId.Equals(userResult.UserId)).FirstOrDefault();
                    HttpContext.Session.SetInt32("userId", userResult.UserId);
                    HttpContext.Session.SetString("userName", userResult.UserName);
                    if (role != null) {
                        HttpContext.Session.SetString("userRole", role.Role.RoleName);
                            }
                    HttpContext.Session.SetString("loggedInTime", DateTime.Now.ToString());

                    TempData["Message"] = "Login successful";

                    return RedirectToAction("Index", "Home");
                }
            }

            // User login failed or usernam/password empty
            TempData["Message"] = "Login failed";

            //Update login attempts in session if failed
            var loginAttempts = HttpContext.Session.GetInt32("loginAttempts");
            if (loginAttempts < LIMIT_LOGIN_ATTEMPT)
            {
                HttpContext.Session.SetInt32("loginAttempts", (int)(++loginAttempts));
            }
            else
            {
                HttpContext.Session.SetString("isBlock", DateTime.Now.ToString());
                TempData["StartBlockingMessage"] = $"You have failed to login {LIMIT_LOGIN_ATTEMPT} time";
            }

            return View(users);
        }

        // GET: User/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("userName");
            HttpContext.Session.Remove("userRole");
            HttpContext.Session.Remove("loggedInTime");
            return RedirectToAction("Index", "Home");
        }
    }
}
