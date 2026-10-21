using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: WheelchairAccessible tablosunu ortak lookup EF kurallariyla esler.
// sistemdeki gorevi: Tablo ve kod indeksini modulun semasina baglar.
public sealed class WheelchairAccessibleConfiguration()
    : VehicleLookupConfiguration<WheelchairAccessible>(WheelchairAccessibleConsts.TableName, WheelchairAccessibleConsts.CodeIndex);
