namespace Byd.VehicleManagement.ExceptionCodes;

// islevi: Arac alan, iliski, tekillik ve kapsam hatalarinin kodlarini tanimlar.
// sistemdeki gorevi: Arac sozlesmesinin ihlallerini lokalize edilebilir kodlara baglar.
public static class VehicleExceptionCodes
{
    public const string ScopeRequired = VehicleManagementErrorCodes.VehicleGroup + "0001";
    public const string TransportModeRequired = VehicleManagementErrorCodes.VehicleGroup + "0002";
    public const string VehicleStatusRequired = VehicleManagementErrorCodes.VehicleGroup + "0003";
    public const string GtfsVehicleIdRequired = VehicleManagementErrorCodes.VehicleGroup + "0004";
    public const string GtfsVehicleIdTooLong = VehicleManagementErrorCodes.VehicleGroup + "0005";
    public const string LabelTooLong = VehicleManagementErrorCodes.VehicleGroup + "0006";
    public const string RegistrationNumberTooLong = VehicleManagementErrorCodes.VehicleGroup + "0007";
    public const string GtfsAgencyIdTooLong = VehicleManagementErrorCodes.VehicleGroup + "0008";
    public const string InServiceFromRequired = VehicleManagementErrorCodes.VehicleGroup + "0009";
    public const string InServiceUntilBeforeFrom = VehicleManagementErrorCodes.VehicleGroup + "0010";
    public const string VehicleTypeNotFound = VehicleManagementErrorCodes.VehicleGroup + "0011";
    public const string VehicleStatusNotFound = VehicleManagementErrorCodes.VehicleGroup + "0012";
    public const string WheelchairAccessibleNotFound = VehicleManagementErrorCodes.VehicleGroup + "0013";
    public const string GtfsVehicleIdAlreadyExists = VehicleManagementErrorCodes.VehicleGroup + "0014";
    public const string RegistrationNumberAlreadyExists = VehicleManagementErrorCodes.VehicleGroup + "0015";
    public const string TransportModeNotFound = VehicleManagementErrorCodes.VehicleGroup + "0016";
    public const string VehicleTypeTransportModeMismatch = VehicleManagementErrorCodes.VehicleGroup + "0017";
    public const string RegistrationSourceNotFound = VehicleManagementErrorCodes.VehicleGroup + "0018";
}
