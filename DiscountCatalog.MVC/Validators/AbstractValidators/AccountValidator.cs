using DiscountCatalog.MVC.REST.Account;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Validators.AbstractValidators
{
    public class AccountValidator : AbstractValidator<AccountREST>
    {
        public AccountValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(a => a.UserName)
                .NotEmpty()
                .NotNull();

            RuleFor(a => a.Email)
                .NotEmpty()
                .NotNull();
        }
    }
}