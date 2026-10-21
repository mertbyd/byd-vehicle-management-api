using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.VehicleTypes;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: VehicleType tablosunun model ve lookup iliskilerini ve organizasyon icindeki kod tekilligini tanimlar.
// sistemdeki gorevi: Kullanilan model veya lookup satirinin silinmesini veritabani duzeyinde de engeller; bir modelin
// birden cok tipi olabilir.
public sealed class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
{
    // EF modeline iliski ve tekillik kurallarini uygular.
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        builder.ToTable(VehicleTypeConsts.TableName, VehicleManagementDbProperties.DbSchema);
        builder.ConfigureByConvention();

        builder.HasOne(type => type.VehicleModel).WithMany()
            .HasForeignKey(type => type.VehicleModelId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(type => type.TransportMode).WithMany()
            .HasForeignKey(type => type.TransportModeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(type => type.FuelType).WithMany()
            .HasForeignKey(type => type.FuelTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(type => new { type.TenantId, type.OrganizationId, type.Code })
            .IsUnique().HasDatabaseName(VehicleTypeConsts.CodeIndex);
    }
}
