using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.Vehicles;
using Byd.VehicleManagement.Interfaces.Vehicles;
using Byd.VehicleManagement.Managers.Vehicles;
using Byd.VehicleManagement.Mappers.Vehicles;
using Volo.Abp.Application.Dtos;

namespace Byd.VehicleManagement.Services.Vehicles;

// islevi: Arac kayitlarinin olustur, oku, filtreli listele, guncelle, sil ve realtime bul-ya da-ekle akisini yurutur.
// sistemdeki gorevi: Controller ve realtime modulu ile Vehicle Manager/Repository zinciri arasindaki tek giris
// noktasidir; is kararlari Manager'da, sorgu repository'de kalir.
public class VehicleAppService : VehicleManagementAppService, IVehicleAppService
{
    private static readonly VehicleMapper Mapper = new();

    private VehicleManager Manager => LazyGetRequiredService<VehicleManager>();

    private IVehicleRepository Repository => LazyGetRequiredService<IVehicleRepository>();

    // Girdi tipinin FluentValidation dogrulayicisini kullanim aninda cozer.
    private IValidator<TInput> Validator<TInput>() => LazyGetRequiredService<IValidator<TInput>>();

    // Girdiyi dogrular, kaydi istegin tenant ve organizasyonunda kurar ve kalici hale getirir.
    public virtual async Task<VehicleDto> CreateAsync(
        CreateVehicleDto input, CancellationToken cancellationToken = default)
    {
        await Validator<CreateVehicleDto>().ValidateAndThrowAsync(input, cancellationToken);
        var entity = await Manager.CreateAsync(Mapper.Map(input), cancellationToken);
        await Repository.InsertAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.Map(entity);
    }

    // Kimligi verilen kaydi istegin organizasyonunda getirir.
    public virtual async Task<VehicleDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => Mapper.Map((await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!);

    // Liste girdisini dogrular, filtreli sayfayi ve toplam sayiyi tek sonuc olarak getirir.
    public virtual async Task<PagedResultDto<VehicleDto>> GetFilteredListAsync(
        DataTableListInput<VehicleFilter> input, CancellationToken cancellationToken = default)
    {
        await Validator<DataTableListInput<VehicleFilter>>().ValidateAndThrowAsync(input, cancellationToken);
        var entities = await Repository.GetFilteredListAsync(input, cancellationToken);
        var totalCount = await Repository.GetFilteredCountAsync(input, cancellationToken);
        return new PagedResultDto<VehicleDto>(totalCount, entities.Select(entity => Mapper.Map(entity)).ToList());
    }

    // Girdiyi dogrular, kaydin duzenlenebilir alanlarini Manager ile gunceller ve kaydeder.
    public virtual async Task<VehicleDto> UpdateAsync(
        Guid id, UpdateVehicleDto input, CancellationToken cancellationToken = default)
    {
        await Validator<UpdateVehicleDto>().ValidateAndThrowAsync(input, cancellationToken);
        var entity = (await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!;
        await Manager.UpdateAsync(entity, Mapper.Map(input), cancellationToken);
        await Repository.UpdateAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.Map(entity);
    }

    // Realtime'da gorulen araci dogrular; organizasyonun cache'teki listesinde isletmecisi ve GTFS-RT kimligiyle varsa
    // onu, yoksa yeni realtime kaydini dondurur. Her pozisyon mesajinda veritabanina gidilmez, yalniz yeni arac yazilir.
    public virtual async Task<VehicleRealtimeDto> FindOrRegisterRealtimeAsync(
        RegisterRealtimeVehicleDto input, CancellationToken cancellationToken = default)
    {
        await Validator<RegisterRealtimeVehicleDto>().ValidateAndThrowAsync(input, cancellationToken);
        var cached = await Manager.FindCachedByGtfsVehicleIdAsync(
            input.GtfsAgencyId, input.GtfsVehicleId, cancellationToken);
        if (cached is not null)
            return Mapper.Map(cached);
        var entity = await Manager.RegisterFromRealtimeAsync(Mapper.Map(input), cancellationToken);
        await Repository.InsertAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        return Mapper.MapToRealtime(entity);
    }

    // Araci kendi donanimi disinda kullanan kayit yoksa donanimiyla birlikte kalici olarak siler.
    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = (await Manager.TryGetByIdAsync(id, cancellationToken: cancellationToken))!;
        await Manager.EnsureCanDeleteAsync(entity, cancellationToken);
        await Repository.DeleteAsync(entity, autoSave: true, cancellationToken: cancellationToken);
    }
}
