namespace Byd.VehicleManagement.Permissions;

// islevi: VehicleModel okuma ve yonetim izinlerini tanimlar.
// sistemdeki gorevi: Arac modeli endpointlerinin erisim sozlesmesini sabitler.
public static partial class VehicleManagementPermissions
{
    public static class VehicleModels
    {
        public const string Default = GroupName + ".VehicleModels";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage";
    }
}
