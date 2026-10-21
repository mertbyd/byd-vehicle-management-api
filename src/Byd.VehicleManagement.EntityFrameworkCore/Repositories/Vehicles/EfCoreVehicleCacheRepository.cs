using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Byd.CoreExtensions.Interfaces;
using Byd.CoreExtensions.Repositories;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Vehicles;
using Byd.VehicleManagement.Interfaces.Vehicles;
using Byd.VehicleManagement.Models.Vehicles;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: Organizasyonun arac listesini core'un cache'li repository tabaniyla saklar ve cache uzerinden sorgular.
// sistemdeki gorevi: Realtime eslesmesinin okuma yoludur. Core cache anahtarini tenant'a gore ayirir; liste organizasyon
// filtresiyle okundugu icin anahtar organizasyonu da tasir, yoksa ayni tenant'in baska organizasyonu bu listeyi okurdu.
[ExposeServices(typeof(IVehicleCacheRepository))]
public class EfCoreVehicleCacheRepository(
    IDbContextProvider<VehicleManagementDbContext> dbContextProvider,
    IDistributedCache<IEnumerable<VehicleCacheModel>> distributedListCache)
    : EfCachedRepository<VehicleManagementDbContext, Vehicle, VehicleCacheModel>(
        dbContextProvider, distributedListCache, VehicleConsts.CacheDurationInSeconds), IVehicleCacheRepository
{
    private ICurrentOrganization CurrentOrganization =>
        LazyServiceProvider.LazyGetRequiredService<ICurrentOrganization>();

    // Tenant'a gore ayrilan core anahtarina yururlukteki organizasyonu ekler.
    protected override string CacheKey =>
        $"{base.CacheKey}{VehicleConsts.CacheKeyOrganizationSeparator}{CurrentOrganization.Id}";

    // Cache modeli tip, kip ve durum adlarini tasidigi icin liste bu navigation'larla okunur.
    public override async Task<IQueryable<Vehicle>> WithDetailsAsync()
        => (await GetQueryableAsync())
            .Include(vehicle => vehicle.VehicleType)
            .Include(vehicle => vehicle.TransportMode)
            .Include(vehicle => vehicle.VehicleStatus);

    // Arac satirini realtime eslesmesinin okudugu sade modele donusturur; core tabani bu donusumu somut repository'ye
    // birakir.
    protected override VehicleCacheModel MapToCacheModel(Vehicle source) => new()
    {
        Id = source.Id,
        VehicleTypeId = source.VehicleTypeId,
        TransportModeId = source.TransportModeId,
        VehicleStatusId = source.VehicleStatusId,
        VehicleTypeName = source.VehicleType?.Name,
        TransportModeName = source.TransportMode.Name,
        VehicleStatusName = source.VehicleStatus.Name,
        GtfsVehicleId = source.GtfsVehicleId,
        Label = source.Label,
        RegistrationNumber = source.RegistrationNumber,
        GtfsAgencyId = source.GtfsAgencyId
    };
}
