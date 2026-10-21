using Byd.CoreExtensions.Repositories;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Interfaces.Lookups;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: EquipmentType lookup repository sozlesmesini core EF tabanina baglar.
// sistemdeki gorevi: Lookup silmesinden once core'un iliski kontrolunu kullanilabilir kilar.
[ExposeServices(typeof(IVehicleLookupRepository<EquipmentType>))]
public class EfCoreEquipmentTypeRepository(IDbContextProvider<VehicleManagementDbContext> dbContextProvider)
    : EfExtendedRepository<VehicleManagementDbContext, EquipmentType, int>(dbContextProvider), IVehicleLookupRepository<EquipmentType>
{
}
