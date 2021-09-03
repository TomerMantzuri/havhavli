﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using havhavli.Data;
using havhavli.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;

namespace havhavli.Controllers
{
    public class ProductsController : Controller
    {
        private readonly havhavliContext _context;

        public ProductsController(havhavliContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var havhavliContext = _context.Product.Include(p => p.category).Include(p => p.supplier).Include(p=>p.productImage);
            return View(await havhavliContext.ToListAsync());
        }

        public async Task<IActionResult> Search(string query)
        {
            var havhavliContext = _context.Product.Include(a => a.category).Include(p => p.supplier).Include(p => p.productImage).Where(a => a.Name.Contains(query) || a.Description.Contains(query)||query==null || a.category.name.Contains(query));
            return View("index",await havhavliContext.ToListAsync());
        }

        public async Task<IActionResult> Category(int id)
        {
            var havhavliContext = _context.Product.Include(a => a.category).Include(p => p.productImage).Where(a => a.categoryId == id);
            return View("index", await havhavliContext.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SupplierProducts(int id)
        {
            var havhavliContext = _context.Product.Include(a => a.supplier).Include(p => p.productImage).Where(a => a.SupplierID == id);
            return View( await havhavliContext.ToListAsync());
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["categoryId"] = new SelectList(_context.Set<category>(), "Id", "name");
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Quantity,Price,categoryId,SupplierID")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "ProductImages");

            }
            ViewData["categoryId"] = new SelectList(_context.Set<category>(), "Id", "Id", product.categoryId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "Id", "Name", product.SupplierID);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
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
            ViewData["categoryId"] = new SelectList(_context.Set<category>(), "Id", "name", product.categoryId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "Id", "Name", product.SupplierID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Quantity,Price,categoryId,SupplierID,QuantityInCart")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
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
            ViewData["categoryId"] = new SelectList(_context.Set<category>(), "Id", "Id", product.categoryId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "Id", "Name", product.SupplierID);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.category)
                .Include(p => p.supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Statistics()
        {
                ICollection<Statistic> ClientStatistic = new Collection<Statistic>();
                var ClientData = from p in _context.Product.Include(o => o.Carts)
                             where (p.Carts.Count) > 0
                             orderby (p.Carts.Count) descending
                             select p;
                foreach (var item in ClientData)
                {
                ClientStatistic.Add(new Statistic(item.Name, item.Carts.Count()));
                }
                ViewBag.ClientPurchase = ClientStatistic;


                ICollection<Statistic> ProductStatistics = new Collection<Statistic>();
                List<Product> products = _context.Product.ToList();
                List<category> categories = _context.category.ToList();
                var ProductData = from product in products
                              join category in categories on product.categoryId equals category.Id
                              group category by category.Id into G
                              select new { id = G.Key, num = G.Count() };

                var CalcStatic = from producct in ProductData
                             join cate in categories on producct.id equals cate.Id
                             select new { category = cate.name, count = producct.num };
                foreach (var item in CalcStatic)
                {
                    if (item.count > 0)
                    ProductStatistics.Add(new Statistic(item.category, item.count));
                }
                ViewBag.CurrentTotalProducts = ProductStatistics;
                return View();
        }


        public async Task<IActionResult> JoinSearch(string productName, string price)
        {
            ViewData["Categoriess"] = new SelectList(_context.category, nameof(category.Id), nameof(category.name));
            float FloatPrice = Int32.Parse(price);
            var products = from pro in _context.Product select pro;
            products = products.Where(x => x.Name.Contains(productName)|| x.Description.Contains(productName));
            products = products.Where(x => x.Price <= FloatPrice);

             var query = from pro in products join image in _context.ProductImage on pro.Id equals image.ProductId select new ProductJoin(pro, image);

             return View("ProductsSearch", await query.ToListAsync());
        }
    }
}

public class Statistic
{
    public string Key;
    public int Values;
    public Statistic(string key, int values)
    {
        Key = key;
        Values = values;
    }
}

public class ProductJoin
{
    public Product product{ get; set; }
    public ProductImage productimage { get; set; }

    public ProductJoin(Product pro, ProductImage Image) { this.product = pro; this.productimage = Image; }
}