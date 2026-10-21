using Byd.VehicleManagement.Dtos.Lookups;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Models.Lookups;
using Riok.Mapperly.Abstractions;

namespace Byd.VehicleManagement.Mappers.Lookups;

// islevi: RegistrationSource entity'si, domain modelleri ve public DTO'lar arasindaki eslemeleri uretir.
// sistemdeki gorevi: Application sinirinda manuel alan kopyalamayi onler.
[Mapper]
public partial class RegistrationSourceMapper : IVehicleLookupCrudMapper<RegistrationSource>
{
    public partial VehicleLookupCreateModel Map(CreateVehicleLookupDto input);
    public partial VehicleLookupUpdateModel Map(UpdateVehicleLookupDto input);
    public partial VehicleLookupDto Map(RegistrationSource entity);
}
