﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Cookies.CookieValidators
{
    public class UserCookieValidator : AbstractValidator<UserCookie>
    {
        public UserCookieValidator()
        {
            RuleFor(c => c.Access_Token)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.UserName)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.Email)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.Role)
                .NotEmpty()
                .NotNull();
        }
    }
}