namespace Byd.VehicleManagement.Consts;

// islevi: Arac modulunun dagitik olay tel adlarini tanimlar.
// sistemdeki gorevi: Yayinlayan ve dinleyen modullerin ayni olay sozlesmesini adla eslemesini saglar.
public static class VehicleEventNames
{
    // ABP arac entity olaylarinin koku; olay adlari .Created, .Updated ve .Deleted ekiyle olusur.
    public const string Vehicle = "VehicleManagement.Vehicle";
    public const string RegistrationRequested = "VehicleManagement.Vehicles.RegistrationRequested";
}
