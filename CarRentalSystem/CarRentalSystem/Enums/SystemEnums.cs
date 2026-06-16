namespace CarRentalSystem.Enums
{
    public static class VehicleStatus
    {
        public const string Available = "San sang";
        public const string Rented = "Dang thue";
        public const string Maintenance = "Bao duong";
        public const string Inactive = "Ngung hoat dong";
    }

    public static class BookingStatus
    {
        public const string Pending = "Cho xac nhan";
        public const string Confirmed = "Da xac nhan";
        public const string Active = "Dang thue";
        public const string Completed = "Hoan thanh";
        public const string Cancelled = "Da huy";
    }

    public static class ImportReceiptStatus
    {
        public const string Pending = "Cho duyet";
        public const string Approved = "Da duyet";
        public const string Rejected = "Tu choi";
    }

    public static class InvoiceStatus
    {
        public const string Paid = "Da thanh toan";
        public const string Unpaid = "Chua thanh toan";
    }

    public static class RoleEnums
    {
        public const string Admin = "Admin";
        public const string Staff = "Staff";
        public const string Customer = "Customer";
    }

    public static class PaymentMethod
    {
        public const string Cash = "Tien mat";
        public const string Transfer = "Chuyen khoan";
    }

    public static class MaintenanceType
    {
        public const string Periodic = "Dinh ky";
        public const string Repair = "Sua chua";
        public const string Inspection = "Kiem tra";
    }

    public static class InvoiceType
    {
        public const string Rental = "Thue xe";
        public const string Deposit = "Dat coc";
        public const string Fine = "Phat";
    }
    public static class MaintenanceStatus
    {
        public const string Requested = "Yeu cau";
        public const string Pending = "Cho xu ly";
        public const string InProgress = "Dang thuc hien";
        public const string Completed = "Hoan thanh";
        public const string Rejected = "Tu choi";
    }

    public static class PaymentStatus
    {
        public const string Success = "Thanh cong";
        public const string Failed = "That bai";
        public const string Pending = "Cho thanh toan";
    }

    // ===== CONSTANTS (Chống hard-code) =====

    public static class RoleIds
    {
        public const int Admin = 1;
        public const int Staff = 2;
        public const int Customer = 3;
    }

    public static class TaxConfig
    {
        public const decimal DefaultTaxRate = 10m;
        public const decimal ProfitMarginRate = 0.35m;
    }

    public static class BookingDefaults
    {
        public const string DefaultPickupLocation = "Tại cửa hàng";
        public const string DefaultReturnLocation = "Tại cửa hàng";
        public const string OnlineChannel = "Online";
        public const string PaymentCash = "Cash";
        public const string PaymentTransfer = "Transfer";
    }

    public static class VehicleDefaults
    {
        public const string DefaultImageUrl = "https://images.unsplash.com/photo-1550355291-bbee04a92027";
        public const string DefaultCondition = "Bình thường";
    }

    public static class CustomerTierThreshold
    {
        public const decimal Gold = 50_000_000m;
        public const decimal Silver = 20_000_000m;
        public const decimal Bronze = 5_000_000m;
    }
}