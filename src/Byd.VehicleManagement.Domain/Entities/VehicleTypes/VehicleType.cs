using System;
using Byd.CoreExtensions.Interfaces;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Entities.VehicleModels;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Byd.VehicleManagement.Entities.VehicleTypes;

// islevi: Bir organizasyonun arac yapilandirmasini; kipini, yakitini, kapasitesini ve erisilebilirligini saklar.
// sistemdeki gorevi: Araclarin kapasite ve kip bilgisini tek yerden okudugu kayittir. Uretici modeline istege bagli
// baglanir; bir modelin birden cok tipi olabilir (ayni model, farkli koltuk duzeni).
public class VehicleType : AuditedAggregateRoot<Guid>, IMultiTenant, IMultiOrganization
{
    // Kaydin ait oldugu tenant.
    public Guid? TenantId { get; internal set; }

    // Tipi tanimlayan organizasyon; tip yalniz bu organizasyonda gorunur.
    public Guid? OrganizationId { get; internal set; }

    // Tipin yapilandirmasi oldugu uretici modeli; bos ise model bilgisi girilmemistir.
    public Guid? VehicleModelId { get; internal set; }

    // Tipin ulasim modu; kodu GTFS route_type ile aynidir.
    public int TransportModeId { get; internal set; }

    // Tipin yakit veya tahrik turu.
    public int FuelTypeId { get; internal set; }

    // Organizasyonda tipi tekil tanimlayan kisa kod.
    public string Code { get; internal set; } = default!;

    // Tipin gorunen adi.
    public string Name { get; internal set; } = default!;

    // Oturan yolcu kapasitesi.
    public int SeatingCapacity { get; internal set; }

    // Ayakta yolcu kapasitesi; GTFS-RT doluluk yuzdesinde %100 oturan ve ayakta kapasitenin toplamidir.
    public int StandingCapacity { get; internal set; }

    // Tekerlekli sandalye yeri sayisi.
    public int WheelchairPlaceCapacity { get; internal set; }

    // Bisiklet yeri sayisi.
    public int BicycleCapacity { get; internal set; }

    // Aracin alcak tabanli olup olmadigi.
    public bool LowFloor { get; internal set; }

    // Aracta rampa veya lift bulunup bulunmadigi.
    public bool HasLiftOrRamp { get; internal set; }

    // Arac uzunlugu, metre; bilinmiyorsa bos.
    public decimal? Length { get; internal set; }

    // Tipin bagli oldugu uretici modeli.
    public VehicleModel? VehicleModel { get; internal set; }
    // Tipin ulasim modu satiri.
    public TransportMode TransportMode { get; internal set; } = default!;
    // Tipin yakit turu satiri.
    public FuelType FuelType { get; internal set; } = default!;

    // EF materialization icin bos kabuk kurar.
    protected VehicleType()
    {
    }

    // Manager'in urettigi kimlik ve istegin kapsamiyla bos tip kaydini kurar; duzenlenebilir alanlari Manager doldurur.
    internal VehicleType(Guid id, Guid tenantId, Guid organizationId) : base(id)
    {
        TenantId = tenantId;
        OrganizationId = organizationId;
    }
}
