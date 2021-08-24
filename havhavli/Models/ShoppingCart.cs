using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace havhavli.Models
{
    public class ShoppingCart
   {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public float TotalPrice { get; set; } = 0;
        public List<Product> Products { get; set; }
        internal static object Index()
        {
            throw new NotImplementedException();
        }
    }
}
