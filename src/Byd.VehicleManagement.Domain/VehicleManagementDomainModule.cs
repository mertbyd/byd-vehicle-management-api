using Byd.AuthManagement;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Byd.VehicleManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(VehicleManagementDomainSharedModule),
    typeof(AuthManagementDomainModule)
)]
public class VehicleManagementDomainModule : AbpModule
{

}
