using System.Collections.Generic;

namespace Byd.VehicleManagement.Consts;

// islevi: TransportMode tablosunun kalici kimliklerini ve kodlarini tanimlar.
// sistemdeki gorevi: Seed akisi ve EF eslemesinin ayni varsayilan satirlari okumasini saglar.
public static class TransportModeConsts
{
    public const string TableName = "TransportModes";
    public const string CodeIndex = "IX_TransportModes_Code";

    public static readonly IReadOnlyList<(int Id, string Name, int Code)> Definitions =
    [
        (1, "Tramvay", 0),
        (2, "Metro", 1),
        (3, "Tren", 2),
        (4, "Otobüs", 3),
        (5, "Feribot", 4),
        (6, "Kablolu tramvay", 5),
        (7, "Teleferik", 6),
        (8, "Füniküler", 7),
        (9, "Troleybüs", 11),
        (10, "Monoray", 12)
    ];
}
