using Microsoft.Extensions.DependencyInjection;
using Byd.LogManagement;
using Byd.VehicleManagement.Entities.Vehicles;
using Byd.VehicleManagement.Etos.Vehicles;
using Volo.Abp.Domain.Entities.Events.Distributed;
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
        // Arac eklenince, guncellenince ve silinince ABP entity olayi VehicleEto ile kendiliginden yayinlanir.
        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.AutoEventSelectors.Add<Vehicle>();
            options.EtoMappings.Add<Vehicle, VehicleEto>();
        });
    }
}
