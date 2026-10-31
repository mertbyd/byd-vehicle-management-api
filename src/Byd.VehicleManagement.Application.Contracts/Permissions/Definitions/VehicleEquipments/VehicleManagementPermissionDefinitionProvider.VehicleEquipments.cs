using Byd.VehicleManagement.Localization;
using Volo.Abp.Authorization.Permissions;

namespace Byd.VehicleManagement.Permissions;

// islevi: VehicleEquipment izin dugumlerini ABP grubuna ekler.
// sistemdeki gorevi: View ve Manage eylemlerini localized olarak sunar.
public partial class VehicleManagementPermissionDefinitionProvider
{
    // VehicleEquipment izin agacini tanimlar.
    private static void AddVehicleEquipmentPermissions(PermissionGroupDefinition group)
    {
        var permission = group.AddPermission(
            VehicleManagementPermissions.VehicleEquipments.Default,
            L(VehicleManagementLocalizationKeys.Permissions.VehicleEquipments));
        permission.AddChild(
            VehicleManagementPermissions.VehicleEquipments.View,
            L(VehicleManagementLocalizationKeys.Permissions.VehicleEquipmentsView));
        permission.AddChild(
            VehicleManagementPermissions.VehicleEquipments.Manage,
            L(VehicleManagementLocalizationKeys.Permissions.VehicleEquipmentsManage));
    }
}
