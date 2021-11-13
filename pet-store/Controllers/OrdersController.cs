using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pet_store.Data;
using pet_store.Models;
using pet_store.Services;

namespace pet_store.Controllers
{
    public class OrdersController : Controller
    {
        private readonly PetStoreDBContext _context;

        public OrdersController(PetStoreDBContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize(Roles = nameof(UserType.Customer))]
        public async Task<IActionResult> Index()
        {
            ViewBag.Users = new SelectList(_context.User, "Id", "FullName");
            List<Order> orders;
            if (User.IsAdmin())
            {
                orders = await _context.Order.ToListAsync();
            }
            else
            {
                orders = await _context.Order.Where(order => order.User == User.GetLoggedInUserId()).ToListAsync();
            }
            return View(orders);
        }

        //POST: Orders/Search
        public async Task<IActionResult> OrderSearch(int? fromPrice, int? untilPrice, int[] usersIds, int[] productsIds, string city, DateTime? from, DateTime? until)
        {
            return View();
        }
        public async Task<IActionResult> ClearCart()
        {
            return View();
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var order = await _context.Order.Include(c => c.Products).FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            if (!AllowedModifyOrder(order.User))
            {
                return Forbid();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = nameof(UserType.Customer))]
        public IActionResult Create()
        {
            ViewBag.now = DateTime.Today;
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,City,Street,Apartment,OrderNotes")] Order order, String productIds)
        {
            if (ModelState.IsValid)
            {
                if (productIds != null)
                {
                    order.OrderDate = DateTime.Now;
                    order.User = User.GetLoggedInUserId();

                    List<int> productIdsParsed = productIds.Split(',').Select(id => int.Parse(id)).ToList();
                    List<Product> products = new List<Product>();
                    productIdsParsed.ForEach(id => products.Add(_context.Product.Find(id)));
                    order.Products = products;

                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ClearCart));
                } else
                {
                    ViewData["Error"] = "Cart is empty. Go add some products!";
                }
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            if (!AllowedModifyOrder(order.User))
            {
                return Forbid();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,City,Street,Apartment,OrderDate,OrderNotes")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (!AllowedModifyOrder(order.User))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            if (!AllowedModifyOrder(order.User))
            {
                return Forbid();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (!AllowedModifyOrder(order.User))
            {
                return Forbid();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }

        private bool AllowedModifyOrder(int userId)
        {
            return User.GetIsLoggedIn() && (userId == User.GetLoggedInUserId() || User.IsAdmin());
        }
    }
}
