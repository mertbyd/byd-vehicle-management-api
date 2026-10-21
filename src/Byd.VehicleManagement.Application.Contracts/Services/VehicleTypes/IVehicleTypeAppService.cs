using System;
using System.Threading;
using System.Threading.Tasks;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.VehicleTypes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Byd.VehicleManagement.Services.VehicleTypes;

// islevi: Arac tipi kayitlarinin olustur, oku, filtreli listele, guncelle ve sil API sozlesmesini tanimlar.
// sistemdeki gorevi: HTTP ve modul tuketicilerini implementation'dan ayirir.
public interface IVehicleTypeAppService : IApplicationService
{
    Task<VehicleTypeDto> CreateAsync(CreateVehicleTypeDto input, CancellationToken cancellationToken = default);
    Task<VehicleTypeDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<VehicleTypeDto>> GetFilteredListAsync(
        DataTableListInput<VehicleTypeFilter> input, CancellationToken cancellationToken = default);
    Task<VehicleTypeDto> UpdateAsync(Guid id, UpdateVehicleTypeDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
