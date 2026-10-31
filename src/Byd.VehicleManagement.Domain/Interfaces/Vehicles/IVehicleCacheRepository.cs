using Byd.CoreExtensions.Interfaces;
using Byd.VehicleManagement.Entities.Vehicles;
using Byd.VehicleManagement.Models.Vehicles;

namespace Byd.VehicleManagement.Interfaces.Vehicles;

// islevi: Organizasyonun arac listesini cache uzerinden okuyan core repository sozlesmesini bildirir.
// sistemdeki gorevi: Realtime eslesmesinin okuma yoludur; yazmalar IVehicleRepository uzerinden yapilir ve cache
// degisiklik olayiyla tazelenir.
public interface IVehicleCacheRepository : ICachedRepository<Vehicle, VehicleCacheModel>
{
}
