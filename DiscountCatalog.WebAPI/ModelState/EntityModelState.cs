using DiscountCatalog.Common.Models;
using FluentValidation.Results;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.ModelState
{
    public class EntityModelState
    {
        private Result Result { get; set; }

        private IList<IdentityResult> IdentityResults { get; set; }
        private IList<ValidationResult> ValidationResults { get; set; }
        private IList<DbEntityValidationResult> DbEntityValidationResults { get; set; }
        private IList<string> Errors { get; set; }
        private IList<KeyValuePair<string, string>> MarkedErrors { get; set; }
        private IList<Result> Results { get; set; }

        public string SuccessMessage { get; set; }

        public EntityModelState()
        {
            Result = new Result();
            IdentityResults = new List<IdentityResult>();
            ValidationResults = new List<ValidationResult>();
            DbEntityValidationResults = new List<DbEntityValidationResult>();
            Errors = new List<string>();
            MarkedErrors = new List<KeyValuePair<string, string>>();
            Results = new List<Result>();
        }

        public void Add(IdentityResult identityResult)
        {
            IdentityResults.Add(identityResult);
        }

        public void Add(ValidationResult validationResult)
        {
            ValidationResults.Add(validationResult);
        }

        public void Add(DbEntityValidationResult dbEntityValidationResult)
        {
            DbEntityValidationResults.Add(dbEntityValidationResult);
        }

        public void Add(string error)
        {
            Errors.Add(error);
        }

        public void Add(string propertyName, string error)
        {
            MarkedErrors.Add(new KeyValuePair<string, string>(propertyName, error));
        }

        public void Add(KeyValuePair<string, string> keyValuePair)
        {
            MarkedErrors.Add(keyValuePair);
        }

        public void Add(Result result)
        {
            Results.Add(result);
        }

        public void Set(List<IdentityResult> identityResults)
        {
            IdentityResults = identityResults;
        }

        public void Set(List<ValidationResult> validationResults)
        {
            ValidationResults = validationResults;
        }

        public void Set(List<DbEntityValidationResult> validationResults)
        {
            DbEntityValidationResults = validationResults;
        }

        public Result GetResult()
        {
            var result = new Result();

            foreach (var dbEntityValidationResult in DbEntityValidationResults)
            {
                if (!dbEntityValidationResult.IsValid)
                {
                    foreach (var error in dbEntityValidationResult.ValidationErrors)
                    {
                        result.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                }
            }

            foreach (var validationResult in ValidationResults)
            {
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        if (error.PropertyName.Contains("[0]"))
                        {
                            result.AddModelError(error.PropertyName.Split('[')[0], error.ErrorMessage);
                        }
                        else
                        {
                            result.AddModelError(error.PropertyName, error.ErrorMessage);
                        }

                    }
                }
            }

            foreach (var identityResult in IdentityResults)
            {
                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        result.Add(error);
                    }
                }
            }

            foreach (var res in Results)
            {
                foreach (var modelState in res.ModelState)
                {
                    result.AddModelError(modelState.Key, modelState.Value);
                }
            }

            foreach (var error in MarkedErrors)
            {
                result.AddModelError(error.Key, error.Value);
            }

            foreach (var error in Errors)
            {
                result.Add(error);
            }

            if (result.Success)
            {
                result.SuccessMessage = SuccessMessage;
            }

            return result;
        }
    }
}