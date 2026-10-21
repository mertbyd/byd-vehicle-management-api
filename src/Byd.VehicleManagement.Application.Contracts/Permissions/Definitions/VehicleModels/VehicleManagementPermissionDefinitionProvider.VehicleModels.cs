using Byd.VehicleManagement.Localization;
using Volo.Abp.Authorization.Permissions;

namespace Byd.VehicleManagement.Permissions;

// islevi: VehicleModel izin dugumlerini ABP grubuna ekler.
// sistemdeki gorevi: View ve Manage eylemlerini localized olarak sunar.
public partial class VehicleManagementPermissionDefinitionProvider
{
    // VehicleModel izin agacini tanimlar.
    private static void AddVehicleModelPermissions(PermissionGroupDefinition group)
    {
        var permission = group.AddPermission(
            VehicleManagementPermissions.VehicleModels.Default,
            L(VehicleManagementLocalizationKeys.Permissions.VehicleModels));
        permission.AddChild(
            VehicleManagementPermissions.VehicleModels.View,
            L(VehicleManagementLocalizationKeys.Permissions.VehicleModelsView));
        permission.AddChild(
            VehicleManagementPermissions.VehicleModels.Manage,
            L(VehicleManagementLocalizationKeys.Permissions.VehicleModelsManage));
    }
}
