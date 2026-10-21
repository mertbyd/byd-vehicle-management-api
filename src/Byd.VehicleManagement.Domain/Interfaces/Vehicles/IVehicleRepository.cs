using Byd.CoreExtensions.Interfaces;
using Byd.VehicleManagement.Entities.Vehicles;

namespace Byd.VehicleManagement.Interfaces.Vehicles;

// islevi: Vehicle icin core repository sozlesmesini bildirir.
// sistemdeki gorevi: Domain kurallarini EF uygulamasindan ayirir.
public interface IVehicleRepository : IExtendedRepository<Vehicle>
{
}
