using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using havhavli.Models;

namespace havhavli.Data
{
    public class havhavliContext : DbContext
    {
        public havhavliContext (DbContextOptions<havhavliContext> options)
            : base(options)
        {
        }

        public DbSet<havhavli.Models.Product> Product { get; set; }

        public DbSet<havhavli.Models.User> User { get; set; }

        public DbSet<havhavli.Models.category> category { get; set; }

        public DbSet<havhavli.Models.Supplier> Supplier { get; set; }

        public DbSet<havhavli.Models.Branch> Branch { get; set; }

        public DbSet<havhavli.Models.SupplierProducts> SupplierProducts { get; set; }

        public DbSet<havhavli.Models.ShoppingCart> ShoppingCart { get; set; }

    }
}
