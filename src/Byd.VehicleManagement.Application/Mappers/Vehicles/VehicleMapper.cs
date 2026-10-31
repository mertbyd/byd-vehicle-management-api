using Byd.VehicleManagement.Dtos.Vehicles;
using Byd.VehicleManagement.Entities.Vehicles;
using Byd.VehicleManagement.Models.Vehicles;
using Riok.Mapperly.Abstractions;

namespace Byd.VehicleManagement.Mappers.Vehicles;

// islevi: Vehicle domain modelleri, cache modeli ve public DTO'lari arasindaki eslemeleri uretir.
// sistemdeki gorevi: Application sinirinda manuel alan kopyalamayi onler.
[Mapper]
public partial class VehicleMapper
{
    public partial VehicleCreateModel Map(CreateVehicleDto input);
    public partial VehicleUpdateModel Map(UpdateVehicleDto input);
    public partial VehicleRealtimeRegistrationModel Map(RegisterRealtimeVehicleDto input);
    // Tip, kip, durum, kaynak ve erisim adlari navigation'lardan duzlestirilir.
    public partial VehicleDto Map(Vehicle entity);
    public partial VehicleRealtimeDto Map(VehicleCacheModel source);
    public partial VehicleRealtimeDto MapToRealtime(Vehicle entity);
}
