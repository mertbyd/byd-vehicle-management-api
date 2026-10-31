using Volo.Abp.Modularity;

namespace Byd.VehicleManagement.Consumer;

// islevi: Arac modulunun baska modullerden gelen dagitik olay handler'larini tasir.
// sistemdeki gorevi: Handler'lar ayri pakette kalir; arac modulunu NuGet olarak alan host onlari istemeden almaz.
[DependsOn(typeof(VehicleManagementDomainModule))]
public class VehicleManagementConsumerModule : AbpModule
{
}
