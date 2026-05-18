using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class ImportReceiptDetail
{
    public int ImportDetailId { get; set; }

    public int ImportReceiptId { get; set; }

    public int VehicleId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }

    public string? VehicleCondition { get; set; }

    public int CurrentKm { get; set; }

    public string? Note { get; set; }

    public virtual ImportReceipt ImportReceipt { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
