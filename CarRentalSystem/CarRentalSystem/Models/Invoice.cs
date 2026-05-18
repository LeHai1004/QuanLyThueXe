using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int? StaffId { get; set; }

    public string LoaiInvoice { get; set; } = null!;

    public decimal SubTotal { get; set; }

    public decimal TaxRate { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal GrandTotal { get; set; }

    public string Status { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? PaidDate { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Staff? Staff { get; set; }
}
