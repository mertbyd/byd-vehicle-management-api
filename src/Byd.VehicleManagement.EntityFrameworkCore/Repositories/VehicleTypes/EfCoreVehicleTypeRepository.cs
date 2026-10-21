using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Byd.CoreExtensions.Querying;
using Byd.CoreExtensions.Repositories;
using Byd.VehicleManagement.Entities.VehicleTypes;
using Byd.VehicleManagement.Interfaces.VehicleTypes;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: VehicleType repository sozlesmesini core EF tabanina baglar; model, ulasim modu ve yakit navigation'larini
// yukler.
// sistemdeki gorevi: Cevap DTO'sunun bagli kayit adlarini tasiyabilmesi icin Include'u tek yerde tutar.
[ExposeServices(typeof(IVehicleTypeRepository))]
public class EfCoreVehicleTypeRepository(IDbContextProvider<VehicleManagementDbContext> dbContextProvider)
    : EfExtendedRepository<VehicleManagementDbContext, VehicleType, Guid>(dbContextProvider), IVehicleTypeRepository
{
    // Tekil okumalarda bagli kayit adlarinin dolu gelmesini saglar.
    public override async Task<IQueryable<VehicleType>> WithDetailsAsync()
        => IncludeRelations(await GetQueryableAsync());

    // Liste sorgusunda da bagli kayit adlarini yukler; filtre, siralama ve sayfalama tabanda kalir.
    protected override async Task<IQueryable<VehicleType>> CreateFilteredQueryAsync<TFiltering>(
        IDataTablePagingProperty<TFiltering> pagingProperty)
        => IncludeRelations(await base.CreateFilteredQueryAsync(pagingProperty));

    // Cevabin model, ulasim modu ve yakit adlarini tasiyan navigation'lari ekler.
    private static IQueryable<VehicleType> IncludeRelations(IQueryable<VehicleType> query)
        => query.Include(type => type.VehicleModel)
            .Include(type => type.TransportMode)
            .Include(type => type.FuelType);
}
