using System;

namespace CarRentalSystem.Helpers
{
    public static class CurrencyHelper
    {
        public static string FormatVnd(decimal amount)
        {
            return amount.ToString("N0") + "đ";
        }

        public static string FormatVnd(double amount)
        {
            return amount.ToString("N0") + "đ";
        }
    }
}