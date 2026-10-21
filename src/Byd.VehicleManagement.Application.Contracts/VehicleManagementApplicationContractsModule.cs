using Byd.AuthManagement;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Byd.VehicleManagement;

[DependsOn(
    typeof(VehicleManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(AuthManagementApplicationContractsModule)
    )]
public class VehicleManagementApplicationContractsModule : AbpModule
{

}
