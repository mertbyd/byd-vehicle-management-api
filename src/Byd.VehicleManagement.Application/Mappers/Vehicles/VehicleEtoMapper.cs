using Byd.VehicleManagement.Entities.Vehicles;
using Byd.VehicleManagement.Etos.Vehicles;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Byd.VehicleManagement.Mappers.Vehicles;

// islevi: Arac entity'sini ABP entity olayinin tasidigi VehicleEto'ya esler.
// sistemdeki gorevi: Otomatik arac olaylarinin ObjectMapper uzerinden doldurulmasini saglar.
[Mapper]
public partial class VehicleEtoMapper : MapperBase<Vehicle, VehicleEto>
{
    public override partial VehicleEto Map(Vehicle source);
    public override partial void Map(Vehicle source, VehicleEto destination);
}
