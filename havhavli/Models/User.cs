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
        [Required(ErrorMessage = "חובה להכניס שם משתמש")]
        [Display(Name = "שם משתמש")]
        public string Username { get; set; }

        [Required(ErrorMessage ="חובה להכניס סיסמא")]
        [Display(Name ="סיסמא")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
      //  [Required(ErrorMessage ="אנא הכנס כתובת מייל")]
        [Display(Name = "אימייל")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
       // [Required(ErrorMessage ="אנא הכנס תאריך לידה")]
        [Display(Name = "תאריך לידה")]
        [DataType(DataType.Date)]
        public string BirthDay { get; set; }

        public UserType Type { get; set; } = UserType.Client;
    }
}
