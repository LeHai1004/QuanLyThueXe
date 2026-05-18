using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class InvoiceDetail
{
    public int InvoiceDetailId { get; set; }

    public int InvoiceId { get; set; }

    public string ItemType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountPercent { get; set; }

    public decimal? LineTotal { get; set; }

    public string? Note { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
