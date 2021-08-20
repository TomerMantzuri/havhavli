using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace havhavli.Models
{
    public class Product { 
        public int Id { get; set; }
        [Required]
        [StringLength(100,MinimumLength =5)]
        [Display(Name = "מוצר")]
        public string Name { get; set; }
        [Display(Name = "תיאור המוצר")]
        public string Description { get; set; }
        [Display(Name = "כמות")]
        [Range(0,40)]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "מחיר")]
        [Range(5,250)]
        [DataType(DataType.Currency)]
        public float Price { get; set; }
        [Display(Name = "תמונה")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        public Category CategoryId { get; set; }
    }
}
