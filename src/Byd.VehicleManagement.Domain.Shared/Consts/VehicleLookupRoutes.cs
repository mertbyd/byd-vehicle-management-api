namespace Byd.VehicleManagement.Consts;

// islevi: Arac lookup HTTP yollarini tek noktada tanimlar.
// sistemdeki gorevi: Her lookup controller'inin rota sozlesmesini sabit tutar.
public static class VehicleLookupRoutes
{
    public const string Root = "api/vehicle-management/lookups";
    public const string TransportModes = "transport-modes";
    public const string FuelTypes = "fuel-types";
    public const string VehicleStatuses = "vehicle-statuses";
    public const string WheelchairAccessibles = "wheelchair-accessibles";
    public const string EquipmentTypes = "equipment-types";
    public const string ById = "{id:int}";
    public const string TransportModesPath = Root + "/" + TransportModes;
    public const string FuelTypesPath = Root + "/" + FuelTypes;
    public const string VehicleStatusesPath = Root + "/" + VehicleStatuses;
    public const string WheelchairAccessiblesPath = Root + "/" + WheelchairAccessibles;
    public const string EquipmentTypesPath = Root + "/" + EquipmentTypes;
}
