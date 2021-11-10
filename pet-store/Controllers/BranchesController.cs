using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pet_store.Data;
using pet_store.Models;
using Newtonsoft.Json;


namespace pet_store.Controllers
{
    public class BranchesController : Controller
    {
        private readonly PetStoreDBContext _context;

        public BranchesController(PetStoreDBContext context)
        {
            _context = context;
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            JsonConvert.SerializeObject(ViewBag.addresses);
            ViewData["addresses"] = GetFullAddress();
            return View(await _context.Branches.ToListAsync());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branches = await _context.Branches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branches == null)
            {
                return NotFound();
            }

            return View(branches);
        }

        // GET: Branches/Create
        [Authorize(Roles = "Admin,Supplier")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> Create([Bind("Id,Name,City,Street,Apartment")] Branches branches)
        {
            if (ModelState.IsValid)
            {
                _context.Add(branches);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(branches);
        }

        // GET: Branches/Edit/5
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branches = await _context.Branches.FindAsync(id);
            if (branches == null)
            {
                return NotFound();
            }
            return View(branches);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,City,Street,Apartment")] Branches branches)
        {
            if (id != branches.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branches);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchesExists(branches.Id))
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
            return View(branches);
        }

        // GET: Branches/Delete/5
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branches = await _context.Branches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branches == null)
            {
                return NotFound();
            }

            return View(branches);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branches = await _context.Branches.FindAsync(id);
            _context.Branches.Remove(branches);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchesExists(int id)
        {
            return _context.Branches.Any(e => e.Id == id);
        }

        private List<String> GetFullAddress()
        {
            var branch = _context.Branches;
            List<String> addresses = new List<string> { };
            if (branch != null)
            {
                foreach (var item in branch)
                {
                    var address = item.City + " " + item.Street + " " + item.Apartment;
                    addresses.Add(address);
                }
            }
            return addresses;
        }
    }
}
