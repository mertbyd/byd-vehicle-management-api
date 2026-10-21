using Localization.Resources.AbpUi;
using Byd.VehicleManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Byd.VehicleManagement;

[DependsOn(
    typeof(VehicleManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class VehicleManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(VehicleManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<VehicleManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
