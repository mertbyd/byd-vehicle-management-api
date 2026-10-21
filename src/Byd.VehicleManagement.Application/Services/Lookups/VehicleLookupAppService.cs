using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Byd.VehicleManagement.Dtos.Lookups;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Interfaces.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Byd.VehicleManagement.Mappers.Lookups;

namespace Byd.VehicleManagement.Services.Lookups;

// islevi: Arac lookup satirlarinin olustur, oku, listele, guncelle ve sil akisini tek yerde yurutur.
// sistemdeki gorevi: Tipli repository, Manager ve Mapperly baglarini merkezde cozerek yaprak servisleri bos tutar.
public abstract class VehicleLookupAppService<TEntity, TManager, TMapper>
    : VehicleManagementAppService, IVehicleLookupCrudAppService
    where TEntity : VehicleLookup
    where TManager : BaseLookupManager<TEntity>
    where TMapper : IVehicleLookupCrudMapper<TEntity>, new()
{
    private static readonly TMapper Mapper = new();
    private TManager Manager => LazyGetRequiredService<TManager>();
    private IVehicleLookupRepository<TEntity> Repository => LazyGetRequiredService<IVehicleLookupRepository<TEntity>>();

    // Girdiyi dogrular, yeni satiri kurar ve kalici hale getirir.
    public virtual async Task<VehicleLookupDto> CreateAsync(
        CreateVehicleLookupDto input, CancellationToken cancellationToken = default)
    {
        await LazyGetRequiredService<IValidator<CreateVehicleLookupDto>>()
            .ValidateAndThrowAsync(input, cancellationToken);
        var entity = await Manager.CreateAsync(Mapper.Map(input), cancellationToken);
        await Repository.InsertAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.Map(entity);
    }

    // Kimligi verilen satiri getirir.
    public virtual async Task<VehicleLookupDto> GetAsync(int id, CancellationToken cancellationToken = default)
        => Mapper.Map((await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!);

    // Turun tum satirlarini koda gore sirali getirir.
    public virtual async Task<List<VehicleLookupDto>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.GetListAsync(cancellationToken: cancellationToken);
        return entities.OrderBy(value => value.Code).Select(value => Mapper.Map(value)).ToList();
    }

    // Girdiyi dogrular, satirin kodunu ve adini gunceller.
    public virtual async Task<VehicleLookupDto> UpdateAsync(
        int id, UpdateVehicleLookupDto input, CancellationToken cancellationToken = default)
    {
        await LazyGetRequiredService<IValidator<UpdateVehicleLookupDto>>()
            .ValidateAndThrowAsync(input, cancellationToken);
        var entity = (await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!;
        await Manager.UpdateAsync(entity, Mapper.Map(input), cancellationToken);
        await Repository.UpdateAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.Map(entity);
    }

    // Hicbir kayit kullanmiyorsa satiri kalici olarak siler.
    public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = (await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!;
        await Manager.EnsureCanDeleteAsync(entity, cancellationToken);
        await Repository.DeleteAsync(entity, autoSave: true, cancellationToken: cancellationToken);
    }
}
