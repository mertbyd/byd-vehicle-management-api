namespace Byd.VehicleManagement.Dtos.VehicleEquipments;

// islevi: Arac donaniminin istemciden gelen duzenlenebilir tur ve adet alanlarini tasir.
// sistemdeki gorevi: Olusturma ve guncelleme isteklerinin ortak alan sozlesmesidir; tenant ve organizasyon istekten
// alinmaz.
public abstract class VehicleEquipmentInputDto
{
    // Foreign Keys
    public int EquipmentTypeId { get; set; }

    // Table Columns / Data
    public int Units { get; set; }
}
