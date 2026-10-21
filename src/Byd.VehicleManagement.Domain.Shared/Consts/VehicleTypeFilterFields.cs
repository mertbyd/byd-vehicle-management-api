namespace Byd.VehicleManagement.Consts;

// islevi: VehicleType filtre yuzeyinde disariya acilan alan adlarini tanimlar.
// sistemdeki gorevi: Filtre DTO'su ile repository sorgu motoru arasindaki kararli alan adi sozlesmesidir.
public static class VehicleTypeFilterFields
{
    public const string VehicleModelId = "vehicleModelId";
    public const string TransportModeId = "transportModeId";
    public const string FuelTypeId = "fuelTypeId";
    public const string Code = "code";
    public const string Name = "name";
    public const string LowFloor = "lowFloor";
    public const string CreationTime = "creationTime";

    public static class PropertyPaths
    {
        public const string VehicleModelId = "VehicleModelId";
        public const string TransportModeId = "TransportModeId";
        public const string FuelTypeId = "FuelTypeId";
        public const string Code = "Code";
        public const string Name = "Name";
        public const string LowFloor = "LowFloor";
        public const string CreationTime = "CreationTime";
    }
}
