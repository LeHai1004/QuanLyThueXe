using CarRentalSystem.Data;
using System;
using System.Linq;

namespace CarRentalSystem.Business
{
    public class VehicleAvailabilityBusiness
    {
        private readonly CarRentalContext _context;

        public VehicleAvailabilityBusiness(CarRentalContext context)
        {
            _context = context;
        }

        public bool IsVehicleAvailable(int vehicleId, DateTime pickup, DateTime returnDate)
        {
            bool isConflict = _context.Bookings.Any(b =>
                b.VehicleId == vehicleId &&
                b.Status != "Da huy" &&
                !(b.ReturnDateTime <= pickup || b.PickupDateTime >= returnDate));

            return !isConflict;
        }
    }
}