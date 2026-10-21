using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Byd.VehicleManagement.Entities.VehicleModels;

// islevi: Ureticinin arac modelini tenant katalogu olarak saklar.
// sistemdeki gorevi: Tenant icindeki tum organizasyonlarin paylastigi katalog kaydidir; arac tipleri bu kayda baglanir.
// Kapasite, kip ve erisilebilirlik modelde degil tipte durur, cunku ayni model farkli yapilandirmalarla alinabilir.
public class VehicleModel : AuditedAggregateRoot<Guid>, IMultiTenant
{
    // Katalogun ait oldugu tenant; tenant icindeki tum organizasyonlar ayni kaydi kullanir.
    public Guid? TenantId { get; internal set; }

    // Aracin ureticisi.
    public string Manufacturer { get; internal set; } = default!;

    // Ureticinin model adi; ayni tenant'ta uretici ile birlikte tekildir.
    public string Name { get; internal set; } = default!;

    // EF materialization icin bos kabuk kurar.
    protected VehicleModel()
    {
    }

    // Manager'in urettigi kimlik ve istegin tenant'iyla bos katalog kaydini kurar; duzenlenebilir alanlari Manager doldurur.
    internal VehicleModel(Guid id, Guid tenantId) : base(id)
    {
        TenantId = tenantId;
    }
}
