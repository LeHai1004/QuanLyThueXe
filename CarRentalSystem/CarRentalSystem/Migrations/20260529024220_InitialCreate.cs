using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__8AFACE1A779A8EA8", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__4BE666B4540D88A6", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VehicleC__19093A0BF7B64D57", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__349DA5A67699BF4F", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    VehicleName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ManufactureYear = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Transmission = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Seats = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VehicleDesc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "San sang"),
                    AverageRating = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vehicle__476B5492060F1E90", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_Xe_VehicleCategory",
                        column: x => x.CategoryId,
                        principalTable: "VehicleCategory",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    UserProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserProf__9E267F6212FE7E1C", x => x.UserProfileId);
                    table.ForeignKey(
                        name: "FK_UserProfile_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<int>(type: "int", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NationalIdFrontImg = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    NationalIdBackImg = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LicenseClass = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LicenseIssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LicenseExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRentals = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__A4AE64D865B0FE28", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customer_UserProfile",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "UserProfileId");
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<int>(type: "int", nullable: false),
                    StaffCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Staff__96D4AB1717B12CDA", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_Staff_UserProfile",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "UserProfileId");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    PickupLocation = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ReturnLocation = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PickupDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ReturnDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    RentalDays = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(datediff(day,[PickupDateTime],[ReturnDateTime]))", stored: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExtraFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Cho xac nhan"),
                    BookingChannel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Online"),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__73951AED904BC481", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Booking_Customer",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_Booking_Staff",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId");
                    table.ForeignKey(
                        name: "FK_Booking_Xe",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateTable(
                name: "ImportReceipt",
                columns: table => new
                {
                    ImportReceiptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoImportReceipt = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    ApprovedByStaffId = table.Column<int>(type: "int", nullable: true),
                    ImportDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ApprovalDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Cho duyet"),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ImportRe__3B303D4067FCBC38", x => x.ImportReceiptId);
                    table.ForeignKey(
                        name: "FK_ImportReceipt_NCC",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "SupplierId");
                    table.ForeignKey(
                        name: "FK_ImportReceipt_NV",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId");
                    table.ForeignKey(
                        name: "FK_ImportReceipt_NVDuyet",
                        column: x => x.ApprovedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceLog",
                columns: table => new
                {
                    MaintenanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    MaintenanceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaintenanceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MaintenanceDateTiep = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Hoan thanh")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Maintena__E60542D5B158E676", x => x.MaintenanceId);
                    table.ForeignKey(
                        name: "FK_MaintenanceLog_Staff",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId");
                    table.ForeignKey(
                        name: "FK_MaintenanceLog_Vehicle",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    LoaiInvoice = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Thue xe"),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 10m),
                    TaxAmount = table.Column<decimal>(type: "decimal(28,8)", nullable: true, computedColumnSql: "(round(([SubTotal]*[TaxRate])/(100),(0)))", stored: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Chua thanh toan"),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaidDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Invoice__D796AAB55EC62084", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoice_Booking",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId");
                    table.ForeignKey(
                        name: "FK_Invoice_KH",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_Invoice_NV",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId");
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    StarRating = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayReview = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Review__74BC79CE98873A01", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Review_Booking",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId");
                    table.ForeignKey(
                        name: "FK_Review_KH",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_Review_Xe",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateTable(
                name: "ImportReceiptDetail",
                columns: table => new
                {
                    ImportDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportReceiptId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(29,2)", nullable: true, computedColumnSql: "([Quantity]*[UnitPrice])", stored: true),
                    VehicleCondition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrentKm = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ImportRe__CDFBBA71AA7A366D", x => x.ImportDetailId);
                    table.ForeignKey(
                        name: "FK_CTPN_ImportReceipt",
                        column: x => x.ImportReceiptId,
                        principalTable: "ImportReceipt",
                        principalColumn: "ImportReceiptId");
                    table.ForeignKey(
                        name: "FK_ImportReceiptDetail_Vehicle",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetail",
                columns: table => new
                {
                    InvoiceDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(38,6)", nullable: true, computedColumnSql: "(round(([Quantity]*[UnitPrice])*((1)-[DiscountPercent]/(100)),(0)))", stored: true),
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__InvoiceD__1F157811EC5BA1D2", x => x.InvoiceDetailId);
                    table.ForeignKey(
                        name: "FK_CTHD_Invoice",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "InvoiceId");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Thanh cong"),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PaymentTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__9B556A38E9EA31C2", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_Invoice",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "InvoiceId");
                    table.ForeignKey(
                        name: "FK_Payment_NV",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Account__A9D1053485AA283D",
                table: "Account",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerId",
                table: "Booking",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_StaffId",
                table: "Booking",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_VehicleId",
                table: "Booking",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__9E267F63FB6EF426",
                table: "Customer",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceipt_ApprovedByStaffId",
                table: "ImportReceipt",
                column: "ApprovedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceipt_StaffId",
                table: "ImportReceipt",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceipt_SupplierId",
                table: "ImportReceipt",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "UQ__ImportRe__1ACA5AF246A28CED",
                table: "ImportReceipt",
                column: "SoImportReceipt",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceiptDetail_ImportReceiptId",
                table: "ImportReceiptDetail",
                column: "ImportReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceiptDetail_VehicleId",
                table: "ImportReceiptDetail",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CustomerId",
                table: "Invoice",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_StaffId",
                table: "Invoice",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "UQ__Invoice__73951AECFA9724DB",
                table: "Invoice",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Invoice__D776E9811E02B3F7",
                table: "Invoice",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_InvoiceId",
                table: "InvoiceDetail",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLog_StaffId",
                table: "MaintenanceLog",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLog_VehicleId",
                table: "MaintenanceLog",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_InvoiceId",
                table: "Payment",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_StaffId",
                table: "Payment",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_CustomerId",
                table: "Review",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_VehicleId",
                table: "Review",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Review__73951AEC58044954",
                table: "Review",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Role__37A723F3742F8C49",
                table: "Role",
                column: "TenRole",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Staff__9E267F63064496C6",
                table: "Staff",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Staff__D83AD81287D3A812",
                table: "Staff",
                column: "StaffCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__UserProf__349DA5A7B2D65AB3",
                table: "UserProfile",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_CategoryId",
                table: "Vehicle",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "UQ__Vehicle__026BC15C34001216",
                table: "Vehicle",
                column: "LicensePlate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportReceiptDetail");

            migrationBuilder.DropTable(
                name: "InvoiceDetail");

            migrationBuilder.DropTable(
                name: "MaintenanceLog");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "ImportReceipt");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "VehicleCategory");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
