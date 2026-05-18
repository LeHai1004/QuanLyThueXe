using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int InvoiceId { get; set; }

    public int? StaffId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? TransactionCode { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime PaymentTime { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual Staff? Staff { get; set; }
}
