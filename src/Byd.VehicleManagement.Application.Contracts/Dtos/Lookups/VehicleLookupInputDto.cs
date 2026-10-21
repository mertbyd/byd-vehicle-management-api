namespace Byd.VehicleManagement.Dtos.Lookups;

// islevi: Arac lookup satirinin istemciden gelen duzenlenebilir alanlarini tasir.
// sistemdeki gorevi: Olusturma ve guncelleme isteklerinin ortak alan sozlesmesidir.
public abstract class VehicleLookupInputDto
{
    // Table Columns / Data
    public int Code { get; set; }
    public string Name { get; set; } = default!;
}
