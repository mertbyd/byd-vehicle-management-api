using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Volo.Abp.Domain.Repositories;

namespace Byd.VehicleManagement.Data.Lookups;

// islevi: Varsayilan tekerlekli sandalye erisimi satirlarini idempotent olarak ekler.
// sistemdeki gorevi: Arac kayitlarinin baglanacagi WheelchairAccessible satirlarinin her ortamda bulunmasini saglar.
public class WheelchairAccessibleDataSeeder(
    WheelchairAccessibleManager manager,
    IRepository<WheelchairAccessible, int> repository)
    : BaseLookupDataSeeder<WheelchairAccessible, WheelchairAccessibleManager>(manager, repository);
