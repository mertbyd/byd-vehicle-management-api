using System;
using System.Threading;
using System.Threading.Tasks;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.VehicleModels;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Byd.VehicleManagement.Services.VehicleModels;

// islevi: Arac modeli kayitlarinin olustur, oku, filtreli listele, guncelle ve sil API sozlesmesini tanimlar.
// sistemdeki gorevi: HTTP ve modul tuketicilerini implementation'dan ayirir.
public interface IVehicleModelAppService : IApplicationService
{
    Task<VehicleModelDto> CreateAsync(CreateVehicleModelDto input, CancellationToken cancellationToken = default);
    Task<VehicleModelDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<VehicleModelDto>> GetFilteredListAsync(
        DataTableListInput<VehicleModelFilter> input, CancellationToken cancellationToken = default);
    Task<VehicleModelDto> UpdateAsync(Guid id, UpdateVehicleModelDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
