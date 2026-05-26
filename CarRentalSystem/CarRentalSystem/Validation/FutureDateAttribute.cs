using System;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Validation
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                // Chỉ lấy phần ngày (bỏ qua giờ/phút) để so sánh với ngày hôm nay
                if (date.Date < DateTime.Now.Date)
                {
                    return new ValidationResult(ErrorMessage ?? "Ngày không được ở trong quá khứ.");
                }
            }
            return ValidationResult.Success;
        }
    }
}