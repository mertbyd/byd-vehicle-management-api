namespace Byd.VehicleManagement.Localization;

// islevi: Modulun izin ve activity log metin anahtarlarini tanimlar.
// sistemdeki gorevi: Gorunen metinlerin kodda daginik string olarak yazilmasini onler.
public static class VehicleManagementLocalizationKeys
{
    public static class Permissions
    {
        public const string Group = "Permission:VehicleManagement";
        public const string Lookups = "Permission:Lookups";
        public const string LookupsView = "Permission:Lookups.View";
        public const string LookupsManage = "Permission:Lookups.Manage";
    }

    public static class ActivityLogs
    {
        public const string LookupUpdated = "ActivityLogs:LookupUpdated";
        public const string LookupDeleted = "ActivityLogs:LookupDeleted";
    }
}
