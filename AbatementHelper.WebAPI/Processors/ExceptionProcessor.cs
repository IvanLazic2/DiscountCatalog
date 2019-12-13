using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public static class ExceptionProcessor
    {
        public static DbEntityValidationException processException(DbEntityValidationException ex)
        {
            var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            var fullErrorMessage = string.Join("", errorMessages);

            var exceptionMessage = string.Concat(ex.Message, "The validation errors are: ", fullErrorMessage);


            return new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        }
    }
}