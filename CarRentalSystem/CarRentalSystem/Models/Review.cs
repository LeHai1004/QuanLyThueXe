using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int VehicleId { get; set; }

    public int StarRating { get; set; }

    public string? Content { get; set; }

    public DateTime NgayReview { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
