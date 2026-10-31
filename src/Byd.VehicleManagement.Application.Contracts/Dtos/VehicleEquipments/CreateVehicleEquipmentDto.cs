using System;

namespace Byd.VehicleManagement.Dtos.VehicleEquipments;

// mirastan gelen: EquipmentTypeId, Units.
// burada yazilan: VehicleId; donanimin takilacagi arac yalniz olusturmada secilir.
public sealed class CreateVehicleEquipmentDto : VehicleEquipmentInputDto
{
    // Foreign Keys
    public Guid VehicleId { get; set; }
}
