namespace Byd.VehicleManagement.Consts;

// islevi: Vehicle filtre yuzeyinde disariya acilan alan adlarini tanimlar.
// sistemdeki gorevi: Filtre DTO'su ile repository sorgu motoru arasindaki kararli alan adi sozlesmesidir.
public static class VehicleFilterFields
{
    public const string VehicleTypeId = "vehicleTypeId";
    public const string TransportModeId = "transportModeId";
    public const string VehicleStatusId = "vehicleStatusId";
    public const string RegistrationSourceId = "registrationSourceId";
    public const string GtfsVehicleId = "gtfsVehicleId";
    public const string Label = "label";
    public const string RegistrationNumber = "registrationNumber";
    public const string GtfsAgencyId = "gtfsAgencyId";
    public const string InServiceFrom = "inServiceFrom";
    public const string CreationTime = "creationTime";

    public static class PropertyPaths
    {
        public const string VehicleTypeId = "VehicleTypeId";
        public const string TransportModeId = "TransportModeId";
        public const string VehicleStatusId = "VehicleStatusId";
        public const string RegistrationSourceId = "RegistrationSourceId";
        public const string GtfsVehicleId = "GtfsVehicleId";
        public const string Label = "Label";
        public const string RegistrationNumber = "RegistrationNumber";
        public const string GtfsAgencyId = "GtfsAgencyId";
        public const string InServiceFrom = "InServiceFrom";
        public const string CreationTime = "CreationTime";
    }
}
