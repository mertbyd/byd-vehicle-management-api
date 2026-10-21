using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Byd.VehicleManagement.EntityFrameworkCore;

public static class VehicleManagementDbContextModelCreatingExtensions
{
    public static void ConfigureVehicleManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
