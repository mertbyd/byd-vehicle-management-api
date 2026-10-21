using Microsoft.AspNetCore.Mvc;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Services.Lookups;

namespace Byd.VehicleManagement.Controllers.Lookups;

// mirastan gelen: Create, Get, GetList, Update ve Delete uclari ile yetki ve activity log metadatasi.
// burada yazilan: ulasim modu rotasi ve AppService baglantisi.
[ApiController]
[Route(VehicleLookupRoutes.TransportModesPath)]
public class TransportModeController : VehicleLookupControllerBase<ITransportModeAppService>
{
}
