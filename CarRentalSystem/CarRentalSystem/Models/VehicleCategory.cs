using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Models;

public partial class VehicleCategory
{
    [Key]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Tên loại xe không được để trống")]
    [StringLength(100, ErrorMessage = "Tên loại xe không được vượt quá 100 ký tự")]
    public string CategoryName { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
