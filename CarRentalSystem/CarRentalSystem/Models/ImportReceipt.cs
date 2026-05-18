using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class ImportReceipt
{
    public int ImportReceiptId { get; set; }

    public string SoImportReceipt { get; set; } = null!;

    public int SupplierId { get; set; }

    public int StaffId { get; set; }

    public int? ApprovedByStaffId { get; set; }

    public DateTime ImportDate { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Staff? ApprovedByStaff { get; set; }

    public virtual ICollection<ImportReceiptDetail> ImportReceiptDetails { get; set; } = new List<ImportReceiptDetail>();

    public virtual Staff Staff { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
