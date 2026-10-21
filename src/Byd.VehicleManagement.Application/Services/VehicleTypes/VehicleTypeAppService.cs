using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.VehicleTypes;
using Byd.VehicleManagement.Interfaces.VehicleTypes;
using Byd.VehicleManagement.Managers.VehicleTypes;
using Byd.VehicleManagement.Mappers.VehicleTypes;
using Volo.Abp.Application.Dtos;

namespace Byd.VehicleManagement.Services.VehicleTypes;

// islevi: Arac tipi kayitlarinin olustur, oku, filtreli listele, guncelle ve sil akisini yurutur.
// sistemdeki gorevi: Controller ile VehicleType Manager/Repository zinciri arasindaki tek public giris noktasidir; is
// kararlari Manager'da, sorgu repository'de kalir.
public class VehicleTypeAppService : VehicleManagementAppService, IVehicleTypeAppService
{
    private static readonly VehicleTypeMapper Mapper = new();

    private VehicleTypeManager Manager => LazyGetRequiredService<VehicleTypeManager>();

    private IVehicleTypeRepository Repository => LazyGetRequiredService<IVehicleTypeRepository>();

    // Girdi tipinin FluentValidation dogrulayicisini kullanim aninda cozer.
    private IValidator<TInput> Validator<TInput>() => LazyGetRequiredService<IValidator<TInput>>();

    // Girdiyi dogrular, kaydi istegin tenant ve organizasyonunda kurar ve kalici hale getirir.
    public virtual async Task<VehicleTypeDto> CreateAsync(
        CreateVehicleTypeDto input, CancellationToken cancellationToken = default)
    {
        await Validator<CreateVehicleTypeDto>().ValidateAndThrowAsync(input, cancellationToken);
        var entity = await Manager.CreateAsync(Mapper.Map(input), cancellationToken);
        await Repository.InsertAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.Map(entity);
    }

    // Kimligi verilen kaydi istegin organizasyonunda getirir.
    public virtual async Task<VehicleTypeDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => Mapper.Map((await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!);

    // Liste girdisini dogrular, filtreli sayfayi ve toplam sayiyi tek sonuc olarak getirir.
    public virtual async Task<PagedResultDto<VehicleTypeDto>> GetFilteredListAsync(
        DataTableListInput<VehicleTypeFilter> input, CancellationToken cancellationToken = default)
    {
        await Validator<DataTableListInput<VehicleTypeFilter>>().ValidateAndThrowAsync(input, cancellationToken);
        var entities = await Repository.GetFilteredListAsync(input, cancellationToken);
        var totalCount = await Repository.GetFilteredCountAsync(input, cancellationToken);
        return new PagedResultDto<VehicleTypeDto>(totalCount, entities.Select(entity => Mapper.Map(entity)).ToList());
    }

    // Girdiyi dogrular, kaydin duzenlenebilir alanlarini Manager ile gunceller ve kaydeder.
    public virtual async Task<VehicleTypeDto> UpdateAsync(
        Guid id, UpdateVehicleTypeDto input, CancellationToken cancellationToken = default)
    {
        await Validator<UpdateVehicleTypeDto>().ValidateAndThrowAsync(input, cancellationToken);
        var entity = (await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!;
        await Manager.UpdateAsync(entity, Mapper.Map(input), cancellationToken);
        await Repository.UpdateAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.Map(entity);
    }

    // Tipi kullanan kayit yoksa kalici olarak siler.
    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = (await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!;
        await Manager.EnsureCanDeleteAsync(entity, cancellationToken);
        await Repository.DeleteAsync(entity, autoSave: true, cancellationToken: cancellationToken);
    }
}
