using Byd.CoreExtensions.Interfaces;
using Byd.VehicleManagement.Entities.VehicleTypes;

namespace Byd.VehicleManagement.Interfaces.VehicleTypes;

// islevi: VehicleType icin core repository sozlesmesini bildirir.
// sistemdeki gorevi: Domain kurallarini EF uygulamasindan ayirir.
public interface IVehicleTypeRepository : IExtendedRepository<VehicleType>
{
}
