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
        [Display(Name = "אימייל")]
        [DataType(DataType.EmailAddress,ErrorMessage ="אנא הכנס כתובת מייל תיקנית")]
        public string EmailAddress { get; set; }
        [Display(Name = "תאריך לידה")]
        [DataType(DataType.Date,ErrorMessage ="אנא הכנס תאריך לידה תקין")]
        public string BirthDay { get; set; } = "01 / 01 / 2001";
        public UserType Type { get; set; } = UserType.Client;
        public ShoppingCart Cart { get; set; } = new ShoppingCart();
    }
}
