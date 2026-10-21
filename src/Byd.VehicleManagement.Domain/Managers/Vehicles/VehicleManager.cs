using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Byd.CoreExtensions.ExceptionCodes;
using Byd.CoreExtensions.Interfaces;
using Byd.CoreExtensions.Managers;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Entities.VehicleEquipments;
using Byd.VehicleManagement.Entities.Vehicles;
using Byd.VehicleManagement.ExceptionCodes;
using Byd.VehicleManagement.Interfaces.Lookups;
using Byd.VehicleManagement.Interfaces.Vehicles;
using Byd.VehicleManagement.Interfaces.VehicleTypes;
using Byd.VehicleManagement.Models.Vehicles;
using Volo.Abp;
using Volo.Abp.Data;

namespace Byd.VehicleManagement.Managers.Vehicles;

// islevi: Aracin kapsamini, kip ve tip uyumunu, lookup baglarini, kayit kaynagini, isletmeciyle birlikte GTFS kimligi
// ile plaka tekilligini, realtime kaynakli otomatik kaydi ve silme kuralini yonetir.
// sistemdeki gorevi: Arac aggregate'inin tek kural sahibidir. Elle eklenen arac Manuel, realtime'da gorulup eklenen arac
// Realtime kaynaklidir; realtime araci tipsiz baslar ve tip sonradan ayni kipte atanir. Realtime girdisi GTFS
// standardinin degerleriyle gelir; kimlik ve kip kaydin sartidir, plaka ve erisim bilgisi bozuksa bos birakilir.
public class VehicleManager(
    IVehicleRepository repository,
    IVehicleCacheRepository cacheRepository,
    IVehicleTypeRepository vehicleTypeRepository,
    IVehicleLookupRepository<TransportMode> transportModeRepository,
    IVehicleLookupRepository<VehicleStatus> vehicleStatusRepository,
    IVehicleLookupRepository<RegistrationSource> registrationSourceRepository,
    IVehicleLookupRepository<WheelchairAccessible> wheelchairAccessibleRepository)
    : BaseManager<Vehicle, Guid>(repository)
{
    private ICurrentOrganization CurrentOrganization =>
        LazyServiceProvider.LazyGetRequiredService<ICurrentOrganization>();

    private IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    // Istegin kapsaminda elle eklenen araci kurar.
    public virtual async Task<Vehicle> CreateAsync(VehicleCreateModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        var entity = NewInScope();
        await ApplyRegistrationSourceAsync(entity, RegistrationSourceConsts.ManualCode, cancellationToken);
        await ApplyAsync(entity, model, cancellationToken);
        return entity;
    }

    // Aracin baglarini ve alanlarini ayni kurallardan gecirerek gunceller; kaynak degismez.
    public virtual Task UpdateAsync(Vehicle entity, VehicleUpdateModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(model);
        return ApplyAsync(entity, model, cancellationToken);
    }

    // Organizasyonun cache'teki arac listesinde isletmeci ve GTFS-RT kimligi esleyen araci arar; veritabanina gitmez.
    public virtual Task<VehicleCacheModel?> FindCachedByGtfsVehicleIdAsync(
        string? gtfsAgencyId, string gtfsVehicleId, CancellationToken cancellationToken = default)
    {
        var agencyId = NormalizeOptionalText(gtfsAgencyId);
        var vehicleId = NormalizeRequiredText(gtfsVehicleId);
        return cacheRepository.CachedGetByAsync(
            vehicle => vehicle.GtfsAgencyId == agencyId && vehicle.GtfsVehicleId == vehicleId, cancellationToken);
    }

    // Realtime'da gorulen ve sistemde olmayan araci tipsiz, hizmette ve Realtime kaynakli olarak kurar.
    public virtual async Task<Vehicle> RegisterFromRealtimeAsync(
        VehicleRealtimeRegistrationModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        var entity = NewInScope();
        await ApplyRegistrationSourceAsync(entity, RegistrationSourceConsts.RealtimeCode, cancellationToken);
        await ApplyRouteTypeAsync(entity, model.RouteType, cancellationToken);
        entity.VehicleStatus = await FindByCodeAsync(vehicleStatusRepository, VehicleStatusConsts.InServiceCode,
            VehicleExceptionCodes.VehicleStatusNotFound, cancellationToken);
        entity.VehicleStatusId = entity.VehicleStatus.Id;
        await ApplyRealtimeWheelchairAccessibleAsync(entity, model.WheelchairAccessible, cancellationToken);
        entity.GtfsAgencyId = NormalizeOptionalText(model.GtfsAgencyId);
        entity.GtfsVehicleId = NormalizeRequiredText(model.GtfsVehicleId);
        entity.Label = NormalizeOptionalText(model.Label);
        entity.InServiceFrom = DateOnly.FromDateTime(Clock.Now);
        await EnsureGtfsVehicleIdUniqueAsync(entity, cancellationToken);
        await ApplyRealtimeRegistrationNumberAsync(entity, model.RegistrationNumber, cancellationToken);
        return entity;
    }

    // Araci kendi donanim satirlari disinda kullanan kayit varsa silmeyi engeller; donanim satirlari aracla silinir.
    public virtual async Task EnsureCanDeleteAsync(Vehicle entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (await Repository.IsDataUsedAsync(entity, [typeof(VehicleEquipment)], cancellationToken))
            throw new BusinessException(GeneralExceptionCodes.RelationalDataDelete);
    }

    // Istegin tenant ve organizasyonunda bos araci kurar; kapsamsiz istekte kimsenin goremeyecegi satir yazilmaz.
    private Vehicle NewInScope()
    {
        if (CurrentTenant.Id is not { } tenantId || CurrentOrganization.Id is not { } organizationId)
            throw new BusinessException(VehicleExceptionCodes.ScopeRequired);
        return new Vehicle(GuidGenerator.Create(), tenantId, organizationId);
    }

    // Baglari dogrular, alanlari uygular ve tekillikleri kontrol eder.
    private async Task ApplyAsync(Vehicle entity, VehicleInputModel model, CancellationToken cancellationToken = default)
    {
        await ApplyTransportModeAsync(entity, model.TransportModeId, cancellationToken);
        await ApplyVehicleTypeAsync(entity, model.VehicleTypeId, cancellationToken);
        entity.VehicleStatus = await vehicleStatusRepository.FindAsync(
                value => value.Id == model.VehicleStatusId, cancellationToken: cancellationToken)
            ?? throw new BusinessException(VehicleExceptionCodes.VehicleStatusNotFound);
        entity.VehicleStatusId = entity.VehicleStatus.Id;
        await ApplyWheelchairAccessibleAsync(entity, model.WheelchairAccessibleId, cancellationToken);
        ApplyColumns(entity, model);
        await EnsureIdentifiersUniqueAsync(entity, cancellationToken);
    }

    // Kip lookup'ta bulunur ve araca baglanir.
    private async Task ApplyTransportModeAsync(Vehicle entity, int transportModeId, CancellationToken cancellationToken = default)
    {
        entity.TransportMode = await transportModeRepository.FindAsync(
                value => value.Id == transportModeId, cancellationToken: cancellationToken)
            ?? throw new BusinessException(VehicleExceptionCodes.TransportModeNotFound);
        entity.TransportModeId = entity.TransportMode.Id;
    }

    // Tip verilmediyse bag kaldirilir; verildiyse istegin organizasyonunda aranir ve kipi aracin kipiyle ayni olmalidir.
    private async Task ApplyVehicleTypeAsync(Vehicle entity, Guid? vehicleTypeId, CancellationToken cancellationToken = default)
    {
        entity.VehicleType = vehicleTypeId is null
            ? null
            : await vehicleTypeRepository.FindAsync(value => value.Id == vehicleTypeId.Value, cancellationToken: cancellationToken)
              ?? throw new BusinessException(VehicleExceptionCodes.VehicleTypeNotFound);
        if (entity.VehicleType is not null && entity.VehicleType.TransportModeId != entity.TransportModeId)
            throw new BusinessException(VehicleExceptionCodes.VehicleTypeTransportModeMismatch);
        entity.VehicleTypeId = entity.VehicleType?.Id;
    }

    // Erisim degeri verilmediyse bag kaldirilir; verildiyse lookup'ta aranir.
    private async Task ApplyWheelchairAccessibleAsync(
        Vehicle entity, int? wheelchairAccessibleId, CancellationToken cancellationToken = default)
    {
        entity.WheelchairAccessible = wheelchairAccessibleId is null
            ? null
            : await wheelchairAccessibleRepository.FindAsync(
                  value => value.Id == wheelchairAccessibleId.Value, cancellationToken: cancellationToken)
              ?? throw new BusinessException(VehicleExceptionCodes.WheelchairAccessibleNotFound);
        entity.WheelchairAccessibleId = entity.WheelchairAccessible?.Id;
    }

    // GTFS route_type degerini lookup koduna cevirip kipi baglar; karsiligi olmayan tur kaydi durdurur, kip tahmin edilmez.
    private async Task ApplyRouteTypeAsync(Vehicle entity, int routeType, CancellationToken cancellationToken = default)
    {
        entity.TransportMode = await FindByCodeAsync(transportModeRepository, ToBasicRouteType(routeType),
            VehicleExceptionCodes.TransportModeNotFound, cancellationToken);
        entity.TransportModeId = entity.TransportMode.Id;
    }

    // Genisletilmis route_type araligin temel karsiligina iner; temel deger oldugu gibi kalir.
    private static int ToBasicRouteType(int routeType)
        => VehicleConsts.ExtendedRouteTypes
               .Where(range => routeType >= range.From && routeType <= range.To)
               .Select(range => (int?)range.RouteType)
               .FirstOrDefault()
           ?? routeType;

    // Realtime erisim degerini lookup koduyla baglar; deger yoksa, tanimsizsa ya da lookup'ta karsiligi yoksa bilgi bos
    // kalir ve kayit durmaz.
    private async Task ApplyRealtimeWheelchairAccessibleAsync(
        Vehicle entity, int? realtimeValue, CancellationToken cancellationToken = default)
    {
        var code = VehicleConsts.RealtimeWheelchairAccessibleCodes
            .Where(value => value.RealtimeValue == realtimeValue)
            .Select(value => (int?)value.Code)
            .FirstOrDefault();
        entity.WheelchairAccessible = code is null
            ? null
            : await wheelchairAccessibleRepository.FindAsync(
                value => value.Code == code.Value, cancellationToken: cancellationToken);
        entity.WheelchairAccessibleId = entity.WheelchairAccessible?.Id;
    }

    // Feed'in plakasi dogrulanmamis bilgidir; tenant'ta baska bir aracta varsa yazilmaz ve arac plakasiz kaydedilir.
    private async Task ApplyRealtimeRegistrationNumberAsync(
        Vehicle entity, string? registrationNumber, CancellationToken cancellationToken = default)
    {
        entity.RegistrationNumber = NormalizeOptionalText(registrationNumber);
        if (await IsRegistrationNumberUsedAsync(entity, cancellationToken))
            entity.RegistrationNumber = null;
    }

    // Kaynak satirini sabit koduyla bulur ve araca baglar.
    private async Task ApplyRegistrationSourceAsync(Vehicle entity, int code, CancellationToken cancellationToken = default)
    {
        entity.RegistrationSource = await FindByCodeAsync(registrationSourceRepository, code,
            VehicleExceptionCodes.RegistrationSourceNotFound, cancellationToken);
        entity.RegistrationSourceId = entity.RegistrationSource.Id;
    }

    // Lookup satirini sabit koduyla bulur; satir yoksa verilen kodla reddeder.
    private static async Task<TLookup> FindByCodeAsync<TLookup>(
        IVehicleLookupRepository<TLookup> lookupRepository, int code, string errorCode,
        CancellationToken cancellationToken = default)
        where TLookup : VehicleLookup
    {
        return await lookupRepository.FindAsync(value => value.Code == code, cancellationToken: cancellationToken)
               ?? throw new BusinessException(errorCode);
    }

    // Kimlik ve metin alanlarini normalize eder, hizmet donemini yazar.
    private static void ApplyColumns(Vehicle entity, VehicleInputModel model)
    {
        entity.GtfsVehicleId = NormalizeRequiredText(model.GtfsVehicleId);
        entity.Label = NormalizeOptionalText(model.Label);
        entity.RegistrationNumber = NormalizeOptionalText(model.RegistrationNumber);
        entity.GtfsAgencyId = NormalizeOptionalText(model.GtfsAgencyId);
        entity.InServiceFrom = model.InServiceFrom;
        entity.InServiceUntil = model.InServiceUntil;
    }

    // Elle girilen kayitta GTFS-RT kimligi ve plaka cakismasi reddedilir.
    private async Task EnsureIdentifiersUniqueAsync(Vehicle entity, CancellationToken cancellationToken = default)
    {
        await EnsureGtfsVehicleIdUniqueAsync(entity, cancellationToken);
        if (await IsRegistrationNumberUsedAsync(entity, cancellationToken))
            throw new BusinessException(VehicleExceptionCodes.RegistrationNumberAlreadyExists);
    }

    // GTFS-RT kimligi organizasyonda isletmeciyle birlikte tekildir; farkli isletmecilerin ayni numarali araclari ayri
    // kayittir, isletmecisi bos araclar birbirine esit sayilir.
    private async Task EnsureGtfsVehicleIdUniqueAsync(Vehicle entity, CancellationToken cancellationToken = default)
    {
        if (await IsUsedByAnotherAsync(entity,
                vehicle => vehicle.GtfsAgencyId == entity.GtfsAgencyId && vehicle.GtfsVehicleId == entity.GtfsVehicleId,
                cancellationToken))
            throw new BusinessException(VehicleExceptionCodes.GtfsVehicleIdAlreadyExists);
    }

    // Plaka tenant'ta tekildir; baska organizasyonun araci da sayilsin diye kontrol boyunca organizasyon filtresi
    // kapatilir. Plakasiz arac kontrole girmez.
    private async Task<bool> IsRegistrationNumberUsedAsync(Vehicle entity, CancellationToken cancellationToken = default)
    {
        if (entity.RegistrationNumber is null)
            return false;
        using (DataFilter.Disable<IMultiOrganization>())
        {
            return await IsUsedByAnotherAsync(entity,
                vehicle => vehicle.RegistrationNumber == entity.RegistrationNumber, cancellationToken);
        }
    }

    // Aracin kendisi disinda kosula uyan bir arac olup olmadigini dondurur.
    private async Task<bool> IsUsedByAnotherAsync(
        Vehicle entity, Expression<Func<Vehicle, bool>> duplicate, CancellationToken cancellationToken = default)
    {
        var id = entity.Id;
        var others = await Repository.GetListAsync(duplicate, cancellationToken: cancellationToken);
        return others.Any(vehicle => vehicle.Id != id);
    }

}
