using System;

namespace Byd.VehicleManagement.Dtos.Vehicles;

// islevi: Aracin istemciden gelen duzenlenebilir baglarini ve alanlarini tasir.
// sistemdeki gorevi: Olusturma ve guncelleme isteklerinin ortak alan sozlesmesidir; tenant, organizasyon ve kayit
// kaynagi istekten alinmaz.
public abstract class VehicleInputDto
{
    // Foreign Keys
    public Guid? VehicleTypeId { get; set; }
    public int TransportModeId { get; set; }
    public int VehicleStatusId { get; set; }
    public int? WheelchairAccessibleId { get; set; }

    // Table Columns / Data
    public string GtfsVehicleId { get; set; } = default!;
    public string? Label { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? GtfsAgencyId { get; set; }
    public DateOnly InServiceFrom { get; set; }
    public DateOnly? InServiceUntil { get; set; }
}
