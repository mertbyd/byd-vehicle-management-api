using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Byd.CoreExtensions.Querying;
using Byd.LogManagement.ActivityLogs;
using Byd.LogManagement.Attributes;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Dtos.Vehicles;
using Byd.VehicleManagement.Localization;
using Byd.VehicleManagement.Permissions;
using Byd.VehicleManagement.Services.Vehicles;
using SystemStandards.Results;
using Volo.Abp.Application.Dtos;

namespace Byd.VehicleManagement.Controllers.Vehicles;

// islevi: Arac islemlerinin HTTP rota, yetki ve activity log metadatasini sunar.
// sistemdeki gorevi: Vehicle use case'lerini ince Controller uzerinden AppService'e yonlendirir.
[ApiController]
[Route(VehicleRoutes.Root)]
public class VehicleController : VehicleManagementController
{
    private IVehicleAppService AppService => LazyGetRequiredService<IVehicleAppService>();

    /// <summary>Istegin organizasyonuna yeni arac ekler.</summary>
    /// <param name="input">Tip, durum, GTFS-RT kimlikleri ve hizmet donemi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   VehicleTypeId          (Guid?)                      → Arac tipi; istege bagli, istegin organizasyonunda ve
    ///                                                         aracin kipinde olmali
    ///   TransportModeId        (int)                        → Ulasim modu lookup kimligi; zorunlu
    ///   VehicleStatusId        (int)                        → Hizmet durumu lookup kimligi; zorunlu
    ///   WheelchairAccessibleId (int?)                       → Tekerlekli sandalye erisimi; yalniz tipten farkliysa
    ///   GtfsVehicleId          (string)                     → GTFS-RT vehicle.id; zorunlu, organizasyonda GtfsAgencyId ile birlikte tekil, en fazla 100
    ///   Label                  (string?)                    → Yolcunun gordugu numara; en fazla 50
    ///   RegistrationNumber     (string?)                    → Plaka; kaynaktaki haliyle tutulur, tekil degildir, en fazla 20
    ///   GtfsAgencyId           (string?)                    → GTFS agency_id; en fazla 100
    ///   InServiceFrom          (DateOnly)                   → Hizmete giris gunu; zorunlu
    ///   InServiceUntil         (DateOnly?)                  → Hizmetten cikis gunu; verildiyse giris gununden once olamaz
    /// }
    /// Response {
    ///   Id, VehicleTypeId, TransportModeId, VehicleStatusId, RegistrationSourceId, WheelchairAccessibleId → Baglar
    ///   VehicleTypeName          (string?) → Arac tipi adi; tip atanmadiysa bos
    ///   TransportModeName        (string?) → Ulasim modu adi; realtime kaydinda route_type yoksa bos
    ///   VehicleStatusName        (string)  → Hizmet durumu adi
    ///   RegistrationSourceName   (string)  → Kayit kaynagi; elle eklenen arac Manuel
    ///   WheelchairAccessibleName (string?) → Erisim adi; tipten farkli deger yoksa bos
    ///   GtfsVehicleId, Label, RegistrationNumber, GtfsAgencyId, InServiceFrom, InServiceUntil → Kayitli degerler
    ///   CreationTime (DateTime), LastModificationTime (DateTime?) → Audit zamanlari
    /// }
    /// Arac istegin organizasyonuna yazilir; tenant ve organizasyonsuz istek reddedilir. Baska organizasyonun tipi
    /// ve farkli kipteki tip secilemez. Arac Manuel kaynakli kaydedilir. GTFS-RT kimligi organizasyonda ayni isletmeci
    /// icin ikinci kez kullanilamaz; plaka tekil sayilmaz. Model ve kapasite tipten okunur; bu endpoint tip ve donanim
    /// olusturmaz.
    /// </remarks>
    [HttpPost]
    [Authorize(VehicleManagementPermissions.Vehicles.Manage)]
    public virtual async Task<Result<VehicleDto>> CreateAsync(
        [FromBody] CreateVehicleDto input, CancellationToken cancellationToken = default)
        => await AppService.CreateAsync(input, cancellationToken);

    /// <summary>Kimligi verilen araci getirir.</summary>
    /// <param name="id">Okunacak aracin kimligi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response { VehicleDto → Kimlik, baglar, VehicleTypeName, VehicleStatusName, WheelchairAccessibleName, GTFS-RT
    /// kimlikleri, hizmet donemi ve audit zamanlari }
    /// Baska organizasyonun araci bulunamadi olarak doner. Aracin donanimlari ve canli konumu bu endpointte okunmaz.
    /// </remarks>
    [HttpGet(VehicleRoutes.ById)]
    [Authorize(VehicleManagementPermissions.Vehicles.View)]
    public virtual async Task<Result<VehicleDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await AppService.GetAsync(id, cancellationToken);

    /// <summary>Filtre, siralama ve sayfalama uygulanmis araclari getirir.</summary>
    /// <param name="input">Arac filtre, siralama ve sayfalama bilgileri.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   Page     (int)                 → Sayfa numarasi; varsayilan 1
    ///   PageSize (int)                 → Sayfa boyutu; varsayilan 10, en fazla 1000
    ///   Sorting  (DataTableSorting[]?) → Siralama alanlari ve yonleri
    ///   Filters  (VehicleFilter?)      → Alan bazli filtreler; her alan { Operator, Value, SecondValue, Values } tasir
    ///     VehicleTypeId        (DataTableFilterItem?) → Arac tipi filtresi
    ///     TransportModeId      (DataTableFilterItem?) → Ulasim modu filtresi
    ///     VehicleStatusId      (DataTableFilterItem?) → Hizmet durumu filtresi
    ///     RegistrationSourceId (DataTableFilterItem?) → Kayit kaynagi filtresi; tipi atanmamis realtime
    ///                                                  araclarini bulmak icin
    ///     GtfsVehicleId      (DataTableFilterItem?) → GTFS-RT vehicle.id filtresi
    ///     Label              (DataTableFilterItem?) → Arac numarasi filtresi
    ///     RegistrationNumber (DataTableFilterItem?) → Plaka filtresi
    ///     GtfsAgencyId       (DataTableFilterItem?) → GTFS agency_id filtresi
    ///     InServiceFrom      (DataTableFilterItem?) → Hizmete giris gunu filtresi
    ///     CreationTime       (DataTableFilterItem?) → Kayit zamani filtresi
    /// }
    /// Response { TotalCount (long) → Filtreye uyan toplam kayit; Items (VehicleDto[]) → Sayfadaki araclar, bagli kayit
    /// adlariyla }
    /// Sayfa yalnizca istegin organizasyonundaki araclari tasir. Eslesen kayit yoksa toplam sayi sifir ve liste bos
    /// doner. Bu endpoint kayit yazmaz.
    /// </remarks>
    [HttpGet(VehicleRoutes.FilteredList)]
    [Authorize(VehicleManagementPermissions.Vehicles.View)]
    public virtual async Task<Result<PagedResultDto<VehicleDto>>> GetFilteredListAsync(
        [FromQuery] DataTableListInput<VehicleFilter> input, CancellationToken cancellationToken = default)
        => await AppService.GetFilteredListAsync(input, cancellationToken);

    /// <summary>Kimligi verilen aracin baglarini ve alanlarini gunceller.</summary>
    /// <param name="id">Guncellenecek aracin kimligi.</param>
    /// <param name="input">Yeni tip, durum, GTFS-RT kimlikleri ve hizmet donemi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   VehicleTypeId          (Guid?)                      → Arac tipi; istege bagli, istegin organizasyonunda ve
    ///                                                         aracin kipinde olmali
    ///   TransportModeId        (int)                        → Ulasim modu lookup kimligi; zorunlu
    ///   VehicleStatusId        (int)                        → Hizmet durumu lookup kimligi; zorunlu
    ///   WheelchairAccessibleId (int?)                       → Tekerlekli sandalye erisimi; yalniz tipten farkliysa
    ///   GtfsVehicleId          (string)                     → GTFS-RT vehicle.id; zorunlu, organizasyonda GtfsAgencyId ile birlikte tekil, en fazla 100
    ///   Label                  (string?)                    → Yolcunun gordugu numara; en fazla 50
    ///   RegistrationNumber     (string?)                    → Plaka; kaynaktaki haliyle tutulur, tekil degildir, en fazla 20
    ///   GtfsAgencyId           (string?)                    → GTFS agency_id; en fazla 100
    ///   InServiceFrom          (DateOnly)                   → Hizmete giris gunu; zorunlu
    ///   InServiceUntil         (DateOnly?)                  → Hizmetten cikis gunu; verildiyse giris gununden once olamaz
    /// }
    /// Response { VehicleDto → Guncel baglar, bagli kayitlarin adlari ve alanlar }
    /// Tenant, organizasyon ve kayit kaynagi degistirilemez. Donanimlar bu endpointte degismez. Degisiklik activity log'a yazilir.
    /// </remarks>
    [HttpPut(VehicleRoutes.ById)]
    [Authorize(VehicleManagementPermissions.Vehicles.Manage)]
    [TrackActivity<VehicleManagementResource>(
        VehicleManagementLocalizationKeys.ActivityLogs.VehicleUpdated,
        ActivityKind.Updated, nameof(VehicleDto.Id))]
    public virtual async Task<Result<VehicleDto>> UpdateAsync(
        Guid id, [FromBody] UpdateVehicleDto input, CancellationToken cancellationToken = default)
        => await AppService.UpdateAsync(id, input, cancellationToken);

    /// <summary>Kimligi verilen araci donanimlariyla birlikte siler.</summary>
    /// <param name="id">Silinecek aracin kimligi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response govdesi yoktur; silme kalicidir ve activity log'a yazilir. Aracin donanim satirlari da silinir; araci
    /// baska bir kayit kullaniyorsa iliski kurali silmeyi engeller. Bagli tip ve lookup satirlari silinmez.
    /// </remarks>
    [HttpDelete(VehicleRoutes.ById)]
    [Authorize(VehicleManagementPermissions.Vehicles.Manage)]
    [TrackActivity<VehicleManagementResource>(
        VehicleManagementLocalizationKeys.ActivityLogs.VehicleDeleted,
        ActivityKind.Deleted, nameof(VehicleDto.Id))]
    public virtual async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await AppService.DeleteAsync(id, cancellationToken);
        return Result.NoContent();
    }
}
