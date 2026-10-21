namespace Byd.VehicleManagement.Consts;

// islevi: VehicleModel filtre yuzeyinde disariya acilan alan adlarini tanimlar.
// sistemdeki gorevi: Filtre DTO'su ile repository sorgu motoru arasindaki kararli alan adi sozlesmesidir.
public static class VehicleModelFilterFields
{
    public const string Manufacturer = "manufacturer";
    public const string Name = "name";
    public const string CreationTime = "creationTime";

    public static class PropertyPaths
    {
        public const string Manufacturer = "Manufacturer";
        public const string Name = "Name";
        public const string CreationTime = "CreationTime";
    }
}
