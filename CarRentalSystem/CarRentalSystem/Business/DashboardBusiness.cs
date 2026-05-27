using System;
using System.Collections.Generic;
using System.Linq;
using CarRentalSystem.Models;

namespace CarRentalSystem.Business
{
    public class DashboardBusiness
    {
        public double CalculateGrowthRate(decimal currentMonthRev, decimal lastMonthRev)
        {
            if (lastMonthRev > 0)
            {
                return (double)((currentMonthRev - lastMonthRev) / lastMonthRev * 100);
            }
            return currentMonthRev > 0 ? 100 : 0;
        }

        public double CalculateOccupancyRate(int totalVehicles, int rentedVehicles)
        {
            if (totalVehicles == 0) return 0;
            return Math.Round((double)rentedVehicles / totalVehicles * 100, 1);
        }
    }
}