using Byd.VehicleManagement.Localization;
using Volo.Abp.Authorization.Permissions;

namespace Byd.VehicleManagement.Permissions;

// islevi: Vehicle izin dugumlerini ABP grubuna ekler.
// sistemdeki gorevi: View ve Manage eylemlerini localized olarak sunar.
public partial class VehicleManagementPermissionDefinitionProvider
{
    // Vehicle izin agacini tanimlar.
    private static void AddVehiclePermissions(PermissionGroupDefinition group)
    {
        var permission = group.AddPermission(
            VehicleManagementPermissions.Vehicles.Default,
            L(VehicleManagementLocalizationKeys.Permissions.Vehicles));
        permission.AddChild(
            VehicleManagementPermissions.Vehicles.View,
            L(VehicleManagementLocalizationKeys.Permissions.VehiclesView));
        permission.AddChild(
            VehicleManagementPermissions.Vehicles.Manage,
            L(VehicleManagementLocalizationKeys.Permissions.VehiclesManage));
    }
}
