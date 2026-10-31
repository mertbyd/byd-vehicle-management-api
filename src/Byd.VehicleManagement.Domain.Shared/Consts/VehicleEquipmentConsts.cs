namespace Byd.VehicleManagement.Consts;

// islevi: VehicleEquipment tablosunun adini, tekillik indeksini ve adet alt sinirini tanimlar.
// sistemdeki gorevi: Entity, EF eslemesi ve validator'in ayni donanim sozlesmesini okumasini saglar.
public static class VehicleEquipmentConsts
{
    public const string TableName = "VehicleEquipments";
    public const string EquipmentTypeIndex = "IX_VehicleEquipments_Vehicle_EquipmentType";

    public const int MinUnits = 1;
}
