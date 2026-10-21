using Byd.VehicleManagement.Localization;
using Volo.Abp.Application.Services;

namespace Byd.VehicleManagement;

// islevi: Arac yonetimi AppService'lerinin ortak tabanidir; lokalizasyon kaynagini ve servislerin gec cozumunu saglar.
// sistemdeki gorevi: Her arac yonetimi use case servisi bu tabandan turer.
public abstract class VehicleManagementAppService : ApplicationService
{
    // ABP servislerini kullanim aninda cozer.
    protected T LazyGetRequiredService<T>() where T : notnull =>
        LazyServiceProvider.LazyGetRequiredService<T>();

    protected VehicleManagementAppService()
    {
        LocalizationResource = typeof(VehicleManagementResource);
        ObjectMapperContext = typeof(VehicleManagementApplicationModule);
    }
}
