using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Entities.VehicleModels;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: VehicleModel tablosunu ve tenant icindeki uretici-model tekilligini tanimlar.
// sistemdeki gorevi: Ayni tenant'ta ayni modelin ikinci kez acilmasini veritabani duzeyinde de engeller.
public sealed class VehicleModelConfiguration : IEntityTypeConfiguration<VehicleModel>
{
    // EF modeline tablo ve tekillik kurallarini uygular.
    public void Configure(EntityTypeBuilder<VehicleModel> builder)
    {
        builder.ToTable(VehicleModelConsts.TableName, VehicleManagementDbProperties.DbSchema);
        builder.ConfigureByConvention();
        builder.HasIndex(model => new { model.TenantId, model.Manufacturer, model.Name })
            .IsUnique().HasDatabaseName(VehicleModelConsts.ManufacturerNameIndex);
    }
}
