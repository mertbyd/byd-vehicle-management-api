using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Byd.VehicleManagement.Entities.Lookups;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: Arac lookup tablolarinin ortak EF eslemesini tanimlar.
// sistemdeki gorevi: Kullanici satirlarinin kimligini 1000'den baslatip varsayilan satirlarin sabit kimlikleriyle
// cakismasini onler; kodu turde tekil tutar.
public abstract class VehicleLookupConfiguration<TEntity>(string tableName, string codeIndex)
    : IEntityTypeConfiguration<TEntity>
    where TEntity : VehicleLookup
{
    // EF modeline lookup tablo, kimlik ve kod kurallarini uygular.
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(tableName, VehicleManagementDbProperties.DbSchema);
        builder.ConfigureByConvention();
        builder.Property(value => value.Id).UseIdentityByDefaultColumn().HasIdentityOptions(startValue: 1000);
        builder.HasIndex(value => value.Code).IsUnique().HasDatabaseName(codeIndex);
    }
}
