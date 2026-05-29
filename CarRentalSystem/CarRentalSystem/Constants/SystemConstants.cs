namespace CarRentalSystem.Constants
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

    public static class RoleConstants
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
}