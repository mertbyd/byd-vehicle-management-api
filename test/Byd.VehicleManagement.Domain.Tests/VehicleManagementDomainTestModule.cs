using Volo.Abp.Modularity;

namespace Byd.VehicleManagement;

[DependsOn(
    typeof(VehicleManagementDomainModule),
    typeof(VehicleManagementTestBaseModule)
)]
public class VehicleManagementDomainTestModule : AbpModule
{

}
