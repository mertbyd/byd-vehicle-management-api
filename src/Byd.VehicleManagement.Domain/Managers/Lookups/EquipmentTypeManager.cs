using System.Collections.Generic;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Interfaces.Lookups;

namespace Byd.VehicleManagement.Managers.Lookups;

// islevi: EquipmentType satirlarinin olusturulmasini ve varsayilan satirlarini ortak lookup kurallarina baglar.
// sistemdeki gorevi: EquipmentType icin tipli entity kurulumunu ve seed tanimlarini saglar.
public class EquipmentTypeManager(IVehicleLookupRepository<EquipmentType> repository)
    : BaseLookupManager<EquipmentType>(repository)
{
    // Varsayilan donanim turu satirlarini Domain.Shared uzerinden alir.
    protected override IReadOnlyList<(int Id, string Name, int Code)> GetDefinitions()
        => EquipmentTypeConsts.Definitions;

    // Tanimli kodu EquipmentType entity'si olarak kurar.
    protected override EquipmentType CreateEntity(int id, string name, int code)
        => new(id, name, code);
}
