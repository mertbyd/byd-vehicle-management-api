using Byd.CoreExtensions.Attributes;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Consts;

namespace Byd.VehicleManagement.Dtos.VehicleTypes;

// islevi: VehicleType liste ucunun filtre alanlarini kararli tel adi ve property yoluyla tanitir.
// sistemdeki gorevi: DataTable motorunun kabul ettigi tipli filtre izin listesidir.
public class VehicleTypeFilter
{
    [DataTable(VehicleTypeFilterFields.VehicleModelId, VehicleTypeFilterFields.PropertyPaths.VehicleModelId)]
    public DataTableFilterItem? VehicleModelId { get; set; }

    [DataTable(VehicleTypeFilterFields.TransportModeId, VehicleTypeFilterFields.PropertyPaths.TransportModeId)]
    public DataTableFilterItem? TransportModeId { get; set; }

    [DataTable(VehicleTypeFilterFields.FuelTypeId, VehicleTypeFilterFields.PropertyPaths.FuelTypeId)]
    public DataTableFilterItem? FuelTypeId { get; set; }

    [DataTable(VehicleTypeFilterFields.Code, VehicleTypeFilterFields.PropertyPaths.Code)]
    public DataTableFilterItem? Code { get; set; }

    [DataTable(VehicleTypeFilterFields.Name, VehicleTypeFilterFields.PropertyPaths.Name)]
    public DataTableFilterItem? Name { get; set; }

    [DataTable(VehicleTypeFilterFields.LowFloor, VehicleTypeFilterFields.PropertyPaths.LowFloor)]
    public DataTableFilterItem? LowFloor { get; set; }

    [DataTable(VehicleTypeFilterFields.CreationTime, VehicleTypeFilterFields.PropertyPaths.CreationTime)]
    public DataTableFilterItem? CreationTime { get; set; }
}
