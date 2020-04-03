using DiscountCatalog.MVC.REST.Store;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Validators.AbstractValidators
{
    public class StoreValidator : AbstractValidator<StoreREST>
    {
        public StoreValidator()
        {
            RuleFor(s => s.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(s => s.StoreName)
                .NotEmpty()
                .NotNull();
            
        }
    }
}