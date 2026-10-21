using Byd.VehicleManagement.Localization;
using Volo.Abp.Authorization.Permissions;

namespace Byd.VehicleManagement.Permissions;

// islevi: VehicleType izin dugumlerini ABP grubuna ekler.
// sistemdeki gorevi: View ve Manage eylemlerini localized olarak sunar.
public partial class VehicleManagementPermissionDefinitionProvider
{
    // VehicleType izin agacini tanimlar.
    private static void AddVehicleTypePermissions(PermissionGroupDefinition group)
    {
        var permission = group.AddPermission(
            VehicleManagementPermissions.VehicleTypes.Default,
            L(VehicleManagementLocalizationKeys.Permissions.VehicleTypes));
        permission.AddChild(
            VehicleManagementPermissions.VehicleTypes.View,
            L(VehicleManagementLocalizationKeys.Permissions.VehicleTypesView));
        permission.AddChild(
            VehicleManagementPermissions.VehicleTypes.Manage,
            L(VehicleManagementLocalizationKeys.Permissions.VehicleTypesManage));
    }
}
