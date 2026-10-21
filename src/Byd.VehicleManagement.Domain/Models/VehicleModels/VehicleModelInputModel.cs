namespace Byd.VehicleManagement.Models.VehicleModels;

// islevi: Arac modelinin duzenlenebilir alanlarini tasir.
// sistemdeki gorevi: Olusturma ve guncellemenin ayni normalizasyon yolundan gecmesini saglar.
public abstract class VehicleModelInputModel
{
    public string Manufacturer { get; set; } = default!;
    public string Name { get; set; } = default!;
}
