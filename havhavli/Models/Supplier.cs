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
        [Display(Name = "שם")]
        public string Name { get; set; }
        [Required(ErrorMessage = "אנא הכנס את שם הסוכן")]
        [Display(Name = "שם סוכן המכירות")]
        public string ContactName { get; set; }
        [Required(ErrorMessage ="אנא הכנס מספר טלפון")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "מספר טלפון")]
        public String Phone { get; set; }
        public List<Branch> Branchs { get; set; }

    }
}
