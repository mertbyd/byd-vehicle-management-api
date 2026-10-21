using Byd.CoreExtensions.Repositories;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Interfaces.Lookups;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: RegistrationSource lookup repository sozlesmesini core EF tabanina baglar.
// sistemdeki gorevi: Lookup silmesinden once core'un iliski kontrolunu kullanilabilir kilar.
[ExposeServices(typeof(IVehicleLookupRepository<RegistrationSource>))]
public class EfCoreRegistrationSourceRepository(IDbContextProvider<VehicleManagementDbContext> dbContextProvider)
    : EfExtendedRepository<VehicleManagementDbContext, RegistrationSource, int>(dbContextProvider), IVehicleLookupRepository<RegistrationSource>
{
}
