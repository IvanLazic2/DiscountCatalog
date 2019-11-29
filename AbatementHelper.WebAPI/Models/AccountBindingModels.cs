 using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AbatementHelper.WebAPI.Models
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        //User, Admin and StoreAdmin section
        //[Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[Required(AllowEmptyStrings = true)]
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

        //Store Section
        public string WorkingHoursWeek { get; set; }
        public string WorkingHoursWeekends { get; set; }
        public string WorkingHoursHolidays { get; set; }

        public string MasterStoreId { get; set; }
    }

    //public class StoreBindingModel
    //{
    //    //[Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
    //    [Display(Name = "First name")]
    //    public string FirstName { get; set; }

    //    //[Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
    //    [Display(Name = "Last name")]
    //    public string LastName { get; set; }

    //    [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
    //    [Display(Name = "Username")]
    //    public string UserName { get; set; }

    //    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    //    [Display(Name = "Email")]
    //    [DataType(DataType.EmailAddress)]
    //    public string Email { get; set; }

    //    //[Required(AllowEmptyStrings = true)]
    //    [Display(Name = "Phone number")]
    //    [DataType(DataType.PhoneNumber)]
    //    public string PhoneNumber { get; set; }

    //    //address section
    //    public string Country { get; set; }
    //    public string City { get; set; }
    //    public string PostalCode { get; set; }
    //    public string Street { get; set; }



    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm password")]
    //    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }

    //    [Required]
    //    public string Role { get; set; }

    //    [Required]
    //    public bool Approved { get; set; }



    //    [Required]
    //    public bool Deleted { get; set; }
    //}

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
