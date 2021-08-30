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
                List<ShoppingCart> havhavliDBContext = _context.ShoppingCart.Include(c => c.User).Include(a => a.Products).ToList();

                foreach (ShoppingCart cart in havhavliDBContext)
                {
                    cart.User = _context.User.FirstOrDefault(u => u.Id == cart.UserId);
                }
                return View(havhavliDBContext);
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
        public async Task<IActionResult> AddToCart(int id,int quantity)
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
                product.QuantityInCart = quantity;
                user.Cart.Products.Add(product);
                product.Carts.Add(cart);
                user.Cart.TotalPrice +=product.Price* product.QuantityInCart;
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
                cart.TotalPrice -= product.Price* product.QuantityInCart;
                cart.Products.Remove(product);//Remove by amount
            }

            _context.Attach<ShoppingCart>(cart);
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyCart));
        }


        [HttpPost, ActionName("EditQuantity")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditQuantity(int id, int quantity)
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

            if (product.QuantityInCart == quantity)
            {
                return RedirectToAction(nameof(MyCart));
            }
            else
            {
                cart.TotalPrice -= (product.Price * product.QuantityInCart);
                cart.TotalPrice += (product.Price * quantity);
                product.QuantityInCart = quantity;
                _context.Update(cart);
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
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
