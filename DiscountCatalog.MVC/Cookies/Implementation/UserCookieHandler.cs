using DiscountCatalog.MVC.Cookies.Contractor;
using DiscountCatalog.MVC.Cookies.CookieValidators;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Cookies.Implementation
{
    public class UserCookieHandler : ICookieHandler<UserCookie>
    {
        public UserCookie Get(HttpContext context)
        {
            try
            {
                HttpCookieCollection cookies = context.Request.Cookies;

                return new UserCookie
                    (
                        cookies["UserID"].Value,
                        cookies["Access_Token"].Value,
                        cookies["UserName"].Value,
                        cookies["Email"].Value,
                        cookies["Role"].Value
                    );
            }
            catch (Exception)
            {
                return new UserCookie();
            }
            
        }

        public bool IsValid(UserCookie cookie)
        {
            UserCookieValidator validator = new UserCookieValidator();

            ValidationResult result = validator.Validate(cookie);

            return result.IsValid;
        }
    }
}