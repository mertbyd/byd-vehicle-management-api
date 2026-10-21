namespace Byd.VehicleManagement.Consts;

// islevi: VehicleModel tablosunun adini, tekillik indeksini ve alan uzunluklarini tanimlar.
// sistemdeki gorevi: Entity, EF eslemesi ve validator'in ayni model sozlesmesini okumasini saglar.
public static class VehicleModelConsts
{
    public const string TableName = "VehicleModels";
    public const string ManufacturerNameIndex = "IX_VehicleModels_Tenant_Manufacturer_Name";

    public const int MaxManufacturerLength = 100;
    public const int MaxNameLength = 100;
}
