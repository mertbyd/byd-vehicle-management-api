using Byd.VehicleManagement.Dtos.VehicleEquipments;
using Byd.VehicleManagement.Entities.VehicleEquipments;
using Byd.VehicleManagement.Models.VehicleEquipments;
using Riok.Mapperly.Abstractions;

namespace Byd.VehicleManagement.Mappers.VehicleEquipments;

// islevi: VehicleEquipment domain modelleri ve public DTO'lari arasindaki eslemeleri uretir.
// sistemdeki gorevi: Application sinirinda manuel alan kopyalamayi onler.
[Mapper]
public partial class VehicleEquipmentMapper
{
    public partial VehicleEquipmentCreateModel Map(CreateVehicleEquipmentDto input);
    public partial VehicleEquipmentUpdateModel Map(UpdateVehicleEquipmentDto input);
    // Aracin GTFS-RT kimligi ve donanim turu adi navigation'lardan duzlestirilir.
    public partial VehicleEquipmentDto Map(VehicleEquipment entity);
}
