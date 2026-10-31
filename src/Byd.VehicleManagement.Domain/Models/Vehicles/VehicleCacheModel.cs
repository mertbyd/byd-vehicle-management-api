using System;
using Byd.CoreExtensions.Interfaces;
using Volo.Abp.Domain.Entities;

namespace Byd.VehicleManagement.Models.Vehicles;

// islevi: Organizasyonun arac listesinde realtime eslesmesinin ihtiyac duydugu alanlari sade model olarak tasir.
// sistemdeki gorevi: Core'un cache'li repository'sinin sakladigi projeksiyondur; her pozisyon mesajinda veritabanina
// gidilmeden araci isletmecisi ve GTFS-RT kimligiyle bulmayi saglar.
public class VehicleCacheModel : ICacheModel, IEntity<Guid>
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

    // IEntity sozlesmesi icin kimligi anahtar dizisi olarak verir.
    public object?[] GetKeys() => [Id];

    // IKeyedObject sozlesmesi icin kimligi metin anahtar olarak verir.
    public string? GetObjectKey() => Id.ToString();
}
