using Volo.Abp.Modularity;

namespace Byd.VehicleManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class VehicleManagementDomainTestBase<TStartupModule> : VehicleManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
