using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Byd.VehicleManagement;

[DependsOn(
    typeof(VehicleManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class VehicleManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(VehicleManagementApplicationContractsModule).Assembly,
            VehicleManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<VehicleManagementHttpApiClientModule>();
        });

    }
}
