namespace Byd.VehicleManagement.ExceptionCodes;

// islevi: Arac tipi alan, iliski ve kapsam hatalarinin kodlarini tanimlar.
// sistemdeki gorevi: Tip sozlesmesinin ihlallerini lokalize edilebilir kodlara baglar.
public static class VehicleTypeExceptionCodes
{
    public const string ScopeRequired = VehicleManagementErrorCodes.VehicleTypeGroup + "0001";
    public const string CodeRequired = VehicleManagementErrorCodes.VehicleTypeGroup + "0002";
    public const string CodeTooLong = VehicleManagementErrorCodes.VehicleTypeGroup + "0003";
    public const string NameRequired = VehicleManagementErrorCodes.VehicleTypeGroup + "0004";
    public const string NameTooLong = VehicleManagementErrorCodes.VehicleTypeGroup + "0005";
    public const string TransportModeRequired = VehicleManagementErrorCodes.VehicleTypeGroup + "0006";
    public const string FuelTypeRequired = VehicleManagementErrorCodes.VehicleTypeGroup + "0007";
    public const string SeatingCapacityInvalid = VehicleManagementErrorCodes.VehicleTypeGroup + "0008";
    public const string StandingCapacityInvalid = VehicleManagementErrorCodes.VehicleTypeGroup + "0009";
    public const string WheelchairPlaceCapacityInvalid = VehicleManagementErrorCodes.VehicleTypeGroup + "0010";
    public const string BicycleCapacityInvalid = VehicleManagementErrorCodes.VehicleTypeGroup + "0011";
    public const string LengthInvalid = VehicleManagementErrorCodes.VehicleTypeGroup + "0012";
    public const string VehicleModelNotFound = VehicleManagementErrorCodes.VehicleTypeGroup + "0013";
    public const string TransportModeNotFound = VehicleManagementErrorCodes.VehicleTypeGroup + "0014";
    public const string FuelTypeNotFound = VehicleManagementErrorCodes.VehicleTypeGroup + "0015";
}
