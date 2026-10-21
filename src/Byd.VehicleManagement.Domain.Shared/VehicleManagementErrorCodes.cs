namespace Byd.VehicleManagement;

// islevi: Modul hata kodlarinin kok ad alanini ve aile gruplarini tanimlar.
// sistemdeki gorevi: ABP hata mesajini Code.Split(':')[0] ile aradigi icin tek duz kok ad alanini garanti eder; aile bilgisi iki haneli grupta tasinir.
public static class VehicleManagementErrorCodes
{
    public const string RootNamespace = "VehicleManagement";
    public const string LookupGroup = RootNamespace + ":01";
    public const string VehicleModelGroup = RootNamespace + ":02";
    public const string VehicleTypeGroup = RootNamespace + ":03";
}
