using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace havhavli.Models
{
    public class Product {
        public int Id { get; set; }
        [Required(ErrorMessage = "אנא הכנס שם למוצר")]
        [StringLength(100, MinimumLength = 5)]
        [Display(Name = "מוצר")]
        public string Name { get; set; }
        [Display(Name = "תיאור המוצר")]
        public string Description { get; set; }
        [Display(Name = "כמות")]
        [Range(0, 40)]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "אנא קבע מחיר למוצר")]
        [Display(Name = "מחיר")]
        [Range(5, 250)]
        [DataType(DataType.Currency)]
        public float Price { get; set; }
        [Display(Name = "תמונה")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }
        [Display(Name = "קטגוריה")]
        public int categoryId { get; set; }
        public category category { get; set; }
        public List<ShoppingCart> Carts { get; set; }
        [Display(Name = "שם הספק")]
        public int SupplierID { get; set; }
        public Supplier supplier { get; set; }
        public int QuantityInCart { get; set; } = 0;
    }
}
