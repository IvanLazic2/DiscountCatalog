using DiscountCatalog.WebAPI.Extentions;
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

            //DATE

            RuleFor(p => p.DiscountDateBegin)
                .NotNull()
                .NotEmpty()
                .WithMessage("Discount date begin should not be empty.");

            RuleFor(p => p.DiscountDateEnd)
                .NotNull()
                .NotEmpty()
                .WithMessage("Discount date end should not be empty.");

            RuleFor(p => p.DiscountDateEnd)
                .GreaterThan(p => p.DiscountDateBegin);

            RuleFor(p => DateTime.Parse(p.DiscountDateEnd))
                .GreaterThan(DateTime.Now)
                .OverridePropertyName("DiscountDateEnd")
                .WithMessage("Discount date end should not be later than todays date.");

            RuleFor(p => DateTime.Parse(p.DiscountDateEnd))
                .GreaterThan(p => p.DateCreated)
                .WithMessage("Discount date end should be later than todays date.");

            //PRICE

            RuleFor(p => p.NewPrice)
                .LessThan(p => p.OldPrice).WithMessage("New price has to be a discount.")
                .Unless(p => !p.OldPrice.HasValue);

            RuleFor(p => p.OldPrice)
                .NotNull()
                .When(p => p.NewPrice.HasValue && !p.DiscountPercentage.HasValue || !p.NewPrice.HasValue && p.DiscountPercentage.HasValue)
                .WithMessage("Fill in at least two values.");

            RuleFor(p => p.NewPrice)
                .NotNull()
                .When(p => p.OldPrice.HasValue && !p.DiscountPercentage.HasValue || !p.OldPrice.HasValue && p.DiscountPercentage.HasValue)
                .WithMessage("Fill in at least two values.");

            RuleFor(p => p.DiscountPercentage)
                .NotNull()
                .When(p => p.NewPrice.HasValue && !p.OldPrice.HasValue || !p.NewPrice.HasValue && p.OldPrice.HasValue)
                .WithMessage("Fill in at least two values.");

            RuleFor(p => p.OldPrice)
                .NotNull()
                .When(p => !p.NewPrice.HasValue && !p.OldPrice.HasValue && !p.DiscountPercentage.HasValue)
                .WithMessage("Fill in at least two values.");

            RuleFor(p => p.NewPrice)
                .NotNull()
                .When(p => !p.NewPrice.HasValue && !p.OldPrice.HasValue && !p.DiscountPercentage.HasValue)
                .WithMessage("Fill in at least two values.");

            RuleFor(p => p.DiscountPercentage)
                .NotNull()
                .When(p => !p.NewPrice.HasValue && !p.OldPrice.HasValue && !p.DiscountPercentage.HasValue)
                .WithMessage("Fill in at least two values.");

        }
    }
}