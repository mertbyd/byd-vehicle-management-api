using Volo.Abp.Modularity;

namespace Byd.VehicleManagement;

[DependsOn(
    typeof(VehicleManagementApplicationModule),
    typeof(VehicleManagementDomainTestModule)
    )]
public class VehicleManagementApplicationTestModule : AbpModule
{

}
