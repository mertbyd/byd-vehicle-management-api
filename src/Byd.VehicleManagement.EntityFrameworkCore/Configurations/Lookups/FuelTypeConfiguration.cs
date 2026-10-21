using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: FuelType tablosunu ortak lookup EF kurallariyla esler.
// sistemdeki gorevi: Tablo ve kod indeksini modulun semasina baglar.
public sealed class FuelTypeConfiguration()
    : VehicleLookupConfiguration<FuelType>(FuelTypeConsts.TableName, FuelTypeConsts.CodeIndex);
