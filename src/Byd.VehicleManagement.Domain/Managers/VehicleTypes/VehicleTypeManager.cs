using System;
using System.Threading;
using System.Threading.Tasks;
using Byd.CoreExtensions.ExceptionCodes;
using Byd.CoreExtensions.Interfaces;
using Byd.CoreExtensions.Managers;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Entities.VehicleModels;
using Byd.VehicleManagement.Entities.VehicleTypes;
using Byd.VehicleManagement.ExceptionCodes;
using Byd.VehicleManagement.Interfaces.Lookups;
using Byd.VehicleManagement.Interfaces.VehicleModels;
using Byd.VehicleManagement.Interfaces.VehicleTypes;
using Byd.VehicleManagement.Models.VehicleTypes;
using Volo.Abp;

namespace Byd.VehicleManagement.Managers.VehicleTypes;

// islevi: Arac tipinin kapsamini, model ve lookup baglarini, alan normalizasyonunu, kod tekilligini ve silme kuralini
// yonetir.
// sistemdeki gorevi: Tip is kurallarinin tek sahibidir. Bagli model istegin tenant'inda, kod istegin organizasyonunda
// aranir; ikisi de core'un global filtreleriyle saglanir.
public class VehicleTypeManager(
    IVehicleTypeRepository repository,
    IVehicleModelRepository vehicleModelRepository,
    IVehicleLookupRepository<TransportMode> transportModeRepository,
    IVehicleLookupRepository<FuelType> fuelTypeRepository)
    : BaseManager<VehicleType, Guid>(repository)
{
    private ICurrentOrganization CurrentOrganization =>
        LazyServiceProvider.LazyGetRequiredService<ICurrentOrganization>();

    // Istegin tenant ve organizasyonunda yeni arac tipini kurar; kapsamsiz istekte kimsenin goremeyecegi satir yazilmaz.
    public virtual async Task<VehicleType> CreateAsync(
        VehicleTypeCreateModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (CurrentTenant.Id is not { } tenantId || CurrentOrganization.Id is not { } organizationId)
            throw new BusinessException(VehicleTypeExceptionCodes.ScopeRequired);
        var entity = new VehicleType(GuidGenerator.Create(), tenantId, organizationId);
        await ApplyAsync(entity, model, cancellationToken);
        return entity;
    }

    // Tipin iliski ve olcu alanlarini ayni kurallardan gecirerek gunceller.
    public virtual Task UpdateAsync(
        VehicleType entity, VehicleTypeUpdateModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(model);
        return ApplyAsync(entity, model, cancellationToken);
    }

    // Tipi kullanan kayit varsa silmeyi engeller.
    public virtual async Task EnsureCanDeleteAsync(VehicleType entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (await Repository.IsDataUsedAsync(entity, ignoredEntityTypes: null, cancellationToken))
            throw new BusinessException(GeneralExceptionCodes.RelationalDataDelete);
    }

    // Baglari dogrular, alanlari uygular ve kodun organizasyonda tekil oldugunu kontrol eder.
    private async Task ApplyAsync(VehicleType entity, VehicleTypeInputModel model, CancellationToken cancellationToken = default)
    {
        await ApplyRelationsAsync(entity, model, cancellationToken);
        ApplyColumns(entity, model);
        var code = entity.Code;
        await CheckAlreadyExistsAsync(value => value.Code == code, entity.Id, cancellationToken);
    }

    // Model, ulasim modu ve yakit turunu bulur; cevap DTO'su adlari tasiyabilsin diye navigation'lari da baglar.
    private async Task ApplyRelationsAsync(
        VehicleType entity, VehicleTypeInputModel model, CancellationToken cancellationToken = default)
    {
        entity.VehicleModel = await FindVehicleModelAsync(model.VehicleModelId, cancellationToken);
        entity.VehicleModelId = entity.VehicleModel?.Id;
        entity.TransportMode = await transportModeRepository.FindAsync(
                value => value.Id == model.TransportModeId, cancellationToken: cancellationToken)
            ?? throw new BusinessException(VehicleTypeExceptionCodes.TransportModeNotFound);
        entity.TransportModeId = entity.TransportMode.Id;
        entity.FuelType = await fuelTypeRepository.FindAsync(
                value => value.Id == model.FuelTypeId, cancellationToken: cancellationToken)
            ?? throw new BusinessException(VehicleTypeExceptionCodes.FuelTypeNotFound);
        entity.FuelTypeId = entity.FuelType.Id;
    }

    // Model verilmediyse bos doner; verildiyse istegin tenant'inda arar.
    private async Task<VehicleModel?> FindVehicleModelAsync(Guid? vehicleModelId, CancellationToken cancellationToken = default)
    {
        if (vehicleModelId is null)
            return null;
        return await vehicleModelRepository.FindAsync(
                   value => value.Id == vehicleModelId.Value, cancellationToken: cancellationToken)
               ?? throw new BusinessException(VehicleTypeExceptionCodes.VehicleModelNotFound);
    }

    // Kod ve adi normalize eder; kapasite, erisilebilirlik ve uzunluk alanlarini yazar.
    private static void ApplyColumns(VehicleType entity, VehicleTypeInputModel model)
    {
        entity.Code = NormalizeRequiredText(model.Code);
        entity.Name = NormalizeRequiredText(model.Name);
        entity.SeatingCapacity = model.SeatingCapacity;
        entity.StandingCapacity = model.StandingCapacity;
        entity.WheelchairPlaceCapacity = model.WheelchairPlaceCapacity;
        entity.BicycleCapacity = model.BicycleCapacity;
        entity.LowFloor = model.LowFloor;
        entity.HasLiftOrRamp = model.HasLiftOrRamp;
        entity.Length = model.Length;
    }
}
