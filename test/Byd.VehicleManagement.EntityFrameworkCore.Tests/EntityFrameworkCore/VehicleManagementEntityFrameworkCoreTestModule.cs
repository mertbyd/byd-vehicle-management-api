using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Byd.CoreExtensions.EntityFrameworkCore;
using Byd.CoreExtensions.Options;
using Byd.VehicleManagement.Consumer;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace Byd.VehicleManagement.EntityFrameworkCore;

[DependsOn(
    typeof(VehicleManagementApplicationTestModule),
    typeof(VehicleManagementEntityFrameworkCoreModule),
    typeof(VehicleManagementConsumerModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
)]
public class VehicleManagementEntityFrameworkCoreTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<CoreExtensionsModuleOptions>(options =>
        {
            options.ModuleName = "VehicleManagement";
            options.EnvironmentName = "Test";
        });
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
        // Testler tek SQLite baglantisinda calisir; tenant degistiren testlerde baglanti dizesi icin tenant deposu
        // (auth'un PostgreSQL tablolari) aranmaz.
        context.Services.Replace(ServiceDescriptor.Transient<IConnectionStringResolver, DefaultConnectionStringResolver>());
        // Ayar tablolari da SQLite'ta yoktur; ayarlar tanimdaki varsayilan degerleriyle okunur.
        context.Services.Replace(ServiceDescriptor.Singleton<ISettingStore, NullSettingStore>());

        var sqliteConnection = CreateDatabaseAndGetConnection();

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(abpDbContextConfigurationContext =>
            {
                abpDbContextConfigurationContext.DbContextOptions.UseSqlite(sqliteConnection);
            });
        });
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new AbpUnitTestSqliteConnection("Data Source=:memory:");
        connection.Open();

        new VehicleManagementDbContext(
            new DbContextOptionsBuilder<VehicleManagementDbContext>().UseSqlite(connection).Options
        ).GetService<IRelationalDatabaseCreator>().CreateTables();

        // Distributed event yayini core'un outbox tablosuna yazar; test DB'si de ayni tabloyu tasir.
        new CoreExtensionsDbContext(
            new DbContextOptionsBuilder<CoreExtensionsDbContext>().UseSqlite(connection).Options
        ).GetService<IRelationalDatabaseCreator>().CreateTables();

        return connection;
    }
}
