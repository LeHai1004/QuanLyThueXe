using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class VwTopRentedVehicle
{
    public int VehicleId { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string VehicleName { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public decimal PricePerDay { get; set; }

    public decimal AverageRating { get; set; }

    public int? SoLanThue { get; set; }

    public decimal? TongDoanhThu { get; set; }
}
