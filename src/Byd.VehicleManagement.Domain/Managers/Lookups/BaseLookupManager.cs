using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.ExceptionCodes;
using Byd.VehicleManagement.Models.Lookups;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Byd.VehicleManagement.Managers.Lookups;

// islevi: Lookup kodunun tekilligini, adin normalizasyonunu ve eksik varsayilan satirlarin belirlenmesini yonetir.
// sistemdeki gorevi: Bes lookup turunun ortak yazma ve seed kurallarini tek yerde uygular.
public abstract class BaseLookupManager<TEntity>(IRepository<TEntity, int> repository) : DomainService
    where TEntity : VehicleLookup
{
    // Alt sinifin Domain.Shared icindeki varsayilan satirlarini dondurur.
    protected abstract IReadOnlyList<(int Id, string Name, int Code)> GetDefinitions();

    // Tanimli kodu tipli entity olarak kurar.
    protected abstract TEntity CreateEntity(int id, string name, int code);

    // Kodun turde bos oldugunu dogrular ve yeni satiri kurar; kimligi veritabani uretir.
    public virtual async Task<TEntity> CreateAsync(
        VehicleLookupCreateModel model, CancellationToken cancellationToken = default)
    {
        await EnsureCodeAvailableAsync(model.Code, 0, cancellationToken);
        return CreateEntity(0, model.Name.Trim(), model.Code);
    }

    // Kodun baska bir satirda olmadigini dogrular ve kod ile adi gunceller.
    public virtual async Task UpdateAsync(
        TEntity entity, VehicleLookupUpdateModel model, CancellationToken cancellationToken = default)
    {
        await EnsureCodeAvailableAsync(model.Code, entity.Id, cancellationToken);
        entity.Code = model.Code;
        entity.Name = model.Name.Trim();
    }

    // Kimligi ya da kodu henuz kullanilmayan varsayilan satirlari kurar; mevcut ve kullanicinin
    // degistirdigi satirlara dokunmaz.
    public virtual async Task<List<TEntity>> CreateMissingAsync(CancellationToken cancellationToken = default)
    {
        var existing = await repository.GetListAsync(cancellationToken: cancellationToken);
        var ids = existing.Select(value => value.Id).ToHashSet();
        var codes = existing.Select(value => value.Code).ToHashSet();
        return GetDefinitions().Where(value => !ids.Contains(value.Id) && !codes.Contains(value.Code))
            .Select(value => CreateEntity(value.Id, value.Name, value.Code)).ToList();
    }

    // Ayni kodun turde baska bir satirda bulunmasini kodlu hatayla engeller.
    private async Task EnsureCodeAvailableAsync(int code, int currentId, CancellationToken cancellationToken = default)
    {
        if (await repository.AnyAsync(
                value => value.Code == code && value.Id != currentId, cancellationToken))
            throw new BusinessException(VehicleLookupExceptionCodes.CodeAlreadyExists);
    }
}
