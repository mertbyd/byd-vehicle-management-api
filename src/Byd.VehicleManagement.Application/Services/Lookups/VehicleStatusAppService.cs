using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Byd.VehicleManagement.Mappers.Lookups;

namespace Byd.VehicleManagement.Services.Lookups;

// islevi: VehicleStatus satirlarinin CRUD akisini ortak lookup tabanina baglar.
// sistemdeki gorevi: Tipli baglari bildirir; dogrulama, Manager, Mapperly ve kalici yazma tabanda kalir.
public class VehicleStatusAppService
    : VehicleLookupAppService<VehicleStatus, VehicleStatusManager, VehicleStatusMapper>,
        IVehicleStatusAppService
{
}
