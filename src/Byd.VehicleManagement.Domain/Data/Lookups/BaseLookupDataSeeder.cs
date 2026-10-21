using System.Threading;
using System.Threading.Tasks;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Byd.VehicleManagement.Data.Lookups;

// islevi: Varsayilan arac lookup satirlarini idempotent olarak eklemenin ortak akisini saglar.
// sistemdeki gorevi: Her lookup seeder'in ayni SeedAsync sablonunu yinelemesini onler.
public abstract class BaseLookupDataSeeder<TEntity, TManager>(
    TManager manager,
    IRepository<TEntity, int> repository) : IDataSeedContributor, ITransientDependency
    where TEntity : VehicleLookup
    where TManager : BaseLookupManager<TEntity>
{
    // Eksik satirlari tek yazimda kaydeder; mevcut ad ve kodlara dokunmaz.
    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        var missing = await manager.CreateMissingAsync(CancellationToken.None);
        if (missing.Count == 0)
            return;
        await repository.InsertManyAsync(missing, autoSave: true, cancellationToken: CancellationToken.None);
    }
}
