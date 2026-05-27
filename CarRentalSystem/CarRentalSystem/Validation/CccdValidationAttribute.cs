using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CarRentalSystem.Validation
{
    public class CccdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string cccd = value.ToString();
                if (!Regex.IsMatch(cccd, @"^\d{12}$"))
                {
                    return new ValidationResult(ErrorMessage ?? "CCCD bắt buộc phải đủ 12 chữ số.");
                }
            }
            return ValidationResult.Success;
        }
    }
}