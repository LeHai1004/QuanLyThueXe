using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public int CategoryId { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string VehicleName { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int ManufactureYear { get; set; }

    public string? Color { get; set; }

    public string? FuelType { get; set; }

    public string? Transmission { get; set; }

    public int Seats { get; set; }

    public decimal PricePerDay { get; set; }

    public string? VehicleDesc { get; set; }

    public string? HinhAnh { get; set; }

    public string Status { get; set; } = null!;

    public decimal AverageRating { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual VehicleCategory Category { get; set; } = null!;

    public virtual ICollection<ImportReceiptDetail> ImportReceiptDetails { get; set; } = new List<ImportReceiptDetail>();

    public virtual ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
