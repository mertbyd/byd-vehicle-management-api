namespace Byd.VehicleManagement.Dtos.VehicleModels;

// islevi: Arac modelinin istemciden gelen duzenlenebilir alanlarini tasir.
// sistemdeki gorevi: Olusturma ve guncelleme isteklerinin ortak alan sozlesmesidir; tenant istekten alinmaz.
public abstract class VehicleModelInputDto
{
    // Table Columns / Data
    public string Manufacturer { get; set; } = default!;
    public string Name { get; set; } = default!;
}
