namespace Byd.VehicleManagement.Consts;

// islevi: VehicleEquipment filtre yuzeyinde disariya acilan alan adlarini tanimlar.
// sistemdeki gorevi: Filtre DTO'su ile repository sorgu motoru arasindaki kararli alan adi sozlesmesidir.
public static class VehicleEquipmentFilterFields
{
    public const string VehicleId = "vehicleId";
    public const string EquipmentTypeId = "equipmentTypeId";
    public const string Units = "units";
    public const string CreationTime = "creationTime";

    public static class PropertyPaths
    {
        public const string VehicleId = "VehicleId";
        public const string EquipmentTypeId = "EquipmentTypeId";
        public const string Units = "Units";
        public const string CreationTime = "CreationTime";
    }
}
