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
    public class StoreCookieHandler : ICookieHandler<StoreCookie>
    {
        public StoreCookie Get(HttpContext context)
        {
            try
            {
                HttpCookieCollection cookies = context.Request.Cookies;

                return new StoreCookie
                    (
                        cookies["StoreID"].Value,
                        cookies["StoreName"].Value
                    );
            }
            catch (Exception)
            {
                return new StoreCookie();
            }
        }

        public bool IsValid(StoreCookie cookie)
        {
            StoreCookieValidator validator = new StoreCookieValidator();

            ValidationResult result = validator.Validate(cookie);

            return result.IsValid;
        }
    }
}