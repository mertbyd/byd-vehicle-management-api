using System.Collections.Generic;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Interfaces.Lookups;

namespace Byd.VehicleManagement.Managers.Lookups;

// islevi: RegistrationSource satirlarinin olusturulmasini ve varsayilan satirlarini ortak lookup kurallarina baglar.
// sistemdeki gorevi: RegistrationSource icin tipli entity kurulumunu ve seed tanimlarini saglar.
public class RegistrationSourceManager(IVehicleLookupRepository<RegistrationSource> repository)
    : BaseLookupManager<RegistrationSource>(repository)
{
    // Varsayilan kayit kaynagi satirlarini Domain.Shared uzerinden alir.
    protected override IReadOnlyList<(int Id, string Name, int Code)> GetDefinitions()
        => RegistrationSourceConsts.Definitions;

    // Tanimli kodu RegistrationSource entity'si olarak kurar.
    protected override RegistrationSource CreateEntity(int id, string name, int code)
        => new(id, name, code);
}
