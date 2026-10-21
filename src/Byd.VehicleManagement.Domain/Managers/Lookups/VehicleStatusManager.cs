using System.Collections.Generic;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Interfaces.Lookups;

namespace Byd.VehicleManagement.Managers.Lookups;

// islevi: VehicleStatus satirlarinin olusturulmasini ve varsayilan satirlarini ortak lookup kurallarina baglar.
// sistemdeki gorevi: VehicleStatus icin tipli entity kurulumunu ve seed tanimlarini saglar.
public class VehicleStatusManager(IVehicleLookupRepository<VehicleStatus> repository)
    : BaseLookupManager<VehicleStatus>(repository)
{
    // Varsayilan arac durumu satirlarini Domain.Shared uzerinden alir.
    protected override IReadOnlyList<(int Id, string Name, int Code)> GetDefinitions()
        => VehicleStatusConsts.Definitions;

    // Tanimli kodu VehicleStatus entity'si olarak kurar.
    protected override VehicleStatus CreateEntity(int id, string name, int code)
        => new(id, name, code);
}
