using System;
using System.Threading;
using System.Threading.Tasks;
using Byd.CoreExtensions.Interfaces;
using Byd.CoreExtensions.Managers;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Entities.VehicleEquipments;
using Byd.VehicleManagement.ExceptionCodes;
using Byd.VehicleManagement.Interfaces.Lookups;
using Byd.VehicleManagement.Interfaces.VehicleEquipments;
using Byd.VehicleManagement.Interfaces.Vehicles;
using Byd.VehicleManagement.Models.VehicleEquipments;
using Volo.Abp;

namespace Byd.VehicleManagement.Managers.VehicleEquipments;

// islevi: Arac donaniminin kapsamini, arac ve donanim turu baglarini ve arac icindeki tur tekilligini yonetir.
// sistemdeki gorevi: Donanim is kurallarinin tek sahibidir. Arac istegin organizasyonunda aranir; core'un global
// filtresi baska organizasyonun aracini gostermez.
public class VehicleEquipmentManager(
    IVehicleEquipmentRepository repository,
    IVehicleRepository vehicleRepository,
    IVehicleLookupRepository<EquipmentType> equipmentTypeRepository)
    : BaseManager<VehicleEquipment, Guid>(repository)
{
    private ICurrentOrganization CurrentOrganization =>
        LazyServiceProvider.LazyGetRequiredService<ICurrentOrganization>();

    // Istegin organizasyonundaki araca yeni donanim kurar; kapsamsiz istekte kimsenin goremeyecegi satir yazilmaz.
    public virtual async Task<VehicleEquipment> CreateAsync(
        VehicleEquipmentCreateModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (CurrentTenant.Id is not { } tenantId || CurrentOrganization.Id is not { } organizationId)
            throw new BusinessException(VehicleEquipmentExceptionCodes.ScopeRequired);
        var entity = new VehicleEquipment(GuidGenerator.Create(), tenantId, organizationId);
        entity.Vehicle = await vehicleRepository.FindAsync(
                value => value.Id == model.VehicleId, cancellationToken: cancellationToken)
            ?? throw new BusinessException(VehicleEquipmentExceptionCodes.VehicleNotFound);
        entity.VehicleId = entity.Vehicle.Id;
        await ApplyAsync(entity, model, cancellationToken);
        return entity;
    }

    // Donanimin turunu ve adedini ayni kurallardan gecirerek gunceller; arac degismez.
    public virtual Task UpdateAsync(
        VehicleEquipment entity, VehicleEquipmentUpdateModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(model);
        return ApplyAsync(entity, model, cancellationToken);
    }

    // Donanim turunu bulur, adedi yazar ve turun aracta tekil oldugunu kontrol eder.
    private async Task ApplyAsync(
        VehicleEquipment entity, VehicleEquipmentInputModel model, CancellationToken cancellationToken = default)
    {
        entity.EquipmentType = await equipmentTypeRepository.FindAsync(
                value => value.Id == model.EquipmentTypeId, cancellationToken: cancellationToken)
            ?? throw new BusinessException(VehicleEquipmentExceptionCodes.EquipmentTypeNotFound);
        entity.EquipmentTypeId = entity.EquipmentType.Id;
        entity.Units = model.Units;
        var vehicleId = entity.VehicleId;
        var equipmentTypeId = entity.EquipmentTypeId;
        await CheckAlreadyExistsAsync(
            value => value.VehicleId == vehicleId && value.EquipmentTypeId == equipmentTypeId, entity.Id,
            cancellationToken);
    }
}
