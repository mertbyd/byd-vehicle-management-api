using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Volo.Abp.Domain.Repositories;

namespace Byd.VehicleManagement.Data.Lookups;

// islevi: Varsayilan yakit turu satirlarini idempotent olarak ekler.
// sistemdeki gorevi: Arac kayitlarinin baglanacagi FuelType satirlarinin her ortamda bulunmasini saglar.
public class FuelTypeDataSeeder(
    FuelTypeManager manager,
    IRepository<FuelType, int> repository)
    : BaseLookupDataSeeder<FuelType, FuelTypeManager>(manager, repository);
