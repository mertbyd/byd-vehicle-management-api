using System;
using System.Threading;
using System.Threading.Tasks;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.VehicleEquipments;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Byd.VehicleManagement.Services.VehicleEquipments;

// islevi: Arac donanimi kayitlarinin olustur, oku, filtreli listele, guncelle ve sil API sozlesmesini tanimlar.
// sistemdeki gorevi: HTTP ve modul tuketicilerini implementation'dan ayirir.
public interface IVehicleEquipmentAppService : IApplicationService
{
    Task<VehicleEquipmentDto> CreateAsync(CreateVehicleEquipmentDto input, CancellationToken cancellationToken = default);
    Task<VehicleEquipmentDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<VehicleEquipmentDto>> GetFilteredListAsync(
        DataTableListInput<VehicleEquipmentFilter> input, CancellationToken cancellationToken = default);
    Task<VehicleEquipmentDto> UpdateAsync(
        Guid id, UpdateVehicleEquipmentDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
