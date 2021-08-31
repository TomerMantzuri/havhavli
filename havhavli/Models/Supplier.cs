using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace havhavli.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="אנא הכנס את שם הספק")]
        [RegularExpression(@"^[א-ת ]+$", ErrorMessage = "שם הספק חייב להיות בעברית בלבד")]
        [Display(Name = "שם")]
        public string Name { get; set; }
        [Required(ErrorMessage = "אנא הכנס את שם הסוכן")]
        [RegularExpression(@"^[א-ת ]+$", ErrorMessage = "שם סוכן המכירות חייב להיות בעברית בלבד")]
        [Display(Name = "שם סוכן המכירות")]
        public string ContactName { get; set; }
        [Required(ErrorMessage ="אנא הכנס מספר טלפון")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="אנא הכנס מספר טלפון תקין")]
        [Display(Name = "מספר טלפון")]
        public String Phone { get; set; }
        public List<Branch> Branchs { get; set; }
        public List<Product> Products { get; set; }


    }
}
