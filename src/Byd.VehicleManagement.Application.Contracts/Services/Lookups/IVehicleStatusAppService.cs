namespace Byd.VehicleManagement.Services.Lookups;

// islevi: VehicleStatus satirlarinin CRUD API sozlesmesini ortak lookup sozlesmesinden devralir.
// sistemdeki gorevi: HTTP ve modul tuketicilerini implementation'dan ayirir.
public interface IVehicleStatusAppService : IVehicleLookupCrudAppService
{
}
