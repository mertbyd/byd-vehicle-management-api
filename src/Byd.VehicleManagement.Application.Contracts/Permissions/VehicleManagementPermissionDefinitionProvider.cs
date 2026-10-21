using Byd.VehicleManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Byd.VehicleManagement.Permissions;

public class VehicleManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        // var myGroup = context.AddGroup(VehicleManagementPermissions.GroupName, L("Permission:VehicleManagement"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VehicleManagementResource>(name);
    }
}
