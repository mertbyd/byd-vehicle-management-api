using System;
using System.Threading;
using System.Threading.Tasks;
using Byd.CoreExtensions.ExceptionCodes;
using Byd.CoreExtensions.Interfaces;
using Byd.CoreExtensions.Managers;
using Byd.VehicleManagement.Entities.VehicleModels;
using Byd.VehicleManagement.ExceptionCodes;
using Byd.VehicleManagement.Interfaces.VehicleModels;
using Byd.VehicleManagement.Models.VehicleModels;
using Volo.Abp;
using Volo.Abp.Data;

namespace Byd.VehicleManagement.Managers.VehicleModels;

// islevi: Arac modelinin tenant kapsamini, alan normalizasyonunu, tekilligini ve silme kuralini yonetir.
// sistemdeki gorevi: Model is kurallarinin tek sahibidir; getirme ve tekillik guard'larini core tabanindan devralir.
public class VehicleModelManager(IVehicleModelRepository repository)
    : BaseManager<VehicleModel, Guid>(repository)
{
    private IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    // Istegin tenant'inda yeni katalog modelini kurar; tenant'siz istekte kimsenin goremeyecegi satir yazilmaz.
    public virtual async Task<VehicleModel> CreateAsync(
        VehicleModelCreateModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        var tenantId = CurrentTenant.Id ?? throw new BusinessException(VehicleModelExceptionCodes.TenantRequired);
        var entity = new VehicleModel(GuidGenerator.Create(), tenantId);
        await ApplyAsync(entity, model, cancellationToken);
        return entity;
    }

    // Modelin uretici ve adini ayni kurallardan gecirerek gunceller.
    public virtual Task UpdateAsync(
        VehicleModel entity, VehicleModelUpdateModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(model);
        return ApplyAsync(entity, model, cancellationToken);
    }

    // Modeli herhangi bir organizasyonun arac tipi kullaniyorsa silmeyi engeller. Model tenant geneli katalog oldugu
    // icin kontrol boyunca organizasyon filtresi kapatilir; aksi halde baska organizasyonun tipi gorunmez.
    public virtual async Task EnsureCanDeleteAsync(VehicleModel entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        using (DataFilter.Disable<IMultiOrganization>())
        {
            if (await Repository.IsDataUsedAsync(entity, ignoredEntityTypes: null, cancellationToken))
                throw new BusinessException(GeneralExceptionCodes.RelationalDataDelete);
        }
    }

    // Uretici ve model adini normalize eder ve tenant icinde ayni cift ikinci kez acilmasin diye dogrular.
    private Task ApplyAsync(VehicleModel entity, VehicleModelInputModel model, CancellationToken cancellationToken = default)
    {
        var manufacturer = NormalizeRequiredText(model.Manufacturer);
        var name = NormalizeRequiredText(model.Name);
        entity.Manufacturer = manufacturer;
        entity.Name = name;
        return CheckAlreadyExistsAsync(
            value => value.Manufacturer == manufacturer && value.Name == name, entity.Id, cancellationToken);
    }
}
