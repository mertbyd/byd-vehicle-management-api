using System;
using System.Threading;
using System.Threading.Tasks;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.Vehicles;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Byd.VehicleManagement.Services.Vehicles;

// islevi: Arac kayitlarinin olustur, oku, filtreli listele, guncelle, sil ve realtime bul-ya da-ekle sozlesmesini
// tanimlar.
// sistemdeki gorevi: HTTP ve modul tuketicilerini implementation'dan ayirir. Realtime metodu realtime modulunun surec
// ici cagirdigi giris noktasidir; uzak servis olarak acilmaz, HTTP ucu ve istemci proxy'si uretilmez.
public interface IVehicleAppService : IApplicationService
{
    Task<VehicleDto> CreateAsync(CreateVehicleDto input, CancellationToken cancellationToken = default);
    Task<VehicleDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<VehicleDto>> GetFilteredListAsync(
        DataTableListInput<VehicleFilter> input, CancellationToken cancellationToken = default);
    Task<VehicleDto> UpdateAsync(Guid id, UpdateVehicleDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    [RemoteService(IsEnabled = false)]
    Task<VehicleRealtimeDto> FindOrRegisterRealtimeAsync(
        RegisterRealtimeVehicleDto input, CancellationToken cancellationToken = default);
}
