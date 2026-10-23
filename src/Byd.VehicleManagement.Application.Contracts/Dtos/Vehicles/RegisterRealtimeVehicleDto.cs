namespace Byd.VehicleManagement.Dtos.Vehicles;

// islevi: Realtime akisinda gorulen aracin GTFS-RT VehicleDescriptor alanlarini, GTFS route_type degerini ve
// isletmecisini standardin kendi degerleriyle tasir; route_type gelmeyebilir.
// sistemdeki gorevi: Realtime modulunun bul-ya da-ekle cagrisinin girdisidir; modulun ic kimliklerini bilmeden her feed
// ayni sozlesmeyle cagirir. Tip, durum ve hizmet donemi alinmaz.
public class RegisterRealtimeVehicleDto
{
    // Table Columns / Data
    public int? RouteType { get; set; }
    public int? WheelchairAccessible { get; set; }
    public string GtfsVehicleId { get; set; } = default!;
    public string? Label { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? GtfsAgencyId { get; set; }
}
