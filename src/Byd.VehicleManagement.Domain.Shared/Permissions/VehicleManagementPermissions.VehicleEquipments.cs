namespace Byd.VehicleManagement.Permissions;

// islevi: VehicleEquipment okuma ve yonetim izinlerini tanimlar.
// sistemdeki gorevi: Arac donanimi endpointlerinin erisim sozlesmesini sabitler.
public static partial class VehicleManagementPermissions
{
    public static class VehicleEquipments
    {
        public const string Default = GroupName + ".VehicleEquipments";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage";
    }
}
