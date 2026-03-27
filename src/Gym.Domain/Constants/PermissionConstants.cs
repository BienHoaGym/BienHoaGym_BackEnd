namespace Gym.Domain.Constants;

public static class PermissionConstants
{
    // Hội viên
    public const string MemberRead = "member.read";
    public const string MemberCreate = "member.create";
    public const string MemberUpdate = "member.update";
    public const string MemberDelete = "member.delete";
    public const string TrainerRead = "trainer.read";

    // Gói tập
    public const string PackageRead = "package.read";
    public const string PackageCreate = "package.create";
    public const string PackageUpdate = "package.update";
    public const string PackageDelete = "package.delete";

    // Check-in
    public const string CheckInRead = "checkin.read";
    public const string CheckInCreate = "checkin.create";

    // Lớp học
    public const string ClassRead = "class.read";
    public const string ClassCreate = "class.create";
    public const string ClassUpdate = "class.update";
    public const string ClassDelete = "class.delete";
    public const string ClassManage = "class.manage"; // Cho PT điểm danh

    // Thiết bị
    public const string EquipmentRead = "equipment.read";
    public const string EquipmentCreate = "equipment.create";
    public const string EquipmentUpdate = "equipment.update";
    public const string EquipmentDelete = "equipment.delete";
    public const string EquipmentReport = "equipment.report"; // Cho PT báo hỏng

    // Kho
    public const string InventoryRead = "inventory.read";
    public const string InventoryCreate = "inventory.create";
    public const string InventoryUpdate = "inventory.update";
    public const string InventoryDelete = "inventory.delete";
    public const string InventoryConsume = "inventory.consume"; // Lấy khăn, nước...

    // Báo cáo
    public const string ReportRead = "report.read";
    public const string ReportFinancial = "report.financial";

    // Sản phẩm
    public const string ProductRead = "product.read";
    public const string ProductCreate = "product.create";
    public const string ProductUpdate = "product.update";
    public const string ProductDelete = "product.delete";

    // Thanh toán & Hóa đơn (Billing)
    public const string BillingRead = "billing.read";
    public const string BillingCreate = "billing.create";
    public const string BillingManage = "billing.manage";

    // Gói đăng ký (Subscription)
    public const string SubscriptionRead = "subscription.read";
    public const string SubscriptionCreate = "subscription.create";
    public const string SubscriptionUpdate = "subscription.update";
    public const string SubscriptionDelete = "subscription.delete";

    // Thanh toán (Payment)
    public const string PaymentRead = "payment.read";
    public const string PaymentCreate = "payment.create";

    // Hệ thống
    public const string AuditLogRead = "auditlog.read";
    public const string SettingsManage = "settings.manage";
    public const string DashboardRead = "report.read"; // Dashboard sử dụng chung quyền xem báo cáo
}
