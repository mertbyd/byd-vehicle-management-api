using System.Collections.Generic;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Interfaces.Lookups;

namespace Byd.VehicleManagement.Managers.Lookups;

// islevi: TransportMode satirlarinin olusturulmasini ve varsayilan satirlarini ortak lookup kurallarina baglar.
// sistemdeki gorevi: TransportMode icin tipli entity kurulumunu ve seed tanimlarini saglar.
public class TransportModeManager(IVehicleLookupRepository<TransportMode> repository)
    : BaseLookupManager<TransportMode>(repository)
{
    // Varsayilan ulasim modu satirlarini Domain.Shared uzerinden alir.
    protected override IReadOnlyList<(int Id, string Name, int Code)> GetDefinitions()
        => TransportModeConsts.Definitions;

    // Tanimli kodu TransportMode entity'si olarak kurar.
    protected override TransportMode CreateEntity(int id, string name, int code)
        => new(id, name, code);
}
