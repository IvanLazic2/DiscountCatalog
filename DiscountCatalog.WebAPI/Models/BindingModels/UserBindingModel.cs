using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Models.BindingModels
{
    public class UserBindingModel
    {
        //public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        //public bool PhoneNumberConfirmed { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Role { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }

        public byte[] UserImage { get; set; }
    }
}