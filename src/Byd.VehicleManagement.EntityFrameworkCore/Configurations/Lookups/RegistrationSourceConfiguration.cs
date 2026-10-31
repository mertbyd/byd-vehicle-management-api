using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Lookups;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: RegistrationSource tablosunu ortak lookup EF kurallariyla esler.
// sistemdeki gorevi: Tablo ve kod indeksini modulun semasina baglar.
public sealed class RegistrationSourceConfiguration()
    : VehicleLookupConfiguration<RegistrationSource>(RegistrationSourceConsts.TableName, RegistrationSourceConsts.CodeIndex);
