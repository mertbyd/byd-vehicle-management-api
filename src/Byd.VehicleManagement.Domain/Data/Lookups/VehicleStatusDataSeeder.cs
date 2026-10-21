using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Volo.Abp.Domain.Repositories;

namespace Byd.VehicleManagement.Data.Lookups;

// islevi: Varsayilan arac durumu satirlarini idempotent olarak ekler.
// sistemdeki gorevi: Arac kayitlarinin baglanacagi VehicleStatus satirlarinin her ortamda bulunmasini saglar.
public class VehicleStatusDataSeeder(
    VehicleStatusManager manager,
    IRepository<VehicleStatus, int> repository)
    : BaseLookupDataSeeder<VehicleStatus, VehicleStatusManager>(manager, repository);
