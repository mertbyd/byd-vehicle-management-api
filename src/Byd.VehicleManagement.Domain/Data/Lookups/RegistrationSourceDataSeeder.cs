using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Volo.Abp.Domain.Repositories;

namespace Byd.VehicleManagement.Data.Lookups;

// islevi: Varsayilan kayit kaynagi satirlarini idempotent olarak ekler.
// sistemdeki gorevi: Arac kayitlarinin baglanacagi RegistrationSource satirlarinin her ortamda bulunmasini saglar.
public class RegistrationSourceDataSeeder(
    RegistrationSourceManager manager,
    IRepository<RegistrationSource, int> repository)
    : BaseLookupDataSeeder<RegistrationSource, RegistrationSourceManager>(manager, repository);
