using System.Threading.Tasks;
using Byd.CoreExtensions.Interfaces;
using Byd.VehicleManagement.Etos.Vehicles;
using Byd.VehicleManagement.Extensions.Vehicles;
using Byd.VehicleManagement.Interfaces.Vehicles;
using Byd.VehicleManagement.Managers.Vehicles;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Byd.VehicleManagement.Consumer.Handlers.Vehicles;

// islevi: Realtime akisinda gorulen ve kayitli olmayan araci olayin kapsaminda realtime kaynakli olarak kaydeder.
// sistemdeki gorevi: Ayni istegin tekrar teslimi ikinci arac yazmaz; arac zaten kayitliysa istek sessizce tamamlanir.
public class VehicleRegistrationRequestedEventHandler(
    VehicleManager vehicleManager,
    IVehicleRepository repository,
    ICurrentTenant currentTenant,
    ICurrentOrganization currentOrganization,
    IUnitOfWorkManager unitOfWorkManager)
    : IDistributedEventHandler<VehicleRegistrationRequestedEto>, ITransientDependency
{
    // Is birimi kapsam icinde acilip kapanir; kayit sonrasi organizasyon cache'i dogru kapsamda temizlenir.
    public virtual async Task HandleEventAsync(VehicleRegistrationRequestedEto eventData)
    {
        using var tenantScope = currentTenant.Change(eventData.TenantId);
        using var organizationScope = currentOrganization.Change(eventData.OrganizationId);
        using var unitOfWork = unitOfWorkManager.Begin(requiresNew: true);
        if (await vehicleManager.FindCachedByGtfsVehicleIdAsync(eventData.GtfsAgencyId, eventData.GtfsVehicleId) is null)
            await repository.InsertAsync(await vehicleManager.RegisterFromRealtimeAsync(eventData.ToRegistrationModel()));
        await unitOfWork.CompleteAsync();
    }
}
