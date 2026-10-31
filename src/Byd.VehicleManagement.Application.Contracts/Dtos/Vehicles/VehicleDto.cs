using System;

namespace Byd.VehicleManagement.Dtos.Vehicles;

// islevi: Aracin baglarini, bagli kayitlarin adlarini, GTFS-RT kimliklerini ve hizmet donemini tasir.
// sistemdeki gorevi: Persistence entity'sini public cevap sozlesmesinden ayirir; istemci tip, kip, durum ve kaynak
// adlarini ikinci bir istek atmadan gosterir.
public class VehicleDto
{
    // Primary Key
    public Guid Id { get; set; }

    // Foreign Keys
    public Guid? VehicleTypeId { get; set; }
    public int? TransportModeId { get; set; }
    public int VehicleStatusId { get; set; }
    public int RegistrationSourceId { get; set; }
    public int? WheelchairAccessibleId { get; set; }

    // Table Columns / Data
    public string? VehicleTypeName { get; set; }
    public string? TransportModeName { get; set; }
    public string VehicleStatusName { get; set; } = default!;
    public string RegistrationSourceName { get; set; } = default!;
    public string? WheelchairAccessibleName { get; set; }
    public string GtfsVehicleId { get; set; } = default!;
    public string? Label { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? GtfsAgencyId { get; set; }
    public DateOnly InServiceFrom { get; set; }
    public DateOnly? InServiceUntil { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
}
