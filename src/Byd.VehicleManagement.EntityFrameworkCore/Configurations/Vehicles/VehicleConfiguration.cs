using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.Vehicles;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: Vehicle tablosunun tip ve lookup iliskilerini; isletmeciyle birlikte GTFS-RT kimligi ile plaka tekilligini
// tanimlar.
// sistemdeki gorevi: Kullanilan tip veya lookup satirinin silinmesini veritabani duzeyinde de engeller.
public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    // EF modeline iliski ve tekillik kurallarini uygular.
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable(VehicleConsts.TableName, VehicleManagementDbProperties.DbSchema);
        builder.ConfigureByConvention();

        builder.HasOne(vehicle => vehicle.VehicleType).WithMany()
            .HasForeignKey(vehicle => vehicle.VehicleTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(vehicle => vehicle.TransportMode).WithMany()
            .HasForeignKey(vehicle => vehicle.TransportModeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(vehicle => vehicle.VehicleStatus).WithMany()
            .HasForeignKey(vehicle => vehicle.VehicleStatusId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(vehicle => vehicle.RegistrationSource).WithMany()
            .HasForeignKey(vehicle => vehicle.RegistrationSourceId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(vehicle => vehicle.WheelchairAccessible).WithMany()
            .HasForeignKey(vehicle => vehicle.WheelchairAccessibleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Isletmecisi bos araclar da birbirine esit sayilir; tek isletmecili feed'de ayni numara iki kez yazilmaz.
        builder.HasIndex(vehicle => new
            {
                vehicle.TenantId, vehicle.OrganizationId, vehicle.GtfsAgencyId, vehicle.GtfsVehicleId
            })
            .IsUnique().AreNullsDistinct(false).HasDatabaseName(VehicleConsts.GtfsVehicleIdIndex);
        builder.HasIndex(vehicle => new { vehicle.TenantId, vehicle.RegistrationNumber })
            .IsUnique().HasFilter(VehicleConsts.RegistrationNumberIndexFilter)
            .HasDatabaseName(VehicleConsts.RegistrationNumberIndex);
    }
}
