using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace havhavli.Models
{
    public class category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="בחר שם לקטגוריה")]
        [Display(Name ="שם הקטגוריה")]
        [RegularExpression(@"^[א-ת ]+$", ErrorMessage = "שם הקטגוריה חייב להיות אותיות בעברית בלבד")]
        public string name { get; set; }
        [Display(Name = "מוצרים")]
        public List<Product> Products { get; set; }

    }
}
