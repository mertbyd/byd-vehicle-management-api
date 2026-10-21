using System.Collections.Generic;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Interfaces.Lookups;

namespace Byd.VehicleManagement.Managers.Lookups;

// islevi: FuelType satirlarinin olusturulmasini ve varsayilan satirlarini ortak lookup kurallarina baglar.
// sistemdeki gorevi: FuelType icin tipli entity kurulumunu ve seed tanimlarini saglar.
public class FuelTypeManager(IVehicleLookupRepository<FuelType> repository)
    : BaseLookupManager<FuelType>(repository)
{
    // Varsayilan yakit turu satirlarini Domain.Shared uzerinden alir.
    protected override IReadOnlyList<(int Id, string Name, int Code)> GetDefinitions()
        => FuelTypeConsts.Definitions;

    // Tanimli kodu FuelType entity'si olarak kurar.
    protected override FuelType CreateEntity(int id, string name, int code)
        => new(id, name, code);
}
