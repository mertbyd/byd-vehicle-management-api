using Byd.CoreExtensions.Attributes;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Consts;

namespace Byd.VehicleManagement.Dtos.VehicleModels;

// islevi: VehicleModel liste ucunun filtre alanlarini kararli tel adi ve property yoluyla tanitir.
// sistemdeki gorevi: DataTable motorunun kabul ettigi tipli filtre izin listesidir.
public class VehicleModelFilter
{
    [DataTable(VehicleModelFilterFields.Manufacturer, VehicleModelFilterFields.PropertyPaths.Manufacturer)]
    public DataTableFilterItem? Manufacturer { get; set; }

    [DataTable(VehicleModelFilterFields.Name, VehicleModelFilterFields.PropertyPaths.Name)]
    public DataTableFilterItem? Name { get; set; }

    [DataTable(VehicleModelFilterFields.CreationTime, VehicleModelFilterFields.PropertyPaths.CreationTime)]
    public DataTableFilterItem? CreationTime { get; set; }
}
