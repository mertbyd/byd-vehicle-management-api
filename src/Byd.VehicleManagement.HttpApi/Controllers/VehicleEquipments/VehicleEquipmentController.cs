using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Byd.CoreExtensions.Querying;
using Byd.LogManagement.ActivityLogs;
using Byd.LogManagement.Attributes;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Dtos.VehicleEquipments;
using Byd.VehicleManagement.Localization;
using Byd.VehicleManagement.Permissions;
using Byd.VehicleManagement.Services.VehicleEquipments;
using SystemStandards.Results;
using Volo.Abp.Application.Dtos;

namespace Byd.VehicleManagement.Controllers.VehicleEquipments;

// islevi: Arac donanimi islemlerinin HTTP rota, yetki ve activity log metadatasini sunar.
// sistemdeki gorevi: VehicleEquipment use case'lerini ince Controller uzerinden AppService'e yonlendirir.
[ApiController]
[Route(VehicleEquipmentRoutes.Root)]
public class VehicleEquipmentController : VehicleManagementController
{
    private IVehicleEquipmentAppService AppService => LazyGetRequiredService<IVehicleEquipmentAppService>();

    /// <summary>Istegin organizasyonundaki bir araca donanim ekler.</summary>
    /// <param name="input">Arac, donanim turu ve adet.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   VehicleId       (Guid) → Donanimin takilacagi arac; zorunlu, istegin organizasyonunda olmali
    ///   EquipmentTypeId (int)  → Donanim turu lookup kimligi; zorunlu, aracta ikinci kez eklenemez
    ///   Units           (int)  → Adet; en az 1
    /// }
    /// Response {
    ///   Id, VehicleId, EquipmentTypeId → Kimlik ve baglar
    ///   VehicleGtfsVehicleId (string) → Aracin GTFS-RT vehicle.id degeri
    ///   EquipmentTypeName    (string) → Donanim turu adi
    ///   Units                (int)    → Kayitli adet
    ///   CreationTime (DateTime), LastModificationTime (DateTime?) → Audit zamanlari
    /// }
    /// Donanim aracin organizasyonuna yazilir; tenant ve organizasyonsuz istek reddedilir. Baska organizasyonun araci
    /// bulunamadi olarak doner. Bu endpoint arac ve donanim turu olusturmaz.
    /// </remarks>
    [HttpPost]
    [Authorize(VehicleManagementPermissions.VehicleEquipments.Manage)]
    public virtual async Task<Result<VehicleEquipmentDto>> CreateAsync(
        [FromBody] CreateVehicleEquipmentDto input, CancellationToken cancellationToken = default)
        => await AppService.CreateAsync(input, cancellationToken);

    /// <summary>Kimligi verilen arac donanimini getirir.</summary>
    /// <param name="id">Okunacak donanimin kimligi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response { VehicleEquipmentDto → Kimlik, baglar, VehicleGtfsVehicleId, EquipmentTypeName, adet ve audit
    /// zamanlari }
    /// Baska organizasyonun donanimi bulunamadi olarak doner. Aracin diger alanlari bu endpointte okunmaz.
    /// </remarks>
    [HttpGet(VehicleEquipmentRoutes.ById)]
    [Authorize(VehicleManagementPermissions.VehicleEquipments.View)]
    public virtual async Task<Result<VehicleEquipmentDto>> GetAsync(
        Guid id, CancellationToken cancellationToken = default)
        => await AppService.GetAsync(id, cancellationToken);

    /// <summary>Filtre, siralama ve sayfalama uygulanmis arac donanimlarini getirir.</summary>
    /// <param name="input">Donanim filtre, siralama ve sayfalama bilgileri.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   Page     (int)                     → Sayfa numarasi; varsayilan 1
    ///   PageSize (int)                     → Sayfa boyutu; varsayilan 10, en fazla 1000
    ///   Sorting  (DataTableSorting[]?)     → Siralama alanlari ve yonleri
    ///   Filters  (VehicleEquipmentFilter?) → Alan bazli filtreler; her alan { Operator, Value, SecondValue, Values } tasir
    ///     VehicleId       (DataTableFilterItem?) → Arac filtresi; bir aracin donanimlarini okumak icin
    ///     EquipmentTypeId (DataTableFilterItem?) → Donanim turu filtresi
    ///     Units           (DataTableFilterItem?) → Adet filtresi
    ///     CreationTime    (DataTableFilterItem?) → Kayit zamani filtresi
    /// }
    /// Response { TotalCount (long) → Filtreye uyan toplam kayit; Items (VehicleEquipmentDto[]) → Sayfadaki
    /// donanimlar, aracin GTFS-RT kimligi ve tur adiyla }
    /// Sayfa yalnizca istegin organizasyonundaki donanimlari tasir. Eslesen kayit yoksa toplam sayi sifir ve liste bos
    /// doner. Bu endpoint kayit yazmaz.
    /// </remarks>
    [HttpGet(VehicleEquipmentRoutes.FilteredList)]
    [Authorize(VehicleManagementPermissions.VehicleEquipments.View)]
    public virtual async Task<Result<PagedResultDto<VehicleEquipmentDto>>> GetFilteredListAsync(
        [FromQuery] DataTableListInput<VehicleEquipmentFilter> input, CancellationToken cancellationToken = default)
        => await AppService.GetFilteredListAsync(input, cancellationToken);

    /// <summary>Kimligi verilen arac donaniminin turunu ve adedini gunceller.</summary>
    /// <param name="id">Guncellenecek donanimin kimligi.</param>
    /// <param name="input">Yeni donanim turu ve adet.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   EquipmentTypeId (int) → Donanim turu lookup kimligi; zorunlu, aracta baska satirda olmamali
    ///   Units           (int) → Adet; en az 1
    /// }
    /// Response { VehicleEquipmentDto → Guncel tur, adet ve bagli kayitlarin degerleri }
    /// Arac, tenant ve organizasyon degistirilemez. Degisiklik activity log'a yazilir.
    /// </remarks>
    [HttpPut(VehicleEquipmentRoutes.ById)]
    [Authorize(VehicleManagementPermissions.VehicleEquipments.Manage)]
    [TrackActivity<VehicleManagementResource>(
        VehicleManagementLocalizationKeys.ActivityLogs.VehicleEquipmentUpdated,
        ActivityKind.Updated, nameof(VehicleEquipmentDto.Id))]
    public virtual async Task<Result<VehicleEquipmentDto>> UpdateAsync(
        Guid id, [FromBody] UpdateVehicleEquipmentDto input, CancellationToken cancellationToken = default)
        => await AppService.UpdateAsync(id, input, cancellationToken);

    /// <summary>Kimligi verilen arac donanimini siler.</summary>
    /// <param name="id">Silinecek donanimin kimligi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response govdesi yoktur; silme kalicidir ve activity log'a yazilir. Arac ve donanim turu satiri silinmez.
    /// </remarks>
    [HttpDelete(VehicleEquipmentRoutes.ById)]
    [Authorize(VehicleManagementPermissions.VehicleEquipments.Manage)]
    [TrackActivity<VehicleManagementResource>(
        VehicleManagementLocalizationKeys.ActivityLogs.VehicleEquipmentDeleted,
        ActivityKind.Deleted, nameof(VehicleEquipmentDto.Id))]
    public virtual async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await AppService.DeleteAsync(id, cancellationToken);
        return Result.NoContent();
    }
}
