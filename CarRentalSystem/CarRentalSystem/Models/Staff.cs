using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int UserProfileId { get; set; }

    public string StaffCode { get; set; } = null!;

    public string? Position { get; set; }

    public string? Department { get; set; }

    public string? Branch { get; set; }

    public DateOnly? HireDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<ImportReceipt> ImportReceiptApprovedByStaffs { get; set; } = new List<ImportReceipt>();

    public virtual ICollection<ImportReceipt> ImportReceiptStaffs { get; set; } = new List<ImportReceipt>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual UserProfile UserProfile { get; set; } = null!;
}
