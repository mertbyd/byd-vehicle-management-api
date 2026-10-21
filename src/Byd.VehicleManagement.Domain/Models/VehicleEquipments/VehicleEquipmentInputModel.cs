namespace Byd.VehicleManagement.Models.VehicleEquipments;

// islevi: Arac donaniminin duzenlenebilir tur ve adet alanlarini tasir.
// sistemdeki gorevi: Olusturma ve guncellemenin ayni dogrulama yolundan gecmesini saglar.
public abstract class VehicleEquipmentInputModel
{
    public int EquipmentTypeId { get; set; }
    public int Units { get; set; }
}
