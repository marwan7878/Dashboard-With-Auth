using Auth.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Auth.Attributes
{
    public class CheckBoxRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Select role!!");
        }
    }
}
