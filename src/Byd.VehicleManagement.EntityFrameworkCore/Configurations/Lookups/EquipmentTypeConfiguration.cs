using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: EquipmentType tablosunu ortak lookup EF kurallariyla esler.
// sistemdeki gorevi: Tablo ve kod indeksini modulun semasina baglar.
public sealed class EquipmentTypeConfiguration()
    : VehicleLookupConfiguration<EquipmentType>(EquipmentTypeConsts.TableName, EquipmentTypeConsts.CodeIndex);
