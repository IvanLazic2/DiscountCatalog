using DiscountCatalog.MVC.Models.Cookies;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Cookies.CookieValidators
{
    public class StoreCookieValidator : AbstractValidator<StoreCookie>
    {
        public StoreCookieValidator()
        {
            RuleFor(s => s.StoreID)
                .NotEmpty()
                .NotNull();

            RuleFor(s => s.StoreName)
                .NotEmpty()
                .NotNull();
        }
    }
}