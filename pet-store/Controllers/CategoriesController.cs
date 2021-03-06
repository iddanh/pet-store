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


namespace pet_store.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly PetStoreDBContext _context;

        public CategoriesController(PetStoreDBContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.Include(c => c.Products).Include(c => c.Parent).FirstOrDefaultAsync(m => m.Id == id);
            var companies = from product in _context.Product
                            select product.Company;

            ViewBag.Companies = companies.Distinct();
            if (category == null)
            {
                return NotFound();
            }

            IEnumerable<Product> newList = new List<Product>();
            foreach (var cat in _context.Category.Include(c => c.Products).Include(c => c.Parent))
            {
                var originalList = cat.Products.AsEnumerable();
                if (cat.ParentId == id)
                {
                    newList = newList.Concat(originalList);
                }
            }
            if (newList.Any())
            {
                return View(new Category
                {
                    Id = category.Id,
                    Image = category.Image,
                    Name = category.Name,
                    Parent = category.Parent,
                    Products = newList.ToList(),
                    ParentId = category.ParentId
                });
            }
            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = nameof(UserType.Admin))]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Create([Bind("Id,Name,ParentId,Image")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
