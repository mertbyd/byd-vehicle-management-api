namespace Byd.VehicleManagement.Permissions;

// islevi: VehicleType okuma ve yonetim izinlerini tanimlar.
// sistemdeki gorevi: Arac tipi endpointlerinin erisim sozlesmesini sabitler.
public static partial class VehicleManagementPermissions
{
    public static class VehicleTypes
    {
        public const string Default = GroupName + ".VehicleTypes";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage";
    }
}
