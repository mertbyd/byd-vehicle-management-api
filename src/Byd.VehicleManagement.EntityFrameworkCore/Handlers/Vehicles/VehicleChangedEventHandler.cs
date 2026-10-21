using System.Threading.Tasks;
using Byd.VehicleManagement.Entities.Vehicles;
using Byd.VehicleManagement.Interfaces.Vehicles;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: Arac satiri degistiginde organizasyonun cache'teki arac listesini siler.
// sistemdeki gorevi: CRUD uclarindan ve realtime kaydindan gelen yazmalar IVehicleRepository uzerinden yapilir; cache'li
// repository yalniz kendi uzerinden yazilani gorur. Bu handler listeyi tazeler, sonraki okuma tabloyu yeniden yukler.
public class VehicleChangedEventHandler(IVehicleCacheRepository cacheRepository)
    : ILocalEventHandler<EntityChangedEventData<Vehicle>>, ITransientDependency
{
    // Degisen aracin organizasyonuna ait listeyi siler.
    public Task HandleEventAsync(EntityChangedEventData<Vehicle> eventData)
        => cacheRepository.HandleEntityChangedAsync(eventData);
}
