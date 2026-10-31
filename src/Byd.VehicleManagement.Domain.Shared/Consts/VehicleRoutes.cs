namespace Byd.VehicleManagement.Consts;

// islevi: Vehicle HTTP yollarini tek noktada tanimlar.
// sistemdeki gorevi: Controller rota sozlesmesini sabit tutar.
public static class VehicleRoutes
{
    public const string Root = "api/vehicle-management/vehicles";
    public const string ById = "{id:guid}";
    public const string FilteredList = "filtered-list";
}
