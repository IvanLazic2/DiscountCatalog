using AbatementHelper.CommonModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbatementHelper.MVC.Models
{
    public class AuthenticationModelMVC
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email or UserName is required")]
        [Display(Name = "Email or UserName")]
        public string EmailOrUserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}