using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PWEB_TP.Controllers
{
    public class DateValidation : ValidationAttribute
    {
        private const string greaterThenMessage = "{0} deve ser maior que a data atual";

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // your validation logic
            if ((DateTime)value >= DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(greaterThenMessage);
            }
        }
    }
}