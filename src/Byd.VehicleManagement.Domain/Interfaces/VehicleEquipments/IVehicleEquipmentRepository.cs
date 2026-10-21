using Byd.CoreExtensions.Interfaces;
using Byd.VehicleManagement.Entities.VehicleEquipments;

namespace Byd.VehicleManagement.Interfaces.VehicleEquipments;

// islevi: VehicleEquipment icin core repository sozlesmesini bildirir.
// sistemdeki gorevi: Domain kurallarini EF uygulamasindan ayirir.
public interface IVehicleEquipmentRepository : IExtendedRepository<VehicleEquipment>
{
}
