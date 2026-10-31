using Byd.VehicleManagement.Etos.Vehicles;
using Byd.VehicleManagement.Models.Vehicles;

namespace Byd.VehicleManagement.Extensions.Vehicles;

// islevi: Realtime kayit istegini arac modulunun realtime kayit modeline cevirir.
// sistemdeki gorevi: Gelen olay sozlesmesini Manager girdisine Domain sinirinda baglar.
public static class VehicleRegistrationRequestedEtoExtensions
{
    public static VehicleRealtimeRegistrationModel ToRegistrationModel(this VehicleRegistrationRequestedEto source) => new()
    {
        RouteType = source.RouteType,
        WheelchairAccessible = source.WheelchairAccessible,
        GtfsVehicleId = source.GtfsVehicleId,
        Label = source.Label,
        RegistrationNumber = source.RegistrationNumber,
        GtfsAgencyId = source.GtfsAgencyId
    };
}
