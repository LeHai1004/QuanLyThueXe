using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class VwBooking
{
    public int BookingId { get; set; }

    public string TenCustomer { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string VehicleName { get; set; } = null!;

    public string VehicleCategory { get; set; } = null!;

    public string PickupLocation { get; set; } = null!;

    public string ReturnLocation { get; set; } = null!;

    public DateTime PickupDateTime { get; set; }

    public DateTime ReturnDateTime { get; set; }

    public int? RentalDays { get; set; }

    public decimal BasePrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal ExtraFee { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public string BookingChannel { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
