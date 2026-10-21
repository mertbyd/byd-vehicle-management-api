using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Byd.VehicleManagement.Mappers.Lookups;
using Volo.Abp.Domain.Repositories;

namespace Byd.VehicleManagement.Services.Lookups;

// islevi: FuelType satirlarinin CRUD akisini ortak lookup tabanina baglar.
// sistemdeki gorevi: Tipli baglari bildirir; dogrulama, Manager, Mapperly ve kalici yazma tabanda kalir.
public class FuelTypeAppService
    : VehicleLookupAppService<FuelType, FuelTypeManager, IRepository<FuelType, int>, FuelTypeMapper>,
        IFuelTypeAppService
{
}
