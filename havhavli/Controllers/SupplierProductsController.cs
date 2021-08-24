using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using havhavli.Data;
using havhavli.Models;

namespace havhavli.Controllers
{
    public class SupplierProductsController : Controller
    {
        private readonly havhavliContext _context;

        public SupplierProductsController(havhavliContext context)
        {
            _context = context;
        }

        // GET: SupplierProducts
        public async Task<IActionResult> Index()
        {
            var havhavliContext = _context.SupplierProducts.Include(s => s.product).Include(s => s.supplier);
            return View(await havhavliContext.ToListAsync());
        }

        // GET: SupplierProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierProducts = await _context.SupplierProducts
                .Include(s => s.product)
                .Include(s => s.supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplierProducts == null)
            {
                return NotFound();
            }

            return View(supplierProducts);
        }

        // GET: SupplierProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "Id", "ContactName");
            return View();
        }

        // POST: SupplierProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,InStock,SupplierId")] SupplierProducts supplierProducts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplierProducts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", supplierProducts.ProductId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "Id", "ContactName", supplierProducts.SupplierId);
            return View(supplierProducts);
        }

        // GET: SupplierProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierProducts = await _context.SupplierProducts.FindAsync(id);
            if (supplierProducts == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", supplierProducts.ProductId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "Id", "ContactName", supplierProducts.SupplierId);
            return View(supplierProducts);
        }

        // POST: SupplierProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,InStock,SupplierId")] SupplierProducts supplierProducts)
        {
            if (id != supplierProducts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplierProducts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierProductsExists(supplierProducts.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", supplierProducts.ProductId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "Id", "ContactName", supplierProducts.SupplierId);
            return View(supplierProducts);
        }

        // GET: SupplierProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierProducts = await _context.SupplierProducts
                .Include(s => s.product)
                .Include(s => s.supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplierProducts == null)
            {
                return NotFound();
            }

            return View(supplierProducts);
        }

        // POST: SupplierProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplierProducts = await _context.SupplierProducts.FindAsync(id);
            _context.SupplierProducts.Remove(supplierProducts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierProductsExists(int id)
        {
            return _context.SupplierProducts.Any(e => e.Id == id);
        }
    }
}
