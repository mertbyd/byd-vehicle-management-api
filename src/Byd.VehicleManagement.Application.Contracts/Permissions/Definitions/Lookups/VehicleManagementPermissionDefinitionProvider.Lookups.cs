using Byd.VehicleManagement.Localization;
using Volo.Abp.Authorization.Permissions;

namespace Byd.VehicleManagement.Permissions;

// islevi: Arac lookup izin dugumlerini ABP grubuna ekler.
// sistemdeki gorevi: View ve Manage eylemlerini localized olarak sunar.
public partial class VehicleManagementPermissionDefinitionProvider
{
    // Lookup izin agacini tanimlar.
    private static void AddLookupPermissions(PermissionGroupDefinition group)
    {
        var lookups = group.AddPermission(
            VehicleManagementPermissions.Lookups.Default,
            L(VehicleManagementLocalizationKeys.Permissions.Lookups));
        lookups.AddChild(
            VehicleManagementPermissions.Lookups.View,
            L(VehicleManagementLocalizationKeys.Permissions.LookupsView));
        lookups.AddChild(
            VehicleManagementPermissions.Lookups.Manage,
            L(VehicleManagementLocalizationKeys.Permissions.LookupsManage));
    }
}
