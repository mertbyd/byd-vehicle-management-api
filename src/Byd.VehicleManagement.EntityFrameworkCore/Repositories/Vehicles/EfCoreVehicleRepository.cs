using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Byd.CoreExtensions.Querying;
using Byd.CoreExtensions.Repositories;
using Byd.VehicleManagement.Entities.Vehicles;
using Byd.VehicleManagement.Interfaces.Vehicles;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: Vehicle repository sozlesmesini core EF tabanina baglar; tip, kip, durum, kaynak ve erisim navigation'larini
// yukler.
// sistemdeki gorevi: Cevap DTO'sunun bagli kayit adlarini tasiyabilmesi icin Include'u tek yerde tutar.
[ExposeServices(typeof(IVehicleRepository))]
public class EfCoreVehicleRepository(IDbContextProvider<VehicleManagementDbContext> dbContextProvider)
    : EfExtendedRepository<VehicleManagementDbContext, Vehicle, Guid>(dbContextProvider), IVehicleRepository
{
    // Tekil okumalarda bagli kayit adlarinin dolu gelmesini saglar.
    public override async Task<IQueryable<Vehicle>> WithDetailsAsync()
        => IncludeRelations(await GetQueryableAsync());

    // Liste sorgusunda da ayni navigation'lari yukler; filtre, siralama ve sayfalama tabanda kalir.
    protected override async Task<IQueryable<Vehicle>> CreateFilteredQueryAsync<TFiltering>(
        IDataTablePagingProperty<TFiltering> pagingProperty)
        => IncludeRelations(await base.CreateFilteredQueryAsync(pagingProperty));

    // Cevabin tip, kip, durum, kaynak ve erisim adlarini tasiyan navigation'lari ekler.
    private static IQueryable<Vehicle> IncludeRelations(IQueryable<Vehicle> query)
        => query.Include(vehicle => vehicle.VehicleType)
            .Include(vehicle => vehicle.TransportMode)
            .Include(vehicle => vehicle.VehicleStatus)
            .Include(vehicle => vehicle.RegistrationSource)
            .Include(vehicle => vehicle.WheelchairAccessible);
}
