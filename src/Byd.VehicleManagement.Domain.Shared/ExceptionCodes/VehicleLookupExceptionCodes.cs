namespace Byd.VehicleManagement.ExceptionCodes;

// islevi: Arac lookup alan ve is kurali hatalarinin kodlarini tanimlar.
// sistemdeki gorevi: Lookup sozlesmesinin ihlallerini lokalize edilebilir kodlara baglar.
public static class VehicleLookupExceptionCodes
{
    public const string NameRequired = VehicleManagementErrorCodes.LookupGroup + "0001";
    public const string CodeInvalid = VehicleManagementErrorCodes.LookupGroup + "0002";
    public const string CodeAlreadyExists = VehicleManagementErrorCodes.LookupGroup + "0003";
}
