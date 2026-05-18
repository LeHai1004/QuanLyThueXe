using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class VwCustomer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? StreetAddress { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public string? NationalId { get; set; }

    public string? LicenseNumber { get; set; }

    public string? LicenseClass { get; set; }

    public int TotalRentals { get; set; }

    public decimal TotalSpent { get; set; }

    public DateTime CreatedAt { get; set; }
}
