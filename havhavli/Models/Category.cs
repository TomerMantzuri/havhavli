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
        public string name { get; set; }
        public List<Product> Products { get; set; }

    }
}
