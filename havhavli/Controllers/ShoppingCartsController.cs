using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using havhavli.Data;
using havhavli.Models;
using Microsoft.AspNetCore.Authorization;

namespace havhavli.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly havhavliContext _context;

        public ShoppingCartsController(havhavliContext context)
        {
            _context = context;
        }

        // GET: ShoppingCarts
        // GET: Carts
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                List<ShoppingCart> havhavliDBContext = _context.ShoppingCart.Include(c => c.User).Include(a => a.Products).ToList();

                foreach (ShoppingCart cart in havhavliDBContext)
                {
                    cart.User = _context.User.FirstOrDefault(u => u.Id == cart.UserId);
                }
                return View(havhavliDBContext);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        [Authorize]
        public IActionResult Search(string query)
        {
            String userName = User.Identity.Name;
            User user = _context.User.FirstOrDefault(x => x.Username == userName);
            ShoppingCart cart = _context.ShoppingCart.Include(db => db.Products).FirstOrDefault(x => x.UserId == user.Id);
            if (query == null)
                return View("MyCart", cart);
            List<Product> products = cart.Products.Where(a => a.Name.Contains(query) || a.Description.Contains(query)).ToList();
            cart.Products = products;
            return View("MyCart", cart);
        }

                // GET: ShoppingCarts/Edit/5
                [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            var shoppingCart = await _context.ShoppingCart.FindAsync(id);
            if (shoppingCart == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Password", shoppingCart.UserId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TotalPrice")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
                    {
                        return RedirectToAction("PageNotFound", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyCart));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Password", shoppingCart.UserId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            var shoppingCart = await _context.ShoppingCart
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.ShoppingCart
               .Include(c => c.User)
               .Include(p => p.Products)
               .FirstOrDefaultAsync(m => m.Id == id);
            cart.Products.Clear();
            cart.TotalPrice = 0;
            _context.Update(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCart.Any(e => e.Id == id);
        }
       [Authorize]
        public IActionResult MyCart()
        {
            String userName = HttpContext.User.Identity.Name;
            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            ShoppingCart cart = _context.ShoppingCart.FirstOrDefault(x => x.UserId == user.Id);
            cart.Products = _context.Product.Where(x => x.Carts.Contains(cart)).ToList();
            if (cart == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            return View(cart);
        }

        [HttpPost, ActionName("AddToCart")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(int id)
        {
            Product product = _context.Product.Include(db => db.Carts).FirstOrDefault(x => x.Id == id);
            String userName = HttpContext.User.Identity.Name;
            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            ShoppingCart cart = _context.ShoppingCart.Include(db => db.Products)
             .FirstOrDefault(x => x.UserId == user.Id);


            if (user.Cart.Products == null)
                user.Cart.Products = new List<Product>();
            if (product.Carts == null)
                product.Carts = new List<ShoppingCart>();

            if (!(cart.Products.Contains(product) && product.Carts.Contains(cart)))
            {
                user.Cart.Products.Add(product);
                product.Carts.Add(cart);
                user.Cart.TotalPrice += product.Price;
                _context.Update(cart);
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Products");
        }

        // POST: Carts/removeProduct/5
        [HttpPost, ActionName("RemoveProduct")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            Product product = _context.Product.FirstOrDefault(x => x.Id == id);
            String userName = HttpContext.User.Identity.Name;

            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            ShoppingCart cart = _context.ShoppingCart.Include(db => db.Products)
                .FirstOrDefault(x => x.UserId == user.Id);
            if (product != null)
            {
                cart.Products.Remove(product);
                cart.TotalPrice -= product.Price;
            }

            _context.Attach<ShoppingCart>(cart);
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyCart));
        }

        [Authorize]
        public async Task<IActionResult> AfterPayment()
        {
            String userName = HttpContext.User.Identity.Name;
            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            if (user == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            ShoppingCart cart = _context.ShoppingCart.Include(db => db.Products).FirstOrDefault(x => x.UserId == user.Id);
            foreach(var item in cart.Products)
            {
                item.Quantity--;
                _context.Update(item);
            }
            int i = cart.Products.RemoveAll(p => p.Id == p.Id);
            cart.TotalPrice = 0;

            _context.Attach<ShoppingCart>(cart);
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
