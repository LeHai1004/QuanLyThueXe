using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int UserProfileId { get; set; }

    public string? NationalId { get; set; }

    public string? NationalIdFrontImg { get; set; }

    public string? NationalIdBackImg { get; set; }

    public string? LicenseNumber { get; set; }

    public string? LicenseClass { get; set; }

    public DateOnly? LicenseIssueDate { get; set; }

    public DateOnly? LicenseExpiryDate { get; set; }

    public decimal TotalSpent { get; set; }

    public int TotalRentals { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual UserProfile UserProfile { get; set; } = null!;
}
