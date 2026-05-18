using System;
using System.Collections.Generic;
using CarRentalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Data;

public partial class CarRentalContext : DbContext
{
    public CarRentalContext()
    {
    }

    public CarRentalContext(DbContextOptions<CarRentalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<ImportReceipt> ImportReceipts { get; set; }

    public virtual DbSet<ImportReceiptDetail> ImportReceiptDetails { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<MaintenanceLog> MaintenanceLogs { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleCategory> VehicleCategories { get; set; }

    public virtual DbSet<VwBooking> VwBookings { get; set; }

    public virtual DbSet<VwCustomer> VwCustomers { get; set; }

    public virtual DbSet<VwMonthlyRevenue> VwMonthlyRevenues { get; set; }

    public virtual DbSet<VwTopRentedVehicle> VwTopRentedVehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=QuanLyThueXe;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Vietnamese_CI_AS");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A67699BF4F");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D1053485AA283D").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__73951AED904BC481");

            entity.ToTable("Booking");

            entity.Property(e => e.BasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BookingChannel)
                .HasMaxLength(20)
                .HasDefaultValue("Online");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExtraFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.PickupDateTime).HasColumnType("datetime");
            entity.Property(e => e.PickupLocation).HasMaxLength(300);
            entity.Property(e => e.RentalDays).HasComputedColumnSql("(datediff(day,[PickupDateTime],[ReturnDateTime]))", true);
            entity.Property(e => e.ReturnDateTime).HasColumnType("datetime");
            entity.Property(e => e.ReturnLocation).HasMaxLength(300);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Cho xac nhan");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Customer");

            entity.HasOne(d => d.Staff).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_Booking_Staff");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Xe");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D865B0FE28");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.UserProfileId, "UQ__Customer__9E267F63FB6EF426").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LicenseClass).HasMaxLength(10);
            entity.Property(e => e.LicenseNumber).HasMaxLength(20);
            entity.Property(e => e.NationalId).HasMaxLength(20);
            entity.Property(e => e.NationalIdBackImg).HasMaxLength(300);
            entity.Property(e => e.NationalIdFrontImg).HasMaxLength(300);
            entity.Property(e => e.TotalSpent).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.UserProfile).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_UserProfile");
        });

        modelBuilder.Entity<ImportReceipt>(entity =>
        {
            entity.HasKey(e => e.ImportReceiptId).HasName("PK__ImportRe__3B303D4067FCBC38");

            entity.ToTable("ImportReceipt");

            entity.HasIndex(e => e.SoImportReceipt, "UQ__ImportRe__1ACA5AF246A28CED").IsUnique();

            entity.Property(e => e.ApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImportDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.SoImportReceipt).HasMaxLength(20);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Cho duyet");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ApprovedByStaff).WithMany(p => p.ImportReceiptApprovedByStaffs)
                .HasForeignKey(d => d.ApprovedByStaffId)
                .HasConstraintName("FK_ImportReceipt_NVDuyet");

            entity.HasOne(d => d.Staff).WithMany(p => p.ImportReceiptStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportReceipt_NV");

            entity.HasOne(d => d.Supplier).WithMany(p => p.ImportReceipts)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportReceipt_NCC");
        });

        modelBuilder.Entity<ImportReceiptDetail>(entity =>
        {
            entity.HasKey(e => e.ImportDetailId).HasName("PK__ImportRe__CDFBBA71AA7A366D");

            entity.ToTable("ImportReceiptDetail", tb => tb.HasTrigger("trg_UpdateImportReceiptTotal"));

            entity.Property(e => e.LineTotal)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", true)
                .HasColumnType("decimal(29, 2)");
            entity.Property(e => e.Note).HasMaxLength(300);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VehicleCondition).HasMaxLength(100);

            entity.HasOne(d => d.ImportReceipt).WithMany(p => p.ImportReceiptDetails)
                .HasForeignKey(d => d.ImportReceiptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTPN_ImportReceipt");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.ImportReceiptDetails)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportReceiptDetail_Vehicle");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoice__D796AAB55EC62084");

            entity.ToTable("Invoice");

            entity.HasIndex(e => e.BookingId, "UQ__Invoice__73951AECFA9724DB").IsUnique();

            entity.HasIndex(e => e.InvoiceNumber, "UQ__Invoice__D776E9811E02B3F7").IsUnique();

            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.GrandTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InvoiceNumber).HasMaxLength(20);
            entity.Property(e => e.IssueDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LoaiInvoice)
                .HasMaxLength(30)
                .HasDefaultValue("Thue xe");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.PaidDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Chua thanh toan");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxAmount)
                .HasComputedColumnSql("(round(([SubTotal]*[TaxRate])/(100),(0)))", true)
                .HasColumnType("decimal(28, 8)");
            entity.Property(e => e.TaxRate)
                .HasDefaultValue(10m)
                .HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Booking).WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_Booking");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_KH");

            entity.HasOne(d => d.Staff).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_Invoice_NV");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.InvoiceDetailId).HasName("PK__InvoiceD__1F157811EC5BA1D2");

            entity.ToTable("InvoiceDetail");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ItemType).HasMaxLength(50);
            entity.Property(e => e.LineTotal)
                .HasComputedColumnSql("(round(([Quantity]*[UnitPrice])*((1)-[DiscountPercent]/(100)),(0)))", true)
                .HasColumnType("decimal(38, 6)");
            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_Invoice");
        });

        modelBuilder.Entity<MaintenanceLog>(entity =>
        {
            entity.HasKey(e => e.MaintenanceId).HasName("PK__Maintena__E60542D5B158E676");

            entity.ToTable("MaintenanceLog");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.MaintenanceType).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Hoan thanh");

            entity.HasOne(d => d.Staff).WithMany(p => p.MaintenanceLogs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_MaintenanceLog_Staff");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.MaintenanceLogs)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceLog_Vehicle");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A38E9EA31C2");

            entity.ToTable("Payment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Note).HasMaxLength(300);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Thanh cong");
            entity.Property(e => e.TransactionCode).HasMaxLength(100);

            entity.HasOne(d => d.Invoice).WithMany(p => p.Payments)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Invoice");

            entity.HasOne(d => d.Staff).WithMany(p => p.Payments)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_Payment_NV");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79CE98873A01");

            entity.ToTable("Review", tb => tb.HasTrigger("trg_UpdateVehicleAverageRating"));

            entity.HasIndex(e => e.BookingId, "UQ__Review__73951AEC58044954").IsUnique();

            entity.Property(e => e.Content).HasMaxLength(500);
            entity.Property(e => e.NgayReview)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Booking).WithOne(p => p.Review)
                .HasForeignKey<Review>(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_Booking");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_KH");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_Xe");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A779A8EA8");

            entity.ToTable("Role");

            entity.HasIndex(e => e.TenRole, "UQ__Role__37A723F3742F8C49").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.TenRole).HasMaxLength(50);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB1717B12CDA");

            entity.HasIndex(e => e.UserProfileId, "UQ__Staff__9E267F63064496C6").IsUnique();

            entity.HasIndex(e => e.StaffCode, "UQ__Staff__D83AD81287D3A812").IsUnique();

            entity.Property(e => e.Branch).HasMaxLength(150);
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.StaffCode).HasMaxLength(20);

            entity.HasOne(d => d.UserProfile).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_UserProfile");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4540D88A6");

            entity.ToTable("Supplier");

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.ContactPerson).HasMaxLength(150);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.SupplierName).HasMaxLength(200);
            entity.Property(e => e.TaxCode).HasMaxLength(20);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserProfileId).HasName("PK__UserProf__9E267F6212FE7E1C");

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.AccountId, "UQ__UserProf__349DA5A7B2D65AB3").IsUnique();

            entity.Property(e => e.AvatarUrl).HasMaxLength(300);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.StreetAddress).HasMaxLength(200);
            entity.Property(e => e.Ward).HasMaxLength(100);

            entity.HasOne(d => d.Account).WithOne(p => p.UserProfile)
                .HasForeignKey<UserProfile>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_Account");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicle__476B5492060F1E90");

            entity.ToTable("Vehicle");

            entity.HasIndex(e => e.LicensePlate, "UQ__Vehicle__026BC15C34001216").IsUnique();

            entity.Property(e => e.AverageRating).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FuelType).HasMaxLength(30);
            entity.Property(e => e.HinhAnh).HasMaxLength(300);
            entity.Property(e => e.LicensePlate).HasMaxLength(15);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.PricePerDay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Seats).HasDefaultValue(5);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("San sang");
            entity.Property(e => e.Transmission).HasMaxLength(30);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.VehicleDesc).HasMaxLength(500);
            entity.Property(e => e.VehicleName).HasMaxLength(150);

            entity.HasOne(d => d.Category).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Xe_VehicleCategory");
        });

        modelBuilder.Entity<VehicleCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__VehicleC__19093A0BF7B64D57");

            entity.ToTable("VehicleCategory");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<VwBooking>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Booking");

            entity.Property(e => e.BasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BookingChannel).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.ExtraFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LicensePlate).HasMaxLength(15);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.PickupDateTime).HasColumnType("datetime");
            entity.Property(e => e.PickupLocation).HasMaxLength(300);
            entity.Property(e => e.ReturnDateTime).HasColumnType("datetime");
            entity.Property(e => e.ReturnLocation).HasMaxLength(300);
            entity.Property(e => e.Status).HasMaxLength(30);
            entity.Property(e => e.TenCustomer).HasMaxLength(150);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VehicleCategory).HasMaxLength(100);
            entity.Property(e => e.VehicleName).HasMaxLength(150);
        });

        modelBuilder.Entity<VwCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Customer");

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.LicenseClass).HasMaxLength(10);
            entity.Property(e => e.LicenseNumber).HasMaxLength(20);
            entity.Property(e => e.NationalId).HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.StreetAddress).HasMaxLength(200);
            entity.Property(e => e.TotalSpent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Ward).HasMaxLength(100);
        });

        modelBuilder.Entity<VwMonthlyRevenue>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_MonthlyRevenue");

            entity.Property(e => e.TongDoanhThu).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<VwTopRentedVehicle>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TopRentedVehicles");

            entity.Property(e => e.AverageRating).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.LicensePlate).HasMaxLength(15);
            entity.Property(e => e.PricePerDay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TongDoanhThu).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.VehicleName).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
