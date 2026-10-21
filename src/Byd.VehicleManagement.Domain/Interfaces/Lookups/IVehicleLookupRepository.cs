using Byd.CoreExtensions.Interfaces;
using Byd.VehicleManagement.Entities.Lookups;

namespace Byd.VehicleManagement.Interfaces.Lookups;

// islevi: Arac lookup turleri icin core repository sozlesmesini tek generic sozlesmede bildirir.
// sistemdeki gorevi: Lookup Manager'larinin silme oncesi iliski kontrolunu core'un IsDataUsedAsync yetenegiyle yapmasini
// saglar.
public interface IVehicleLookupRepository<TEntity> : IExtendedRepository<TEntity>
    where TEntity : VehicleLookup
{
}
