using System.Collections.Generic;

namespace Byd.VehicleManagement.Consts;

// islevi: FuelType tablosunun kalici kimliklerini ve kodlarini tanimlar.
// sistemdeki gorevi: Seed akisi ve EF eslemesinin ayni varsayilan satirlari okumasini saglar.
public static class FuelTypeConsts
{
    public const string TableName = "FuelTypes";
    public const string CodeIndex = "IX_FuelTypes_Code";

    public static readonly IReadOnlyList<(int Id, string Name, int Code)> Definitions =
    [
        (1, "Dizel", 1),
        (2, "Benzin", 2),
        (3, "CNG", 3),
        (4, "Elektrik (batarya)", 4),
        (5, "Elektrik (havai hat)", 5),
        (6, "Hibrit", 6),
        (7, "Hidrojen", 7)
    ];
}
