using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace havhavli.Models
{
    public class Branch
    {
        public int Id { get; set; }
        [Display(Name = "שם")]
        [RegularExpression(@"^[א-ת ]+$", ErrorMessage = "שם הסניף חייב להיות בעברית בלבד ")]
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "מספר טלפון")]
        [DataType(DataType.PhoneNumber)]
        public String PhoneNumber { get; set; }
        [Required]
        [Display(Name = "כתובת")]
        public String Address { get; set; }
        [Required]
        [Display(Name = "שעות פעילות")]
        public String OpeningHours { get; set; }
        [Display(Name = "ספקים")]
        public List<Supplier> Suppliers { get; set; }
    }
}
