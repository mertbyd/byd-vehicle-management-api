using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: TransportMode tablosunu ortak lookup EF kurallariyla esler.
// sistemdeki gorevi: Tablo ve kod indeksini modulun semasina baglar.
public sealed class TransportModeConfiguration()
    : VehicleLookupConfiguration<TransportMode>(TransportModeConsts.TableName, TransportModeConsts.CodeIndex);
