namespace Byd.VehicleManagement.Models.Vehicles;

// islevi: Realtime akisinda gorulen aracin GTFS-RT VehicleDescriptor bilgilerini ve feed'in kipini tasir.
// sistemdeki gorevi: Realtime kaynakli otomatik kaydin domain girdisidir; tip, durum ve hizmet donemi istekten alinmaz.
public class VehicleRealtimeRegistrationModel
{
    public int? RouteType { get; set; }
    public int? WheelchairAccessible { get; set; }
    public string GtfsVehicleId { get; set; } = default!;
    public string? Label { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? GtfsAgencyId { get; set; }
}
