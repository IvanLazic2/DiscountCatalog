using AbatementHelper.CommonModels.WebApiModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.MVC.Validators
{
    public class ProductValidator : AbstractValidator<WebApiProduct>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductNewPrice)
                .LessThan(p => p.ProductOldPrice).WithMessage("New price has to be a discount!");
        }
    }
}