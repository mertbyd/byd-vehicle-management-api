using Byd.VehicleManagement.Dtos.VehicleModels;
using Byd.VehicleManagement.Entities.VehicleModels;
using Byd.VehicleManagement.Models.VehicleModels;
using Riok.Mapperly.Abstractions;

namespace Byd.VehicleManagement.Mappers.VehicleModels;

// islevi: VehicleModel domain modelleri ve public DTO'lari arasindaki eslemeleri uretir.
// sistemdeki gorevi: Application sinirinda manuel alan kopyalamayi onler.
[Mapper]
public partial class VehicleModelMapper
{
    public partial VehicleModelCreateModel Map(CreateVehicleModelDto input);
    public partial VehicleModelUpdateModel Map(UpdateVehicleModelDto input);
    public partial VehicleModelDto Map(VehicleModel entity);
}
