using System;

namespace Byd.VehicleManagement.Dtos.VehicleTypes;

// islevi: Arac tipinin istemciden gelen duzenlenebilir baglarini ve alanlarini tasir.
// sistemdeki gorevi: Olusturma ve guncelleme isteklerinin ortak alan sozlesmesidir; tenant ve organizasyon istekten
// alinmaz.
public abstract class VehicleTypeInputDto
{
    // Foreign Keys
    public Guid? VehicleModelId { get; set; }
    public int TransportModeId { get; set; }
    public int FuelTypeId { get; set; }

    // Table Columns / Data
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
