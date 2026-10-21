using System;

namespace Byd.VehicleManagement.Dtos.VehicleTypes;

// islevi: Arac tipinin baglarini, bagli kayitlarin adlarini, kapasite ve erisilebilirlik alanlarini tasir.
// sistemdeki gorevi: Persistence entity'sini public cevap sozlesmesinden ayirir; istemci model, mod ve yakit adini
// ikinci bir istek atmadan gosterir.
public class VehicleTypeDto
{
    // Primary Key
    public Guid Id { get; set; }

    // Foreign Keys
    public Guid? VehicleModelId { get; set; }
    public int TransportModeId { get; set; }
    public int FuelTypeId { get; set; }

    // Table Columns / Data
    public string? VehicleModelName { get; set; }
    public string TransportModeName { get; set; } = default!;
    public string FuelTypeName { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int SeatingCapacity { get; set; }
    public int StandingCapacity { get; set; }
    public int WheelchairPlaceCapacity { get; set; }
    public int BicycleCapacity { get; set; }
    public bool LowFloor { get; set; }
    public bool HasLiftOrRamp { get; set; }
    public decimal? Length { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
}
