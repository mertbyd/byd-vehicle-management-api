using Byd.CoreExtensions.Attributes;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Consts;

namespace Byd.VehicleManagement.Dtos.Vehicles;

// islevi: Vehicle liste ucunun filtre alanlarini kararli tel adi ve property yoluyla tanitir.
// sistemdeki gorevi: DataTable motorunun kabul ettigi tipli filtre izin listesidir.
public class VehicleFilter
{
    [DataTable(VehicleFilterFields.VehicleTypeId, VehicleFilterFields.PropertyPaths.VehicleTypeId)]
    public DataTableFilterItem? VehicleTypeId { get; set; }

    [DataTable(VehicleFilterFields.TransportModeId, VehicleFilterFields.PropertyPaths.TransportModeId)]
    public DataTableFilterItem? TransportModeId { get; set; }

    [DataTable(VehicleFilterFields.VehicleStatusId, VehicleFilterFields.PropertyPaths.VehicleStatusId)]
    public DataTableFilterItem? VehicleStatusId { get; set; }

    [DataTable(VehicleFilterFields.RegistrationSourceId, VehicleFilterFields.PropertyPaths.RegistrationSourceId)]
    public DataTableFilterItem? RegistrationSourceId { get; set; }

    [DataTable(VehicleFilterFields.GtfsVehicleId, VehicleFilterFields.PropertyPaths.GtfsVehicleId)]
    public DataTableFilterItem? GtfsVehicleId { get; set; }

    [DataTable(VehicleFilterFields.Label, VehicleFilterFields.PropertyPaths.Label)]
    public DataTableFilterItem? Label { get; set; }

    [DataTable(VehicleFilterFields.RegistrationNumber, VehicleFilterFields.PropertyPaths.RegistrationNumber)]
    public DataTableFilterItem? RegistrationNumber { get; set; }

    [DataTable(VehicleFilterFields.GtfsAgencyId, VehicleFilterFields.PropertyPaths.GtfsAgencyId)]
    public DataTableFilterItem? GtfsAgencyId { get; set; }

    [DataTable(VehicleFilterFields.InServiceFrom, VehicleFilterFields.PropertyPaths.InServiceFrom)]
    public DataTableFilterItem? InServiceFrom { get; set; }

    [DataTable(VehicleFilterFields.CreationTime, VehicleFilterFields.PropertyPaths.CreationTime)]
    public DataTableFilterItem? CreationTime { get; set; }
}
