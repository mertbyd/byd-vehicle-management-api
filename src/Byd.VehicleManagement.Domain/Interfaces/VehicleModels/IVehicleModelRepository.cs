using Byd.CoreExtensions.Interfaces;
using Byd.VehicleManagement.Entities.VehicleModels;

namespace Byd.VehicleManagement.Interfaces.VehicleModels;

// islevi: VehicleModel icin core repository sozlesmesini bildirir.
// sistemdeki gorevi: Domain kurallarini EF uygulamasindan ayirir.
public interface IVehicleModelRepository : IExtendedRepository<VehicleModel>
{
}
