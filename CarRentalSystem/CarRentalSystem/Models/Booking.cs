using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int VehicleId { get; set; }

    public int? StaffId { get; set; }

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

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Invoice? Invoice { get; set; }

    public virtual Review? Review { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual Vehicle Vehicle { get; set; } = null!;
}
