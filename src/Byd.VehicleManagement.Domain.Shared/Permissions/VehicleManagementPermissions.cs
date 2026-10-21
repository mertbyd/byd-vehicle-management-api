using Volo.Abp.Reflection;

namespace Byd.VehicleManagement.Permissions;

// islevi: Modulun izin grubunu ve tum izin sabitlerini toplayan kok parcadir.
// sistemdeki gorevi: Ozellik bazli izin parcalarinin ortak grup adini saglar.
public static partial class VehicleManagementPermissions
{
    public const string GroupName = "VehicleManagement";

    public static string[] GetAll() =>
        ReflectionHelper.GetPublicConstantsRecursively(typeof(VehicleManagementPermissions));
}
