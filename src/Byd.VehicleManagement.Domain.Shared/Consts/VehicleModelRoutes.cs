namespace Byd.VehicleManagement.Consts;

// islevi: VehicleModel HTTP yollarini tek noktada tanimlar.
// sistemdeki gorevi: Controller rota sozlesmesini sabit tutar.
public static class VehicleModelRoutes
{
    public const string Root = "api/vehicle-management/vehicle-models";
    public const string ById = "{id:guid}";
    public const string FilteredList = "filtered-list";
}
