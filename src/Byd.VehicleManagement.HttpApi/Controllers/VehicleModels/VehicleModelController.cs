using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Byd.CoreExtensions.Querying;
using Byd.LogManagement.ActivityLogs;
using Byd.LogManagement.Attributes;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Dtos.VehicleModels;
using Byd.VehicleManagement.Localization;
using Byd.VehicleManagement.Permissions;
using Byd.VehicleManagement.Services.VehicleModels;
using SystemStandards.Results;
using Volo.Abp.Application.Dtos;

namespace Byd.VehicleManagement.Controllers.VehicleModels;

// islevi: Arac modeli islemlerinin HTTP rota, yetki ve activity log metadatasini sunar.
// sistemdeki gorevi: VehicleModel use case'lerini ince Controller uzerinden AppService'e yonlendirir.
[ApiController]
[Route(VehicleModelRoutes.Root)]
public class VehicleModelController : VehicleManagementController
{
    private IVehicleModelAppService AppService => LazyGetRequiredService<IVehicleModelAppService>();

    /// <summary>Istegin tenant'ina yeni arac modeli ekler.</summary>
    /// <param name="input">Uretici ve model adi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   Manufacturer (string) → Uretici; zorunlu, en fazla 100 karakter
    ///   Name         (string) → Model adi; zorunlu, en fazla 100 karakter
    /// }
    /// Response {
    ///   Id                   (Guid)      → Yeni modelin kimligi
    ///   Manufacturer         (string)    → Kayitli uretici
    ///   Name                 (string)    → Kayitli model adi
    ///   CreationTime         (DateTime)  → Olusturma zamani
    ///   LastModificationTime (DateTime?) → Son guncelleme zamani; yeni kayitta bos
    /// }
    /// Model tenant katalogudur; tenant icindeki tum organizasyonlar ayni kaydi kullanir. Ayni uretici ve model adi
    /// tenant'ta ikinci kez acilamaz. Tenant'siz istek reddedilir. Bu endpoint arac tipi olusturmaz.
    /// </remarks>
    [HttpPost]
    [Authorize(VehicleManagementPermissions.VehicleModels.Manage)]
    public virtual async Task<Result<VehicleModelDto>> CreateAsync(
        [FromBody] CreateVehicleModelDto input, CancellationToken cancellationToken = default)
        => await AppService.CreateAsync(input, cancellationToken);

    /// <summary>Kimligi verilen arac modelini getirir.</summary>
    /// <param name="id">Okunacak modelin kimligi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response { Id (Guid) → Model kimligi; Manufacturer (string) → Uretici; Name (string) → Model adi; CreationTime
    /// (DateTime), LastModificationTime (DateTime?) → Audit zamanlari }
    /// Baska tenant'in modeli bulunamadi olarak doner. Modele bagli arac tipleri bu endpointte okunmaz.
    /// </remarks>
    [HttpGet(VehicleModelRoutes.ById)]
    [Authorize(VehicleManagementPermissions.VehicleModels.View)]
    public virtual async Task<Result<VehicleModelDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await AppService.GetAsync(id, cancellationToken);

    /// <summary>Filtre, siralama ve sayfalama uygulanmis arac modellerini getirir.</summary>
    /// <param name="input">Model filtre, siralama ve sayfalama bilgileri.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   Page     (int)                 → Sayfa numarasi; varsayilan 1
    ///   PageSize (int)                 → Sayfa boyutu; varsayilan 10, en fazla 1000
    ///   Sorting  (DataTableSorting[]?) → Siralama alanlari ve yonleri
    ///   Filters  (VehicleModelFilter?) → Alan bazli filtreler; her alan { Operator, Value, SecondValue, Values } tasir
    ///     Manufacturer (DataTableFilterItem?) → Uretici filtresi
    ///     Name         (DataTableFilterItem?) → Model adi filtresi
    ///     CreationTime (DataTableFilterItem?) → Kayit zamani filtresi
    /// }
    /// Response { TotalCount (long) → Filtreye uyan toplam kayit; Items (VehicleModelDto[]) → Sayfadaki modeller }
    /// Sayfa yalnizca istegin tenant'indaki modelleri tasir. Eslesen kayit yoksa toplam sayi sifir ve liste bos doner.
    /// Bu endpoint kayit yazmaz.
    /// </remarks>
    [HttpGet(VehicleModelRoutes.FilteredList)]
    [Authorize(VehicleManagementPermissions.VehicleModels.View)]
    public virtual async Task<Result<PagedResultDto<VehicleModelDto>>> GetFilteredListAsync(
        [FromQuery] DataTableListInput<VehicleModelFilter> input, CancellationToken cancellationToken = default)
        => await AppService.GetFilteredListAsync(input, cancellationToken);

    /// <summary>Kimligi verilen arac modelinin uretici ve adini gunceller.</summary>
    /// <param name="id">Guncellenecek modelin kimligi.</param>
    /// <param name="input">Yeni uretici ve model adi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   Manufacturer (string) → Yeni uretici; zorunlu, en fazla 100 karakter
    ///   Name         (string) → Yeni model adi; zorunlu, en fazla 100 karakter
    /// }
    /// Response { Id (Guid) → Model kimligi; Manufacturer (string), Name (string) → Guncel degerler; audit zamanlari }
    /// Modele bagli tiplerin cevaplarindaki model adi da bu degisiklikle guncellenir. Degisiklik activity log'a yazilir.
    /// </remarks>
    [HttpPut(VehicleModelRoutes.ById)]
    [Authorize(VehicleManagementPermissions.VehicleModels.Manage)]
    [TrackActivity<VehicleManagementResource>(
        VehicleManagementLocalizationKeys.ActivityLogs.VehicleModelUpdated,
        ActivityKind.Updated, nameof(VehicleModelDto.Id))]
    public virtual async Task<Result<VehicleModelDto>> UpdateAsync(
        Guid id, [FromBody] UpdateVehicleModelDto input, CancellationToken cancellationToken = default)
        => await AppService.UpdateAsync(id, input, cancellationToken);

    /// <summary>Kimligi verilen arac modelini siler.</summary>
    /// <param name="id">Silinecek modelin kimligi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response govdesi yoktur; silme kalicidir ve activity log'a yazilir. Tenant'taki herhangi bir organizasyonun arac
    /// tipi modeli kullaniyorsa iliski kurali silmeyi engeller.
    /// </remarks>
    [HttpDelete(VehicleModelRoutes.ById)]
    [Authorize(VehicleManagementPermissions.VehicleModels.Manage)]
    [TrackActivity<VehicleManagementResource>(
        VehicleManagementLocalizationKeys.ActivityLogs.VehicleModelDeleted,
        ActivityKind.Deleted, nameof(VehicleModelDto.Id))]
    public virtual async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await AppService.DeleteAsync(id, cancellationToken);
        return Result.NoContent();
    }
}
