using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Volo.Abp.Domain.Repositories;

namespace Byd.VehicleManagement.Data.Lookups;

// islevi: Varsayilan donanim turu satirlarini idempotent olarak ekler.
// sistemdeki gorevi: Arac kayitlarinin baglanacagi EquipmentType satirlarinin her ortamda bulunmasini saglar.
public class EquipmentTypeDataSeeder(
    EquipmentTypeManager manager,
    IRepository<EquipmentType, int> repository)
    : BaseLookupDataSeeder<EquipmentType, EquipmentTypeManager>(manager, repository);
