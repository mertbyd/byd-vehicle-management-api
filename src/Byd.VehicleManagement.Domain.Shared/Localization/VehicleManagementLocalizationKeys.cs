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
        public const string Vehicles = "Permission:Vehicles";
        public const string VehiclesView = "Permission:Vehicles.View";
        public const string VehiclesManage = "Permission:Vehicles.Manage";
        public const string VehicleEquipments = "Permission:VehicleEquipments";
        public const string VehicleEquipmentsView = "Permission:VehicleEquipments.View";
        public const string VehicleEquipmentsManage = "Permission:VehicleEquipments.Manage";
    }

    public static class ActivityLogs
    {
        public const string LookupUpdated = "ActivityLogs:LookupUpdated";
        public const string LookupDeleted = "ActivityLogs:LookupDeleted";
        public const string VehicleModelUpdated = "ActivityLogs:VehicleModelUpdated";
        public const string VehicleModelDeleted = "ActivityLogs:VehicleModelDeleted";
        public const string VehicleTypeUpdated = "ActivityLogs:VehicleTypeUpdated";
        public const string VehicleTypeDeleted = "ActivityLogs:VehicleTypeDeleted";
        public const string VehicleUpdated = "ActivityLogs:VehicleUpdated";
        public const string VehicleDeleted = "ActivityLogs:VehicleDeleted";
        public const string VehicleEquipmentUpdated = "ActivityLogs:VehicleEquipmentUpdated";
        public const string VehicleEquipmentDeleted = "ActivityLogs:VehicleEquipmentDeleted";
    }
}
