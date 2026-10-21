using Byd.VehicleManagement.Dtos.VehicleTypes;
using Byd.VehicleManagement.Entities.VehicleTypes;
using Byd.VehicleManagement.Models.VehicleTypes;
using Riok.Mapperly.Abstractions;

namespace Byd.VehicleManagement.Mappers.VehicleTypes;

// islevi: VehicleType domain modelleri ve public DTO'lari arasindaki eslemeleri uretir.
// sistemdeki gorevi: Application sinirinda manuel alan kopyalamayi onler.
[Mapper]
public partial class VehicleTypeMapper
{
    public partial VehicleTypeCreateModel Map(CreateVehicleTypeDto input);
    public partial VehicleTypeUpdateModel Map(UpdateVehicleTypeDto input);
    // Bagli model, ulasim modu ve yakit adlari navigation'lardan duzlestirilir.
    public partial VehicleTypeDto Map(VehicleType entity);
}
