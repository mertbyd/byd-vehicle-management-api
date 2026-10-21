using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Volo.Abp.Domain.Repositories;

namespace Byd.VehicleManagement.Data.Lookups;

// islevi: Varsayilan ulasim modu satirlarini idempotent olarak ekler.
// sistemdeki gorevi: Arac kayitlarinin baglanacagi TransportMode satirlarinin her ortamda bulunmasini saglar.
public class TransportModeDataSeeder(
    TransportModeManager manager,
    IRepository<TransportMode, int> repository)
    : BaseLookupDataSeeder<TransportMode, TransportModeManager>(manager, repository);
