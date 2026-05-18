using System;
using System.Collections.Generic;

namespace CarRentalSystem.Models;

public partial class VwMonthlyRevenue
{
    public int? Nam { get; set; }

    public int? Thang { get; set; }

    public int? InvoiceNumber { get; set; }

    public decimal? TongDoanhThu { get; set; }
}
