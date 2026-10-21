namespace Byd.VehicleManagement.Permissions;

// islevi: Arac lookup okuma ve yonetim izinlerini tanimlar.
// sistemdeki gorevi: Lookup endpointlerinin erisim sozlesmesini sabitler.
public static partial class VehicleManagementPermissions
{
    public static class Lookups
    {
        public const string Default = GroupName + ".Lookups";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage";
    }
}
