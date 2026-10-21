namespace Byd.VehicleManagement.Services.Lookups;

// islevi: TransportMode satirlarinin CRUD API sozlesmesini ortak lookup sozlesmesinden devralir.
// sistemdeki gorevi: HTTP ve modul tuketicilerini implementation'dan ayirir.
public interface ITransportModeAppService : IVehicleLookupCrudAppService
{
}
