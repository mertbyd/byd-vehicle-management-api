using System;
using Byd.VehicleManagement.Consts;
using Volo.Abp.EventBus;

namespace Byd.VehicleManagement.Etos.Vehicles;

// islevi: Arac eklendiginde, guncellendiginde veya silindiginde ABP entity olayinin tasidigi arac kimligidir.
// sistemdeki gorevi: Diger modullerin akistaki GTFS-RT kimligini arac kaydina eslemesini saglar.
[EventName(VehicleEventNames.Vehicle)]
public class VehicleEto
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? OrganizationId { get; set; }
    public string? GtfsAgencyId { get; set; }
    public string GtfsVehicleId { get; set; } = default!;
}
