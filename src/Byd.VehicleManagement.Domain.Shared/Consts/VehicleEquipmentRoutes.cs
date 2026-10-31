namespace Byd.VehicleManagement.Consts;

// islevi: VehicleEquipment HTTP yollarini tek noktada tanimlar.
// sistemdeki gorevi: Controller rota sozlesmesini sabit tutar.
public static class VehicleEquipmentRoutes
{
    public const string Root = "api/vehicle-management/vehicle-equipments";
    public const string ById = "{id:guid}";
    public const string FilteredList = "filtered-list";
}
