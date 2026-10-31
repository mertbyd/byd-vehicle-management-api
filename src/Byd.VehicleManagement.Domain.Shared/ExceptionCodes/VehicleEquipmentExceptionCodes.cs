namespace Byd.VehicleManagement.ExceptionCodes;

// islevi: Arac donanimi alan, iliski ve kapsam hatalarinin kodlarini tanimlar.
// sistemdeki gorevi: Donanim sozlesmesinin ihlallerini lokalize edilebilir kodlara baglar.
public static class VehicleEquipmentExceptionCodes
{
    public const string ScopeRequired = VehicleManagementErrorCodes.VehicleEquipmentGroup + "0001";
    public const string VehicleRequired = VehicleManagementErrorCodes.VehicleEquipmentGroup + "0002";
    public const string EquipmentTypeRequired = VehicleManagementErrorCodes.VehicleEquipmentGroup + "0003";
    public const string UnitsInvalid = VehicleManagementErrorCodes.VehicleEquipmentGroup + "0004";
    public const string VehicleNotFound = VehicleManagementErrorCodes.VehicleEquipmentGroup + "0005";
    public const string EquipmentTypeNotFound = VehicleManagementErrorCodes.VehicleEquipmentGroup + "0006";
}
