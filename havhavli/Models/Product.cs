﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace havhavli.Models
{
    public class Product {
        public int Id { get; set; }
        [Required(ErrorMessage = "אנא הכנס שם למוצר")]
        [RegularExpression(@"^[א-ת ]+$", ErrorMessage = "שם המוצר חייב להיות בעברית בלבד")]
        [StringLength(100, MinimumLength = 3,ErrorMessage ="על שם המוצר להיות גדול יותר מ-3 תווים")]
        [Display(Name = "מוצר")]
        public string Name { get; set; }
        [Display(Name = "תיאור המוצר")]
        public string Description { get; set; }
        [Display(Name = "כמות")]
        [RegularExpression(@"^[0-9\s]*$", ErrorMessage = " כמות מספרים בלבד")]
        [Range(0, 100,ErrorMessage ="ניתן להכניס עד 100 מוצרים למלאי")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "אנא קבע מחיר למוצר")]
        [Display(Name = "מחיר")]
        [Range(5, 800,ErrorMessage ="על המחיר להיות 5-800 שח")]
        [DataType(DataType.Currency,ErrorMessage ="אנא השתמש בספרות בלבד")]
        public float Price { get; set; }
        [Required]
        [Display(Name = "קטגוריה")]
        public int categoryId { get; set; }
        public category category { get; set; }
        public List<ShoppingCart> Carts { get; set; }
        [Display(Name = "שם הספק")]
        [Required]
        public int SupplierID { get; set; }
        public Supplier supplier { get; set; }
        public int QuantityInCart { get; set; }
        [Display(Name = "תמונה")]
        public ProductImage productImage { get; set; }
    }
}
