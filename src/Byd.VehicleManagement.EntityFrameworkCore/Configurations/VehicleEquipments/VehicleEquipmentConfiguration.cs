using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.VehicleEquipments;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: VehicleEquipment tablosunun arac ve donanim turu iliskilerini ve arac icindeki tur tekilligini tanimlar.
// sistemdeki gorevi: Arac silinince donanimlarini da siler; ayni aracta ayni turun iki satir olmasini ve kullanilan
// turun silinmesini veritabani duzeyinde de engeller.
public sealed class VehicleEquipmentConfiguration : IEntityTypeConfiguration<VehicleEquipment>
{
    // EF modeline iliski ve tekillik kurallarini uygular.
    public void Configure(EntityTypeBuilder<VehicleEquipment> builder)
    {
        builder.ToTable(VehicleEquipmentConsts.TableName, VehicleManagementDbProperties.DbSchema);
        builder.ConfigureByConvention();

        builder.HasOne(equipment => equipment.Vehicle).WithMany()
            .HasForeignKey(equipment => equipment.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(equipment => equipment.EquipmentType).WithMany()
            .HasForeignKey(equipment => equipment.EquipmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(equipment => new { equipment.VehicleId, equipment.EquipmentTypeId })
            .IsUnique().HasDatabaseName(VehicleEquipmentConsts.EquipmentTypeIndex);
    }
}
