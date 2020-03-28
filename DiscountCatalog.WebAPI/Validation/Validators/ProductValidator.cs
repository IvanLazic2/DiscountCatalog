using DiscountCatalog.WebAPI.Models.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Validation.Validators
{
    public class ProductValidator : AbstractValidator<ProductEntity>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Product id not set.");

            RuleFor(p => p.ProductName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Product name should not be empty.");

            RuleFor(p => p.Store)
                .NotNull()
                .WithMessage("Store does not exist.");
        }
    }
}