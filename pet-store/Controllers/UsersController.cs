using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pet_store.Data;
using pet_store.Models;
using pet_store.Services;

namespace pet_store.Controllers
{
    public class UsersController : Controller
    {
        private readonly PetStoreDBContext _context;

        public UsersController(PetStoreDBContext context)
        {
            _context = context;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = nameof(UserType.Admin))]
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (!AllowedModifyUser(user.Id))
            {
                return Forbid();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FullName,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var existing = await _context.User.FirstOrDefaultAsync(m => m.Email == user.Email);

                if (existing != null)
                {
                    ViewData["Error"] = "Email already in use.";
                    return View();
                }

                user.RegisterTime = DateTime.Now;
                _context.Add(user);
                await _context.SaveChangesAsync();

                await LoginUser(user);
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }
            return View(user);
        }

        // GET: Users/Create
        public IActionResult RegisterSupplier()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSupplier(int id, [Bind("CompanyName,CompanyId")] User supplier)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            user.Type = UserType.Supplier;
            user.CompanyName = supplier.CompanyName;
            user.CompanyId = supplier.CompanyId;
            _context.Update(user);

            await _context.SaveChangesAsync();
            await LoginUser(user);
            return RedirectToAction(nameof(Details), new { id = id });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(ClearCart));
        }

        public async Task<IActionResult> ClearCart()
        {
            return View();
        }

        private async Task LoginUser(User user)
        {
            // HttpContext.Session.SetString("username", username);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Type.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        // GET: Users/Create
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,Password")] User user, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var q = from u in _context.User
                        where u.Email == user.Email &&
                                u.Password == user.Password
                        select u;

                if (q.Count() > 0)
                {
                    LoginUser(q.First());
                    if (ReturnUrl != null)
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    ViewData["Error"] = "Username/password is incorrect.";
                }
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!AllowedModifyUser(user.Id))
            {
                return Forbid();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (!AllowedModifyUser(user.Id))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var oldUser = await _context.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                user.Type = oldUser.Type;

                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [Authorize(Roles = nameof(UserType.Admin))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool AllowedModifyUser(int id)
        {
            return User.GetIsLoggedIn() && (id == User.GetLoggedInUserId() || User.IsAdmin());
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
