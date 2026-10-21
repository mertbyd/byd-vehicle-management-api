using Microsoft.EntityFrameworkCore;
using Byd.CoreExtensions.EntityFrameworkCore;

namespace Byd.VehicleManagement.EntityFrameworkCore;

public class VehicleManagementDbContextFactory
    : CoreExtensionsDesignTimeDbContextFactory<VehicleManagementDbContext>
{
    protected override string ConnectionStringName => VehicleManagementDbProperties.ConnectionStringName;

    protected override VehicleManagementDbContext CreateInstance(
        DbContextOptions<VehicleManagementDbContext> options)
    {
        return new(options);
    }
}
