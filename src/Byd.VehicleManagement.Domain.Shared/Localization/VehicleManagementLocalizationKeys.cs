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
        public const string VehicleModels = "Permission:VehicleModels";
        public const string VehicleModelsView = "Permission:VehicleModels.View";
        public const string VehicleModelsManage = "Permission:VehicleModels.Manage";
        public const string VehicleTypes = "Permission:VehicleTypes";
        public const string VehicleTypesView = "Permission:VehicleTypes.View";
        public const string VehicleTypesManage = "Permission:VehicleTypes.Manage";
    }

    public static class ActivityLogs
    {
        public const string LookupUpdated = "ActivityLogs:LookupUpdated";
        public const string LookupDeleted = "ActivityLogs:LookupDeleted";
        public const string VehicleModelUpdated = "ActivityLogs:VehicleModelUpdated";
        public const string VehicleModelDeleted = "ActivityLogs:VehicleModelDeleted";
        public const string VehicleTypeUpdated = "ActivityLogs:VehicleTypeUpdated";
        public const string VehicleTypeDeleted = "ActivityLogs:VehicleTypeDeleted";
    }
}
