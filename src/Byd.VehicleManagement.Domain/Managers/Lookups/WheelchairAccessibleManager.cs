using System.Collections.Generic;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;
using Volo.Abp.Domain.Repositories;

namespace Byd.VehicleManagement.Managers.Lookups;

// islevi: WheelchairAccessible satirlarinin olusturulmasini ve varsayilan satirlarini ortak lookup kurallarina baglar.
// sistemdeki gorevi: WheelchairAccessible icin tipli entity kurulumunu ve seed tanimlarini saglar.
public class WheelchairAccessibleManager(IRepository<WheelchairAccessible, int> repository)
    : BaseLookupManager<WheelchairAccessible>(repository)
{
    // Varsayilan tekerlekli sandalye erisimi satirlarini Domain.Shared uzerinden alir.
    protected override IReadOnlyList<(int Id, string Name, int Code)> GetDefinitions()
        => WheelchairAccessibleConsts.Definitions;

    // Tanimli kodu WheelchairAccessible entity'si olarak kurar.
    protected override WheelchairAccessible CreateEntity(int id, string name, int code)
        => new(id, name, code);
}
