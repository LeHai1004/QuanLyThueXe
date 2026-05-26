using System;

namespace CarRentalSystem.Helpers
{
    public static class CodeGeneratorHelper
    {
        public static string GenerateInvoiceCode()
        {
            return "HD" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString("D3");
        }

        public static string GenerateReceiptCode()
        {
            return "PN" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString("D3");
        }
    }
}