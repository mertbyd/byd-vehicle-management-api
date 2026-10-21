using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: VehicleStatus tablosunu ortak lookup EF kurallariyla esler.
// sistemdeki gorevi: Tablo ve kod indeksini modulun semasina baglar.
public sealed class VehicleStatusConfiguration()
    : VehicleLookupConfiguration<VehicleStatus>(VehicleStatusConsts.TableName, VehicleStatusConsts.CodeIndex);
