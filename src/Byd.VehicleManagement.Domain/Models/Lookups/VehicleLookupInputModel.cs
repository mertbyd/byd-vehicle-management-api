namespace Byd.VehicleManagement.Models.Lookups;

// islevi: Lookup olusturma ve guncelleme isteklerinin domain tarafindaki ortak alanlarini tasir.
// sistemdeki gorevi: Manager'in DTO'ya bagimli olmadan lookup kurallarini uygulamasini saglar.
public abstract class VehicleLookupInputModel
{
    public int Code { get; set; }
    public string Name { get; set; } = default!;
}
