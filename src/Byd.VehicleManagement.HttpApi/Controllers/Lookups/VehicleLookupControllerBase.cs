using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Dtos.Lookups;
using Byd.VehicleManagement.Localization;
using Byd.VehicleManagement.Permissions;
using Byd.VehicleManagement.Services.Lookups;
using Byd.LogManagement.ActivityLogs;
using Byd.LogManagement.Attributes;
using SystemStandards.Results;

namespace Byd.VehicleManagement.Controllers.Lookups;

// islevi: Arac lookup turlerinin ortak HTTP eylemlerini, yetki ve activity log metadatasini sunar.
// sistemdeki gorevi: Her lookup controller'inin yalniz rotasini bildirip CRUD uclarini bu tabandan almasini saglar.
public abstract class VehicleLookupControllerBase<TAppService> : VehicleManagementController
    where TAppService : IVehicleLookupCrudAppService
{
    private TAppService AppService => LazyGetRequiredService<TAppService>();

    /// <summary>Yeni lookup kaydi olusturur.</summary>
    /// <param name="input">Kod ve gorunen ad.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   Code (int)    → Turde tekil kod; sifir veya daha buyuk
    ///   Name (string) → Gorunen ad; zorunlu, bas ve sondaki bosluklar kirpilir
    /// }
    /// Response { Id (int) → Yeni kaydin kimligi; Code (int) → Kayitli kod; Name (string) → Kayitli ad }
    /// Kayit host kapsamindadir; tum tenant ve organizasyonlar ayni listeyi gorur. Kod turde baska bir kayitta
    /// varsa hata doner. Bu endpoint diger lookup turlerine dokunmaz.
    /// </remarks>
    [HttpPost]
    [Authorize(VehicleManagementPermissions.Lookups.Manage)]
    public virtual async Task<Result<VehicleLookupDto>> CreateAsync(
        [FromBody] CreateVehicleLookupDto input, CancellationToken cancellationToken = default)
        => await AppService.CreateAsync(input, cancellationToken);

    /// <summary>Kimligi verilen lookup kaydini getirir.</summary>
    /// <param name="id">Lookup kimligi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response { Id (int) → Kayit kimligi; Code (int) → Lookup kodu; Name (string) → Gorunen ad }
    /// Kayit yoksa bulunamadi hatasi doner. Bu endpoint kayit yazmaz.
    /// </remarks>
    [HttpGet(VehicleLookupRoutes.ById)]
    [Authorize(VehicleManagementPermissions.Lookups.View)]
    public virtual async Task<Result<VehicleLookupDto>> GetAsync(int id, CancellationToken cancellationToken = default)
        => await AppService.GetAsync(id, cancellationToken);

    /// <summary>Lookup turunun tum kayitlarini getirir.</summary>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response {
    ///   [] (VehicleLookupDto) → Turun kayitlari, koda gore sirali
    ///     Id   (int)    → Kayit kimligi
    ///     Code (int)    → Lookup kodu
    ///     Name (string) → Gorunen ad
    /// }
    /// Liste sayfasizdir; kayit yoksa bos liste doner. Bu endpoint kayit yazmaz.
    /// </remarks>
    [HttpGet]
    [Authorize(VehicleManagementPermissions.Lookups.View)]
    public virtual async Task<Result<List<VehicleLookupDto>>> GetListAsync(CancellationToken cancellationToken = default)
        => await AppService.GetListAsync(cancellationToken);

    /// <summary>Kimligi verilen lookup kaydini gunceller.</summary>
    /// <param name="id">Lookup kimligi.</param>
    /// <param name="input">Yeni kod ve gorunen ad.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Request {
    ///   Code (int)    → Yeni kod; turde tekil, sifir veya daha buyuk
    ///   Name (string) → Yeni gorunen ad; zorunlu
    /// }
    /// Response { Id (int) → Kayit kimligi; Code (int) → Guncel kod; Name (string) → Guncel ad }
    /// Kimlik degismez. Kod turde baska bir kayitta varsa hata doner. Degisiklik activity log'a yazilir.
    /// </remarks>
    [HttpPut(VehicleLookupRoutes.ById)]
    [Authorize(VehicleManagementPermissions.Lookups.Manage)]
    [TrackActivity<VehicleManagementResource>(
        VehicleManagementLocalizationKeys.ActivityLogs.LookupUpdated,
        ActivityKind.Updated, nameof(VehicleLookupDto.Id))]
    public virtual async Task<Result<VehicleLookupDto>> UpdateAsync(
        int id, [FromBody] UpdateVehicleLookupDto input, CancellationToken cancellationToken = default)
        => await AppService.UpdateAsync(id, input, cancellationToken);

    /// <summary>Kimligi verilen lookup kaydini siler.</summary>
    /// <param name="id">Lookup kimligi.</param>
    /// <param name="cancellationToken">Istek iptal belirteci.</param>
    /// <remarks>
    /// Response govdesi yoktur; kayit kalici olarak silinir ve silme activity log'a yazilir. Silinen satir
    /// varsayilan satirlardan biriyse ve kodu bos kaldiysa uygulama bir sonraki acilista onu yeniden ekler.
    /// Bu endpoint diger lookup turlerine dokunmaz.
    /// </remarks>
    [HttpDelete(VehicleLookupRoutes.ById)]
    [Authorize(VehicleManagementPermissions.Lookups.Manage)]
    [TrackActivity<VehicleManagementResource>(
        VehicleManagementLocalizationKeys.ActivityLogs.LookupDeleted,
        ActivityKind.Deleted, nameof(VehicleLookupDto.Id))]
    public virtual async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await AppService.DeleteAsync(id, cancellationToken);
        return Result.NoContent();
    }
}
