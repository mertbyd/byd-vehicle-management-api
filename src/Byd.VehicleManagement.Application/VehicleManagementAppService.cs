using Byd.VehicleManagement.Localization;
using Volo.Abp.Application.Services;

namespace Byd.VehicleManagement;

public abstract class VehicleManagementAppService : ApplicationService
{
    protected VehicleManagementAppService()
    {
        LocalizationResource = typeof(VehicleManagementResource);
        ObjectMapperContext = typeof(VehicleManagementApplicationModule);
    }
}
