using Byd.VehicleManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Byd.VehicleManagement;

public abstract class VehicleManagementController : AbpControllerBase
{
    protected VehicleManagementController()
    {
        LocalizationResource = typeof(VehicleManagementResource);
    }
}
