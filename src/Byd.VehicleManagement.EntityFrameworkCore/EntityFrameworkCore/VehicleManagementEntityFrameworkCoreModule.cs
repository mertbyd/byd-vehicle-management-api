using Microsoft.Extensions.DependencyInjection;
using Byd.AuthManagement.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Byd.VehicleManagement.EntityFrameworkCore;

[DependsOn(
    typeof(VehicleManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AuthManagementEntityFrameworkCoreModule)
)]
public class VehicleManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<VehicleManagementDbContext>(options => { options.AddDefaultRepositories(true); });
    }
}
