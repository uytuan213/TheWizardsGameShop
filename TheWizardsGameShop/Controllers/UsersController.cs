using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using TheWizardsGameShop.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

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
            return NotFound();
        }

        // GET: User/Employee
        public IActionResult Employee()
        {
            if (!UserHelper.IsEmployee(this)) return UserHelper.RequireEmployee(this);

            return View();
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.WizardsUser
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
        public async Task<IActionResult> Create(string passwordConfirm, [Bind("UserId,UserName,PasswordHash,FirstName,Dob,LastName,Phone,Email,Gender,ReceivePromotionalEmails")] WizardsUser users)
        {
            Boolean isValid = true;

            if (string.IsNullOrEmpty(users.PasswordHash) || passwordConfirm != users.PasswordHash)
            {
                isValid = false;
                TempData["PasswordConfirmMessage"] = "Password does not match.";
            }
            if (_context.WizardsUser.Where(u => u.UserName.Equals(users.UserName)).Any())
            {
                isValid = false;
                TempData["UserExistedMessage"] = "Username is used by another user.";
            }
            if (isValid && ModelState.IsValid)
            {
                users.PasswordHash = HashHelper.ComputeHash(users.PasswordHash);
                UserRole userRole = new UserRole();
                
                // Assign "Customer" role to the new user
                userRole.Role = _context.WizardsRole.Where(r => r.RoleName.Equals("Customer")).FirstOrDefault();
                userRole.User = users;
                _context.Add(users);
                _context.Add(userRole);
                await _context.SaveChangesAsync();

                CreateUserSession(users);

                return RedirectToAction("index", "home");
            }
            ViewData["Gender"] = new SelectList(_context.Gender, "Gender1", "Gender1", users.Gender);
            return View(users);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

            var sessionUserId = HttpContext.Session.GetInt32("userId");
            // Check if logged in and the param matches session
            if (UserHelper.IsLoggedIn(this) && id == null)
            {
                id = sessionUserId;
            }
            
            if (id != null && id != sessionUserId)
            {
                HttpContext.Session.SetString("modalTitle", "Not authorized");
                HttpContext.Session.SetString("modalMessage", "You are not authorized to access the page.");
                return RedirectToAction("Index", "Home");
            }

            var users = await _context.WizardsUser.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = id.ToString();
            ViewData["Gender"] = new SelectList(_context.Gender, "Gender1", "Gender1", users.Gender);
            return View(users);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,FirstName,Dob,LastName,Phone,Email,Gender,ReceivePromotionalEmails")] WizardsUser users)
        {
            //if (id == null && IsLoggedIn())
            //{
            //    int sessionUserId = HttpContext.Session.GetInt32("userId");
            //    id = sessionUserId;
            //}
            
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userToUpdate = _context.WizardsUser.Where(u => u.UserId.Equals(users.UserId)).FirstOrDefault();
                    userToUpdate.FirstName = users.FirstName;
                    userToUpdate.LastName = users.LastName;
                    userToUpdate.Gender = users.Gender;
                    userToUpdate.Email = users.Email;
                    userToUpdate.Dob = users.Dob;
                    userToUpdate.Phone = users.Phone;
                    userToUpdate.ReceivePromotionalEmails = users.ReceivePromotionalEmails;

                    _context.Update(userToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserHelper.UserExists(users.UserId, _context))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Menu));
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

            var users = await _context.WizardsUser
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
            var users = await _context.WizardsUser.FindAsync(id);
            _context.WizardsUser.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Login(String? actionName, String? controllerName, [Bind("UserName, PasswordHash")] WizardsUser users)
        {
            TempData["LoginMessage"] = UserHelper.NOT_LOGGED_IN_MESSAGE;
            TempData["RequestedActionName"] = actionName;
            TempData["RequestedControllerName"] = controllerName;

            // If user is blocked because of going over the login attempts
            if (HttpContext.Session.GetString("isBlock") != null)
            {
                TimeSpan t = DateTime.Now - DateTime.Parse(HttpContext.Session.GetString("isBlock"));
                if (t.TotalSeconds > TOTAL_WAIT_IN_SECOND)
                {
                    HttpContext.Session.Remove("isBlock");

                    //Reset loginAttempts in session
                    HttpContext.Session.SetInt32("loginAttempts", 1);
                }
                else
                {
                    TempData["Message"] = $"Please try again in {TOTAL_WAIT_IN_SECOND - t.TotalSeconds} second(s).";
                    return View(users);
                }
            }

            // If user already logged in
            if (UserHelper.IsLoggedIn(this))
            {
                if (!String.IsNullOrEmpty(actionName) && !String.IsNullOrEmpty(controllerName))
                {
                    return RedirectToAction(actionName, controllerName);
                }
                return RedirectToAction("Index", "Home");
            }

            var username = users.UserName;
            var password = users.PasswordHash;

            // Check empty
            if (string.IsNullOrEmpty(username))
            {
                TempData["Message"] = "Please enter username.";
                return View(users);
            }
            if (string.IsNullOrEmpty(password))
            {
                TempData["Message"] = "Please enter password.";
                return View(users);
            }

            var passwordHash = HashHelper.ComputeHash(password);
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var userResult = _context.WizardsUser.Where(u => u.UserName.Equals(username)).FirstOrDefault();
                var isMatch = userResult != null && userResult.PasswordHash.Equals(passwordHash);

                // User logged in
                if (isMatch)
                {
                    CreateUserSession(userResult);

                    TempData["Message"] = "";

                    if (!String.IsNullOrEmpty(actionName) && !String.IsNullOrEmpty(controllerName))
                    {
                        return RedirectToAction(actionName, controllerName);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }

            // User login failed
            TempData["Message"] = "Login failed. Please check username and password.";

            //Update login attempts in session if failed
            var loginAttempts = HttpContext.Session.GetInt32("loginAttempts");
            if (loginAttempts < LIMIT_LOGIN_ATTEMPT)
            {
                HttpContext.Session.SetInt32("loginAttempts", (int)(++loginAttempts));
            }
            else
            {
                HttpContext.Session.SetString("isBlock", DateTime.Now.ToString());
                TempData["Message"] = $"Login is temporarily blocked after {LIMIT_LOGIN_ATTEMPT} failed attempt(s). Please contact customer support.";
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

        // GET: User/Logout
        [HttpGet]
        public IActionResult NotLoggedIn()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CloseModal()
        {
            HttpContext.Session.Remove("modalTitle");
            HttpContext.Session.Remove("modalMessage");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string userName)
        {
            //_context.WizardsUser.Where(u => u.UserName.Equals(userName)).FirstOrDefault();
            var user = UserHelper.GetUser(userName, _context);
            if (user != null)
            {
                // Generate a random password
                //var randomPassword = GenerateRandomPassword();
                //user.PasswordHash = HashHelper.ComputeHash(randomPassword);
                //_context.Update(user);
                //await _context.SaveChangesAsync();

                var token = CreateToken(user.UserId, user.UserName);

                //Prepare email to send user
                string subject = "Reset Password";
                string link = $"{Environment.GetEnvironmentVariable("BASE_URL")}/Users/ChangePassword?token={token}";
                BodyBuilder bodyBuilder = PrepareResetEmailBody(link);

                //Create MailboxAddress for user
                MailboxAddress userMailboxAddress = new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email);

                //Send email
                EmailHelper.SendEmail(userMailboxAddress, subject, bodyBuilder);

            }
            TempData["Message"] = "The reset password email has been sent to your email address.";
            return RedirectToAction("login", "users");
        }

        [HttpGet]
        public IActionResult ChangePassword(string token = null)
        {
            if (token == null)
            {
                if (!UserHelper.IsLoggedIn(this)) return UserHelper.RequireLogin(this);

                ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            }
            else
            {
                int userId;
                string username;
                string strRequestedTime;
                try
                {
                    var payload = ReadToken(token);
                    userId = int.Parse(payload.Where(p => p.Key.Equals("userId")).First().Value.ToString());
                    username = payload.Where(p => p.Key.Equals("username")).First().Value.ToString();
                    strRequestedTime = payload.Where(p => p.Key.Equals("requestedTime")).First().Value.ToString();
                }
                catch (Exception ex)
                {
                    // random token that can break the ReadToken()
                    return NotFound();
                }
                
                if (!UserHelper.GetUser(username, _context).UserId.Equals(userId))
                {
                    return NotFound();
                }
                var totalTime = DateTime.UtcNow - DateTime.Parse(strRequestedTime);
                if (totalTime.TotalMinutes <= 60)
                {
                    ViewData["userId"] = userId;
                    ViewData["OldPasswordRequired"] = false;
                }
                else
                {
                    TempData["errorMessage"] = "The reset link is expired";
                    return RedirectToAction("error", "home");
                }
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string newPassword, string newPasswordConfirm, [Bind("UserId, PasswordHash")] WizardsUser users)
        {
            var userResult = _context.WizardsUser
                .Where(u => u.UserId.Equals(users.UserId))
                .FirstOrDefault();

            var username = users.UserName;
            var password = users.PasswordHash;
            var passwordHash = HashHelper.ComputeHash(password);
            var isMatch = userResult != null && userResult.PasswordHash.Equals(passwordHash);

            // Password correct
            if (isMatch)
            {
                if (newPassword == newPasswordConfirm)
                {
                    if (newPassword.Length < 8)
                    {
                        TempData["Message"] = "Password must be at least 8 characters.";
                    }
                    else if (!ValidationHelper.PasswordValidation(newPassword))
                    {
                        
                        TempData["Message"] = "Password must contain at least one number, one lowercase and one uppercase letter.";
                    } else
                    {
                        userResult.PasswordHash = HashHelper.ComputeHash(newPassword);
                        _context.Update(userResult);
                        await _context.SaveChangesAsync();

                        TempData["Message"] = "";
                        HttpContext.Session.SetString("modalTitle", "Password changed");
                        HttpContext.Session.SetString("modalMessage", "Your password has been changed successfully.");
                        return RedirectToAction(nameof(Menu));
                    }
                } else
                {
                    TempData["Message"] = "Confirm New Password not match";
                }
            } else
            {
                TempData["Message"] = "Current password is incorrect";
            }

            ViewData["UserId"] = HttpContext.Session.GetInt32("userId");
            return View();
        }
        private string GenerateRandomPassword()
        {
            StringBuilder generatedPassword = new StringBuilder();
            Random random = new Random();

            bool digit = true;
            bool lowercase = true;
            bool uppercase = true;

            while (generatedPassword.Length < WizardsUser.PASSWORD_MIN_LENGTH)
            {
                char c = (char)random.Next(49, 126);

                generatedPassword.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
            }

            if (digit)
                generatedPassword.Append((char)random.Next(48, 58));
            if (lowercase)
                generatedPassword.Append((char)random.Next(97, 123));
            if (uppercase)
                generatedPassword.Append((char)random.Next(65, 91));

            return generatedPassword.ToString();
        }

        private BodyBuilder PrepareResetEmailBody(string link)
        {
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<h1>Reset password</h1><br><p>You have requested a password reset. Please click the following link: <a href='{link}'>{link}<a></p>";
            bodyBuilder.TextBody = $"You have requested a password reset. Please go to this page and enter your new password: {link}";

            return bodyBuilder;
        }

        private void CreateUserSession(WizardsUser user)
        {
            var role = _context.UserRole.Where(us => us.UserId.Equals(user.UserId)).FirstOrDefault();
            var roleName = _context.WizardsRole.Where(r => r.RoleId.Equals(role.RoleId)).FirstOrDefault().RoleName;
            HttpContext.Session.SetInt32("userId", user.UserId);
            HttpContext.Session.SetString("userName", user.UserName);
            if (role != null && roleName != null)
            {
                HttpContext.Session.SetString("userRole", roleName);
            }
            HttpContext.Session.SetString("loggedInTime", DateTime.Now.ToString());
        }

        private string CreateToken(int userId, string username)
        {
            // Define const Key this should be private secret key  stored in some safe place
            string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

           // Create Security key  using private key above:
           // not that latest version of JWT using Microsoft namespace instead of System
            var securityKey = new Microsoft
               .IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            // Also note that securityKey length should be >256b
            // so you have to make sure that your private key has a proper length
            //
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials
                              (securityKey, SecurityAlgorithms.HmacSha256Signature);

            //  Finally create a Token
            var header = new JwtHeader(credentials);

            //Some PayLoad that contain information about the  customer
            var payload = new JwtPayload
           {
               { "userId", userId },
               { "username", username},
               { "requestedTime", DateTime.UtcNow},
           };

            //
            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            var tokenString = handler.WriteToken(secToken);

            return tokenString;
        }

        private JwtPayload ReadToken(string tokenString)
        {
            // Define const Key this should be private secret key  stored in some safe place
            string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

            // Create Security key  using private key above:
            // not that latest version of JWT using Microsoft namespace instead of System
            var securityKey = new Microsoft
               .IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            // Also note that securityKey length should be >256b
            // so you have to make sure that your private key has a proper length
            //
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials
                              (securityKey, SecurityAlgorithms.HmacSha256Signature);

            //
            var handler = new JwtSecurityTokenHandler();

            // And finally when  you received token from client
            // you can  either validate it or try to  read
            var token = handler.ReadJwtToken(tokenString);

            return token.Payload;
        }
    }
}
