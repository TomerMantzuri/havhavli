using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace havhavli.Models
{
    public enum UserType
    {
        Client,
        Admin
    }

    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "שם משתמש")]
        public string Username { get; set; }

        [Required]
        //[Range(8,12,ErrorMessage ="אנא הכנס סיסמא בעלת 8-12 תווים")]
        [Display(Name ="סיסמא")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       // [Required]
        [Display(Name = "אימייל")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
      //  [Required]
        [Display(Name = "תאריך לידה")]
        [DataType(DataType.Date)]
        public string BirthDay { get; set; }

        public UserType Type { get; set; } = UserType.Client;
    }
}
