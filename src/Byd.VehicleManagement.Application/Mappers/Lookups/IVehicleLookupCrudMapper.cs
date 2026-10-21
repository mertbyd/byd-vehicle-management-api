using Byd.VehicleManagement.Dtos.Lookups;
using Byd.VehicleManagement.Models.Lookups;

namespace Byd.VehicleManagement.Mappers.Lookups;

// islevi: Lookup CRUD akisinin ihtiyac duydugu DTO, model ve entity eslemelerini tanimlar.
// sistemdeki gorevi: Ortak lookup AppService'inin her turun Mapperly sinifini ayni sozlesmeyle kullanmasini saglar.
public interface IVehicleLookupCrudMapper<TEntity>
{
    VehicleLookupCreateModel Map(CreateVehicleLookupDto input);
    VehicleLookupUpdateModel Map(UpdateVehicleLookupDto input);
    VehicleLookupDto Map(TEntity entity);
}
