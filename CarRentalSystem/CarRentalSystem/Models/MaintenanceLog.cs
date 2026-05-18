using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class MaintenanceLog
{
    public int MaintenanceId { get; set; }

    public int VehicleId { get; set; }

    public int? StaffId { get; set; }

    public string MaintenanceType { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Cost { get; set; }

    public DateOnly MaintenanceDate { get; set; }

    public DateOnly? MaintenanceDateTiep { get; set; }

    public string Status { get; set; } = null!;

    public virtual Staff? Staff { get; set; }

    public virtual Vehicle Vehicle { get; set; } = null!;
}
