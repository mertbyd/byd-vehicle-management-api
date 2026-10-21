using System;

namespace Byd.VehicleManagement.Models.Vehicles;

// islevi: Aracin duzenlenebilir baglarini ve alanlarini tasir.
// sistemdeki gorevi: Olusturma ve guncellemenin ayni dogrulama ve normalizasyon yolundan gecmesini saglar.
public abstract class VehicleInputModel
{
    public Guid? VehicleTypeId { get; set; }
    public int TransportModeId { get; set; }
    public int VehicleStatusId { get; set; }
    public int? WheelchairAccessibleId { get; set; }
    public string GtfsVehicleId { get; set; } = default!;
    public string? Label { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? GtfsAgencyId { get; set; }
    public DateOnly InServiceFrom { get; set; }
    public DateOnly? InServiceUntil { get; set; }
}
