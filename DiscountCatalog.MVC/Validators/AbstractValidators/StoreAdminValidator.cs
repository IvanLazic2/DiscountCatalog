using DiscountCatalog.MVC.REST.StoreAdmin;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Validators.AbstractValidators
{
    public class StoreAdminValidator : AbstractValidator<StoreAdminREST>
    {
        public StoreAdminValidator()
        {
            RuleFor(s => s.Identity)
                .NotNull();

            RuleFor(s => s.Identity.Id)
                .NotEmpty()
                .NotNull().When(s => s.Identity != null);

            RuleFor(s => s.Identity.UserName)
                .NotEmpty()
                .NotNull().When(s => s.Identity != null);
            
            RuleFor(s => s.Stores)
                .NotNull();
        }
    }
}