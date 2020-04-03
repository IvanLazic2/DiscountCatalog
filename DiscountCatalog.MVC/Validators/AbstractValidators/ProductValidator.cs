using DiscountCatalog.MVC.REST.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Validators.AbstractValidators
{
    public class ProductValidator : AbstractValidator<ProductREST>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.ProductName)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.OldPrice)
                .NotNull();

            RuleFor(p => p.NewPrice)
                .NotNull();

            RuleFor(p => p.DiscountPercentage)
                .NotNull();

            RuleFor(p => p.DiscountDateBegin)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.DiscountDateEnd)
                .NotEmpty()
                .NotNull();
        }
    }
}