using DiscountCatalog.WebAPI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Validation.Validators
{
    public class UserValidator : AbstractValidator<ApplicationUser>
    {
        public UserValidator()
        {
            RuleFor(u => u.Roles)
                .NotEmpty()
                .OverridePropertyName("Role")
                .WithMessage("Please add user to role.");
                
            RuleForEach(u => u.Roles)
                .NotNull()
                .OverridePropertyName("Role")
                .WithMessage("Role does not exist.");

            RuleFor(u => u.Id)
                .NotNull()
                .WithMessage("Id not assigned.");

            RuleFor(u => u.UserName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Username should not be empty.");

            RuleFor(u => u.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("Email should not be empty.");
        }
    }
}