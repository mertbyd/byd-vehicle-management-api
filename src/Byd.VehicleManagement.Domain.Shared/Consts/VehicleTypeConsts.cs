namespace Byd.VehicleManagement.Consts;

// islevi: VehicleType tablosunun adini, tekillik indeksini ve alan uzunluklarini tanimlar.
// sistemdeki gorevi: Entity, EF eslemesi ve validator'in ayni arac tipi sozlesmesini okumasini saglar.
public static class VehicleTypeConsts
{
    public const string TableName = "VehicleTypes";
    public const string CodeIndex = "IX_VehicleTypes_Scope_Code";

    public const int MaxCodeLength = 50;
    public const int MaxNameLength = 200;
}
