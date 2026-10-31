namespace Byd.VehicleManagement.Permissions;

// islevi: Vehicle okuma ve yonetim izinlerini tanimlar.
// sistemdeki gorevi: Arac endpointlerinin erisim sozlesmesini sabitler.
public static partial class VehicleManagementPermissions
{
    public static class Vehicles
    {
        public const string Default = GroupName + ".Vehicles";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage";
    }
}
