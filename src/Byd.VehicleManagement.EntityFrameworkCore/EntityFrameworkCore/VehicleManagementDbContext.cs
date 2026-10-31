using Microsoft.EntityFrameworkCore;
using Byd.CoreExtensions.EntityFrameworkCore;
using Byd.CoreExtensions.Extensions;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Entities.VehicleEquipments;
using Byd.VehicleManagement.Entities.VehicleModels;
using Byd.VehicleManagement.Entities.Vehicles;
using Byd.VehicleManagement.Entities.VehicleTypes;
using Volo.Abp.Data;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: Arac yonetimi tablolarinin EF Core DbContext'idir.
// sistemdeki gorevi: Core'un tenant ve aktiflik filtrelerini tasiyan tabandan turer; tablo kurallari konfigurasyon siniflarindan uygulanir.
[ConnectionStringName(VehicleManagementDbProperties.ConnectionStringName)]
public class VehicleManagementDbContext : BydDbContext<VehicleManagementDbContext>, IVehicleManagementDbContext
{
    public DbSet<TransportMode> TransportModes { get; set; } = default!;
    public DbSet<FuelType> FuelTypes { get; set; } = default!;
    public DbSet<VehicleStatus> VehicleStatuses { get; set; } = default!;
    public DbSet<WheelchairAccessible> WheelchairAccessibles { get; set; } = default!;
    public DbSet<EquipmentType> EquipmentTypes { get; set; } = default!;
    public DbSet<RegistrationSource> RegistrationSources { get; set; } = default!;
    public DbSet<VehicleModel> VehicleModels { get; set; } = default!;
    public DbSet<VehicleType> VehicleTypes { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;
    public DbSet<VehicleEquipment> VehicleEquipments { get; set; } = default!;

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
