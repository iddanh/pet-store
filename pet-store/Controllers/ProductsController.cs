using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pet_store.Data;
using pet_store.Models;
using pet_store.Services;

namespace pet_store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly PetStoreDBContext _context;
        private readonly ProductsService _service;
        private readonly SeedService _seed;

        public ProductsController(PetStoreDBContext context, ProductsService service, SeedService seed)
        {
            _context = context;
            _service = service;
            _seed = seed;
        }

        // GET: Products
        public IActionResult Checkout()
        {
            return View();

        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = new SelectList(_context.Category, "Name", "Name");
            ViewBag.Companies = new SelectList(_context.Product, nameof(Product.Company), nameof(Product.Company));
            var product = _context.Product.Include(p => p.Category).OrderBy(p => p.CategoryId);
            //await DeleteAll();
            //await _seed.GooProSearch();
            return View(await product.ToListAsync());
        }

        public IActionResult Sort(Comparer<Product> comparer, IEnumerable<Product> products)
        {
            return View(nameof(Index),products.OrderBy(p=>p, comparer));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GlobalSearch(string searchString, string productCategory, string productCompany)
        {
            ViewBag.Categories = new SelectList(_context.Category, "Name", "Name");
            ViewBag.Companies = new SelectList(_context.Product, nameof(Product.Company), nameof(Product.Company));
            var products = _service.Search(searchString, productCategory, productCompany).Include(x => x.Category);
            return View("Index", await products.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string searchString, string productCategory, string productCompany, double minPrice, double maxPrice)
        {
            var products = _service.Search(searchString, productCategory, productCompany, minPrice, maxPrice).Include(x => x.Category);
            return Json(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(p => p.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = nameof(UserType.Supplier))]
        // GET: Products/Create
        public IActionResult Create()
        {

            //var categories = _context.Category.FirstOrDefault();
            //TODO In the case of no categories

            /*if (categories == null)
            {
                return View("CategoryDoesNotexist");
            }*/
            ViewBag.Categories = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserType.Supplier))]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CategoryId,Price,Company,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Attach the current logged in user id to the new product being created
                product.Supplier = User.GetLoggedInUserId();


                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            if (!AllowedModifyProduct(product.Supplier))
            {
                return Forbid();
            }
            ViewBag.Categories = new SelectList(_context.Category, "Id", "Name");
            var suppliers = await _context.User.Where(u=>u.Type.Equals(UserType.Supplier)).ToListAsync();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "FullName");

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Supplier,Name,Description,CategoryId,Price,Company,Image")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (!AllowedModifyProduct(product.Supplier))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(p => p.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            if (!AllowedModifyProduct(product.Supplier))
            {
                return Forbid();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (!AllowedModifyProduct(product.Supplier))
            {
                return Forbid();
            }
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool AllowedModifyProduct(int productId)
        {
            return User.GetIsLoggedIn() && (productId == User.GetLoggedInUserId() || User.IsAdmin());
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserType.Admin))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {

            foreach (var product in _context.Product)
            {
                if (product.Id > 5)
                {
                    _context.Product.Remove(product);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Seed()
        {
            foreach (var product in _context.Product)
            {
                if (product.Name.Contains(product.Company))
                {
                    //product.Name = new StringBuilder().AppendJoin(' ',product.Name.Split().Except(product.Company.Split())).ToString();
                    //product.Company = new StringBuilder().AppendJoin(' ',product.Name.Split().Take(3)).ToString();
                    _context.Update(product);
                }
            }
            await _context.SaveChangesAsync();

            /*
            if (_context.Product.Count() < 4)
            {
                //await _seed.GooProSearch();
            }
            */
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetBreadCrumbs(int categoryId)
        {
            var breadCrumbs = _service.GetBreadCrumbs(await _context.Category.FindAsync(categoryId));
            return Json(breadCrumbs);
        }
    }

}
