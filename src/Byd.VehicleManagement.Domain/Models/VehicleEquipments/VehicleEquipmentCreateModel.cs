using System;

namespace Byd.VehicleManagement.Models.VehicleEquipments;

// mirastan gelen: EquipmentTypeId, Units.
// burada yazilan: VehicleId; donanimin takilacagi arac yalniz olusturmada secilir.
public sealed class VehicleEquipmentCreateModel : VehicleEquipmentInputModel
{
    public Guid VehicleId { get; set; }
}
