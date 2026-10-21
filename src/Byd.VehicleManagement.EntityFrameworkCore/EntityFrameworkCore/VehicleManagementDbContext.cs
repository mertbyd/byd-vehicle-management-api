using Microsoft.EntityFrameworkCore;
using Byd.CoreExtensions.EntityFrameworkCore;
using Byd.CoreExtensions.Extensions;
using Volo.Abp.Data;

namespace Byd.VehicleManagement.EntityFrameworkCore;

[ConnectionStringName(VehicleManagementDbProperties.ConnectionStringName)]
public class VehicleManagementDbContext : BydDbContext<VehicleManagementDbContext>, IVehicleManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public VehicleManagementDbContext(DbContextOptions<VehicleManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(VehicleManagementDbContext).Assembly);

        builder.ConfigureVehicleManagement();

        builder.ApplySnakeCaseNaming();
    }
}
