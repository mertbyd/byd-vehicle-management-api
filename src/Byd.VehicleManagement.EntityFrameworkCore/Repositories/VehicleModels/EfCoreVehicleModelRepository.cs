using System;
using Byd.CoreExtensions.Repositories;
using Byd.VehicleManagement.Entities.VehicleModels;
using Byd.VehicleManagement.Interfaces.VehicleModels;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: VehicleModel repository sozlesmesini core EF tabanina baglar.
// sistemdeki gorevi: Filtreli liste ve silme oncesi iliski kontrolunu core'un extended repository'sinden saglar.
[ExposeServices(typeof(IVehicleModelRepository))]
public class EfCoreVehicleModelRepository(IDbContextProvider<VehicleManagementDbContext> dbContextProvider)
    : EfExtendedRepository<VehicleManagementDbContext, VehicleModel, Guid>(dbContextProvider), IVehicleModelRepository
{
}
