using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TheWizardsUnitTest
{
    public class DateNotInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if(value == null || Convert.ToDateTime(value) <= DateTime.Today)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"{validationContext.DisplayName} cannot be the future.");
                }
            }
            catch(Exception)
            {
                return new ValidationResult($"{validationContext.DisplayName} is not a valid date");
            }
        }
    }
}
