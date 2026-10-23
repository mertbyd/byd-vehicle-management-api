using System;
using Byd.VehicleManagement.Consts;
using Volo.Abp.EventBus;

namespace Byd.VehicleManagement.Etos.Vehicles;

// islevi: Realtime akisinda gorulen ve kayitli olmayan aracin GTFS-RT VehicleDescriptor bilgisini tasir.
// sistemdeki gorevi: Arac modulunun araci kendi kapsaminda realtime kaynakli olarak kaydetmesini ister.
[EventName(VehicleEventNames.RegistrationRequested)]
public class VehicleRegistrationRequestedEto
{
    public Guid TenantId { get; set; }
    public Guid OrganizationId { get; set; }
    public string GtfsVehicleId { get; set; } = default!;
    public string? GtfsAgencyId { get; set; }
    public string? Label { get; set; }
    public string? RegistrationNumber { get; set; }
    public int? RouteType { get; set; }
    public int? WheelchairAccessible { get; set; }
}
