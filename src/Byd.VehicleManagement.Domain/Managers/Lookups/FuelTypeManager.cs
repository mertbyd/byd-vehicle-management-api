using System.Collections.Generic;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;
using Volo.Abp.Domain.Repositories;

namespace Byd.VehicleManagement.Managers.Lookups;

// islevi: FuelType satirlarinin olusturulmasini ve varsayilan satirlarini ortak lookup kurallarina baglar.
// sistemdeki gorevi: FuelType icin tipli entity kurulumunu ve seed tanimlarini saglar.
public class FuelTypeManager(IRepository<FuelType, int> repository)
    : BaseLookupManager<FuelType>(repository)
{
    // Varsayilan yakit turu satirlarini Domain.Shared uzerinden alir.
    protected override IReadOnlyList<(int Id, string Name, int Code)> GetDefinitions()
        => FuelTypeConsts.Definitions;

    // Tanimli kodu FuelType entity'si olarak kurar.
    protected override FuelType CreateEntity(int id, string name, int code)
        => new(id, name, code);
}
