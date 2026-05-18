using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class UserProfile
{
    public int UserProfileId { get; set; }

    public int AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? StreetAddress { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public string? AvatarUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual Staff? Staff { get; set; }
}
