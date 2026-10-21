using Microsoft.Extensions.DependencyInjection;
using Byd.LogManagement;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace Byd.VehicleManagement;

// Auth'un Application katmani token sunucusunu tasidigi icin yerine log'un Application katmani baglanir.
[DependsOn(
    typeof(VehicleManagementDomainModule),
    typeof(VehicleManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule),
    typeof(LogManagementApplicationModule)
    )]
public class VehicleManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<VehicleManagementApplicationModule>();
    }
}
