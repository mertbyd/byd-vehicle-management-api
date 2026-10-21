using Byd.VehicleManagement.Constants;

namespace Byd.VehicleManagement;

public static class VehicleManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "VehicleManagement";

    public static string? DbSchema { get; set; } = VehicleManagementDatabaseConstants.SchemaName;

    public const string ConnectionStringName = "VehicleManagement";
}
