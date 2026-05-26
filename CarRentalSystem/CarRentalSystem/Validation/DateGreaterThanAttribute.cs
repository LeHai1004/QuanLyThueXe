using System;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Validation
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = value as DateTime?;
            if (currentValue == null)
            {
                return ValidationResult.Success;
            }

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
            {
                return new ValidationResult($"Property {_comparisonProperty} not found.");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance) as DateTime?;
            if (comparisonValue == null)
            {
                return ValidationResult.Success;
            }

            if (currentValue.Value <= comparisonValue.Value)
            {
                return new ValidationResult(ErrorMessage ?? "Ngày kết thúc phải lớn hơn ngày bắt đầu.");
            }

            return ValidationResult.Success;
        }
    }
}