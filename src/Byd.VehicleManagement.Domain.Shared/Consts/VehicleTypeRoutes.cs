namespace Byd.VehicleManagement.Consts;

// islevi: VehicleType HTTP yollarini tek noktada tanimlar.
// sistemdeki gorevi: Controller rota sozlesmesini sabit tutar.
public static class VehicleTypeRoutes
{
    public const string Root = "api/vehicle-management/vehicle-types";
    public const string ById = "{id:guid}";
    public const string FilteredList = "filtered-list";
}
