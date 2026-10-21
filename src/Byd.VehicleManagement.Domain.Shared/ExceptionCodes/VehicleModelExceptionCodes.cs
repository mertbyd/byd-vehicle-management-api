namespace Byd.VehicleManagement.ExceptionCodes;

// islevi: Arac modeli alan ve kapsam hatalarinin kodlarini tanimlar.
// sistemdeki gorevi: Model sozlesmesinin ihlallerini lokalize edilebilir kodlara baglar.
public static class VehicleModelExceptionCodes
{
    public const string TenantRequired = VehicleManagementErrorCodes.VehicleModelGroup + "0001";
    public const string ManufacturerRequired = VehicleManagementErrorCodes.VehicleModelGroup + "0002";
    public const string ManufacturerTooLong = VehicleManagementErrorCodes.VehicleModelGroup + "0003";
    public const string NameRequired = VehicleManagementErrorCodes.VehicleModelGroup + "0004";
    public const string NameTooLong = VehicleManagementErrorCodes.VehicleModelGroup + "0005";
}
