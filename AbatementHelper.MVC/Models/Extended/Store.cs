//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;

//namespace AbatementHelper.MVC.Models
//{
//    [MetadataType(typeof(StoreMetadata))]
//    public partial class Store
//    {
//        public string ConfirmPassword { get; set; }
//    }

//    public class StoreMetadata
//    {
//        [Required(AllowEmptyStrings = false, ErrorMessage = "Store name is required")]
//        [Display(Name = "Store Name")]
//        public string UserName { get; set; }

//        [Display(Name = "Phone number")]
//        [DataType(DataType.PhoneNumber)]
//        public string PhoneNumber { get; set; }

//        [Required(AllowEmptyStrings = false)]
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