using System;

namespace Byd.VehicleManagement.Models.VehicleTypes;

// islevi: Arac tipinin duzenlenebilir iliski ve olcu alanlarini tasir.
// sistemdeki gorevi: Olusturma ve guncellemenin ayni dogrulama ve normalizasyon yolundan gecmesini saglar.
public abstract class VehicleTypeInputModel
{
    public Guid? VehicleModelId { get; set; }
    public int TransportModeId { get; set; }
    public int FuelTypeId { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int SeatingCapacity { get; set; }
    public int StandingCapacity { get; set; }
    public int WheelchairPlaceCapacity { get; set; }
    public int BicycleCapacity { get; set; }
    public bool LowFloor { get; set; }
    public bool HasLiftOrRamp { get; set; }
    public decimal? Length { get; set; }
}
