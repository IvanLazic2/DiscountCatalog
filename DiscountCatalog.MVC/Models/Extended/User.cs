﻿//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DiscountCatalog.MVC.Models
//{
//    [MetadataType(typeof(UserMetadata))]
//    public partial class User
//    {
//        public string ConfirmPassword { get; set; }
//    }

//    public class UserMetadata
//    {
//        ////[Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
//        //[Display(Name = "First name")]
//        //public string FirstName { get; set; }

//        ////[Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
//        //[Display(Name = "Last name")]
//        //public string LastName { get; set; }



//        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
//        [Display(Name = "Email")]
//        [DataType(DataType.EmailAddress)]
//        public string Email { get; set; }

//        ////[Required]
//        //[Display(Name = "Phone number")]
//        //[DataType(DataType.PhoneNumber)]
//        //public string PhoneNumber { get; set; }

//        [Required]
//        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
//        [DataType(DataType.Password)]
//        [Display(Name = "Password")]
//        public string Password { get; set; }

//        [DataType(DataType.Password)]
//        [Display(Name = "Confirm password")]
//        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
//        public string ConfirmPassword { get; set; }
//    }
//}
