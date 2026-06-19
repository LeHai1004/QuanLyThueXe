using System;

namespace CarRentalSystem.Helpers
{
    public static class CodeGeneratorHelper
    {
        private static readonly Random _random = new Random();

        public static string GenerateInvoiceCode()
        {
            return "HD" + DateTime.Now.ToString("yyyyMMddHHmmss") + _random.Next(100, 999).ToString();
        }

        public static string GenerateReceiptCode()
        {
            return "PN" + DateTime.Now.ToString("yyyyMMddHHmmss") + _random.Next(100, 999).ToString();
        }
    }
}