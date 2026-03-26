namespace Gym.Domain.Constants;

public static class RoleConstants
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Receptionist = "Receptionist";
    public const string Trainer = "Trainer";
    public const string Member = "Member";

    // Mặc định Permissions cho từng vai trò
    public static readonly List<string> AdminPermissions = new() { "*" }; // Toàn quyền

    public static readonly List<string> ReceptionistPermissions = new()
    {
        PermissionConstants.MemberRead,
        PermissionConstants.MemberCreate,
        PermissionConstants.MemberUpdate,
        PermissionConstants.PackageRead,
        PermissionConstants.CheckInCreate,
        PermissionConstants.CheckInRead,
        PermissionConstants.InventoryRead,
        PermissionConstants.InventoryConsume,
        PermissionConstants.ReportRead,
        PermissionConstants.ClassRead
    };

    public static readonly List<string> TrainerPermissions = new()
    {
        PermissionConstants.MemberRead,
        PermissionConstants.ClassRead,
        PermissionConstants.ClassManage,
        PermissionConstants.EquipmentRead,
        PermissionConstants.EquipmentReport,
        PermissionConstants.InventoryConsume
    };

    public static readonly List<string> MemberPermissions = new()
    {
        PermissionConstants.MemberRead,
        PermissionConstants.CheckInCreate,
        PermissionConstants.ClassRead
    };
}
