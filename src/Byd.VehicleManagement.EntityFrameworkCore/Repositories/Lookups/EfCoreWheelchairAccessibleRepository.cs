using Byd.CoreExtensions.Repositories;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Interfaces.Lookups;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: WheelchairAccessible lookup repository sozlesmesini core EF tabanina baglar.
// sistemdeki gorevi: Lookup silmesinden once core'un iliski kontrolunu kullanilabilir kilar.
[ExposeServices(typeof(IVehicleLookupRepository<WheelchairAccessible>))]
public class EfCoreWheelchairAccessibleRepository(IDbContextProvider<VehicleManagementDbContext> dbContextProvider)
    : EfExtendedRepository<VehicleManagementDbContext, WheelchairAccessible, int>(dbContextProvider), IVehicleLookupRepository<WheelchairAccessible>
{
}
