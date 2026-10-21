using Byd.VehicleManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Byd.VehicleManagement.Permissions;

// islevi: Arac yonetimi izin grubunu ve her ozelligin izin agacini ABP'ye tanitir.
// sistemdeki gorevi: Ozellik izinleri ayri partial dosyalarda tanimlanir; bu dosya grubu kurar ve sirayla ekler.
public partial class VehicleManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(
            VehicleManagementPermissions.GroupName,
            L(VehicleManagementLocalizationKeys.Permissions.Group));
        AddLookupPermissions(group);
        AddVehicleModelPermissions(group);
        AddVehicleTypePermissions(group);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VehicleManagementResource>(name);
    }
}
