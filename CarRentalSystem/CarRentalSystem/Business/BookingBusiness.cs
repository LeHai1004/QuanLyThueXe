using System;
using CarRentalSystem.Enums;

namespace CarRentalSystem.Business
{
    public class BookingBusiness
    {
        public int CalculateRentalDays(DateTime pickupDate, DateTime returnDate)
        {
            int days = (int)Math.Ceiling((returnDate - pickupDate).TotalDays);
            return days <= 0 ? 1 : days;
        }

        public decimal CalculateBasePrice(int days, decimal pricePerDay)
        {
            return days * pricePerDay;
        }

        public decimal CalculateDeposit(decimal basePrice)
        {
            return basePrice * TaxConfig.DepositRate;
        }

        public decimal CalculateTotalAmount(decimal basePrice, decimal discount)
        {
            decimal total = basePrice - discount;
            return total < 0 ? 0 : total;
        }
    }
}