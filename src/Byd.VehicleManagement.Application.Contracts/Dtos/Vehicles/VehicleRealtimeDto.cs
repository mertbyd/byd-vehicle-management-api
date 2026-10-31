using System;

namespace Byd.VehicleManagement.Dtos.Vehicles;

// islevi: Realtime eslesmesinin ihtiyac duydugu arac kimligini, baglarini ve adlarini tasir.
// sistemdeki gorevi: Bul-ya da-ekle cagrisinin cevabidir; realtime modulu pozisyonu bu kimlikle araca baglar.
public class VehicleRealtimeDto
{
    // Primary Key
    public Guid Id { get; set; }

    // Foreign Keys
    public Guid? VehicleTypeId { get; set; }
    public int? TransportModeId { get; set; }
    public int VehicleStatusId { get; set; }

    // Table Columns / Data
    public string? VehicleTypeName { get; set; }
    public string? TransportModeName { get; set; }
    public string VehicleStatusName { get; set; } = default!;
    public string GtfsVehicleId { get; set; } = default!;
    public string? Label { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? GtfsAgencyId { get; set; }
}
