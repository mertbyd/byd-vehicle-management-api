using Byd.VehicleManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Byd.VehicleManagement;

// islevi: Arac yonetimi controller'larinin ortak tabanidir; lokalizasyon kaynagini ve servislerin gec cozumunu saglar.
// sistemdeki gorevi: Her arac yonetimi HTTP ucu bu tabandan turer.
public abstract class VehicleManagementController : AbpControllerBase
{
    // ABP servislerini kullanim aninda cozer.
    protected T LazyGetRequiredService<T>() where T : notnull =>
        LazyServiceProvider.LazyGetRequiredService<T>();

    protected VehicleManagementController()
    {
        LocalizationResource = typeof(VehicleManagementResource);
    }
}
