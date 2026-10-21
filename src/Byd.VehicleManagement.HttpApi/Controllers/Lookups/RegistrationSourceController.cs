using Microsoft.AspNetCore.Mvc;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Services.Lookups;

namespace Byd.VehicleManagement.Controllers.Lookups;

// mirastan gelen: Create, Get, GetList, Update ve Delete uclari ile yetki ve activity log metadatasi.
// burada yazilan: kayit kaynagi rotasi ve AppService baglantisi.
[ApiController]
[Route(VehicleLookupRoutes.RegistrationSourcesPath)]
public class RegistrationSourceController : VehicleLookupControllerBase<IRegistrationSourceAppService>
{
}
