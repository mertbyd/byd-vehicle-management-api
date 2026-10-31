using System;
using Byd.CoreExtensions.Interfaces;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Entities.Vehicles;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Byd.VehicleManagement.Entities.VehicleEquipments;

// islevi: Bir aracta takili donanim turunu ve adedini saklar.
// sistemdeki gorevi: Kendi CRUD akisi olan kayittir; arac basina her donanim turu bir kez bulunur ve arac silinince
// aracla birlikte silinir.
public class VehicleEquipment : AuditedAggregateRoot<Guid>, IMultiTenant, IMultiOrganization
{
    // Kaydin ait oldugu tenant; aracin tenant'idir.
    public Guid? TenantId { get; internal set; }

    // Kaydin ait oldugu organizasyon; aracin organizasyonudur.
    public Guid? OrganizationId { get; internal set; }

    // Donanimin takili oldugu arac; kayit olustuktan sonra degismez.
    public Guid VehicleId { get; internal set; }

    // Donanim turu; ayni arac icin tekildir.
    public int EquipmentTypeId { get; internal set; }

    // Aractaki adet.
    public int Units { get; internal set; }

    // Donanimin takili oldugu arac.
    public Vehicle Vehicle { get; internal set; } = default!;
    // Donanim turu satiri.
    public EquipmentType EquipmentType { get; internal set; } = default!;

    // EF materialization icin bos kabuk kurar.
    protected VehicleEquipment()
    {
    }

    // Manager'in urettigi kimlik ve istegin kapsamiyla bos donanim kaydini kurar; arac, tur ve adedi Manager doldurur.
    internal VehicleEquipment(Guid id, Guid tenantId, Guid organizationId) : base(id)
    {
        TenantId = tenantId;
        OrganizationId = organizationId;
    }
}
