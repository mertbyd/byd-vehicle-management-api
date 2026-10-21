using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.VehicleModels;
using Byd.VehicleManagement.Interfaces.VehicleModels;
using Byd.VehicleManagement.Managers.VehicleModels;
using Byd.VehicleManagement.Mappers.VehicleModels;
using Volo.Abp.Application.Dtos;

namespace Byd.VehicleManagement.Services.VehicleModels;

// islevi: Arac modeli kayitlarinin olustur, oku, filtreli listele, guncelle ve sil akisini yurutur.
// sistemdeki gorevi: Controller ile VehicleModel Manager/Repository zinciri arasindaki tek public giris noktasidir; is
// kararlari Manager'da, sorgu repository'de kalir.
public class VehicleModelAppService : VehicleManagementAppService, IVehicleModelAppService
{
    private static readonly VehicleModelMapper Mapper = new();

    private VehicleModelManager Manager => LazyGetRequiredService<VehicleModelManager>();

    private IVehicleModelRepository Repository => LazyGetRequiredService<IVehicleModelRepository>();

    // Girdi tipinin FluentValidation dogrulayicisini kullanim aninda cozer.
    private IValidator<TInput> Validator<TInput>() => LazyGetRequiredService<IValidator<TInput>>();

    // Girdiyi dogrular, kaydi istegin tenant'inda kurar ve kalici hale getirir.
    public virtual async Task<VehicleModelDto> CreateAsync(
        CreateVehicleModelDto input, CancellationToken cancellationToken = default)
    {
        await Validator<CreateVehicleModelDto>().ValidateAndThrowAsync(input, cancellationToken);
        var entity = await Manager.CreateAsync(Mapper.Map(input), cancellationToken);
        await Repository.InsertAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.Map(entity);
    }

    // Kimligi verilen kaydi istegin tenant'inda getirir.
    public virtual async Task<VehicleModelDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => Mapper.Map((await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!);

    // Liste girdisini dogrular, filtreli sayfayi ve toplam sayiyi tek sonuc olarak getirir.
    public virtual async Task<PagedResultDto<VehicleModelDto>> GetFilteredListAsync(
        DataTableListInput<VehicleModelFilter> input, CancellationToken cancellationToken = default)
    {
        await Validator<DataTableListInput<VehicleModelFilter>>().ValidateAndThrowAsync(input, cancellationToken);
        var entities = await Repository.GetFilteredListAsync(input, cancellationToken);
        var totalCount = await Repository.GetFilteredCountAsync(input, cancellationToken);
        return new PagedResultDto<VehicleModelDto>(totalCount, entities.Select(entity => Mapper.Map(entity)).ToList());
    }

    // Girdiyi dogrular, kaydin duzenlenebilir alanlarini Manager ile gunceller ve kaydeder.
    public virtual async Task<VehicleModelDto> UpdateAsync(
        Guid id, UpdateVehicleModelDto input, CancellationToken cancellationToken = default)
    {
        await Validator<UpdateVehicleModelDto>().ValidateAndThrowAsync(input, cancellationToken);
        var entity = (await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!;
        await Manager.UpdateAsync(entity, Mapper.Map(input), cancellationToken);
        await Repository.UpdateAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.Map(entity);
    }

    // Hicbir organizasyonun arac tipi kullanmiyorsa modeli kalici olarak siler.
    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = (await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!;
        await Manager.EnsureCanDeleteAsync(entity, cancellationToken);
        await Repository.DeleteAsync(entity, autoSave: true, cancellationToken: cancellationToken);
    }
}
