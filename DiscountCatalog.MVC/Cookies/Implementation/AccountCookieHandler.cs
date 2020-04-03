using DiscountCatalog.MVC.Cookies.Contractor;
using DiscountCatalog.MVC.Cookies.CookieValidators;
using DiscountCatalog.MVC.Models.Cookies;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Cookies.Implementation
{
    public class AccountCookieHandler : ICookieHandler<AccountCookie>
    {
        public AccountCookie Get(HttpContext context)
        {
            try
            {
                HttpCookieCollection cookies = context.Request.Cookies;

                return new AccountCookie
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
                return new AccountCookie();
            }
            
        }

        public bool IsValid(AccountCookie cookie)
        {
            AccountCookieValidator validator = new AccountCookieValidator();

            ValidationResult result = validator.Validate(cookie);

            return result.IsValid;
        }
    }
}