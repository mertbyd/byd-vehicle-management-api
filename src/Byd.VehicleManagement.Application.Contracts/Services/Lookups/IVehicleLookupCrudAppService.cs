using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Byd.VehicleManagement.Dtos.Lookups;
using Volo.Abp.Application.Services;

namespace Byd.VehicleManagement.Services.Lookups;

// islevi: Arac lookup turlerinin olustur, oku, listele, guncelle ve sil API sozlesmesini tanimlar.
// sistemdeki gorevi: Her lookup turunun ayni CRUD sozlesmesini paylasmasini saglar.
public interface IVehicleLookupCrudAppService : IApplicationService
{
    Task<VehicleLookupDto> CreateAsync(CreateVehicleLookupDto input, CancellationToken cancellationToken = default);
    Task<VehicleLookupDto> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<List<VehicleLookupDto>> GetListAsync(CancellationToken cancellationToken = default);
    Task<VehicleLookupDto> UpdateAsync(int id, UpdateVehicleLookupDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
