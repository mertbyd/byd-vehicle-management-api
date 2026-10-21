using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Byd.CoreExtensions.Querying;
using Byd.LogManagement.ActivityLogs;
using Byd.LogManagement.Attributes;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Dtos.VehicleTypes;
using Byd.VehicleManagement.Localization;
using Byd.VehicleManagement.Permissions;
using Byd.VehicleManagement.Services.VehicleTypes;
using SystemStandards.Results;
using Volo.Abp.Application.Dtos;

namespace Byd.VehicleManagement.Controllers.VehicleTypes;

// islevi: Arac tipi islemlerinin HTTP rota, yetki ve activity log metadatasini sunar.
// sistemdeki gorevi: VehicleType use case'lerini ince Controller uzerinden AppService'e yonlendirir.
[ApiController]
[Route(VehicleTypeRoutes.Root)]
public class VehicleTypeController : VehicleManagementController
{
    private IVehicleTypeAppService AppService => LazyGetRequiredService<IVehicleTypeAppService>();

    /// <summary>Istegin organizasyonuna yeni arac tipi ekler.</summary>
    /// <param name="input">Model, ulasim modu, yakit, kod, ad, kapasite ve erisilebilirlik alanlari.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   VehicleModelId          (Guid?)    → Uretici modeli; istege bagli, istegin tenant'inda olmali
    ///   TransportModeId         (int)      → Ulasim modu lookup kimligi; zorunlu
    ///   FuelTypeId              (int)      → Yakit turu lookup kimligi; zorunlu
    ///   Code                    (string)   → Organizasyonda tekil kod; zorunlu, en fazla 50 karakter
    ///   Name                    (string)   → Gorunen ad; zorunlu, en fazla 200 karakter
    ///   SeatingCapacity         (int)      → Oturan kapasitesi; sifir veya daha buyuk
    ///   StandingCapacity        (int)      → Ayakta kapasitesi; sifir veya daha buyuk
    ///   WheelchairPlaceCapacity (int)      → Tekerlekli sandalye yeri; sifir veya daha buyuk
    ///   BicycleCapacity         (int)      → Bisiklet yeri; sifir veya daha buyuk
    ///   LowFloor                (bool)     → Alcak taban
    ///   HasLiftOrRamp           (bool)     → Rampa veya lift
    ///   Length                  (decimal?) → Uzunluk, metre; verildiyse sifirdan buyuk
    /// }
    /// Response {
    ///   Id, VehicleModelId, TransportModeId, FuelTypeId → Kimlik ve baglar
    ///   VehicleModelName  (string?) → Model adi; model baglanmadiysa bos
    ///   TransportModeName (string)  → Ulasim modu adi
    ///   FuelTypeName      (string)  → Yakit turu adi
    ///   Code, Name, kapasiteler, LowFloor, HasLiftOrRamp, Length → Kayitli degerler
    ///   CreationTime (DateTime), LastModificationTime (DateTime?) → Audit zamanlari
    /// }
    /// Tip istegin organizasyonuna yazilir; tenant ve organizasyonsuz istek reddedilir. Model, mod ya da yakit
    /// bulunamazsa kodlu hata doner. Bu endpoint arac kaydi olusturmaz.
    /// </remarks>
    [HttpPost]
    [Authorize(VehicleManagementPermissions.VehicleTypes.Manage)]
    public virtual async Task<Result<VehicleTypeDto>> CreateAsync(
        [FromBody] CreateVehicleTypeDto input, CancellationToken cancellationToken = default)
        => await AppService.CreateAsync(input, cancellationToken);

    /// <summary>Kimligi verilen arac tipini getirir.</summary>
    /// <param name="id">Okunacak tipin kimligi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response { VehicleTypeDto → Kimlik, baglar, VehicleModelName, TransportModeName, FuelTypeName, kapasite ve
    /// erisilebilirlik alanlari, audit zamanlari }
    /// Baska organizasyonun tipi bulunamadi olarak doner. Tipe bagli araclar bu endpointte okunmaz.
    /// </remarks>
    [HttpGet(VehicleTypeRoutes.ById)]
    [Authorize(VehicleManagementPermissions.VehicleTypes.View)]
    public virtual async Task<Result<VehicleTypeDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await AppService.GetAsync(id, cancellationToken);

    /// <summary>Filtre, siralama ve sayfalama uygulanmis arac tiplerini getirir.</summary>
    /// <param name="input">Tip filtre, siralama ve sayfalama bilgileri.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   Page     (int)                 → Sayfa numarasi; varsayilan 1
    ///   PageSize (int)                 → Sayfa boyutu; varsayilan 10, en fazla 1000
    ///   Sorting  (DataTableSorting[]?) → Siralama alanlari ve yonleri
    ///   Filters  (VehicleTypeFilter?)  → Alan bazli filtreler; her alan { Operator, Value, SecondValue, Values } tasir
    ///     VehicleModelId  (DataTableFilterItem?) → Uretici modeli filtresi
    ///     TransportModeId (DataTableFilterItem?) → Ulasim modu filtresi
    ///     FuelTypeId      (DataTableFilterItem?) → Yakit turu filtresi
    ///     Code            (DataTableFilterItem?) → Kod filtresi
    ///     Name            (DataTableFilterItem?) → Ad filtresi
    ///     LowFloor        (DataTableFilterItem?) → Alcak taban filtresi
    ///     CreationTime    (DataTableFilterItem?) → Kayit zamani filtresi
    /// }
    /// Response { TotalCount (long) → Filtreye uyan toplam kayit; Items (VehicleTypeDto[]) → Sayfadaki tipler, bagli
    /// model, mod ve yakit adlariyla }
    /// Sayfa yalnizca istegin organizasyonundaki tipleri tasir. Eslesen kayit yoksa toplam sayi sifir ve liste bos
    /// doner. Bu endpoint kayit yazmaz.
    /// </remarks>
    [HttpGet(VehicleTypeRoutes.FilteredList)]
    [Authorize(VehicleManagementPermissions.VehicleTypes.View)]
    public virtual async Task<Result<PagedResultDto<VehicleTypeDto>>> GetFilteredListAsync(
        [FromQuery] DataTableListInput<VehicleTypeFilter> input, CancellationToken cancellationToken = default)
        => await AppService.GetFilteredListAsync(input, cancellationToken);

    /// <summary>Kimligi verilen arac tipinin baglarini ve alanlarini gunceller.</summary>
    /// <param name="id">Guncellenecek tipin kimligi.</param>
    /// <param name="input">Yeni model, ulasim modu, yakit, kod, ad, kapasite ve erisilebilirlik alanlari.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   VehicleModelId          (Guid?)    → Uretici modeli; bos gonderilirse model bagi kaldirilir
    ///   TransportModeId         (int)      → Ulasim modu lookup kimligi; zorunlu
    ///   FuelTypeId              (int)      → Yakit turu lookup kimligi; zorunlu
    ///   Code                    (string)   → Organizasyonda tekil kod; zorunlu, en fazla 50 karakter
    ///   Name                    (string)   → Gorunen ad; zorunlu, en fazla 200 karakter
    ///   SeatingCapacity         (int)      → Oturan kapasitesi; sifir veya daha buyuk
    ///   StandingCapacity        (int)      → Ayakta kapasitesi; sifir veya daha buyuk
    ///   WheelchairPlaceCapacity (int)      → Tekerlekli sandalye yeri; sifir veya daha buyuk
    ///   BicycleCapacity         (int)      → Bisiklet yeri; sifir veya daha buyuk
    ///   LowFloor                (bool)     → Alcak taban
    ///   HasLiftOrRamp           (bool)     → Rampa veya lift
    ///   Length                  (decimal?) → Uzunluk, metre; verildiyse sifirdan buyuk
    /// }
    /// Response { VehicleTypeDto → Guncel baglar, bagli kayitlarin adlari ve alanlar }
    /// Tenant ve organizasyon degistirilemez. Degisiklik activity log'a yazilir.
    /// </remarks>
    [HttpPut(VehicleTypeRoutes.ById)]
    [Authorize(VehicleManagementPermissions.VehicleTypes.Manage)]
    [TrackActivity<VehicleManagementResource>(
        VehicleManagementLocalizationKeys.ActivityLogs.VehicleTypeUpdated,
        ActivityKind.Updated, nameof(VehicleTypeDto.Id))]
    public virtual async Task<Result<VehicleTypeDto>> UpdateAsync(
        Guid id, [FromBody] UpdateVehicleTypeDto input, CancellationToken cancellationToken = default)
        => await AppService.UpdateAsync(id, input, cancellationToken);

    /// <summary>Kimligi verilen arac tipini siler.</summary>
    /// <param name="id">Silinecek tipin kimligi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response govdesi yoktur; silme kalicidir ve activity log'a yazilir. Tipi kullanan kayit varsa iliski kurali
    /// silmeyi engeller. Bagli model ve lookup satirlari silinmez.
    /// </remarks>
    [HttpDelete(VehicleTypeRoutes.ById)]
    [Authorize(VehicleManagementPermissions.VehicleTypes.Manage)]
    [TrackActivity<VehicleManagementResource>(
        VehicleManagementLocalizationKeys.ActivityLogs.VehicleTypeDeleted,
        ActivityKind.Deleted, nameof(VehicleTypeDto.Id))]
    public virtual async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await AppService.DeleteAsync(id, cancellationToken);
        return Result.NoContent();
    }
}
