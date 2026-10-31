using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.VehicleEquipments;
using Byd.VehicleManagement.Interfaces.VehicleEquipments;
using Byd.VehicleManagement.Managers.VehicleEquipments;
using Byd.VehicleManagement.Mappers.VehicleEquipments;
using Volo.Abp.Application.Dtos;

namespace Byd.VehicleManagement.Services.VehicleEquipments;

// islevi: Arac donanimi kayitlarinin olustur, oku, filtreli listele, guncelle ve sil akisini yurutur.
// sistemdeki gorevi: Controller ile VehicleEquipment Manager/Repository zinciri arasindaki tek public giris noktasidir;
// is kararlari Manager'da, sorgu repository'de kalir.
public class VehicleEquipmentAppService : VehicleManagementAppService, IVehicleEquipmentAppService
{
    private static readonly VehicleEquipmentMapper Mapper = new();

    private VehicleEquipmentManager Manager => LazyGetRequiredService<VehicleEquipmentManager>();

    private IVehicleEquipmentRepository Repository => LazyGetRequiredService<IVehicleEquipmentRepository>();

    // Girdi tipinin FluentValidation dogrulayicisini kullanim aninda cozer.
    private IValidator<TInput> Validator<TInput>() => LazyGetRequiredService<IValidator<TInput>>();

    // Girdiyi dogrular, donanimi istegin organizasyonundaki araca kurar ve kalici hale getirir.
    public virtual async Task<VehicleEquipmentDto> CreateAsync(
        CreateVehicleEquipmentDto input, CancellationToken cancellationToken = default)
    {
        await Validator<CreateVehicleEquipmentDto>().ValidateAndThrowAsync(input, cancellationToken);
        var entity = await Manager.CreateAsync(Mapper.Map(input), cancellationToken);
        await Repository.InsertAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.Map(entity);
    }

    // Kimligi verilen kaydi istegin organizasyonunda getirir.
    public virtual async Task<VehicleEquipmentDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => Mapper.Map((await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!);

    // Liste girdisini dogrular, filtreli sayfayi ve toplam sayiyi tek sonuc olarak getirir.
    public virtual async Task<PagedResultDto<VehicleEquipmentDto>> GetFilteredListAsync(
        DataTableListInput<VehicleEquipmentFilter> input, CancellationToken cancellationToken = default)
    {
        await Validator<DataTableListInput<VehicleEquipmentFilter>>().ValidateAndThrowAsync(input, cancellationToken);
        var entities = await Repository.GetFilteredListAsync(input, cancellationToken);
        var totalCount = await Repository.GetFilteredCountAsync(input, cancellationToken);
        return new PagedResultDto<VehicleEquipmentDto>(
            totalCount, entities.Select(entity => Mapper.Map(entity)).ToList());
    }

    // Girdiyi dogrular, donanimin turunu ve adedini Manager ile gunceller ve kaydeder.
    public virtual async Task<VehicleEquipmentDto> UpdateAsync(
        Guid id, UpdateVehicleEquipmentDto input, CancellationToken cancellationToken = default)
    {
        await Validator<UpdateVehicleEquipmentDto>().ValidateAndThrowAsync(input, cancellationToken);
        var entity = (await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!;
        await Manager.UpdateAsync(entity, Mapper.Map(input), cancellationToken);
        await Repository.UpdateAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.Map(entity);
    }

    // Donanimi kalici olarak siler; arac ve donanim turu satiri silinmez.
    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = (await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!;
        await Repository.DeleteAsync(entity, autoSave: true, cancellationToken: cancellationToken);
    }
}
