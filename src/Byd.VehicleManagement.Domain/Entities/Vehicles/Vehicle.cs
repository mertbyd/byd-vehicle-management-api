using System;
using Byd.CoreExtensions.Interfaces;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Entities.VehicleTypes;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Byd.VehicleManagement.Entities.Vehicles;

// islevi: Bir organizasyonun filosundaki tek bir araci; GTFS-RT kimligini, kipini, numarasini, plakasini, durumunu,
// kaynagini ve hizmet donemini saklar.
// sistemdeki gorevi: Arac aggregate'inin kokudur; donanimlar kendi kayitlarinda tutulur ve aracla birlikte silinir.
// Realtime'dan otomatik eklenen aracin tipi henuz bilinmez; kip feed'den gelir, tip atanana kadar model ve kapasite bos kalir.
public class Vehicle : AuditedAggregateRoot<Guid>, IMultiTenant, IMultiOrganization
{
    // Kaydin ait oldugu tenant.
    public Guid? TenantId { get; internal set; }

    // Araci isleten organizasyon; arac yalniz bu organizasyonda gorunur.
    public Guid? OrganizationId { get; internal set; }

    // Aracin yapilandirmasi; bos ise realtime'dan eklenmis ve tipi henuz atanmamistir. Ayni organizasyonun ve ayni kipin
    // tipi olmak zorundadir.
    public Guid? VehicleTypeId { get; internal set; }

    // Aracin ulasim modu; realtime'da feed'in kipinden, elle eklemede secimden gelir.
    public int TransportModeId { get; internal set; }

    // Aracin hizmet durumu.
    public int VehicleStatusId { get; internal set; }

    // Aracin sisteme nereden geldigi (realtime veya elle).
    public int RegistrationSourceId { get; internal set; }

    // Tekerlekli sandalye erisimi; yalniz tipten farkli oldugunda dolu (GTFS-RT wheelchair_accessible).
    public int? WheelchairAccessibleId { get; internal set; }

    // GTFS-RT vehicle.id; organizasyonda tekildir ve realtime eslesmesinin anahtaridir.
    public string GtfsVehicleId { get; internal set; } = default!;

    // Yolcunun gordugu arac numarasi (GTFS-RT label).
    public string? Label { get; internal set; }

    // Plaka (GTFS-RT license_plate); tenant icinde tekildir.
    public string? RegistrationNumber { get; internal set; }

    // Araci isleten GTFS ajansinin agency_id degeri.
    public string? GtfsAgencyId { get; internal set; }

    // Aracin hizmete girdigi gun.
    public DateOnly InServiceFrom { get; internal set; }

    // Aracin hizmetten ciktigi gun; hizmetteyse bos.
    public DateOnly? InServiceUntil { get; internal set; }

    // Aracin tipi.
    public VehicleType? VehicleType { get; internal set; }
    // Aracin ulasim modu satiri.
    public TransportMode TransportMode { get; internal set; } = default!;
    // Aracin hizmet durumu satiri.
    public VehicleStatus VehicleStatus { get; internal set; } = default!;
    // Aracin kayit kaynagi satiri.
    public RegistrationSource RegistrationSource { get; internal set; } = default!;
    // Aracin tekerlekli sandalye erisimi satiri.
    public WheelchairAccessible? WheelchairAccessible { get; internal set; }

    // EF materialization icin bos kabuk kurar.
    protected Vehicle()
    {
    }

    // Manager'in urettigi kimlik ve istegin kapsamiyla bos arac kaydini kurar; duzenlenebilir alanlari Manager doldurur.
    internal Vehicle(Guid id, Guid tenantId, Guid organizationId) : base(id)
    {
        TenantId = tenantId;
        OrganizationId = organizationId;
    }
}
