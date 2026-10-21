using Byd.CoreExtensions.Attributes;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Consts;

namespace Byd.VehicleManagement.Dtos.VehicleEquipments;

// islevi: VehicleEquipment liste ucunun filtre alanlarini kararli tel adi ve property yoluyla tanitir.
// sistemdeki gorevi: DataTable motorunun kabul ettigi tipli filtre izin listesidir; bir aracin donanimlari VehicleId
// filtresiyle okunur.
public class VehicleEquipmentFilter
{
    [DataTable(VehicleEquipmentFilterFields.VehicleId, VehicleEquipmentFilterFields.PropertyPaths.VehicleId)]
    public DataTableFilterItem? VehicleId { get; set; }

    [DataTable(VehicleEquipmentFilterFields.EquipmentTypeId, VehicleEquipmentFilterFields.PropertyPaths.EquipmentTypeId)]
    public DataTableFilterItem? EquipmentTypeId { get; set; }

    [DataTable(VehicleEquipmentFilterFields.Units, VehicleEquipmentFilterFields.PropertyPaths.Units)]
    public DataTableFilterItem? Units { get; set; }

    [DataTable(VehicleEquipmentFilterFields.CreationTime, VehicleEquipmentFilterFields.PropertyPaths.CreationTime)]
    public DataTableFilterItem? CreationTime { get; set; }
}
