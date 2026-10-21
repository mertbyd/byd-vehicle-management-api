using Microsoft.EntityFrameworkCore;
using Byd.VehicleManagement.Entities.Lookups;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: Arac yonetimi DbContext sozlesmesini tanimlar.
// sistemdeki gorevi: Modulun baglanti dizesi adini EF katmanina bildirir.
[ConnectionStringName(VehicleManagementDbProperties.ConnectionStringName)]
public interface IVehicleManagementDbContext : IEfCoreDbContext
{
    DbSet<TransportMode> TransportModes { get; }
    DbSet<FuelType> FuelTypes { get; }
    DbSet<VehicleStatus> VehicleStatuses { get; }
    DbSet<WheelchairAccessible> WheelchairAccessibles { get; }
    DbSet<EquipmentType> EquipmentTypes { get; }
}
