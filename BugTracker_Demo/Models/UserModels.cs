using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker_Demo.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
    }

    public class LoginModel
    {
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "You must enter your username.")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "You must enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public int UserId { get; set; }

        [Display(Name = "FullName")]
        [Required(ErrorMessage = "You must enter your full name.")]
        public string FullName { get; set; }

        [Display(Name = "Desired Username")]
        [Required(ErrorMessage = "A username is required.")]
        public string DesiredUserName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Your email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Desired Password")]
        [Required(ErrorMessage = "A password is required")]
        [DataType(DataType.Password)]
        public string DesiredPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "You must confirm your password.")]
        [Compare(nameof(DesiredPassword), ErrorMessage = "Your password and confirm password must match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
