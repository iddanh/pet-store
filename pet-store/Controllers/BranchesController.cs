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
using pet_store.Services;

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
            return View(await _context.Branch.ToListAsync());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branches = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branches == null)
            {
                return NotFound();
            }

            return View(branches);
        }

        // GET: Branches/Create
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            ViewBag.UserIds = new SelectList(_context.User.Where(u => u.Type.Equals(UserType.Manager) && u.Branch == null), "Id", "Name");
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Id,Name,City,Street,Apartment,UserId")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                if (!User.IsInRole(nameof(UserType.Admin)))
                {
                    branch.UserId = User.GetLoggedInUserId();
                }
                _context.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        // GET: Branches/Edit/5
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,City,Street,Apartment")] Branch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchesExists(branch.Id))
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
            return View(branch);
        }

        // GET: Branches/Delete/5
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branch.FindAsync(id);
            _context.Branch.Remove(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchesExists(int id)
        {
            return _context.Branch.Any(e => e.Id == id);
        }

        private List<String> GetFullAddress()
        {
            var branch = _context.Branch;
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
