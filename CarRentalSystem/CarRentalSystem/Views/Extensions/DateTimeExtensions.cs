using System;

namespace CarRentalSystem.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToVnFormat(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm");
        }
    }
}